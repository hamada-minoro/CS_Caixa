using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ControlePagamentoCreditoMap : EntityTypeConfiguration<ControlePagamentoCredito>
    {
        public ControlePagamentoCreditoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TipoCredito)
                .HasMaxLength(50);

            this.Property(t => t.Usuario)
                .HasMaxLength(100);

            this.Property(t => t.Origem)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ControlePagamentoCredito");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.TipoCredito).HasColumnName("TipoCredito");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Origem).HasColumnName("Origem");
            this.Property(t => t.Importado).HasColumnName("Importado");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
