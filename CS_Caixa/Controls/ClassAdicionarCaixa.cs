using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassAdicionarCaixa
    {
        CS_CAIXA_DBContext Contexto { get; set; }

        public ClassAdicionarCaixa()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<Adicionar_Caixa> ListaAdicionarCaixaData(DateTime dataInicio, DateTime dataFim)
        {
            Contexto = new CS_CAIXA_DBContext();
            return Contexto.Adicionar_Caixa.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public void SalvarRegistro(Adicionar_Caixa novoItem, string status)
        {
            try
            {
                Adicionar_Caixa itemSalvar;
                if (status == "alterar")
                {
                    itemSalvar = Contexto.Adicionar_Caixa.Where(p => p.Cod == novoItem.Cod).FirstOrDefault();
                }
                else
                {
                    itemSalvar = new Adicionar_Caixa();
                }
                itemSalvar.Data = novoItem.Data;
                itemSalvar.Atribuicao = novoItem.Atribuicao;
                itemSalvar.TpPagamento = novoItem.TpPagamento;
                itemSalvar.Valor = novoItem.Valor;
                itemSalvar.Descricao = novoItem.Descricao;

                if (status == "novo")
                    Contexto.Adicionar_Caixa.Add(itemSalvar);

                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Adicionar_Caixa> ListaTodosPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return Contexto.Adicionar_Caixa.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw new Exception(ex.Message);
            }
        }

        public void ExcluirRegistro(Adicionar_Caixa ExcluirItem)
        {
            try
            {
                Adicionar_Caixa itemExcluir = new Adicionar_Caixa();
                itemExcluir = Contexto.Adicionar_Caixa.Where(p => p.Cod == ExcluirItem.Cod).FirstOrDefault();

                Contexto.Adicionar_Caixa.Remove(itemExcluir);

                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
