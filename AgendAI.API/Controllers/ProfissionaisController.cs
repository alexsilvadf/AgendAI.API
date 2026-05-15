using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/profissionais")]
public sealed class ProfissionaisController(IAgendaService agendaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agenda:view") && !User.HasPermission("atendimento:create"))
            return Forbid();

        var itens = await agendaService.ListarProfissionaisAsync(cancellationToken);
        return Ok(itens);
    }
}
