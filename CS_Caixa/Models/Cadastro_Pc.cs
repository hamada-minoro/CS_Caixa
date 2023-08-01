using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Cadastro_Pc
    {
        public int Cadastro_Pc_Id { get; set; }
        public Nullable<System.DateTime> Data_Cadastro { get; set; }
        public string Identificador_Pc { get; set; }
        public string Nome_Pc { get; set; }
        public string Caracter { get; set; }
        public string Tipo_Atendimento { get; set; }
        public string FalaOutros { get; set; }
        public int Tipo_Entrada { get; set; }
        public string Ip_Pc { get; set; }
        public int Porta_Pc { get; set; }
        public int SetorId { get; set; }
        public int TipoChamadaSenha { get; set; }
    }
}
