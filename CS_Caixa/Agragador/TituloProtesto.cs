using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS_Caixa.Agragador
{
    public class TituloProtesto
    {
        public int ID_ATO { get; set; }
        public int CODIGO { get; set; }
        public int RECIBO { get; set; }
        public DateTime DT_ENTRADA { get; set; }
        public DateTime DT_PROTOCOLO { get; set; }
        public int PROTOCOLO { get; set; }
        public int LIVRO_PROTOCOLO { get; set; }
        public string FOLHA_PROTOCOLO { get; set; }
        public DateTime DT_PRAZO { get; set; }
        public DateTime DT_REGISTRO { get; set; }
        public int REGISTRO { get; set; }
        public int LIVRO_REGISTRO { get; set; }
        public string FOLHA_REGISTRO { get; set; }
        public string SELO_REGISTRO { get; set; }
        public DateTime DT_PAGAMENTO { get; set; }
        public string SELO_PAGAMENTO { get; set; }
        public int RECIBO_PAGAMENTO { get; set; }
        public string COBRANCA { get; set; }
        public decimal EMOLUMENTOS { get; set; }
        public decimal FETJ { get; set; }
        public decimal FUNDPERJ { get; set; }
        public decimal FUNPERJ { get; set; }
        public decimal FUNARPEN { get; set; }
        public decimal PMCMV { get; set; }
        public decimal ISS { get; set; }
        public decimal MUTUA { get; set; }
        public decimal DISTRIBUICAO { get; set; }
        public decimal ACOTERJ { get; set; }
        public decimal TOTAL { get; set; }
        public int TIPO_PROTESTO { get; set; }
        public int TIPO_TITULO { get; set; }
        public string Especie_Titulo { get; set; }
        public string NUMERO_TITULO { get; set; }
        public DateTime DT_TITULO { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTA { get; set; }
        public decimal VALOR_TITULO { get; set; }
        public decimal SALDO_PROTESTO { get; set; }
        public DateTime DT_VENCIMENTO { get; set; }
        public string CONVENIO { get; set; }
        public string TIPO_APRESENTACAO { get; set; }
        public DateTime DT_ENVIO { get; set; }
        public string CODIGO_APRESENTANTE { get; set; }
        public string TIPO_INTIMACAO { get; set; }
        public string MOTIVO_INTIMACAO { get; set; }
        public DateTime DT_INTIMACAO { get; set; }
        public DateTime DT_PUBLICACAO { get; set; }
        public decimal VALOR_PAGAMENTO { get; set; }
        public string APRESENTANTE { get; set; }
        public string CPF_CNPJ_APRESENTANTE { get; set; }
        public string TIPO_APRESENTANTE { get; set; }
        public string CEDENTE { get; set; }
        public string NOSSO_NUMERO { get; set; }
        public string SACADOR { get; set; }
        public string DEVEDOR { get; set; }
        public string CPF_CNPJ_DEVEDOR { get; set; }
        public string TIPO_DEVEDOR { get; set; }
        public string AGENCIA_CEDENTE { get; set; }
        public string PRACA_PROTESTO { get; set; }
        public string TIPO_ENDOSSO { get; set; }
        public string ACEITE { get; set; }
        public string CPF_ESCREVENTE { get; set; }
        public string CPF_ESCREVENTE_PG { get; set; }
        public string OBSERVACAO { get; set; }
        public DateTime DT_SUSTADO { get; set; }
        public DateTime DT_RETIRADO { get; set; }
        public string STATUS { get; set; }
        public string PROTESTADO { get; set; }
        public string ENVIADO_APONTAMENTO { get; set; }
        public string ENVIADO_PROTESTO { get; set; }
        public string ENVIADO_PAGAMENTO { get; set; }
        public string ENVIADO_RETIRADO { get; set; }
        public string ENVIADO_SUSTADO { get; set; }
        public string ENVIADO_DEVOLVIDO { get; set; }
        public string EXPORTADO { get; set; }
        public string AVALISTA_DEVEDOR { get; set; }
        public string FINS_FALIMENTARES { get; set; }
        public decimal TARIFA_BANCARIA { get; set; }
        public string FORMA_PAGAMENTO { get; set; }
        public string NUMERO_PAGAMENTO { get; set; }
        public string ARQUIVO { get; set; }
        public string RETORNO { get; set; }
        public DateTime DT_DEVOLVIDO { get; set; }
        public string CPF_CNPJ_SACADOR { get; set; }
        public int ID_MSG { get; set; }
        public decimal VALOR_AR { get; set; }
        public string ELETRONICO { get; set; }
        public int IRREGULARIDADE { get; set; }
        public string AVISTA { get; set; }
        public decimal SALDO_TITULO { get; set; }
        public string TIPO_SUSTACAO { get; set; }
        public string RETORNO_PROTESTO { get; set; }
        public DateTime DT_RETORNO_PROTESTO { get; set; }
        public DateTime DT_DEFINITIVA { get; set; }
        public string JUDICIAL { get; set; }
        public string ALEATORIO_PROTESTO { get; set; }
        public string ALEATORIO_SOLUCAO { get; set; }
        public string CCT { get; set; }
        public string DETERMINACAO { get; set; }
        public string ANTIGO { get; set; }
        public string LETRA { get; set; }
        public int PROTOCOLO_DISTRIBUIDOR { get; set; }
        public string EMAIL_ADVOGADO { get; set; }
        public string TELEFONE_ADVOGADO { get; set; }
        public string INTIMADO_ARQUIVO { get; set; }
        public string BAIXADO_ARQUIVO { get; set; }
        public string RETORNO_CRA { get; set; }
        public DateTime DT_PROT_CRA { get; set; }
        public bool Irregular { get; set; }


    }

}
