using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ReciboNota
    {
        public int ReciboNotasId { get; set; }
        public Nullable<int> AtoId { get; set; }
        public int Recibo { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string Atribuicao { get; set; }
        public Nullable<System.DateTime> DataEntrega { get; set; }
        public Nullable<int> ApresentanteId { get; set; }
        public string Status { get; set; }
    }
}
