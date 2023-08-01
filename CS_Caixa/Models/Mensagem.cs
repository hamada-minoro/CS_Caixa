using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Mensagem
    {
        public int Mensagem_Id { get; set; }
        public string Texto { get; set; }
        public int Grau_Importacia { get; set; }
        public bool Pisca { get; set; }
        public int Cor { get; set; }
    }
}
