using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{

    public class ClassIndiceEscritura
    {
        CS_CAIXA_DBContext contexto { get; set; }
        public ClassIndiceEscritura()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<IndiceEscritura> ListarIndiceEscrituraNome(string Nome, string tipoParte)
        {
            List<IndiceEscritura> retornoLista = new List<IndiceEscritura>();

            try
            {
                if (tipoParte == "OUTORGANTE")
                    retornoLista = contexto.IndiceEscrituras.Where(p => p.Outorgante.Contains(Nome)).OrderBy(p => p.Outorgante).ToList();

                if (tipoParte == "OUTORGADO")
                    retornoLista = contexto.IndiceEscrituras.Where(p => p.Outorgado.Contains(Nome)).OrderBy(p => p.Outorgado).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public List<IndiceEscritura> ListarIndiceEscrituraSalvo(int id)
        {
            List<IndiceEscritura> retornoLista = new List<IndiceEscritura>();

            try
            {
                retornoLista = contexto.IndiceEscrituras.Where(p => p.IdIndiceEscritura == id).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }


        public int SalvarIndiceEscritura(IndiceEscritura salvarIndiceEscritura, string status)
        {
            try
            {
                IndiceEscritura Salvar;

                if (status == "novo")
                    Salvar = new IndiceEscritura();
                else
                    Salvar = contexto.IndiceEscrituras.Where(p => p.IdIndiceEscritura == salvarIndiceEscritura.IdIndiceEscritura).FirstOrDefault();

                Salvar.Outorgante = salvarIndiceEscritura.Outorgante;
                Salvar.Outorgado = salvarIndiceEscritura.Outorgado;
                Salvar.Ordem = salvarIndiceEscritura.Ordem;
                Salvar.Natureza = salvarIndiceEscritura.Natureza;
                Salvar.MesDist = salvarIndiceEscritura.MesDist;
                Salvar.Mes = salvarIndiceEscritura.Mes;
                Salvar.Livro = salvarIndiceEscritura.Livro;
                Salvar.Fls = salvarIndiceEscritura.Fls;
                Salvar.DiaDist = salvarIndiceEscritura.DiaDist;
                Salvar.Dia = salvarIndiceEscritura.Dia;
                Salvar.Ato = salvarIndiceEscritura.Ato;
                Salvar.AnoDist = salvarIndiceEscritura.AnoDist;
                Salvar.Ano = salvarIndiceEscritura.Ano;

                if (status == "novo")
                    contexto.IndiceEscrituras.Add(Salvar);


                contexto.SaveChanges();

                return Salvar.IdIndiceEscritura;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool Excluir(IndiceEscritura ExcluirIndiceEscritura)
        {
            try
            {

                IndiceEscritura Excluir = new IndiceEscritura();
                Excluir = contexto.IndiceEscrituras.Where(p => p.IdIndiceEscritura == ExcluirIndiceEscritura.IdIndiceEscritura).FirstOrDefault();
                contexto.IndiceEscrituras.Remove(Excluir);

                contexto.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static string RemoveAcentos(string nome)
        {
            string Nome = nome;

            Nome = Nome.Replace("Ã", "A").Replace("Â", "A").Replace("Á", "A").Replace("À", "A").Replace("Ç", "C").Replace("É", "E").Replace("Ê", "E").Replace("Ô", "O").Replace("Ó", "O").Replace("Í", "I").Replace("Ú", "U");

            Nome = Nome.Trim();

            return Nome;
        }

    }
}
