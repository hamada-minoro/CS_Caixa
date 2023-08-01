using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassReciboNotas
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassReciboNotas()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public int ProximoReciboNotas()
        {
            var query = contexto.ReciboNotas.Where(p => p.Status == "LIVRE").ToList();

            int proximoRecibo = 0;
            int idRecibo = 0;

            if (query.Count > 0)
            {
                proximoRecibo = query.Min(p => p.Recibo);
                idRecibo = query.Where(p => p.Recibo == proximoRecibo).Select(p => p.ReciboNotasId).FirstOrDefault();
                ReservaReciboLivre(idRecibo);
                return idRecibo;
            }
            else
            {
                proximoRecibo = contexto.ReciboNotas.Max(p => p.Recibo) + 1;
                idRecibo = contexto.ReciboNotas.Max(p => p.ReciboNotasId) + 1;
                ReservaProximoRecibo(idRecibo, proximoRecibo);
                return idRecibo;
            }
        }

        public void ReservaReciboLivre(int idRecibo)
        {
            ReciboNota reciboNotas = new ReciboNota();
            reciboNotas = contexto.ReciboNotas.Where(p => p.ReciboNotasId == idRecibo).FirstOrDefault();
            reciboNotas.Status = "RESERVADO";
            contexto.SaveChanges();
        }

        public void ReservaProximoRecibo(int idRecibo, int proximoRecibo)
        {
            ReciboNota reciboNotas = new ReciboNota();
            reciboNotas.ReciboNotasId = idRecibo;
            reciboNotas.Status = "RESERVADO";
            reciboNotas.Recibo = proximoRecibo;
            contexto.ReciboNotas.Add(reciboNotas);
            contexto.SaveChanges();
        }

        public ReciboNota RetornaReciboLivre(ReciboNota reciboLivre)
        {
            ReciboNota reciboNotas = contexto.ReciboNotas.Where(p => p.ReciboNotasId == reciboLivre.ReciboNotasId).FirstOrDefault();
            reciboNotas.ReciboNotasId = reciboLivre.ReciboNotasId;
            reciboNotas.Status = "LIVRE";
            reciboNotas.Recibo = reciboLivre.Recibo;
            reciboNotas.ApresentanteId = null;
            reciboNotas.AtoId = null;
            reciboNotas.Atribuicao = "NOTAS";
            reciboNotas.Data = null;
            reciboNotas.DataEntrega = null;
            contexto.SaveChanges();
            return reciboNotas;
        }

        public ReciboNota CancelarRecibo(ReciboNota reciboLivre)
        {
            ReciboNota reciboNotas = contexto.ReciboNotas.Where(p => p.ReciboNotasId == reciboLivre.ReciboNotasId).FirstOrDefault();
            reciboNotas.ReciboNotasId = reciboLivre.ReciboNotasId;
            reciboNotas.Status = "CANCELADO";
            reciboNotas.Recibo = reciboLivre.Recibo;
            reciboNotas.ApresentanteId = reciboLivre.ApresentanteId;
            reciboNotas.AtoId = reciboLivre.AtoId;
            reciboNotas.Atribuicao = "NOTAS";
            reciboNotas.Data = reciboLivre.Data;
            reciboNotas.DataEntrega = reciboLivre.DataEntrega;
            contexto.SaveChanges();
            return reciboNotas;
        }

        public ReciboNota ObterReciboNotasPorIdAto(int atoId)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboNotas.Where(p => p.AtoId == atoId && p.Status == "UTILIZADO").FirstOrDefault();
        }

        public ReciboNota ObterReciboNotasPorIdAtoAsNoTracking(int atoId)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboNotas.AsNoTracking().Where(p => p.AtoId == atoId && p.Status == "UTILIZADO").FirstOrDefault();
        }

        public ReciboNota ObterReciboNotasCanceladoPorIdAto(int atoId)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboNotas.Where(p => p.AtoId == atoId && p.Status == "CANCELADO").FirstOrDefault();
        }

        public ReciboNota ObterReciboNotasPorIdReciboNotas(int reciboNotasId)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.ReciboNotas.Where(p => p.ReciboNotasId == reciboNotasId).FirstOrDefault();
        }

        public ReciboNota AlterarReciboNotas(ReciboNota recibo)
        {
            ReciboNota reciboSalvar = contexto.ReciboNotas.Where(p => p.ReciboNotasId == recibo.ReciboNotasId).FirstOrDefault();

            reciboSalvar.ApresentanteId = recibo.ApresentanteId;
            reciboSalvar.AtoId = recibo.AtoId;
            reciboSalvar.Atribuicao = "NOTAS";
            reciboSalvar.Data = recibo.Data;
            reciboSalvar.DataEntrega = recibo.DataEntrega;
            reciboSalvar.Status = "UTILIZADO";

            contexto.SaveChanges();

            return reciboSalvar;
        }



        public List<ReciboNota> ObterTodosPorDataAsNoTracking(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.ReciboNotas.AsNoTracking().Where(p => p.Data >= dataInicio && p.Data <= dataFim).OrderByDescending(p => p.Recibo).ToList();
        }




        public int SalvarParte(Parte parte, string tipoSalvar)
        {
            Parte parteSalvar;

            if (tipoSalvar == "novo")
            {
                parteSalvar = new Parte();
            }
            else
            {
                parteSalvar = contexto.Partes.Where(p => p.ParteId == parte.ParteId).FirstOrDefault();
            }

            parteSalvar.Celular = parte.Celular;
            parteSalvar.Cpf = parte.Cpf;
            parteSalvar.CpfOutorgado = parte.CpfOutorgado;
            parteSalvar.Email = parte.Email;
            parteSalvar.Endereco = parte.Endereco;
            parteSalvar.Nome = parte.Nome;
            parteSalvar.Outorgado = parte.Outorgado;
            parteSalvar.Telefone = parte.Telefone;

            if (tipoSalvar == "novo")
                contexto.Partes.Add(parteSalvar);

            contexto.SaveChanges();

            return parteSalvar.ParteId;
        }

        public Parte ObterPartesPorIdParte(int parteId)
        {
            contexto = new CS_CAIXA_DBContext();
            return contexto.Partes.Where(p => p.ParteId == parteId).FirstOrDefault();
        }

        public Ato ObterAtoPorIdAtoAsNoTracking(int? idAto)
        {
            return (idAto != null ? contexto.Atoes.AsNoTracking().Where(p => p.Id_Ato == idAto).FirstOrDefault() : null);
        }
    }
}
