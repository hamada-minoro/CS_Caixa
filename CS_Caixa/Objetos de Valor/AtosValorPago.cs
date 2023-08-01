using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class AtosValorPago
    {
        public int IdPagamento { get; set; }
        public int IdAto { get; set; }
        public int IdReciboBalcao { get; set; }
        public bool Pago { get; set; }
        public string Atribuicao { get; set; }
        public string TipoCobranca { get; set; }
        public string TipoPagamento { get; set; }
        public int Recibo { get; set; }
        public int Protocolo { get; set; }
        public string Funcionario { get; set; }
        public decimal Total { get; set; }     
    }
}
