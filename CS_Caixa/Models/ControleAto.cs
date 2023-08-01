using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ControleAto
    {
        public ControleAto()
        {
            this.ItensControleAtoNotas = new List<ItensControleAtoNota>();
            this.ItensControleAtoProtestoes = new List<ItensControleAtoProtesto>();
            this.ItensCustasControleAtosNotas = new List<ItensCustasControleAtosNota>();
            this.ItensCustasControleAtosProtestoes = new List<ItensCustasControleAtosProtesto>();
        }

        public int Id_ControleAtos { get; set; }
        public System.DateTime DataAto { get; set; }
        public Nullable<int> AtoNaoGratuito { get; set; }
        public Nullable<int> AtoGratuito { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Atribuicao { get; set; }
        public string LetraSelo { get; set; }
        public Nullable<int> NumeroSelo { get; set; }
        public string Faixa { get; set; }
        public string Matricula { get; set; }
        public string Livro { get; set; }
        public Nullable<int> FolhaInical { get; set; }
        public Nullable<int> FolhaFinal { get; set; }
        public Nullable<int> NumeroAto { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public Nullable<int> Recibo { get; set; }
        public Nullable<int> Id_Ato { get; set; }
        public Nullable<int> ReciboBalcao { get; set; }
        public string TipoAto { get; set; }
        public string Natureza { get; set; }
        public string Convenio { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<int> QtdAtos { get; set; }
        public Nullable<decimal> Total { get; set; }
        public virtual ICollection<ItensControleAtoNota> ItensControleAtoNotas { get; set; }
        public virtual ICollection<ItensControleAtoProtesto> ItensControleAtoProtestoes { get; set; }
        public virtual ICollection<ItensCustasControleAtosNota> ItensCustasControleAtosNotas { get; set; }
        public virtual ICollection<ItensCustasControleAtosProtesto> ItensCustasControleAtosProtestoes { get; set; }
    }
}
