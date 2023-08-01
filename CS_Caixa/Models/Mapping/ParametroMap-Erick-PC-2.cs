using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class ParametroMap : EntityTypeConfiguration<Parametro>
    {
        public ParametroMap()
        {
            // Primary Key
            this.HasKey(t => t.Parametro_Id);

            // Properties
            this.Property(t => t.CodigoInstalacao)
                .HasMaxLength(100);

            this.Property(t => t.Saudacao)
                .HasMaxLength(32);

            this.Property(t => t.Letra_Botao_1)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Botao_2)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Botao_3)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Setor_1)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Setor_2)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Setor_3)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Letra_Setor_4)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.Nome_Botao_1)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Botao_2)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Botao_3)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Setor_1)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Setor_2)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Setor_3)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Setor_4)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Botao_1)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Botao_2)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Botao_3)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Setor_1)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Setor_2)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Setor_3)
                .HasMaxLength(50);

            this.Property(t => t.Voz_Setor_4)
                .HasMaxLength(50);

            this.Property(t => t.Nome_Empresa)
                .HasMaxLength(32);

            this.Property(t => t.HoraInicioExpediente)
                .HasMaxLength(8);

            this.Property(t => t.HoraFimExpediente)
                .HasMaxLength(8);

            this.Property(t => t.HoraDesligarPainel)
                .HasMaxLength(8);

            this.Property(t => t.HoraDesligarSenha)
                .HasMaxLength(8);

            this.Property(t => t.HoraDesligarEstacao)
                .HasMaxLength(8);

            this.Property(t => t.MensagemInicioExpediente)
                .HasMaxLength(50);

            this.Property(t => t.MensagemFimExpediente)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Parametros");
            this.Property(t => t.Parametro_Id).HasColumnName("Parametro_Id");
            this.Property(t => t.CodigoInstalacao).HasColumnName("CodigoInstalacao");
            this.Property(t => t.DataInstalacao).HasColumnName("DataInstalacao");
            this.Property(t => t.Bip_Aviso).HasColumnName("Bip_Aviso");
            this.Property(t => t.Falar_Senha).HasColumnName("Falar_Senha");
            this.Property(t => t.Voz_RetiradaSenha).HasColumnName("Voz_RetiradaSenha");
            this.Property(t => t.Passar_Mensagem).HasColumnName("Passar_Mensagem");
            this.Property(t => t.Mostrar_Hora).HasColumnName("Mostrar_Hora");
            this.Property(t => t.Utilizar_Aleatorio).HasColumnName("Utilizar_Aleatorio");
            this.Property(t => t.Saudacao).HasColumnName("Saudacao");
            this.Property(t => t.Qtd_Caracteres_Senha).HasColumnName("Qtd_Caracteres_Senha");
            this.Property(t => t.Tipo_Senha).HasColumnName("Tipo_Senha");
            this.Property(t => t.ZerarSenhaDiaSeguinte).HasColumnName("ZerarSenhaDiaSeguinte");
            this.Property(t => t.ModoRetiradaSenhaManual).HasColumnName("ModoRetiradaSenhaManual");
            this.Property(t => t.Letra_Botao_1).HasColumnName("Letra_Botao_1");
            this.Property(t => t.Letra_Botao_2).HasColumnName("Letra_Botao_2");
            this.Property(t => t.Letra_Botao_3).HasColumnName("Letra_Botao_3");
            this.Property(t => t.Letra_Setor_1).HasColumnName("Letra_Setor_1");
            this.Property(t => t.Letra_Setor_2).HasColumnName("Letra_Setor_2");
            this.Property(t => t.Letra_Setor_3).HasColumnName("Letra_Setor_3");
            this.Property(t => t.Letra_Setor_4).HasColumnName("Letra_Setor_4");
            this.Property(t => t.Nome_Botao_1).HasColumnName("Nome_Botao_1");
            this.Property(t => t.Nome_Botao_2).HasColumnName("Nome_Botao_2");
            this.Property(t => t.Nome_Botao_3).HasColumnName("Nome_Botao_3");
            this.Property(t => t.Nome_Setor_1).HasColumnName("Nome_Setor_1");
            this.Property(t => t.Nome_Setor_2).HasColumnName("Nome_Setor_2");
            this.Property(t => t.Nome_Setor_3).HasColumnName("Nome_Setor_3");
            this.Property(t => t.Nome_Setor_4).HasColumnName("Nome_Setor_4");
            this.Property(t => t.Habilitado_Botao_1).HasColumnName("Habilitado_Botao_1");
            this.Property(t => t.Habilitado_Botao_2).HasColumnName("Habilitado_Botao_2");
            this.Property(t => t.Habilitado_Botao_3).HasColumnName("Habilitado_Botao_3");
            this.Property(t => t.Habilitado_Setor_1).HasColumnName("Habilitado_Setor_1");
            this.Property(t => t.Habilitado_Setor_2).HasColumnName("Habilitado_Setor_2");
            this.Property(t => t.Habilitado_Setor_3).HasColumnName("Habilitado_Setor_3");
            this.Property(t => t.Habilitado_Setor_4).HasColumnName("Habilitado_Setor_4");
            this.Property(t => t.Voz_Padrao).HasColumnName("Voz_Padrao");
            this.Property(t => t.Voz_Botao_1).HasColumnName("Voz_Botao_1");
            this.Property(t => t.Voz_Botao_2).HasColumnName("Voz_Botao_2");
            this.Property(t => t.Voz_Botao_3).HasColumnName("Voz_Botao_3");
            this.Property(t => t.Voz_Setor_1).HasColumnName("Voz_Setor_1");
            this.Property(t => t.Voz_Setor_2).HasColumnName("Voz_Setor_2");
            this.Property(t => t.Voz_Setor_3).HasColumnName("Voz_Setor_3");
            this.Property(t => t.Voz_Setor_4).HasColumnName("Voz_Setor_4");
            this.Property(t => t.Nome_Empresa).HasColumnName("Nome_Empresa");
            this.Property(t => t.InicioFimExpediente).HasColumnName("InicioFimExpediente");
            this.Property(t => t.Domingo).HasColumnName("Domingo");
            this.Property(t => t.Segunda).HasColumnName("Segunda");
            this.Property(t => t.Terca).HasColumnName("Terca");
            this.Property(t => t.Quarta).HasColumnName("Quarta");
            this.Property(t => t.Quinta).HasColumnName("Quinta");
            this.Property(t => t.Sexta).HasColumnName("Sexta");
            this.Property(t => t.Sabado).HasColumnName("Sabado");
            this.Property(t => t.HoraInicioExpediente).HasColumnName("HoraInicioExpediente");
            this.Property(t => t.HoraFimExpediente).HasColumnName("HoraFimExpediente");
            this.Property(t => t.DesligarPainel).HasColumnName("DesligarPainel");
            this.Property(t => t.HoraDesligarPainel).HasColumnName("HoraDesligarPainel");
            this.Property(t => t.DesligarSenha).HasColumnName("DesligarSenha");
            this.Property(t => t.HoraDesligarSenha).HasColumnName("HoraDesligarSenha");
            this.Property(t => t.DesligarEstacao).HasColumnName("DesligarEstacao");
            this.Property(t => t.HoraDesligarEstacao).HasColumnName("HoraDesligarEstacao");
            this.Property(t => t.MensagemInicioExpediente).HasColumnName("MensagemInicioExpediente");
            this.Property(t => t.MensagemFimExpediente).HasColumnName("MensagemFimExpediente");
            this.Property(t => t.CadastroCliente).HasColumnName("CadastroCliente");
            this.Property(t => t.ValorSaldoControleInterno).HasColumnName("ValorSaldoControleInterno");
        }
    }
}
