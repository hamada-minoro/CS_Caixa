using System;
using System.Collections.Generic;

namespace CS_Models.Models
{
    public partial class Ato
    {
        public Ato()
        {
            this.Ato1 = new List<Ato>();
        }

        public int Id_Ato { get; set; }
        public System.DateTime DataPagamento { get; set; }
        public System.DateTime DataAto { get; set; }
        public string Status { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Atribuicao { get; set; }
        public string Livro { get; set; }
        public Nullable<int> FolhaInical { get; set; }
        public Nullable<int> FolhaFinal { get; set; }
        public Nullable<int> NumeroAto { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public Nullable<int> Recibo { get; set; }
        public string Natureza { get; set; }
        public string Escrevente { get; set; }
        public string Convenio { get; set; }
        public string TipoCobranca { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Distribuicao { get; set; }
        public Nullable<decimal> Indisponibilidade { get; set; }
        public Nullable<decimal> Prenotacao { get; set; }
        public Nullable<decimal> Ar { get; set; }
        public Nullable<decimal> Bancaria { get; set; }
        public Nullable<decimal> Total { get; set; }
        public virtual ICollection<Ato> Ato1 { get; set; }
        public virtual Ato Ato2 { get; set; }
    }
}
