using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class AtoMap : EntityTypeConfiguration<Ato>
    {
        public AtoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Ato);

            // Properties
            this.Property(t => t.TipoPagamento)
                .HasMaxLength(50);

            this.Property(t => t.Usuario)
                .HasMaxLength(100);

            this.Property(t => t.Atribuicao)
                .HasMaxLength(15);

            this.Property(t => t.LetraSelo)
                .HasMaxLength(4);

            this.Property(t => t.Mensalista)
                .HasMaxLength(50);

            this.Property(t => t.Faixa)
                .HasMaxLength(100);

            this.Property(t => t.Portador)
                .HasMaxLength(100);

            this.Property(t => t.Livro)
                .HasMaxLength(10);

            this.Property(t => t.TipoAto)
                .HasMaxLength(40);

            this.Property(t => t.Natureza)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.Escrevente)
                .HasMaxLength(100);

            this.Property(t => t.Convenio)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.TipoCobranca)
                .HasMaxLength(20);

            this.Property(t => t.TipoPrenotacao)
                .HasMaxLength(20);

            this.Property(t => t.Aleatorio)
                .HasMaxLength(3);

            this.Property(t => t.DescricaoAto)
                .HasMaxLength(150);

            this.Property(t => t.FichaAto)
                .HasMaxLength(15);

            // Table & Column Mappings
            this.ToTable("Ato");
            this.Property(t => t.Id_Ato).HasColumnName("Id_Ato");
            this.Property(t => t.DataPagamento).HasColumnName("DataPagamento");
            this.Property(t => t.TipoPagamento).HasColumnName("TipoPagamento");
            this.Property(t => t.DataAto).HasColumnName("DataAto");
            this.Property(t => t.Pago).HasColumnName("Pago");
            this.Property(t => t.IdUsuario).HasColumnName("IdUsuario");
            this.Property(t => t.Usuario).HasColumnName("Usuario");
            this.Property(t => t.Atribuicao).HasColumnName("Atribuicao");
            this.Property(t => t.LetraSelo).HasColumnName("LetraSelo");
            this.Property(t => t.NumeroSelo).HasColumnName("NumeroSelo");
            this.Property(t => t.ValorEscrevente).HasColumnName("ValorEscrevente");
            this.Property(t => t.ValorAdicionar).HasColumnName("ValorAdicionar");
            this.Property(t => t.ValorDesconto).HasColumnName("ValorDesconto");
            this.Property(t => t.Mensalista).HasColumnName("Mensalista");
            this.Property(t => t.ValorCorretor).HasColumnName("ValorCorretor");
            this.Property(t => t.Faixa).HasColumnName("Faixa");
            this.Property(t => t.Portador).HasColumnName("Portador");
            this.Property(t => t.ValorTitulo).HasColumnName("ValorTitulo");
            this.Property(t => t.Livro).HasColumnName("Livro");
            this.Property(t => t.FolhaInical).HasColumnName("FolhaInical");
            this.Property(t => t.FolhaFinal).HasColumnName("FolhaFinal");
            this.Property(t => t.NumeroAto).HasColumnName("NumeroAto");
            this.Property(t => t.Protocolo).HasColumnName("Protocolo");
            this.Property(t => t.Recibo).HasColumnName("Recibo");
            this.Property(t => t.IdReciboBalcao).HasColumnName("IdReciboBalcao");
            this.Property(t => t.ReciboBalcao).HasColumnName("ReciboBalcao");
            this.Property(t => t.TipoAto).HasColumnName("TipoAto");
            this.Property(t => t.Natureza).HasColumnName("Natureza");
            this.Property(t => t.Escrevente).HasColumnName("Escrevente");
            this.Property(t => t.Convenio).HasColumnName("Convenio");
            this.Property(t => t.TipoCobranca).HasColumnName("TipoCobranca");
            this.Property(t => t.Emolumentos).HasColumnName("Emolumentos");
            this.Property(t => t.Fetj).HasColumnName("Fetj");
            this.Property(t => t.Fundperj).HasColumnName("Fundperj");
            this.Property(t => t.Funperj).HasColumnName("Funperj");
            this.Property(t => t.Funarpen).HasColumnName("Funarpen");
            this.Property(t => t.Pmcmv).HasColumnName("Pmcmv");
            this.Property(t => t.Iss).HasColumnName("Iss");
            this.Property(t => t.Mutua).HasColumnName("Mutua");
            this.Property(t => t.Acoterj).HasColumnName("Acoterj");
            this.Property(t => t.Distribuicao).HasColumnName("Distribuicao");
            this.Property(t => t.Indisponibilidade).HasColumnName("Indisponibilidade");
            this.Property(t => t.TipoPrenotacao).HasColumnName("TipoPrenotacao");
            this.Property(t => t.Prenotacao).HasColumnName("Prenotacao");
            this.Property(t => t.QuantIndisp).HasColumnName("QuantIndisp");
            this.Property(t => t.QuantPrenotacao).HasColumnName("QuantPrenotacao");
            this.Property(t => t.QuantDistrib).HasColumnName("QuantDistrib");
            this.Property(t => t.QuantCopia).HasColumnName("QuantCopia");
            this.Property(t => t.NumeroRequisicao).HasColumnName("NumeroRequisicao");
            this.Property(t => t.QtdAtos).HasColumnName("QtdAtos");
            this.Property(t => t.ValorPago).HasColumnName("ValorPago");
            this.Property(t => t.ValorTroco).HasColumnName("ValorTroco");
            this.Property(t => t.Bancaria).HasColumnName("Bancaria");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.Aleatorio).HasColumnName("Aleatorio");
            this.Property(t => t.DescricaoAto).HasColumnName("DescricaoAto");
            this.Property(t => t.FichaAto).HasColumnName("FichaAto");
            this.Property(t => t.Checked).HasColumnName("Checked");
        }
    }
}
