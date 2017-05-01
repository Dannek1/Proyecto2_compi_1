using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Extension
    {
        string clase;

        public Extension siguiente;
        public Extension anterior;

        public Extension(string c)
        {
            clase = c;

            siguiente = null;
            anterior = null;
        }
    }
}
