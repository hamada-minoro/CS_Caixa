using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassCadastroCheque
    {
        CS_CAIXA_DBContext Contexto { get; set; }

        public ClassCadastroCheque()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CadCheque> ListaCadChequeData(DateTime dataInicio, DateTime dataFim)
        {
            return Contexto.CadCheques.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public void SalvarRegistro(CadCheque novoItem, string status)
        {
            try
            {


                CadCheque itemSalvar;
                if (status == "alterar")
                {
                    itemSalvar = Contexto.CadCheques.Where(p => p.Id == novoItem.Id).FirstOrDefault();
                }
                else
                {
                    itemSalvar = new CadCheque();
                }
                itemSalvar.Data = novoItem.Data;
                itemSalvar.NumCheque = novoItem.NumCheque;
                itemSalvar.Caixa = novoItem.Caixa;
                itemSalvar.Valor = novoItem.Valor;
                itemSalvar.Obs = novoItem.Obs;

                if (status == "novo")
                    Contexto.CadCheques.Add(itemSalvar);

                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<CadCheque> ListarTodosPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return Contexto.CadCheques.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public void ExcluirRegistro(CadCheque ExcluirItem)
        {
            try
            {
                CadCheque itemExcluir = new CadCheque();
                itemExcluir = Contexto.CadCheques.Where(p => p.Id == ExcluirItem.Id).FirstOrDefault();

                Contexto.CadCheques.Remove(itemExcluir);

                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
