using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Email
    {
        public int EmailId { get; set; }
        public System.DateTime Data { get; set; }
        public string Indicador { get; set; }
        public string Documento { get; set; }
        public bool Enviado { get; set; }
        public string Mensagem { get; set; }
    }
}
