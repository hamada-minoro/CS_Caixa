using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CadMensalistaMap : EntityTypeConfiguration<CadMensalista>
    {
        public CadMensalistaMap()
        {
            // Primary Key
            this.HasKey(t => t.Codigo);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(255);

            this.Property(t => t.Cod)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("CadMensalista");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Cod).HasColumnName("Cod");
        }
    }
}
