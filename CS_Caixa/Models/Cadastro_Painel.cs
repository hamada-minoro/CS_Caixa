using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Cadastro_Painel
    {
        public int Cadastro_Painel_Id { get; set; }
        public Nullable<System.DateTime> Data_Cadastro { get; set; }
        public string Identificador_Pc { get; set; }
        public string Nome_Pc { get; set; }
        public string Ip_Pc { get; set; }
        public int Porta_Pc { get; set; }
    }
}
