using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoStop.MODEL
{
    [MetadataType(typeof(MD_cliente))]
    public partial class Cliente
    {
        internal class MD_cliente
        {
            [DisplayName("Usuário")]
            public int IDUsuario { get; set; }

            [DisplayName("Permissão")]
            public int IDRole { get; set; }

            [DisplayName("Estabelecimento")]
            public int IDEstabelecimento { get; set; }
        }
    }
}
