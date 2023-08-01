using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class AtualizaSite
    {
        public int AtualizaSiteId { get; set; }
        public string DataAtualizacao { get; set; }
        public string HoraAtualizacao { get; set; }
        public string Status { get; set; }
        public string PcAtualizacao { get; set; }
    }
}
