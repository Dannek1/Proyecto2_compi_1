
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

        private bool final;
        private bool publico;

        public Funcion siguiente;
        public Funcion anterior;
        public Parametros parametros;

        

        public Funcion(string t, string n)
        {
            nombre = n;
            tipo = t;

            
            variables = new Variables();
            parametros = null;

            siguiente = null;
            anterior = null;

            final = false;
            publico = false; ;
        }



        public Funcion(string t, string n, bool f)
        {
            nombre = n;
            tipo = t;

            variables = new Variables();
            parametros = null;

            siguiente = null;
            anterior = null;

            final = f;
            publico = false;
        }

        public Funcion(string t, string n, bool f,bool p)
        {
            nombre = n;
            tipo = t;

            variables = new Variables();
            parametros = null;

            siguiente = null;
            anterior = null;

            final = f;
            publico = p;
        }

        public bool IsFinal()
        {
            return final;
        }

        public bool IsPublico()
        {
            return publico;
        }
    }
}
