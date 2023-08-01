using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioCadastro_Painel : RepositorioBase<Cadastro_Painel>
    {
        public Cadastro_Painel ObterPorIdentificador(string id)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Cadastro_Painel.Where(p => p.Identificador_Pc == id).FirstOrDefault();
        }
        public Cadastro_Painel ObterPorNome(string nome)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.Cadastro_Painel.Where(p => p.Nome_Pc == nome).FirstOrDefault();
        }
    }
}
