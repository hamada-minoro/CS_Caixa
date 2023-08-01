using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class VerificaBackupMap : EntityTypeConfiguration<VerificaBackup>
    {
        public VerificaBackupMap()
        {
            // Primary Key
            this.HasKey(t => t.VerificaBackupId);

            // Properties
            this.Property(t => t.HoraVerificacao)
                .HasMaxLength(10);

            this.Property(t => t.Status)
                .HasMaxLength(50);

            this.Property(t => t.MaquinaVerificou)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("VerificaBackup");
            this.Property(t => t.VerificaBackupId).HasColumnName("VerificaBackupId");
            this.Property(t => t.DataVerificacao).HasColumnName("DataVerificacao");
            this.Property(t => t.HoraVerificacao).HasColumnName("HoraVerificacao");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.MaquinaVerificou).HasColumnName("MaquinaVerificou");
        }
    }
}
