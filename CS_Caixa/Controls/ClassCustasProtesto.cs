using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;


namespace CS_Caixa.Controls
{
    public class ClassCustasProtesto
    {
        public CS_CAIXA_DBContext Contexto {get; set;}


        public ClassCustasProtesto()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CustasProtesto> ListaCustas()
        {
            try
            {
                return Contexto.CustasProtestoes.AsNoTracking().Select(p => p).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }

        public void SalvarItensLista(ItensCustasProtesto item)
        {
            ItensCustasProtesto novoItem = new ItensCustasProtesto();

            novoItem.Id_Ato = item.Id_Ato;

            novoItem.Tabela = item.Tabela;

            novoItem.Item = item.Item;

            novoItem.SubItem = item.SubItem;

            novoItem.Quantidade = item.Quantidade;

            novoItem.Valor = item.Valor;

            novoItem.Total = item.Total;

            novoItem.Descricao = item.Descricao;

            Contexto.ItensCustasProtestoes.Add(novoItem);

            Contexto.SaveChanges();
        }

        public void SalvarItensListaControleAtosProtesto(ItensCustasControleAtosProtesto item)
        {
            ItensCustasControleAtosProtesto novoItem = new ItensCustasControleAtosProtesto();

            novoItem.Id_ControleAto = item.Id_ControleAto;


            novoItem.Tabela = item.Tabela;

            novoItem.Item = item.Item;

            novoItem.SubItem = item.SubItem;

            novoItem.Quantidade = item.Quantidade;

            novoItem.Valor = item.Valor;

            novoItem.Total = item.Total;

            novoItem.Descricao = item.Descricao;

            Contexto.ItensCustasControleAtosProtestoes.Add(novoItem);

            Contexto.SaveChanges();
        }

        public List<ItensCustasProtesto> ListarItensCustas(int id)
        {
            return Contexto.ItensCustasProtestoes.Where(p => p.Id_Ato == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public List<ItensCustasControleAtosProtesto> ListarItensCustasControleAtosProtesto(int id)
        {
            return Contexto.ItensCustasControleAtosProtestoes.Where(p => p.Id_ControleAto == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public void RemoverItensCustas(int id)
        {
            List<ItensCustasProtesto> itens = new List<ItensCustasProtesto>();

            var ItensExcluir = Contexto.ItensCustasProtestoes.Where(p => p.Id_Ato == id).ToList();

            itens = ItensExcluir.ToList();

            for (int cont = 0; cont <= itens.Count - 1; cont++)
            {
                Contexto.ItensCustasProtestoes.Remove(itens[cont]);
                Contexto.SaveChanges();
            }
        }

        public void RemoverItensCustasControleAtosProtesto(int id)
        {
            List<ItensCustasControleAtosProtesto> itens = new List<ItensCustasControleAtosProtesto>();

            var ItensExcluir = Contexto.ItensCustasControleAtosProtestoes.Where(p => p.Id_ControleAto == id).ToList();

            itens = ItensExcluir.ToList();

            for (int cont = 0; cont <= itens.Count - 1; cont++)
            {
                Contexto.ItensCustasControleAtosProtestoes.Remove(itens[cont]);
                Contexto.SaveChanges();
            }
        }

        public decimal CalcularISSProtesto(decimal emol, bool formula, decimal alicotaIss)
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
