using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassRetiradaCaixa
    {
        CS_CAIXA_DBContext Contexto { get; set; }
        public ClassRetiradaCaixa()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<Retirada_Caixa> ListaRetiradaCaixaData(DateTime dataInicio, DateTime dataFim)
        {
             return Contexto.Retirada_Caixa.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public int SalvarRegistro(Retirada_Caixa novoItem, string status)
        {
            try
            {
                Retirada_Caixa itemSalvar;
                if (status == "alterar")
                {
                    itemSalvar = Contexto.Retirada_Caixa.Where(p => p.Cod == novoItem.Cod).FirstOrDefault();
                }
                else
                {
                    itemSalvar = new Retirada_Caixa();
                }
                itemSalvar.Data = novoItem.Data;
                itemSalvar.Valor = novoItem.Valor;
                itemSalvar.Descricao = novoItem.Descricao;

                if (status == "novo")
                    itemSalvar.NumeroRecibo = ProximoNumeroRecibo();
                else
                    itemSalvar.NumeroRecibo = novoItem.NumeroRecibo;

                if (status == "novo")
                    Contexto.Retirada_Caixa.Add(itemSalvar);

                Contexto.SaveChanges();

                return Convert.ToInt32(itemSalvar.NumeroRecibo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int ProximoNumeroRecibo()
        {
            try
            {
                int numero = Contexto.Retirada_Caixa.Max(p => p.NumeroRecibo).Value;

                return numero + 1;
            }
            catch (Exception)
            {
                return 1;
            }
        }

        public void ExcluirRegistro(Retirada_Caixa ExcluirItem)
        {
            try
            {
                Retirada_Caixa itemExcluir = new Retirada_Caixa();
                itemExcluir = Contexto.Retirada_Caixa.Where(p => p.Cod == ExcluirItem.Cod).FirstOrDefault();

                Contexto.Retirada_Caixa.Remove(itemExcluir);

                Contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
