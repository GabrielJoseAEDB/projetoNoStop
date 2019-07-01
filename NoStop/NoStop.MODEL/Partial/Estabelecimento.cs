using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoStop.MODEL
{
    [MetadataType(typeof(MD_estab))]
    public partial class Estabelecimento
    {
        internal class MD_estab
        {
            [DisplayName("Endereço")]
            public string Endereco { get; set; }
        }
    }
}
