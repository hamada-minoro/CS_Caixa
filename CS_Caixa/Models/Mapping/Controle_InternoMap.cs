using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Controle_InternoMap : EntityTypeConfiguration<Controle_Interno>
    {
        public Controle_InternoMap()
        {
            // Primary Key
            this.HasKey(t => t.ControleInternoId);

            // Properties
            this.Property(t => t.Tipo)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.EntradaSaida)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Usuario)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Controle Interno");
            this.Property(t => t.ControleInternoId).HasColumnName("ControleInternoId");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.EntradaSaida).HasColumnName("EntradaSaida");
            this.Property(t => t.Valor).HasColumnName("Valor");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
        }
    }
}
