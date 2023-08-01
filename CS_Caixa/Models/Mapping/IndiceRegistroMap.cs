using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class IndiceRegistroMap : EntityTypeConfiguration<IndiceRegistro>
    {
        public IndiceRegistroMap()
        {
            // Primary Key
            this.HasKey(t => t.IdIndiceRegistros);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(255);

            this.Property(t => t.Livro)
                .HasMaxLength(255);

            this.Property(t => t.Reg)
                .HasMaxLength(255);

            this.Property(t => t.Numero)
                .HasMaxLength(255);

            this.Property(t => t.Ordem)
                .HasMaxLength(255);

            this.Property(t => t.Fls)
                .HasMaxLength(255);

            this.Property(t => t.TipoPessoa)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CpfCnpj)
                .HasMaxLength(15);

            this.Property(t => t.TipoAto)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("IndiceRegistros");
            this.Property(t => t.IdIndiceRegistros).HasColumnName("IdIndiceRegistros");
            this.Property(t => t.Nome).HasColumnName("Nome");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.Reg).HasColumnName("Reg");
            this.Property(t => t.Numero).HasColumnName("Numero");
            this.Property(t => t.Ordem).HasColumnName("Ordem");
            this.Property(t => t.Fls).HasColumnName("Fls");
            this.Property(t => t.TipoPessoa).HasColumnName("TipoPessoa");
            this.Property(t => t.CpfCnpj).HasColumnName("CpfCnpj");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.DataRegistro).HasColumnName("DataRegistro");
            this.Property(t => t.DataVenda).HasColumnName("DataVenda");
            this.Property(t => t.Enviado).HasColumnName("Enviado");
        }
    }
}
