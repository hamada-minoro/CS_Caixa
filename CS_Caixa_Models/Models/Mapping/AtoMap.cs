using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa_Models.Models.Mapping
{
    public class AtoMap : EntityTypeConfiguration<Ato>
    {
        public AtoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Ato);

            // Properties
            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Usuario)
                .HasMaxLength(100);

            this.Property(t => t.Atribuicao)
                .HasMaxLength(15);

            this.Property(t => t.Livro)
                .HasMaxLength(10);

            this.Property(t => t.Natureza)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Escrevente)
                .HasMaxLength(100);

            this.Property(t => t.Convenio)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TipoCobranca)
                .IsFixedLength()
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("Ato");
            this.Property(t => t.Id_Ato).HasColumnName("Id_Ato");
            this.Property(t => t.DataPagamento).HasColumnName("DataPagamento");
            this.Property(t => t.DataAto).HasColumnName("DataAto");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.FolhaInical).HasColumnName("FolhaInical");
            this.Property(t => t.FolhaFinal).HasColumnName("FolhaFinal");
            this.Property(t => t.NumeroAto).HasColumnName("NumeroAto");
            this.Property(t => t.Protocolo).HasColumnName("Protocolo");
            this.Property(t => t.Recibo).HasColumnName("Recibo");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Escrevente).HasColumnName("Escrevente");
            this.Property(t => t.Convenio).HasColumnName("Convenio");
            this.Property(t => t.TipoCobranca).HasColumnName("TipoCobranca");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.Distribuicao).HasColumnName("Distribuicao");
            this.Property(t => t.Indisponibilidade).HasColumnName("Indisponibilidade");
            this.Property(t => t.Prenotacao).HasColumnName("Prenotacao");
            this.Property(t => t.Ar).HasColumnName("Ar");
            this.Property(t => t.Bancaria).HasColumnName("Bancaria");
            this.Property(t => t.Total).HasColumnName("Total");

            // Relationships
            this.HasRequired(t => t.Ato2)
                .WithMany(t => t.Ato1)
                .HasForeignKey(d => d.IdUsuario);

        }
    }
}
