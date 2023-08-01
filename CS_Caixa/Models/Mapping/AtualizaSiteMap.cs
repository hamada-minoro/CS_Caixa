using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class AtualizaSiteMap : EntityTypeConfiguration<AtualizaSite>
    {
        public AtualizaSiteMap()
        {
            // Primary Key
            this.HasKey(t => t.AtualizaSiteId);

            // Properties
            this.Property(t => t.AtualizaSiteId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.DataAtualizacao)
                .HasMaxLength(10);

            this.Property(t => t.HoraAtualizacao)
                .HasMaxLength(8);

            this.Property(t => t.Status)
                .HasMaxLength(20);

            this.Property(t => t.PcAtualizacao)
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("AtualizaSite");
            this.Property(t => t.AtualizaSiteId).HasColumnName("AtualizaSiteId");
            this.Property(t => t.DataAtualizacao).HasColumnName("DataAtualizacao");
            this.Property(t => t.HoraAtualizacao).HasColumnName("HoraAtualizacao");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.PcAtualizacao).HasColumnName("PcAtualizacao");
        }
    }
}
