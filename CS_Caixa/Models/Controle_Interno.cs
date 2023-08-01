using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Controle_Interno
    {
        public int ControleInternoId { get; set; }
        public System.DateTime Data { get; set; }
        public string Tipo { get; set; }
        public string Descricao { get; set; }
        public string EntradaSaida { get; set; }
        public decimal Valor { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
    }
}
