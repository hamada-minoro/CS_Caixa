using CS_Caixa.Models;
using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassImportarAtosTotal
    {
        int ano = DateTime.Now.Year;

        List<CustasProtesto> listaCustas = new List<CustasProtesto>();
        ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();
        ClassCustasNotas classCustasNotas = new ClassCustasNotas();
        ClassCustasRgi classCustasRgi = new ClassCustasRgi();
        decimal porcentagemIss = 0;
        ClassAto classAto;
        List<Ato> atosLancados;


        public ClassImportarAtosTotal()
        {
            classAto = new ClassAto();
            atosLancados = new List<Ato>();
        }


        private int ObeterProtocoloEntrada(string idAto)
        {
            int retorno = 0;
            try
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando = string.Format("select protocolo from titulos where id_ato = '{0}'", idAto);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;


                        retorno = Convert.ToInt32(cmdTotal.ExecuteScalar());
                    }
                }

                return retorno;
            }
            catch (Exception)
            {
                return retorno;
            }
        }


        public List<Ato> ObterReciboProtesto(DateTime dataAto, Usuario usuarioLogado)
        {

            Ato ato;

            atosLancados = classAto.ListarAtoDataAto(dataAto, dataAto, "PROTESTO").Where(P => P.TipoAto == "CERTIDÃO PROTESTO" || P.TipoAto == "CANCELAMENTO").ToList();

            var atosNaoLancados = new List<Ato>();
            
            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {

                string data = dataAto.ToShortDateString().Replace("/", ".");

                string comando = string.Format("select * from certidoes where dt_pedido = '{0}'", data);


                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;


                    conn.Open();
                    dr = cmdTotal.ExecuteReader();



                    while (dr.Read())
                    {
                        if (dr["RECIBO"].ToString() != "")
                        {
                            if (atosLancados.Where(p => p.Recibo == Convert.ToInt32(dr["RECIBO"])).Count() == 0)
                            {

                                ato = new Ato();

                                // data do pagamento
                                ato.DataPagamento = Convert.ToDateTime(dr["DT_PEDIDO"]);

                                ato.FolhaInical = 1;                                    

                                // data do ato
                                ato.DataAto = Convert.ToDateTime(dr["DT_PEDIDO"]);

                                // pago
                                ato.Pago = false;

                                // convenio
                                ato.Convenio = dr["CONVENIO"].ToString();

                                ato.DescricaoAto = dr["ID_CERTIDAO"].ToString();

                                // Número do recibo
                                ato.Recibo = Convert.ToInt32(dr["RECIBO"]);

                                //Numero protocolo
                                ato.Protocolo = ObeterProtocoloEntrada(dr["ID_ATO"].ToString());
                                
                                // id usuario
                                ato.IdUsuario = usuarioLogado.Id_Usuario;

                                // Usuario
                                ato.Usuario = usuarioLogado.NomeUsu;

                                // Atribuiçao
                                ato.Atribuicao = "PROTESTO";
                                
                                //idReciboBalcao
                                ato.IdReciboBalcao = 0;

                                // Recibo Balcao
                                ato.ReciboBalcao = 0;

                               
                                ato.Portador = dr["REQUERENTE"].ToString();

                                if (dr["TIPO_CERTIDAO"].ToString() == "E")
                                {
                                    ato.TipoAto = "CERTIDÃO PROTESTO";
                                    if (dr["REQUERENTE"].ToString() == "SERASA")
                                        ato.Natureza = "CERTIDÃO SERASA";
                                    else
                                        ato.Natureza = "CERTIDÃO BOA VISTA";

                                    ato.Faixa = (Convert.ToInt32(dr["CANCELADOS"]) + Convert.ToInt32(dr["PROTESTADOS"])).ToString();
                                }
                                else if (dr["TIPO_CERTIDAO"].ToString() == "X")
                                {
                                    ato.Natureza = "CANCELAMENTO";
                                    ato.TipoAto = "CANCELAMENTO";
                                }
                                else if (dr["TIPO_CERTIDAO"].ToString() == "I")
                                {
                                    ato.Natureza = "CERTIDÃO PROTESTO";
                                    ato.TipoAto = "CERTIDÃO PROTESTO";
                                }
                                else if (dr["TIPO_CERTIDAO"].ToString() == "N")
                                {
                                    ato.Natureza = "CERTIDÃO PROTESTO";
                                    ato.TipoAto = "CERTIDÃO PROTESTO";
                                }

                                // TipoCobranca
                                switch (dr["COBRANCA"].ToString())
                                {
                                    case "CC":
                                        ato.TipoCobranca = "COM COBRANÇA";
                                        break;
                                    case "SC":
                                        ato.TipoCobranca = "SEM COBRANÇA";
                                        break;
                                    case "JG":
                                        ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                        break;
                                    default:
                                        ato.TipoCobranca = "NIHILL";
                                        break;
                                }

                                //Emolumentos
                                ato.Emolumentos = Convert.ToDecimal(dr["EMOLUMENTOS"].ToString());


                                //Fetj
                                ato.Fetj = Convert.ToDecimal(dr["FETJ"].ToString());

                                //Fundperj
                                ato.Fundperj = Convert.ToDecimal(dr["FUNDPERJ"].ToString());


                                //Funperj
                                ato.Funperj = Convert.ToDecimal(dr["FUNPERJ"].ToString());


                                //Funarpen
                                ato.Funarpen = Convert.ToDecimal(dr["FUNARPEN"].ToString());

                                // Pmcmv
                                ato.Pmcmv = Convert.ToDecimal(dr["PMCMV"].ToString());

                                //ISS
                                if (ato.Natureza == "CERTIDÃO SERASA")
                                    ato.Iss = ato.Fundperj;
                                else
                                    ato.Iss = Convert.ToDecimal(dr["ISS"]);



                                // Mutua
                                ato.Mutua = Convert.ToDecimal(dr["MUTUA"].ToString());


                                // Acoterj
                                ato.Acoterj = Convert.ToDecimal(dr["ACOTERJ"].ToString());


                                // Tarifa
                                ato.Bancaria = 0;


                                // Total
                                ato.Total = ato.Emolumentos + ato.Fetj + ato.Fundperj + ato.Funperj + ato.Funarpen + ato.Pmcmv + ato.Iss + ato.Mutua + ato.Acoterj;




                                if (ato.TipoAto == "CANCELAMENTO")
                                {
                                    int idTotal = Convert.ToInt32(dr["ID_ATO"]);
                                    Ato apontamento = ObterAtoApontamentoDoCancelamento(idTotal, ato.TipoCobranca, ato.Recibo, ato.DataPagamento);

                                    if (apontamento.TipoAto == "APONTAMENTO CANCELAMENTO")
                                        atosNaoLancados.Add(apontamento);

                                    ato.FichaAto = apontamento.Portador;
                                }


                                atosNaoLancados.Add(ato);


                                if (dr["EXCOBRANCA"].ToString() != "")
                                {
                                    if (dr["RECIBO"].ToString() != "")
                                    {
                                        if (atosLancados.Where(p => p.Recibo == Convert.ToInt32(dr["RECIBO"])).Count() == 0)
                                        {

                                            ato = new Ato();

                                            // data do pagamento
                                            ato.DataPagamento = Convert.ToDateTime(dr["DT_PEDIDO"]);


                                            // data do ato
                                            ato.DataAto = Convert.ToDateTime(dr["DT_PEDIDO"]);

                                            // pago
                                            ato.Pago = false;

                                            ato.FolhaInical = Convert.ToInt32(dr["FOLHAS"]);

                                            // convenio
                                            ato.Convenio = dr["CONVENIO"].ToString();

                                            ato.DescricaoAto = dr["ID_CERTIDAO"].ToString();

                                            // Número do recibo
                                            ato.Recibo = Convert.ToInt32(dr["RECIBO"]);

                                            //Numero protocolo
                                            ato.Protocolo = Convert.ToInt32(dr["RECIBO"]);


                                            // id usuario
                                            ato.IdUsuario = usuarioLogado.Id_Usuario;

                                            // Usuario
                                            ato.Usuario = usuarioLogado.NomeUsu;

                                            // Atribuiçao
                                            ato.Atribuicao = "PROTESTO";


                                            //idReciboBalcao
                                            ato.IdReciboBalcao = 0;

                                            // Recibo Balcao
                                            ato.ReciboBalcao = 0;

                                            // TipoAto

                                            ato.TipoAto = "CERTIDÃO PROTESTO";
                                            ato.Portador = dr["REQUERENTE"].ToString();
                                            // Natureza
                                            ato.Natureza = "FOLHA EXCEDENTE";


                                            // TipoCobranca
                                            switch (dr["EXCOBRANCA"].ToString())
                                            {
                                                case "CC":
                                                    ato.TipoCobranca = "COM COBRANÇA";
                                                    break;
                                                case "SC":
                                                    ato.TipoCobranca = "SEM COBRANÇA";
                                                    break;
                                                case "JG":
                                                    ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                                    break;
                                                default:
                                                    ato.TipoCobranca = "NIHILL";
                                                    break;
                                            }

                                            //Emolumentos
                                            ato.Emolumentos = Convert.ToDecimal(dr["EXEMOLUMENTOS"].ToString());


                                            //Fetj
                                            ato.Fetj = Convert.ToDecimal(dr["EXFETJ"].ToString());

                                            //Fundperj
                                            ato.Fundperj = Convert.ToDecimal(dr["EXFUNDPERJ"].ToString());


                                            //Funperj
                                            ato.Funperj = Convert.ToDecimal(dr["EXFUNPERJ"].ToString());


                                            //Funarpen
                                            ato.Funarpen = Convert.ToDecimal(dr["EXFUNARPEN"].ToString());



                                            //ISS
                                            ato.Iss = Convert.ToDecimal(dr["EXISS"]);


                                            // Tarifa
                                            ato.Bancaria = 0;


                                            // Total
                                            ato.Total = Convert.ToDecimal(dr["EXTOTAL"].ToString());


                                            atosNaoLancados.Add(ato);
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }


            return atosNaoLancados;
        }


        private string ObterPagamentoAntecipado(string apresentante)
        {

            string retorno = string.Empty;
            try
            {
                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando = string.Format("select pagamento_antecipado from portadores where nome = '{0}'", apresentante);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;


                        retorno = cmdTotal.ExecuteScalar().ToString();
                    }
                }

                return retorno;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private Ato ObterAtoApontamentoDoCancelamento(int idAto, string tipoCobranca, int? recibo, DateTime dataRecibo)
        {
            Ato atoApontamento = new Ato();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
            {

                string comando = string.Format("select * from titulos where id_ato = {0}", idAto);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();


                    while (dr.Read())
                    {

                        atoApontamento = new Ato();

                        atoApontamento.DataPagamento = dataRecibo;

                        atoApontamento.DataAto = dataRecibo;

                        atoApontamento.Protocolo = Convert.ToInt32(dr["PROTOCOLO"]);

                        atoApontamento.Atribuicao = "PROTESTO";

                        atoApontamento.Pago = false;

                        atoApontamento.Convenio = dr["CONVENIO"].ToString();

                        atoApontamento.TipoCobranca = tipoCobranca;

                        string dataInicial = dr["DT_VENCIMENTO"].ToString().Replace(".", "/");
                        string dataFinal = dr["DT_ENTRADA"].ToString().Replace(".", "/");

                        int totalDias = 0;

                        if (dataFinal != "" && dataInicial != "")
                            totalDias = (DateTime.Parse(dataFinal).Subtract(DateTime.Parse(dataInicial))).Days + 1;

                        string postecipado = dr["POSTECIPADO"].ToString();

                        if (postecipado == "P" && totalDias < 366)
                            atoApontamento.TipoAto = "APONTAMENTO CANCELAMENTO";
                        else
                        {
                            if (atoApontamento.Convenio == "S")
                            {
                                if (ObterPagamentoAntecipado(dr["APRESENTANTE"].ToString()) != "S")
                                    atoApontamento.TipoAto = "APONTAMENTO CANCELAMENTO";
                                else
                                    atoApontamento.TipoAto = "CANCELAMENTO";
                            }
                            else
                                atoApontamento.TipoAto = "CANCELAMENTO";


                        }
                        atoApontamento.DescricaoAto = dr["ID_ATO"].ToString();

                        atoApontamento.Faixa = Faixa(Convert.ToDouble(dr["SALDO_TITULO"]));

                        atoApontamento.ValorTitulo = Convert.ToDecimal(dr["SALDO_TITULO"]);

                        atoApontamento.Recibo = recibo;

                        atoApontamento.Portador = dr["APRESENTANTE"].ToString();

                        decimal emol = 0;
                        decimal fetj_20 = 0;
                        decimal fundperj_5 = 0;
                        decimal funperj_5 = 0;
                        decimal funarpen_4 = 0;
                        decimal pmcmv_2 = 0;
                        decimal iss = 0;

                        string Semol = "0,00";
                        string Sfetj_20 = "0,00";
                        string Sfundperj_5 = "0,00";
                        string Sfunperj_5 = "0,00";
                        string Sfunarpen_4 = "0,00";
                        string Spmcmv_2 = "0,00";
                        string Siss = "0,00";
                        int index;




                        try
                        {

                            listaCustas = classCustasProtesto.ListaCustas();

                            porcentagemIss = Convert.ToDecimal(listaCustas.Where(p => p.DESCR == "PORCENTAGEM ISS" && p.ANO == ano).Select(p => p.VALOR).FirstOrDefault());

                            listaCustas = listaCustas.Where(p => p.ANO == ano && p.TIPO == "P").OrderBy(p => p.ORDEM).Select(p => p).ToList();


                            CustasProtesto custas = listaCustas.Where(p => p.SUB == atoApontamento.Faixa).FirstOrDefault();
                            atoApontamento.Natureza = custas.DESCR;
                            emol = Convert.ToDecimal(custas.VALOR);
                            if (tipoCobranca == "COM COBRANÇA" || tipoCobranca == "NIHILL")
                            {
                                fetj_20 = emol * 20 / 100;
                                fundperj_5 = emol * 5 / 100;
                                funperj_5 = emol * 5 / 100;
                                funarpen_4 = emol * 4 / 100;

                                //iss = (100 - porcentagemIss) / 100;
                                //iss = emol / iss - emol;

                                iss = emol * porcentagemIss / 100;

                                pmcmv_2 = Convert.ToDecimal(emol * 2) / 100;

                                if (tipoCobranca == "COM COBRANÇA")
                                {
                                    Semol = Convert.ToString(emol);
                                }
                                Sfetj_20 = Convert.ToString(fetj_20);
                                Sfundperj_5 = Convert.ToString(fundperj_5);
                                Sfunperj_5 = Convert.ToString(funperj_5);
                                Sfunarpen_4 = Convert.ToString(funarpen_4);
                                Spmcmv_2 = Convert.ToString(pmcmv_2);
                                Siss = Convert.ToString(iss);

                            }


                            index = Semol.IndexOf(',');
                            Semol = Semol.Substring(0, index + 3);

                            index = Sfetj_20.IndexOf(',');
                            Sfetj_20 = Sfetj_20.Substring(0, index + 3);


                            index = Sfundperj_5.IndexOf(',');
                            Sfundperj_5 = Sfundperj_5.Substring(0, index + 3);

                            index = Sfunperj_5.IndexOf(',');
                            Sfunperj_5 = Sfunperj_5.Substring(0, index + 3);

                            index = Sfunarpen_4.IndexOf(',');
                            Sfunarpen_4 = Sfunarpen_4.Substring(0, index + 3);

                            index = Spmcmv_2.IndexOf(',');
                            Spmcmv_2 = Spmcmv_2.Substring(0, index + 3);

                            index = Siss.IndexOf(',');
                            Siss = Siss.Substring(0, index + 3);

                            atoApontamento.Emolumentos = Convert.ToDecimal(Semol);

                            atoApontamento.Fetj = Convert.ToDecimal(Sfetj_20);

                            atoApontamento.Fundperj = Convert.ToDecimal(Sfundperj_5);

                            atoApontamento.Funperj = Convert.ToDecimal(Sfunperj_5);

                            atoApontamento.Funarpen = Convert.ToDecimal(Sfunarpen_4);

                            atoApontamento.Pmcmv = Convert.ToDecimal(Spmcmv_2);

                            atoApontamento.Iss = Convert.ToDecimal(Siss);

                            atoApontamento.Mutua = Convert.ToDecimal(dr["MUTUA"]);

                            atoApontamento.Acoterj = Convert.ToDecimal(dr["ACOTERJ"]);

                            atoApontamento.Distribuicao = Convert.ToDecimal(dr["DISTRIBUICAO"]);

                            atoApontamento.Total = atoApontamento.Emolumentos + atoApontamento.Fetj + atoApontamento.Fundperj +
                                atoApontamento.Funperj + atoApontamento.Funarpen + atoApontamento.Pmcmv + atoApontamento.Iss +
                                atoApontamento.Mutua + atoApontamento.Acoterj + atoApontamento.Distribuicao;

                        }
                        catch (Exception)
                        {

                        }

                    }
                }
            }
            return atoApontamento;
        }

        public void SalvarItensCustas(int idAtoTotal, int idAtoCaixa, int? folhas, string tipoAto)
        {

            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();
            CustasProtesto emolumentos = new CustasProtesto();
            listaCustas = classCustasProtesto.ListaCustas();

            if (tipoAto == "CERTIDÃO PROTESTO")
            {              

                if (folhas == 1)
                {
                    emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS" && p.ANO == DateTime.Now.Date.Year).FirstOrDefault();

                    ItensCustasProtesto novoIten = new ItensCustasProtesto();
                    novoIten.Id_Ato = idAtoCaixa;
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    classCustasProtesto.SalvarItensLista(novoIten);

                    emolumentos = new CustasProtesto();

                    emolumentos = listaCustas.Where(p => p.DESCR == "BUSCAS EM LIVROS OU PAPÉIS" && p.ANO == DateTime.Now.Date.Year).FirstOrDefault();
                    novoIten = new ItensCustasProtesto();
                    novoIten.Id_Ato = idAtoCaixa;
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = "1";
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    classCustasProtesto.SalvarItensLista(novoIten);

                }
                else
                {
                    emolumentos = listaCustas.Where(p => p.DESCR == "CERTIDÕES EXTRAÍDAS DE LIVROS" && p.ANO == DateTime.Now.Date.Year).FirstOrDefault();

                    ItensCustasProtesto novoIten = new ItensCustasProtesto();
                    novoIten.Id_Ato = idAtoCaixa;
                    novoIten.Item = emolumentos.ITEM;
                    novoIten.SubItem = emolumentos.SUB;
                    novoIten.Tabela = emolumentos.TAB;
                    novoIten.Descricao = emolumentos.TEXTO;
                    novoIten.Quantidade = (folhas - 1).ToString();
                    novoIten.Valor = emolumentos.VALOR;
                    novoIten.Total = novoIten.Valor * Convert.ToInt16(novoIten.Quantidade);
                    classCustasProtesto.SalvarItensLista(novoIten);

                }


            }
            else
            {

                using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingProtesto))
                {

                    string comando = string.Format("select * from custas where id_ato = {0}", idAtoTotal);
                    conn.Open();

                    using (FbCommand cmdTotal = new FbCommand(comando, conn))
                    {
                        cmdTotal.CommandType = CommandType.Text;
                        FbDataReader dr = cmdTotal.ExecuteReader();
                        ItensCustasProtesto item;
                        while (dr.Read())
                        {

                            item = new ItensCustasProtesto();

                            item.Id_Ato = idAtoCaixa;

                            item.Tabela = dr["TABELA"].ToString();

                            item.Item = dr["ITEM"].ToString();

                            item.SubItem = dr["SUBITEM"].ToString();

                            item.Quantidade = dr["QTD"].ToString();

                            item.Valor = Convert.ToDecimal(dr["VALOR"]);

                            item.Total = Convert.ToDecimal(dr["TOTAL"]);

                            item.Descricao = "ATO IMPORTADO DA TOTAL";

                            classCustasProtesto.SalvarItensLista(item);
                        }
                    }
                }
            }
        }


        public void SalvarItensCustasNotas(List<ItensCustasNota> custas, int idato)
        {
            ItensCustasNota itemNovo;
            foreach (var item in custas)
            {
                itemNovo = new ItensCustasNota();

                itemNovo.Id_Ato = idato;

                itemNovo.Tabela = item.Tabela;

                itemNovo.Item = item.Item;

                itemNovo.SubItem = item.SubItem;

                itemNovo.Quantidade = item.Quantidade;

                itemNovo.Valor = item.Valor;

                itemNovo.Total = item.Total;

                itemNovo.Descricao = item.Descricao;

               classCustasNotas.SalvarItensLista(itemNovo);
            }

        }

        public void SalvarItensCustasRgi(List<ItensCustasRgi> custas, int idato)
        {
            ItensCustasRgi itemNovo;
            foreach (var item in custas)
            {
                itemNovo = new ItensCustasRgi();

                itemNovo.Id_Ato = idato;

                itemNovo.Tabela = item.Tabela;

                itemNovo.Item = item.Item;

                itemNovo.SubItem = item.SubItem;

                itemNovo.Quantidade = item.Quantidade;

                itemNovo.Valor = item.Valor;

                itemNovo.Total = item.Total;

                itemNovo.Descricao = item.Descricao;

                classCustasRgi.SalvarItensLista(itemNovo);
            }

        }


        public List<ItensCustasNota> ObterCustasNotas(int idato)
        {

            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();
            listaCustas = classCustasProtesto.ListaCustas();     

            List<ItensCustasNota> custas = new List<ItensCustasNota>();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingReciboNotas))
            {

                string comando = string.Format("select * from custas where id_ato = {0}", idato);


                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;
                    conn.Open();
                    dr = cmdTotal.ExecuteReader();



                    while (dr.Read())
                    {
                        ItensCustasNota itemCusta = new ItensCustasNota();
                                                
                        itemCusta.Tabela = dr["TABELA"].ToString();

                        itemCusta.Item = dr["ITEM"].ToString();

                        itemCusta.SubItem = dr["SUBITEM"].ToString();

                        itemCusta.Valor = Convert.ToDecimal(dr["VALOR"]);

                        itemCusta.Quantidade = dr["QTD"].ToString();

                        itemCusta.Total = Convert.ToDecimal(dr["TOTAL"]);

                        itemCusta.Descricao = "IMPORTADO TOTAL";

                        custas.Add(itemCusta);
                    }

                }

            }

            return custas;
        }

        public List<ItensCustasRgi> ObterCustasRgi(int idato)
        {
            ClassCustasProtesto classCustasProtesto = new ClassCustasProtesto();
            listaCustas = classCustasProtesto.ListaCustas();     

            List<ItensCustasRgi> custas = new List<ItensCustasRgi>();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingReciboRgi))
            {

                string comando = string.Format("select * from custas where id_ato = {0}", idato);


                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;
                    conn.Open();
                    dr = cmdTotal.ExecuteReader();

                    int cont = 0;

                    while (dr.Read())
                    {
                        

                        ItensCustasRgi itemCusta = new ItensCustasRgi();

                        itemCusta.Tabela = dr["TABELA"].ToString();

                        itemCusta.Item = dr["ITEM"].ToString();

                        itemCusta.SubItem = dr["SUBITEM"].ToString();

                        itemCusta.Valor = Convert.ToDecimal(dr["VALOR"]);

                        itemCusta.Quantidade = dr["QTD"].ToString();

                        itemCusta.Total = Convert.ToDecimal(dr["TOTAL"]);
                                                
                        itemCusta.Descricao = "IMPORTADO DA TOTAL";

                        itemCusta.Cont = cont + 1;

                        cont = Convert.ToInt32(itemCusta.Cont);

                        custas.Add(itemCusta);
                    }

                }

            }

            return custas;
        }

        public string Faixa(double Valor)
        {
            string letra = string.Empty;

            if (Valor <= 50.00)
            {
                letra = "A";
            }
            else if (Valor <= 100.00)
            {
                letra = "B";
            }
            else if (Valor <= 150.00)
            {
                letra = "C";
            }
            else if (Valor <= 200.00)
            {
                letra = "D";
            }
            else if (Valor <= 250.00)
            {
                letra = "E";
            }
            else if (Valor <= 300.00)
            {
                letra = "F";
            }
            else if (Valor <= 350.00)
            {
                letra = "G";
            }
            else if (Valor <= 400.00)
            {
                letra = "H";
            }
            else if (Valor <= 450.00)
            {
                letra = "I";
            }
            else if (Valor <= 500.00)
            {
                letra = "J";
            }
            else if (Valor <= 600.00)
            {
                letra = "K";
            }
            else if (Valor <= 700.00)
            {
                letra = "L";
            }
            else if (Valor <= 800.00)
            {
                letra = "M";
            }
            else if (Valor <= 900.00)
            {
                letra = "N";
            }
            else if (Valor <= 1000.00)
            {
                letra = "O";
            }
            else if (Valor <= 1500.00)
            {
                letra = "P";
            }
            else if (Valor <= 2000.00)
            {
                letra = "Q";
            }
            else if (Valor <= 2500.00)
            {
                letra = "R";
            }
            else if (Valor <= 3000.00)
            {
                letra = "S";
            }
            else if (Valor <= 3500.00)
            {
                letra = "T";
            }
            else if (Valor <= 4000.00)
            {
                letra = "U";
            }
            else if (Valor <= 4500.00)
            {
                letra = "V";
            }
            else if (Valor <= 5000.00)
            {
                letra = "W";
            }
            else if (Valor <= 7500.00)
            {
                letra = "X";
            }
            else if (Valor <= 10000.00)
            {
                letra = "Y";
            }
            else if (Valor > 10000.00)
            {
                letra = "Z";
            }

            return letra;
        }


        public List<Ato> ObterReciboNotas(DateTime dataAto, Usuario usuarioLogado)
        {
            Ato ato;

            atosLancados = classAto.ListarAtoDataAto(dataAto, dataAto, "NOTAS").ToList();

            var atosNaoLancadosNotas = new List<Ato>();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingReciboNotas))
            {

                string data = dataAto.ToShortDateString().Replace("/", ".");

                string comando = string.Format("select * from talao where dt_entrada = '{0}' and STATUS = 'EN'", data);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();



                    while (dr.Read())
                    {

                        if (atosLancados.Where(p => p.Recibo == Convert.ToInt32(dr["RECIBO"])).Count() == 0)
                        {

                            ato = new Ato();

                            ato.Recibo = Convert.ToInt32(dr["RECIBO"]);

                            ato.DataPagamento = Convert.ToDateTime(dr["DT_ENTRADA"]);

                            ato.DataAto = Convert.ToDateTime(dr["DT_ENTRADA"]);

                            // id usuario
                            ato.IdUsuario = usuarioLogado.Id_Usuario;

                            // Usuario
                            ato.Usuario = usuarioLogado.NomeUsu;

                            ato.Atribuicao = "NOTAS";

                            ato.Portador = dr["APRESENTANTE"].ToString();

                            ato.Pago = false;

                            //idReciboBalcao
                            ato.IdReciboBalcao = 0;

                            // Recibo Balcao
                            ato.ReciboBalcao = 0;


                            ato.Id_Ato = Convert.ToInt32(dr["ID_TALAO"]);

                            ato.Faixa = dr["DESCRICAO_GERAL"].ToString();

                            ato.QtdAtos = Convert.ToInt32(dr["QTD_ATOS"]);

                            ato.QuantDistrib = 0;

                            ato.QuantPrenotacao = Convert.ToInt32(dr["QTD_PRENOTACAO"]);

                            ato.Prenotacao = Convert.ToInt32(dr["VALOR_PRENOTACAO"]);

                            ato.QuantIndisp = Convert.ToInt32(dr["QTD_BUSCAS"]);

                            ato.Indisponibilidade = Convert.ToInt32(dr["VALOR_BUSCAS"]);

                            ato.Total = Convert.ToDecimal(dr["VALOR_TOTAL"]);

                            ato.TipoAto = "CERTIDÃO NOTAS";

                            ato.Natureza = "CERTIDÃO NOTAS";

                            ato.DescricaoAto = "I";

                            // TipoCobranca
                            switch (dr["COBRANCA"].ToString())
                            {
                                case "CC":
                                    ato.TipoCobranca = "COM COBRANÇA";
                                    break;
                                case "SC":
                                    ato.TipoCobranca = "SEM COBRANÇA";
                                    break;
                                case "JG":
                                    ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                    break;
                                default:
                                    ato.TipoCobranca = "NIHILL";
                                    break;
                            }


                            using (FbConnection conn2 = new FbConnection(Properties.Settings.Default.SettingReciboNotas))
                            {

                                string comando2 = string.Format("select * from ATOS where ID_TALAO = {0}", ato.Id_Ato);
                                conn2.Open();

                                using (FbCommand cmdTotal2 = new FbCommand(comando2, conn2))
                                {

                                    cmdTotal2.CommandType = CommandType.Text;

                                    FbDataReader dr2;

                                    dr2 = cmdTotal2.ExecuteReader();

                                    ato.Emolumentos = 0M;

                                    ato.Fetj = 0M;

                                    ato.Fundperj = 0M;

                                    ato.Funperj = 0M;

                                    ato.Funarpen = 0M;

                                    ato.Pmcmv = 0M;

                                    ato.Iss = 0M;

                                    ato.Mutua = 0M;

                                    ato.Acoterj = 0M;

                                    ato.Distribuicao = 0M;



                                    while (dr2.Read())
                                    {
                                        if (dr2["PROTOCOLO"].ToString() != "")
                                            ato.Protocolo = Convert.ToInt32(dr2["PROTOCOLO"]);


                                        ato.Emolumentos = ato.Emolumentos + Convert.ToDecimal(dr2["EMOLUMENTOS"]);

                                        ato.Fetj = ato.Fetj + Convert.ToDecimal(dr2["FETJ"]);

                                        ato.Fundperj = ato.Fundperj + Convert.ToDecimal(dr2["FUNDPERJ"]);

                                        ato.Funperj = ato.Funperj + Convert.ToDecimal(dr2["FUNPERJ"]);

                                        ato.Funarpen = ato.Funarpen + Convert.ToDecimal(dr2["FUNARPEN"]);

                                        ato.Pmcmv = ato.Pmcmv + Convert.ToDecimal(dr2["PMCMV"]);

                                        ato.Iss = ato.Iss + Convert.ToDecimal(dr2["ISS"]);

                                        ato.Mutua = ato.Mutua + Convert.ToDecimal(dr2["MUTUA"]);

                                        ato.Acoterj = ato.Acoterj + Convert.ToDecimal(dr2["ACOTERJ"]);

                                        ato.Distribuicao = ato.Distribuicao + Convert.ToDecimal(dr2["DISTRIBUICAO"]);

                                        ato.ItensCustasNotas = ObterCustasNotas(Convert.ToInt32(dr2["ID_ATO"]));

                                    }

                                }

                            }


                            atosNaoLancadosNotas.Add(ato);


                        }


                    }



                }
            }


            return atosNaoLancadosNotas;
        }




        public List<Ato> ObterReciboRgi(DateTime dataAto, Usuario usuarioLogado)
        {



            Ato ato;

            List<Ato> atosLancados = new List<Ato>();

            List<Ato> atosNaoLancadosRgi = new List<Ato>();

            atosLancados = classAto.ListarAtoDataAto(dataAto, dataAto, "RGI").ToList();

            atosNaoLancadosRgi = new List<Ato>();

            using (FbConnection conn = new FbConnection(Properties.Settings.Default.SettingReciboRgi))
            {

                string data = dataAto.ToShortDateString().Replace("/", ".");

                string comando = string.Format("select * from talao where dt_entrada = '{0}' and STATUS = 'EN'", data);
                conn.Open();

                using (FbCommand cmdTotal = new FbCommand(comando, conn))
                {
                    cmdTotal.CommandType = CommandType.Text;

                    FbDataReader dr;

                    dr = cmdTotal.ExecuteReader();



                    while (dr.Read())
                    {

                        if (atosLancados.Where(p => p.Recibo == Convert.ToInt32(dr["RECIBO"])).Count() == 0)
                        {

                            ato = new Ato();


                            ato.Recibo = Convert.ToInt32(dr["RECIBO"]);


                            ato.DataPagamento = Convert.ToDateTime(dr["DT_ENTRADA"]);

                            ato.DataAto = Convert.ToDateTime(dr["DT_ENTRADA"]);
                            
                            ato.Atribuicao = "RGI";

                            ato.Portador = dr["APRESENTANTE"].ToString();

                            ato.Pago = false;

                            ato.Natureza = dr["DESCRICAO_GERAL"].ToString();

                            ato.Id_Ato = Convert.ToInt32(dr["ID_TALAO"]);

                            ato.Faixa = dr["DESCRICAO_GERAL"].ToString();

                            ato.QtdAtos = Convert.ToInt32(dr["QTD_ATOS"]);

                            ato.QuantPrenotacao = Convert.ToInt32(dr["QTD_PRENOTACAO"]);

                            ato.Prenotacao = Convert.ToDecimal(dr["VALOR_PRENOTACAO"]);

                            ato.QuantIndisp = Convert.ToInt32(dr["QTD_BUSCAS"]);

                            ato.Indisponibilidade = Convert.ToDecimal(dr["VALOR_BUSCAS"]);

                            ato.Total = Convert.ToDecimal(dr["VALOR_TOTAL"]);

                            if (ato.QuantPrenotacao > 0)
                                ato.TipoPrenotacao = "COM PRENOTAÇÃO";
                            else
                                ato.TipoPrenotacao = "SEM PRENOTAÇÃO";

                            // TipoCobranca
                            switch (dr["COBRANCA"].ToString())
                            {
                                case "CC":
                                    ato.TipoCobranca = "COM COBRANÇA";
                                    break;
                                case "SC":
                                    ato.TipoCobranca = "SEM COBRANÇA";
                                    break;
                                case "JG":
                                    ato.TipoCobranca = "JUSTIÇA GRATUITA";
                                    break;
                                default:
                                    ato.TipoCobranca = "NIHILL";
                                    break;
                            }

                            ato.ValorAdicionar = 0;
                            ato.ValorDesconto = 0;
                            

                            ato.DescricaoAto = "I";


                            using (FbConnection conn2 = new FbConnection(Properties.Settings.Default.SettingReciboRgi))
                            {

                                string comando2 = string.Format("select * from ATOS where ID_TALAO = {0}", ato.Id_Ato);
                                conn2.Open();

                                using (FbCommand cmdTotal2 = new FbCommand(comando2, conn2))
                                {

                                    cmdTotal2.CommandType = CommandType.Text;

                                    FbDataReader dr2;

                                    dr2 = cmdTotal2.ExecuteReader();



                                    ato.Emolumentos = 0M;

                                    ato.Fetj = 0M;

                                    ato.Fundperj = 0M;

                                    ato.Funperj = 0M;

                                    ato.Funarpen = 0M;

                                    ato.Pmcmv = 0M;

                                    ato.Iss = 0M;

                                    ato.Mutua = 0M;

                                    ato.Acoterj = 0M;

                                    ato.Distribuicao = 0M;


                                    int cont = 0;

                                    while (dr2.Read())
                                    {
                                        if (dr2["PROTOCOLO"].ToString() != "")
                                            ato.Protocolo = Convert.ToInt32(dr2["PROTOCOLO"]);


                                        var itensAtoRgi = new ItensAtoRgi();

                                        itensAtoRgi.Cont = cont + 1;

                                        cont = Convert.ToInt32(itensAtoRgi.Cont);

                                        itensAtoRgi.Protocolo = ato.Protocolo;
                                        itensAtoRgi.Recibo = ato.Recibo;
                                        itensAtoRgi.TipoAto = "REGISTRO";
                                        itensAtoRgi.Natureza = ato.Natureza;

                                        itensAtoRgi.Emolumentos = Convert.ToDecimal(dr2["EMOLUMENTOS"]);

                                        itensAtoRgi.Fetj = Convert.ToDecimal(dr2["FETJ"]);

                                        itensAtoRgi.Fundperj =  Convert.ToDecimal(dr2["FUNDPERJ"]);

                                        itensAtoRgi.Funperj =  Convert.ToDecimal(dr2["FUNPERJ"]);

                                        itensAtoRgi.Funarpen =  Convert.ToDecimal(dr2["FUNARPEN"]);

                                        itensAtoRgi.Pmcmv = Convert.ToDecimal(dr2["PMCMV"]);

                                        itensAtoRgi.Iss = Convert.ToDecimal(dr2["ISS"]);

                                        itensAtoRgi.Mutua =  Convert.ToDecimal(dr2["MUTUA"]);

                                        itensAtoRgi.Acoterj =  Convert.ToDecimal(dr2["ACOTERJ"]);

                                        itensAtoRgi.Distribuicao = Convert.ToDecimal(dr2["DISTRIBUICAO"]);

                                        itensAtoRgi.Total = Convert.ToDecimal(dr2["TOTAL"]);

                                        itensAtoRgi.Id_Ato = Convert.ToInt32(dr2["ID_ATO"]); 

                                        ato.ItensAtoRgis.Add(itensAtoRgi);


                                        ato.Emolumentos = ato.Emolumentos + Convert.ToDecimal(dr2["EMOLUMENTOS"]);

                                        ato.Fetj = ato.Fetj + Convert.ToDecimal(dr2["FETJ"]);

                                        ato.Fundperj = ato.Fundperj + Convert.ToDecimal(dr2["FUNDPERJ"]);

                                        ato.Funperj = ato.Funperj + Convert.ToDecimal(dr2["FUNPERJ"]);

                                        ato.Funarpen = ato.Funarpen + Convert.ToDecimal(dr2["FUNARPEN"]);

                                        ato.Pmcmv = ato.Pmcmv + Convert.ToDecimal(dr2["PMCMV"]);

                                        ato.Iss = ato.Iss + Convert.ToDecimal(dr2["ISS"]);

                                        ato.Mutua = ato.Mutua + Convert.ToDecimal(dr2["MUTUA"]);

                                        ato.Acoterj = ato.Acoterj + Convert.ToDecimal(dr2["ACOTERJ"]);

                                        ato.Distribuicao = ato.Distribuicao + Convert.ToDecimal(dr2["DISTRIBUICAO"]);

                                              

                                    }

                                }

                            }


                            atosNaoLancadosRgi.Add(ato);


                        }


                    }



                }
            }


            return atosNaoLancadosRgi;
        }
       

    }

    


}
