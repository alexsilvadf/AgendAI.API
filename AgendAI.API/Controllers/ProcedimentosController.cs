using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/procedimentos")]
public sealed class ProcedimentosController(IProcedimentoService procedimentoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        if (!User.HasPermission("procedimentos:view"))
            return Forbid();

        return Ok(await procedimentoService.ListarAsync(cancellationToken));
    }

    [HttpGet("ativos")]
    public async Task<IActionResult> ListarAtivos(CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agendamento:create") && !User.HasPermission("procedimentos:view"))
            return Forbid();

        return Ok(await procedimentoService.ListarAtivosAsync(cancellationToken));
    }
}
