using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Atendimentos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/atendimentos")]
public sealed class AtendimentosController(IAtendimentoService atendimentoService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Listar(
        [FromQuery] string? data,
        [FromQuery] Guid? profissionalId,
        [FromQuery] bool? pago,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agenda:view") && !User.HasPermission("financeiro:view"))
            return Forbid();

        DateOnly? dataFiltro = null;
        if (!string.IsNullOrWhiteSpace(data))
        {
            if (!DateOnly.TryParse(data, out var parsed))
                return BadRequest(new { detail = "Parâmetro data inválido. Use yyyy-MM-dd." });
            dataFiltro = parsed;
        }

        var itens = await atendimentoService.ListarAsync(dataFiltro, profissionalId, pago, cancellationToken);
        return Ok(itens);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(
        [FromBody] CriarAtendimentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("atendimento:create"))
            return Forbid();

        var criado = await atendimentoService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(Listar), new { data = criado.Data }, criado);
    }

    [HttpPost("{id:guid}/pagar")]
    public async Task<IActionResult> RegistrarPagamento(
        Guid id,
        [FromBody] RegistrarPagamentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("agenda:edit") && !User.HasPermission("financeiro:edit"))
            return Forbid();

        var atualizado = await atendimentoService.RegistrarPagamentoAsync(id, request, cancellationToken);
        return Ok(atualizado);
    }
}
