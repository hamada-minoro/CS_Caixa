using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassMensalista
    {
        public CS_CAIXA_DBContext Contexto { get; set; }

        public ClassMensalista()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CadMensalista> ListaMensalistas()
        {
            try
            {
                return Contexto.CadMensalistas.Select(p => p).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }

        public int SalvarMensalista(CadMensalista mensalista, string tipoSalvar)
        {
            try
            {
                CadMensalista salvarMensalista;

                if (tipoSalvar == "novo")
                    salvarMensalista = new CadMensalista();
                else
                    salvarMensalista = Contexto.CadMensalistas.Where(p => p.Codigo == mensalista.Codigo).FirstOrDefault();
                
                salvarMensalista.Cod = mensalista.Cod;
                salvarMensalista.Nome = mensalista.Nome;

                if (tipoSalvar == "novo")
                    Contexto.CadMensalistas.Add(salvarMensalista);
                
                Contexto.SaveChanges();

                return salvarMensalista.Codigo;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void ExcluirMensalista(CadMensalista mensalista)
        {
            Contexto.CadMensalistas.Remove(mensalista);
            Contexto.SaveChanges();
        }

        public CadMensalista ConsultarPorCod(string cod)
        {
            return Contexto.CadMensalistas.Where(p => p.Cod == cod).FirstOrDefault();
        }

        public List<CadMensalista> Consultar(string tipo, string texto)
        {
            Contexto = new CS_CAIXA_DBContext();
            List<CadMensalista> mensalistas = new List<CadMensalista>();

            if (tipo == "nome")
            {
                mensalistas = Contexto.CadMensalistas.Where(p => p.Nome.Contains(texto)).ToList();
            }
            if (tipo == "cod")
            {
                mensalistas = Contexto.CadMensalistas.Where(p => p.Cod.Contains(texto)).ToList();
            }

            return mensalistas;
        }
    }
}
