using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassBalcao
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassBalcao()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public int ProximoReciboBalcao()
        {
            var query = contexto.ReciboBalcaos.Where(p => p.Status == "LIVRE").ToList();

            int proximoRecibo = 0;
            int idRecibo = 0;

            if (query.Count > 0)
            {
                proximoRecibo = query.Min(p => p.NumeroRecibo);
                idRecibo = query.Where(p => p.NumeroRecibo == proximoRecibo).Select(p => p.IdReciboBalcao).FirstOrDefault();
                ReservaReciboLivre(idRecibo);
                return idRecibo;
            }
            else
            {
                proximoRecibo = contexto.ReciboBalcaos.Max(p => p.NumeroRecibo) + 1;
                idRecibo = contexto.ReciboBalcaos.Max(p => p.IdReciboBalcao) + 1;
                ReservaProximoRecibo(idRecibo, proximoRecibo);
                return idRecibo;
            }



        }

        public int ProximoNumeroRequisicao(string nomeMensalista)
        {
            try
            {
                ReciboBalcao query = contexto.ReciboBalcaos.Where(p => p.Mensalista == nomeMensalista).OrderByDescending(p => p.NumeroRequisicao).FirstOrDefault();
                int proximoNumero = Convert.ToInt32(query.NumeroRequisicao) + 1;
                return proximoNumero;
            }
            catch (Exception)
            {
                return 0;
            }

        }


        public List<Ato> ListarSelo(DateTime DataSelo)
        {
            return contexto.Atoes.Where(p => p.DataAto == DataSelo).ToList();
        }


        public ReciboBalcao ReciboBalcao(int idRecibo)
        {
            return contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == idRecibo).FirstOrDefault();
        }


        public void ReservaReciboLivre(int idRecibo)
        {
            ReciboBalcao reciboBalcao = new ReciboBalcao();
            reciboBalcao = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == idRecibo).FirstOrDefault();
            reciboBalcao.Status = "RESERVADO";
            contexto.SaveChanges();
        }

        public void ReservaProximoRecibo(int idRecibo, int proximoRecibo)
        {
            ReciboBalcao reciboBalcao = new ReciboBalcao();
            reciboBalcao.IdReciboBalcao = idRecibo;
            reciboBalcao.Status = "RESERVADO";
            reciboBalcao.NumeroRecibo = proximoRecibo;
            reciboBalcao.Pago = false;
            contexto.ReciboBalcaos.Add(reciboBalcao);
            contexto.SaveChanges();

        }

        public void SalvarRecibo(ReciboBalcao reciboBalcao)
        {
            ReciboBalcao ReciboBalcao = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == reciboBalcao.IdReciboBalcao).FirstOrDefault();
            ReciboBalcao.Data = reciboBalcao.Data;
            ReciboBalcao.IdUsuario = reciboBalcao.IdUsuario;
            ReciboBalcao.Usuario = reciboBalcao.Usuario;
            ReciboBalcao.Status = reciboBalcao.Status;
            ReciboBalcao.Pago = reciboBalcao.Pago;
            ReciboBalcao.TipoPagamento = reciboBalcao.TipoPagamento;
            ReciboBalcao.TipoCustas = reciboBalcao.TipoCustas;
            ReciboBalcao.QuantAut = reciboBalcao.QuantAut;
            ReciboBalcao.QuantAbert = reciboBalcao.QuantAbert;
            ReciboBalcao.QuantRecAut = reciboBalcao.QuantRecAut;
            ReciboBalcao.QuantRecSem = reciboBalcao.QuantRecSem;
            ReciboBalcao.QuantMaterializacao = reciboBalcao.QuantMaterializacao;
            ReciboBalcao.QuantCopia = reciboBalcao.QuantCopia;
            ReciboBalcao.ValorAdicionar = reciboBalcao.ValorAdicionar;
            ReciboBalcao.ValorDesconto = reciboBalcao.ValorDesconto;
            ReciboBalcao.Mensalista = reciboBalcao.Mensalista;
            ReciboBalcao.NumeroRequisicao = reciboBalcao.NumeroRequisicao;
            ReciboBalcao.Emolumentos = reciboBalcao.Emolumentos;
            ReciboBalcao.Fetj = reciboBalcao.Fetj;
            ReciboBalcao.Fundperj = reciboBalcao.Fundperj;
            ReciboBalcao.Funperj = reciboBalcao.Funperj;
            ReciboBalcao.Funarpen = reciboBalcao.Funarpen;
            ReciboBalcao.Pmcmv = reciboBalcao.Pmcmv;
            ReciboBalcao.Iss = reciboBalcao.Iss;
            ReciboBalcao.Mutua = reciboBalcao.Mutua;
            ReciboBalcao.Acoterj = reciboBalcao.Acoterj;
            ReciboBalcao.Total = reciboBalcao.Total;
            ReciboBalcao.ValorPago = reciboBalcao.ValorPago;
            ReciboBalcao.ValorTroco = reciboBalcao.ValorTroco;
            contexto.SaveChanges();
        }

        public ValorPago ObterValorPagoPorIdReciboBalcao(int idReciboBalcao)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ValorPagoes.Where(p => p.IdReciboBalcao == idReciboBalcao).FirstOrDefault();
        }



        public List<ReciboBalcao> ListarTodosPorData(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.ReciboBalcaos.Where(p => p.Data >= dataInicio && p.Data <= dataFim && p.Pago == true).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ReciboBalcao> ListarTodosPorDataGeral(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                return contexto.ReciboBalcaos.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void RetornaReciboLivre(ReciboBalcao reciboLivre)
        {

            ReciboBalcao reciboBalcao = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == reciboLivre.IdReciboBalcao).FirstOrDefault();
            reciboBalcao.IdReciboBalcao = reciboLivre.IdReciboBalcao;
            reciboBalcao.Status = "LIVRE";
            reciboBalcao.NumeroRecibo = reciboLivre.NumeroRecibo;
            reciboBalcao.Pago = false;
            contexto.SaveChanges();

        }

        public SeloAtualBalcao LetraSeloAtual()
        {
            return contexto.SeloAtualBalcaos.Select(p => p).FirstOrDefault();
        }

        public void SalvarUltimoSelo(int numero)
        {
            try
            {
                SeloAtualBalcao selo = new SeloAtualBalcao();
                selo = contexto.SeloAtualBalcaos.Select(p => p).FirstOrDefault();
                selo.Numero = numero + 1;
                contexto.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Ato> VerificaSeloExistente(string letra, int numero)
        {
            contexto = new CS_CAIXA_DBContext();

            List<Ato> seloBanco = new List<Ato>();
            seloBanco = contexto.Atoes.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero && p.Atribuicao == "BALCÃO").ToList();

            return seloBanco;
        }

        public List<ReciboBalcao> ListaRecibosBalcaoNomeMensalista(string nomeMensalista)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.Mensalista.Contains(nomeMensalista)).OrderByDescending(p => p.NumeroRequisicao).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoNumeroRecisicao(int numeroRequisicao)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.NumeroRequisicao == numeroRequisicao).OrderByDescending(p => p.NumeroRequisicao).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoData(DateTime data)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.Data == data).OrderByDescending(p => p.NumeroRecibo).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoDataNaoPago(DateTime data)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.Data == data && p.Pago == false).OrderByDescending(p => p.NumeroRecibo).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoDataNomeNaoPago(DateTime data, Usuario usuario)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.Data == data && p.Pago == false && p.IdUsuario == usuario.Id_Usuario).OrderByDescending(p => p.NumeroRecibo).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoDataNomeNaoPagoTodos(DateTime data)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.Data == data && p.Pago == false).OrderByDescending(p => p.NumeroRecibo).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoRecibo(int recibo)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboBalcaos.Where(p => p.NumeroRecibo == recibo).ToList();
        }

        public List<ReciboBalcao> ListaRecibosBalcaoSelo(string letra, int numero)
        {
            List<ReciboBalcao> idRecibo = new List<ReciboBalcao>();
            Ato atoselo = new Ato();
            atoselo = contexto.Atoes.Where(p => p.LetraSelo == letra && p.NumeroSelo == numero && p.Atribuicao == "BALCÃO").FirstOrDefault();

            if (atoselo != null)
            {
                idRecibo = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == atoselo.IdReciboBalcao).ToList();

            }
            else
            {
                idRecibo = null;
            }

            return idRecibo;
        }


        public List<Ato> ListaSelosBalcaoRecibo(ReciboBalcao recibo)
        {
            List<Ato> selos = new List<Ato>();
            selos = contexto.Atoes.Where(p => p.IdReciboBalcao == recibo.IdReciboBalcao).ToList();


            return selos;
        }


        public void MudarTipoPagamento(int numeroRecibo, string tipo)
        {
            List<Ato> atoSelo = new List<Ato>();


            atoSelo = contexto.Atoes.Where(p => p.ReciboBalcao == numeroRecibo).ToList();


            for (int i = 0; i < atoSelo.Count; i++)
            {
                atoSelo[i].TipoPagamento = tipo;

                contexto.SaveChanges();

            }

            var recibo = contexto.ReciboBalcaos.Where(p => p.NumeroRecibo == numeroRecibo).FirstOrDefault();
            recibo.TipoPagamento = tipo;
            contexto.SaveChanges();
            

        }

        public void PagarRecibo(ReciboBalcao reciboBalcao)
        {
            ReciboBalcao recibo = new ReciboBalcao();


            reciboBalcao.Pago = reciboBalcao.Pago;

            recibo = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == reciboBalcao.IdReciboBalcao).FirstOrDefault();

            recibo.Pago = reciboBalcao.Pago;



            contexto.SaveChanges();



        }

        public void PagarSelo(ReciboBalcao reciboBalcao)
        {
            List<Ato> atoSelo = new List<Ato>();


            atoSelo = contexto.Atoes.Where(p => p.IdReciboBalcao == reciboBalcao.IdReciboBalcao).ToList();


            for (int i = 0; i < atoSelo.Count; i++)
            {
                Ato selo = new Ato();

                int id = atoSelo[i].Id_Ato;

                selo = contexto.Atoes.Where(p => p.Id_Ato == id).FirstOrDefault();

                selo.Pago = reciboBalcao.Pago;

                contexto.SaveChanges();

            }

        }

        public void ExcluirRecibo(ReciboBalcao reciboBalcao)
        {
            int idRecibo = reciboBalcao.IdReciboBalcao;

            List<Ato> atoSelo = new List<Ato>();


            atoSelo = contexto.Atoes.Where(p => p.IdReciboBalcao == idRecibo).ToList();


            for (int i = 0; i < atoSelo.Count; i++)
            {
                Ato selo = new Ato();

                int id = atoSelo[i].Id_Ato;

                selo = contexto.Atoes.Where(p => p.Id_Ato == id).FirstOrDefault();

                contexto.Atoes.Remove(selo);

                contexto.SaveChanges();

            }

            ReciboBalcao recibo = new ReciboBalcao();
            recibo = contexto.ReciboBalcaos.Where(p => p.IdReciboBalcao == idRecibo).FirstOrDefault();
            contexto.ReciboBalcaos.Remove(recibo);
            contexto.SaveChanges();

            ValorPago valorPago = new ValorPago();
            valorPago = contexto.ValorPagoes.Where(p => p.IdReciboBalcao == idRecibo).FirstOrDefault();
            if (valorPago != null)
            {
                contexto.ValorPagoes.Remove(valorPago);
                contexto.SaveChanges();
            }

        }

        public List<Ato> ListarSelosIdRecibo(int idRecibo)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.Atoes.Where(p => p.IdReciboBalcao == idRecibo).ToList();
        }

        public void RemoverSelosExistentes(int idReciboBalcao)
        {
            List<Ato> listaRemover = new List<Ato>();
            Ato itemRemorer;

            listaRemover = contexto.Atoes.Where(p => p.IdReciboBalcao == idReciboBalcao).ToList();

            for (int i = 0; i < listaRemover.Count; i++)
            {
                itemRemorer = new Ato();

                itemRemorer = listaRemover[i];

                contexto.Atoes.Remove(itemRemorer);
            }

        }

        public void RemoverSeloExistente(int idAto)
        {
            Ato ato = contexto.Atoes.Where(p => p.Id_Ato == idAto).FirstOrDefault();

            contexto.Atoes.Remove(ato);

            contexto.SaveChanges();

        }


        public void AlterarSerieSelo(string selo)
        {
            try
            {
                SeloAtualBalcao seloNovo = new SeloAtualBalcao();
                seloNovo = contexto.SeloAtualBalcaos.Select(p => p).FirstOrDefault();
                seloNovo.Letra = selo;
                contexto.SaveChanges();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public string PegaLetraAtual()
        {
            return contexto.SeloAtualBalcaos.Select(p => p.Letra).FirstOrDefault();
        }



    }
}
