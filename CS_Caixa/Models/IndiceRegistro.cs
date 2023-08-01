using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class IndiceRegistro
    {
        public int IdIndiceRegistros { get; set; }
        public string Nome { get; set; }
        public string Livro { get; set; }
        public string Reg { get; set; }
        public string Numero { get; set; }
        public string Ordem { get; set; }
        public string Fls { get; set; }
        public string TipoPessoa { get; set; }
        public string CpfCnpj { get; set; }
        public string TipoAto { get; set; }
        public Nullable<System.DateTime> DataRegistro { get; set; }
        public Nullable<System.DateTime> DataVenda { get; set; }
        public Nullable<bool> Enviado { get; set; }
    }
}
