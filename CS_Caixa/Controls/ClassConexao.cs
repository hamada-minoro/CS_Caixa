using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CS_Caixa.Controls
{
    public class ClassConexao
    {
        TcpClient tcpCliente;
        // A thread que ira enviar a informação para o cliente
        private Thread thrSender;
        private StreamReader srReceptor;
        private StreamWriter swEnviador;
        private string usuarioAtual;
        private string strResposta;

        // O construtor da classe que que toma a conexão TCP
        public ClassConexao(TcpClient tcpCon)
        {
            tcpCliente = tcpCon;
            // A thread que aceita o cliente e espera a mensagem
            thrSender = new Thread(AceitaCliente);
            // A thread chama o método AceitaCliente()
            thrSender.Start();
        }


        public void FechaConexao()
        {
            // Fecha os objetos abertos
            tcpCliente.Close();
            srReceptor.Close();
            swEnviador.Close();

        }

        // Ocorre quando um novo cliente é aceito
        private void AceitaCliente()
        {
            try
            {
                srReceptor = new System.IO.StreamReader(tcpCliente.GetStream());
                swEnviador = new System.IO.StreamWriter(tcpCliente.GetStream());

                // Lê a informação da conta do cliente
                usuarioAtual = srReceptor.ReadLine();

                // temos uma resposta do cliente
                if (usuarioAtual != "")
                {
                    //// Armazena o nome do usuário na hash table
                    //if (Servidor.htUsuarios.Contains(usuarioAtual) == true)
                    //{
                    //    // 0 => significa não conectado
                    //    swEnviador.WriteLine("0|Este nome de usuário já existe.");
                    //    swEnviador.Flush();
                    //    FechaConexao();
                    //    return;
                    //}
                    //else if (usuarioAtual == "Administrator")
                    //{
                    //    // 0 => não conectado
                    //    swEnviador.WriteLine("0|Este nome de usuário é reservado.");
                    //    swEnviador.Flush();
                    //    FechaConexao();
                    //    return;
                    //}
                    //else
                    //{
                    //    // 1 => conectou com sucesso
                    //    swEnviador.WriteLine("1");
                    //    swEnviador.Flush();

                    //    // Inclui o usuário na hash table e inicia a escuta de suas mensagens
                    //    Servidor.IncluiUsuario(tcpCliente, usuarioAtual);
                    //}



                    FechaConexao();
                    ClassServidor.IncluiUsuario(tcpCliente, usuarioAtual);
                    return;
                }
                else
                {
                    FechaConexao();
                    return;
                }
                //
            }
            catch (Exception) { }
            try
            {
                // Continua aguardando por uma mensagem do usuário
                while ((strResposta = srReceptor.ReadLine()) != "")
                {
                    // Se for inválido remove-o
                    if (strResposta == null || strResposta == "fechar")
                    {
                        ClassServidor.RemoveUsuario(tcpCliente);
                        FechaConexao();
                    }
                    else
                    {
                        // envia a mensagem para todos os outros usuários
                        ClassServidor.EnviaMensagem(usuarioAtual, strResposta);


                    }


                }
            }
            catch
            {
                // Se houve um problema com este usuário desconecta-o

                ClassServidor.RemoveUsuario(tcpCliente);

            }


        }
    }
}
