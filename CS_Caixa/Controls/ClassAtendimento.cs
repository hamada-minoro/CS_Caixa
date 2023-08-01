using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{  
     
    public class ClassAtendimento
    {
        CS_CAIXA_DBContext contexto { get; set; }


        public ConexaoPainelSenha ObterConexaoServidorAtendimento()
        {
            contexto = new CS_CAIXA_DBContext();

            return contexto.ConexaoPainelSenhas.FirstOrDefault();
        }

        public ConexaoPainelSenha SalvarConexaoServidorAtendimento(string ip, int porta)
        {

            ConexaoPainelSenha alterar = contexto.ConexaoPainelSenhas.FirstOrDefault();

            bool novo;

            if (alterar == null)
            {
                alterar = new ConexaoPainelSenha();
                novo = true;
            }
            else
            {
                novo = false;
            }

            alterar.IpServidorAtendimento = ip;
            alterar.PortaConexao = porta;

            if (novo == true)
                contexto.ConexaoPainelSenhas.Add(alterar);

            contexto.SaveChanges();


            return alterar;
        }

        public ConexaoPainelSenha SalvarConexaoServidorAtendimentoNotas(string ip, int porta)
        {

            ConexaoPainelSenha alterar = contexto.ConexaoPainelSenhas.FirstOrDefault();

            bool novo;

            if (alterar == null)
            {
                alterar = new ConexaoPainelSenha();
                novo = true;
            }
            else
            {
                novo = false;
            }

            alterar.IpServidorAtendimentoNotas = ip;
            alterar.PortaConexaoNotas = porta;

            if (novo == true)
                contexto.ConexaoPainelSenhas.Add(alterar);

            contexto.SaveChanges();


            return alterar;
        }

        public List<Atendimento> ListaEmEsperaPrioridade(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaPrioridade = new List<Atendimento>();

                listaPrioridade = contexto.Atendimentoes.Where(p => p.Data == data && p.Status == "EM ESPERA" && (p.TipoAtendimento == "P" || p.TipoAtendimento == "E")).OrderBy(p => p.Fila).ToList();

                return listaPrioridade;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<Atendimento> ListaEmEsperaPrioridade65(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaPrioridade = new List<Atendimento>();

                listaPrioridade = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "E" && p.Status == "EM ESPERA").OrderBy(p => p.Fila).ToList();

                return listaPrioridade;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Atendimento> ListaEmEsperaNormal(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "G" && p.Status == "EM ESPERA").OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Atendimento> ListaEmEsperaInformacao(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "I" && p.Status == "EM ESPERA").OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public List<Atendimento> ListaTodosInformacao(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "I").OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Atendimento> ListaEmEsperaNotas(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "S" && p.Status == "EM ESPERA" && p.NomeAtendente == null).OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public List<Atendimento> ListaTodosNotas(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "S").OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Atendimento> ListaVinculadosNotasEmEspera(DateTime data)
        {
            try
            {
                contexto = new CS_CAIXA_DBContext();

                var listaNormal = new List<Atendimento>();

                listaNormal = contexto.Atendimentoes.Where(p => p.Data == data && p.TipoAtendimento == "S" && p.Status == "EM ESPERA" && p.NomeAtendente != null).OrderBy(p => p.Fila).ToList();

                return listaNormal;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public Atendimento AtualizaAtendimento(Atendimento atendimento)
        {
            var atendimentoAlterar = contexto.Atendimentoes.Where(p => p.AtendimentoId == atendimento.AtendimentoId).FirstOrDefault();
            atendimentoAlterar.HoraAtendimento = atendimento.HoraAtendimento;
            atendimentoAlterar.Status = atendimento.Status;
            atendimentoAlterar.HoraFinalizado = atendimento.HoraFinalizado;
            atendimentoAlterar.HoraRetirada = atendimento.HoraRetirada;
            atendimentoAlterar.IdUsuario = atendimento.IdUsuario;
            atendimentoAlterar.NomeAtendente = atendimento.NomeAtendente;
            atendimentoAlterar.OrdemChamada = atendimento.OrdemChamada;
            
            contexto.SaveChanges();


            return atendimentoAlterar;


        }

        public string UltimaSenhaAtendidaUsuario(int IdUsuario, DateTime data)
        {
            Atendimento senha = contexto.Atendimentoes.Where(p => p.IdUsuario == IdUsuario && p.Data == data && p.HoraAtendimento != null).OrderByDescending(p => p.HoraAtendimento).FirstOrDefault();

            if (senha != null)
                return senha.Senha;
            else
                return "";


        }

        public List<Atendimento> ObterAtendimentosFinalizadosData(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && p.Status == "FINALIZADO").ToList();
        }

        public List<Senha> ObterAtendimentosFinalizadosPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.Senhas.Where(p => (p.Data >= dataInicio && p.Data <= dataFim) && p.Status == "FINALIZADA").ToList();
        }


        public List<ReciboBalcao> ObterReciboPorIdAtendimento(int idAtend)
        {
            return contexto.ReciboBalcaos.Where(p => p.IdAtendimento == idAtend).ToList();
        }

        public List<Ato> ObterSelosRecibo(int idRecibo)
        {
            return contexto.Atoes.Where(p => p.IdReciboBalcao == idRecibo).ToList();
        }

        public List<Atendimento> ObterAtendimentosEmAtendimentoData(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && p.Status == "ATENDENDO").ToList();
        }

        public List<Atendimento> ObterAtendimentosChamadaData(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && p.Status == "SENHA CHAMADA").ToList();
        }

        public List<Atendimento> ObterAtendimentosCanceladasData(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && p.Status == "CANCELADO").ToList();
        }

        public List<Atendimento> ObterAtendimentosData(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data ).ToList();
        }


        public Atendimento ObterAtendimentosPorId(int id)
        {
            return contexto.Atendimentoes.Where(p => p.AtendimentoId == id).FirstOrDefault();
        }

        public List<Atendimento> ObterAtendimentosSenha(string senha)
        {
            return contexto.Atendimentoes.Where(p => p.Senha == senha && p.Status == "FINALIZADO").ToList();
        }

        public Atendimento ObterUltimoAtendimento(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && p.OrdemChamada != null).OrderByDescending(p => p.OrdemChamada).FirstOrDefault();
        }

        public Atendimento ObterUltimaChamada(DateTime data)
        {
            return contexto.Atendimentoes.Where(p => p.Data == data && (p.TipoAtendimento == "P" || p.TipoAtendimento == "G")).OrderByDescending(p => p.OrdemChamada).FirstOrDefault();
        }

        public Atendimento RetirarSenhaRapida(Atendimento atendimento)
        {
           var atend = new Atendimento();

            atend.Data = atendimento.Data;
            atend.Fila = atendimento.Fila;
            atend.HoraAtendimento = atendimento.HoraAtendimento;
            atend.HoraRetirada = atendimento.HoraRetirada;
            atend.IdUsuario = atendimento.IdUsuario;
            atend.NomeAtendente = atendimento.NomeAtendente;
            atend.OrdemChamada = atendimento.OrdemChamada;
            atend.Senha = atendimento.Senha;
            atend.Status = atendimento.Status;
            atend.HoraFinalizado = atendimento.HoraFinalizado;
            atend.TipoAtendimento = "R";


            contexto.Atendimentoes.Add(atend);
            contexto.SaveChanges();

            return atend;
        }

        public void DeletarAtendimento(int? atendimento)
        {
            contexto = new CS_CAIXA_DBContext();

            if (atendimento != null)
            {
                Senha atend = contexto.Senhas.Where(p => p.Senha_Id == atendimento).FirstOrDefault();

                contexto.Senhas.Remove(atend);
                contexto.SaveChanges();
            }
        }
    }
}
