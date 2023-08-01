using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CS_Caixa.Repositorios
{
    public class RepositorioMP2032
    {
        public int ConfiguraModeloImpressora()
        {
            return MP2032.ConfiguraModeloImpressora(7);
        }

        public int IniciaPorta(string porta)
        {
            return MP2032.IniciaPorta(porta);
        }

        public int Le_Status()
        {
            return MP2032.Le_Status();
        }

        public int FechaPorta()
        {
            return MP2032.FechaPorta();
        }


        public int FormataTX(string texto, int TipoLetra, int italico, int sublinhado, int expandido, int enfatizado)
        {
            return MP2032.FormataTX(texto, TipoLetra, italico, sublinhado, expandido, enfatizado);
        }

        public int AcionaGuilhotina(int parcial_full)
        {
            return MP2032.AcionaGuilhotina(parcial_full);
        }
    }
}
