using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ParcelaMap : EntityTypeConfiguration<Parcela>
    {
        public ParcelaMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id_Parcela, t.Id_Divida, t.Id_Usuario, t.Data_Emissao, t.Data_Vencimento });

            // Properties
            this.Property(t => t.Id_Parcela)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Id_Divida)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Id_Usuario)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Data_Pagamento)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("Parcela");
            this.Property(t => t.Id_Parcela).HasColumnName("Id_Parcela");
            this.Property(t => t.Id_Divida).HasColumnName("Id_Divida");
            this.Property(t => t.Id_Usuario).HasColumnName("Id_Usuario");
            this.Property(t => t.Data_Emissao).HasColumnName("Data_Emissao");
            this.Property(t => t.Data_Vencimento).HasColumnName("Data_Vencimento");
            this.Property(t => t.Data_Pagamento).HasColumnName("Data_Pagamento");
            this.Property(t => t.Pago).HasColumnName("Pago");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
