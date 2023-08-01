using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class LoteamentoMap : EntityTypeConfiguration<Loteamento>
    {
        public LoteamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.LoteamentoId);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(200);

            this.Property(t => t.Proprietario)
                .HasMaxLength(200);

            this.Property(t => t.Localizacao)
                .HasMaxLength(15);

            this.Property(t => t.NumeroInscricao)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("Loteamentos");
            this.Property(t => t.LoteamentoId).HasColumnName("LoteamentoId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Proprietario).HasColumnName("Proprietario");
            this.Property(t => t.Localizacao).HasColumnName("Localizacao");
            this.Property(t => t.Matricula).HasColumnName("Matricula");
            this.Property(t => t.NumeroInscricao).HasColumnName("NumeroInscricao");
        }
    }
}
