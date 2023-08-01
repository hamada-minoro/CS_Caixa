using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class IndiceEscritura
    {
        public int IdIndiceEscritura { get; set; }
        public string Outorgante { get; set; }
        public string Outorgado { get; set; }
        public string DiaDist { get; set; }
        public string MesDist { get; set; }
        public string AnoDist { get; set; }
        public string Natureza { get; set; }
        public string Dia { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public string Ato { get; set; }
        public string Livro { get; set; }
        public string Fls { get; set; }
        public string Ordem { get; set; }
    }
}
