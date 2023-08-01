using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class Controle_UsoMap : EntityTypeConfiguration<Controle_Uso>
    {
        public Controle_UsoMap()
        {
            // Primary Key
            this.HasKey(t => t.ControleId);

            // Properties
            this.Property(t => t.Versao)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Demo)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.CodigoAtivacao)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DataAtivacao)
                .HasMaxLength(100);

            this.Property(t => t.DataValidadeInicio)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DataValidadeFim)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.DataUltimaUtilizacao)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.AtivacaoUso)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Controle_Uso");
            this.Property(t => t.ControleId).HasColumnName("ControleId");
            this.Property(t => t.Versao).HasColumnName("Versao");
            this.Property(t => t.Demo).HasColumnName("Demo");
            this.Property(t => t.CodigoAtivacao).HasColumnName("CodigoAtivacao");
            this.Property(t => t.DataAtivacao).HasColumnName("DataAtivacao");
            this.Property(t => t.DataValidadeInicio).HasColumnName("DataValidadeInicio");
            this.Property(t => t.DataValidadeFim).HasColumnName("DataValidadeFim");
            this.Property(t => t.DataUltimaUtilizacao).HasColumnName("DataUltimaUtilizacao");
            this.Property(t => t.DiasUtilizado).HasColumnName("DiasUtilizado");
            this.Property(t => t.AtivacaoUso).HasColumnName("AtivacaoUso");
        }
    }
}
