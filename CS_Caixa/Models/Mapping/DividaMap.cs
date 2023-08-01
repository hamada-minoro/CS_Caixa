using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CS_Caixa.Models.Mapping
{
    public class DividaMap : EntityTypeConfiguration<Divida>
    {
        public DividaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id_Divida);

            // Properties
            this.Property(t => t.Descricao)
                .HasMaxLength(50);

            this.Property(t => t.Tipo)
                .HasMaxLength(20);

            this.Property(t => t.Tipo_Divida)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Divida");
            this.Property(t => t.Id_Divida).HasColumnName("Id_Divida");
            this.Property(t => t.Id_Usuario).HasColumnName("Id_Usuario");
            this.Property(t => t.Data).HasColumnName("Data");
            this.Property(t => t.Dia_Pagamento).HasColumnName("Dia_Pagamento");
            this.Property(t => t.Data_Inicio_Parcela).HasColumnName("Data_Inicio_Parcela");
            this.Property(t => t.Data_Fim_Parcela).HasColumnName("Data_Fim_Parcela");
            this.Property(t => t.Descricao).HasColumnName("Descricao");
            this.Property(t => t.Tipo).HasColumnName("Tipo");
            this.Property(t => t.Tipo_Divida).HasColumnName("Tipo_Divida");
            this.Property(t => t.Qtd_Parcelas).HasColumnName("Qtd_Parcelas");
            this.Property(t => t.Divida_Paga).HasColumnName("Divida_Paga");
            this.Property(t => t.Qtd_Parcelas_Pagas).HasColumnName("Qtd_Parcelas_Pagas");
            this.Property(t => t.Valor_Pago).HasColumnName("Valor_Pago");
            this.Property(t => t.Valor_Divida).HasColumnName("Valor_Divida");
        }
    }
}
