using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassControleInterno
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassControleInterno()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<Controle_Interno> ListarEntrada(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Controle_Internoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.EntradaSaida == "ENTRADA").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Controle_Interno> ListarSaida(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Controle_Internoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.EntradaSaida == "SAÍDA").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void Excluir(Controle_Interno Excluir)
        {
            try
            {
                Controle_Interno excluir = new Controle_Interno();

                excluir = contexto.Controle_Internoes.Where(p => p.ControleInternoId == Excluir.ControleInternoId).FirstOrDefault();

                contexto.Controle_Internoes.Remove(excluir);

                contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool SalvarCredito(Controle_Interno ControleSalvar, string status)
        {
            bool ok = false;

            try
            {
                Controle_Interno controleSalvar;

                if (status == "novo")
                    controleSalvar = new Controle_Interno();
                else
                    controleSalvar = contexto.Controle_Internoes.Where(p => p.ControleInternoId == ControleSalvar.ControleInternoId).FirstOrDefault();

                controleSalvar.Data = ControleSalvar.Data;

                controleSalvar.Descricao = ControleSalvar.Descricao;

                controleSalvar.Valor = ControleSalvar.Valor;

                controleSalvar.EntradaSaida = ControleSalvar.EntradaSaida;

                controleSalvar.IdUsuario = ControleSalvar.IdUsuario;

                controleSalvar.Tipo = ControleSalvar.Tipo;

                controleSalvar.Usuario = ControleSalvar.Usuario;

                if (status == "novo")
                    contexto.Controle_Internoes.Add(controleSalvar);

                contexto.SaveChanges();

                ok = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ok;

        }

        public decimal ObterSaldoControleInterno()
        {
            contexto = new CS_CAIXA_DBContext();

            decimal retorno = -1;

            var parametros = contexto.Parametros.FirstOrDefault();

            if (parametros != null && parametros.ValorSaldoControleInterno != null)
                retorno = Convert.ToDecimal(parametros.ValorSaldoControleInterno);

            return retorno;
        }

        public void SalvarSaldoControle(decimal valor)
        {
            Parametro parametros = contexto.Parametros.FirstOrDefault();

            parametros.ValorSaldoControleInterno = valor;

            contexto.SaveChanges();
        }

        public decimal CalcularSaldo(decimal valor, bool sinal)
        {
            contexto = new CS_CAIXA_DBContext();

            decimal valorRetorno = 0M;

            Parametro parametros = contexto.Parametros.FirstOrDefault();

            if (parametros != null)
            {
                if (sinal == true)
                    valorRetorno = Convert.ToDecimal(parametros.ValorSaldoControleInterno) + valor;
                else
                    valorRetorno = Convert.ToDecimal(parametros.ValorSaldoControleInterno) - valor;
            }

            parametros.ValorSaldoControleInterno = valorRetorno;
            contexto.SaveChanges();

            return valorRetorno;
        }

        public List<string> ObterTiposControleInterno()
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.Controle_Internoes.Select(p => p.Tipo).Distinct().ToList();
        }
    }
}
