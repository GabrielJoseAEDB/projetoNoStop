using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoStop.MODEL
{
    [MetadataType(typeof(MD_roles))]
    public partial class Roles
    {
        internal class MD_roles
        {
            [DisplayName("Nome da Permissão")]
            public string Nome { get; set; }

            [DisplayName("Status")]
            public int IDStatus { get; set; }
        }
    }
}
