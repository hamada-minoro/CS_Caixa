using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassAto
    {

        CS_CAIXA_DBContext contexto { get; set; }

        public ClassAto()
        {
            contexto = new CS_CAIXA_DBContext();
        }



        public List<Ato> ListarAto(string tipoAto, string atribuicao)
        {
            List<Ato> listaAtos = new List<Ato>();
            try
            {
                if (atribuicao == "NOTAS")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "PROTESTO")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "RGI")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto).OrderByDescending(p => p.DataPagamento).ToList();


            }
            catch (Exception)
            {
                return null;
            }

            return listaAtos;
        }


        public List<Ato> ListarAtoNome(string tipoAto, string atribuicao, int idNome)
        {
            List<Ato> listaAtos = new List<Ato>();
            try
            {
                if (atribuicao == "NOTAS")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "PROTESTO")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "RGI")
                    listaAtos = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.TipoAto == tipoAto && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();


            }
            catch (Exception)
            {
                return null;
            }

            return listaAtos;
        }


        public List<Ato> ListarAtoDataProtesto(DateTime dataIni, DateTime dataFim, string tipoAto)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<Ato> ListarAtoPorReciboRgi(int? recibo)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.Recibo == recibo).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }




        public List<Ato> ListarTodosOsAtosPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Atoes.Where(p => p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.Pago == true).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarTodosOsAtosPorDataSemDut(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Atoes.Where(p => p.Pago == true && p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.TipoAto != "REC AUTENTICIDADE (DUT)").OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarTodosOsAtosPorDataGeralSemDut(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Atoes.Where(p => p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.TipoAto != "REC AUTENTICIDADE (DUT)").OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarTodosOsAtosPorDataGeralDut(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Atoes.Where(p => p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.TipoAto == "REC AUTENTICIDADE (DUT)").OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarTodosOsAtosPorDataDut(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.Atoes.Where(p => p.Pago == true && p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.TipoAto == "REC AUTENTICIDADE (DUT)").OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarTodosOsAtosPorDataNPago(DateTime dataInicio, DateTime dataFim, string nome)
        {
            try
            {
                return contexto.Atoes.Where(p => p.DataPagamento >= dataInicio && p.DataPagamento <= dataFim && p.Pago == false && p.Escrevente == nome).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtosPorProtocoloIgualRecibo(int protocoloOuRecibo, DateTime dataAto)
        {
            return contexto.Atoes.Where(p => p.Protocolo == protocoloOuRecibo && p.Recibo == protocoloOuRecibo && p.DataAto == dataAto).ToList();

        }


        public List<Ato> ListarAtoProtocolo(int protocolo, string tipoAto)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "PAGAMENTO" || tipoAto == "APONTAMENTO RETIRADO" || tipoAto == "EMISSÃO DE GUIA" || tipoAto == "APONTAMENTO CANCELAMENTO")
                    return contexto.Atoes.Where(p => (p.Atribuicao == "PROTESTO" && p.TipoAto.Contains(tipoAto) && p.Protocolo == protocolo) || (p.Protocolo == protocolo && p.TipoAto == null)).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Recibo == protocolo).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoRecibo(int Recibo, string tipoAto)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "CANCELAMENTO")
                    return contexto.Atoes.Where(p => (p.Atribuicao == "PROTESTO" && p.TipoAto.Contains(tipoAto) && p.Recibo == Recibo) || (p.Recibo == Recibo && p.TipoAto == null)).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Recibo == Recibo).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoProtocoloNomeEscrevente(int protocolo, string tipoAto, int IdNome)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "PAGAMENTO" || tipoAto == "RETIRADO")
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Protocolo == protocolo && p.IdUsuario == IdNome).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Recibo == protocolo && p.IdUsuario == IdNome).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoReciboNomeEscrevente(int Recibo, string tipoAto, int IdNome)
        {
            try
            {
                if (tipoAto == "APONTAMENTO" || tipoAto == "PAGAMENTO" || tipoAto == "RETIRADO")
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Recibo == Recibo && p.IdUsuario == IdNome).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.Recibo == Recibo && p.IdUsuario == IdNome).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<ItensAtoRgi> CarregaGridItensAto(int idAto)
        {
            return contexto.ItensAtoRgis.Where(p => p.Id_Ato == idAto).ToList();
        }


        public List<ItensCustasRgi> CarregaItensCustasAlterar(int idAtoRgi)
        {
            return contexto.ItensCustasRgis.Where(p => p.Id_AtoRgi == idAtoRgi).ToList();
        }

        public List<Ato> ListarAtoProtocoloRgi(int protocolo, string tipoAto)
        {
            try
            {
                if (tipoAto == "REGISTRO" || tipoAto == "ABERBAÇÃO")
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.Protocolo == protocolo).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.Recibo == protocolo).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoProtocoloRgiNomeEscrevente(int protocolo, string tipoAto, int IdNome)
        {
            try
            {
                if (tipoAto == "REGISTRO" || tipoAto == "ABERBAÇÃO")
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.Protocolo == protocolo && p.IdUsuario == IdNome).ToList();
                else
                    return contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.Recibo == protocolo && p.IdUsuario == IdNome).ToList();



            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoData(DateTime dataIni, DateTime dataFim, string tipoAto, string atribuicao)
        {
            try
            {
                List<Ato> ListarAtoData = new List<Ato>();



                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "PROTESTO")
                {
                    switch (tipoAto)
                    {
                        case "APONTAMENTO":
                            ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && (p.TipoAto.Contains(tipoAto) || p.TipoAto.Contains("RETIRADO")) && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();
                            break;
                        case "CANCELAMENTO":
                            ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == "CANCELAMENTO" && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();
                            break;
                        case "PAGAMENTO":
                            ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && (p.TipoAto == tipoAto || p.TipoAto == "EMISSÃO DE GUIA") && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();
                            break;
                        default:
                            ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataPagamento).ToList();
                            break;
                    }
                }

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoDataNaoPago(DateTime dataIni, DateTime dataFim, string atribuicao, Usuario usuario)
        {
            try
            {
                List<Ato> ListarAtoData = new List<Ato>();



                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.Pago == false && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == usuario.Id_Usuario).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.Pago == false && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == usuario.Id_Usuario).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "PROTESTO")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.Pago == false && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == usuario.Id_Usuario).OrderByDescending(p => p.DataPagamento).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoDataNaoPagoTodos(DateTime dataIni, DateTime dataFim, string atribuicao)
        {
            try
            {
                List<Ato> ListarAtoData = new List<Ato>();

                ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == atribuicao && p.Pago == false && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoDataAto(DateTime dataIni, DateTime dataFim, string atribuicao)
        {
            try
            {
                List<Ato> ListarAtoData = new List<Ato>();



                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.Atoes.Where(p => p.DataAto >= dataIni && p.DataAto <= dataFim && p.Atribuicao == "NOTAS").OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "BALCÃO")
                    ListarAtoData = contexto.Atoes.Where(p => p.DataAto >= dataIni && p.DataAto <= dataFim && p.Atribuicao == "BALCÃO").OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                if (atribuicao == "PROTESTO")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoDataAto(DateTime dataIni, DateTime dataFim)
        {
            try
            {

                return contexto.Atoes.Where(p => p.DataAto >= dataIni && p.DataAto <= dataFim).OrderByDescending(p => p.DataAto).ToList();

            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoDataNomeEscrevente(DateTime dataIni, DateTime dataFim, string tipoAto, string atribuicao, int idNome)
        {
            try
            {
                List<Ato> ListarAtoData = new List<Ato>();

                if (atribuicao == "NOTAS")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "RGI")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "RGI" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();

                if (atribuicao == "PROTESTO")
                    ListarAtoData = contexto.Atoes.Where(p => p.Atribuicao == "PROTESTO" && p.TipoAto == tipoAto && p.DataPagamento >= dataIni && p.DataPagamento <= dataFim && p.IdUsuario == idNome).OrderByDescending(p => p.DataPagamento).ToList();

                return ListarAtoData;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoLivro(string livro, string tipoAto)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.Livro == livro).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoLivroNomeEscrevente(string livro, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.Livro == livro && p.IdUsuario == IdNome).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<Ato> ListarAtoNumeroAto(int ato, string tipoAto)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.NumeroAto == ato).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoNumeroAtoNomeEscrevente(int ato, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.NumeroAto == ato && p.IdUsuario == IdNome).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }



        public List<Ato> ListarAtoSelo(string letra, int numero, string tipoAto)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.LetraSelo == letra && p.NumeroSelo == numero).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoSeloBalcao(string letra, int numero)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "BALCÃO" && p.LetraSelo == letra && p.NumeroSelo == numero).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }


        public List<Ato> ListarAtoSeloNomeEscrevente(string letra, int numero, string tipoAto, int IdNome)
        {
            try
            {

                return contexto.Atoes.Where(p => p.Atribuicao == "NOTAS" && p.TipoAto == tipoAto && p.LetraSelo == letra && p.NumeroSelo == numero && p.IdUsuario == IdNome).OrderByDescending(p => p.DataPagamento).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public ValorPago ObterValorPagoPorIdAto(int idAto)
        {
            return contexto.ValorPagoes.Where(p => p.IdAto == idAto).FirstOrDefault();
        }

        public int ObterProximoIdPagamentoValorPago()
        {
            return Convert.ToInt32(contexto.ValorPagoes.Max(p => p.IdPagamento)) + 1;
        }

        public ValorPago ObterValorPagoPorIdReciboBalcao(int IdReciboBalcao)
        {
            return contexto.ValorPagoes.Where(p => p.IdReciboBalcao == IdReciboBalcao).FirstOrDefault();
        }

        public List<ValorPago> ObterValorPagoPorData(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.ValorPagoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public List<ValorPago> ObterValorPagoPorDataIdPagamento(DateTime data)
        {

            contexto = new CS_CAIXA_DBContext();
            return contexto.ValorPagoes.Where(p => p.Data == data && p.IdPagamento > 0).ToList();
        }


        public int SalvarValorPago(ValorPago valor, string status, string id)
        {
            ValorPago valorPago;
            string addAlterar = "nao";

            if (status == "novo")
                valorPago = new ValorPago();
            else
            {
                if (id == "IdAto")
                    valorPago = contexto.ValorPagoes.Where(p => p.IdAto == valor.IdAto).FirstOrDefault();
                else
                    valorPago = contexto.ValorPagoes.Where(p => p.IdReciboBalcao == valor.IdReciboBalcao).FirstOrDefault();
            }

            if (valorPago == null && status == "alterar")
            {
                valorPago = new ValorPago();
                addAlterar = "sim";
            }

            valorPago.Data = valor.Data;
            valorPago.IdAto = valor.IdAto;
            valorPago.Boleto = valor.Boleto;
            valorPago.Cheque = valor.Cheque;
            valorPago.ChequePre = valor.ChequePre;
            valorPago.Deposito = valor.Deposito;
            valorPago.Dinheiro = valor.Dinheiro;
            valorPago.Mensalista = valor.Mensalista;
            valorPago.IdReciboBalcao = valor.IdReciboBalcao;

            if (valor.CartaoCredito == null)
                valorPago.CartaoCredito = 0M;
            else
                valorPago.CartaoCredito = valor.CartaoCredito;

            if (valor.Troco == null)
                valorPago.Troco = 0M;
            else
                valorPago.Troco = valor.Troco;

            if (valorPago.IdPagamento == null)
                valorPago.IdPagamento = 0;
            else
                valorPago.IdPagamento = valorPago.IdPagamento;

            valorPago.DataModificado = DateTime.Now.ToShortDateString();
            valorPago.HoraModificado = DateTime.Now.ToLongTimeString();
            valorPago.IdUsuario = valor.IdUsuario;
            valorPago.NomeUsuario = valor.NomeUsuario;

            
            valorPago.Total = valorPago.Boleto + valorPago.Cheque + valorPago.ChequePre + valorPago.Deposito + valorPago.Dinheiro + valorPago.Mensalista + valorPago.CartaoCredito;

            if (status == "novo" || addAlterar == "sim")
            {
                contexto.ValorPagoes.Add(valorPago);
            }

            return contexto.SaveChanges();
        }


        public int SalvarAto(Ato atoCorrente, string status)
        {
            Ato atoSalvar;

            if (status == "novo")
            {
                atoSalvar = new Ato();
            }
            else
            {
                atoSalvar = contexto.Atoes.Where(p => p.Id_Ato == atoCorrente.Id_Ato).FirstOrDefault();

            }


            atoSalvar.DataPagamento = atoCorrente.DataPagamento;




            atoSalvar.DataAto = atoCorrente.DataAto;

            atoSalvar.Pago = atoCorrente.Pago;


            atoSalvar.IdUsuario = atoCorrente.IdUsuario;

            atoSalvar.Usuario = atoCorrente.Usuario;


            atoSalvar.Atribuicao = atoCorrente.Atribuicao;

            atoSalvar.LetraSelo = atoCorrente.LetraSelo;

            atoSalvar.NumeroSelo = atoCorrente.NumeroSelo;

            atoSalvar.Aleatorio = atoCorrente.Aleatorio;

            atoSalvar.ValorEscrevente = atoCorrente.ValorEscrevente;

            atoSalvar.ValorAdicionar = atoCorrente.ValorAdicionar;

            atoSalvar.ValorDesconto = atoCorrente.ValorDesconto;

            atoSalvar.Mensalista = atoCorrente.Mensalista;

            atoSalvar.ValorCorretor = atoCorrente.ValorCorretor;

            atoSalvar.ValorTitulo = atoCorrente.ValorTitulo;

            atoSalvar.Portador = atoCorrente.Portador;

            atoSalvar.Convenio = atoCorrente.Convenio;

            atoSalvar.Faixa = atoCorrente.Faixa;


            atoSalvar.Livro = atoCorrente.Livro;

            atoSalvar.FolhaInical = atoCorrente.FolhaInical;

            atoSalvar.FolhaFinal = atoCorrente.FolhaFinal;

            atoSalvar.NumeroAto = atoCorrente.NumeroAto;

            atoSalvar.Recibo = atoCorrente.Recibo;

            atoSalvar.Protocolo = atoCorrente.Protocolo;


            atoSalvar.IdReciboBalcao = atoCorrente.IdReciboBalcao;

            atoSalvar.ReciboBalcao = atoCorrente.ReciboBalcao;


            atoSalvar.TipoAto = atoCorrente.TipoAto;

            atoSalvar.Natureza = atoCorrente.Natureza;

            atoSalvar.Escrevente = atoCorrente.Escrevente;

            atoSalvar.TipoCobranca = atoCorrente.TipoCobranca;

            if (atoSalvar.TipoCobranca == "COM COBRANÇA" || atoSalvar.TipoCobranca == "NIHILL")
                atoSalvar.TipoPagamento = atoCorrente.TipoPagamento;
            else
                atoSalvar.TipoPagamento = atoCorrente.TipoCobranca;

            atoSalvar.Emolumentos = atoCorrente.Emolumentos;

            atoSalvar.Fetj = atoCorrente.Fetj;

            atoSalvar.Fundperj = atoCorrente.Fundperj;

            atoSalvar.Funperj = atoCorrente.Funperj;

            atoSalvar.Funarpen = atoCorrente.Funarpen;

            atoSalvar.Pmcmv = atoCorrente.Pmcmv;

            atoSalvar.Iss = atoCorrente.Iss;

            atoSalvar.Mutua = atoCorrente.Mutua;

            atoSalvar.Acoterj = atoCorrente.Acoterj;

            atoSalvar.Distribuicao = atoCorrente.Distribuicao;

            atoSalvar.Indisponibilidade = atoCorrente.Indisponibilidade;

            atoSalvar.TipoPrenotacao = atoCorrente.TipoPrenotacao;

            atoSalvar.Prenotacao = atoCorrente.Prenotacao;

            atoSalvar.QuantIndisp = atoCorrente.QuantIndisp;

            atoSalvar.QuantDistrib = atoCorrente.QuantDistrib;

            atoSalvar.NumeroRequisicao = atoCorrente.NumeroRequisicao;

            atoSalvar.Bancaria = atoCorrente.Bancaria;

            atoSalvar.ValorPago = atoCorrente.ValorPago;

            atoSalvar.QtdAtos = atoCorrente.QtdAtos;

            atoSalvar.ValorTroco = atoCorrente.ValorTroco;

            atoSalvar.Total = atoCorrente.Total;

            atoSalvar.DescricaoAto = atoCorrente.DescricaoAto;

            atoSalvar.FichaAto = atoCorrente.FichaAto;

            if (status == "novo")
            {
                contexto.Atoes.Add(atoSalvar);
            }

            contexto.SaveChanges();


            return atoSalvar.Id_Ato;

        }

        public void ExcluirAtoBalcao(int idReciboBalcao)
        {
            Ato atoExcluir = new Ato();

            var atos = new List<Ato>();
            atos = contexto.Atoes.Where(p => p.IdReciboBalcao == idReciboBalcao).ToList();

            for (int i = 0; i < atos.Count; i++)
            {

                int id = atos[i].Id_Ato;

                var excluir = contexto.Atoes.Where(p => p.Id_Ato == id).FirstOrDefault();

                atoExcluir = excluir;
                contexto.Atoes.Remove(atoExcluir);
                contexto.SaveChanges();
            }

        }

        public string ExcluirAto(int id, string atribuicao)
        {

            try
            {
                if (atribuicao == "NOTAS")
                {
                    List<ItensCustasNota> itens = new List<ItensCustasNota>();


                    var ItensExcluir = contexto.ItensCustasNotas.Where(p => p.Id_Ato == id).ToList();

                    itens = ItensExcluir;

                    for (int cont = 0; cont <= itens.Count - 1; cont++)
                    {
                        contexto.ItensCustasNotas.Remove(itens[cont]);
                        contexto.SaveChanges();
                    }

                    var enotariado = contexto.Enotariadoes.Where(p => p.IdAto == id).ToList();

                    foreach (var item in enotariado)
                    {
                        contexto.Enotariadoes.Remove(item);
                        contexto.SaveChanges();
                    }


                    ValorPago valorPago = new ValorPago();
                    valorPago = contexto.ValorPagoes.Where(p => p.IdAto == id).FirstOrDefault();
                    if (valorPago != null)
                    {
                        contexto.ValorPagoes.Remove(valorPago);
                        contexto.SaveChanges();
                    }

                    Ato atoExcluir = new Ato();
                    var excluir = contexto.Atoes.Where(p => p.Id_Ato == id);

                    atoExcluir = excluir.FirstOrDefault();
                    contexto.Atoes.Remove(atoExcluir);
                    contexto.SaveChanges();


                    return "Exclusão realizada com sucesso.";
                }

                if (atribuicao == "PROTESTO")
                {
                    List<ItensCustasProtesto> itens = new List<ItensCustasProtesto>();


                    var ItensExcluir = contexto.ItensCustasProtestoes.Where(p => p.Id_Ato == id).ToList();

                    itens = ItensExcluir;

                    for (int cont = 0; cont <= itens.Count - 1; cont++)
                    {
                        contexto.ItensCustasProtestoes.Remove(itens[cont]);
                        contexto.SaveChanges();
                    }

                    ValorPago valorPago = new ValorPago();
                    valorPago = contexto.ValorPagoes.Where(p => p.IdAto == id).FirstOrDefault();
                    if (valorPago != null)
                    {
                        contexto.ValorPagoes.Remove(valorPago);
                        contexto.SaveChanges();
                    }

                    Ato atoExcluir = new Ato();
                    var excluir = contexto.Atoes.Where(p => p.Id_Ato == id);

                    atoExcluir = excluir.FirstOrDefault();
                    contexto.Atoes.Remove(atoExcluir);
                    contexto.SaveChanges();


                    return "Exclusão realizada com sucesso.";
                }
                if (atribuicao == "RGI")
                {
                    var id_AtoRgi = contexto.ItensAtoRgis.Where(p => p.Id_Ato == id).ToList();

                    for (int cont = 0; cont < id_AtoRgi.Count; cont++)
                    {
                        var excluirItens = contexto.ItensCustasRgis.Where(p => p.Id_Ato == id).ToList();

                        for (int i = 0; i < excluirItens.Count; i++)
                        {
                            contexto.ItensCustasRgis.Remove(excluirItens[i]);
                            contexto.SaveChanges();
                        }

                    }

                    for (int i = 0; i < id_AtoRgi.Count; i++)
                    {
                        contexto.ItensAtoRgis.Remove(id_AtoRgi[i]);
                        contexto.SaveChanges();
                    }

                    ValorPago valorPago = new ValorPago();
                    valorPago = contexto.ValorPagoes.Where(p => p.IdAto == id).FirstOrDefault();
                    if (valorPago != null)
                    {
                        contexto.ValorPagoes.Remove(valorPago);
                        contexto.SaveChanges();
                    }

                    Ato atoExcluir = new Ato();
                    var excluir = contexto.Atoes.Where(p => p.Id_Ato == id);

                    atoExcluir = excluir.FirstOrDefault();
                    contexto.Atoes.Remove(atoExcluir);
                    contexto.SaveChanges();


                    return "Exclusão realizada com sucesso.";

                }




                return string.Empty;
            }
            catch (Exception)
            {
                return "Ocorreu um erro inesperado, registro não foi excluído.";
            }
        }


        public Ato ObterAtoPorIdAto(int idAto)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.Atoes.Where(p => p.Id_Ato == idAto).FirstOrDefault();
        }

        public ReciboBalcao ObterReciboBalcaoPorIdAto(int idRecibo)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == idRecibo).FirstOrDefault();
        }




        public int ProximoIdAto()
        {
            contexto = new CS_CAIXA_DBContext();

            var id = contexto.Atoes.Select(p => p).ToList();

            if (id.Count > 0)
            {
                return id.Max(p => p.Id_Ato) + 1;

            }
            else
            {
                return 1;
            }
        }
    }
}
