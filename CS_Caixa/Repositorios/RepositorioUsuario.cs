using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioUsuario : RepositorioBase<Usuario>
    {
        public string CriptogravarSenha(string senha)
        {
            return ClassCriptografia.Encrypt(senha);
        }

        public string DecriptogravarSenha(string senha)
        {
            return ClassCriptografia.Decrypt(senha);
        }
    }
}
