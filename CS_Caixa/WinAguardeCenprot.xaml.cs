using CS_Caixa.Agragador;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml;
using System.Xml.Linq;

namespace CS_Caixa
{
    /// <summary>
    /// Interaction logic for WinAguardeCenprot.xaml
    /// </summary>
    public partial class WinAguardeCenprot : Window
    {
        BackgroundWorker worker;

        List<TiposTitulos> tipos = new List<TiposTitulos>();
        List<Sacadores> sacadores = new List<Sacadores>();
        List<Devedores> devedores = new List<Devedores>();
        List<Portadores> portadores = new List<Portadores>();

        List<string> especiesCenprot = new List<string> { "CBI", "CC", "CCB", "CCC", "CCE", "CCI", "CCR", "CD", "CDA", "CH", "CHP", "CJV", "CM",
            "CPH", "CPS", "CRH", "CRP", "CT", "DM", "DMI", "DR", "DRI", "DS", "DSI", "EC", "LC", "NCC", "NCE", "NCI", "NCR",
        "NP", "NPR", "RA","SJ","TA","TM","TS","W","ZZZ"};

        WinCenprot _cenprot;

        int cont = 0;

        TituloProtesto tituloProtesto;

        FbDataReader dr;

        List<string> listaIrregularidades = new List<string>();

        bool criticarDevedor = false;
        bool criticarSacador = false;

