using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassEnotariado
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassEnotariado()
        {
            contexto = new CS_CAIXA_DBContext();
        }


        public Enotariado ObterPorIdAto(int idAto)
        {
            return contexto.Enotariadoes.Where(p => p.IdAto == idAto).FirstOrDefault();
        }

        public Enotariado ObterPorIdEnotariado(int idEnotariado)
        {
            return contexto.Enotariadoes.Where(p => p.IdEnotariado == idEnotariado).FirstOrDefault();
        }

        public List<Enotariado> ObterPorData(DateTime dataInicio, DateTime dataFim)
        {
            return contexto.Enotariadoes.Where(p => p.Data >= dataInicio && p.Data <= dataFim).ToList();
        }

        public void ExcluirEnotariado(int idAto)
        {
            var enotariado = contexto.Enotariadoes.Where(p => p.IdAto == idAto).ToList();


            if (enotariado != null)
                foreach (var item in enotariado)
                {
                    contexto.Enotariadoes.Remove(item);
                    contexto.SaveChanges();
                }
        }

        public int SalvaEnotariado(Enotariado salvar, string status)
        {
            try
            {
                Enotariado salvarEnotariado;

                if (status == "alterar")
                {
                    salvarEnotariado = contexto.Enotariadoes.Where(p => p.IdEnotariado == salvar.IdEnotariado).FirstOrDefault();
                    salvarEnotariado.IdAto = salvar.IdAto;
                    salvarEnotariado.Data = salvar.Data;
                    salvarEnotariado.Valor = salvar.Valor;

                    contexto.SaveChanges();
                }
                else
                {
                    salvarEnotariado = new Enotariado();

                    salvarEnotariado.IdAto = salvar.IdAto;
                    salvarEnotariado.Data = salvar.Data;
                    salvarEnotariado.Valor = salvar.Valor;

                    contexto.Enotariadoes.Add(salvarEnotariado);

                    contexto.SaveChanges();
                }

                return salvarEnotariado.IdEnotariado;
            }
            catch (Exception)
            {
                return 0;
            }

            
        }
    }
}
