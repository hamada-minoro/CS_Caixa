using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Adicionar_CaixaMap : EntityTypeConfiguration<Adicionar_Caixa>
    {
        public Adicionar_CaixaMap()
        {
            // Primary Key
            this.HasKey(t => t.Cod);

            // Properties
            this.Property(t => t.Atribuicao)
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(255);

            this.Property(t => t.TpPagamento)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Adicionar_Caixa");
            this.Property(t => t.Cod).HasColumnName("Cod");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.TpPagamento).HasColumnName("TpPagamento");
        }
    }
}
