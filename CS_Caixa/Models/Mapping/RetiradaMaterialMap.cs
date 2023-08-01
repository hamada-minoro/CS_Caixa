using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class RetiradaMaterialMap : EntityTypeConfiguration<RetiradaMaterial>
    {
        public RetiradaMaterialMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdRetiradaMaterial, t.Data, t.Quantidade, t.Material, t.Funcionario });

            // Properties
            this.Property(t => t.IdRetiradaMaterial)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Quantidade)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Material)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Funcionario)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("RetiradaMaterial");
            this.Property(t => t.IdRetiradaMaterial).HasColumnName("IdRetiradaMaterial");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade");
            this.Property(t => t.Material).HasColumnName("Material");
            this.Property(t => t.Funcionario).HasColumnName("Funcionario");
        }
    }
}
