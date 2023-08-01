using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Usuario);

            // Properties
            this.Property(t => t.Id_Usuario)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NomeUsu)
                .HasMaxLength(50);

            this.Property(t => t.Senha)
                .HasMaxLength(50);

            this.Property(t => t.Qualificacao)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Usuarios");
            this.Property(t => t.Id_Usuario).HasColumnName("Id_Usuario");
            this.Property(t => t.NomeUsu).HasColumnName("NomeUsu");
            this.Property(t => t.Senha).HasColumnName("Senha");
            this.Property(t => t.Notas).HasColumnName("Notas");
            this.Property(t => t.Rgi).HasColumnName("Rgi");
            this.Property(t => t.Protesto).HasColumnName("Protesto");
            this.Property(t => t.Master).HasColumnName("Master");
            this.Property(t => t.Caixa).HasColumnName("Caixa");
            this.Property(t => t.Balcao).HasColumnName("Balcao");
            this.Property(t => t.ExcluirAtos).HasColumnName("ExcluirAtos");
            this.Property(t => t.AlterarAtos).HasColumnName("AlterarAtos");
            this.Property(t => t.ImprimirMatricula).HasColumnName("ImprimirMatricula");
            this.Property(t => t.Qualificacao).HasColumnName("Qualificacao");
            this.Property(t => t.Alterar_Status_Senha).HasColumnName("Alterar_Status_Senha");
            this.Property(t => t.Cadastrar_Painel).HasColumnName("Cadastrar_Painel");
            this.Property(t => t.Cadastrar_Pc).HasColumnName("Cadastrar_Pc");
            this.Property(t => t.Cadastrar_Usuario).HasColumnName("Cadastrar_Usuario");
            this.Property(t => t.Configurar_Botoes).HasColumnName("Configurar_Botoes");
            this.Property(t => t.Configurar_Senha).HasColumnName("Configurar_Senha");
            this.Property(t => t.Configurar_Mensagem).HasColumnName("Configurar_Mensagem");
            this.Property(t => t.Chamar_Senha_Cancelada).HasColumnName("Chamar_Senha_Cancelada");
            this.Property(t => t.Chamar_Senha_Fora_Sequencia).HasColumnName("Chamar_Senha_Fora_Sequencia");
            this.Property(t => t.Adm).HasColumnName("Adm");
        }
    }
}
