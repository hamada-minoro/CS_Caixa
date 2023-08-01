using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CS_Caixa.Models;

namespace CS_Caixa.Controls
{
    public class ClassCustasRgi
    {
        public CS_CAIXA_DBContext Contexto { get; set; }
       
        public ClassCustasRgi()
        {
            Contexto = new CS_CAIXA_DBContext();
        }

        public List<CustasRgi> ListaCustas()
        {
            try
            {
                return Contexto.CustasRgis.AsNoTracking().Select(p => p).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }

        public List<CustasDistribuicao> ListaCustasDistribuicao()
        {
            try
            {
                return Contexto.CustasDistribuicaos.Select(p => p).ToList();
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }

        }

        public void SalvarItensLista(ItensCustasRgi item)
        {
            ItensCustasRgi novoItem = new ItensCustasRgi();

            Contexto = new CS_CAIXA_DBContext();

            novoItem.Id_AtoRgi = item.Id_AtoRgi;

            novoItem.Id_Ato = item.Id_Ato;

            novoItem.Cont = item.Cont;

            novoItem.Tabela = item.Tabela;

            novoItem.Item = item.Item;

            novoItem.SubItem = item.SubItem;

            novoItem.Quantidade = item.Quantidade;

            novoItem.Valor = item.Valor;

            novoItem.Total = item.Total;

            novoItem.Descricao = item.Descricao;

            Contexto.ItensCustasRgis.Add(novoItem);

            Contexto.SaveChanges();
        }


        public List<ItensAtoRgi> ListarAtoRgi(int idAto)
        {
            return Contexto.ItensAtoRgis.Where(p => p.Id_Ato == idAto).ToList();
        }


        public int SalvarItensListaAto(ItensAtoRgi itensSalvar)
        {
            ItensAtoRgi itemSalvar = new ItensAtoRgi();
            itemSalvar.Id_Ato = itensSalvar.Id_Ato;
            itemSalvar.Cont = itensSalvar.Cont;
            itemSalvar.Protocolo = itensSalvar.Protocolo;
            itemSalvar.Recibo = itensSalvar.Recibo;
            itemSalvar.TipoAto = itensSalvar.TipoAto;
            itemSalvar.Natureza = itensSalvar.Natureza;
            itemSalvar.Emolumentos = itensSalvar.Emolumentos;
            itemSalvar.Fetj = itensSalvar.Fetj;
            itemSalvar.Fundperj = itensSalvar.Fundperj;
            itemSalvar.Funperj = itensSalvar.Funperj;
            itemSalvar.Funarpen = itensSalvar.Funarpen;
            itemSalvar.Pmcmv = itensSalvar.Pmcmv;
            itemSalvar.Iss = itensSalvar.Iss;
            itemSalvar.Mutua = itensSalvar.Mutua;
            itemSalvar.Acoterj = itensSalvar.Acoterj;
            itemSalvar.Distribuicao = itensSalvar.Distribuicao;
            itemSalvar.QuantDistrib = itensSalvar.QuantDistrib;
            itemSalvar.Total = itensSalvar.Total;

            Contexto.ItensAtoRgis.Add(itemSalvar);

            Contexto.SaveChanges();

            return itemSalvar.Id_AtoRgi;
        }


        public List<ItensCustasRgi> ListarItensCustas(int id)
        {
            return Contexto.ItensCustasRgis.Where(p => p.Id_AtoRgi == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public List<ItensCustasRgi> ListarItensCustasIdAto(int id)
        {
            return Contexto.ItensCustasRgis.Where(p => p.Id_Ato == id).OrderBy(p => p.Id_Custa).ToList();
        }

        public void RemoverItensCustas(int id)
        {
            List<ItensCustasRgi> itens = new List<ItensCustasRgi>();

            var ItensExcluir = Contexto.ItensCustasRgis.Where(p => p.Id_Ato == id).ToList();

            itens = ItensExcluir.ToList();

            for (int cont = 0; cont <= itens.Count - 1; cont++)
            {
                Contexto.ItensCustasRgis.Remove(itens[cont]);
                Contexto.SaveChanges();
            }
        }

        public void RemoverItensAto(int idAtoRgi)
        {
            ItensAtoRgi itemAtoRgi = new ItensAtoRgi();
            itemAtoRgi = Contexto.ItensAtoRgis.Where(p => p.Id_AtoRgi == idAtoRgi).FirstOrDefault();

            Contexto.ItensAtoRgis.Remove(itemAtoRgi);
            Contexto.SaveChanges();

        }

        public decimal CalcularISSRgi(decimal emol, bool formula, decimal alicotaIss)
        {

            decimal iss = 0M;

            if (formula == true)
            {
                iss = emol / ((100M - alicotaIss) / 100M);
                iss = iss - emol;
            }
            else
            {
                iss = (emol * alicotaIss) / 100M;
            }

            return iss;
        }
    }
}
