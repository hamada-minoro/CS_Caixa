using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CS_Caixa.Models.Mapping;

namespace CS_Caixa.Models
{
    public partial class CS_CAIXA_DBContext : DbContext
    {
        static CS_CAIXA_DBContext()
        {
            Database.SetInitializer<CS_CAIXA_DBContext>(null);
        }

        public CS_CAIXA_DBContext()
            : base("Name=CS_CAIXA_DBContext")
        {
        }

        public DbSet<Adicionar_Caixa> Adicionar_Caixa { get; set; }
        public DbSet<AgendaEletronica> AgendaEletronicas { get; set; }
        public DbSet<Atendimento> Atendimentoes { get; set; }
        public DbSet<Ato> Atoes { get; set; }
        public DbSet<AtualizaSite> AtualizaSites { get; set; }
        public DbSet<Cadastro_Painel> Cadastro_Painel { get; set; }
        public DbSet<Cadastro_Pc> Cadastro_Pc { get; set; }
        public DbSet<CadastroCliente> CadastroClientes { get; set; }
        public DbSet<CadastroCompraMaterial> CadastroCompraMaterials { get; set; }
        public DbSet<CadastroMaterial> CadastroMaterials { get; set; }
        public DbSet<CadCheque> CadCheques { get; set; }
        public DbSet<CadMensalista> CadMensalistas { get; set; }
        public DbSet<ConexaoPainelSenha> ConexaoPainelSenhas { get; set; }
        public DbSet<Controle_Interno> Controle_Internoes { get; set; }
        public DbSet<Controle_Uso> Controle_Uso { get; set; }
        public DbSet<ControleAto> ControleAtos { get; set; }
        public DbSet<ControlePagamentoCredito> ControlePagamentoCreditoes { get; set; }
        public DbSet<ControlePagamentoDebito> ControlePagamentoDebitoes { get; set; }
        public DbSet<CustasDistribuicao> CustasDistribuicaos { get; set; }
        public DbSet<CustasNota> CustasNotas { get; set; }
        public DbSet<CustasProtesto> CustasProtestoes { get; set; }
        public DbSet<CustasRgi> CustasRgis { get; set; }
        public DbSet<Divida> Dividas { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Enotariado> Enotariadoes { get; set; }
        public DbSet<ImportarMa> ImportarMas { get; set; }
        public DbSet<IndiceEscritura> IndiceEscrituras { get; set; }
        public DbSet<IndiceProcuracao> IndiceProcuracaos { get; set; }
        public DbSet<IndiceRegistro> IndiceRegistros { get; set; }
        public DbSet<Indisponibilidade> Indisponibilidades { get; set; }
        public DbSet<ItensAtoNota> ItensAtoNotas { get; set; }
        public DbSet<ItensAtoRgi> ItensAtoRgis { get; set; }
        public DbSet<ItensControleAtoNota> ItensControleAtoNotas { get; set; }
        public DbSet<ItensControleAtoProtesto> ItensControleAtoProtestoes { get; set; }
        public DbSet<ItensCustasControleAtosNota> ItensCustasControleAtosNotas { get; set; }
        public DbSet<ItensCustasControleAtosProtesto> ItensCustasControleAtosProtestoes { get; set; }
        public DbSet<ItensCustasNota> ItensCustasNotas { get; set; }
        public DbSet<ItensCustasProtesto> ItensCustasProtestoes { get; set; }
        public DbSet<ItensCustasRgi> ItensCustasRgis { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Loteamento> Loteamentos { get; set; }
        public DbSet<Mensagem> Mensagems { get; set; }
        public DbSet<Parametro> Parametros { get; set; }
        public DbSet<Parcela> Parcelas { get; set; }
        public DbSet<Pc_Painel> Pc_Painel { get; set; }
        public DbSet<Portador> Portadors { get; set; }
        public DbSet<ReciboBalcao> ReciboBalcaos { get; set; }
        public DbSet<RepasseCaixa> RepasseCaixas { get; set; }
        public DbSet<Retirada_Caixa> Retirada_Caixa { get; set; }
        public DbSet<RetiradaMaterial> RetiradaMaterials { get; set; }
        public DbSet<SeloAtualBalcao> SeloAtualBalcaos { get; set; }
        public DbSet<Senha> Senhas { get; set; }
        public DbSet<sysdiagram> sysdiagrams { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<ValorPago> ValorPagoes { get; set; }
        public DbSet<VerificaBackup> VerificaBackups { get; set; }
        public DbSet<VerificacaoCaixa> VerificacaoCaixas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Adicionar_CaixaMap());
            modelBuilder.Configurations.Add(new AgendaEletronicaMap());
            modelBuilder.Configurations.Add(new AtendimentoMap());
            modelBuilder.Configurations.Add(new AtoMap());
            modelBuilder.Configurations.Add(new AtualizaSiteMap());
            modelBuilder.Configurations.Add(new Cadastro_PainelMap());
            modelBuilder.Configurations.Add(new Cadastro_PcMap());
            modelBuilder.Configurations.Add(new CadastroClienteMap());
            modelBuilder.Configurations.Add(new CadastroCompraMaterialMap());
            modelBuilder.Configurations.Add(new CadastroMaterialMap());
            modelBuilder.Configurations.Add(new CadChequeMap());
            modelBuilder.Configurations.Add(new CadMensalistaMap());
            modelBuilder.Configurations.Add(new ConexaoPainelSenhaMap());
            modelBuilder.Configurations.Add(new Controle_InternoMap());
            modelBuilder.Configurations.Add(new Controle_UsoMap());
            modelBuilder.Configurations.Add(new ControleAtoMap());
            modelBuilder.Configurations.Add(new ControlePagamentoCreditoMap());
            modelBuilder.Configurations.Add(new ControlePagamentoDebitoMap());
            modelBuilder.Configurations.Add(new CustasDistribuicaoMap());
            modelBuilder.Configurations.Add(new CustasNotaMap());
            modelBuilder.Configurations.Add(new CustasProtestoMap());
            modelBuilder.Configurations.Add(new CustasRgiMap());
            modelBuilder.Configurations.Add(new DividaMap());
            modelBuilder.Configurations.Add(new EmailMap());
            modelBuilder.Configurations.Add(new EnotariadoMap());
            modelBuilder.Configurations.Add(new ImportarMaMap());
            modelBuilder.Configurations.Add(new IndiceEscrituraMap());
            modelBuilder.Configurations.Add(new IndiceProcuracaoMap());
            modelBuilder.Configurations.Add(new IndiceRegistroMap());
            modelBuilder.Configurations.Add(new IndisponibilidadeMap());
            modelBuilder.Configurations.Add(new ItensAtoNotaMap());
            modelBuilder.Configurations.Add(new ItensAtoRgiMap());
            modelBuilder.Configurations.Add(new ItensControleAtoNotaMap());
            modelBuilder.Configurations.Add(new ItensControleAtoProtestoMap());
            modelBuilder.Configurations.Add(new ItensCustasControleAtosNotaMap());
            modelBuilder.Configurations.Add(new ItensCustasControleAtosProtestoMap());
            modelBuilder.Configurations.Add(new ItensCustasNotaMap());
            modelBuilder.Configurations.Add(new ItensCustasProtestoMap());
            modelBuilder.Configurations.Add(new ItensCustasRgiMap());
            modelBuilder.Configurations.Add(new LogMap());
            modelBuilder.Configurations.Add(new LoteamentoMap());
            modelBuilder.Configurations.Add(new MensagemMap());
            modelBuilder.Configurations.Add(new ParametroMap());
            modelBuilder.Configurations.Add(new ParcelaMap());
            modelBuilder.Configurations.Add(new Pc_PainelMap());
            modelBuilder.Configurations.Add(new PortadorMap());
            modelBuilder.Configurations.Add(new ReciboBalcaoMap());
            modelBuilder.Configurations.Add(new RepasseCaixaMap());
            modelBuilder.Configurations.Add(new Retirada_CaixaMap());
            modelBuilder.Configurations.Add(new RetiradaMaterialMap());
            modelBuilder.Configurations.Add(new SeloAtualBalcaoMap());
            modelBuilder.Configurations.Add(new SenhaMap());
            modelBuilder.Configurations.Add(new sysdiagramMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
            modelBuilder.Configurations.Add(new ValorPagoMap());
            modelBuilder.Configurations.Add(new VerificaBackupMap());
            modelBuilder.Configurations.Add(new VerificacaoCaixaMap());
        }
    }
}
