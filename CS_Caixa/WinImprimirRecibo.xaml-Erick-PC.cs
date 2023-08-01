using CS_Caixa.Models;
using CS_Caixa.RelatoriosForms;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
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
using Xceed.Words.NET;

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinImprimirRecibo.xaml
    /// </summary>
    public partial class WinImprimirRecibo : Window
    {
        ReciboBalcao _reciboBalcao;
        List<Ato> _listaSelos;
        System.Windows.Forms.RichTextBox richTextBox = new System.Windows.Forms.RichTextBox();
        string texto = "";

        public WinImprimirRecibo(ReciboBalcao reciboBalcao, List<Ato> listaSelos)
        {
            _reciboBalcao = reciboBalcao;
            _listaSelos = listaSelos;

            InitializeComponent();
        }


        private void btnImprimirRecibo_Click(object sender, RoutedEventArgs e)
        {
            string caminho = @"\\Servidor\c\Cartorio\CS_Sistemas\CS_Caixa\Recibo";

            ImprimirRecibo(_reciboBalcao, _listaSelos, caminho);

            this.Close();
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                this.Close();
        }


        private void CarregarListaDeImpressoras()
        {
            impressoraComboBox.Items.Clear();
            List<string> impressoras = new List<string>();

            foreach (var printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                impressoras.Add(printer.ToString());

            impressoraComboBox.ItemsSource = impressoras;
            impressoraComboBox.SelectedItem = impressoras.Where(p => p.Contains("MP") || p.Contains("mp")).FirstOrDefault();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarListaDeImpressoras();

            foreach (var item in _listaSelos)
            {
                item.Checked = true;
            }           

           
            txtCpfCnpj.Focus();
        }


        void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            var printDocument = sender as System.Drawing.Printing.PrintDocument;

            if (printDocument != null)
            {
                if (printDocument != null)
                {
                    using (var font = new System.Drawing.Font("Times New Roman", 9))
                    using (var brush = new SolidBrush(System.Drawing.Color.Black))
                    {
                        e.Graphics.DrawString(
                            texto,
                            font,
                            brush,
                            new RectangleF(0, 0, printDocument.DefaultPageSettings.PrintableArea.Width, printDocument.DefaultPageSettings.PrintableArea.Height));
                    }
                }
            }
        }


        private void AlterarDadosRecibo(string parametro, string dado)
        {
            System.Text.RegularExpressions.Regex regExpRecibo = new System.Text.RegularExpressions.Regex(parametro);
            foreach (System.Text.RegularExpressions.Match matchRecibo in regExpRecibo.Matches(richTextBox.Text))
            {
                richTextBox.Select(matchRecibo.Index, matchRecibo.Length);
                richTextBox.SelectedText = string.Format("{0}", dado);
            }
        }


        private string CalcularEspacoDecimal(string valor)
        {
            string retorno = valor;
            int tamanho = 8 - retorno.Length;
            string espacoBanco = "";

            for (int i = 0; i < tamanho; i++)
            {
                espacoBanco += " ";
            }

            retorno = espacoBanco += retorno;

            return retorno;
        }

        private void ImprimirRecibo(ReciboBalcao recibo, List<Ato> atos, string caminho)
        {

            try
            {

                richTextBox.LoadFile(@"\\servidor\CS_Sistemas\CS_Caixa\Recibo\Recibo.rtf");
                AlterarDadosRecibo("#recibo", string.Format("{0}", recibo.NumeroRecibo));
                AlterarDadosRecibo("#data", string.Format("{0}", recibo.Data.Value.ToShortDateString()));
                AlterarDadosRecibo("#hora", string.Format("{0}", DateTime.Now.ToLongTimeString()));


                AlterarDadosRecibo("#requerente", string.Format("{0}", txtNome.Text));
                AlterarDadosRecibo("#documento", string.Format("CPF/CNPJ: {0}", txtCpfCnpj.Text));

                List<string> servicos = _listaSelos.Select(p => p.Natureza).Distinct().ToList();
                string servicosRealizados = string.Empty;
                foreach (var item in servicos)
                {
                    servicosRealizados += string.Format("\n{0} : {1}", item, _listaSelos.Where(p => p.Natureza == item).Count());

                }

                if (recibo.QuantCopia > 0)
                    servicosRealizados += string.Format("\n{0} : {1}", "CÓPIA", recibo.QuantCopia);


                AlterarDadosRecibo("#servicos", servicosRealizados);
                AlterarDadosRecibo("#cobranca", string.Format("{0}", recibo.TipoCustas));
                AlterarDadosRecibo("#emol", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Emolumentos)));
                AlterarDadosRecibo("#fetj", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Fetj)));
                AlterarDadosRecibo("#fundperj", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Fundperj)));
                AlterarDadosRecibo("#funperj", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Funperj)));
                AlterarDadosRecibo("#funarpen", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Funarpen)));
                AlterarDadosRecibo("#pmcmv", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Pmcmv)));
                AlterarDadosRecibo("#iss", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Iss)));


                string servicosValores = string.Empty;
                foreach (var item in servicos)
                {
                    servicosValores += string.Format("\n{0} : R$ {1:n2}", item, _listaSelos.Where(p => p.Natureza == item).Sum(p => p.Total));

                }

                if (recibo.QuantCopia > 0)
                    servicosValores += string.Format("\n{0} : R$ {1:n2}", "CÓPIA", recibo.Total - _listaSelos.Sum(p => p.Total));


                AlterarDadosRecibo("#valores", servicosValores);

                AlterarDadosRecibo("#total", CalcularEspacoDecimal(string.Format("{0:n2}", recibo.Total)));

                string tipoPagamento = recibo.TipoPagamento;
                if (tipoPagamento.Contains("PIX"))
                    tipoPagamento = "PIX";

                AlterarDadosRecibo("#pagamento", string.Format("{0}", tipoPagamento));
                AlterarDadosRecibo("#araruama", string.Format("{0}", recibo.Data.Value.ToShortDateString()));
                AlterarDadosRecibo("#funcionario", string.Format("{0}", recibo.Usuario));

                texto = richTextBox.Text;




                using (var printDocument = new System.Drawing.Printing.PrintDocument())
                {
                    printDocument.PrintPage += printDocument_PrintPage;
                    printDocument.PrinterSettings.PrinterName = impressoraComboBox.SelectedItem.ToString();
                    printDocument.Print();
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }



        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void txtCpfCnpj_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            int key = (int)e.Key;
            e.Handled = !(key >= 34 && key <= 43 || key >= 74 && key <= 83 || key == 2 || key == 3 || key >= 23 && key <= 25);

            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

        private void txtCpfCnpj_LostFocus(object sender, RoutedEventArgs e)
        {
            switch (txtCpfCnpj.Text.Length)
            {
                case 11:
                    txtNome.Text = ObterNomePeloDocumento(txtCpfCnpj.Text);
                    FormatarCPF(txtCpfCnpj.Text);
                    break;
                case 14:
                    txtNome.Text = ObterNomePeloDocumento(txtCpfCnpj.Text);
                    FormatarCNPJ(txtCpfCnpj.Text);
                    break;
                default:

                    break;
            }
        }

        private string ObterNomePeloDocumento(string documento)
        {
            string nome = "";

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string comando = string.Format("select nome from clientes where CNPJ = '{0}'", documento);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {
                        if(dr.HasRows)
                        nome = dr["NOME"].ToString();
                    }
                }

               
            }

            if(nome == "")
            using (FbConnection conn2 = new FbConnection(Properties.Settings.Default.SettingBalcaoSite))
            {

                string comando = string.Format("select nome from fichas where CPF = '{0}'", documento);
                conn2.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn2))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                            nome = dr["NOME"].ToString();
                    }
                }


            }

            return nome;
        }

        private void FormatarCPF(string cpf)
        {

            string cpfFormatado = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                    cpfFormatado = cpf.Substring(0, 3) + ".";
                if (i == 1)
                    cpfFormatado += cpf.Substring(3, 3) + ".";
                if (i == 2)
                    cpfFormatado += cpf.Substring(6, 3) + "-";

                if (i == 3)
                    cpfFormatado += cpf.Substring(9, 2);
            }


            txtCpfCnpj.Text = cpfFormatado;
        }


        private void FormatarCNPJ(string cnpj)
        {
            string cnpjFormatado = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                    cnpjFormatado = cnpj.Substring(0, 2) + ".";
                if (i == 1)
                    cnpjFormatado += cnpj.Substring(2, 3) + ".";
                if (i == 2)
                    cnpjFormatado += cnpj.Substring(5, 3) + "/";
                if (i == 3)
                    cnpjFormatado += cnpj.Substring(8, 4) + "-";
                if (i == 4)
                    cnpjFormatado += cnpj.Substring(12, 2);
            }
                        
            txtCpfCnpj.Text = cnpjFormatado;
        }

        private void txtCpfCnpj_GotFocus(object sender, RoutedEventArgs e)
        {
            txtCpfCnpj.Text = txtCpfCnpj.Text.Replace(".", "").Replace("-", "").Replace("/", "");
        }

        private void txtNome_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(
                new TraversalRequest(
                FocusNavigationDirection.Next));

            }
        }

       

        
    }
}
