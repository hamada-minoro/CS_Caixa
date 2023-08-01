using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassLoteamentos
    {
        CS_CAIXA_DBContext contexto { get; set; }


        public ClassLoteamentos()
        {
            contexto = new CS_CAIXA_DBContext();
        }


        public List<Loteamento> ListaLoteamentosTodos()
        {
            return contexto.Loteamentos.Select(p => p).ToList();
        }

        public List<Loteamento> ListaLoteamentosPorNome(string nome)
        {
            return contexto.Loteamentos.Where(p => p.Nome.Contains(nome)).ToList();
        }

        public List<Loteamento> ListaLoteamentosPorProprietario(string proprietario)
        {
            return contexto.Loteamentos.Where(p => p.Proprietario.Contains(proprietario)).ToList();
        }

        public List<Loteamento> ListaLoteamentosPorMatricula(int matricula)
        {
            return contexto.Loteamentos.Where(p => p.Matricula == matricula).ToList();
        }

        public Loteamento Salvar(Loteamento loteamento, string status)
        {
            try
            {
                Loteamento itemSalvar;
                if (status == "alterar")
                {
                    itemSalvar = contexto.Loteamentos.Where(p => p.LoteamentoId == loteamento.LoteamentoId).FirstOrDefault();
                }
                else
                {
                    itemSalvar = new Loteamento();
                }
                itemSalvar.Nome = loteamento.Nome;
                itemSalvar.Proprietario = loteamento.Proprietario;
                itemSalvar.NumeroInscricao = loteamento.NumeroInscricao;
                itemSalvar.Matricula = loteamento.Matricula;
                itemSalvar.Localizacao = loteamento.Localizacao;

                if (status == "novo")
                    contexto.Loteamentos.Add(itemSalvar);

                contexto.SaveChanges();

                return itemSalvar;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Excluir(Loteamento ExcluirLoteamento)
        {
            try
            {

                Loteamento Excluir = new Loteamento();
                Excluir = contexto.Loteamentos.Where(p => p.LoteamentoId == ExcluirLoteamento.LoteamentoId).FirstOrDefault();
                contexto.Loteamentos.Remove(Excluir);

                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
