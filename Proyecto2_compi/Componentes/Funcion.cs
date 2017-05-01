
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Funcion
    {
        string tipo;
        string nombre;
        bool arreglo;
        public Variables variables;

        public Funcion siguiente;
        public Funcion anterior;
        public Parametros parametros;
        

        public Funcion(string t, string n)
        {
            nombre = n;
            tipo = t;

            variables = null;
            parametros = null;

            siguiente = null;
            anterior = null;
        }
    }
}
