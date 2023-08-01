using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Caixa.Agragador
{
    public class Devedores
    {

        public int ID_ATO { get; set; }
        public int ID_DEVEDOR { get; set; }
        public int ORDEM { get; set; }
        public string NOME { get; set; }
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public string IFP_DETRAN { get; set; }
        public string IDENTIDADE { get; set; }
        public DateTime DT_EMISSAO { get; set; }
        public string ORGAO { get; set; }
        public string CEP { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string MUNICIPIO { get; set; }
        public string UF { get; set; }
        public string IGNORADO { get; set; }
        public string JUSTIFICATIVA { get; set; }
        public string TELEFONE { get; set; }
        public string MOTIVO_INTIMACAO { get; set; }
        public string INTIMADO_PESSOAL { get; set; }
        public DateTime DATA_PESSOAL { get; set; }
        public string INTIMADO_EDITAL { get; set; }
        public DateTime DATA_EDITAL { get; set; }
        public DateTime DATA_PUBLICACAO { get; set; }
        public string INTIMADO_CARTA { get; set; }
        public DateTime DATA_CARTA { get; set; }

    }
}
