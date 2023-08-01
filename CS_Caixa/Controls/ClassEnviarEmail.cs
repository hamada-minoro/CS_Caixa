using CS_Caixa.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CS_Caixa.Controls
{
    public class ClassEnviarEmail
    {
        public static string EnviaMensagemEmail(string Destinatario, string Remetente, string Assunto, string enviaMensagem)
        {
            try
            {

                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(Destinatario);

                // Se o email não é validao retorna uma mensagem
                if (bValidaEmail == false)
                    return "Email do destinatário inválido: " + Destinatario;

                // cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(Remetente, Destinatario, Assunto, enviaMensagem);
                mensagemEmail.IsBodyHtml = true;

                SmtpClient client = new SmtpClient("smtp.1oficioararuama.com.br", 587);
                client.EnableSsl = false;
                NetworkCredential cred = new NetworkCredential("contato@1oficioararuama.com.br", "P@$$w0rd");
                client.Credentials = cred;
                // envia a mensagem
                client.Send(mensagemEmail);

                return "Mensagem enviada para  " + Destinatario + " às " + DateTime.Now.ToString() + ".";


            }
            catch (Exception ex)
            {
                string erro = ex.InnerException.ToString();
                return ex.Message.ToString() + erro;
            }
        }

        public bool EnvioMensagem(string Destinatario, string Remetente, string Assunto, string enviaMensagem)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                //Endereço que irá aparecer no e-mail do usuário 
                mailMessage.From = new MailAddress("contato@1oficioararuama.com.br", "Cartório do 1º Ofício Araruama");
                //destinatarios do e-mail, para incluir mais de um basta separar por ponto e virgula  
                mailMessage.To.Add(Destinatario);

                mailMessage.Subject = Assunto;
                mailMessage.IsBodyHtml = true;
                //conteudo do corpo do e-mail 
                mailMessage.Body = enviaMensagem;
                //mailMessage.Priority = MailPriority.High;
                mailMessage.Priority = MailPriority.Normal;
                mailMessage.BodyEncoding = System.Text.Encoding.GetEncoding("ISO-8859-1");
                //smtp do e-mail que irá enviar 
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mailMessage);

                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }


        public static string EnviaMensagemComAnexos(string Destinatario, string Remetente,
                                string Assunto, string enviaMensagem, ArrayList anexos)
        {
            try
            {
                // valida o email
                bool bValidaEmail = ValidaEnderecoEmail(Destinatario);

                if (bValidaEmail == false)
                    return "Email do destinatário inválido:" + Destinatario;

                // Cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(
                   Remetente, Destinatario, Assunto, enviaMensagem);
                // Obtem os anexos contidos em um arquivo arraylist e inclui na mensagem
                foreach (string anexo in anexos)
                {
                    Attachment anexado = new Attachment(anexo, MediaTypeNames.Application.Octet);
                    mensagemEmail.Attachments.Add(anexado);
                }

                SmtpClient client = new SmtpClient("smtp.1oficioararuama.com.br", 587);
                client.EnableSsl = false;
                NetworkCredential cred = new NetworkCredential("contato@1oficioararuama.com.br", "P@$$w0rd");
                client.Credentials = cred;
                mensagemEmail.IsBodyHtml = true;
                // Inclui as credenciais
                client.UseDefaultCredentials = false;

                // envia a mensagem
                client.Send(mensagemEmail);

                return "Mensagem enviada para " + Destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (Exception ex)
            {
                string erro = ex.InnerException.ToString();
                return ex.Message.ToString() + erro;
            }
        }


        public static bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regulara para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(texto_Validar))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public Email SalvarEmail(Email email)
        {
            CS_CAIXA_DBContext context = new CS_CAIXA_DBContext();

            Email salvarEmail = context.Emails.SingleOrDefault(p => p.Indicador == email.Indicador);


            if (salvarEmail == null)
            {
                salvarEmail = new Email();

                salvarEmail.Data = email.Data;
                salvarEmail.Documento = email.Documento;
                salvarEmail.Enviado = false;
                salvarEmail.Indicador = email.Indicador;
                salvarEmail.Mensagem = email.Mensagem;
                context.Emails.Add(salvarEmail);

                context.SaveChanges();
            }



            return salvarEmail;
        }



        public Email AlterarEmailEnviado(Email email)
        {
            CS_CAIXA_DBContext context = new CS_CAIXA_DBContext();

            Email salvarEmail = context.Emails.SingleOrDefault(p => p.EmailId == email.EmailId);

            if (salvarEmail != null)
            {
                context.Emails.Remove(salvarEmail);
                context.SaveChanges();
            }
            
            salvarEmail = new Email();
            salvarEmail.Data = email.Data;
            salvarEmail.Documento = email.Documento;
            salvarEmail.Enviado = true;
            salvarEmail.Indicador = email.Indicador;
            salvarEmail.Mensagem = email.Mensagem;
            context.Emails.Add(salvarEmail);
            context.SaveChanges();
            
            return salvarEmail;
        }



        public List<Email> ObterEmailsEnviar(string documento)
        {
            CS_CAIXA_DBContext context = new CS_CAIXA_DBContext();

            return context.Emails.Where(p => p.Enviado == false && p.Documento == documento).ToList();
        }


        public string ObterArquivoHtml(string nome, string mensagem)
        {
            string arquivo;

            arquivo = System.IO.File.ReadAllText(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\Email_Atos\" + "Email_Atos.txt");

            arquivo = arquivo.Replace(">nome<", nome).Replace(">mensagem<", mensagem);

            return arquivo;
        }

    }
}
