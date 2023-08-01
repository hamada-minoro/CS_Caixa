using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassIndiceRgi
    {
        CS_CAIXA_DBContext contexto { get; set; }
        public ClassIndiceRgi()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<IndiceRegistro> ListarIndiceRegistroNome(string Nome)
        {
            var retornoLista = new List<IndiceRegistro>();

            try
            {

                retornoLista = contexto.IndiceRegistros.Where(p => p.CpfCnpj == Nome).OrderBy(p => p.Nome).ToList();


                if (retornoLista.Count > 0)
                    return retornoLista;

                retornoLista = contexto.IndiceRegistros.Where(p => p.Nome.Contains(Nome)).OrderBy(p => p.Nome).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        


        public List<IndiceRegistro> ListarIndiceRegistroNomeConsulta(string Nome)
        {
            List<IndiceRegistro> retornoLista = new List<IndiceRegistro>();

            contexto = new CS_CAIXA_DBContext();

            try
            {

                retornoLista = contexto.IndiceRegistros.Where(p => p.Nome.Contains(Nome)).OrderBy(p => p.Nome).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<IndiceRegistro> ListarIndiceRegistroPeriodo(DateTime inicio, DateTime fim)
        {
            List<IndiceRegistro> retornoLista = new List<IndiceRegistro>();

            contexto = new CS_CAIXA_DBContext();

            try
            {

                retornoLista = contexto.IndiceRegistros.Where(p => p.DataRegistro >= inicio && p.DataRegistro <= fim).OrderBy(p => p.Nome).ToList();

               
                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<IndiceRegistro> ListarIndiceRegistroMatricula(string matricula)
        {
            List<IndiceRegistro> retornoLista = new List<IndiceRegistro>();

            contexto = new CS_CAIXA_DBContext();

            try
            {

                retornoLista = contexto.IndiceRegistros.Where(p => p.Ordem == matricula).OrderBy(p => p.Nome).ToList();


                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<IndiceRegistro> ListarIndiceRegistroSalvo(int id)
        {
            List<IndiceRegistro> retornoLista = new List<IndiceRegistro>();

            try
            {
                retornoLista = contexto.IndiceRegistros.Where(p => p.IdIndiceRegistros == id).ToList();

                return retornoLista;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public int SalvarIndiceRegistro(IndiceRegistro SalvarIndiceRegistro, string status)
        {
            try
            {
                IndiceRegistro Salvar;

                if (status == "novo")
                    Salvar = new IndiceRegistro();
                else
                    Salvar = contexto.IndiceRegistros.Where(p => p.IdIndiceRegistros == SalvarIndiceRegistro.IdIndiceRegistros).FirstOrDefault();

                Salvar.Nome = SalvarIndiceRegistro.Nome;
                Salvar.Livro = SalvarIndiceRegistro.Livro;
                Salvar.Numero = SalvarIndiceRegistro.Numero;
                Salvar.Ordem = SalvarIndiceRegistro.Ordem;
                Salvar.Reg = SalvarIndiceRegistro.Reg;
                Salvar.Fls = SalvarIndiceRegistro.Fls;
                Salvar.CpfCnpj = SalvarIndiceRegistro.CpfCnpj;
                Salvar.TipoPessoa = SalvarIndiceRegistro.TipoPessoa;
                Salvar.TipoAto = SalvarIndiceRegistro.TipoAto;
                Salvar.DataRegistro = SalvarIndiceRegistro.DataRegistro;
                Salvar.DataVenda = SalvarIndiceRegistro.DataVenda;
                
                if(Salvar.Enviado != true)
                Salvar.Enviado = SalvarIndiceRegistro.Enviado;


                if (status == "novo")
                    contexto.IndiceRegistros.Add(Salvar);


                contexto.SaveChanges();

                return Salvar.IdIndiceRegistros;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Excluir(IndiceRegistro ExcluirIndiceRegistro)
        {
            try
            {

                IndiceRegistro Excluir = new IndiceRegistro();
                Excluir = contexto.IndiceRegistros.Where(p => p.IdIndiceRegistros == ExcluirIndiceRegistro.IdIndiceRegistros).FirstOrDefault();
                contexto.IndiceRegistros.Remove(Excluir);

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

       public List<IndiceRegistro> TirarPontosMatricula(List<IndiceRegistro> matriculas)
        {

            var matriculasAlterar = new List<IndiceRegistro>();
           
            for (int i = 0; i < matriculas.Count; i++)
            {
                if (matriculas[i].Ordem.Contains(".") || matriculas[i].Ordem.Contains("/") || matriculas[i].Ordem.Contains("-"))
                {
                    matriculas[i].Ordem = matriculas[i].Ordem.Replace(".", "").Replace("/", "").Replace("-", "");
                    matriculasAlterar.Add(matriculas[i]);
                }
            }


            return matriculasAlterar;
        }


       public static string VerificarPastaImagem(int matricula)
       {

           if (matricula >= 0 && matricula < 1000)
               return "00000000";

           if (matricula >= 1000 && matricula < 2000)
               return "00000001";

           if (matricula >= 2000 && matricula < 3000)
               return "00000002";

           if (matricula >= 3000 && matricula < 4000)
               return "00000003";

           if (matricula >= 4000 && matricula < 5000)
               return "00000004";

           if (matricula >= 5000 && matricula < 6000)
               return "00000005";

           if (matricula >= 6000 && matricula < 7000)
               return "00000006";

           if (matricula >= 7000 && matricula < 8000)
               return "00000007";

           if (matricula >= 8000 && matricula < 9000)
               return "00000008";

           if (matricula >= 9000 && matricula < 10000)
               return "00000009";

           if (matricula >= 10000 && matricula < 11000)
               return "00000010";

           if (matricula >= 11000 && matricula < 12000)
               return "00000011";

           if (matricula >= 12000 && matricula < 13000)
               return "00000012";

           if (matricula >= 13000 && matricula < 14000)
               return "00000013";

           if (matricula >= 14000 && matricula < 15000)
               return "00000014";

           if (matricula >= 15000 && matricula < 16000)
               return "00000015";

           if (matricula >= 16000 && matricula < 17000)
               return "00000016";

           if (matricula >= 17000 && matricula < 18000)
               return "00000017";

           if (matricula >= 18000 && matricula < 19000)
               return "00000018";

           if (matricula >= 19000 && matricula < 20000)
               return "00000019";

           if (matricula >= 20000 && matricula < 21000)
               return "00000020";

           if (matricula >= 21000 && matricula < 22000)
               return "00000021";

           if (matricula >= 22000 && matricula < 23000)
               return "00000022";

           if (matricula >= 23000 && matricula < 24000)
               return "00000023";

           if (matricula >= 24000 && matricula < 25000)
               return "00000024";

           if (matricula >= 25000 && matricula < 26000)
               return "00000025";

           if (matricula >= 26000 && matricula < 27000)
               return "00000026";

           if (matricula >= 27000 && matricula < 28000)
               return "00000027";

           if (matricula >= 28000 && matricula < 29000)
               return "00000028";

           if (matricula >= 29000 && matricula < 30000)
               return "00000029";

           if (matricula >= 30000 && matricula < 31000)
               return "00000030";

           if (matricula >= 31000 && matricula < 32000)
               return "00000031";

           if (matricula >= 32000 && matricula < 33000)
               return "00000032";

           if (matricula >= 33000 && matricula < 34000)
               return "00000033";

           if (matricula >= 34000 && matricula < 35000)
               return "00000034";

           if (matricula >= 35000 && matricula < 36000)
               return "00000035";

           if (matricula >= 36000 && matricula < 37000)
               return "00000036";

           if (matricula >= 37000 && matricula < 38000)
               return "00000037";

           if (matricula >= 38000 && matricula < 39000)
               return "00000038";

           if (matricula >= 39000 && matricula < 40000)
               return "00000039";

           if (matricula >= 40000 && matricula < 41000)
               return "00000040";

           if (matricula >= 41000 && matricula < 42000)
               return "00000041";

           if (matricula >= 42000 && matricula < 43000)
               return "00000042";

           if (matricula >= 43000 && matricula < 44000)
               return "00000043";

           if (matricula >= 44000 && matricula < 45000)
               return "00000044";

           if (matricula >= 45000 && matricula < 46000)
               return "00000045";

           if (matricula >= 46000 && matricula < 47000)
               return "00000046";

           if (matricula >= 47000 && matricula < 48000)
               return "00000047";

           if (matricula >= 48000 && matricula < 49000)
               return "00000048";

           if (matricula >= 49000 && matricula < 50000)
               return "00000049";

          

           if (matricula >= 50000 && matricula < 51000)
               return "00000050";

           if (matricula >= 51000 && matricula < 52000)
               return "00000051";

           if (matricula >= 52000 && matricula < 53000)
               return "00000052";

           if (matricula >= 53000 && matricula < 54000)
               return "00000053";

           if (matricula >= 54000 && matricula < 55000)
               return "00000054";

           if (matricula >= 55000 && matricula < 56000)
               return "00000055";

           if (matricula >= 56000 && matricula < 57000)
               return "00000056";

           if (matricula >= 57000 && matricula < 58000)
               return "00000057";

           if (matricula >= 58000 && matricula < 59000)
               return "00000058";

           if (matricula >= 59000 && matricula < 60000)
               return "00000059";

           if (matricula >= 60000 && matricula < 61000)
               return "00000060";
          
           if (matricula >= 61000 && matricula < 62000)
               return "00000061";

           if (matricula >= 62000 && matricula < 63000)
               return "00000062";

           if (matricula >= 63000 && matricula < 64000)
               return "00000063";

           if (matricula >= 64000 && matricula < 65000)
               return "00000064";

           if (matricula >= 65000 && matricula < 66000)
               return "00000065";




           

           
           if (matricula >= 66000 && matricula < 67000)
               return "00000066";
           if (matricula >= 67000 && matricula < 68000)
               return "00000067";
           if (matricula >= 68000 && matricula < 69000)
               return "00000068";
           if (matricula >= 69000 && matricula < 70000)
               return "00000069";
           if (matricula >= 70000 && matricula < 71000)
               return "00000070";
           if (matricula >= 71000 && matricula < 72000)
               return "00000071";
           if (matricula >= 72000 && matricula < 73000)
               return "00000072";
           if (matricula >= 73000 && matricula < 74000)
               return "00000073";
           if (matricula >= 74000 && matricula < 75000)
               return "00000074";
           if (matricula >= 75000 && matricula < 76000)
               return "00000075";
           if (matricula >= 76000 && matricula < 77000)
               return "00000076";
           if (matricula >= 77000 && matricula < 78000)
               return "00000077";
           if (matricula >= 78000 && matricula < 79000)
               return "00000078";
           if (matricula >= 79000 && matricula < 80000)
               return "00000079";
           if (matricula >= 80000 && matricula < 81000)
               return "00000080";
           if (matricula >= 81000 && matricula < 82000)
               return "00000081";
           if (matricula >= 82000 && matricula < 83000)
               return "00000082";
           if (matricula >= 83000 && matricula < 84000)
               return "00000083";
           if (matricula >= 84000 && matricula < 85000)
               return "00000084";
           if (matricula >= 85000 && matricula < 86000)
               return "00000085";
           if (matricula >= 86000 && matricula < 87000)
               return "00000086";
           if (matricula >= 87000 && matricula < 88000)
               return "00000087";
           if (matricula >= 88000 && matricula < 89000)
               return "00000088";
           if (matricula >= 89000 && matricula < 90000)
               return "00000089";
           if (matricula >= 90000 && matricula < 91000)
               return "00000090";
           if (matricula >= 91000 && matricula < 92000)
               return "00000091";
           if (matricula >= 92000 && matricula < 93000)
               return "00000092";
           if (matricula >= 93000 && matricula < 94000)
               return "00000093";
           if (matricula >= 94000 && matricula < 95000)
               return "00000094";
           if (matricula >= 95000 && matricula < 96000)
               return "00000095";
           if (matricula >= 96000 && matricula < 97000)
               return "00000096";
           if (matricula >= 97000 && matricula < 98000)
               return "00000097";
           if (matricula >= 98000 && matricula < 99000)
               return "00000098";
           if (matricula >= 99000 && matricula < 100000)
               return "00000099";

           return "Maior que 99999";
       }
    }
}
