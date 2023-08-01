using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Parcela
    {
        public int Id_Parcela { get; set; }
        public int Id_Divida { get; set; }
        public int Id_Usuario { get; set; }
        public System.DateTime Data_Emissao { get; set; }
        public System.DateTime Data_Vencimento { get; set; }
        public string Data_Pagamento { get; set; }
        public Nullable<bool> Pago { get; set; }
        public Nullable<decimal> Valor { get; set; }
    }
}
