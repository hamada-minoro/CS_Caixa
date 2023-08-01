using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class AtosPraticadosSite
    {

        public List<AtosFirmasSite> Firmas { get; set; }

        public List<ReciboRgiSite> ReciboRgi { get; set; }

        public List<AtosRgiSite> AtosRgi { get; set; }

        public List<AtosNotasSite> AtosNotas { get; set; }

        public List<AtosProtestoSite> AtosProtesto { get; set; }

    }
}
