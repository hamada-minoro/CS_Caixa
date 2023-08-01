using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class AgendaEletronicaMap : EntityTypeConfiguration<AgendaEletronica>
    {
        public AgendaEletronicaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdAgenda);

            // Properties
            this.Property(t => t.Hora)
                .HasMaxLength(5);

            this.Property(t => t.Usuario)
                .HasMaxLength(150);

            this.Property(t => t.CorBotao)
                .HasMaxLength(20);

            this.Property(t => t.TipoAto)
                .HasMaxLength(150);

            this.Property(t => t.NomeCliente)
                .HasMaxLength(150);

            this.Property(t => t.Observacao)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("AgendaEletronica");
            this.Property(t => t.IdAgenda).HasColumnName("IdAgenda");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Hora).HasColumnName("Hora");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.CorBotao).HasColumnName("CorBotao");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.NomeCliente).HasColumnName("NomeCliente");
            this.Property(t => t.Observacao).HasColumnName("Observacao");
        }
    }
}
