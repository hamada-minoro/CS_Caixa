using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class IndiceEscrituraMap : EntityTypeConfiguration<IndiceEscritura>
    {
        public IndiceEscrituraMap()
        {
            // Primary Key
            this.HasKey(t => t.IdIndiceEscritura);

            // Properties
            this.Property(t => t.Outorgante)
                .HasMaxLength(255);

            this.Property(t => t.Outorgado)
                .HasMaxLength(255);

            this.Property(t => t.DiaDist)
                .HasMaxLength(2);

            this.Property(t => t.MesDist)
                .HasMaxLength(2);

            this.Property(t => t.AnoDist)
                .HasMaxLength(4);

            this.Property(t => t.Natureza)
                .HasMaxLength(255);

            this.Property(t => t.Dia)
                .HasMaxLength(2);

            this.Property(t => t.Mes)
                .HasMaxLength(2);

            this.Property(t => t.Ano)
                .HasMaxLength(4);

            this.Property(t => t.Ato)
                .HasMaxLength(5);

            this.Property(t => t.Livro)
                .HasMaxLength(5);

            this.Property(t => t.Fls)
                .HasMaxLength(5);

            this.Property(t => t.Ordem)
                .HasMaxLength(5);

            // Table & Column Mappings
            this.ToTable("IndiceEscritura");
            this.Property(t => t.IdIndiceEscritura).HasColumnName("IdIndiceEscritura");
            this.Property(t => t.Outorgante).HasColumnName("Outorgante");
            this.Property(t => t.Outorgado).HasColumnName("Outorgado");
            this.Property(t => t.DiaDist).HasColumnName("DiaDist");
            this.Property(t => t.MesDist).HasColumnName("MesDist");
            this.Property(t => t.AnoDist).HasColumnName("AnoDist");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Dia).HasColumnName("Dia");
            this.Property(t => t.Mes).HasColumnName("Mes");
            this.Property(t => t.Ano).HasColumnName("Ano");
            this.Property(t => t.Ato).HasColumnName("Ato");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.Fls).HasColumnName("Fls");
            this.Property(t => t.Ordem).HasColumnName("Ordem");
        }
    }
}
