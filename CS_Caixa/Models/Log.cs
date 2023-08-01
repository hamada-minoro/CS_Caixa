using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Log
    {
        public int IdLog { get; set; }
        public System.DateTime Data { get; set; }
        public string Usuario { get; set; }
        public string Acao { get; set; }
        public string Descricao { get; set; }
        public string Tela { get; set; }
    }
}
