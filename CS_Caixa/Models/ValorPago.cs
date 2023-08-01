using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ValorPago
    {
        public int ValorPagoId { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public int IdAto { get; set; }
        public int IdReciboBalcao { get; set; }
        public Nullable<decimal> Dinheiro { get; set; }
        public Nullable<decimal> Deposito { get; set; }
        public Nullable<decimal> Cheque { get; set; }
        public Nullable<decimal> ChequePre { get; set; }
        public Nullable<decimal> Boleto { get; set; }
        public Nullable<decimal> Mensalista { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> Troco { get; set; }
        public Nullable<int> IdPagamento { get; set; }
        public Nullable<decimal> CartaoCredito { get; set; }
        public string DataModificado { get; set; }
        public string HoraModificado { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public string NomeUsuario { get; set; }
    }
}
