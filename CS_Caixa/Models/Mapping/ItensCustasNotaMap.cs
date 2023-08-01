using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ItensCustasNotaMap : EntityTypeConfiguration<ItensCustasNota>
    {
        public ItensCustasNotaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Custa);

            // Properties
            this.Property(t => t.Tabela)
                .HasMaxLength(20);

            this.Property(t => t.Item)
                .HasMaxLength(20);

            this.Property(t => t.SubItem)
                .HasMaxLength(20);

            this.Property(t => t.Quantidade)
                .HasMaxLength(10);

            this.Property(t => t.Complemento)
                .HasMaxLength(50);

            this.Property(t => t.Excessao)
                .HasMaxLength(50);

            this.Property(t => t.Descricao)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("ItensCustasNotas");
            this.Property(t => t.Id_Custa).HasColumnName("Id_Custa");
            this.Property(t => t.Id_Ato).HasColumnName("Id_Ato");
            this.Property(t => t.Id_AtoNotas).HasColumnName("Id_AtoNotas");
            this.Property(t => t.Tabela).HasColumnName("Tabela");
            this.Property(t => t.Item).HasColumnName("Item");
            this.Property(t => t.SubItem).HasColumnName("SubItem");
            this.Property(t => t.Quantidade).HasColumnName("Quantidade");
            this.Property(t => t.Complemento).HasColumnName("Complemento");
            this.Property(t => t.Excessao).HasColumnName("Excessao");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.Descricao).HasColumnName("Descricao");

            // Relationships
            this.HasRequired(t => t.Ato)
                .WithMany(t => t.ItensCustasNotas)
                .HasForeignKey(d => d.Id_Ato);

        }
    }
}
