using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassTabelaCustas
    {
        CS_CAIXA_DBContext Contexto { get; set; }

        public ClassTabelaCustas()
        {
            Contexto = new CS_CAIXA_DBContext();
        }


        public List<CustasNota> ListarCustasNotas(int ano)
        {
            return Contexto.CustasNotas.Where(p => p.ANO == ano).ToList();
        }

        public void SalvarCustasNotas(CustasNota custaSalvar)
        {
            Contexto.CustasNotas.Add(custaSalvar);
            Contexto.SaveChanges();
        }

        public void ExcluirCustasNotas(CustasNota custaExcluir)
        {
            Contexto.CustasNotas.Remove(custaExcluir);
            Contexto.SaveChanges();
        }


        public List<CustasProtesto> ListarCustasProtesto(int ano)
        {
            return Contexto.CustasProtestoes.Where(p => p.ANO == ano).ToList();
        }

        public void SalvarCustasProtesto(CustasProtesto custaSalvar)
        {
            Contexto.CustasProtestoes.Add(custaSalvar);
            Contexto.SaveChanges();
        }

        public void ExcluirCustasProtesto(CustasProtesto custaExcluir)
        {
            Contexto.CustasProtestoes.Remove(custaExcluir);
            Contexto.SaveChanges();
        }

        public List<CustasRgi> ListarCustasRgi(int ano)
        {
           return  Contexto.CustasRgis.Where(p => p.ANO == ano).ToList();
        }

        public void SalvarCustasRgi(CustasRgi custaSalvar)
        {
            Contexto.CustasRgis.Add(custaSalvar);
            Contexto.SaveChanges();
        }

        public void ExcluirCustasRgi(CustasRgi custaExcluir)
        {
            Contexto.CustasRgis.Remove(custaExcluir);
            Contexto.SaveChanges();
        }


        public List<CustasDistribuicao> ListarCustasDistribuicao(int ano)
        {
            return Contexto.CustasDistribuicaos.Where(p => p.Ano == ano).ToList();
        }

        public void SalvarCustasDistribuicao(CustasDistribuicao custaSalvar)
        {
            Contexto.CustasDistribuicaos.Add(custaSalvar);
            Contexto.SaveChanges();
        }

        public void ExcluirCustasDistribuicao(CustasDistribuicao custaExcluir)
        {
            Contexto.CustasDistribuicaos.Remove(custaExcluir);
            Contexto.SaveChanges();
        }

       
        
    }
}
