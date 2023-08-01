using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ReciboBalcaoMap : EntityTypeConfiguration<ReciboBalcao>
    {
        public ReciboBalcaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdReciboBalcao);

            // Properties
            this.Property(t => t.IdReciboBalcao)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Usuario)
                .HasMaxLength(100);

            this.Property(t => t.Status)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.TipoCustas)
                .HasMaxLength(50);

            this.Property(t => t.TipoPagamento)
                .HasMaxLength(50);

            this.Property(t => t.Mensalista)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ReciboBalcao");
            this.Property(t => t.IdReciboBalcao).HasColumnName("IdReciboBalcao");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.NumeroRecibo).HasColumnName("NumeroRecibo");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Pago).HasColumnName("Pago");
            this.Property(t => t.TipoCustas).HasColumnName("TipoCustas");
            this.Property(t => t.TipoPagamento).HasColumnName("TipoPagamento");
            this.Property(t => t.QuantAut).HasColumnName("QuantAut");
            this.Property(t => t.QuantAbert).HasColumnName("QuantAbert");
            this.Property(t => t.QuantRecAut).HasColumnName("QuantRecAut");
            this.Property(t => t.QuantRecSem).HasColumnName("QuantRecSem");
            this.Property(t => t.QuantCopia).HasColumnName("QuantCopia");
            this.Property(t => t.ValorAdicionar).HasColumnName("ValorAdicionar");
            this.Property(t => t.ValorDesconto).HasColumnName("ValorDesconto");
            this.Property(t => t.Mensalista).HasColumnName("Mensalista");
            this.Property(t => t.NumeroRequisicao).HasColumnName("NumeroRequisicao");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.ValorPago).HasColumnName("ValorPago");
            this.Property(t => t.ValorTroco).HasColumnName("ValorTroco");
            this.Property(t => t.QuantMaterializacao).HasColumnName("QuantMaterializacao");
            this.Property(t => t.IdAtendimento).HasColumnName("IdAtendimento");
        }
    }
}
