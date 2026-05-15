using AgendAI.Infra.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendAI.API.Controllers;

[ApiController]
[Route("api/v1/health")]
public sealed class HealthController(AgendAiDbContext db) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        try
        {
            var canConnect = await db.Database.CanConnectAsync(cancellationToken);
            if (!canConnect)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    status = "unhealthy",
                    service = "AgendAI API v1",
                    database = "unreachable"
                });
            }

            return Ok(new
            {
                status = "healthy",
                service = "AgendAI API v1",
                database = db.Database.IsInMemory() ? "in-memory" : "connected"
            });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "unhealthy",
                service = "AgendAI API v1",
                database = "error"
            });
        }
    }
}
