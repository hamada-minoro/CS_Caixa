using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ReciboBalcao
    {
        public int IdReciboBalcao { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public int NumeroRecibo { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Status { get; set; }
        public bool Pago { get; set; }
        public string TipoCustas { get; set; }
        public string TipoPagamento { get; set; }
        public Nullable<int> QuantAut { get; set; }
        public Nullable<int> QuantAbert { get; set; }
        public Nullable<int> QuantRecAut { get; set; }
        public Nullable<int> QuantRecSem { get; set; }
        public Nullable<int> QuantCopia { get; set; }
        public Nullable<decimal> ValorAdicionar { get; set; }
        public Nullable<decimal> ValorDesconto { get; set; }
        public string Mensalista { get; set; }
        public Nullable<int> NumeroRequisicao { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Total { get; set; }
        public Nullable<decimal> ValorPago { get; set; }
        public Nullable<decimal> ValorTroco { get; set; }
        public Nullable<int> QuantMaterializacao { get; set; }
        public Nullable<int> IdAtendimento { get; set; }
    }
}
