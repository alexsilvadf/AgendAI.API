using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/pacientes")]
public sealed class PacientesController(IPacienteService pacienteService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? nome, CancellationToken cancellationToken)
    {
        if (!User.HasPermission("pacientes:view"))
            return Forbid();

        var itens = await pacienteService.ListarAsync(nome, cancellationToken);
        return Ok(itens);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id, CancellationToken cancellationToken)
    {
        if (!User.HasPermission("pacientes:view"))
            return Forbid();

        var paciente = await pacienteService.ObterPorIdAsync(id, cancellationToken);
        return paciente is null ? NotFound() : Ok(paciente);
    }
}
