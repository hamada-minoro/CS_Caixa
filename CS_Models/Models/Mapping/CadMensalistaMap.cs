using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Models.Models.Mapping
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

            // Table & Column Mappings
            this.ToTable("CadMensalista");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}
