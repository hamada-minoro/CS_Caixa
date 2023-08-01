using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Parte
    {
        public int ParteId { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Outorgado { get; set; }
        public string CpfOutorgado { get; set; }
    }
}
