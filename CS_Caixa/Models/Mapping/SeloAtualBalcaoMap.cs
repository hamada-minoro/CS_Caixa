using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class SeloAtualBalcaoMap : EntityTypeConfiguration<SeloAtualBalcao>
    {
        public SeloAtualBalcaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdSeloBalcao);

            // Properties
            this.Property(t => t.Letra)
                .HasMaxLength(4);

            // Table & Column Mappings
            this.ToTable("SeloAtualBalcao");
            this.Property(t => t.IdSeloBalcao).HasColumnName("IdSeloBalcao");
            this.Property(t => t.Letra).HasColumnName("Letra");
            this.Property(t => t.Numero).HasColumnName("Numero");
        }
    }
}
