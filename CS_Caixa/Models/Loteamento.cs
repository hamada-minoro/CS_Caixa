using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Loteamento
    {
        public int LoteamentoId { get; set; }
        public string Nome { get; set; }
        public string Proprietario { get; set; }
        public string Localizacao { get; set; }
        public Nullable<int> Matricula { get; set; }
        public string NumeroInscricao { get; set; }
    }
}
