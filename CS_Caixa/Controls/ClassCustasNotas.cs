using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassCustasNotas
    {
        public CS_CAIXA_DBContext Contexto { get; set; }

        public ClassCustasNotas()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CustasNota> ListaCustas()
        {
            try
            {
                return Contexto.CustasNotas.AsNoTracking().Select(p => p).OrderBy(p => p.ORDEM).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }

        public List<CustasDistribuicao> ListaCustasDistribuicao()
        {
            try
            {
                return Contexto.CustasDistribuicaos.Select(p => p).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }


        public decimal ValorCopia(int ano)
        {
            try
            {

                decimal valor = Convert.ToDecimal(Contexto.CustasNotas.Where(p => p.DESCR == "Cópia" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());

                return valor;
            }
            catch (Exception ex)
            { 
                return 0;
                throw new Exception(ex.Message);
            }
        }

        public decimal ValorDutRegistro(int ano)
        {
            try
            {

                decimal dut = Convert.ToDecimal(Contexto.CustasNotas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE (DUT)" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());
               
                return dut;
            }
            catch (Exception ex)
            {
                return 0;
                throw new Exception(ex.Message);
            }
        }



        public void SalvarItensLista(ItensCustasNota item)
        {
            ItensCustasNota novoItem = new ItensCustasNota();

            novoItem.Id_Ato = item.Id_Ato;

            novoItem.Tabela = item.Tabela;

            novoItem.Item = item.Item;

            novoItem.SubItem = item.SubItem;

            novoItem.Quantidade = item.Quantidade;

            novoItem.Valor = item.Valor;

            novoItem.Total = item.Total;

            novoItem.Descricao = item.Descricao;

            novoItem.Complemento = item.Complemento;

            Contexto.ItensCustasNotas.Add(novoItem);

            Contexto.SaveChanges();
        }

        public void SalvarItensListaControleAtos(ItensCustasControleAtosNota item)
        {
            var novoItem = new ItensCustasControleAtosNota();

            novoItem.Id_ControleAto = item.Id_ControleAto;

            novoItem.Tabela = item.Tabela;

            novoItem.Item = item.Item;

            novoItem.SubItem = item.SubItem;

            novoItem.Quantidade = item.Quantidade;

            novoItem.Valor = item.Valor;

            novoItem.Total = item.Total;

            novoItem.Descricao = item.Descricao;

            novoItem.Complemento = item.Complemento;

            Contexto.ItensCustasControleAtosNotas.Add(novoItem);

            Contexto.SaveChanges();
        }

        public List<ItensCustasNota> ListarItensCustas(int id)
        {
            return Contexto.ItensCustasNotas.Where(p => p.Id_Ato == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public List<ItensCustasControleAtosNota> ListarItensControleAtosCustas(int id)
        {
            return Contexto.ItensCustasControleAtosNotas.Where(p => p.Id_ControleAto == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public void RemoverItensCustas(int id)
        {
            List<ItensCustasNota> itens = new List<ItensCustasNota>();

            var ItensExcluir = Contexto.ItensCustasNotas.Where(p => p.Id_Ato == id).ToList();

            itens = ItensExcluir.ToList();

            for (int cont = 0; cont <= itens.Count - 1; cont++)
            {
                Contexto.ItensCustasNotas.Remove(itens[cont]);
                Contexto.SaveChanges();
            }
        }

        public void RemoverItensCustasControleAtos(int id)
        {
            List<ItensCustasControleAtosNota> itens = new List<ItensCustasControleAtosNota>();

            var ItensExcluir = Contexto.ItensCustasControleAtosNotas.Where(p => p.Id_ControleAto == id).ToList();

            itens = ItensExcluir.ToList();

            for (int cont = 0; cont <= itens.Count - 1; cont++)
            {
                Contexto.ItensCustasControleAtosNotas.Remove(itens[cont]);
                Contexto.SaveChanges();
            }
        }

        public decimal CalcularISSNotas(decimal emol, bool formula, decimal alicotaIss)
        {

            decimal iss = 0M;

            if (formula == true)
            {
                iss = emol / ((100M - alicotaIss) / 100M);
                iss = iss - emol;
            }
            else
            {
                iss = (emol * alicotaIss) / 100M;
            }

            return iss;
        }

    }
}
