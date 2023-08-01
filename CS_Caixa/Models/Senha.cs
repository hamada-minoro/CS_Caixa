using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Senha
    {
        public int Senha_Id { get; set; }
        public int Sequencia_Chamada { get; set; }
        public string Tipo { get; set; }
        public System.DateTime Data { get; set; }
        public int Numero_Senha { get; set; }
        public string Status { get; set; }
        public string Identificador_Pc { get; set; }
        public string DescricaoLocalAtendimento { get; set; }
        public string Caracter_Atendimento { get; set; }
        public string Aleatorio_Confirmacao { get; set; }
        public string Hora_Retirada { get; set; }
        public string Hora_Chamada { get; set; }
        public string Hora_Atendimento { get; set; }
        public string Hora_Finalizado { get; set; }
        public string Hora_Cancelado { get; set; }
        public Nullable<int> Usuario_Id { get; set; }
        public string Nome_Usuario { get; set; }
        public int SenhaTipo { get; set; }
        public string FalaOutros { get; set; }
        public int SetorId { get; set; }
        public string LetraSetor { get; set; }
        public string NomeSetor { get; set; }
        public string Voz { get; set; }
        public bool ModoSequencial { get; set; }
        public int NumeroSequencia { get; set; }
        public int QtdCaracteres { get; set; }
        public int CadastroCliente_Id { get; set; }
    }
}
