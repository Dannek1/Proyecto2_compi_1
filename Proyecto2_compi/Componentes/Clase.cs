using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Clase
    {
        private string nombre;
        private bool publico;
        private bool final;

        public ParseTreeNode nodo;

        public Funciones funciones;
        public Variables variables;

        public Clase siguiente;
        public Clase anterior;
        Extensiones extensiones;

        private bool extiende;
        private string nombreEx;

        public Clase(string nombre,bool p)
        {
            this.nombre = nombre;
            funciones = new Funciones();
            variables = new Variables();
            publico = p;

            final = false;
            extiende = false;
            nombreEx = "";
            extensiones = null;
        }

        public Clase(string nombre, bool p,  bool f)
        {
            this.nombre = nombre;
            funciones = new Funciones();
            variables = new Variables();

            publico = p;
            final = f;
            extiende = true;
            nombreEx = "";
            extensiones = null;
        }

        public Clase(string nombre,bool p, string ex)
        {
            this.nombre = nombre;
            funciones = new Funciones();
            variables = new Variables();

            publico = p;
            final = false;
            extiende = true;
            nombreEx = ex;
            extensiones = new Extensiones();

            string[] extendido = nombreEx.Split(',');

            if (extendido.Length > 1)
            {
                for(int x = 0; x < extendido.Length; x++)
                {
                    extensiones.Insertar(new Extension(extendido[x]));
                }
            }
            else
            {
                extensiones.Insertar(new Extension(extendido[0]));
            }



        }

        public Clase(string nombre, bool p, string ex,bool f)
        {
            this.nombre = nombre;
            funciones = new Funciones();
            variables = new Variables();

            publico = p;
            final = f;
            extiende = true;
            nombreEx = ex;
            extensiones = new Extensiones();

            string[] extendido = nombreEx.Split(',');

            if (extendido.Length > 1)
            {
                for (int x = 0; x < extendido.Length; x++)
                {
                    extensiones.Insertar(new Extension(extendido[x]));
                }
            }
            else
            {
                extensiones.Insertar(new Extension(extendido[0]));
            }
        }

        public string GetNombre()
        {
            return nombre;
        }

        public bool IsExtiende()
        {
            return extiende;
        }

        public bool IsPublico()
        {
            return publico;
        }

        public bool IsFinal()
        {
            return final;
        }


    }
}
