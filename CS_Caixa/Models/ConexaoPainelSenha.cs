using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class ConexaoPainelSenha
    {
        public int ConexaoId { get; set; }
        public string IpServidorAtendimento { get; set; }
        public int PortaConexao { get; set; }
        public string IpServidorAtendimentoNotas { get; set; }
        public Nullable<int> PortaConexaoNotas { get; set; }
    }
}
