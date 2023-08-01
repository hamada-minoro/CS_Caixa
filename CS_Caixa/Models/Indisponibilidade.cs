using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Indisponibilidade
    {
        public int IdIndisponibilidade { get; set; }
        public string Titulo { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Oficio { get; set; }
        public string Aviso { get; set; }
        public string Processo { get; set; }
        public string Valor { get; set; }
    }
}
