using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa_Models.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Usuario);

            // Properties
            this.Property(t => t.NomeUsu)
                .HasMaxLength(50);

            this.Property(t => t.Senha)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Usuarios");
            this.Property(t => t.Id_Usuario).HasColumnName("Id_Usuario");
            this.Property(t => t.NomeUsu).HasColumnName("NomeUsu");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.Notas).HasColumnName("Notas");
            this.Property(t => t.Rgi).HasColumnName("Rgi");
            this.Property(t => t.Protesto).HasColumnName("Protesto");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.Indice).HasColumnName("Indice");
            this.Property(t => t.Caixa).HasColumnName("Caixa");
            this.Property(t => t.Balcao).HasColumnName("Balcao");
        }
    }
}
