using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Variable
    {
        Arreglo valores;
        bool arreglo;
        private string nombre;
        private string valor;
        private string tipo; 
        int dimensiones;
        int dim_ocupadas;

        public Variable siguiente;
        public Variable anterior;

        public Variable(string t,string n)
        {
            nombre = n;
            tipo = t;

            siguiente = null;
            anterior = null;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n,string v)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n, string v, int dimensiones)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;

            valores = new Arreglo();
            arreglo = true;
            this.dimensiones = dimensiones;

        }


        public void Asignar(string va)
        {
            valor = va;
        }

        public string GetValor()
        {
            return valor;

        }

        public string GetNombre()
        {
            return nombre;
        }

    }
}
