using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CadastroCompraMaterial
    {
        public int IdCompraMaterial { get; set; }
        public System.DateTime Data { get; set; }
        public int Quant { get; set; }
        public string NotaFiscal { get; set; }
        public string Descricao { get; set; }
        public Nullable<float> ValorUni { get; set; }
        public Nullable<float> ValorTotal { get; set; }
        public string CadFunc { get; set; }
    }
}
