using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassPortador
    {
        public CS_CAIXA_DBContext contexto { get; set; }

        public ClassPortador()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<Portador> ListaPortador()
        {
            return contexto.Portadors.OrderBy(p => p.NOME).ToList();
        }

        public List<Portador> VerificaExiste(string NomePortador)
        {
            return contexto.Portadors.Where(p => p.NOME == NomePortador).ToList();
        }


        public void SalvarPortador(Portador portadorSalvar)
        {
            Portador portador = new Portador();

            portador.CODIGO = portadorSalvar.CODIGO;

            portador.NOME = portadorSalvar.NOME;

            portador.TIPO = portadorSalvar.TIPO;

            portador.DOCUMENTO = portadorSalvar.DOCUMENTO;

            portador.ENDERECO = portadorSalvar.ENDERECO;

            portador.BANCO = portadorSalvar.BANCO;

            portador.CONVENIO = portadorSalvar.CONVENIO;

            contexto.Portadors.Add(portador);

            contexto.SaveChanges();
        }
    }
}
