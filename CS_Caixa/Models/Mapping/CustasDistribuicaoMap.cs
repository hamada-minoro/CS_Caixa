using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CustasDistribuicaoMap : EntityTypeConfiguration<CustasDistribuicao>
    {
        public CustasDistribuicaoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_custas);

            // Properties
            // Table & Column Mappings
            this.ToTable("CustasDistribuicao");
            this.Property(t => t.Id_custas).HasColumnName("Id_custas");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Quant_Exced).HasColumnName("Quant_Exced");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.VrFixo).HasColumnName("VrFixo");
            this.Property(t => t.VrExced).HasColumnName("VrExced");
            this.Property(t => t.Ano).HasColumnName("Ano");
        }
    }
}
