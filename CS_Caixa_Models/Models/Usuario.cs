using System;
using System.Collections.Generic;

namespace CS_Caixa_Models.Models
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
        public Nullable<bool> Indice { get; set; }
        public Nullable<bool> Caixa { get; set; }
        public Nullable<bool> Balcao { get; set; }
    }
}
