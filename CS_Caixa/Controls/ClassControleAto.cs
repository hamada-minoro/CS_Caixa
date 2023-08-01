using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassControleAto
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassControleAto()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<ControleAto> ListarAto(string tipoAto, string atribuicao)
        {
            var listaAtos = new List<ControleAto>();

            try
            {
                listaAtos = contexto.ControleAtos.Where(p => p.Atribuicao == atribuicao).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }

            return listaAtos;
        }


        public List<ControleAto> ListarAtoNome(string tipoAto, string atribuicao, int idNome)
        {
            var listaAtos = new List<ControleAto>();
            try
            {
                if (atribuicao == "NOTAS")
                    listaAtos = contexto.ControleAtos.Where(p => p.Atribuicao == atribuicao && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "PROTESTO")
                    listaAtos = contexto.ControleAtos.Where(p => p.Atribuicao == atribuicao && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "RGI")
                    listaAtos = contexto.ControleAtos.Where(p => p.Atribuicao == atribuicao && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();


            }
            catch (Exception)
            {
                return null;
            }

            return listaAtos;
        }


        public List<ControleAto> ListarAtoDataProtesto(DateTime dataIni, DateTime dataFim, string tipoAto)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarTodosOsAtosPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.ControleAtos.Where(p => p.DataAto >= dataInicio && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<ControleAto> ListarAtoProtocolo(int protocolo, string tipoAto)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "PAGAMENTO" || tipoAto == "RETIRADA")
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.Protocolo == protocolo).ToList();
                else
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.Recibo == protocolo).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarAtoProtocoloNomeEscrevente(int protocolo, string tipoAto, int IdNome)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "PAGAMENTO" || tipoAto == "RETIRADA")
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.Protocolo == protocolo && p.IdUsuario == IdNome).ToList();
                else
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.Recibo == protocolo && p.IdUsuario == IdNome).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }



        //public List<ItensAtoRgi> CarregaGridItensAto(int idAto)
        //{
        //    return contexto.ItensAtoRgis.Where(p => p.Id_Ato == idAto).ToList();
        //}


        //public List<ItensCustasRgi> CarregaItensCustasAlterar(int idAtoRgi)
        //{
        //    return contexto.ItensCustasRgis.Where(p => p.Id_AtoRgi == idAtoRgi).ToList();
        //}

        public List<Ato> ListarAtoProtocoloRgi(int protocolo, string tipoAto)
        {
            try
            {
                if (tipoAto == "REGISTRO" || tipoAto == "ABERBAÇÃO")
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.Protocolo == protocolo).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.Recibo == protocolo).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ControleAto> ListarAtoProtocoloRgiNomeEscrevente(int protocolo, string tipoAto, int IdNome)
        {
            try
            {
                if (tipoAto == "REGISTRO" || tipoAto == "ABERBAÇÃO")
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "RGI" && p.Protocolo == protocolo && p.IdUsuario == IdNome).ToList();
                else
                    return contexto.ControleAtos.Where(p => p.Atribuicao == "RGI" && p.Recibo == protocolo && p.IdUsuario == IdNome).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarAtoData(DateTime dataIni, DateTime dataFim, string tipoAto, string atribuicao)
        {
            try
            {
                var ListarAtoData = new List<ControleAto>();

                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "RGI" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "PROTESTO")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ControleAto> ListarNotasLivroFolhasNumeroAto(string livro, int folhaIni, int folhaFim, int ato, string tipoAto)
        {
            try
            {
                var ListarAtoData = new List<ControleAto>();

                ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.Livro == livro && p.FolhaInical == folhaIni && p.FolhaFinal == folhaFim && p.NumeroAto == ato).OrderByDescending(p => p.DataAto).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<ControleAto> ListarAtoDataNomeEscrevente(DateTime dataIni, DateTime dataFim, string tipoAto, string atribuicao, int idNome)
        {
            try
            {
                var ListarAtoData = new List<ControleAto>();

                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.DataAto >= dataIni && p.DataAto <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "RGI" && p.DataAto >= dataIni && p.DataAto <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "PROTESTO")
                    ListarAtoData = contexto.ControleAtos.Where(p => p.Atribuicao == "PROTESTO" && p.DataAto >= dataIni && p.DataAto <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataAto).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarAtoLivro(string livro, string tipoAto)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.Livro == livro).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarAtoLivroNomeEscrevente(string livro, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.Livro == livro && p.IdUsuario == IdNome).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ControleAto> ListarAtoNumeroAto(int ato, string tipoAto)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.NumeroAto == ato).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<ControleAto> ListarAtoNumeroAtoNomeEscrevente(int ato, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.NumeroAto == ato && p.IdUsuario == IdNome).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<ControleAto> ListarAtoSelo(string letra, int numero, string tipoAto)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.LetraSelo == letra && p.NumeroSelo == numero).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<ControleAto> ListarAtoSeloNomeEscrevente(string letra, int numero, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.ControleAtos.Where(p => p.Atribuicao == "NOTAS" && p.LetraSelo == letra && p.NumeroSelo == numero && p.IdUsuario == IdNome).OrderByDescending(p => p.DataAto).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int SalvarAto(ControleAto atoCorrente, string status)
        {
            ControleAto atoSalvar;


            if (status == "novo")
                atoSalvar = new ControleAto();
            else
                atoSalvar = contexto.ControleAtos.Where(p => p.Id_ControleAtos == atoCorrente.Id_ControleAtos).FirstOrDefault();



            atoSalvar.Id_Ato = atoCorrente.Id_Ato;

            atoSalvar.DataAto = atoCorrente.DataAto;

            atoSalvar.IdUsuario = atoCorrente.IdUsuario;

            atoSalvar.Usuario = atoCorrente.Usuario;

            atoSalvar.Atribuicao = atoCorrente.Atribuicao;

            atoSalvar.LetraSelo = atoCorrente.LetraSelo;

            atoSalvar.NumeroSelo = atoCorrente.NumeroSelo;

            atoSalvar.Convenio = atoCorrente.Convenio;

            atoSalvar.Faixa = atoCorrente.Faixa;

            atoSalvar.Livro = atoCorrente.Livro;

            atoSalvar.FolhaInical = atoCorrente.FolhaInical;

            atoSalvar.FolhaFinal = atoCorrente.FolhaFinal;

            atoSalvar.NumeroAto = atoCorrente.NumeroAto;

            atoSalvar.Recibo = atoCorrente.Recibo;

            atoSalvar.Protocolo = atoCorrente.Protocolo;

            atoSalvar.ReciboBalcao = atoCorrente.ReciboBalcao;

            atoSalvar.AtoGratuito = atoCorrente.AtoGratuito;

            atoSalvar.AtoNaoGratuito = atoCorrente.AtoNaoGratuito;

            atoSalvar.TipoAto = atoCorrente.TipoAto;

            atoSalvar.Natureza = atoCorrente.Natureza;

            atoSalvar.Emolumentos = atoCorrente.Emolumentos;

            atoSalvar.Fetj = atoCorrente.Fetj;

            atoSalvar.Fundperj = atoCorrente.Fundperj;

            atoSalvar.Funperj = atoCorrente.Funperj;

            atoSalvar.Funarpen = atoCorrente.Funarpen;

            atoSalvar.Pmcmv = atoCorrente.Pmcmv;

            atoSalvar.Iss = atoCorrente.Iss;

            atoSalvar.Mutua = atoCorrente.Mutua;

            atoSalvar.Acoterj = atoCorrente.Acoterj;

            atoSalvar.QtdAtos = atoCorrente.QtdAtos;

            atoSalvar.Total = atoCorrente.Total;

            if(status == "novo")
            contexto.ControleAtos.Add(atoSalvar);

            contexto.SaveChanges();

            if (status == "novo")
                return atoSalvar.Id_ControleAtos;
            else
                return atoCorrente.Id_ControleAtos;
        }


        public string ExcluirAtoPorId_Ato(int id_Ato)
        {

            try
            {
                var ControleAtoexcluir = contexto.ControleAtos.Where(p => p.Id_Ato == id_Ato).FirstOrDefault();

                var itens = new List<ItensCustasControleAtosNota>();

                itens = contexto.ItensCustasControleAtosNotas.Where(p => p.Id_ControleAto == ControleAtoexcluir.Id_ControleAtos).ToList();

                for (int cont = 0; cont <= itens.Count - 1; cont++)
                {
                    contexto.ItensCustasControleAtosNotas.Remove(itens[cont]);
                    contexto.SaveChanges();
                }

                contexto.ControleAtos.Remove(ControleAtoexcluir);
                contexto.SaveChanges();
                return "Exclusão realizada com sucesso.";

            }
            catch (Exception)
            {
                return "Ocorreu um erro inesperado, registro não foi excluído.";
            }
        }

        public string ExcluirAtoPorReciboBalcao(int ReciboBalcao)
        {

            try
            {


                var ControleAtoexcluir = contexto.ControleAtos.Where(p => p.ReciboBalcao == ReciboBalcao).ToList();

               
                for (int i = 0; i < ControleAtoexcluir.Count; i++)
                {
                    contexto.ControleAtos.Remove(ControleAtoexcluir[i]);
                    contexto.SaveChanges();
                }

               
                return "Exclusão realizada com sucesso.";

            }
            catch (Exception)
            {
                return "Ocorreu um erro inesperado, registro não foi excluído.";
            }
        }
        public string ExcluirAto(int id, string atribuicao)
        {

            try
            {
                if (atribuicao == "NOTAS")
                {
                    var itens = new List<ItensCustasControleAtosNota>();



                    var ItensExcluir = contexto.ItensCustasControleAtosNotas.Where(p => p.Id_ControleAto == id).ToList();

                    itens = ItensExcluir;

                    for (int cont = 0; cont <= itens.Count - 1; cont++)
                    {
                        contexto.ItensCustasControleAtosNotas.Remove(itens[cont]);
                        contexto.SaveChanges();
                    }


                    var atoExcluir = new ControleAto();
                    var excluir = contexto.ControleAtos.Where(p => p.Id_ControleAtos == id);

                    atoExcluir = excluir.FirstOrDefault();
                    contexto.ControleAtos.Remove(atoExcluir);
                    contexto.SaveChanges();
                    return "Exclusão realizada com sucesso.";
                }


                //if (atribuicao == "PROTESTO")
                //{
                //    List<ItensCustasProtesto> itens = new List<ItensCustasProtesto>();


                //    var ItensExcluir = contexto.ItensCustasProtestoes.Where(p => p.Id_Ato == id).ToList();

                //    itens = ItensExcluir;

                //    for (int cont = 0; cont <= itens.Count - 1; cont++)
                //    {
                //        contexto.ItensCustasProtestoes.Remove(itens[cont]);
                //        contexto.SaveChanges();
                //    }


                //    Ato atoExcluir = new Ato();
                //    var excluir = contexto.Atoes.Where(p => p.Id_Ato == id);

                //    atoExcluir = excluir.FirstOrDefault();
                //    contexto.Atoes.Remove(atoExcluir);
                //    contexto.SaveChanges();
                //    return "Exclusão realizada com sucesso.";
                //}
                //if (atribuicao == "RGI")
                //{
                //    var id_AtoRgi = contexto.ItensAtoRgis.Where(p => p.Id_Ato == id).ToList();

                //    for (int cont = 0; cont < id_AtoRgi.Count; cont++)
                //    {
                //        var excluirItens = contexto.ItensCustasRgis.Where(p => p.Id_Ato == id).ToList();

                //        for (int i = 0; i < excluirItens.Count; i++)
                //        {
                //            contexto.ItensCustasRgis.Remove(excluirItens[i]);
                //            contexto.SaveChanges();
                //        }

                //    }

                //    for (int i = 0; i < id_AtoRgi.Count; i++)
                //    {
                //        contexto.ItensAtoRgis.Remove(id_AtoRgi[i]);
                //        contexto.SaveChanges();
                //    }

                //    Ato atoExcluir = new Ato();

                //    var excluir = contexto.Atoes.Where(p => p.Id_Ato == id);
                //    atoExcluir = excluir.FirstOrDefault();
                //    contexto.Atoes.Remove(atoExcluir);
                //    contexto.SaveChanges();
                //    return "Exclusão realizada com sucesso.";

                //}




                return string.Empty;
            }
            catch (Exception)
            {
                return "Ocorreu um erro inesperado, registro não foi excluído.";
            }
        }





        public int ProximoIdAto()
        {
            contexto = new CS_CAIXA_DBContext();

            var id = contexto.ControleAtos.Select(p => p).ToList();

            if (id.Count > 0)
            {
                return id.Max(p => p.Id_ControleAtos) + 1;

            }
            else
            {
                return 1;
            }
        }
    }
}
