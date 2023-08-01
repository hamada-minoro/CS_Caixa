using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ValorPagoMap : EntityTypeConfiguration<ValorPago>
    {
        public ValorPagoMap()
        {
            // Primary Key
            this.HasKey(t => t.ValorPagoId);

            // Properties
            this.Property(t => t.DataModificado)
                .HasMaxLength(10);

            this.Property(t => t.HoraModificado)
                .HasMaxLength(10);

            this.Property(t => t.NomeUsuario)
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("ValorPago");
            this.Property(t => t.ValorPagoId).HasColumnName("ValorPagoId");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.IdAto).HasColumnName("IdAto");
            this.Property(t => t.IdReciboBalcao).HasColumnName("IdReciboBalcao");
            this.Property(t => t.Dinheiro).HasColumnName("Dinheiro");
            this.Property(t => t.Deposito).HasColumnName("Deposito");
            this.Property(t => t.Cheque).HasColumnName("Cheque");
            this.Property(t => t.ChequePre).HasColumnName("ChequePre");
            this.Property(t => t.Boleto).HasColumnName("Boleto");
            this.Property(t => t.Mensalista).HasColumnName("Mensalista");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.Troco).HasColumnName("Troco");
            this.Property(t => t.IdPagamento).HasColumnName("IdPagamento");
            this.Property(t => t.CartaoCredito).HasColumnName("CartaoCredito");
            this.Property(t => t.DataModificado).HasColumnName("DataModificado");
            this.Property(t => t.HoraModificado).HasColumnName("HoraModificado");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.NomeUsuario).HasColumnName("NomeUsuario");
        }
    }
}
