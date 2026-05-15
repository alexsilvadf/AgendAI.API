using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Agendamentos;
using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Domain.Exceptions;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class AgendamentoService(AgendAiDbContext db) : IAgendamentoService
{
    public async Task<AgendamentoDto> CriarAsync(
        CriarAgendamentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = DateOnly.Parse(request.Data);
        var horaInicio = TimeOnly.Parse(request.Hora);
        var horaFim = horaInicio.AddMinutes(30);

        await ValidarConflitosAsync(request.ProfissionalId, request.PacienteId, data, horaInicio, null, cancellationToken);
        await ValidarSlotLivreAsync(request.ProfissionalId, data, horaInicio, horaFim, cancellationToken);

        var procedimento = await db.Procedimentos
            .FirstOrDefaultAsync(p => p.Id == request.ProcedimentoId && p.Status == StatusProcedimento.Ativo, cancellationToken)
            ?? throw new NotFoundException("Procedimento", request.ProcedimentoId);

        var paciente = await db.Pacientes
            .FirstOrDefaultAsync(p => p.Id == request.PacienteId && p.Ativo, cancellationToken)
            ?? throw new NotFoundException("Paciente", request.PacienteId);

        var profissional = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.ProfissionalId && u.Ativo && u.Role == UserRole.Dentista, cancellationToken)
            ?? throw new NotFoundException("Profissional", request.ProfissionalId);

        var now = DateTime.UtcNow;
        var agendamento = new Agendamento
        {
            Id = Guid.NewGuid(),
            ProfissionalId = profissional.Id,
            PacienteId = paciente.Id,
            ProcedimentoId = procedimento.Id,
            Data = data,
            HoraInicio = horaInicio,
            HoraFim = horaFim,
            Valor = request.Valor ?? procedimento.Valor,
            Status = StatusAgendamento.Agendado,
            Observacoes = request.Observacoes,
            CriadoEm = now,
            AtualizadoEm = now
        };

        db.Agendamentos.Add(agendamento);
        await db.SaveChangesAsync(cancellationToken);

        return EntityMapper.ToDto(agendamento);
    }

    public async Task<AgendamentoDto> CancelarAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var agendamento = await db.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException("Agendamento", id);

        if (agendamento.Status != StatusAgendamento.Agendado)
            throw new ConflictException("Somente agendamentos ativos podem ser cancelados.");

        agendamento.Status = StatusAgendamento.Cancelado;
        agendamento.AtualizadoEm = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);

        return EntityMapper.ToDto(agendamento);
    }

    public async Task<AgendamentoDto> RemarcarAsync(
        Guid id,
        RemarcarAgendamentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var agendamento = await db.Agendamentos
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException("Agendamento", id);

        if (agendamento.Status != StatusAgendamento.Agendado)
            throw new ConflictException("Somente agendamentos ativos podem ser remarcados.");

        var novaHora = TimeOnly.Parse(request.NovaHora);
        var novaFim = novaHora.AddMinutes(30);

        await ValidarConflitosAsync(agendamento.ProfissionalId, agendamento.PacienteId, agendamento.Data, novaHora, agendamento.Id, cancellationToken);
        await ValidarSlotLivreAsync(agendamento.ProfissionalId, agendamento.Data, novaHora, novaFim, cancellationToken);

        agendamento.HoraInicio = novaHora;
        agendamento.HoraFim = novaFim;
        agendamento.Status = StatusAgendamento.Remarcado;
        agendamento.AtualizadoEm = DateTime.UtcNow;

        var novo = new Agendamento
        {
            Id = Guid.NewGuid(),
            ProfissionalId = agendamento.ProfissionalId,
            PacienteId = agendamento.PacienteId,
            ProcedimentoId = agendamento.ProcedimentoId,
            Data = agendamento.Data,
            HoraInicio = novaHora,
            HoraFim = novaFim,
            Valor = agendamento.Valor,
            Status = StatusAgendamento.Agendado,
            Observacoes = agendamento.Observacoes,
            CriadoEm = DateTime.UtcNow,
            AtualizadoEm = DateTime.UtcNow
        };

        db.Agendamentos.Add(novo);
        await db.SaveChangesAsync(cancellationToken);

        return EntityMapper.ToDto(novo);
    }

    private async Task ValidarConflitosAsync(
        Guid profissionalId,
        Guid pacienteId,
        DateOnly data,
        TimeOnly hora,
        Guid? ignorarAgendamentoId,
        CancellationToken cancellationToken)
    {
        var conflitoProfissional = await db.Agendamentos.AnyAsync(a =>
            a.Id != ignorarAgendamentoId &&
            a.ProfissionalId == profissionalId &&
            a.Data == data &&
            a.HoraInicio == hora &&
            a.Status == StatusAgendamento.Agendado, cancellationToken);

        if (conflitoProfissional)
            throw new ConflictException("Este profissional já possui um agendamento na mesma data e horário.");

        var conflitoPaciente = await db.Agendamentos.AnyAsync(a =>
            a.Id != ignorarAgendamentoId &&
            a.PacienteId == pacienteId &&
            a.Data == data &&
            a.HoraInicio == hora &&
            a.Status == StatusAgendamento.Agendado, cancellationToken);

        if (conflitoPaciente)
            throw new ConflictException("Este paciente já possui um agendamento na mesma data e horário.");
    }

    private async Task ValidarSlotLivreAsync(
        Guid profissionalId,
        DateOnly data,
        TimeOnly horaInicio,
        TimeOnly horaFim,
        CancellationToken cancellationToken)
    {
        var bloqueado = await db.BloqueiosAgenda.AnyAsync(b =>
            b.ProfissionalId == profissionalId &&
            b.Data == data &&
            b.HoraInicio <= horaInicio &&
            b.HoraFim > horaInicio, cancellationToken);

        if (bloqueado)
            throw new ConflictException("Horário indisponível (bloqueio de agenda).");
    }
}
