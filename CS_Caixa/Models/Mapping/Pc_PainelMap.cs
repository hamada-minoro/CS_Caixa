using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Pc_PainelMap : EntityTypeConfiguration<Pc_Painel>
    {
        public Pc_PainelMap()
        {
            // Primary Key
            this.HasKey(t => t.Pc_Painel_Id);

            // Properties
            this.Property(t => t.Cadastro_Pc_Id)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Cadastro_Painel_Id)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("Pc_Painel");
            this.Property(t => t.Pc_Painel_Id).HasColumnName("Pc_Painel_Id");
            this.Property(t => t.Cadastro_Pc_Id).HasColumnName("Cadastro_Pc_Id");
            this.Property(t => t.Cadastro_Painel_Id).HasColumnName("Cadastro_Painel_Id");
        }
    }
}
