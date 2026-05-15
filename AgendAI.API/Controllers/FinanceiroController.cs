using AgendAI.API.Extensions;
using AgendAI.Application.Abstractions;
using AgendAI.Application.DTOs.Financeiro;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgendAI.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/financeiro")]
public sealed class FinanceiroController(IFinanceiroService financeiroService) : ControllerBase
{
    [HttpGet("lancamentos")]
    public async Task<IActionResult> ListarLancamentos(
        [FromQuery] string? dataInicio,
        [FromQuery] string? dataFim,
        [FromQuery] string? tipo,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("financeiro:view"))
            return Forbid();

        DateOnly? inicio = TryParseDate(dataInicio);
        DateOnly? fim = TryParseDate(dataFim);

        if (dataInicio is not null && !inicio.HasValue)
            return BadRequest(new { detail = "Parâmetro dataInicio inválido." });

        if (dataFim is not null && !fim.HasValue)
            return BadRequest(new { detail = "Parâmetro dataFim inválido." });

        var itens = await financeiroService.ListarAsync(inicio, fim, tipo, status, cancellationToken);
        return Ok(itens);
    }

    [HttpPost("lancamentos")]
    public async Task<IActionResult> CriarLancamento(
        [FromBody] CriarLancamentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("financeiro:edit"))
            return Forbid();

        var criado = await financeiroService.CriarAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ListarLancamentos), new { dataInicio = criado.Data }, criado);
    }

    [HttpPatch("lancamentos/{id:guid}/status")]
    public async Task<IActionResult> AtualizarStatus(
        Guid id,
        [FromBody] AtualizarStatusLancamentoRequest request,
        CancellationToken cancellationToken)
    {
        if (!User.HasPermission("financeiro:edit"))
            return Forbid();

        var atualizado = await financeiroService.AtualizarStatusAsync(id, request, cancellationToken);
        return Ok(atualizado);
    }

    private static DateOnly? TryParseDate(string? value) =>
        string.IsNullOrWhiteSpace(value)
            ? null
            : DateOnly.TryParse(value, out var parsed) ? parsed : null;
}
