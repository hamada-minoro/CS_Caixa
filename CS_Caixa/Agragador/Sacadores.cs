using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Caixa.Agragador
{
    public class Sacadores
    {
        public int ID_ATO { get; set; }
        public int ID_SACADOR { get; set; }
        public string NOME { get; set; }
        public string TIPO { get; set; }
        public string DOCUMENTO { get; set; }
        public string ENDERECO { get; set; }
        public string BAIRRO { get; set; }
        public string CEP { get; set; }
        public string MUNICIPIO { get; set; }
        public string UF { get; set; }
        public int ORDEM { get; set; }

    }
}
