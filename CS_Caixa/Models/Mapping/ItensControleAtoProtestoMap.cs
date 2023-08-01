using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ItensControleAtoProtestoMap : EntityTypeConfiguration<ItensControleAtoProtesto>
    {
        public ItensControleAtoProtestoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_ControleAtoProtesto);

            // Properties
            this.Property(t => t.TipoAto)
                .HasMaxLength(40);

            this.Property(t => t.Natureza)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("ItensControleAtoProtesto");
            this.Property(t => t.Id_ControleAtoProtesto).HasColumnName("Id_ControleAtoProtesto");
            this.Property(t => t.Id_ControleAto).HasColumnName("Id_ControleAto");
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
            this.HasRequired(t => t.ControleAto)
                .WithMany(t => t.ItensControleAtoProtestoes)
                .HasForeignKey(d => d.Id_ControleAto);

        }
    }
}
