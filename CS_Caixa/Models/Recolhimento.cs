using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Recolhimento
    {
        public int RecolhimentoId { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public string TipoAto { get; set; }
        public string Natureza { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public string Matricula { get; set; }
        public string Livro { get; set; }
        public string Folha { get; set; }
        public string Ato { get; set; }
        public string Selo { get; set; }
        public Nullable<bool> Gratuito { get; set; }
        public Nullable<decimal> Emol { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fund { get; set; }
        public Nullable<decimal> Funp { get; set; }
        public Nullable<decimal> Funa { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public string Atribuicao { get; set; }
        public Nullable<bool> Convenio { get; set; }
        public Nullable<int> Excedente { get; set; }
    }
}
