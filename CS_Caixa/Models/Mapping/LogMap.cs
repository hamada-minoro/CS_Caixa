using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class LogMap : EntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            // Primary Key
            this.HasKey(t => t.IdLog);

            // Properties
            this.Property(t => t.Usuario)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Acao)
                .IsRequired()
                .HasMaxLength(30);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Tela)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Log");
            this.Property(t => t.IdLog).HasColumnName("IdLog");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Acao).HasColumnName("Acao");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Tela).HasColumnName("Tela");
        }
    }
}
