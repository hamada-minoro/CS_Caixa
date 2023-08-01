using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioSenha : RepositorioBase<Senha>
    {


        public int ObterProximoFila(DateTime data)
        {
            Db = new CS_CAIXA_DBContext();
            var todos = ObterTodosPorData(data);

            if (todos.Count() == 0)
                return 1;

            return todos.Max(p => p.Sequencia_Chamada) + 1;
        }


        public List<Senha> ObterTodosPorData(DateTime data)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Senhas.Where(p => p.Data == data).ToList();
        }


        public List<Senha> ObterTodosPorSetorData(int setor, DateTime data)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Senhas.Where(p => p.SetorId == setor && p.Data == data).ToList();
        }

        public List<Senha> ObterTodosPorSetorSequencia(int setor, int sequencia)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Senhas.Where(p => p.SetorId == setor && p.NumeroSequencia == sequencia).ToList();
        }

        public int ObterUltimaSequenciaManual()
        {
            Db = new CS_CAIXA_DBContext();
            if (Db.Senhas.Where(p => p.SetorId == 5).Count() > 0)
                return Db.Senhas.Where(p => p.SetorId == 5).Max(p => p.NumeroSequencia);
            else
                return 0;
        }

        public int ObterProximaSequenciaManual()
        {
            Db = new CS_CAIXA_DBContext();
            if (Db.Senhas.Where(p => p.SetorId == 5).Count() == 0)
                return 1;
            else
                return Db.Senhas.Where(p => p.SetorId == 5).Max(p => p.NumeroSequencia) + 1;
        }

        public int SaberSeGeraMaisUmaSequenciaManual()
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Senhas.Where(p => p.SetorId == 5 && p.Status == "EM ESPERA").Count();
        }

        public int OberProximaSenha(bool zerarDiaSeguinte, int setorId, int tipoSenha, out int numeroSequencia, int qtdcCaracteres)
        {
            Db = new CS_CAIXA_DBContext();
            List<Senha> senhas = new List<Senha>();
            int senha = 0;
            int sequencia = 0;

            if (zerarDiaSeguinte == true)
            {
                senhas = ObterTodosPorData(DateTime.Now.Date);

                if (senhas.Where(p => p.SenhaTipo == tipoSenha && p.SetorId == setorId && p.ModoSequencial == false).Count() > 0)
                    senha = senhas.Where(p => p.SenhaTipo == tipoSenha && p.SetorId == setorId && p.ModoSequencial == false).Max(p => p.Numero_Senha) + 1;
                else
                    senha = 1;
            }
            else
            {
                if (Db.Parametros.FirstOrDefault().ModoRetiradaSenhaManual == true)
                {
                    senhas = Db.Senhas.Where(p => p.ModoSequencial == true && p.QtdCaracteres == qtdcCaracteres && p.SetorId == setorId && p.SenhaTipo == tipoSenha).ToList();


                    if (senhas.Count == 0)
                    {
                        sequencia = 1;
                        senha = 1;
                    }
                    else
                    {
                        sequencia = senhas.Where(p => p.QtdCaracteres == qtdcCaracteres && p.ModoSequencial == true).Max(p => p.NumeroSequencia);

                        senha = senhas.Where(p => p.NumeroSequencia == sequencia).Max(p => p.Numero_Senha) + 1;
                    }


                    switch (qtdcCaracteres)
                    {
                        case 0:
                            if (senha > 999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;
                        case 1:
                            if (senha > 9999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;
                        case 2:
                            if (senha > 99999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;

                        default:
                            numeroSequencia = 0;
                            senha = 1;
                            break;
                    }

                }
                else
                {
                    sequencia = Db.Senhas.Where(p => p.ModoSequencial == true && p.QtdCaracteres == qtdcCaracteres && p.SetorId == setorId && p.SenhaTipo == tipoSenha).Max(p => p.NumeroSequencia);
                    senha = Db.Senhas.Where(p => p.ModoSequencial == true && p.QtdCaracteres == qtdcCaracteres && p.SetorId == setorId && p.SenhaTipo == tipoSenha && p.NumeroSequencia == sequencia).Max(p => p.Numero_Senha) + 1;

                    switch (qtdcCaracteres)
                    {
                        case 0:
                            if (senha > 999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;
                        case 1:
                            if (senha > 9999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;
                        case 2:
                            if (senha > 99999)
                            {
                                sequencia = sequencia + 1;
                                senha = 1;
                            }
                            break;

                        default:
                            numeroSequencia = 0;
                            senha = 1;
                            break;
                    }
                }


            }

            numeroSequencia = sequencia;
            return senha;
        }
    }
}
