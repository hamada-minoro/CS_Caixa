using CS_Caixa.Agragador;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CS_Caixa.RelatoriosForms
{
    /// <summary>
    /// Lógica interna para WinRelatorioImprimirQrCode.xaml
    /// </summary>
    public partial class WinRelatorioImprimirQrCode : Window
    {
        string _caminhoImagem;
        string _selo;
        int _modelo;
        string _atribuicao;
        int _esquerda;
        AtoPrincipalQrCode _atoSelecionado;
        string _escrevente;
        DateTime _data;
        string _matricula;
        string _recibo;
        string _gravame;
        string _obs;

        TituloProtesto _titulo;

        public WinRelatorioImprimirQrCode(string caminhoImagem, string selo, int modelo, string atribuicao, int esquerda, AtoPrincipalQrCode atoSelecionado, string escrevente, DateTime data, string matricula, string recibo, string gravame, string obs)
        {
            _caminhoImagem = caminhoImagem;
            _selo = selo;
            _modelo = modelo;
            _atribuicao = atribuicao;
            _esquerda = esquerda;
            _atoSelecionado = atoSelecionado;
            _escrevente = escrevente;
            _data = data;
            _matricula = matricula;
            _recibo = recibo;
            _gravame = gravame;
            _obs = obs;
            InitializeComponent();
        }

        public WinRelatorioImprimirQrCode(string caminhoImagem, string selo, int modelo, string atribuicao, int esquerda)
        {
            _caminhoImagem = caminhoImagem;
            _selo = selo;
            _modelo = modelo;
            _atribuicao = atribuicao;
            _esquerda = esquerda;

            InitializeComponent();
        }


        public WinRelatorioImprimirQrCode(string caminhoImagem, string tipo, AtoPrincipalQrCode atoSelecionado)
        {
            _caminhoImagem = caminhoImagem;
            _atribuicao = tipo;
            _atoSelecionado = atoSelecionado;

            InitializeComponent();
        }

        public WinRelatorioImprimirQrCode(string caminhoImagem, TituloProtesto titulo, string escrevente)
        {
            _caminhoImagem = caminhoImagem;
            _atribuicao = "protesto";
            _titulo = titulo;
            _escrevente = escrevente;
            InitializeComponent();
        }





        public static List<string> JustificarTexto(string text, int width)
        {
            string[] palabras = text.Split(' ');
            StringBuilder sb1 = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();
            int length = palabras.Length;
            List<string> resultado = new List<string>();
            for (int i = 0; i < length; i++)
            {
                sb1.AppendFormat("{0} ", palabras[i]);
                if (sb1.ToString().Length > width)
                {
                    resultado.Add(sb2.ToString());
                    sb1 = new StringBuilder();
                    sb2 = new StringBuilder();
                    i--;
                }
                else
                {
                    sb2.AppendFormat("{0} ", palabras[i]);
                }
            }
            resultado.Add(sb2.ToString());

            List<string> resultado2 = new List<string>();
            string temp;

            int index1, index2, salto;
            string target;
            int limite = resultado.Count;
            foreach (var item in resultado)
            {
                target = " ";
                temp = item.ToString().Trim();
                index1 = 0; index2 = 0; salto = 2;

                if (limite <= 1)
                {
                    resultado2.Add(temp);
                    break;
                }
                while (temp.Length <= width)
                {
                    if (temp.IndexOf(target, index2) < 0)
                    {
                        index1 = 0; index2 = 0;
                        target = target + " ";
                        salto++;
                    }
                    index1 = temp.IndexOf(target, index2);
                    temp = temp.Insert(temp.IndexOf(target, index2), " ");
                    index2 = index1 + salto;

                }
                limite--;
                resultado2.Add(temp);
            }
            return resultado2;


        }

        private void ReportViewer_Load(object sender, EventArgs e)
        {
            List<ReportParameter> reportParameter = new List<ReportParameter>();

            _caminhoImagem = string.Format("File://{0}", _caminhoImagem);

            string selos = string.Empty;
                        

            if (_atribuicao == "protesto")
            {
                reportParameter.Add(new ReportParameter("Selo", string.Format("{0}-{1}", _titulo.SELO_REGISTRO, _titulo.ALEATORIO_PROTESTO)));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));
                reportParameter.Add(new ReportParameter("DataCertidao", _titulo.DT_PROTOCOLO.ToShortDateString()));
                reportParameter.Add(new ReportParameter("DataProtocolo", _titulo.DT_REGISTRO.ToShortDateString()));
                reportParameter.Add(new ReportParameter("Protocolo", _titulo.PROTOCOLO.ToString()));
                reportParameter.Add(new ReportParameter("Recibo", _titulo.RECIBO.ToString()));
                reportParameter.Add(new ReportParameter("Livro", _titulo.LIVRO_REGISTRO.ToString()));
                reportParameter.Add(new ReportParameter("Folha", _titulo.FOLHA_REGISTRO));
                reportParameter.Add(new ReportParameter("Funcionario", _escrevente));
                reportParameter.Add(new ReportParameter("Emol", _titulo.EMOLUMENTOS.ToString()));
                reportParameter.Add(new ReportParameter("Fund", _titulo.FUNDPERJ.ToString()));
                reportParameter.Add(new ReportParameter("Funp", _titulo.FUNPERJ.ToString()));
                reportParameter.Add(new ReportParameter("Funarpen", _titulo.FUNARPEN.ToString()));
                reportParameter.Add(new ReportParameter("Pmcmv", _titulo.PMCMV.ToString()));
                reportParameter.Add(new ReportParameter("Iss", _titulo.ISS.ToString()));
                reportParameter.Add(new ReportParameter("Total", _titulo.TOTAL.ToString()));
                reportParameter.Add(new ReportParameter("Fetj", _titulo.FETJ.ToString())); 
                reportParameter.Add(new ReportParameter("Mutua", _titulo.MUTUA.ToString()));
                reportParameter.Add(new ReportParameter("Acoterj", _titulo.ACOTERJ.ToString()));
                reportParameter.Add(new ReportParameter("Distribuicao", _titulo.DISTRIBUICAO.ToString()));
                reportParameter.Add(new ReportParameter("Ano", _titulo.DT_PROTOCOLO.Year.ToString()));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeProtesto.RepQrCodeInteiroTeorProtesto.rdlc";


               reportViewer.LocalReport.EnableExternalImages = true;

                reportViewer.LocalReport.SetParameters(reportParameter);
                reportViewer.SetDisplayMode(DisplayMode.PrintLayout);
                reportViewer.RefreshReport();

                return;
            }


            if (_atribuicao != "prenotacao")
            {

                do
                {
                    if (_selo.Contains("="))
                        selos = selos += string.Format("{0} {1} {2}\n", _selo.Substring(0, 4), _selo.Substring(4, 5), _selo.Substring(9, 3));
                    else
                        selos = selos += string.Format("{0} {1} {2}", _selo.Substring(0, 4), _selo.Substring(4, 5), _selo.Substring(9, 3));


                    string remover;

                    if (_selo.Length >= 13)
                        remover = _selo.Substring(0, 13);
                    else
                        remover = _selo.Substring(0, 12);

                    _selo = _selo.Replace(remover, "");


                }
                while (_selo != "");
            }
            //if (selos.Substring(selos.Count() - 2, 2) == "\n")
            //    selos.Remove(selos.Count() - 2, 2);



            if (_atribuicao == "notas")
            {
                reportParameter.Add(new ReportParameter("Selo", selos));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeNotas.RepQrCodeModeloNotas.rdlc";

            }
            if (_atribuicao == "rgi")
            {
                reportParameter.Add(new ReportParameter("Selo", selos));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeRgi.RepQrCodeModeloRgi.rdlc";
            }

            if (_atribuicao == "prenotacao")
            {
                reportParameter.Add(new ReportParameter("Selo", string.Format("{0}-{1}", _atoSelecionado.Selo, _atoSelecionado.Aleatorio)));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));
                reportParameter.Add(new ReportParameter("Data", _atoSelecionado.Data.ToShortDateString()));
                reportParameter.Add(new ReportParameter("Protocolo", _atoSelecionado.Protocolo));
                reportParameter.Add(new ReportParameter("Livro", _atoSelecionado.Livro));
                reportParameter.Add(new ReportParameter("FolhasInicio", _atoSelecionado.FolhasInicio));
                reportParameter.Add(new ReportParameter("Funcionario", _atoSelecionado.Cerp));
                reportParameter.Add(new ReportParameter("Emol", _atoSelecionado.Emol.ToString()));
                reportParameter.Add(new ReportParameter("Fund", _atoSelecionado.Fund.ToString()));
                reportParameter.Add(new ReportParameter("Funp", _atoSelecionado.Funp.ToString()));
                reportParameter.Add(new ReportParameter("Funarpen", _atoSelecionado.Funarpen.ToString()));
                reportParameter.Add(new ReportParameter("Pmcmv", _atoSelecionado.Pmcmv.ToString()));
                reportParameter.Add(new ReportParameter("Iss", _atoSelecionado.Iss.ToString()));
                reportParameter.Add(new ReportParameter("Total", _atoSelecionado.Total.ToString()));
                reportParameter.Add(new ReportParameter("Fetj", _atoSelecionado.Fetj.ToString()));


                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeRgi.RepQrCodeModeloRgiPrenotacao.rdlc";
            }


            if (_atribuicao == "rgiEtiquetaLivre")
            {
                reportParameter.Add(new ReportParameter("Selo", selos));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));

                string matricula;
                try
                {
                    matricula = string.Format("{0:0000000}", Convert.ToInt32(_matricula));
                }
                catch (Exception)
                {
                    matricula = _matricula;
                }

                if (_matricula == "")
                    matricula = "**********";

                reportParameter.Add(new ReportParameter("Mat", matricula));
                reportParameter.Add(new ReportParameter("Data", _data.ToShortDateString()));
                reportParameter.Add(new ReportParameter("Recibo", _recibo));
                reportParameter.Add(new ReportParameter("Escrevente", _escrevente));
                reportParameter.Add(new ReportParameter("Emol", _atoSelecionado.Emol.ToString()));
                reportParameter.Add(new ReportParameter("Fund", _atoSelecionado.Fund.ToString()));
                reportParameter.Add(new ReportParameter("Funp", _atoSelecionado.Funp.ToString()));
                reportParameter.Add(new ReportParameter("Funarpen", _atoSelecionado.Funarpen.ToString()));
                reportParameter.Add(new ReportParameter("Pmcmv", _atoSelecionado.Pmcmv.ToString()));
                reportParameter.Add(new ReportParameter("Iss", _atoSelecionado.Iss.ToString()));
                reportParameter.Add(new ReportParameter("Total", _atoSelecionado.Total.ToString()));
                reportParameter.Add(new ReportParameter("Fetj", _atoSelecionado.Fetj.ToString()));
                reportParameter.Add(new ReportParameter("Obs", _obs));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeRgi.RepQrCodeEtiquetaOnusRgi.rdlc";
            }

            if (_atribuicao == "rgiEtiquetaGravame")
            {
                reportParameter.Add(new ReportParameter("Selo", selos));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));

                string matricula;
                try
                {
                    matricula = string.Format("{0:0000000}", Convert.ToInt32(_matricula));
                }
                catch (Exception)
                {
                    matricula = _matricula;
                }


                if (_matricula == "")
                    matricula = "**********";


                reportParameter.Add(new ReportParameter("Mat", matricula));
                reportParameter.Add(new ReportParameter("Data", _data.ToShortDateString()));
                reportParameter.Add(new ReportParameter("Recibo", _recibo));
                reportParameter.Add(new ReportParameter("Escrevente", _escrevente));
                reportParameter.Add(new ReportParameter("Emol", _atoSelecionado.Emol.ToString()));
                reportParameter.Add(new ReportParameter("Fund", _atoSelecionado.Fund.ToString()));
                reportParameter.Add(new ReportParameter("Funp", _atoSelecionado.Funp.ToString()));
                reportParameter.Add(new ReportParameter("Funarpen", _atoSelecionado.Funarpen.ToString()));
                reportParameter.Add(new ReportParameter("Pmcmv", _atoSelecionado.Pmcmv.ToString()));
                reportParameter.Add(new ReportParameter("Iss", _atoSelecionado.Iss.ToString()));
                reportParameter.Add(new ReportParameter("Total", _atoSelecionado.Total.ToString()));
                reportParameter.Add(new ReportParameter("Fetj", _atoSelecionado.Fetj.ToString()));
                reportParameter.Add(new ReportParameter("Obs", _obs));

                if (_gravame == "")
                    _gravame = "*-*-*-*-*-*-*-*-";

                reportParameter.Add(new ReportParameter("Gravame", _gravame));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeRgi.RepQrCodeEtiquetaOnusRgiGravame.rdlc";
            }
            if (_atribuicao == "rgiEtiquetaInteiroTeor")
            {
                reportParameter.Add(new ReportParameter("Selo", selos));
                reportParameter.Add(new ReportParameter("Imagem", _caminhoImagem));

                string matricula;
                try
                {
                    matricula = string.Format("{0:0000000}", Convert.ToInt32(_matricula));
                }
                catch (Exception)
                {
                    matricula = _matricula;
                }


                if (_matricula == "")
                    matricula = "**********";


                reportParameter.Add(new ReportParameter("Mat", matricula));
                reportParameter.Add(new ReportParameter("Data", _data.ToShortDateString()));
                reportParameter.Add(new ReportParameter("Recibo", _recibo));
                reportParameter.Add(new ReportParameter("Escrevente", _escrevente));
                reportParameter.Add(new ReportParameter("Emol", _atoSelecionado.Emol.ToString()));
                reportParameter.Add(new ReportParameter("Fund", _atoSelecionado.Fund.ToString()));
                reportParameter.Add(new ReportParameter("Funp", _atoSelecionado.Funp.ToString()));
                reportParameter.Add(new ReportParameter("Funarpen", _atoSelecionado.Funarpen.ToString()));
                reportParameter.Add(new ReportParameter("Pmcmv", _atoSelecionado.Pmcmv.ToString()));
                reportParameter.Add(new ReportParameter("Iss", _atoSelecionado.Iss.ToString()));
                reportParameter.Add(new ReportParameter("Total", _atoSelecionado.Total.ToString()));
                reportParameter.Add(new ReportParameter("Fetj", _atoSelecionado.Fetj.ToString()));
                reportParameter.Add(new ReportParameter("Obs", _obs));

                reportViewer.LocalReport.ReportEmbeddedResource = "CS_Caixa.Relatorios.QrCodeRgi.RepQrCodeEtiquetaOnusRgiInteiroTeor.rdlc";
            }

            reportViewer.LocalReport.EnableExternalImages = true;

            reportViewer.LocalReport.SetParameters(reportParameter);
            reportViewer.SetDisplayMode(DisplayMode.PrintLayout);



            var setup = reportViewer.GetPageSettings();

            int top = 1164;

            int left = 0;

            switch (_modelo)
            {
                case 0:

                    break;

                case 1:
                    top = top - 20;

                    break;
                case 2:
                    top = top - 40;

                    break;
                case 3:
                    top = top - 60;

                    break;

                case 4:
                    top = top - 79;

                    break;

                case 5:
                    top = top - 100;

                    break;

                case 6:
                    top = top - 118;

                    break;

                case 7:
                    top = top - 138;

                    break;

                case 8:
                    top = top - 157;

                    break;

                case 9:
                    top = top - 177;

                    break;
                case 10:
                    top = top - 196;

                    break;

                case 11:
                    top = top - 217;

                    break;

                case 12:
                    top = top - 235;

                    break;

                case 13:
                    top = top - 255;

                    break;

                case 14:
                    top = top - 274;

                    break;

                case 15:
                    top = top - 294;

                    break;

                case 16:
                    top = top - 313;

                    break;

                case 17:
                    top = top - 333;

                    break;
                case 18:
                    top = top - 352;

                    break;

                case 19:
                    top = top - 372;

                    break;
                case 20:
                    top = top - 392;

                    break;
                case 21:
                    top = top - 411;

                    break;

                case 22:
                    top = top - 430;

                    break;
                case 23:
                    top = top - 450;

                    break;
                case 24:
                    top = top - 471;

                    break;
                case 25:
                    top = top - 492;

                    break;

                case 26:
                    top = top - 512;

                    break;
                case 27:
                    top = top - 532;

                    break;
                case 28:
                    top = top - 553;

                    break;
                case 29:
                    top = top - 573;

                    break;
                case 30:
                    top = top - 594;

                    break;
                case 31:
                    top = top - 614;

                    break;
                case 32:
                    top = top - 634;

                    break;
                case 33:
                    top = top - 653;

                    break;
                case 34:
                    top = top - 673;

                    break;

                case 35:
                    top = top - 693;

                    break;

                case 36:
                    top = top - 713;

                    break;
                case 37:
                    top = top - 733;

                    break;
                case 38:
                    top = top - 752;

                    break;
                case 39:
                    top = top - 772;

                    break;
                case 40:
                    top = top - 792;

                    break;
                case 41:
                    top = top - 812;

                    break;
                case 42:
                    top = top - 832;

                    break;
                case 43:
                    top = top - 852;

                    break;
                case 44:
                    top = top - 872;

                    break;
                case 45:
                    top = top - 892;

                    break;
                case 46:
                    top = top - 911;

                    break;
                case 47:
                    top = top - 931;

                    break;
                case 48:
                    top = top - 951;

                    break;
                case 49:
                    top = top - 971;

                    break;
                case 50:
                    top = top - 991;

                    break;

                case 51:
                    top = top - 1011;

                    break;
                case 52:
                    top = top - 1031;

                    break;

                case 53:
                    top = top - 1051;

                    break;
                case 54:
                    top = top - 1071;

                    break;
                case 55:
                    top = top - 1091;

                    break;

                case 56:
                    top = top - 1111;

                    break;
                default:
                    top = 0;
                    break;
            }



            switch (_esquerda)
            {
                case 0: // 2 cm
                    break;

                case 1: // 2,50 cm
                    left = 20;

                    break;
                case 2: // 3 cm
                    left = 40;

                    break;
                case 3: // 3,50 cm
                    left = 60;

                    break;

                case 4: // 4 cm
                    left = 80;

                    break;

                case 5: // 4,50 cm
                    left = 99;

                    break;

                case 6: // 5 cm
                    left = 119;

                    break;

                case 7: // 5,50 cm
                    left = 139;

                    break;

                case 8: // 6 cm
                    left = 159;

                    break;

                case 9: // 6,50 cm
                    left = 181;

                    break;
                case 10: // 7 cm
                    left = 201;

                    break;

                case 11: // 7,50 cm
                    left = 220;

                    break;

                case 12: // 8 cm
                    left = 240;

                    break;

                case 13: // 8,50 cm
                    left = 259;

                    break;

                case 14: // 9 cm
                    left = 279;

                    break;

                case 15: // 9,50 cm
                    left = 299;

                    break;

                case 16: // 10 cm
                    left = 319;

                    break;

                case 17: // 10,50 cm
                    left = 339;

                    break;
                case 18: // 11 cm
                    left = 359;

                    break;

                case 19: // 11,50 cm
                    left = 379;

                    break;
                case 20: // 12 cm
                    left = 397;

                    break;
                case 21: // 12,50 cm
                    left = 416;

                    break;

                case 22: // 13 cm
                    left = 436;

                    break;
                case 23:  // 13,50 cm
                    left = 456;

                    break;
                case 24: // 14 cm
                    left = 476;

                    break;
                case 25: // 14,50 cm
                    left = 496;

                    break;


                default:

                    break;

            }


            setup.Margins = new System.Drawing.Printing.Margins(left, 0, top, 0);


            reportViewer.SetPageSettings(setup);


            reportViewer.RefreshReport();
        }



    }

}
