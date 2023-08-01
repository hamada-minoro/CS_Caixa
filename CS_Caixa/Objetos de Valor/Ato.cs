using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public abstract class Ato
    {
        public int Id { get; set; }

        public int IdAto { get; set; }

        public string Documento { get; set; }

        public DateTime DataAto { get; set; }

        public string Tipo { get; set; }

        public string Atribuicao { get; set; }

        public string Descricao { get; set; }

        public string Status { get; set; }

        public string Selo { get; set; }

        public string Aleatorio { get; set; }

        public string TipoCobranca { get; set; }

        public decimal Total { get; set; }

    }
}
