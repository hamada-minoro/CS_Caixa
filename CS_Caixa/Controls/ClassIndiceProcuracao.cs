using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassIndiceProcuracao
    {
        CS_CAIXA_DBContext contexto { get; set; }
        public ClassIndiceProcuracao()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<IndiceProcuracao> ListarIndiceProcuracaoNome(string Nome, string tipoParte)
        {
            List<IndiceProcuracao> retornoLista = new List<IndiceProcuracao>();

            try
            {
                if (tipoParte == "OUTORGANTE")
                    retornoLista = contexto.IndiceProcuracaos.Where(p => p.Outorgante.Contains(Nome)).OrderBy(p => p.Outorgante).ToList();

                if (tipoParte == "OUTORGADO")
                    retornoLista = contexto.IndiceProcuracaos.Where(p => p.Outorgado.Contains(Nome)).OrderBy(p => p.Outorgado).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<IndiceProcuracao> ListarIndiceProcuracaoSalvo(int id)
        {
            List<IndiceProcuracao> retornoLista = new List<IndiceProcuracao>();

            try
            {
                retornoLista = contexto.IndiceProcuracaos.Where(p => p.IdIndiceProcuracao == id).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int SalvarIndiceProcuracao(IndiceProcuracao SalvarIndiceProcuracao, string status)
        {
            try
            {
                IndiceProcuracao Salvar;

                if (status == "novo")
                    Salvar = new IndiceProcuracao();
                else
                    Salvar = contexto.IndiceProcuracaos.Where(p => p.IdIndiceProcuracao == SalvarIndiceProcuracao.IdIndiceProcuracao).FirstOrDefault();

                Salvar.Outorgante = SalvarIndiceProcuracao.Outorgante;
                Salvar.Outorgado = SalvarIndiceProcuracao.Outorgado;
                Salvar.Mes = SalvarIndiceProcuracao.Mes;
                Salvar.Livro = SalvarIndiceProcuracao.Livro;
                Salvar.Fls = SalvarIndiceProcuracao.Fls;
                Salvar.Dia = SalvarIndiceProcuracao.Dia;
                Salvar.Ato = SalvarIndiceProcuracao.Ato;
                Salvar.Ano = SalvarIndiceProcuracao.Ano;

                if (status == "novo")
                    contexto.IndiceProcuracaos.Add(Salvar);


                contexto.SaveChanges();

                return Salvar.IdIndiceProcuracao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool Excluir(IndiceProcuracao ExcluirIndiceProcuracao)
        {
            try
            {

                IndiceProcuracao Excluir = new IndiceProcuracao();
                Excluir = contexto.IndiceProcuracaos.Where(p => p.IdIndiceProcuracao == ExcluirIndiceProcuracao.IdIndiceProcuracao).FirstOrDefault();
                contexto.IndiceProcuracaos.Remove(Excluir);

                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string RemoveAcentos(string nome)
        {
            string Nome = nome;

            Nome = Nome.Replace("Ã", "A").Replace("Â", "A").Replace("Á", "A").Replace("À", "A").Replace("Ç", "C").Replace("É", "E").Replace("Ê", "E").Replace("Ô", "O").Replace("Ó", "O").Replace("Í", "I").Replace("Ú", "U");

            Nome = Nome.Trim();

            return Nome;
        }
    }
}
