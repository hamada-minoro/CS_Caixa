using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Models
{
    public class AtosValores
    {
        public DateTime DataPagamento { get; set; }
        public string TipoPagamento { get; set; }
        public DateTime DataAto { get; set; }
        public bool Pago { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Atribuicao { get; set; }
        public string LetraSelo { get; set; }
        public Nullable<int> NumeroSelo { get; set; }
        public Nullable<decimal> ValorEscrevente { get; set; }
        public Nullable<decimal> ValorAdicionar { get; set; }
        public Nullable<decimal> ValorDesconto { get; set; }
        public string Mensalista { get; set; }
        public Nullable<decimal> ValorCorretor { get; set; }
        public string Faixa { get; set; }
        public string Portador { get; set; }
        public Nullable<decimal> ValorTitulo { get; set; }
        public string Livro { get; set; }
        public Nullable<int> FolhaInical { get; set; }
        public Nullable<int> FolhaFinal { get; set; }
        public Nullable<int> NumeroAto { get; set; }
        public string AtoNotas { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public Nullable<int> Recibo { get; set; }
        public Nullable<int> IdReciboBalcao { get; set; }
        public Nullable<int> ReciboBalcao { get; set; }
        public string TipoAto { get; set; }
        public string Natureza { get; set; }
        public string Escrevente { get; set; }
        public string Convenio { get; set; }
        public string TipoCobranca { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Distribuicao { get; set; }
        public Nullable<decimal> Indisponibilidade { get; set; }
        public string TipoPrenotacao { get; set; }
        public Nullable<decimal> Prenotacao { get; set; }
        public Nullable<int> QuantIndisp { get; set; }
        public Nullable<int> QuantPrenotacao { get; set; }
        public Nullable<int> QuantDistrib { get; set; }
        public Nullable<int> QuantCopia { get; set; }
        public Nullable<int> NumeroRequisicao { get; set; }
        public Nullable<int> QtdAtos { get; set; }
        public Nullable<int> QuantAut { get; set; }
        public Nullable<int> QuantAbert { get; set; }
        public Nullable<int> QuantRecAut { get; set; }
        public Nullable<int> QuantRecSem { get; set; }
        public Nullable<int> QuantMaterializacao { get; set; }
        public Nullable<decimal> ValorPago { get; set; }
        public Nullable<decimal> ValorTroco { get; set; }
        public Nullable<decimal> Bancaria { get; set; }
        public Nullable<decimal> Dinheiro { get; set; }
        public Nullable<decimal> Deposito { get; set; }
        public Nullable<decimal> Cheque { get; set; }
        public Nullable<decimal> ChequePre { get; set; }
        public Nullable<decimal> Boleto { get; set; }
        public Nullable<decimal> CartaoCredito { get; set; }
        public Nullable<decimal> VrMensalista { get; set; }
        public Nullable<decimal> Total { get; set; }
    }
}
