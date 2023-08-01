using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class RecolhimentoMap : EntityTypeConfiguration<Recolhimento>
    {
        public RecolhimentoMap()
        {
            // Primary Key
            this.HasKey(t => t.RecolhimentoId);

            // Properties
            this.Property(t => t.TipoAto)
                .HasMaxLength(200);

            this.Property(t => t.Natureza)
                .HasMaxLength(200);

            this.Property(t => t.Matricula)
                .HasMaxLength(10);

            this.Property(t => t.Livro)
                .HasMaxLength(20);

            this.Property(t => t.Folha)
                .HasMaxLength(10);

            this.Property(t => t.Ato)
                .HasMaxLength(10);

            this.Property(t => t.Selo)
                .HasMaxLength(10);

            this.Property(t => t.Atribuicao)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Recolhimento");
            this.Property(t => t.RecolhimentoId).HasColumnName("RecolhimentoId");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Protocolo).HasColumnName("Protocolo");
            this.Property(t => t.Matricula).HasColumnName("Matricula");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.Folha).HasColumnName("Folha");
            this.Property(t => t.Ato).HasColumnName("Ato");
            this.Property(t => t.Selo).HasColumnName("Selo");
            this.Property(t => t.Gratuito).HasColumnName("Gratuito");
            this.Property(t => t.Emol).HasColumnName("Emol");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fund).HasColumnName("Fund");
            this.Property(t => t.Funp).HasColumnName("Funp");
            this.Property(t => t.Funa).HasColumnName("Funa");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.Convenio).HasColumnName("Convenio");
            this.Property(t => t.Excedente).HasColumnName("Excedente");
        }
    }
}
