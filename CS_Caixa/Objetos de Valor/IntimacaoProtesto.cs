using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class IntimacaoProtesto : Ato
    {

        public DateTime DataEntrada { get; set; }

        public int Protocolo { get; set; }

        public DateTime DataPrazo { get; set; }

        public DateTime DataIntimacao { get; set; }

        public string Devedor { get; set; }

        public string CpfCnpjDevedor { get; set; }

        //--------Pegar dado na tabela DEVEDORES da Total
        public string Endereco { get; set; }

        public string Bairro { get; set; }

        public string Municipio { get; set; }

        public string UF { get; set; }

        public string CEP { get; set; }

        //------------------------------

        public string TipoPessoa { get; set; }

        //--------Pegar dado na tabela TIPOS da Total
        public string TipoTitulo { get; set; }
        //------------------------------------------------

        public string NumeroTitulo { get; set; }

        public string Portador { get; set; }

        public string Cedente { get; set; }

        public string Sacador { get; set; }

        public string Praca { get; set; }

        public string FinsFalimentares { get; set; }

        public DateTime DataTitulo { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal ValorTitulo { get; set; }

        public decimal Custas { get; set; }

        public decimal Distribuicao { get; set; }

        public decimal ValorChequeAdm { get; set; }

        public decimal Tarifa { get; set; }

        public decimal ValoBoleto { get; set; }

        public bool Visualizado { get; set; }
    }
}