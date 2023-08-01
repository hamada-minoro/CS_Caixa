using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class VerificaBackup
    {
        public int VerificaBackupId { get; set; }
        public Nullable<System.DateTime> DataVerificacao { get; set; }
        public string HoraVerificacao { get; set; }
        public string Status { get; set; }
        public string MaquinaVerificou { get; set; }
    }
}
