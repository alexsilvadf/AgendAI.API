# Persistência de enums — AgendAI

## Decisão

| Aspecto        | Escolha                          |
|----------------|----------------------------------|
| Tipo no banco  | **STRING** (`nvarchar`)          |
| Formato        | **snake_case** minúsculo         |
| Alternativa    | ~~INT~~ (não adotado)            |

## Motivação

1. **Contrato com o Angular** — o frontend usa union types string (`'administrador' \| 'dentista'`, `'livre' \| 'ocupado'`, etc.). A API e o banco usam os mesmos literais, sem tabela de mapeamento int ↔ string.
2. **Legibilidade** — consultas, seeds e suporte: `WHERE Status = 'agendado'` em vez de `WHERE Status = 1`.
3. **Índices filtrados** — ex.: `IX_Agendamentos_*` com filtro `[Status] = N'agendado'`.
4. **Evolução segura** — incluir ou reordenar valores no enum C# não altera registros já gravados (ao contrário de int fixo por ordem de declaração).
5. **Alinhamento JSON ↔ EF** — mesma regra de serialização no Domain (`SnakeCaseEnumJsonConverter`, `EnumJsonValueAttribute`) e no EF (`EnumSnakeCaseConverter`).

## Quando não usar snake_case automático

Atributo `[EnumJsonValue("...")]` no `AgendAI.Domain` para literais que não seguem snake_case:

| Enum           | Exemplo no banco / JSON   |
|----------------|---------------------------|
| `Permission`   | `agenda:view`             |
| `TipoSanguineo`| `A+`, `O-`, `nao_informado` |

## Implementação

```
Domain (enum + EnumJsonValue)
    ↓ SnakeCaseEnumJsonConverter / EnumJsonValue
API JSON
    ↓ EnumSnakeCaseConverter (EF ValueConverter)
SQL Server (nvarchar)
```

- Configuração: `AgendAI.Infra/Persistence/Configurations/*Configuration.cs` → `.HasConversion(EnumSnakeCaseConverter.Create<TEnum>())`
- Tamanho típico: `.HasMaxLength(50)` (constante `EnumPersistenceStrategy.DefaultMaxLength`)

## Exemplos C# → banco

| Enum              | Membro C#              | Valor persistido              |
|-------------------|------------------------|-------------------------------|
| `UserRole`        | `Administrador`        | `administrador`               |
| `StatusAgendamento` | `Agendado`           | `agendado`                    |
| `FormaPagamento`  | `CartaoCreditoParcelado` | `cartao_credito_parcelado` |
| `Permission`      | `AgendaView`           | `agenda:view`                 |
| `TipoSanguineo`   | `APositivo`            | `A+`                          |

## Se no futuro usar INT

Seria necessário:

- Alterar todas as colunas enum para `int` / `tinyint`
- Atualizar filtros de índice (`agendado` → `0`)
- Desalinhar ou mapear DTOs da API (frontend continuaria com strings)
- Migration de dados com script de conversão

**Recomendação:** manter string enquanto o contrato da API for JSON com literais do frontend.
