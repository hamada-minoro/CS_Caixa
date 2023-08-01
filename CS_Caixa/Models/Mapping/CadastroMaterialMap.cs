using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CadastroMaterialMap : EntityTypeConfiguration<CadastroMaterial>
    {
        public CadastroMaterialMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdDescricao, t.DescMaterial, t.Quantidade, t.Codigo });

            // Properties
            this.Property(t => t.IdDescricao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.DescMaterial)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Quantidade)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Codigo)
                .IsRequired()
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("CadastroMaterial");
            this.Property(t => t.IdDescricao).HasColumnName("IdDescricao");
            this.Property(t => t.DescMaterial).HasColumnName("DescMaterial");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade");
            this.Property(t => t.Codigo).HasColumnName("Codigo");
        }
    }
}
