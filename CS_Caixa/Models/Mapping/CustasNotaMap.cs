using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class CustasNotaMap : EntityTypeConfiguration<CustasNota>
    {
        public CustasNotaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Custas);

            // Properties
            this.Property(t => t.VAI)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TAB)
                .HasMaxLength(20);

            this.Property(t => t.ITEM)
                .HasMaxLength(20);

            this.Property(t => t.SUB)
                .HasMaxLength(20);

            this.Property(t => t.DESCR)
                .HasMaxLength(100);

            this.Property(t => t.TEXTO)
                .HasMaxLength(250);

            this.Property(t => t.TIPO)
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("CustasNotas");
            this.Property(t => t.Id_Custas).HasColumnName("Id_Custas");
            this.Property(t => t.ORDEM).HasColumnName("ORDEM");
            this.Property(t => t.ANO).HasColumnName("ANO");
            this.Property(t => t.VAI).HasColumnName("VAI");
            this.Property(t => t.TAB).HasColumnName("TAB");
            this.Property(t => t.ITEM).HasColumnName("ITEM");
            this.Property(t => t.SUB).HasColumnName("SUB");
            this.Property(t => t.DESCR).HasColumnName("DESCR");
            this.Property(t => t.VALOR).HasColumnName("VALOR");
            this.Property(t => t.TEXTO).HasColumnName("TEXTO");
            this.Property(t => t.TIPO).HasColumnName("TIPO");
        }
    }
}
