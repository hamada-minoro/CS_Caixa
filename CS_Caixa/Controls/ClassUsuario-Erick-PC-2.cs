using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassUsuario
    {
        public CS_CAIXA_DBContext Contexto { get; set; }

        public ClassUsuario()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<Usuario> ListaUsuarios()
        {
            Contexto = new CS_CAIXA_DBContext();

            var listaUsuarios = Contexto.Usuarios.OrderBy(p => p.NomeUsu).Select(p => p);
            return listaUsuarios.ToList();
        }

        public List<Usuario> ListaUsuariosMaster()
        {
            Contexto = new CS_CAIXA_DBContext();

            var listaUsuarios = Contexto.Usuarios.Where(p => p.Master == true).OrderBy(p => p.NomeUsu).Select(p => p);

            return listaUsuarios.ToList();
        }

        public bool VerificaAutenticacao(int idUsu, string senha)
        {
            List<Usuario> usuario = new List<Usuario>();
            usuario = Contexto.Usuarios.Where(p => p.Id_Usuario == idUsu && p.Senha == senha).ToList();
            if (usuario.Count <= 0)
                return false;
            else
                return true;
        }

        public bool ExcluirUsuario(int idUsuario)
        {
            try
            {

                Usuario usuario = Contexto.Usuarios.Where(p => p.Id_Usuario == idUsuario).FirstOrDefault();

                Contexto.Usuarios.Remove(usuario);

                Contexto.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SalvarUsuario(Usuario usuario, string acao)
        {
            Usuario salvarUsuario;

            if (acao == "adicionar")
            {
                salvarUsuario = new Usuario();
            }
            else
            {
                salvarUsuario = Contexto.Usuarios.Where(p => p.Id_Usuario == usuario.Id_Usuario).FirstOrDefault();
            }
            salvarUsuario.Id_Usuario = usuario.Id_Usuario;
            salvarUsuario.NomeUsu = usuario.NomeUsu;
            salvarUsuario.Senha = usuario.Senha;
            salvarUsuario.Master = usuario.Master;
            salvarUsuario.Notas = usuario.Notas;
            salvarUsuario.Protesto = usuario.Protesto;
            salvarUsuario.Rgi = usuario.Rgi;
            salvarUsuario.AlterarAtos = usuario.AlterarAtos;
            salvarUsuario.Balcao = usuario.Balcao;
            salvarUsuario.Caixa = usuario.Caixa;
            salvarUsuario.ExcluirAtos = usuario.ExcluirAtos;
            salvarUsuario.ImprimirMatricula = usuario.ImprimirMatricula;
            salvarUsuario.Adm = usuario.Adm;
           

            if (acao == "adicionar")
            {
                Contexto.Usuarios.Add(salvarUsuario);
            }
            try
            {
                Contexto.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
           
        }

        public int ProximoIdDisponivel()
        {
            Contexto = new CS_CAIXA_DBContext();
            return Contexto.Usuarios.Max(p => p.Id_Usuario) + 1;
        }
    }
}
