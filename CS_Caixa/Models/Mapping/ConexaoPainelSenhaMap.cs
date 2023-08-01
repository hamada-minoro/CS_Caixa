using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ConexaoPainelSenhaMap : EntityTypeConfiguration<ConexaoPainelSenha>
    {
        public ConexaoPainelSenhaMap()
        {
            // Primary Key
            this.HasKey(t => t.ConexaoId);

            // Properties
            this.Property(t => t.IpServidorAtendimento)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.IpServidorAtendimentoNotas)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("ConexaoPainelSenhas");
            this.Property(t => t.ConexaoId).HasColumnName("ConexaoId");
            this.Property(t => t.IpServidorAtendimento).HasColumnName("IpServidorAtendimento");
            this.Property(t => t.PortaConexao).HasColumnName("PortaConexao");
            this.Property(t => t.IpServidorAtendimentoNotas).HasColumnName("IpServidorAtendimentoNotas");
            this.Property(t => t.PortaConexaoNotas).HasColumnName("PortaConexaoNotas");
        }
    }
}
