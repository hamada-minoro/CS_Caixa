using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class AgendaEletronica
    {
        public int IdAgenda { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Hora { get; set; }
        public string Usuario { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public byte[] CorBotao { get; set; }
        public string TipoAto { get; set; }
        public string NomeCliente { get; set; }
        public string Observacao { get; set; }
    }
}
