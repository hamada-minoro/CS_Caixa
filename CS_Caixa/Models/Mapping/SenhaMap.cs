using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class SenhaMap : EntityTypeConfiguration<Senha>
    {
        public SenhaMap()
        {
            // Primary Key
            this.HasKey(t => t.Senha_Id);

            // Properties
            this.Property(t => t.Tipo)
                .HasMaxLength(1);

            this.Property(t => t.Status)
                .HasMaxLength(30);

            this.Property(t => t.Identificador_Pc)
                .HasMaxLength(30);

            this.Property(t => t.DescricaoLocalAtendimento)
                .HasMaxLength(11);

            this.Property(t => t.Caracter_Atendimento)
                .HasMaxLength(3);

            this.Property(t => t.Aleatorio_Confirmacao)
                .HasMaxLength(3);

            this.Property(t => t.Hora_Retirada)
                .HasMaxLength(8);

            this.Property(t => t.Hora_Chamada)
                .HasMaxLength(8);

            this.Property(t => t.Hora_Atendimento)
                .HasMaxLength(8);

            this.Property(t => t.Hora_Finalizado)
                .HasMaxLength(8);

            this.Property(t => t.Hora_Cancelado)
                .HasMaxLength(8);

            this.Property(t => t.Nome_Usuario)
                .HasMaxLength(100);

            this.Property(t => t.FalaOutros)
                .HasMaxLength(50);

            this.Property(t => t.LetraSetor)
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.NomeSetor)
                .HasMaxLength(50);

            this.Property(t => t.Voz)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Senha");
            this.Property(t => t.Senha_Id).HasColumnName("Senha_Id");
            this.Property(t => t.Sequencia_Chamada).HasColumnName("Sequencia_Chamada");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Numero_Senha).HasColumnName("Numero_Senha");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Identificador_Pc).HasColumnName("Identificador_Pc");
            this.Property(t => t.DescricaoLocalAtendimento).HasColumnName("DescricaoLocalAtendimento");
            this.Property(t => t.Caracter_Atendimento).HasColumnName("Caracter_Atendimento");
            this.Property(t => t.Aleatorio_Confirmacao).HasColumnName("Aleatorio_Confirmacao");
            this.Property(t => t.Hora_Retirada).HasColumnName("Hora_Retirada");
            this.Property(t => t.Hora_Chamada).HasColumnName("Hora_Chamada");
            this.Property(t => t.Hora_Atendimento).HasColumnName("Hora_Atendimento");
            this.Property(t => t.Hora_Finalizado).HasColumnName("Hora_Finalizado");
            this.Property(t => t.Hora_Cancelado).HasColumnName("Hora_Cancelado");
            this.Property(t => t.Usuario_Id).HasColumnName("Usuario_Id");
            this.Property(t => t.Nome_Usuario).HasColumnName("Nome_Usuario");
            this.Property(t => t.SenhaTipo).HasColumnName("SenhaTipo");
            this.Property(t => t.FalaOutros).HasColumnName("FalaOutros");
            this.Property(t => t.SetorId).HasColumnName("SetorId");
            this.Property(t => t.LetraSetor).HasColumnName("LetraSetor");
            this.Property(t => t.NomeSetor).HasColumnName("NomeSetor");
            this.Property(t => t.Voz).HasColumnName("Voz");
            this.Property(t => t.ModoSequencial).HasColumnName("ModoSequencial");
            this.Property(t => t.NumeroSequencia).HasColumnName("NumeroSequencia");
            this.Property(t => t.QtdCaracteres).HasColumnName("QtdCaracteres");
            this.Property(t => t.CadastroCliente_Id).HasColumnName("CadastroCliente_Id");
        }
    }
}
