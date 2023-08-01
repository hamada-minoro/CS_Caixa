using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Divida
    {
        public int Id_Divida { get; set; }
        public int Id_Usuario { get; set; }
        public System.DateTime Data { get; set; }
        public Nullable<int> Dia_Pagamento { get; set; }
        public Nullable<System.DateTime> Data_Inicio_Parcela { get; set; }
        public Nullable<System.DateTime> Data_Fim_Parcela { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Tipo_Divida { get; set; }
        public Nullable<int> Qtd_Parcelas { get; set; }
        public Nullable<bool> Divida_Paga { get; set; }
        public Nullable<int> Qtd_Parcelas_Pagas { get; set; }
        public Nullable<decimal> Valor_Pago { get; set; }
        public Nullable<decimal> Valor_Divida { get; set; }
    }
}
