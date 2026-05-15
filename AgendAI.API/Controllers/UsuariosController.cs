using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/usuarios")]
public sealed class UsuariosController(IUsuarioService usuarioService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        if (!User.HasPermission("usuarios:view"))
            return Forbid();

        return Ok(await usuarioService.ListarAsync(cancellationToken));
    }
}
