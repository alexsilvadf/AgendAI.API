namespace AgendAI.Infra.Persistence.Seed;

/// <summary>IDs fixos para seed e testes (paridade com mocks do frontend).</summary>
public static class SeedIds
{
    public static readonly Guid UsuarioAdmin = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1");
    public static readonly Guid UsuarioAnaMartins = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2");
    public static readonly Guid UsuarioBrunoCosta = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3");
    public static readonly Guid UsuarioCarlaRecepcao = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4");
    public static readonly Guid UsuarioCarlaDias = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5");

    public static readonly Guid PacienteMaria = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1");
    public static readonly Guid PacienteJoao = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2");
    public static readonly Guid PacienteAnaLuiza = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3");

    public static readonly Guid ProcedimentoConsultaGeral = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1");
    public static readonly Guid ProcedimentoRetorno = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2");
    public static readonly Guid ProcedimentoCardio = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3");
    public static readonly Guid ProcedimentoEletro = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc4");
    public static readonly Guid ProcedimentoDermato = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc5");
    public static readonly Guid ProcedimentoCirurgiaDermato = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc6");

    public static readonly Guid AtendimentoPendenteAna = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddd01");
}
