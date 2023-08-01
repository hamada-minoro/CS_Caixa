using CS_Caixa.Agragador;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
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

namespace CS_Caixa
{
    /// <summary>
    /// Lógica interna para WinEnviarBoleto.xaml
    /// </summary>
    public partial class WinEnviarBoleto : Window
    {

        public System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        List<Boleto> boletos = new List<Boleto>();
        WinPrincipal _principal;
        public WinEnviarBoleto(WinPrincipal principal)
        {
            _principal = principal;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarregarBoletos();
        }


        private void CarregarBoletos()
        {

            boletos = new List<Boleto>();
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select * from Boletos";

                    var reader = comm.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    ConverteBoletos(table);

                    dataGrid1.ItemsSource = boletos.OrderByDescending(p => p.Boleto_id);
                    if (boletos.Count > 0)
                        dataGrid1.SelectedIndex = 0;
                }
            }
        }

        private void ConverteBoletos(DataTable dataTable)
        {

            for (int i = 0; i < dataTable.DefaultView.Count; i++)
            {
                var boleto = new Boleto();

                boleto.Boleto_id = Convert.ToInt32(dataTable.DefaultView[i][0]);
                boleto.Protocolo = dataTable.DefaultView[i][1].ToString();
                boleto.Arquivo = (byte[])dataTable.DefaultView[i][2];
                boleto.data_envio = (DateTime)dataTable.DefaultView[i][3];
                
                bool visu = (bool)dataTable.DefaultView[i][4];

                if (visu == true)
                    boleto.Visualizado = "SIM";
                else
                    boleto.Visualizado = "NÃO";

                boleto.QtdVisualizacao = (int)dataTable.DefaultView[i][5];

                boletos.Add(boleto);
            }

        }

        private bool VerificarExistente()
        {
            bool protocolo = false;

            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "select Protocolo from Boletos where Protocolo = '" + System.IO.Path.GetFileName(txtCaminho.Text).Replace(".pdf", "") + "'";

                    var reader = comm.ExecuteReader();
                    var table = new DataTable();
                    table.Load(reader);
                    if (table.Rows.Count > 0)
                        return true;

                }
            }


            return protocolo;
        }

              

        private void DeletarExistente()
        {


            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "delete from Boletos where Protocolo = '" + System.IO.Path.GetFileName(txtCaminho.Text).Replace(".pdf", "") + "'";

                    comm.ExecuteNonQuery();

                }
            }

        }


        private IDbConnection AbrirConexao()
        {
            return new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
        }

        private void SalvarArquivo(string arquivo)
        {
            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = "INSERT INTO Boletos (Protocolo, Arquivo, data_envio, Visualizado, QtdVisualizacao) VALUES (@Protocolo, @Arquivo, @data_envio, @Visualizado, 0)";
                    ConfigurarParametrosSalvar(comm, arquivo);
                    comm.ExecuteNonQuery();
                }
            }

            using (var conn = AbrirConexao())
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandText = string.Format("update ConsultaApontado set SolicitarBoleto = false where Protocolo = '{0}'", System.IO.Path.GetFileName(arquivo).Replace(".pdf", ""));
                    comm.ExecuteNonQuery();

                }
            }
        }
        private void ConfigurarParametrosSalvar(IDbCommand comm, string arquivo)
        {
            comm.Parameters.Add(new MySqlParameter("Protocolo", System.IO.Path.GetFileName(arquivo).Replace(".pdf", "")));
            comm.Parameters.Add(new MySqlParameter("Arquivo", File.ReadAllBytes(arquivo)));
            comm.Parameters.Add(new MySqlParameter("data_envio", DateTime.Now.Date));
            comm.Parameters.Add(new MySqlParameter("Visualizado", false));
        }

        private void btnSelecionar_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog1.Filter = "All files (*.pdf)|*.pdf";

            openFileDialog1.InitialDirectory = @"\\SERVIDOR\Total\Protesto\BOLETOS";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtCaminho.Text = openFileDialog1.FileName;

                try
                {

                    if (!string.IsNullOrWhiteSpace(txtCaminho.Text))
                    {

                        if (VerificarExistente())
                        {
                            DeletarExistente();
                        }
                                                

                        SalvarArquivo(txtCaminho.Text);

                        CarregarBoletos();

                        FileInfo arquivoEnviado = new FileInfo(openFileDialog1.FileName);

                        _principal.ConsultarSolicitacaoBoletos();
                        
                        MessageBox.Show("Arquivo " + arquivoEnviado.Name.Replace(".pdf", "") + " enviado com sucesso.\n\n Para visualizar acesse: 1oficioararuama.com.br/boleto", "Enviado com Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);

                        
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                txtCaminho.Text = "";
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F5)
                CarregarBoletos();

            if (e.Key == Key.Delete)
            {
                if (dataGrid1.SelectedItem != null)
                {
                    using (var conn = AbrirConexao())
                    {
                        conn.Open();
                        using (var comm = conn.CreateCommand())
                        {

                            Boleto boleto = (Boleto)dataGrid1.SelectedItem;

                            comm.CommandText = "delete from Boletos where Protocolo = '" + boleto.Protocolo + "'";

                            comm.ExecuteNonQuery();

                            CarregarBoletos();
                            MessageBox.Show("Boleto "+ boleto.Protocolo +" excluído com sucesso.", "Excluído", MessageBoxButton.OK, MessageBoxImage.Warning);

                            
                        }
                    }
                }
            }
        }

        private void dataGrid1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid1.SelectedItem != null)
            {

                Boleto selecionado = (Boleto)dataGrid1.SelectedItem;


                byte[] b = selecionado.Arquivo as byte[];

                try
                {
                    string caminho = @"\\SERVIDOR\CS_Sistemas\CS_Caixa\Boletos\" + selecionado.Protocolo + ".pdf";

                    // crio o arquivo na pasta temporaria
                    FileStream fs = new FileStream(caminho, FileMode.Create);
                    //escrevo os bytes no arquivo
                    fs.Write(b, 0, b.Length);                   
                    fs.Dispose();
                    //abro o arquivo salvo na pasta temp
                    Process.Start(caminho);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

      
    }
}
