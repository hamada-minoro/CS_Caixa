using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassIndisponibilidade
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassIndisponibilidade()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<Indisponibilidade> ListarIndisponibilidade(string tipo, string caracteres)
        {
            try
            {
                if (tipo == "NOME")
                    return contexto.Indisponibilidades.Where(p => p.Nome.Contains(caracteres)).Select(p => p).OrderByDescending(p => p.IdIndisponibilidade).ToList();
                else
                    return contexto.Indisponibilidades.Where(p => p.CpfCnpj.Contains(caracteres)).Select(p => p).OrderByDescending(p => p.IdIndisponibilidade).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        

        public bool SalvarIndisp(Indisponibilidade salvarIndisp, string status)
        {
            Indisponibilidade indisponibilidade;
            try
            {
                if (status == "novo" || status == "importar")
                {
                    indisponibilidade = new Indisponibilidade();
                }
                else
                {
                    indisponibilidade = contexto.Indisponibilidades.Where(p => p.IdIndisponibilidade == salvarIndisp.IdIndisponibilidade).FirstOrDefault();
                }

                indisponibilidade.Titulo = salvarIndisp.Titulo;
                indisponibilidade.Nome = salvarIndisp.Nome;
                indisponibilidade.CpfCnpj = salvarIndisp.CpfCnpj;
                indisponibilidade.Oficio = salvarIndisp.Oficio;
                indisponibilidade.Aviso = salvarIndisp.Aviso;
                indisponibilidade.Processo = salvarIndisp.Processo;
                indisponibilidade.Valor = salvarIndisp.Valor;


                if (status == "novo" || status == "importar")
                    contexto.Indisponibilidades.Add(indisponibilidade);

                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
                
            }
        }

        public bool ExcluirIndisp(Indisponibilidade salvarIndisp)
        {
            Indisponibilidade indisponibilidade = new Indisponibilidade();

            bool ok = false;
            try
            {
                indisponibilidade = contexto.Indisponibilidades.Where(p => p.IdIndisponibilidade == salvarIndisp.IdIndisponibilidade).FirstOrDefault();

                contexto.Indisponibilidades.Remove(indisponibilidade);

                contexto.SaveChanges();

                ok = true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ok;
        }
    }
}
