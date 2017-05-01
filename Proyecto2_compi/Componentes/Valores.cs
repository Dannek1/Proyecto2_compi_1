using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Valores
    {
        string valor;
        int indice;
        public Valores siguiente;
        public Valores anterior;

        public Valores(string v,int i)
        {
            valor = v;
            indice = i;

            siguiente = null;
            anterior = null;
        }
    }
}
