using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoStop.MODEL.ViewModels
{
    public class ClientePermissoes
    {
        public int IDCliente { get; set; }
        public int IDEstabelecimento { get; set; }
        public string NomeCliente { get; set; }
        public bool Role { get; set; }
    }
}
