using CS_Caixa.Controls;
using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Repositorios
{
    public class RepositorioControle_Uso : RepositorioBase<Controle_Uso>
    {
        public void SalvarControle(string demo, string ativarlicenca, string dataInicio, string dataFim, string dataAtivacao, string codigoAtivacao, string versao)
        {
            Controle_Uso controle = new Controle_Uso();
            controle.Demo = ClassCriptografia.Encrypt(demo);
            controle.AtivacaoUso = ClassCriptografia.Encrypt(ativarlicenca);
            controle.DataUltimaUtilizacao = ClassCriptografia.Encrypt(DateTime.Now.Date.ToShortDateString());
            controle.DataValidadeInicio = ClassCriptografia.Encrypt(dataInicio);
            controle.DataValidadeFim = ClassCriptografia.Encrypt(dataFim);
            controle.CodigoAtivacao = codigoAtivacao;
            controle.DataAtivacao = ClassCriptografia.Encrypt(dataAtivacao);
            controle.DiasUtilizado = 0;
            controle.Versao = ClassCriptografia.Encrypt(versao);
            Adicionar(controle);
        }
    }
}