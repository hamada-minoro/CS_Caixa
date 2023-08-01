using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Objetos_de_Valor;

namespace CS_Caixa.Agragador
{
    public class AtoPrincipalQrCode
    {
        public int AtoId { get; set; }

        public DateTime Data { get; set; }

        public string Livro { get; set; }

        public string FolhasInicio { get; set; }

        public string FolhasFim { get; set; }

        public int Ato { get; set; }

        public string Selo { get; set; }

        public string Aleatorio { get; set; }

        public string Tipo { get; set; }

        public int Serventia { get; set; }

        public string Cerp { get; set; }

        public string Obs { get; set; }

        public string Protocolo { get; set; }

        public string Natureza { get; set; }

        public string Matricula { get; set; }

        public decimal Emol { get; set; }

        public decimal Fetj { get; set; }

        public decimal Fund { get; set; }

        public decimal Funp { get; set; }

        public decimal Funarpen { get; set; }

        public decimal Pmcmv { get; set; }

        public decimal Iss { get; set; }

        public decimal Total { get; set; }

        public List<AtoConjuntoQrCode> AtosConjuntos { get; set; }

       


    }
}
