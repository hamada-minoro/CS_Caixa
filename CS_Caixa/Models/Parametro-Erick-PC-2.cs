using System;
using System.Collections.Generic;

namespace CS_Caixa.Models
{
    public partial class Parametro
    {
        public int Parametro_Id { get; set; }
        public string CodigoInstalacao { get; set; }
        public System.DateTime DataInstalacao { get; set; }
        public bool Bip_Aviso { get; set; }
        public bool Falar_Senha { get; set; }
        public bool Voz_RetiradaSenha { get; set; }
        public bool Passar_Mensagem { get; set; }
        public bool Mostrar_Hora { get; set; }
        public bool Utilizar_Aleatorio { get; set; }
        public string Saudacao { get; set; }
        public int Qtd_Caracteres_Senha { get; set; }
        public int Tipo_Senha { get; set; }
        public bool ZerarSenhaDiaSeguinte { get; set; }
        public bool ModoRetiradaSenhaManual { get; set; }
        public string Letra_Botao_1 { get; set; }
        public string Letra_Botao_2 { get; set; }
        public string Letra_Botao_3 { get; set; }
        public string Letra_Setor_1 { get; set; }
        public string Letra_Setor_2 { get; set; }
        public string Letra_Setor_3 { get; set; }
        public string Letra_Setor_4 { get; set; }
        public string Nome_Botao_1 { get; set; }
        public string Nome_Botao_2 { get; set; }
        public string Nome_Botao_3 { get; set; }
        public string Nome_Setor_1 { get; set; }
        public string Nome_Setor_2 { get; set; }
        public string Nome_Setor_3 { get; set; }
        public string Nome_Setor_4 { get; set; }
        public bool Habilitado_Botao_1 { get; set; }
        public bool Habilitado_Botao_2 { get; set; }
        public bool Habilitado_Botao_3 { get; set; }
        public bool Habilitado_Setor_1 { get; set; }
        public bool Habilitado_Setor_2 { get; set; }
        public bool Habilitado_Setor_3 { get; set; }
        public bool Habilitado_Setor_4 { get; set; }
        public Nullable<bool> Voz_Padrao { get; set; }
        public string Voz_Botao_1 { get; set; }
        public string Voz_Botao_2 { get; set; }
        public string Voz_Botao_3 { get; set; }
        public string Voz_Setor_1 { get; set; }
        public string Voz_Setor_2 { get; set; }
        public string Voz_Setor_3 { get; set; }
        public string Voz_Setor_4 { get; set; }
        public string Nome_Empresa { get; set; }
        public bool InicioFimExpediente { get; set; }
        public bool Domingo { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }
        public string HoraInicioExpediente { get; set; }
        public string HoraFimExpediente { get; set; }
        public bool DesligarPainel { get; set; }
        public string HoraDesligarPainel { get; set; }
        public bool DesligarSenha { get; set; }
        public string HoraDesligarSenha { get; set; }
        public bool DesligarEstacao { get; set; }
        public string HoraDesligarEstacao { get; set; }
        public string MensagemInicioExpediente { get; set; }
        public string MensagemFimExpediente { get; set; }
        public bool CadastroCliente { get; set; }
        public Nullable<decimal> ValorSaldoControleInterno { get; set; }
    }
}
