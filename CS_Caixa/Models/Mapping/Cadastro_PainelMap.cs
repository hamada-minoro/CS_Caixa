using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Cadastro_PainelMap : EntityTypeConfiguration<Cadastro_Painel>
    {
        public Cadastro_PainelMap()
        {
            // Primary Key
            this.HasKey(t => t.Cadastro_Painel_Id);

            // Properties
            this.Property(t => t.Identificador_Pc)
                .HasMaxLength(30);

            this.Property(t => t.Nome_Pc)
                .HasMaxLength(30);

            this.Property(t => t.Ip_Pc)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Cadastro_Painel");
            this.Property(t => t.Cadastro_Painel_Id).HasColumnName("Cadastro_Painel_Id");
            this.Property(t => t.Data_Cadastro).HasColumnName("Data_Cadastro");
            this.Property(t => t.Identificador_Pc).HasColumnName("Identificador_Pc");
            this.Property(t => t.Nome_Pc).HasColumnName("Nome_Pc");
            this.Property(t => t.Ip_Pc).HasColumnName("Ip_Pc");
            this.Property(t => t.Porta_Pc).HasColumnName("Porta_Pc");
        }
    }
}
