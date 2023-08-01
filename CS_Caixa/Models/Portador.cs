using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Portador
    {
        public int ID_PORTADOR { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public string ENDERECO { get; set; }
        public string BANCO { get; set; }
        public string CONVENIO { get; set; }
        public string CONTA { get; set; }
        public string OBSERVACAO { get; set; }
        public string AGENCIA { get; set; }
        public string PRACA { get; set; }
        public string CRA { get; set; }
        public Nullable<int> SEQUENCIA { get; set; }
        public Nullable<decimal> VALOR_DOC { get; set; }
        public Nullable<decimal> VALOR_TED { get; set; }
        public string NOMINAL { get; set; }
        public string FORCA_LEI { get; set; }
    }
}
