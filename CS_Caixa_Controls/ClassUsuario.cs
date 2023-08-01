using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa_Models.Models;


namespace CS_Caixa_Controls
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
            var listaUsuarios = (from p in Contexto.Usuarios select p);

            return listaUsuarios;
        }
    }
}
