using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace CS_Caixa.Controls
{
    static public class GerarXmlIndiceRgi
    {
        public static bool GerarXml(List<IndiceRegistro> NomesGerarXml, out string caminhoArq)
        {
            ClassIndiceRgi classIndiceRgi = new ClassIndiceRgi();
            bool ok = false;
            try
            {

                string nomeArquivo = "BDLight " + DateTime.Now;

                DateTime dataInicio = Convert.ToDateTime("01/01/1976");

                string caminho = string.Format(@"\\SERVIDOR\CS_Sistemas\CS_Caixa\Arquivos Indice Rgi\{0}.xml", nomeArquivo.Replace("/", "").Replace(":", "").Replace(" ", "_"));

                XmlTextWriter writer = new XmlTextWriter(caminho, System.Text.Encoding.GetEncoding("ISO-8859-1"));

                //inicia o documento xml
                writer.WriteStartDocument();
                //Usa a formatação
                writer.Formatting = Formatting.Indented;
                //Escreve o elemento raiz
                writer.WriteStartElement("BANCOLIGHT");
                writer.WriteAttributeString("xmlns:xsi",null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xsi:noNamespaceSchemaLocation", "http://www.arisp.com.br/xsd/BDLIGHT_OPCAO1.xsd");

                for (int i = 0; i < NomesGerarXml.Count; i++)
                {

                    var documemtoValido = false;

                    if (NomesGerarXml[i].CpfCnpj.Length == 11)
                      documemtoValido = ValidaCpfCnpj.ValidaCPF(NomesGerarXml[i].CpfCnpj);

                    if (NomesGerarXml[i].CpfCnpj.Length == 14)
                        documemtoValido = ValidaCpfCnpj.ValidaCNPJ(NomesGerarXml[i].CpfCnpj);

                    if (documemtoValido)
                    {
                        if (NomesGerarXml[i].CpfCnpj != null && NomesGerarXml[i].CpfCnpj != "" && NomesGerarXml[i].DataRegistro >= dataInicio && NomesGerarXml[i].DataVenda >= dataInicio)
                        {
                            //Inicia um elemento
                            writer.WriteStartElement("INDIVIDUO");

                            string Nome = NomesGerarXml[i].Nome.Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("&", "E").Replace("/", ".").Replace('"', '§');

                            Nome = Nome.Replace("§", "&quot;");



                            string TipoAto = NomesGerarXml[i].TipoAto.Replace("<", "&lt;").Replace(">", "&gt;").Replace("'", "&apos;").Replace("&", "E").Replace("/", ".").Replace('"', '§');

                            TipoAto = TipoAto.Replace("§", "&quot;");


                            //e sub-elementos
                            writer.WriteElementString("NOME", Nome);
                            writer.WriteElementString("CNPJCPF", NomesGerarXml[i].CpfCnpj);

                            if (NomesGerarXml[i].Ordem != null && NomesGerarXml[i].Ordem != "")
                                writer.WriteElementString("NMATRICULA", NomesGerarXml[i].Ordem);
                            else
                                writer.WriteElementString("NMATRICULA", "0");

                            writer.WriteElementString("TIPODEATO", TipoAto);

                            if (NomesGerarXml[i].DataRegistro.ToString().Length >= 8)
                                writer.WriteElementString("DTREGAVERB", NomesGerarXml[i].DataRegistro.ToString().Replace("/", "").Replace("-", "").Replace(".", "").Substring(0, 8));
                            else
                                writer.WriteElementString("DTREGAVERB", null);
                            if (NomesGerarXml[i].DataVenda.ToString().Length >= 8)
                                writer.WriteElementString("DTVENDA", NomesGerarXml[i].DataVenda.ToString().Replace("/", "").Replace("-", "").Replace(".", "").Substring(0, 8));
                            else
                                writer.WriteElementString("DTVENDA", null);
                            //encerra os elementos itens
                            writer.WriteEndElement();

                            NomesGerarXml[i].Enviado = true;

                            classIndiceRgi.SalvarIndiceRegistro(NomesGerarXml[i], "alterar");

                            
                        }

                    }

                }
                // encerra o elemento raiz
                writer.WriteFullEndElement();
                //escreve o XML para o arquivo e fecha o escritor
                writer.Close();
                caminhoArq = caminho;
                ok = true;
                return ok;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

      

        private static bool VerificarData(DateTime data)
        {
            if (data != null)
            {
                try
                {
                    var dataVerificar = data.ToShortDateString();

                    var dia = Convert.ToInt16(dataVerificar.Substring(0, 2));

                    var mes = Convert.ToInt16(dataVerificar.Substring(3, 2));

                    var ano = Convert.ToInt16(dataVerificar.Substring(6, 4));


                    if (dia <= 31 && mes <= 12 && ano > 1900)
                    {
                        if (mes == 2 && dia > 29)
                        {
                            return false;
                        }
                        else
                        {
                            if((mes == 1 || mes == 3 || mes == 5 || mes == 7 || mes == 8 || mes == 10 || mes == 12) && dia > 31)
                            {
                                return false;
                            }
                            else
                            {
                                if((mes == 4 || mes == 6 || mes == 9 || mes == 11) && dia > 30)
                                {
                                    return false;
                                }
                            }
                        }
                        
                    }
                    else
                    {
                        return false;
                    }


                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }


        public static List<IndiceRegistro> VerificarDatas(List<IndiceRegistro> ListaVerificar)
        {
            var retornoVerificacao = new List<IndiceRegistro>();


            for (int i = 0; i < ListaVerificar.Count; i++)
            {

                if(ListaVerificar[i].DataRegistro != null)
                {
                    if(VerificarData(Convert.ToDateTime(ListaVerificar[i].DataRegistro)) == false)
                    {
                        retornoVerificacao.Add(ListaVerificar[i]);
                    }
                }

                if (ListaVerificar[i].DataVenda != null)
                {
                    if (VerificarData(Convert.ToDateTime(ListaVerificar[i].DataVenda)) == false)
                    {
                        retornoVerificacao.Add(ListaVerificar[i]);
                    }
                }
                
            }
            

            return retornoVerificacao;
        }



    }

}