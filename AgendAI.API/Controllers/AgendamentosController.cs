using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Agendamentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/agendamentos")]
public sealed class AgendamentosController(IAgendamentoService agendamentoService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarAgendamentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agendamento:create"))
            return Forbid();

        var criado = await agendamentoService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Criar), new { id = criado.Id }, criado);
    }

    [HttpPost("{id:guid}/cancelar")]
    public async Task<IActionResult> Cancelar(Guid id, CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agendamento:cancel"))
            return Forbid();

        var cancelado = await agendamentoService.CancelarAsync(id, cancellationToken);
        return Ok(cancelado);
    }

    [HttpPost("{id:guid}/remarcar")]
    public async Task<IActionResult> Remarcar(
        Guid id,
        [FromBody] RemarcarAgendamentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agenda:edit"))
            return Forbid();

        var remarcado = await agendamentoService.RemarcarAsync(id, request, cancellationToken);
        return Ok(remarcado);
    }
}
