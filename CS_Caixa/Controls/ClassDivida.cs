using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassDivida
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassDivida()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public List<Divida> ObterDividasPorUsuario(int idUsuario)
        {
            return contexto.Dividas.Where(p => p.Id_Usuario == idUsuario).ToList();
        }

        public List<Parcela> ObterParcelasPendentesPorUsuario(int idUsuario, DateTime data)
        {
            return contexto.Parcelas.Where(p => p.Id_Usuario == idUsuario && p.Data_Vencimento <= data && p.Pago == false).ToList();
        }

        public List<Divida> ObterDividasPendentesPorUsuario(int idUsuario)
        {
            return contexto.Dividas.Where(p => p.Id_Usuario == idUsuario && p.Divida_Paga == false).ToList();
        }

        public List<Divida> ObterDividasQuitadasPorUsuario(int idUsuario)
        {
            return contexto.Dividas.Where(p => p.Id_Usuario == idUsuario && p.Divida_Paga == true).ToList();
        }

        public List<Divida> ObterDividasVitaliciasPorUsuario(int idUsuario)
        {
            return contexto.Dividas.Where(p => p.Id_Usuario == idUsuario && p.Tipo == "VITALÍCIA").ToList();
        }

        public List<Parcela> ObterParcelasPorIdDivida(int IdDivida)
        {
            return contexto.Parcelas.Where(p => p.Id_Divida == IdDivida).ToList();
        }



        public void ExcluirDivida( int id)
        {

            var parcelas = contexto.Parcelas.Where(p => p.Id_Divida == id).ToList();

            foreach (var item in parcelas)
            {
                contexto.Parcelas.Remove(item);
                contexto.SaveChanges();
            }


            var divida = contexto.Dividas.Where(p => p.Id_Divida == id).FirstOrDefault();
            contexto.Dividas.Remove(divida);
            contexto.SaveChanges();
        }

        public Parcela PagarDatarParcela(Parcela parcela)
        {
            var parc = contexto.Parcelas.Where(p => p.Id_Parcela == parcela.Id_Parcela).FirstOrDefault();

            parc.Pago = true;
            parc.Data_Pagamento = DateTime.Now.Date.ToShortDateString();

            contexto.SaveChanges();

            return parc;
        }

        public Parcela DespagarDatarParcela(Parcela parcela)
        {
            var parc = contexto.Parcelas.Where(p => p.Id_Parcela == parcela.Id_Parcela).FirstOrDefault();

            parc.Pago = false;
            parc.Data_Pagamento = "";

            contexto.SaveChanges();

            return parc;
        }

        public int SalvarDivida(Divida divida)
        {
            try
            {
                Divida dividaSalvar;

                if (divida.Id_Divida != 0)
                    dividaSalvar = contexto.Dividas.Where(p => p.Id_Divida == divida.Id_Divida).FirstOrDefault();
                else
                    dividaSalvar = new Divida();

                dividaSalvar.Data = divida.Data;
                dividaSalvar.Data_Fim_Parcela = divida.Data_Fim_Parcela;
                dividaSalvar.Data_Inicio_Parcela = divida.Data_Inicio_Parcela;
                dividaSalvar.Descricao = divida.Descricao;
                dividaSalvar.Dia_Pagamento = divida.Dia_Pagamento;
                dividaSalvar.Divida_Paga = divida.Divida_Paga;
                dividaSalvar.Id_Usuario = divida.Id_Usuario;
                dividaSalvar.Qtd_Parcelas = divida.Qtd_Parcelas;
                dividaSalvar.Qtd_Parcelas_Pagas = divida.Qtd_Parcelas_Pagas;
                dividaSalvar.Tipo = divida.Tipo;
                dividaSalvar.Tipo_Divida = divida.Tipo_Divida;
                dividaSalvar.Valor_Divida = divida.Valor_Divida;
                dividaSalvar.Valor_Pago = divida.Valor_Pago;

                if (divida.Id_Divida == 0)
                    contexto.Dividas.Add(dividaSalvar);


                contexto.SaveChanges();

                return dividaSalvar.Id_Divida;
            }
            catch (Exception)
            {
                return -1;               
            }

           
        }

        public int SalvarParcelas(List<Parcela> paracelas, int idDivida)
        {
            try
            {
                for (int i = 0; i < paracelas.Count; i++)
                {
                    Parcela parc;

                    if (paracelas[i].Id_Parcela != 0)
                        parc = contexto.Parcelas.SingleOrDefault(p => p.Id_Parcela == paracelas[i].Id_Parcela);
                    else
                        parc = new Parcela();


                    parc.Data_Emissao = paracelas[i].Data_Emissao;
                    parc.Data_Pagamento = paracelas[i].Data_Pagamento;
                    parc.Data_Vencimento = paracelas[i].Data_Vencimento;
                    parc.Id_Divida = idDivida;
                    parc.Id_Usuario = paracelas[i].Id_Usuario;
                    parc.Pago = paracelas[i].Pago;
                    parc.Valor = paracelas[i].Valor;

                    if (paracelas[i].Id_Parcela == 0)
                        contexto.Parcelas.Add(parc);

                    contexto.SaveChanges();


                }
                return 1;

               
                    
            }
            catch (Exception)
            {
                return 0;
            }

        }
    }
}
