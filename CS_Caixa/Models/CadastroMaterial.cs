using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CadastroMaterial
    {
        public int IdDescricao { get; set; }
        public string DescMaterial { get; set; }
        public int Quantidade { get; set; }
        public string Codigo { get; set; }
    }
}
