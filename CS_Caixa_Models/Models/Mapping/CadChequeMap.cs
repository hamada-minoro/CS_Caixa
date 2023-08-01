using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa_Models.Models.Mapping
{
    public class CadChequeMap : EntityTypeConfiguration<CadCheque>
    {
        public CadChequeMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.Valor, t.Data });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Caixa)
                .IsFixedLength()
                .HasMaxLength(15);

            this.Property(t => t.NumCheque)
                .IsFixedLength()
                .HasMaxLength(20);

            this.Property(t => t.Valor)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

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
