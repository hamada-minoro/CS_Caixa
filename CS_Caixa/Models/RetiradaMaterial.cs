using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class RetiradaMaterial
    {
        public int IdRetiradaMaterial { get; set; }
        public System.DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public string Material { get; set; }
        public string Funcionario { get; set; }
    }
}
