using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioCadastroCliente : RepositorioBase<CadastroCliente>
    {

        public CadastroCliente ObterClientesPorCpfCnpj(string documento)
        {
            Db = new CS_CAIXA_DBContext();
            return Db.CadastroClientes.Where(p => p.CPF_CNPJ == documento).FirstOrDefault();
        }
    }
}
