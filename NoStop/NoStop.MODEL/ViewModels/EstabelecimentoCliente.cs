using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoStop.MODEL.ViewModels
{
    public class EstabelecimentoCliente
    {
        public int IdCliente { get; set; }
        public int idEstabelecimento { get; set; }
        public string NomeEstabelecimento { get; set; }
        public string Endereco { get; set; }
    }
}
