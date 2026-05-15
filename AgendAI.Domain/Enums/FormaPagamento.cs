using System.Text.Json.Serialization;
using AgendAI.Domain.Serialization;

namespace AgendAI.Domain.Enums;

/// <summary>
/// Espelha <c>FormaPagamento</c> do frontend (<c>atendimento.service.ts</c>).
/// </summary>
[JsonConverter(typeof(SnakeCaseEnumJsonConverter<FormaPagamento>))]
public enum FormaPagamento
{
    Dinheiro,
    Pix,
    CartaoDebito,
    CartaoCredito,
    CartaoCreditoParcelado,
    Convenio
}
