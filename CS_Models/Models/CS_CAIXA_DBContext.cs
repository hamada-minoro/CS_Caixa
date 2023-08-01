using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using CS_Models.Models.Mapping;

namespace CS_Models.Models
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
        public DbSet<Ato> Atoes { get; set; }
        public DbSet<CadCheque> CadCheques { get; set; }
        public DbSet<CadMensalista> CadMensalistas { get; set; }
        public DbSet<Retirada_Caixa> Retirada_Caixa { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new Adicionar_CaixaMap());
            modelBuilder.Configurations.Add(new AtoMap());
            modelBuilder.Configurations.Add(new CadChequeMap());
            modelBuilder.Configurations.Add(new CadMensalistaMap());
            modelBuilder.Configurations.Add(new Retirada_CaixaMap());
            modelBuilder.Configurations.Add(new UsuarioMap());
        }
    }
}
