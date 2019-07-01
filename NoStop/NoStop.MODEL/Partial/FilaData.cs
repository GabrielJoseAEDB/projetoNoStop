using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NoStop.MODE
{
    [MetadataType(typeof(MD_roles))]
    public partial class FilaData
    {
        internal class MD_roles
        {
            [DisplayName("Cliente")]
            public int IDCliente { get; set; }

            [DisplayName("Setor")]
            public int IDSetor { get; set; }
        }
    }
}
