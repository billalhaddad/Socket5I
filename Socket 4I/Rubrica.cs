using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Socket_4I
{
    internal class Rubrica
    {
        public List<Contatto> _contatti { get; private set; }
        public Rubrica()
        {
            _contatti = new List<Contatto>();
        }

    }
}
