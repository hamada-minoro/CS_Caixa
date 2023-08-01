using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassAtualizaSite
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassAtualizaSite()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public bool VerificaAtualizar(DateTime data)
        {

            var atualiza = contexto.AtualizaSites.FirstOrDefault();

            if(data.ToShortDateString() == atualiza.DataAtualizacao)
            {
                if (atualiza.Status == "ATUALIZANDO")
                {
                    var diferenca = DateTime.Now.Hour - Convert.ToInt16(atualiza.HoraAtualizacao.Substring(0,2));
                    
                    if (diferenca > 0)
                    {
                        return true;
                    }
                }               
            }
            else
            {
                return true;
            }

            return false;
        }


        public void SalvarAtualizar(AtualizaSite atualizaSite)
        {
            var atualiza = contexto.AtualizaSites.FirstOrDefault();

            atualiza.DataAtualizacao = atualizaSite.DataAtualizacao;
            atualiza.HoraAtualizacao = atualizaSite.HoraAtualizacao;
            atualiza.PcAtualizacao = atualizaSite.PcAtualizacao;
            atualiza.Status = atualizaSite.Status;

            contexto.SaveChanges();
        }

        public AtualizaSite ObterAtualizaSite()
        {
            return contexto.AtualizaSites.FirstOrDefault();
        }

    }
}
