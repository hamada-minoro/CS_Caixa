using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class EmailMap : EntityTypeConfiguration<Email>
    {
        public EmailMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EmailId, t.Data, t.Indicador, t.Documento, t.Enviado });

            // Properties
            this.Property(t => t.EmailId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Indicador)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Documento)
                .IsRequired()
                .HasMaxLength(15);

            this.Property(t => t.Mensagem)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Email");
            this.Property(t => t.EmailId).HasColumnName("EmailId");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Indicador).HasColumnName("Indicador");
            this.Property(t => t.Documento).HasColumnName("Documento");
            this.Property(t => t.Enviado).HasColumnName("Enviado");
            this.Property(t => t.Mensagem).HasColumnName("Mensagem");
        }
    }
}
