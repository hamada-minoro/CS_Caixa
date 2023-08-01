using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class EnotariadoMap : EntityTypeConfiguration<Enotariado>
    {
        public EnotariadoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdEnotariado);

            // Properties
            // Table & Column Mappings
            this.ToTable("Enotariado");
            this.Property(t => t.IdEnotariado).HasColumnName("IdEnotariado");
            this.Property(t => t.IdAto).HasColumnName("IdAto");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
