using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS_Caixa.Objetos_de_Valor
{
    public class UsuariosSite
    {

        public string Id { get; set; }

        public string Nome { get; set; }
        public string Documento { get; set; }
        public string TipoPessoa { get; set; }
        public bool ReceberNotificacao { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataModificado { get; set; }

        public DateTime DataUltimaAtualizacao { get; set; }

        public bool ConfirmacaoPresencial { get; set; }

        public int AtosFirmas { get; set; }

        public int AtosNotas { get; set; }

        public int AtosProtesto { get; set; }

        public int AtosRgi { get; set; }

        public int IntimacaoProtesto { get; set; }

        public string Email { get; set; }

        public bool EnviarEmail { get; set; }
    }
}
