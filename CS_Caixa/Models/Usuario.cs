using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Usuario
    {
        public int Id_Usuario { get; set; }
        public string NomeUsu { get; set; }
        public string Senha { get; set; }
        public Nullable<bool> Notas { get; set; }
        public Nullable<bool> Rgi { get; set; }
        public Nullable<bool> Protesto { get; set; }
        public Nullable<bool> Master { get; set; }
        public Nullable<bool> Caixa { get; set; }
        public Nullable<bool> Balcao { get; set; }
        public Nullable<bool> ExcluirAtos { get; set; }
        public Nullable<bool> AlterarAtos { get; set; }
        public Nullable<bool> ImprimirMatricula { get; set; }
        public string Qualificacao { get; set; }
        public Nullable<bool> Alterar_Status_Senha { get; set; }
        public Nullable<bool> Cadastrar_Painel { get; set; }
        public Nullable<bool> Cadastrar_Pc { get; set; }
        public Nullable<bool> Cadastrar_Usuario { get; set; }
        public Nullable<bool> Configurar_Botoes { get; set; }
        public Nullable<bool> Configurar_Senha { get; set; }
        public Nullable<bool> Configurar_Mensagem { get; set; }
        public Nullable<bool> Chamar_Senha_Cancelada { get; set; }
        public Nullable<bool> Chamar_Senha_Fora_Sequencia { get; set; }
        public Nullable<bool> Adm { get; set; }
    }
}
