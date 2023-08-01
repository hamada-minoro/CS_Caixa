using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassRecolhimento
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassRecolhimento()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public Recolhimento SalvarRecolhimento(Recolhimento recolhimento, string tipo)
        {
            Recolhimento Salvar;

            if (tipo == "novo")
                Salvar = new Recolhimento();
            else
                Salvar = contexto.Recolhimentoes.Where(p => p.RecolhimentoId == recolhimento.RecolhimentoId).FirstOrDefault();

            Salvar.Ato = recolhimento.Ato;
            Salvar.Atribuicao = recolhimento.Atribuicao;
            Salvar.Convenio = recolhimento.Convenio;
            Salvar.Data = recolhimento.Data;
            Salvar.Emol = recolhimento.Emol;
            Salvar.Fetj = recolhimento.Fetj;
            Salvar.Folha = recolhimento.Folha;
            Salvar.Funa = recolhimento.Funa;
            Salvar.Fund = recolhimento.Fund;
            Salvar.Funp = recolhimento.Funp;
            Salvar.Gratuito = recolhimento.Gratuito;
            Salvar.Iss = recolhimento.Iss;
            Salvar.Livro = recolhimento.Livro;
            Salvar.Matricula = recolhimento.Matricula;
            Salvar.Natureza = recolhimento.Natureza;
            Salvar.Pmcmv = recolhimento.Pmcmv;
            Salvar.Protocolo = recolhimento.Protocolo;
            Salvar.Selo = recolhimento.Selo;
            Salvar.TipoAto = recolhimento.TipoAto;
            Salvar.Excedente = recolhimento.Excedente;

            if (tipo == "novo")
                contexto.Recolhimentoes.Add(Salvar);

            contexto.SaveChanges();

            return Salvar;
        }



        public Recolhimento ObterRecolhimentoPorId(int recolhimentoId)
        {
            return contexto.Recolhimentoes.SingleOrDefault(p => p.RecolhimentoId == recolhimentoId);
        }

        public List<Recolhimento> ObterRecolhimentoPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }
        public List<Recolhimento> ObterRecolhimentoPorPeriodoAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public List<Recolhimento> ObterRecolhimentoPorPeriodoRgi(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "RGI").ToList();
        }
        public List<Recolhimento> ObterRecolhimentoPorPeriodoRgiAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "RGI").ToList();
        }


        public List<Recolhimento> ObterRecolhimentoPorPeriodoNotas(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "NOTAS").ToList();
        }
        public List<Recolhimento> ObterRecolhimentoPorPeriodoNotasAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "NOTAS").ToList();
        }


        public List<Recolhimento> ObterRecolhimentoPorPeriodoProtesto(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "PROTESTO").ToList();
        }
        public List<Recolhimento> ObterRecolhimentoPorPeriodoProtestoAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "PROTESTO").ToList();
        }


        public List<Recolhimento> ObterRecolhimentoPorPeriodoBalcao(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "BALCÃO").ToList();
        }
        public List<Recolhimento> ObterRecolhimentoPorPeriodoBalcaoAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Recolhimentoes.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Atribuicao == "BALCÃO").ToList();
        }




        public List<Recolhimento> ObterAtoRgiPorProtocolo(int protocolo)
        {
            return contexto.Recolhimentoes.Where(p => p.Protocolo == protocolo && p.Atribuicao == "RGI").ToList();
        }

        public void ExcluirRecolhimento(Recolhimento excluir)
        {
            contexto.Recolhimentoes.Remove(excluir);
            contexto.SaveChanges();
        }

    }
}
