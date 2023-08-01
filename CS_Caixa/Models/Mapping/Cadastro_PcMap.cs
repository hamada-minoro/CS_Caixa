using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Cadastro_PcMap : EntityTypeConfiguration<Cadastro_Pc>
    {
        public Cadastro_PcMap()
        {
            // Primary Key
            this.HasKey(t => t.Cadastro_Pc_Id);

            // Properties
            this.Property(t => t.Identificador_Pc)
                .HasMaxLength(30);

            this.Property(t => t.Nome_Pc)
                .HasMaxLength(30);

            this.Property(t => t.Caracter)
                .HasMaxLength(3);

            this.Property(t => t.Tipo_Atendimento)
                .HasMaxLength(11);

            this.Property(t => t.FalaOutros)
                .HasMaxLength(50);

            this.Property(t => t.Ip_Pc)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Cadastro_Pc");
            this.Property(t => t.Cadastro_Pc_Id).HasColumnName("Cadastro_Pc_Id");
            this.Property(t => t.Data_Cadastro).HasColumnName("Data_Cadastro");
            this.Property(t => t.Identificador_Pc).HasColumnName("Identificador_Pc");
            this.Property(t => t.Nome_Pc).HasColumnName("Nome_Pc");
            this.Property(t => t.Caracter).HasColumnName("Caracter");
            this.Property(t => t.Tipo_Atendimento).HasColumnName("Tipo_Atendimento");
            this.Property(t => t.FalaOutros).HasColumnName("FalaOutros");
            this.Property(t => t.Tipo_Entrada).HasColumnName("Tipo_Entrada");
            this.Property(t => t.Ip_Pc).HasColumnName("Ip_Pc");
            this.Property(t => t.Porta_Pc).HasColumnName("Porta_Pc");
            this.Property(t => t.SetorId).HasColumnName("SetorId");
            this.Property(t => t.TipoChamadaSenha).HasColumnName("TipoChamadaSenha");
        }
    }
}
