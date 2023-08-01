using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class VerificacaoCaixa
    {
        public int VerificacaoCaixaId { get; set; }
        public string Status { get; set; }
        public string Valor { get; set; }
    }
}
