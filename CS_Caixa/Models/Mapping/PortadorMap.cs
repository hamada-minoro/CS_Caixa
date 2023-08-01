using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class PortadorMap : EntityTypeConfiguration<Portador>
    {
        public PortadorMap()
        {
            // Primary Key
            this.HasKey(t => t.ID_PORTADOR);

            // Properties
            this.Property(t => t.CODIGO)
                .HasMaxLength(10);

            this.Property(t => t.NOME)
                .HasMaxLength(100);

            this.Property(t => t.TIPO)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.DOCUMENTO)
                .HasMaxLength(14);

            this.Property(t => t.ENDERECO)
                .HasMaxLength(100);

            this.Property(t => t.BANCO)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CONVENIO)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.CONTA)
                .HasMaxLength(20);

            this.Property(t => t.OBSERVACAO)
                .HasMaxLength(100);

            this.Property(t => t.AGENCIA)
                .HasMaxLength(6);

            this.Property(t => t.PRACA)
                .HasMaxLength(7);

            this.Property(t => t.CRA)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.NOMINAL)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.FORCA_LEI)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("Portador");
            this.Property(t => t.ID_PORTADOR).HasColumnName("ID_PORTADOR");
            this.Property(t => t.CODIGO).HasColumnName("CODIGO");
            this.Property(t => t.NOME).HasColumnName("NOME");
            this.Property(t => t.TIPO).HasColumnName("TIPO");
            this.Property(t => t.DOCUMENTO).HasColumnName("DOCUMENTO");
            this.Property(t => t.ENDERECO).HasColumnName("ENDERECO");
            this.Property(t => t.BANCO).HasColumnName("BANCO");
            this.Property(t => t.CONVENIO).HasColumnName("CONVENIO");
            this.Property(t => t.CONTA).HasColumnName("CONTA");
            this.Property(t => t.OBSERVACAO).HasColumnName("OBSERVACAO");
            this.Property(t => t.AGENCIA).HasColumnName("AGENCIA");
            this.Property(t => t.PRACA).HasColumnName("PRACA");
            this.Property(t => t.CRA).HasColumnName("CRA");
            this.Property(t => t.SEQUENCIA).HasColumnName("SEQUENCIA");
            this.Property(t => t.VALOR_DOC).HasColumnName("VALOR_DOC");
            this.Property(t => t.VALOR_TED).HasColumnName("VALOR_TED");
            this.Property(t => t.NOMINAL).HasColumnName("NOMINAL");
            this.Property(t => t.FORCA_LEI).HasColumnName("FORCA_LEI");
        }
    }
}
