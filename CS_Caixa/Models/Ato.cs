using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Ato
    {
        public Ato()
        {
            this.ItensAtoNotas = new List<ItensAtoNota>();
            this.ItensAtoRgis = new List<ItensAtoRgi>();
            this.ItensCustasNotas = new List<ItensCustasNota>();
            this.ItensCustasProtestoes = new List<ItensCustasProtesto>();
            this.ItensCustasRgis = new List<ItensCustasRgi>();
        }

        public int Id_Ato { get; set; }
        public System.DateTime DataPagamento { get; set; }
        public string TipoPagamento { get; set; }
        public System.DateTime DataAto { get; set; }
        public bool Pago { get; set; }
        public int IdUsuario { get; set; }
        public string Usuario { get; set; }
        public string Atribuicao { get; set; }
        public string LetraSelo { get; set; }
        public Nullable<int> NumeroSelo { get; set; }
        public Nullable<decimal> ValorEscrevente { get; set; }
        public Nullable<decimal> ValorAdicionar { get; set; }
        public Nullable<decimal> ValorDesconto { get; set; }
        public string Mensalista { get; set; }
        public Nullable<decimal> ValorCorretor { get; set; }
        public string Faixa { get; set; }
        public string Portador { get; set; }
        public Nullable<decimal> ValorTitulo { get; set; }
        public string Livro { get; set; }
        public Nullable<int> FolhaInical { get; set; }
        public Nullable<int> FolhaFinal { get; set; }
        public Nullable<int> NumeroAto { get; set; }
        public Nullable<int> Protocolo { get; set; }
        public Nullable<int> Recibo { get; set; }
        public Nullable<int> IdReciboBalcao { get; set; }
        public Nullable<int> ReciboBalcao { get; set; }
        public string TipoAto { get; set; }
        public string Natureza { get; set; }
        public string Escrevente { get; set; }
        public string Convenio { get; set; }
        public string TipoCobranca { get; set; }
        public Nullable<decimal> Emolumentos { get; set; }
        public Nullable<decimal> Fetj { get; set; }
        public Nullable<decimal> Fundperj { get; set; }
        public Nullable<decimal> Funperj { get; set; }
        public Nullable<decimal> Funarpen { get; set; }
        public Nullable<decimal> Pmcmv { get; set; }
        public Nullable<decimal> Iss { get; set; }
        public Nullable<decimal> Mutua { get; set; }
        public Nullable<decimal> Acoterj { get; set; }
        public Nullable<decimal> Distribuicao { get; set; }
        public Nullable<decimal> Indisponibilidade { get; set; }
        public string TipoPrenotacao { get; set; }
        public Nullable<decimal> Prenotacao { get; set; }
        public Nullable<int> QuantIndisp { get; set; }
        public Nullable<int> QuantPrenotacao { get; set; }
        public Nullable<int> QuantDistrib { get; set; }
        public Nullable<int> QuantCopia { get; set; }
        public Nullable<int> NumeroRequisicao { get; set; }
        public Nullable<int> QtdAtos { get; set; }
        public Nullable<decimal> ValorPago { get; set; }
        public Nullable<decimal> ValorTroco { get; set; }
        public Nullable<decimal> Bancaria { get; set; }
        public Nullable<decimal> Total { get; set; }
        public string Aleatorio { get; set; }
        public string DescricaoAto { get; set; }
        public string FichaAto { get; set; }
        public Nullable<bool> Checked { get; set; }
        public virtual ICollection<ItensAtoNota> ItensAtoNotas { get; set; }
        public virtual ICollection<ItensAtoRgi> ItensAtoRgis { get; set; }
        public virtual ICollection<ItensCustasNota> ItensCustasNotas { get; set; }
        public virtual ICollection<ItensCustasProtesto> ItensCustasProtestoes { get; set; }
        public virtual ICollection<ItensCustasRgi> ItensCustasRgis { get; set; }
    }
}