        int titulosProtestados = 0;
        int titulosCancelados = 0;
        int titulosRetirados = 0;
        int titulosPagos = 0;
        int titulosSustados = 0;
        int titulosDevolvidos = 0;
        int total = 0;
        DateTime _inicio;
        DateTime _fim;
        public WinAguardeCenprot(WinCenprot cenprot)
        {

            _cenprot = cenprot;
            _inicio = cenprot.datePickerdataConsulta.SelectedDate.Value;
            _fim = cenprot.datePickerdataConsultaFim.SelectedDate.Value;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (_cenprot.tipoConsulta == "data")
            {
                ObterPortadores();
                ObterTiposTitulo();
                criticarDevedor = _cenprot.ckbEnderecoDevedor.IsChecked.Value;
                criticarSacador = _cenprot.ckbEnderecoSacador.IsChecked.Value;
            }

            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync();
        }


        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_cenprot.tipoConsulta == "data")
            {
                progressBar1.Value = e.ProgressPercentage;

                progressBar1.Maximum = total;

                _cenprot.qtdTitulos.Content = string.Format("Qtd. de Titulos: {0}", e.ProgressPercentage);

                _cenprot.qtdTitulosProtestados.Content = string.Format("Protestados: {0}", titulosProtestados);

                _cenprot.qtdTitulosCancelados.Content = string.Format("Cancelados: {0}", titulosCancelados);

                _cenprot.qtdTitulosPagos.Content = string.Format("Pagos: {0}", titulosPagos);

                _cenprot.qtdTitulosRetirados.Content = string.Format("Retirados: {0}", titulosRetirados);

                _cenprot.qtdTitulosSustados.Content = string.Format("Sustados: {0}", titulosSustados);

                _cenprot.qtdTitulosDevolvidos.Content = string.Format("Devolvido: {0}", titulosDevolvidos);

                label2.Content = string.Format("Verificando Protocolo {0}", tituloProtesto.PROTOCOLO);
            }

        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (_cenprot.tipoConsulta == "data")
                    ObterDataTableTitulosPorPeriodo(_inicio, _fim);
                else
                {
                    LerArquivoXml();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocorreu um erro ao tentar obter os dados. " + ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private void LerArquivoXml()
        {

            Portadores apresentante;
            _cenprot.titulos = new List<TituloProtesto>();
            int codigoPortador = 0;

            XDocument xml = XDocument.Load(_cenprot.openFileDialog1.FileName);
            foreach (XElement dadosApresentantes in
                xml.Element("estados").Elements("estado").Elements("cartorios").Elements("cartorio").Elements("apresentantes").Elements("apresentante"))
            {

                apresentante = new Portadores();
                apresentante.ID_PORTADOR = codigoPortador++;
                apresentante.CODIGO = dadosApresentantes.Attribute("codigo").Value;
                apresentante.NOME = dadosApresentantes.Element("nome").Value;

                if (apresentante.CODIGO == "TAB")
                {
                    apresentante.TIPO = "F";
                }
                else
                {
                    apresentante.TIPO = "J";
                }

                apresentante.SEQUENCIA = Convert.ToInt32(dadosApresentantes.Element("titulos").Attribute("total").Value);



                portadores.Add(apresentante);
            }



            foreach (XElement dadosTitulos in
         xml.Element("estados").Elements("estado").Elements("cartorios").Elements("cartorio").Elements("apresentantes").Elements("apresentante").Elements("titulos").Elements("titulo"))
            {

                tituloProtesto = new TituloProtesto();

                tituloProtesto.Especie_Titulo = dadosTitulos.Element("especie").Value;

                tituloProtesto.DEVEDOR = dadosTitulos.Element("devedores").Element("devedor").Element("nome").Value;

                tituloProtesto.CEDENTE = dadosTitulos.Element("cedente").Element("nome").Value;

                tituloProtesto.SACADOR = dadosTitulos.Element("sacador").Element("nome").Value;

                tituloProtesto.DT_ENVIO = Convert.ToDateTime(dadosTitulos.Element("data_ocorrencia").Value);

                tituloProtesto.PROTOCOLO = Convert.ToInt32(dadosTitulos.Element("protocolo").Value);

                switch (dadosTitulos.Element("ocorrencia").Value)
                {
                    case "1":
                        tituloProtesto.STATUS = "PAGO";
                        titulosPagos++;
                        break;
                    case "2":
                        tituloProtesto.STATUS = "PROTESTADO";
                        titulosProtestados++;
                        break;

                    case "3":
                        tituloProtesto.STATUS = "RETIRADO";
                        titulosRetirados++;
                        break;

                    case "4":
                        tituloProtesto.STATUS = "SUSTADO";
                        titulosSustados++;
                        break;

                    case "5":
                        tituloProtesto.STATUS = "DEVOLVIDO";
                        titulosDevolvidos++;
                        break;

                    case "A":
                        tituloProtesto.STATUS = "CANCELADO";
                        titulosCancelados++;
                        break;
                    default:
                        break;
                }

                total++;
                _cenprot.titulos.Add(tituloProtesto);

            }

            int inicio = 0;
            int fim = 0;

            foreach (var item in portadores)
            {               

                fim = inicio + item.SEQUENCIA;

                for (int i = inicio; i < fim; i++)
                {



                    _cenprot.titulos[i].APRESENTANTE = item.NOME;

                    _cenprot.titulos[i].TIPO_APRESENTANTE = item.TIPO;


                }

                inicio = inicio + item.SEQUENCIA;
            }



        }






        public void ConverterDataRowEmObjetoTituloProtesto(FbDataReader linha)
        {


            _cenprot.titulos = new List<TituloProtesto>();

            while (linha.Read())
            {
                cont = cont + 1;

                tituloProtesto = new TituloProtesto();

                tituloProtesto.ACEITE = linha["ACEITE"].ToString();

                if (linha["ACOTERJ"].ToString() != "")
                    tituloProtesto.ACOTERJ = Convert.ToDecimal(linha["ACOTERJ"]);

                tituloProtesto.AGENCIA = linha["AGENCIA"].ToString();
                tituloProtesto.AGENCIA_CEDENTE = linha["AGENCIA_CEDENTE"].ToString();
                tituloProtesto.ALEATORIO_PROTESTO = linha["ALEATORIO_PROTESTO"].ToString();
                tituloProtesto.ALEATORIO_SOLUCAO = linha["ALEATORIO_SOLUCAO"].ToString();
                tituloProtesto.ANTIGO = linha["ANTIGO"].ToString();
                tituloProtesto.APRESENTANTE = linha["APRESENTANTE"].ToString();
                tituloProtesto.ARQUIVO = linha["ARQUIVO"].ToString();

                tituloProtesto.AVALISTA_DEVEDOR = linha["AVALISTA_DEVEDOR"].ToString();
                tituloProtesto.AVISTA = linha["AVISTA"].ToString();
                tituloProtesto.BAIXADO_ARQUIVO = linha["BAIXADO_ARQUIVO"].ToString();
                tituloProtesto.BANCO = linha["BANCO"].ToString();
                tituloProtesto.CCT = linha["CCT"].ToString();
                tituloProtesto.CEDENTE = linha["CEDENTE"].ToString();
                tituloProtesto.COBRANCA = linha["COBRANCA"].ToString();

                if (linha["CODIGO"].ToString() != "")
                    tituloProtesto.CODIGO = Convert.ToInt32(linha["CODIGO"]);

                tituloProtesto.CODIGO_APRESENTANTE = linha["CODIGO_APRESENTANTE"].ToString();

                tituloProtesto.CONTA = linha["CONTA"].ToString();

                tituloProtesto.CONVENIO = linha["CONVENIO"].ToString();
                tituloProtesto.CPF_CNPJ_APRESENTANTE = linha["CPF_CNPJ_APRESENTANTE"].ToString();
                tituloProtesto.CPF_CNPJ_DEVEDOR = linha["CPF_CNPJ_DEVEDOR"].ToString();
                tituloProtesto.CPF_CNPJ_SACADOR = linha["CPF_CNPJ_SACADOR"].ToString();
                tituloProtesto.CPF_ESCREVENTE = linha["CPF_ESCREVENTE"].ToString();
                tituloProtesto.CPF_ESCREVENTE_PG = linha["CPF_ESCREVENTE_PG"].ToString();
                tituloProtesto.DETERMINACAO = linha["DETERMINACAO"].ToString();
                tituloProtesto.DEVEDOR = linha["DEVEDOR"].ToString();

                if (linha["DISTRIBUICAO"].ToString() != "")
                    tituloProtesto.DISTRIBUICAO = Convert.ToDecimal(linha["DISTRIBUICAO"]);

                if (linha["DT_DEFINITIVA"].ToString() != "")
                    tituloProtesto.DT_DEFINITIVA = Convert.ToDateTime(linha["DT_DEFINITIVA"]);


                if (linha["DT_DEVOLVIDO"].ToString() != "")
                    tituloProtesto.DT_DEVOLVIDO = Convert.ToDateTime(linha["DT_DEVOLVIDO"]);

                if (linha["DT_ENTRADA"].ToString() != "")
                    tituloProtesto.DT_ENTRADA = Convert.ToDateTime(linha["DT_ENTRADA"]);


                if (linha["DT_ENVIO"].ToString() != "")
                    tituloProtesto.DT_ENVIO = Convert.ToDateTime(linha["DT_ENVIO"]);

                if (linha["DT_INTIMACAO"].ToString() != "")
                    tituloProtesto.DT_INTIMACAO = Convert.ToDateTime(linha["DT_INTIMACAO"]);

                if (linha["DT_PAGAMENTO"].ToString() != "")
                    tituloProtesto.DT_PAGAMENTO = Convert.ToDateTime(linha["DT_PAGAMENTO"]);

                if (linha["DT_PRAZO"].ToString() != "")
                    tituloProtesto.DT_PRAZO = Convert.ToDateTime(linha["DT_PRAZO"]);

                if (linha["DT_PROT_CRA"].ToString() != "")
                    tituloProtesto.DT_PROT_CRA = Convert.ToDateTime(linha["DT_PROT_CRA"]);

                if (linha["DT_PROTOCOLO"].ToString() != "")
                    tituloProtesto.DT_PROTOCOLO = Convert.ToDateTime(linha["DT_PROTOCOLO"]);

                if (linha["DT_PUBLICACAO"].ToString() != "")
                    tituloProtesto.DT_PUBLICACAO = Convert.ToDateTime(linha["DT_PUBLICACAO"]);

                if (linha["DT_REGISTRO"].ToString() != "")
                    tituloProtesto.DT_REGISTRO = Convert.ToDateTime(linha["DT_REGISTRO"]);

                if (linha["DT_RETORNO_PROTESTO"].ToString() != "")
                    tituloProtesto.DT_RETORNO_PROTESTO = Convert.ToDateTime(linha["DT_RETORNO_PROTESTO"]);

                if (linha["DT_SUSTADO"].ToString() != "")
                    tituloProtesto.DT_SUSTADO = Convert.ToDateTime(linha["DT_SUSTADO"]);

                if (linha["DT_TITULO"].ToString() != "")
                    tituloProtesto.DT_TITULO = Convert.ToDateTime(linha["DT_TITULO"]);

                if (linha["DT_RETIRADO"].ToString() != "")
                    tituloProtesto.DT_RETIRADO = Convert.ToDateTime(linha["DT_RETIRADO"]);

                if (linha["DT_VENCIMENTO"].ToString() != "")
                    tituloProtesto.DT_VENCIMENTO = Convert.ToDateTime(linha["DT_VENCIMENTO"]);

                tituloProtesto.ELETRONICO = linha["ELETRONICO"].ToString();
                tituloProtesto.EMAIL_ADVOGADO = linha["EMAIL_ADVOGADO"].ToString();

                if (linha["EMOLUMENTOS"].ToString() != "")
                    tituloProtesto.EMOLUMENTOS = Convert.ToDecimal(linha["EMOLUMENTOS"]);

                tituloProtesto.ENVIADO_APONTAMENTO = linha["ENVIADO_APONTAMENTO"].ToString();
                tituloProtesto.ENVIADO_DEVOLVIDO = linha["ENVIADO_DEVOLVIDO"].ToString();
                tituloProtesto.ENVIADO_PAGAMENTO = linha["ENVIADO_PAGAMENTO"].ToString();
                tituloProtesto.ENVIADO_PROTESTO = linha["ENVIADO_PROTESTO"].ToString();
                tituloProtesto.ENVIADO_RETIRADO = linha["ENVIADO_RETIRADO"].ToString();
                tituloProtesto.ENVIADO_SUSTADO = linha["ENVIADO_SUSTADO"].ToString();
                tituloProtesto.EXPORTADO = linha["EXPORTADO"].ToString();

                if (linha["FETJ"].ToString() != "")
                    tituloProtesto.FETJ = Convert.ToDecimal(linha["FETJ"]);

                tituloProtesto.FINS_FALIMENTARES = linha["FINS_FALIMENTARES"].ToString();
                tituloProtesto.FOLHA_PROTOCOLO = linha["FOLHA_PROTOCOLO"].ToString();
                tituloProtesto.FOLHA_REGISTRO = linha["FOLHA_REGISTRO"].ToString();
                tituloProtesto.FORMA_PAGAMENTO = linha["FORMA_PAGAMENTO"].ToString();

                if (linha["FUNARPEN"].ToString() != "")
                    tituloProtesto.FUNARPEN = Convert.ToDecimal(linha["FUNARPEN"]);
                if (linha["FUNDPERJ"].ToString() != "")
                    tituloProtesto.FUNDPERJ = Convert.ToDecimal(linha["FUNDPERJ"]);
                if (linha["FUNPERJ"].ToString() != "")
                    tituloProtesto.FUNPERJ = Convert.ToDecimal(linha["FUNPERJ"]);

                if (linha["ID_ATO"].ToString() != "")
                    tituloProtesto.ID_ATO = Convert.ToInt32(linha["ID_ATO"]);
                if (linha["ID_MSG"].ToString() != "")
                    tituloProtesto.ID_MSG = Convert.ToInt32(linha["ID_MSG"]);

                tituloProtesto.INTIMADO_ARQUIVO = linha["INTIMADO_ARQUIVO"].ToString();

                if (linha["IRREGULARIDADE"].ToString() != "")
                    tituloProtesto.IRREGULARIDADE = Convert.ToInt32(linha["IRREGULARIDADE"]);

                if (linha["ISS"].ToString() != "")
                    tituloProtesto.ISS = Convert.ToDecimal(linha["ISS"]);



                tituloProtesto.JUDICIAL = linha["JUDICIAL"].ToString();
                tituloProtesto.LETRA = linha["LETRA"].ToString();

                if (linha["LIVRO_PROTOCOLO"].ToString() != "")
                    tituloProtesto.LIVRO_PROTOCOLO = Convert.ToInt32(linha["LIVRO_PROTOCOLO"]);

                if (linha["LIVRO_REGISTRO"].ToString() != "")
                    tituloProtesto.LIVRO_REGISTRO = Convert.ToInt32(linha["LIVRO_REGISTRO"]);

                tituloProtesto.MOTIVO_INTIMACAO = linha["MOTIVO_INTIMACAO"].ToString();

                if (linha["MUTUA"].ToString() != "")
                    tituloProtesto.MUTUA = Convert.ToDecimal(linha["MUTUA"]);

                tituloProtesto.NOSSO_NUMERO = linha["NOSSO_NUMERO"].ToString();
                tituloProtesto.NUMERO_PAGAMENTO = linha["NUMERO_PAGAMENTO"].ToString();
                tituloProtesto.NUMERO_TITULO = linha["NUMERO_TITULO"].ToString();
                tituloProtesto.OBSERVACAO = linha["OBSERVACAO"].ToString();

                if (linha["PMCMV"].ToString() != "")
                    tituloProtesto.PMCMV = Convert.ToDecimal(linha["PMCMV"]);


                tituloProtesto.PRACA_PROTESTO = linha["PRACA_PROTESTO"].ToString();
                tituloProtesto.PROTESTADO = linha["PROTESTADO"].ToString();

                if (linha["PROTOCOLO"].ToString() != "")
                    tituloProtesto.PROTOCOLO = Convert.ToInt32(linha["PROTOCOLO"]);

                if (linha["PROTOCOLO_DISTRIBUIDOR"].ToString() != "")
                    tituloProtesto.PROTOCOLO_DISTRIBUIDOR = Convert.ToInt32(linha["PROTOCOLO_DISTRIBUIDOR"]);

                if (linha["RECIBO"].ToString() != "")
                    tituloProtesto.RECIBO = Convert.ToInt32(linha["RECIBO"]);

                if (linha["RECIBO_PAGAMENTO"].ToString() != "")
                    tituloProtesto.RECIBO_PAGAMENTO = Convert.ToInt32(linha["RECIBO_PAGAMENTO"]);

                if (linha["RECIBO_PAGAMENTO"].ToString() != "")
                    tituloProtesto.RECIBO_PAGAMENTO = Convert.ToInt32(linha["RECIBO_PAGAMENTO"]);

                if (linha["REGISTRO"].ToString() != "")
                    tituloProtesto.REGISTRO = Convert.ToInt32(linha["REGISTRO"]);

                if (linha["REGISTRO"].ToString() != "")
                    tituloProtesto.REGISTRO = Convert.ToInt32(linha["REGISTRO"]);


                tituloProtesto.RETORNO = linha["RETORNO"].ToString();
                tituloProtesto.RETORNO_CRA = linha["RETORNO_CRA"].ToString();
                tituloProtesto.RETORNO_PROTESTO = linha["RETORNO_PROTESTO"].ToString();
                tituloProtesto.SACADOR = linha["SACADOR"].ToString();

                if (linha["SALDO_PROTESTO"].ToString() != "")
                    tituloProtesto.SALDO_PROTESTO = Convert.ToDecimal(linha["SALDO_PROTESTO"]);

                if (linha["SALDO_TITULO"].ToString() != "")
                    tituloProtesto.SALDO_TITULO = Convert.ToDecimal(linha["SALDO_TITULO"]);


                tituloProtesto.SELO_PAGAMENTO = linha["SELO_PAGAMENTO"].ToString();
                tituloProtesto.SELO_REGISTRO = linha["SELO_REGISTRO"].ToString();
                tituloProtesto.STATUS = linha["STATUS"].ToString();

                if (linha["TARIFA_BANCARIA"].ToString() != "")
                    tituloProtesto.TARIFA_BANCARIA = Convert.ToDecimal(linha["TARIFA_BANCARIA"]);

                tituloProtesto.TELEFONE_ADVOGADO = linha["TELEFONE_ADVOGADO"].ToString();
                tituloProtesto.TIPO_APRESENTACAO = linha["TIPO_APRESENTACAO"].ToString();
                tituloProtesto.TIPO_APRESENTANTE = linha["TIPO_APRESENTANTE"].ToString();
                tituloProtesto.TIPO_DEVEDOR = linha["TIPO_DEVEDOR"].ToString();
                tituloProtesto.TIPO_ENDOSSO = linha["TIPO_ENDOSSO"].ToString();
                tituloProtesto.TIPO_INTIMACAO = linha["TIPO_INTIMACAO"].ToString();


                if (linha["TIPO_PROTESTO"].ToString() != "")
                    tituloProtesto.TIPO_PROTESTO = Convert.ToInt32(linha["TIPO_PROTESTO"]);

                tituloProtesto.TIPO_SUSTACAO = linha["TIPO_SUSTACAO"].ToString();

                if (linha["TIPO_TITULO"].ToString() != "")
                    tituloProtesto.TIPO_TITULO = Convert.ToInt32(linha["TIPO_TITULO"]);

                if (linha["TOTAL"].ToString() != "")
                    tituloProtesto.TOTAL = Convert.ToDecimal(linha["TOTAL"]);

                if (linha["VALOR_AR"].ToString() != "")
                    tituloProtesto.VALOR_AR = Convert.ToDecimal(linha["VALOR_AR"]);

                if (linha["VALOR_PAGAMENTO"].ToString() != "")
                    tituloProtesto.VALOR_PAGAMENTO = Convert.ToDecimal(linha["VALOR_PAGAMENTO"]);

                if (linha["VALOR_TITULO"].ToString() != "")
                    tituloProtesto.VALOR_TITULO = Convert.ToDecimal(linha["VALOR_TITULO"]);

                tituloProtesto.Especie_Titulo = tipos.Where(p => p.CODIGO == tituloProtesto.TIPO_TITULO).Select(p => p.SIGLA).FirstOrDefault();

                if (!especiesCenprot.Contains(tituloProtesto.Especie_Titulo))
                    tituloProtesto.Especie_Titulo = "DV";


                switch (tituloProtesto.STATUS)
                {
                    case "PROTESTADO":
                        titulosProtestados++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_REGISTRO;
                        break;

                    case "CANCELADO":
                        titulosCancelados++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_PAGAMENTO;
                        break;

                    case "PAGO":
                        titulosPagos++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_PAGAMENTO;
                        break;

                    case "RETIRADO":
                        titulosRetirados++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_RETIRADO;
                        break;

                    case "SUSTADO":
                        titulosSustados++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_SUSTADO;
                        break;

                    case "DEVOLVIDO":
                        titulosDevolvidos++;
                        tituloProtesto.DT_ENVIO = tituloProtesto.DT_DEVOLVIDO;
                        break;

                    default:
                        break;
                }

                for (int i = cont; i < cont + 1; i++)
                {
                    Thread.Sleep(1);
                    worker.ReportProgress(cont);
                }



                ConverterEmObjetoSacador(tituloProtesto.ID_ATO);
                ConverterEmObjetoDevedor(tituloProtesto.ID_ATO);

                string irregu = VerificarIrregularidade(tituloProtesto);

                if (irregu != "")
                {
                    listaIrregularidades.Add(irregu);
                    tituloProtesto.Irregular = true;
                }


                _cenprot.titulos.Add(tituloProtesto);










            }
        }






        private string VerificarIrregularidade(TituloProtesto titulo)
        {
            string irregularidade = "";

            bool irregular = false;

            int numeroProtocolo = titulo.PROTOCOLO;

            Portadores portador = portadores.Where(p => p.NOME == titulo.APRESENTANTE || p.CODIGO == titulo.CODIGO_APRESENTANTE).FirstOrDefault();
            if (portador != null)
            {
                if (titulo.APRESENTANTE == "" || titulo.APRESENTANTE == null)
                {
                    irregularidade = irregularidade += "- Nome Apresentante. ";
                    irregular = true;
                }

                if (titulo.TIPO_APRESENTANTE == "J" && (portador.CODIGO == "" || portador.CODIGO == null))
                {
                    irregularidade = irregularidade += "- Código Apresentante. ";
                    irregular = true;
                }

                //if (portador.DOCUMENTO == "" || portador.DOCUMENTO == null)
                //{
                //    irregularidade = irregularidade += "- Documento Apresentante. ";
                //    irregular = true;
                //}

            }
            else
            {
                irregularidade = irregularidade += "- Ausência de Apresentante. ";
                irregular = true;
            }



            List<Sacadores> sacador = sacadores.Where(p => p.ID_ATO == titulo.ID_ATO).ToList();

            if (sacador != null)
                foreach (var item in sacador)
                {
                    if (item.NOME == "" || item.NOME == null)
                    {
                        irregularidade = irregularidade += "- Nome Sacador. ";
                        irregular = true;
                    }
                    if (item.DOCUMENTO == "" || item.DOCUMENTO == null)
                    {
                        irregularidade = irregularidade += "- Documento Sacador. ";
                        irregular = true;
                    }

                    if (criticarSacador == true)
                    {
                        if (item.ENDERECO == "" || item.ENDERECO == null)
                        {
                            irregularidade = irregularidade += "- Endereço Sacador. ";
                            irregular = true;
                        }

                        if (item.BAIRRO == "" || item.BAIRRO == null)
                        {
                            irregularidade = irregularidade += "- Bairro Sacador. ";
                            irregular = true;
                        }

                        if (item.CEP == "" || item.CEP == null)
                        {
                            irregularidade = irregularidade += "- CEP Sacador. ";
                            irregular = true;
                        }
                        if (item.MUNICIPIO == "" || item.MUNICIPIO == null)
                        {
                            irregularidade = irregularidade += "- Município Sacador. ";
                            irregular = true;
                        }
                        if (item.UF == "" || item.UF == null)
                        {
                            irregularidade = irregularidade += "- UF Sacador. ";
                            irregular = true;
                        }
                    }
                }
            else
            {
                irregularidade = irregularidade += "- Ausência de Sacador. ";
                irregular = true;
            }





            if (titulo.CEDENTE == "" || titulo.CEDENTE == null)
            {
                irregularidade = irregularidade += "- Nome Cedente. ";
                irregular = true;
            }

            if (titulo.Especie_Titulo == "" || titulo.Especie_Titulo == null)
            {
                irregularidade = irregularidade += "- Espécie Título. ";
                irregular = true;
            }

            if (titulo.NUMERO_TITULO == "" || titulo.NUMERO_TITULO == null)
            {
                irregularidade = irregularidade += "- Número Título. ";
                irregular = true;
            }
            if (titulo.DT_TITULO.ToShortDateString() == "01/01/0001" || titulo.DT_TITULO.ToString() == "" || titulo.DT_TITULO == null)
            {
                irregularidade = irregularidade += "- Data Título. ";
                irregular = true;
            }

            if (titulo.VALOR_TITULO.ToString() == "")
            {
                irregularidade = irregularidade += "- Valor Título. ";
                irregular = true;
            }
            if (titulo.SALDO_TITULO.ToString() == "")
            {
                irregularidade = irregularidade += "- Valor Título. ";
                irregular = true;
            }

            if (titulo.PRACA_PROTESTO.ToString() == "" || titulo.PRACA_PROTESTO == null)
            {
                irregularidade = irregularidade += "- Praça Protesto. ";
                irregular = true;
            }


            List<Devedores> devedor = devedores.Where(p => p.ID_ATO == titulo.ID_ATO).ToList();

            if (devedor != null)
                foreach (var item in devedor)
                {
                    if (item.NOME.ToString() == "" || item.NOME == null)
                    {
                        irregularidade = irregularidade += "- Nome Devedor. ";
                        irregular = true;
                    }
                    if (item.DOCUMENTO.ToString() == "" || item.DOCUMENTO == null)
                    {
                        irregularidade = irregularidade += "- Documento Devedor. ";
                        irregular = true;
                    }

                    if (criticarDevedor == true)
                    {
                        if (item.ENDERECO.ToString() == "" || item.ENDERECO == null)
                        {
                            irregularidade = irregularidade += "- Endereço Devedor. ";
                            irregular = true;
                        }

                        if (item.BAIRRO == "" || item.BAIRRO == null)
                        {
                            irregularidade = irregularidade += "- Bairro Devedor. ";
                            irregular = true;
                        }

                        if (item.CEP == "" || item.CEP == null)
                        {
                            irregularidade = irregularidade += "- CEP Devedor. ";
                            irregular = true;
                        }
                        if (item.MUNICIPIO == "" || item.MUNICIPIO == null)
                        {
                            irregularidade = irregularidade += "- Município Devedor. ";
                            irregular = true;
                        }
                        if (item.UF == "" || item.UF == null)
                        {
                            irregularidade = irregularidade += "- UF Devedor. ";
                            irregular = true;
                        }
                    }
                }
            else
            {
                irregularidade = irregularidade += "- Ausência de Devedor. ";
                irregular = true;
            }



            if (titulo.DT_PROTOCOLO.ToShortDateString() == "01/01/0001" || titulo.DT_PROTOCOLO.ToString() == "" || titulo.DT_PROTOCOLO == null)
            {
                irregularidade = irregularidade += "- Data Protocolo. ";
                irregular = true;
            }

            if (titulo.DT_ENTRADA.ToShortDateString() == "01/01/0001" || titulo.DT_ENTRADA.ToString() == "" || titulo.DT_ENTRADA == null)
            {
                irregularidade = irregularidade += "- Data Entrada. ";
                irregular = true;
            }
            if (titulo.PROTOCOLO.ToString() == "")
            {
                irregularidade = irregularidade += "- Número Protocolo. ";
                irregular = true;
            }





            if (irregular == true)
                return string.Format("{0} : {1}", numeroProtocolo, irregularidade);
            else
                return "";
        }





        public void ObterDataTableTitulosPorPeriodo(DateTime dataInicio, DateTime dataFim)
        {

            string dataIni = string.Format("{0:0000}.{1:00}.{2:00}", dataInicio.Year, dataInicio.Month, dataInicio.Day);

            string dataFinal = string.Format("{0:0000}.{1:00}.{2:00}", dataFim.Year, dataFim.Month, dataFim.Day);

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {
                try
                {
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from titulos where (status = 'PROTESTADO' AND DT_REGISTRO between '{0}' AND '{1}') or (PROTESTADO = 'S' AND DT_PAGAMENTO between '{0}' AND '{1}') or (status = 'RETIRADO' AND DT_RETIRADO between '{0}' AND '{1}') or (PROTESTADO = 'N' AND DT_PAGAMENTO between '{0}' AND '{1}') or (status = 'SUSTADO' AND DT_SUSTADO between '{0}' AND '{1}') or (status = 'DEVOLVIDO' AND DT_DEVOLVIDO between '{0}' AND '{1}')", dataIni, dataFinal);


                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        dr = cmdTotal.ExecuteReader();

                        while (dr.Read())
                        {
                            total++;
                        }

                    }


                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;



                        dr = cmdTotal.ExecuteReader();


                        ConverterDataRowEmObjetoTituloProtesto(dr);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

        }

        private void ObterTiposTitulo()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {
                try
                {

                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from TIPOS");

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        dr = cmdTotal.ExecuteReader();

                        while (dr.Read())
                        {
                            TiposTitulos tipo = new TiposTitulos();
                            tipo.CODIGO = Convert.ToInt32(dr["CODIGO"]);
                            tipo.DESCRICAO = dr["DESCRICAO"].ToString();
                            tipo.SIGLA = dr["SIGLA"].ToString();
                            tipos.Add(tipo);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }


        private void ObterPortadores()
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {
                try
                {
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from portadores");

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        dr = cmdTotal.ExecuteReader();

                        while (dr.Read())
                        {
                            Portadores portador = new Portadores();

                            portador.AGENCIA = dr["AGENCIA"].ToString();
                            portador.BANCO = dr["BANCO"].ToString();
                            portador.CODIGO = dr["CODIGO"].ToString();
                            portador.CONTA = dr["CONTA"].ToString();
                            portador.CONVENIO = dr["CONVENIO"].ToString();
                            portador.CRA = dr["CRA"].ToString();
                            portador.DOCUMENTO = dr["DOCUMENTO"].ToString();
                            portador.ENDERECO = dr["ENDERECO"].ToString();
                            portador.ESPECIE = dr["ESPECIE"].ToString();
                            portador.FORCA_LEI = dr["FORCA_LEI"].ToString();
                            portador.ID_PORTADOR = (int)dr["ID_PORTADOR"];
                            portador.NOME = dr["NOME"].ToString();
                            portador.NOMINAL = dr["NOMINAL"].ToString();
                            portador.OBSERVACAO = dr["OBSERVACAO"].ToString();
                            portador.PAGAMENTO_ANTECIPADO = dr["PAGAMENTO_ANTECIPADO"].ToString();
                            portador.PRACA = dr["PRACA"].ToString();
                            if (dr["SEQUENCIA"].ToString() != "")
                                portador.SEQUENCIA = (int)dr["SEQUENCIA"];
                            portador.TIPO = dr["TIPO"].ToString();


                            if (dr["VALOR_DOC"].ToString() != "" || dr["VALOR_DOC"] == null)
                                portador.VALOR_DOC = Convert.ToDecimal(dr["VALOR_DOC"]);
                            if (dr["VALOR_TED"].ToString() != "" || dr["VALOR_TED"] == null)
                                portador.VALOR_TED = Convert.ToDecimal(dr["VALOR_TED"]);


                            portadores.Add(portador);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        public void ConverterEmObjetoSacador(int id_ato)
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {
                Sacadores sacador = new Sacadores();
                try
                {
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from sacadores where id_ato = " + id_ato);

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        dr = cmdTotal.ExecuteReader();
                        while (dr.Read())
                        {
                            sacador.BAIRRO = dr["BAIRRO"].ToString();
                            sacador.CEP = dr["CEP"].ToString();
                            sacador.DOCUMENTO = dr["DOCUMENTO"].ToString();
                            sacador.ENDERECO = dr["ENDERECO"].ToString();
                            sacador.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);
                            sacador.ID_SACADOR = Convert.ToInt32(dr["ID_SACADOR"]);
                            sacador.MUNICIPIO = dr["MUNICIPIO"].ToString();
                            sacador.NOME = dr["NOME"].ToString();
                            sacador.ORDEM = Convert.ToInt32(dr["ORDEM"]);
                            sacador.TIPO = dr["TIPO"].ToString();
                            sacador.UF = dr["UF"].ToString();

                            sacadores.Add(sacador);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }


        public void ConverterEmObjetoDevedor(int id_ato)
        {
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {
                Devedores devedor = new Devedores();
                try
                {
                    string comando = string.Empty;
                    conn.Open();

                    comando = string.Format("select * from devedores where id_ato = " + id_ato);

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;

                        dr = cmdTotal.ExecuteReader();
                        while (dr.Read())
                        {
                            devedor.BAIRRO = dr["BAIRRO"].ToString();
                            devedor.CEP = dr["CEP"].ToString();

                            if (dr["DATA_CARTA"].ToString() != "")
                                devedor.DATA_CARTA = Convert.ToDateTime(dr["DATA_CARTA"]);

                            if (dr["DATA_EDITAL"].ToString() != "")
                                devedor.DATA_EDITAL = Convert.ToDateTime(dr["DATA_EDITAL"]);

                            if (dr["DATA_PESSOAL"].ToString() != "")
                                devedor.DATA_PESSOAL = Convert.ToDateTime(dr["DATA_PESSOAL"]);

                            if (dr["DATA_PUBLICACAO"].ToString() != "")
                                devedor.DATA_PUBLICACAO = Convert.ToDateTime(dr["DATA_PUBLICACAO"]);

                            devedor.DOCUMENTO = dr["DOCUMENTO"].ToString();

                            if (dr["DT_EMISSAO"].ToString() != "")
                                devedor.DT_EMISSAO = Convert.ToDateTime(dr["DT_EMISSAO"]);

                            devedor.ENDERECO = dr["ENDERECO"].ToString();
                            devedor.ID_ATO = Convert.ToInt32(dr["ID_ATO"]);
                            devedor.ID_DEVEDOR = Convert.ToInt32(dr["ID_DEVEDOR"]);
                            devedor.IDENTIDADE = dr["IDENTIDADE"].ToString();
                            devedor.IFP_DETRAN = dr["IFP_DETRAN"].ToString();
                            devedor.IGNORADO = dr["IGNORADO"].ToString();
                            devedor.INTIMADO_CARTA = dr["INTIMADO_CARTA"].ToString();
                            devedor.INTIMADO_EDITAL = dr["INTIMADO_EDITAL"].ToString();
                            devedor.INTIMADO_PESSOAL = dr["INTIMADO_PESSOAL"].ToString();
                            devedor.JUSTIFICATIVA = dr["JUSTIFICATIVA"].ToString();
                            devedor.MOTIVO_INTIMACAO = dr["MOTIVO_INTIMACAO"].ToString();
                            devedor.MUNICIPIO = dr["MUNICIPIO"].ToString();
                            devedor.NOME = dr["NOME"].ToString();

                            if (dr["ORDEM"].ToString() != "")
                                devedor.ORDEM = Convert.ToInt32(dr["ORDEM"]);

                            devedor.ORGAO = dr["ORGAO"].ToString();
                            devedor.TELEFONE = dr["TELEFONE"].ToString();
                            devedor.TIPO = dr["TIPO"].ToString();
                            devedor.UF = dr["UF"].ToString();
                            devedores.Add(devedor);
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Ocorreu um erro durante a consulta dos títulos", "Atenção", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }




        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
                _cenprot.qtdTitulosProtestados.Content = string.Format("Protestados: {0}", titulosProtestados);
                _cenprot.qtdTitulosCancelados.Content = string.Format("Cancelados: {0}", titulosCancelados);
                _cenprot.qtdTitulosPagos.Content = string.Format("Pagos: {0}", titulosPagos);
                _cenprot.qtdTitulosRetirados.Content = string.Format("Retirados: {0}", titulosRetirados);
                _cenprot.qtdTitulosSustados.Content = string.Format("Sustados: {0}", titulosSustados);
                _cenprot.qtdTitulosDevolvidos.Content = string.Format("Devolvido: {0}", titulosDevolvidos);



                string[] documento = new string[4];
                string cpfApresentante = string.Format("CPF Apresentante: {0}", _cenprot.titulos.Where(p => p.TIPO_APRESENTANTE == "F").Count());
                string cnpjApresentante = string.Format("CNPJ Apresentante: {0}", _cenprot.titulos.Where(p => p.TIPO_APRESENTANTE == "J").Count());
                string cpfDevedor = string.Format("CPF Devedor: {0}", _cenprot.titulos.Where(p => p.TIPO_DEVEDOR == "F").Count());
                string cnpjDevedor = string.Format("CNPJ Devedor: {0}", _cenprot.titulos.Where(p => p.TIPO_DEVEDOR == "J").Count());

                documento[0] = cpfDevedor;
                documento[1] = cnpjDevedor;
                documento[2] = cpfApresentante;
                documento[3] = cnpjApresentante;
                _cenprot.listViewDocumento.ItemsSource = documento;


                List<string> especies = new List<string>();
                especies = _cenprot.titulos.Select(p => p.Especie_Titulo).Distinct().ToList();

                string[] esp = new string[especies.Count];

                for (int i = 0; i < especies.Count; i++)
                {
                    esp[i] = string.Format("{0} : {1}", especies[i], _cenprot.titulos.Where(p => p.Especie_Titulo == especies[i]).Count());
                }
                _cenprot.listViewEspecies.ItemsSource = esp;
                _cenprot.QtdEspecies.Content = string.Format("Espécies dos Títulos: {0}", especies.Count());

                List<string> apresent = new List<string>();
                apresent = _cenprot.titulos.Select(p => p.APRESENTANTE).Distinct().ToList();

                string[] apres = new string[apresent.Count];

                for (int i = 0; i < apresent.Count; i++)
                {
                    apres[i] = string.Format("{0} : {1}", apresent[i], _cenprot.titulos.Where(p => p.APRESENTANTE == apresent[i]).Count());
                }
                _cenprot.listViewApresentantes.ItemsSource = apres;
                _cenprot.QtdApresentantes.Content = string.Format("Apresentantes: {0}", apresent.Count());


                _cenprot.listViewIrregularidades.ItemsSource = listaIrregularidades;
                _cenprot.dataGridConsulta.Items.Refresh();
                _cenprot.QtdIrrgularidades.Content = string.Format("Irregularidades : {0}", listaIrregularidades.Count);

                _cenprot.dataGridConsulta.ItemsSource = _cenprot.titulos.OrderBy(p => p.STATUS);
            

            this.Close();
        }



    }
}
