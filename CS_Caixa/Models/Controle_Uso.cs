using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Controle_Uso
    {
        public int ControleId { get; set; }
        public string Versao { get; set; }
        public string Demo { get; set; }
        public string CodigoAtivacao { get; set; }
        public string DataAtivacao { get; set; }
        public string DataValidadeInicio { get; set; }
        public string DataValidadeFim { get; set; }
        public string DataUltimaUtilizacao { get; set; }
        public int DiasUtilizado { get; set; }
        public string AtivacaoUso { get; set; }
    }
}
