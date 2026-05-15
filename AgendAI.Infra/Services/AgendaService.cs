using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Agenda;
using AgendAI.Domain.Enums;
using AgendAI.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.Infra.Services;

public sealed class AgendaService(AgendAiDbContext db) : IAgendaService
{
    public async Task<IReadOnlyList<ProfessionalDto>> ListarProfissionaisAsync(
        CancellationToken cancellationToken = default)
    {
        return await db.Usuarios
            .AsNoTracking()
            .Where(u => u.Role == UserRole.Dentista && u.Ativo)
            .OrderBy(u => u.Nome)
            .Select(u => new ProfessionalDto
            {
                Id = u.Id,
                Name = u.Nome,
                Specialty = u.Especialidade ?? string.Empty
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<DayScheduleDto>> MontarGradeAsync(
        DateOnly data,
        Guid? profissionalId,
        UserRole role,
        Guid? usuarioLogadoId,
        CancellationToken cancellationToken = default)
    {
        if (role == UserRole.Dentista && usuarioLogadoId.HasValue)
            profissionalId = usuarioLogadoId;

        var config = await db.ConfiguracoesClinica.AsNoTracking().FirstAsync(cancellationToken);
        var profissionais = await db.Usuarios
            .AsNoTracking()
            .Where(u => u.Role == UserRole.Dentista && u.Ativo)
            .Where(u => !profissionalId.HasValue || u.Id == profissionalId)
            .ToListAsync(cancellationToken);

        var agendamentos = await db.Agendamentos
            .AsNoTracking()
            .Include(a => a.Paciente)
            .Include(a => a.Procedimento)
            .Where(a => a.Data == data && a.Status == StatusAgendamento.Agendado)
            .Where(a => !profissionalId.HasValue || a.ProfissionalId == profissionalId)
            .ToListAsync(cancellationToken);

        var bloqueios = await db.BloqueiosAgenda
            .AsNoTracking()
            .Where(b => b.Data == data)
            .Where(b => !profissionalId.HasValue || b.ProfissionalId == profissionalId)
            .ToListAsync(cancellationToken);

        var atendimentosPendentes = await db.Atendimentos
            .AsNoTracking()
            .Where(a => a.Data == data && !a.Pago)
            .Where(a => !profissionalId.HasValue || a.ProfissionalId == profissionalId)
            .ToListAsync(cancellationToken);

        var slotsBase = GerarSlots(config.HoraAbertura, config.HoraFechamento, config.IntervaloMinutos);
        var resultado = new List<DayScheduleDto>();

        foreach (var prof in profissionais)
        {
            var slots = slotsBase.Select(s => new AgendaSlotDto
            {
                Start = s.Start,
                End = s.End,
                Status = SlotStatus.Livre.ToJsonValue()
            }).ToList();

            foreach (var bloqueio in bloqueios.Where(b => b.ProfissionalId == prof.Id))
            {
                AplicarIntervalo(slots, bloqueio.HoraInicio, bloqueio.HoraFim, SlotStatus.Indisponivel.ToJsonValue(), bloqueio.Motivo);
            }

            foreach (var ag in agendamentos.Where(a => a.ProfissionalId == prof.Id))
            {
                AplicarIntervalo(slots, ag.HoraInicio, ag.HoraFim, SlotStatus.Ocupado.ToJsonValue(), ag.Procedimento.Nome, ag.Paciente.Nome, ag.Id);
            }

            foreach (var at in atendimentosPendentes.Where(a => a.ProfissionalId == prof.Id))
            {
                var slot = slots.FirstOrDefault(s => s.Start == at.Hora.ToString("HH:mm"));
                if (slot is not null)
                {
                    slot.PendentePagamento = true;
                    slot.AtendimentoId = at.Id;
                }
            }

            resultado.Add(new DayScheduleDto
            {
                Date = data.ToString("yyyy-MM-dd"),
                ProfessionalId = prof.Id,
                Slots = slots
            });
        }

        return resultado;
    }

    private static List<(string Start, string End)> GerarSlots(TimeOnly abertura, TimeOnly fechamento, int intervaloMinutos)
    {
        var slots = new List<(string Start, string End)>();
        var atual = abertura;

        while (atual.AddMinutes(intervaloMinutos) <= fechamento)
        {
            var fim = atual.AddMinutes(intervaloMinutos);
            slots.Add((atual.ToString("HH:mm"), fim.ToString("HH:mm")));
            atual = fim;
        }

        return slots;
    }

    private static void AplicarIntervalo(
        List<AgendaSlotDto> slots,
        TimeOnly inicio,
        TimeOnly fim,
        string status,
        string? detail = null,
        string? patientName = null,
        Guid? agendamentoId = null)
    {
        foreach (var slot in slots)
        {
            var slotInicio = TimeOnly.Parse(slot.Start);
            if (slotInicio >= inicio && slotInicio < fim)
            {
                slot.Status = status;
                slot.Detail = detail;
                slot.PatientName = patientName;
                slot.AgendamentoId = agendamentoId;
            }
        }
    }
}
