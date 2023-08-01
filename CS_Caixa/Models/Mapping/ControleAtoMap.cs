using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ControleAtoMap : EntityTypeConfiguration<ControleAto>
    {
        public ControleAtoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_ControleAtos);

            // Properties
            this.Property(t => t.Usuario)
                .HasMaxLength(100);

            this.Property(t => t.Atribuicao)
                .HasMaxLength(15);

            this.Property(t => t.LetraSelo)
                .HasMaxLength(4);

            this.Property(t => t.Faixa)
                .HasMaxLength(100);

            this.Property(t => t.Matricula)
                .HasMaxLength(10);

            this.Property(t => t.Livro)
                .HasMaxLength(10);

            this.Property(t => t.TipoAto)
                .HasMaxLength(40);

            this.Property(t => t.Natureza)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Convenio)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("ControleAtos");
            this.Property(t => t.Id_ControleAtos).HasColumnName("Id_ControleAtos");
            this.Property(t => t.DataAto).HasColumnName("DataAto");
            this.Property(t => t.AtoNaoGratuito).HasColumnName("AtoNaoGratuito");
            this.Property(t => t.AtoGratuito).HasColumnName("AtoGratuito");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.LetraSelo).HasColumnName("LetraSelo");
            this.Property(t => t.NumeroSelo).HasColumnName("NumeroSelo");
            this.Property(t => t.Faixa).HasColumnName("Faixa");
            this.Property(t => t.Matricula).HasColumnName("Matricula");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.FolhaInical).HasColumnName("FolhaInical");
            this.Property(t => t.FolhaFinal).HasColumnName("FolhaFinal");
            this.Property(t => t.NumeroAto).HasColumnName("NumeroAto");
            this.Property(t => t.Protocolo).HasColumnName("Protocolo");
            this.Property(t => t.Recibo).HasColumnName("Recibo");
            this.Property(t => t.Id_Ato).HasColumnName("Id_Ato");
            this.Property(t => t.ReciboBalcao).HasColumnName("ReciboBalcao");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Convenio).HasColumnName("Convenio");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.QtdAtos).HasColumnName("QtdAtos");
            this.Property(t => t.Total).HasColumnName("Total");
        }
    }
}
