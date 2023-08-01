using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassRepasseCaixa
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassRepasseCaixa()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<RepasseCaixa> ObterTodosPorPeriodo(DateTime _dataInicio, DateTime _dataFim)
        {
            return contexto.RepasseCaixas.Where(p => p.DataCaixa >= _dataInicio && p.DataCaixa <= _dataFim).ToList();
        }

        public List<RepasseCaixa> ObterTodosPorPeriodoRestante(DateTime _dataInicio, DateTime _dataFim)
        {
            return contexto.RepasseCaixas.Where(p => p.DataPagamentoRepasse >= _dataInicio && p.DataPagamentoRepasse <= _dataFim).ToList();
        }

        public RepasseCaixa SalvarRegistro(RepasseCaixa novoItem, string status)
        {
            try
            {
                RepasseCaixa itemSalvar;
                if (status == "alterar")
                {
                    itemSalvar = contexto.RepasseCaixas.Where(p => p.RepasseId == novoItem.RepasseId).FirstOrDefault();
                }
                else
                {
                    itemSalvar = new RepasseCaixa();
                }
                itemSalvar.DataCaixa = novoItem.DataCaixa;
                itemSalvar.DataPagamentoRepasse = novoItem.DataPagamentoRepasse;
                itemSalvar.Descricao = novoItem.Descricao;
                itemSalvar.ValorCaixa = novoItem.ValorCaixa;
                itemSalvar.ValorRepasse = novoItem.ValorRepasse;
                itemSalvar.ValorRestante = novoItem.ValorRestante;

                if (status == "novo")
                    contexto.RepasseCaixas.Add(itemSalvar);

                contexto.SaveChanges();

                return itemSalvar;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ExcluirRegistro(RepasseCaixa ExcluirItem)
        {
            try
            {

                if (ExcluirItem != null)
                {
                    RepasseCaixa itemExcluir = new RepasseCaixa();
                    itemExcluir = contexto.RepasseCaixas.Where(p => p.RepasseId == ExcluirItem.RepasseId).FirstOrDefault();

                    contexto.RepasseCaixas.Remove(itemExcluir);

                    contexto.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal ObterValorDinheiroCaixaPorDataInicioDataFim(DateTime _dataInicio, DateTime _dataFim)
        {
            decimal valorDinheiroCaixa = 0;

            List<ValorPago> valoresPagos = new List<ValorPago>();
            List<ValorPago> valoresPagosNaoPagos = new List<ValorPago>();
            List<Adicionar_Caixa> valoresAdicionados = new List<Adicionar_Caixa>();
            List<Retirada_Caixa> valoresRetirados = new List<Retirada_Caixa>();
            ClassAto classAto = new ClassAto();

            valoresPagos = classAto.ObterValorPagoPorData(_dataInicio, _dataFim);


            Ato ato = new Ato();
            ReciboBalcao recibo = new ReciboBalcao();

            foreach (var item in valoresPagos)
            {
                if(item.IdAto != 0)
                {
                    ato = classAto.ObterAtoPorIdAto(item.IdAto);
                    if (ato.Pago == false)
                        valoresPagosNaoPagos.Add(item);
                }
                else
                {
                    recibo = classAto.ObterReciboBalcaoPorIdAto(item.IdReciboBalcao);
                    if (recibo.Pago == false)
                        valoresPagosNaoPagos.Add(item);
                }
            }
          
            ClassAdicionarCaixa adicionarCaixa = new ClassAdicionarCaixa();
            valoresAdicionados = adicionarCaixa.ListaTodosPorData(_dataInicio, _dataFim);
            
            ClassRetiradaCaixa classRetirada = new ClassRetiradaCaixa();
            valoresRetirados = classRetirada.ListaRetiradaCaixaData(_dataInicio, _dataFim);

            if (valoresPagos.Count > 0)
            valorDinheiroCaixa = valorDinheiroCaixa + Convert.ToDecimal(valoresPagos.Sum(p => p.Dinheiro));

            if (valoresAdicionados.Count > 0)
                valorDinheiroCaixa = valorDinheiroCaixa + Convert.ToDecimal(valoresAdicionados.Where(p => p.TpPagamento == "DINHEIRO").Sum(p => p.Valor));

            if (valoresRetirados.Count > 0)
                valorDinheiroCaixa = valorDinheiroCaixa - Convert.ToDecimal(valoresRetirados.Sum(p => p.Valor));

            if (valoresPagos.Count > 0)
                valorDinheiroCaixa = valorDinheiroCaixa - Convert.ToDecimal(valoresPagos.Sum(p => p.Troco));

            if (valoresPagos.Count > 0)
                valorDinheiroCaixa = valorDinheiroCaixa - Convert.ToDecimal(valoresPagosNaoPagos.Sum(p => p.Dinheiro));

            return valorDinheiroCaixa;
        }
    }
}
