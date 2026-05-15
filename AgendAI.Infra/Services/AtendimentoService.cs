using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Atendimentos;
using AgendAI.Domain.Entities;
using AgendAI.Domain.Enums;
using AgendAI.Domain.Exceptions;
using AgendAI.Infra.Mapping;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class AtendimentoService(AgendAiDbContext db) : IAtendimentoService
{
    public async Task<IReadOnlyList<AtendimentoDto>> ListarAsync(
        DateOnly? data,
        Guid? profissionalId,
        bool? pago,
        CancellationToken cancellationToken = default)
    {
        var query = db.Atendimentos
            .AsNoTracking()
            .Include(a => a.Profissional)
            .Include(a => a.Paciente)
            .Include(a => a.Procedimento)
            .AsQueryable();

        if (data.HasValue)
            query = query.Where(a => a.Data == data.Value);

        if (profissionalId.HasValue)
            query = query.Where(a => a.ProfissionalId == profissionalId.Value);

        if (pago.HasValue)
            query = query.Where(a => a.Pago == pago.Value);

        var itens = await query
            .OrderBy(a => a.Data)
            .ThenBy(a => a.Hora)
            .ToListAsync(cancellationToken);

        return itens.Select(EntityMapper.ToDto).ToList();
    }

    public async Task<AtendimentoDto> CriarAsync(
        CriarAtendimentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var data = DateOnly.Parse(request.Data);
        var hora = TimeOnly.Parse(request.Hora);

        var profissional = await db.Usuarios
            .FirstOrDefaultAsync(u => u.Id == request.ProfissionalId && u.Ativo && u.Role == UserRole.Dentista, cancellationToken)
            ?? throw new NotFoundException("Profissional", request.ProfissionalId);

        var paciente = await db.Pacientes
            .FirstOrDefaultAsync(p => p.Id == request.PacienteId && p.Ativo, cancellationToken)
            ?? throw new NotFoundException("Paciente", request.PacienteId);

        var procedimento = await db.Procedimentos
            .FirstOrDefaultAsync(p => p.Id == request.ProcedimentoId, cancellationToken)
            ?? throw new NotFoundException("Procedimento", request.ProcedimentoId);

        var duplicado = await db.Atendimentos.AnyAsync(a =>
            a.ProfissionalId == request.ProfissionalId &&
            a.Data == data &&
            a.Hora == hora, cancellationToken);

        if (duplicado)
            throw new ConflictException("Já existe atendimento registrado para este profissional, data e horário.");

        Agendamento? agendamento = null;
        if (request.AgendamentoId.HasValue)
        {
            agendamento = await db.Agendamentos
                .FirstOrDefaultAsync(a => a.Id == request.AgendamentoId.Value, cancellationToken)
                ?? throw new NotFoundException("Agendamento", request.AgendamentoId.Value);
        }
        else
        {
            agendamento = await db.Agendamentos.FirstOrDefaultAsync(a =>
                a.ProfissionalId == request.ProfissionalId &&
                a.PacienteId == request.PacienteId &&
                a.Data == data &&
                a.HoraInicio == hora &&
                a.Status == StatusAgendamento.Agendado, cancellationToken);
        }

        var now = DateTime.UtcNow;
        var atendimento = new Atendimento
        {
            Id = Guid.NewGuid(),
            AgendamentoId = agendamento?.Id,
            ProfissionalId = profissional.Id,
            PacienteId = paciente.Id,
            ProcedimentoId = procedimento.Id,
            Data = data,
            Hora = hora,
            Valor = request.Valor,
            Observacoes = request.Observacoes,
            Dentes = request.Dentes,
            Retorno = request.Retorno,
            Pago = false,
            CriadoEm = now
        };

        if (agendamento is not null)
        {
            agendamento.Status = StatusAgendamento.Atendido;
            agendamento.AtualizadoEm = now;
        }

        db.PacienteHistoricos.Add(new PacienteHistorico
        {
            Id = Guid.NewGuid(),
            PacienteId = paciente.Id,
            Data = data,
            Procedimento = procedimento.Nome,
            Profissional = profissional.Nome,
            Observacoes = request.Observacoes,
            Valor = request.Valor
        });

        paciente.AtualizadoEm = now;

        db.Atendimentos.Add(atendimento);
        await db.SaveChangesAsync(cancellationToken);

        await db.Entry(atendimento).Reference(a => a.Profissional).LoadAsync(cancellationToken);
        await db.Entry(atendimento).Reference(a => a.Paciente).LoadAsync(cancellationToken);
        await db.Entry(atendimento).Reference(a => a.Procedimento).LoadAsync(cancellationToken);

        return EntityMapper.ToDto(atendimento);
    }

    public async Task<AtendimentoDto> RegistrarPagamentoAsync(
        Guid id,
        RegistrarPagamentoRequest request,
        CancellationToken cancellationToken = default)
    {
        var atendimento = await db.Atendimentos
            .Include(a => a.Profissional)
            .Include(a => a.Paciente)
            .Include(a => a.Procedimento)
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken)
            ?? throw new NotFoundException("Atendimento", id);

        if (atendimento.Pago)
            throw new ConflictException("Pagamento já registrado para este atendimento.");

        var forma = EnumExtensions.FromJsonValue<FormaPagamento>(request.FormaPagamento);

        atendimento.Pago = true;
        atendimento.FormaPagamento = forma;
        atendimento.Parcelas = forma == FormaPagamento.CartaoCreditoParcelado
            ? request.Parcelas ?? 1
            : null;

        var jaTemLancamento = await db.Lancamentos.AnyAsync(
            l => l.AtendimentoId == atendimento.Id, cancellationToken);

        if (!jaTemLancamento)
        {
            var hoje = DateOnly.FromDateTime(DateTime.Today);
            db.Lancamentos.Add(new Lancamento
            {
                Id = Guid.NewGuid(),
                Tipo = TipoLancamento.Receita,
                Descricao = $"{atendimento.Procedimento.Nome} — {atendimento.Paciente.Nome}",
                Valor = atendimento.Valor,
                Data = hoje,
                Vencimento = hoje,
                Status = StatusLancamento.Pago,
                Categoria = CategoriaLancamento.Atendimento,
                FormaPagamento = forma,
                AtendimentoId = atendimento.Id,
                Paciente = atendimento.Paciente.Nome,
                Profissional = atendimento.Profissional.Nome,
                Procedimento = atendimento.Procedimento.Nome,
                CriadoEm = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync(cancellationToken);
        return EntityMapper.ToDto(atendimento);
    }
}
