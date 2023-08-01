using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ItensAtoNotaMap : EntityTypeConfiguration<ItensAtoNota>
    {
        public ItensAtoNotaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_AtoNotas);

            // Properties
            this.Property(t => t.TipoAto)
                .HasMaxLength(40);

            this.Property(t => t.Natureza)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("ItensAtoNotas");
            this.Property(t => t.Id_AtoNotas).HasColumnName("Id_AtoNotas");
            this.Property(t => t.Id_Ato).HasColumnName("Id_Ato");
            this.Property(t => t.Cont).HasColumnName("Cont");
            this.Property(t => t.Protocolo).HasColumnName("Protocolo");
            this.Property(t => t.Recibo).HasColumnName("Recibo");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.Distribuicao).HasColumnName("Distribuicao");
            this.Property(t => t.QuantDistrib).HasColumnName("QuantDistrib");
            this.Property(t => t.Total).HasColumnName("Total");

            // Relationships
            this.HasRequired(t => t.Ato)
                .WithMany(t => t.ItensAtoNotas)
                .HasForeignKey(d => d.Id_Ato);

        }
    }
}
