using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeControleAtos.xaml
    /// </summary>
    public partial class WinAguardeControleAtos : Window
    {

        DateTime dataInicio;
        DateTime dataFim;
        private WinControleAtosNotas winControleAtosNotas;
        List<ControleAto> listaAtosExcluir = new List<ControleAto>();
        List<Ato> listaAtosSincronizarNotas = new List<Ato>();
        List<Ato> listaAtosSincronizarBalcao = new List<Ato>();
        ClassControleAto controleAtos = new ClassControleAto();
        
        ClassAto ato = new ClassAto();
        string acao = string.Empty;

        BackgroundWorker worker;
        public WinAguardeControleAtos()
        {
            InitializeComponent();
        }

        public WinAguardeControleAtos(WinControleAtosNotas winControleAtosNotas, DateTime dataInicio, DateTime dataFim)
        {


            // TODO: Complete member initialization
            this.winControleAtosNotas = winControleAtosNotas;
            this.dataInicio = dataInicio;
            this.dataFim = dataFim;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label2.Content = "Carregando Informações...";

            listaAtosExcluir = controleAtos.ListarAtoData(dataInicio, dataFim, "", "NOTAS");

            listaAtosSincronizarNotas = ato.ListarAtoDataAto(dataInicio, dataFim, "NOTAS");

            listaAtosSincronizarBalcao = ato.ListarAtoDataAto(dataInicio, dataFim, "BALCÃO");

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            Processo();
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;

            if (acao == "removendo")
            {
                progressBar1.Maximum = listaAtosExcluir.Count;
                label2.Content = "Removendo Registros Existentes.";
            }
            else
            {
                if (acao == "Sincronizando Notas.")
                {
                    progressBar1.Maximum = listaAtosSincronizarNotas.Count;
                    label2.Content = acao;
                }

                if (acao == "Sincronizando Balcão.")
                {
                    progressBar1.Maximum = listaAtosSincronizarBalcao.Count;
                    label2.Content = acao;
                }
            }


        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {



            winControleAtosNotas.cmbTpConsulta.SelectedIndex = 0;

            winControleAtosNotas.datePickerdataConsulta.Visibility = Visibility.Visible;
            winControleAtosNotas.datePickerdataConsultaFim.Visibility = Visibility.Visible;
            winControleAtosNotas.txtConsulta.Visibility = Visibility.Hidden;

            winControleAtosNotas.datePickerdataConsulta.SelectedDate = dataInicio;

            winControleAtosNotas.datePickerdataConsultaFim.SelectedDate = dataFim;
            
            winControleAtosNotas.ProcConsultar();

            this.Close();
        }

        private void Processo()
        {
            try
            {
                RemoverRegistrosExistentes();

                SincronizarNotas();

                SincronizarBalcao();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro inesperado. " + ex.Message);
                this.Close();
            }
        }


        private void RemoverRegistrosExistentes()
        {
            acao = "removendo";

            for (int i = 0; i < listaAtosExcluir.Count; i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);
                
                controleAtos.ExcluirAto(listaAtosExcluir[i].Id_ControleAtos, "NOTAS");

            }
        }


        private void SincronizarNotas()
        {
            acao = "Sincronizando Notas.";

            ClassCustasNotas custasNotas;
            ControleAto controleAto;

            ItensCustasControleAtosNota custasControleNotas;

            for (int i = 0; i < listaAtosSincronizarNotas.Count; i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);     

                controleAto = new ControleAto();

                controleAto.Id_Ato = listaAtosSincronizarNotas[i].Id_Ato;
                controleAto.DataAto = listaAtosSincronizarNotas[i].DataAto;
                controleAto.IdUsuario = listaAtosSincronizarNotas[i].IdUsuario;
                controleAto.Usuario = listaAtosSincronizarNotas[i].Usuario;
                controleAto.Atribuicao = listaAtosSincronizarNotas[i].Atribuicao;
                controleAto.LetraSelo = listaAtosSincronizarNotas[i].LetraSelo;
                controleAto.NumeroSelo = listaAtosSincronizarNotas[i].NumeroSelo;
                controleAto.Convenio = listaAtosSincronizarNotas[i].Convenio;
                controleAto.Livro = listaAtosSincronizarNotas[i].Livro;
                controleAto.FolhaInical = listaAtosSincronizarNotas[i].FolhaInical;
                controleAto.FolhaFinal = listaAtosSincronizarNotas[i].FolhaFinal;
                controleAto.NumeroAto = listaAtosSincronizarNotas[i].NumeroAto;
                controleAto.Recibo = listaAtosSincronizarNotas[i].Recibo;
                controleAto.Protocolo = listaAtosSincronizarNotas[i].Protocolo;
                controleAto.ReciboBalcao = listaAtosSincronizarNotas[i].ReciboBalcao;

                if (listaAtosSincronizarNotas[i].TipoCobranca == "JUSTIÇA GRATUITA")
                {
                    controleAto.AtoGratuito = 1;
                    controleAto.AtoNaoGratuito = 0;
                }
                else
                {
                    controleAto.AtoGratuito = 0;
                    controleAto.AtoNaoGratuito = 1;
                }


                controleAto.Faixa = listaAtosSincronizarNotas[i].Faixa;
                controleAto.Natureza = listaAtosSincronizarNotas[i].Natureza;

                if (listaAtosSincronizarNotas[i].TipoAto == "ESCRITURA")
                {
                    controleAto.Faixa = listaAtosSincronizarNotas[i].ValorTitulo.ToString();
                    controleAto.Natureza = listaAtosSincronizarNotas[i].Faixa;
                }
               
                controleAto.TipoAto = listaAtosSincronizarNotas[i].TipoAto;
                controleAto.Emolumentos = listaAtosSincronizarNotas[i].Emolumentos;
                controleAto.Fetj = listaAtosSincronizarNotas[i].Fetj;
                controleAto.Fundperj = listaAtosSincronizarNotas[i].Fundperj;
                controleAto.Funperj = listaAtosSincronizarNotas[i].Funperj;
                controleAto.Funarpen = listaAtosSincronizarNotas[i].Funarpen;
                controleAto.Pmcmv = listaAtosSincronizarNotas[i].Pmcmv;
                controleAto.Iss = listaAtosSincronizarNotas[i].Iss;
                controleAto.Mutua = listaAtosSincronizarNotas[i].Mutua;
                controleAto.Acoterj = listaAtosSincronizarNotas[i].Acoterj;
                controleAto.QtdAtos = listaAtosSincronizarNotas[i].QtdAtos;
                controleAto.Total = listaAtosSincronizarNotas[i].Total;



               int idControleAto =  controleAtos.SalvarAto(controleAto, "novo");

                

                //----------------------------------------------------


                //----------------- Salvar ItensCustasControleAtos ----------------------
                custasNotas = new ClassCustasNotas();

                List<ItensCustasNota> custasAto =  custasNotas.ListarItensCustas(listaAtosSincronizarNotas[i].Id_Ato);


                for (int p = 0; p < custasAto.Count; p++)
                {
                    custasControleNotas = new ItensCustasControleAtosNota();

                    custasControleNotas.Id_ControleAto = idControleAto;
                    custasControleNotas.Tabela = custasAto[p].Tabela;
                    custasControleNotas.Item = custasAto[p].Item;
                    custasControleNotas.SubItem = custasAto[p].SubItem;
                    custasControleNotas.Quantidade = custasAto[p].Quantidade;
                    custasControleNotas.Valor = custasAto[p].Valor;
                    custasControleNotas.Total = custasAto[p].Total;
                    custasControleNotas.Descricao = custasAto[p].Descricao;

                    custasNotas.SalvarItensListaControleAtos(custasControleNotas);
                }



            }

        }

        private void SincronizarBalcao()
        {
            acao = "Sincronizando Balcão.";


            ClassCustasNotas custasNotas = new ClassCustasNotas();
            List<CustasNota> valorCustasNotas = custasNotas.ListaCustas();

            valorCustasNotas = valorCustasNotas.Where(p => p.ANO == dataInicio.Date.Year).ToList();

            ControleAto controleAto;

            ItensCustasControleAtosNota custasControleNotas;

            for (int i = 0; i < listaAtosSincronizarBalcao.Count; i++)
            {
                Thread.Sleep(1);
                worker.ReportProgress(i);

                controleAto = new ControleAto();

                controleAto.Id_Ato = listaAtosSincronizarBalcao[i].Id_Ato;
                controleAto.DataAto = listaAtosSincronizarBalcao[i].DataAto;
                controleAto.IdUsuario = listaAtosSincronizarBalcao[i].IdUsuario;
                controleAto.Usuario = listaAtosSincronizarBalcao[i].Usuario;
                controleAto.Atribuicao = "NOTAS";
                controleAto.LetraSelo = listaAtosSincronizarBalcao[i].LetraSelo;
                controleAto.NumeroSelo = listaAtosSincronizarBalcao[i].NumeroSelo;
                controleAto.Convenio = listaAtosSincronizarBalcao[i].Convenio;
                controleAto.Faixa = listaAtosSincronizarBalcao[i].Faixa;
                controleAto.Livro = listaAtosSincronizarBalcao[i].Livro;
                controleAto.FolhaInical = listaAtosSincronizarBalcao[i].FolhaInical;
                controleAto.FolhaFinal = listaAtosSincronizarBalcao[i].FolhaFinal;
                controleAto.NumeroAto = listaAtosSincronizarBalcao[i].NumeroAto;
                controleAto.Recibo = listaAtosSincronizarBalcao[i].Recibo;
                controleAto.Protocolo = listaAtosSincronizarBalcao[i].Protocolo;
                controleAto.ReciboBalcao = listaAtosSincronizarBalcao[i].ReciboBalcao;

                if (listaAtosSincronizarBalcao[i].TipoCobranca == "JUSTIÇA GRATUITA")
                {
                    controleAto.AtoGratuito = 1;
                    controleAto.AtoNaoGratuito = 0;
                }
                else
                {
                    controleAto.AtoGratuito = 0;
                    controleAto.AtoNaoGratuito = 1;
                }

                

                controleAto.TipoAto = listaAtosSincronizarBalcao[i].Atribuicao;
                controleAto.Natureza = listaAtosSincronizarBalcao[i].TipoAto;
                controleAto.Emolumentos = listaAtosSincronizarBalcao[i].Emolumentos;
                controleAto.Fetj = listaAtosSincronizarBalcao[i].Fetj;
                controleAto.Fundperj = listaAtosSincronizarBalcao[i].Fundperj;
                controleAto.Funperj = listaAtosSincronizarBalcao[i].Funperj;
                controleAto.Funarpen = listaAtosSincronizarBalcao[i].Funarpen;
                controleAto.Pmcmv = listaAtosSincronizarBalcao[i].Pmcmv;
                controleAto.Iss = listaAtosSincronizarBalcao[i].Iss;
                controleAto.Mutua = listaAtosSincronizarBalcao[i].Mutua;
                controleAto.Acoterj = listaAtosSincronizarBalcao[i].Acoterj;
                controleAto.QtdAtos = listaAtosSincronizarBalcao[i].QtdAtos;
                controleAto.Total = listaAtosSincronizarBalcao[i].Total;



                int idControleAto = controleAtos.SalvarAto(controleAto, "novo");

                List<CustasNota> custasAtual = new List<CustasNota>();




                if (listaAtosSincronizarBalcao[i].TipoAto == "AUTENTICAÇÃO")
                {
                    custasAtual.Add(valorCustasNotas.Where(p => p.DESCR == "AUTENTICAÇÃO POR DOCUMENTO OU PÁGINA").Select(p => p).FirstOrDefault()); 
                }


                if (listaAtosSincronizarBalcao[i].TipoAto == "ABERTURA DE FIRMAS")
                {
                    custasAtual.Add(valorCustasNotas.Where(p => p.DESCR == "ABERTURA DE FIRMA").Select(p => p).FirstOrDefault());
                    custasAtual.Add(valorCustasNotas.Where(p => p.DESCR == "ARQUIVAMENTO/DESARQUIVAMENTO").Select(p => p).FirstOrDefault());
                }

                if (listaAtosSincronizarBalcao[i].TipoAto == "REC AUTENTICIDADE")
                {
                    custasAtual.Add(valorCustasNotas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR AUTENTICIDADE").Select(p => p).FirstOrDefault());
                }

                if (listaAtosSincronizarBalcao[i].TipoAto == "REC SEMELHANÇA")
                {
                    custasAtual.Add(valorCustasNotas.Where(p => p.DESCR == "RECONHECIMENTO DE FIRMA POR SEMELHANÇA OU CHANCELA").Select(p => p).FirstOrDefault());
                }

              

                //----------------- Salvar ItensCustasControleAtos ----------------------
                custasNotas = new ClassCustasNotas();

                for (int p = 0; p < custasAtual.Count; p++)
                {
                    custasControleNotas = new ItensCustasControleAtosNota();

                    custasControleNotas.Id_ControleAto = idControleAto;
                    custasControleNotas.Tabela = custasAtual[p].TAB;
                    custasControleNotas.Item = custasAtual[p].ITEM;
                    custasControleNotas.SubItem = custasAtual[p].SUB;
                    custasControleNotas.Quantidade = "1";
                    custasControleNotas.Valor = custasAtual[p].VALOR;
                    custasControleNotas.Total = custasAtual[p].VALOR;
                    custasControleNotas.Descricao = custasAtual[p].DESCR;

                    custasNotas.SalvarItensListaControleAtos(custasControleNotas);
                }



            } 
        }
    }
}
