using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ParteMap : EntityTypeConfiguration<Parte>
    {
        public ParteMap()
        {
            // Primary Key
            this.HasKey(t => t.ParteId);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(150);

            this.Property(t => t.Cpf)
                .HasMaxLength(14);

            this.Property(t => t.Endereco)
                .HasMaxLength(255);

            this.Property(t => t.Telefone)
                .HasMaxLength(11);

            this.Property(t => t.Celular)
                .HasMaxLength(11);

            this.Property(t => t.Email)
                .HasMaxLength(100);

            this.Property(t => t.Outorgado)
                .HasMaxLength(150);

            this.Property(t => t.CpfOutorgado)
                .HasMaxLength(14);

            // Table & Column Mappings
            this.ToTable("Parte");
            this.Property(t => t.ParteId).HasColumnName("ParteId");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Cpf).HasColumnName("Cpf");
            this.Property(t => t.Endereco).HasColumnName("Endereco");
            this.Property(t => t.Telefone).HasColumnName("Telefone");
            this.Property(t => t.Celular).HasColumnName("Celular");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Outorgado).HasColumnName("Outorgado");
            this.Property(t => t.CpfOutorgado).HasColumnName("CpfOutorgado");
        }
    }
}
