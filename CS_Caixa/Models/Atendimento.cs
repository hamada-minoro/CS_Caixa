using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Atendimento
    {
        public int AtendimentoId { get; set; }
        public Nullable<int> Fila { get; set; }
        public string Senha { get; set; }
        public string TipoAtendimento { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string HoraRetirada { get; set; }
        public string Status { get; set; }
        public string HoraAtendimento { get; set; }
        public Nullable<int> IdUsuario { get; set; }
        public string NomeAtendente { get; set; }
        public string HoraFinalizado { get; set; }
        public string OrdemChamada { get; set; }
    }
}
