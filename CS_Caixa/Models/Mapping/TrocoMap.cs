using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class TrocoMap : EntityTypeConfiguration<Troco>
    {
        public TrocoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdTroco, t.Data, t.Valor });

            // Properties
            this.Property(t => t.IdTroco)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Valor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("Troco");
            this.Property(t => t.IdTroco).HasColumnName("IdTroco");
            this.Property(t => t.IdAto).HasColumnName("IdAto");
            this.Property(t => t.IdReciboBalcao).HasColumnName("IdReciboBalcao");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
