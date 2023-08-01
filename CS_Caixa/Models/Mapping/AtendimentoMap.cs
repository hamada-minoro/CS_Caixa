using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class AtendimentoMap : EntityTypeConfiguration<Atendimento>
    {
        public AtendimentoMap()
        {
            // Primary Key
            this.HasKey(t => t.AtendimentoId);

            // Properties
            this.Property(t => t.Senha)
                .HasMaxLength(6);

            this.Property(t => t.TipoAtendimento)
                .HasMaxLength(1);

            this.Property(t => t.HoraRetirada)
                .HasMaxLength(8);

            this.Property(t => t.Status)
                .HasMaxLength(15);

            this.Property(t => t.HoraAtendimento)
                .HasMaxLength(8);

            this.Property(t => t.NomeAtendente)
                .HasMaxLength(50);

            this.Property(t => t.HoraFinalizado)
                .HasMaxLength(8);

            this.Property(t => t.OrdemChamada)
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("Atendimento");
            this.Property(t => t.AtendimentoId).HasColumnName("AtendimentoId");
            this.Property(t => t.Fila).HasColumnName("Fila");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.TipoAtendimento).HasColumnName("TipoAtendimento");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.HoraRetirada).HasColumnName("HoraRetirada");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.HoraAtendimento).HasColumnName("HoraAtendimento");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.NomeAtendente).HasColumnName("NomeAtendente");
            this.Property(t => t.HoraFinalizado).HasColumnName("HoraFinalizado");
            this.Property(t => t.OrdemChamada).HasColumnName("OrdemChamada");
        }
    }
}
