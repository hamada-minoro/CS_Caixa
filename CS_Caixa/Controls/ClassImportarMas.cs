using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace CS_Caixa.Controls
{
    public class ClassImportarMas
    {
        CS_CAIXA_DBContext Contexto { get; set; }
        public ClassImportarMas()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<ImportarMa> ListarTodosImportados()
        {
            return Contexto.ImportarMas.OrderBy(p => p.Data).ToList();
        }

        public List<ImportarMa> ConsultaDetalhada(string tipoConsulta, string dados)
        {
            var retornoLista = new List<ImportarMa>();

            switch (tipoConsulta)
            {
                case "ATRIBUIÇÃO":
                    retornoLista = Contexto.ImportarMas.Where(p => p.Atribuicao == dados).ToList();
                    break;

                case "TIPO DE ATO":
                    retornoLista = Contexto.ImportarMas.Where(p => p.TipoAto == dados).ToList();
                    break;

                case "SELO":
                    retornoLista = Contexto.ImportarMas.Where(p => p.Selo == dados).ToList();
                    break;

                default:
                    break;
            }

            return retornoLista;
        }

        public ImportarMa SalvarAto(ImportarMa atoAlteradoNovo, string tipo)
        {
            ImportarMa ato;

            if (tipo == "novo")
                ato = new ImportarMa();

            if (tipo == "alterar")
                ato = Contexto.ImportarMas.Where(p => p.IdImportarMas == atoAlteradoNovo.IdImportarMas).FirstOrDefault();

            ato = atoAlteradoNovo;

            if (tipo == "novo")
                Contexto.ImportarMas.Add(ato);

            Contexto.SaveChanges();

            return ato;
        }

        public ImportarMa CalcularValores(ImportarMa atoCalcular)
        {
            
            int index;
            string Siss = "0,00";

            try
            {
                if (atoCalcular.Emolumentos > 0)
                {
                    var iss = atoCalcular.Emolumentos / 0.95M;
                    iss = iss - atoCalcular.Emolumentos;



                    Siss = Convert.ToString(iss);

                    index = Siss.IndexOf(',');
                    if(Siss.Length - index > 2)
                    Siss = Siss.Substring(0, index + 3);

                    atoCalcular.Iss = Convert.ToDecimal(Siss);



                    atoCalcular.Total = atoCalcular.Emolumentos + atoCalcular.Acoterj + atoCalcular.Distribuidor + atoCalcular.Fetj + atoCalcular.Funarpen + atoCalcular.Fundperj +
                        atoCalcular.Funperj + atoCalcular.Iss + atoCalcular.Mutua + atoCalcular.Ressag;
                }
                else
                {
                    atoCalcular.Iss = 0;
                    atoCalcular.Total = 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
            return atoCalcular;
        }


        public List<string> CarregarListaTipoAtos()
        {
            return Contexto.ImportarMas.Select(p => p.TipoAto).Distinct().ToList();
        }

        public List<string> CarregarListaAtribuicoes()
        {
            return Contexto.ImportarMas.Select(p => p.Atribuicao).Distinct().ToList();
        }

        public ImportarMa ExcluirAto(ImportarMa atoExcluir)
        {
            Contexto = new CS_CAIXA_DBContext();

            ImportarMa ato = Contexto.ImportarMas.Where(p => p.IdImportarMas == atoExcluir.IdImportarMas).FirstOrDefault();

            Contexto.ImportarMas.Remove(ato);
            Contexto.SaveChanges();

            return ato;

        }

        public List<ImportarMa> ListarAtosPorPeriodo(DateTime inicio, DateTime fim)
        {
            return Contexto.ImportarMas.Where(p => p.Data >= inicio && p.Data <= fim).OrderBy(p => p.Data).ToList();
        }

        public List<ImportarMa> VerificarRegistrosExistentesPorData(DateTime data)
        {
            return Contexto.ImportarMas.Where(p => p.Data == data.Date).ToList();                     
        }

        public List<ImportarMa> LerArquivoXml(string caminho)
        {
            XmlTextReader leituraXml;
            var AtoAdd = new ImportarMa();
            var AtosImpotados = new List<ImportarMa>();

            string leituraAtual = string.Empty;

            leituraXml = new XmlTextReader(caminho);

            leituraXml.ReadToFollowing("Relatorio");
            string data = leituraXml.GetAttribute("DataPratica");

            while (leituraXml.Read())
            {
                switch (leituraXml.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (leituraXml.Name)
                        {

                            case "DESC_ATRIBUICAO":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "CD_SELO":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "CD_ALEATORIO":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "DESC_TIPO_ATO":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "CD_TIPO_COBRANCA":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "VALOR_EMOLUMENTOS":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "FETJ":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "FUNDPERJ":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "FUNPERJ":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "FUNARPEN":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "RESSAG":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "MUTUA":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "ACOTERJ":
                                leituraAtual = leituraXml.Name;
                                break;

                            case "DISTRIBUIDOR":
                                leituraAtual = leituraXml.Name;
                                break;
                        }

                        break;

                    case XmlNodeType.Text:

                        switch (leituraAtual)
                        {
                            case "DESC_ATRIBUICAO":
                                AtoAdd.Atribuicao = leituraXml.Value;
                                break;

                            case "CD_SELO":
                                AtoAdd.Selo = leituraXml.Value;
                                break;

                            case "CD_ALEATORIO":
                                AtoAdd.Aleatorio = leituraXml.Value;
                                break;

                            case "DESC_TIPO_ATO":
                                AtoAdd.TipoAto = leituraXml.Value;
                                break;

                            case "CD_TIPO_COBRANCA":
                                AtoAdd.TipoCobranca = leituraXml.Value;
                                break;

                            case "VALOR_EMOLUMENTOS":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Emolumentos = Convert.ToDecimal(valor);
                                }
                                break;

                            case "FETJ":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Fetj = Convert.ToDecimal(valor);
                                }
                                break;

                            case "FUNDPERJ":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Fundperj = Convert.ToDecimal(valor);
                                }
                                break;

                            case "FUNPERJ":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Funperj = Convert.ToDecimal(valor);
                                }
                                break;

                            case "FUNARPEN":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Funarpen = Convert.ToDecimal(valor);
                                }
                                break;

                            case "RESSAG":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Ressag = Convert.ToDecimal(valor);
                                }
                                break;

                            case "MUTUA":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Mutua = Convert.ToDecimal(valor);
                                }
                                break;

                            case "ACOTERJ":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Acoterj = Convert.ToDecimal(valor);
                                }
                                break;

                            case "DISTRIBUIDOR":
                                if (leituraXml.Value != "" && leituraXml.Value != null)
                                {
                                    var valor = leituraXml.Value.Replace('.', ',');
                                    AtoAdd.Distribuidor = Convert.ToDecimal(valor);
                                }
                                break;
                        }

                        break;

                    case XmlNodeType.EndElement:
                        if (leituraXml.Name == "ItemRelatorio")
                        {
                            AtoAdd.Data = Convert.ToDateTime(data);
                            AtosImpotados.Add(AtoAdd);

                            AtoAdd = new ImportarMa();

                        }


                        break;
                }
            }

            return AtosImpotados;
        }
    }
}
