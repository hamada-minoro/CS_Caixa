using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CadastroCompraMaterialMap : EntityTypeConfiguration<CadastroCompraMaterial>
    {
        public CadastroCompraMaterialMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdCompraMaterial, t.Data, t.Quant, t.CadFunc });

            // Properties
            this.Property(t => t.IdCompraMaterial)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Quant)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NotaFiscal)
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(100);

            this.Property(t => t.CadFunc)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("CadastroCompraMaterial");
            this.Property(t => t.IdCompraMaterial).HasColumnName("IdCompraMaterial");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Quant).HasColumnName("Quant");
            this.Property(t => t.NotaFiscal).HasColumnName("NotaFiscal");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.ValorUni).HasColumnName("ValorUni");
            this.Property(t => t.ValorTotal).HasColumnName("ValorTotal");
            this.Property(t => t.CadFunc).HasColumnName("CadFunc");
        }
    }
}
