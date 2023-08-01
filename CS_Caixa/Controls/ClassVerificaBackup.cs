using CS_Caixa.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Controls
{
    public class ClassVerificaBackup
    {
        CS_CAIXA_DBContext contexto { get; set; }

        public ClassVerificaBackup()
        {
            contexto = new CS_CAIXA_DBContext();
        }

        public VerificaBackup AdicionarVerificaBackup(VerificaBackup novoRegistro)
        {

            contexto = new CS_CAIXA_DBContext();

            VerificaBackup novo = new VerificaBackup();

            novo.DataVerificacao = novoRegistro.DataVerificacao;
            novo.HoraVerificacao = novoRegistro.HoraVerificacao;
            novo.MaquinaVerificou = novoRegistro.MaquinaVerificou;
            novo.Status = novoRegistro.Status;

            contexto.VerificaBackups.Add(novo);
            contexto.SaveChanges();

            return novo;
        }


        public void AlterarVerificaBackup(int alterarcodigo, string status)
        {

            contexto = new CS_CAIXA_DBContext();

            VerificaBackup alterar = contexto.VerificaBackups.Where(p => p.VerificaBackupId == alterarcodigo).FirstOrDefault();



            alterar.Status = status;

            contexto.SaveChanges();
        }


        public VerificaBackup ObterUltimaVerificacao()
        {
            contexto = new CS_CAIXA_DBContext();

            List<VerificaBackup> list = new List<VerificaBackup>();

            list = contexto.VerificaBackups.OrderBy(p => p.VerificaBackupId).ToList();
            
            return list.LastOrDefault();
        }
    }
}
