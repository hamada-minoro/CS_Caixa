using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CS_Caixa.Repositorios
{
    public class RepositorioParametros : RepositorioBase<Parametro>
    {
        public bool VerificarArquivoXml(string caminho, string CodigoInstalacao)
        {
            XmlDocument oXML = new XmlDocument();

            string ArquivoXML = caminho;

            oXML.Load(ArquivoXML);

            string codigo = oXML.SelectSingleNode("Config").ChildNodes[5].InnerText;


            if (codigo == CodigoInstalacao)
                return true;
            else
                return false;

        }
    }
}