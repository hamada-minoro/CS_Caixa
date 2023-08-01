using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CadChequeMap : EntityTypeConfiguration<CadCheque>
    {
        public CadChequeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Data });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Caixa)
                .HasMaxLength(15);

            this.Property(t => t.NumCheque)
                .HasMaxLength(20);

            this.Property(t => t.Obs)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CadCheque");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Caixa).HasColumnName("Caixa");
            this.Property(t => t.NumCheque).HasColumnName("NumCheque");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Obs).HasColumnName("Obs");
            this.Property(t => t.Data).HasColumnName("Data");
        }
    }
}
