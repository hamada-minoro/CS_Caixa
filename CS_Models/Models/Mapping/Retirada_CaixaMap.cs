using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Models.Models.Mapping
{
    public class Retirada_CaixaMap : EntityTypeConfiguration<Retirada_Caixa>
    {
        public Retirada_CaixaMap()
        {
            // Primary Key
            this.HasKey(t => t.Cod);

            // Properties
            this.Property(t => t.Descricao)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Retirada_Caixa");
            this.Property(t => t.Cod).HasColumnName("Cod");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
