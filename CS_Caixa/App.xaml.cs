using CS_Caixa.Controls;
using CS_Caixa.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Xml;

namespace CS_Caixa
{
    /// <summary>
    /// Interação lógica para App.xaml
    /// </summary>
    public partial class App : Application
    {       

        public App()
        {

            string verificarSite = Verificacao();


            if (verificarSite == "S")
            {
                BloquearCaixa();                
            }

            if (verificarSite == "N")
            {
                DesbloquearCaixa();
            }


            if (VerificacaoCaixa() == "S")
            {
                MessageBox.Show("Erro #0045FD. O Windows será desligado.", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                Application.Current.Shutdown();
            }

        }

        private string Verificacao()
        {
            try
            {
                string result = string.Empty;

                MySqlConnection con = new MySqlConnection("server=cartorio1oficio.mysql.uhserver.com;database=cartorio1oficio;uid=erick1;password=Erick@6477");
                con.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT StatusVerificacao FROM VerificacaoCaixa WHERE VerificacaoCaixaId = 1", con);

                cmd.CommandType = CommandType.Text;

                result = ClassCriptografia.Decrypt(cmd.ExecuteScalar().ToString());

                con.Close();

                return result;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }


        private string VerificacaoCaixa()
        {
            try
            {
                string result = string.Empty;

                SqlConnection con = new SqlConnection("Data Source=servidor;Initial Catalog=CS_CAIXA_DB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd");
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT Valor FROM VerificacaoCaixa WHERE VerificacaoCaixaId = 1", con);

                cmd.CommandType = CommandType.Text;

                result = ClassCriptografia.Decrypt(cmd.ExecuteScalar().ToString());

                con.Close();

                return result;

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private void BloquearCaixa()
        {
            try
            {

                string valor = ClassCriptografia.Encrypt("S");

                string result = string.Empty;

                SqlConnection con = new SqlConnection("Data Source=servidor;Initial Catalog=CS_CAIXA_DB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd");
                con.Open();

                SqlCommand cmd = new SqlCommand("update VerificacaoCaixa set Status = 'S', Valor = '" + valor + "' where VerificacaoCaixaId = 1", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception)
            {
            }
        }

        private void DesbloquearCaixa()
        {
            try
            {
                string valor = ClassCriptografia.Encrypt("N");

                string result = string.Empty;

                SqlConnection con = new SqlConnection("Data Source=servidor;Initial Catalog=CS_CAIXA_DB;Persist Security Info=True;User ID=sa;Password=P@$$w0rd");
                con.Open();

                SqlCommand cmd = new SqlCommand("update VerificacaoCaixa set Status = 'N', Valor = '" + valor + "' where VerificacaoCaixaId = 1", con);

                cmd.CommandType = CommandType.Text;

                cmd.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception)
            {
            }
        }
    }



}
