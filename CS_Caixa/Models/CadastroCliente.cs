using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class CadastroCliente
    {
        public int CadastroClienteId { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public System.DateTime DataNascimento { get; set; }
        public string DataUltimaAtualizacao { get; set; }
        public string Nome { get; set; }
        public string CPF_CNPJ { get; set; }
        public string TipoPessoa { get; set; }
        public string Sexo { get; set; }
        public string RG { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
    }
}
