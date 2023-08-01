using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ImportarMaMap : EntityTypeConfiguration<ImportarMa>
    {
        public ImportarMaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdImportarMas);

            // Properties
            this.Property(t => t.Atribuicao)
                .HasMaxLength(50);

            this.Property(t => t.TipoAto)
                .HasMaxLength(50);

            this.Property(t => t.Selo)
                .HasMaxLength(9);

            this.Property(t => t.Aleatorio)
                .HasMaxLength(3);

            this.Property(t => t.TipoCobranca)
                .HasMaxLength(2);

            // Table & Column Mappings
            this.ToTable("ImportarMas");
            this.Property(t => t.IdImportarMas).HasColumnName("IdImportarMas");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.Selo).HasColumnName("Selo");
            this.Property(t => t.Aleatorio).HasColumnName("Aleatorio");
            this.Property(t => t.TipoCobranca).HasColumnName("TipoCobranca");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Ressag).HasColumnName("Ressag");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.Distribuidor).HasColumnName("Distribuidor");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Total).HasColumnName("Total");
        }
    }
}
