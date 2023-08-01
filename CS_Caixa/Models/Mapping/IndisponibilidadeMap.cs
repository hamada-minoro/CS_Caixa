using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class IndisponibilidadeMap : EntityTypeConfiguration<Indisponibilidade>
    {
        public IndisponibilidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.IdIndisponibilidade);

            // Properties
            this.Property(t => t.Titulo)
                .HasMaxLength(100);

            this.Property(t => t.Nome)
                .HasMaxLength(255);

            this.Property(t => t.CpfCnpj)
                .HasMaxLength(255);

            this.Property(t => t.Oficio)
                .HasMaxLength(255);

            this.Property(t => t.Aviso)
                .HasMaxLength(255);

            this.Property(t => t.Processo)
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Indisponibilidade");
            this.Property(t => t.IdIndisponibilidade).HasColumnName("IdIndisponibilidade");
            this.Property(t => t.Titulo).HasColumnName("Titulo");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.CpfCnpj).HasColumnName("CpfCnpj");
            this.Property(t => t.Oficio).HasColumnName("Oficio");
            this.Property(t => t.Aviso).HasColumnName("Aviso");
            this.Property(t => t.Processo).HasColumnName("Processo");
            this.Property(t => t.Valor).HasColumnName("Valor");
        }
    }
}
