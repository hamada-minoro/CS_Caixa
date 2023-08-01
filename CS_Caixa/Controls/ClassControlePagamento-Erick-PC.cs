using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassControlePagamento
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassControlePagamento()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<ControlePagamentoCredito> ListarCredito(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.ControlePagamentoCreditoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<ControlePagamentoDebito> ListarDebito(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.ControlePagamentoDebitoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ControlePagamentoCredito ObterCredito(DateTime data, string descricao, decimal valor)
        {
            try
            {
                return contexto.ControlePagamentoCreditoes.Where(p => p.Data == data && p.Descricao == descricao && p.Valor == valor).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public ControlePagamentoDebito ObterDebto(DateTime data, string descricao, decimal valor)
        {
            try
            {
                return contexto.ControlePagamentoDebitoes.Where(p => p.Data == data && p.Descricao == descricao && p.Valor == valor).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool SalvarCredito(ControlePagamentoCredito CreditoSalvar, string status)
        {
            bool ok = false;

            try
            {
                ControlePagamentoCredito creditoSalvar;

                if (status == "novo")
                    creditoSalvar = new ControlePagamentoCredito();
                else
                    creditoSalvar = contexto.ControlePagamentoCreditoes.Where(p => p.Id == CreditoSalvar.Id).FirstOrDefault();

                creditoSalvar.Data = CreditoSalvar.Data;

                creditoSalvar.Descricao = CreditoSalvar.Descricao;

                creditoSalvar.Valor = CreditoSalvar.Valor;

                creditoSalvar.Importado = CreditoSalvar.Importado;

                creditoSalvar.TipoCredito = CreditoSalvar.TipoCredito;

                creditoSalvar.Origem = CreditoSalvar.Origem;

                if (status == "novo")
                    contexto.ControlePagamentoCreditoes.Add(creditoSalvar);

                contexto.SaveChanges();

                ok = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
                return ok;
            
        }

       


        public bool SalvarDebito(ControlePagamentoDebito DebitoSalvar, string status, bool importado)
        {
            bool ok = false;

            try
            {
                ControlePagamentoDebito debitoSalvar;

                if (status == "novo")
                    debitoSalvar = new ControlePagamentoDebito();
                else
                    debitoSalvar = contexto.ControlePagamentoDebitoes.Where(p => p.Id == DebitoSalvar.Id).FirstOrDefault();

                debitoSalvar.Data = DebitoSalvar.Data;

                debitoSalvar.Importado = importado;
                
                debitoSalvar.Descricao = DebitoSalvar.Descricao;

                debitoSalvar.Valor = DebitoSalvar.Valor;

                debitoSalvar.TipoDebito = DebitoSalvar.TipoDebito;

                debitoSalvar.Origem = DebitoSalvar.Origem;

                debitoSalvar.Importado = DebitoSalvar.Importado;

                debitoSalvar.IdUsuario = DebitoSalvar.IdUsuario;

                debitoSalvar.Usuario = DebitoSalvar.Usuario;

                if (status == "novo")
                    contexto.ControlePagamentoDebitoes.Add(debitoSalvar);

                contexto.SaveChanges();

                ok = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ok;

        }



        public void ExcluirCredito(ControlePagamentoCredito ExcluirCredito)
        {
            try
            {
                ControlePagamentoCredito excluirCredito = new ControlePagamentoCredito();

                excluirCredito = contexto.ControlePagamentoCreditoes.Where(p => p.Id == ExcluirCredito.Id).FirstOrDefault();

                contexto.ControlePagamentoCreditoes.Remove(excluirCredito);

                contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void ExcluirDebito(ControlePagamentoDebito ExcluirDebito)
        {
            try
            {
                ControlePagamentoDebito excluirDebito = new ControlePagamentoDebito();

                excluirDebito = contexto.ControlePagamentoDebitoes.Where(p => p.Id == ExcluirDebito.Id).FirstOrDefault();

                contexto.ControlePagamentoDebitoes.Remove(excluirDebito);

                contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
