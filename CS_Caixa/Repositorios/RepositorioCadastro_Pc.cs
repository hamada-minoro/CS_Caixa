using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioCadastro_Pc : RepositorioBase<Cadastro_Pc>
    {

        public Cadastro_Pc ObterPorIdentificadorPc(string id)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Cadastro_Pc.Where(p => p.Identificador_Pc == id).FirstOrDefault();
        }

        public Cadastro_Pc ObterPorNomePc(string nome)
        {
            Db = new CS_CAIXA_DBContext();

            return Db.Cadastro_Pc.Where(p => p.Nome_Pc == nome).FirstOrDefault();
        }

        public string ObterIdentificadorPc()
        {
            return IdentificacaoDigital.Valor();
        }

        public string ObterIpMaquina()
        {
            
            IPAddress[] ip = Dns.GetHostAddresses(Dns.GetHostName());

            string ipResult = string.Empty;

            foreach (var item in ip)
            {
                if (item.AddressFamily == AddressFamily.InterNetwork)
                    ipResult = item.ToString();
            }

            return ipResult;

        }

        public string ObterNomeMaquina()
        {
            return System.Net.Dns.GetHostName();
        }
    }
}
