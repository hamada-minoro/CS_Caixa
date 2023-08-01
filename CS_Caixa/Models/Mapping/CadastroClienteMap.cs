using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CadastroClienteMap : EntityTypeConfiguration<CadastroCliente>
    {
        public CadastroClienteMap()
        {
            // Primary Key
            this.HasKey(t => t.CadastroClienteId);

            // Properties
            this.Property(t => t.DataUltimaAtualizacao)
                .HasMaxLength(10);

            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.CPF_CNPJ)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.TipoPessoa)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.Sexo)
                .IsFixedLength()
                .HasMaxLength(100);

            this.Property(t => t.RG)
                .HasMaxLength(20);

            this.Property(t => t.Endereco)
                .HasMaxLength(200);

            this.Property(t => t.Telefone)
                .HasMaxLength(15);

            this.Property(t => t.Email)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("CadastroCliente");
            this.Property(t => t.CadastroClienteId).HasColumnName("CadastroClienteId");
            this.Property(t => t.DataCadastro).HasColumnName("DataCadastro");
            this.Property(t => t.DataNascimento).HasColumnName("DataNascimento");
            this.Property(t => t.DataUltimaAtualizacao).HasColumnName("DataUltimaAtualizacao");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.CPF_CNPJ).HasColumnName("CPF_CNPJ");
            this.Property(t => t.TipoPessoa).HasColumnName("TipoPessoa");
            this.Property(t => t.Sexo).HasColumnName("Sexo");
            this.Property(t => t.RG).HasColumnName("RG");
            this.Property(t => t.Endereco).HasColumnName("Endereco");
            this.Property(t => t.Telefone).HasColumnName("Telefone");
            this.Property(t => t.Email).HasColumnName("Email");
        }
    }
}
