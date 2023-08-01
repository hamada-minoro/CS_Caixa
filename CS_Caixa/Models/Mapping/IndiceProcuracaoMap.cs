using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class IndiceProcuracaoMap : EntityTypeConfiguration<IndiceProcuracao>
    {
        public IndiceProcuracaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdIndiceProcuracao);

            // Properties
            this.Property(t => t.Outorgante)
                .HasMaxLength(255);

            this.Property(t => t.Outorgado)
                .HasMaxLength(255);

            this.Property(t => t.Dia)
                .HasMaxLength(5);

            this.Property(t => t.Mes)
                .HasMaxLength(5);

            this.Property(t => t.Ano)
                .HasMaxLength(5);

            this.Property(t => t.Ato)
                .HasMaxLength(10);

            this.Property(t => t.Livro)
                .HasMaxLength(10);

            this.Property(t => t.Fls)
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("IndiceProcuracao");
            this.Property(t => t.IdIndiceProcuracao).HasColumnName("IdIndiceProcuracao");
            this.Property(t => t.Outorgante).HasColumnName("Outorgante");
            this.Property(t => t.Outorgado).HasColumnName("Outorgado");
            this.Property(t => t.Dia).HasColumnName("Dia");
            this.Property(t => t.Mes).HasColumnName("Mes");
            this.Property(t => t.Ano).HasColumnName("Ano");
            this.Property(t => t.Ato).HasColumnName("Ato");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.Fls).HasColumnName("Fls");
        }
    }
}
