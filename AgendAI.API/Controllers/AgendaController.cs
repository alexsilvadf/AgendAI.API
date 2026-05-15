using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/agenda")]
public sealed class AgendaController(IAgendaService agendaService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ObterGrade(
        [FromQuery] string data,
        [FromQuery] Guid? profissionalId,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agenda:view"))
            return Forbid();

        if (!DateOnly.TryParse(data, out var dataConsulta))
            return BadRequest(new { detail = "Parâmetro data inválido. Use yyyy-MM-dd." });

        var grade = await agendaService.MontarGradeAsync(
            dataConsulta,
            profissionalId,
            User.GetUserRole(),
            User.GetUserId(),
            cancellationToken);

        return Ok(grade);
    }
}
