using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassAgendaEletronica
    {
        public CS_CAIXA_DBContext Contexto { get; set; }

        public ClassAgendaEletronica()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CS_Caixa.Models.AgendaEletronica> AgendasPorData(DateTime data, int usuarioId)
        {
            Contexto = new CS_CAIXA_DBContext();
            return Contexto.AgendaEletronicas.Where(p => p.Data == data && p.IdUsuario == usuarioId).ToList();
        }

        public CS_Caixa.Models.AgendaEletronica AgendasPorId(int id)
        {
            return Contexto.AgendaEletronicas.Where(p => p.IdAgenda == id).FirstOrDefault();
        }

        public List<string> AgendasTipoAtoPorIdUsuario(int usuarioId)
        {
            return Contexto.AgendaEletronicas.Where(p => p.IdUsuario == usuarioId).Select(p => p.TipoAto).Distinct().ToList();
        }

        public List<string> AgendasNomeClientePorIdUsuario(int usuarioId)
        {
            return Contexto.AgendaEletronicas.Where(p => p.IdUsuario == usuarioId).Select(p => p.NomeCliente).Distinct().ToList();
        }

        public void ExcluirAgenda(CS_Caixa.Models.AgendaEletronica agenda)
        {
            var excluir = Contexto.AgendaEletronicas.Where(p => p.IdAgenda == agenda.IdAgenda).FirstOrDefault();

            if (excluir != null)
            {
                Contexto.AgendaEletronicas.Remove(excluir);
                Contexto.SaveChanges();
            }
        }

        public CS_Caixa.Models.AgendaEletronica SalvarAgenda(CS_Caixa.Models.AgendaEletronica agenda, string tipo)
        {
            Models.AgendaEletronica agendaSalvar;

            if (tipo == "novo")
                agendaSalvar = new Models.AgendaEletronica();
            else
                agendaSalvar = Contexto.AgendaEletronicas.Where(p => p.IdAgenda == agenda.IdAgenda).FirstOrDefault();

            if (agendaSalvar != null)
            {
                agendaSalvar.IdAgenda = agenda.IdAgenda;
                agendaSalvar.CorBotao = agenda.CorBotao;
                agendaSalvar.Data = agenda.Data;
                agendaSalvar.Hora = agenda.Hora;
                agendaSalvar.IdUsuario = agenda.IdUsuario;
                agendaSalvar.NomeCliente = agenda.NomeCliente;
                agendaSalvar.Observacao = agenda.Observacao;
                agendaSalvar.TipoAto = agenda.TipoAto;
                agendaSalvar.Usuario = agenda.Usuario;

                if (tipo == "novo")
                    Contexto.AgendaEletronicas.Add(agendaSalvar);
               

                    Contexto.SaveChanges();
            }

            return agendaSalvar;
        }
    }
}
