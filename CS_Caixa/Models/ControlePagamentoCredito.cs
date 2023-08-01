using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ControlePagamentoCredito
    {
        public int Id { get; set; }
        public System.DateTime Data { get; set; }
        public string Descricao { get; set; }
        public string TipoCredito { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Origem { get; set; }
        public Nullable<bool> Importado { get; set; }
        public decimal Valor { get; set; }
    }
}
