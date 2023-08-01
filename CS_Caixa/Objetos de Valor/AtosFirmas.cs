using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class AtosFirmas
    {
        public int ID_ATO { get; set; }

        public int ANO { get; set; }

        public string DATA { get; set; }

        public string HORA { get; set; }

        public int RECIBO { get; set; }

        public int CODIGO { get; set; }

        public string TIPO { get; set; }

        public string DESCRICAO { get; set; }

        public string SELO { get; set; }

        public string ALEATORIO { get; set; }

        public int ID_FICHA { get; set; }

        public int FICHA { get; set; }

        public string LIVRO { get; set; }

        public string FOLHA { get; set; }

        public string TERMO { get; set; }

        public string COBRANCA { get; set; }

        public decimal EMOLUMENTOS { get; set; }

        public decimal FETJ { get; set; }

        public decimal FUNDPERJ { get; set; }

        public decimal FUNPERJ { get; set; }

        public decimal FUNARPEN { get; set; }

        public decimal PMCMV { get; set; }

        public decimal ISS { get; set; }

        public decimal TOTAL { get; set; }

        public string LOGADO { get; set; }

        public string STATUS { get; set; }

        public int TIPO_DOCUMENTO { get; set; }

    }
}
