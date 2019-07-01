using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.Design;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NoStop.MODEL
{
    [MetadataType(typeof(MD_Usuario))]

    public partial class Usuario
    {
        internal class MD_Usuario
        {
            [DisplayName]
            public int ID { get; set; }

            [Required]
            [DisplayName("Nome do Usuário")]
            public string Nome { get; set; }

            [Required]
            [DisplayName("Senha")]
            public string Senha { get; set; }

            [Required]
            [DisplayName("Email")]
            public string Email { get; set; }

            [Required]
            [DisplayName("CPF")]
            public string CPF { get; set; }

            [Required]
            [DisplayName("Telefone")]
            public string Telefone { get; set; }

            [DisplayName("Permissão")]
            public string Roles { get; set; }
        }
    }
}
