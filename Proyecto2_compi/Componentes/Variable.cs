using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Variable
    {
        Arreglo[] valores;
        bool arreglo;
        private string nombre;
        private string valor;
        private string tipo;
        private bool final;
        int dimensiones;
        int dim_ocupadas;

        public Variable siguiente;
        public Variable anterior;

        public Variable(string t, string n)
        {
            nombre = n;
            tipo = t;

            siguiente = null;
            anterior = null;

            final = false;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n,bool f)
        {
            nombre = n;
            tipo = t;

            siguiente = null;
            anterior = null;

            final = f;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n, string v)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;
            final = false;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n, string v,bool f)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;
            final = f;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n, string v, int dimensiones,string valoresN)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;

            valores = new Arreglo[dimensiones];
            string[] dimension = valoresN.Split(',');

            for(int x = 0; x < dimensiones; x++)
            {
                valores[x] = new Arreglo();
                valores[x].SetMax(Convert.ToInt32(dimension[x]));
            }

            arreglo = true;
            this.dimensiones = dimensiones;
            final = false;

        }

        public Variable(string t, string n, string v, int dimensiones, string valoresN, bool f)
        {
            nombre = n;
            tipo = t;
            valor = v;

            siguiente = null;
            anterior = null;

            valores = new Arreglo[dimensiones];
            string[] dimension = valoresN.Split(',');

            for (int x = 0; x < dimensiones; x++)
            {
                valores[x] = new Arreglo();
                valores[x].SetMax(Convert.ToInt32(dimension[x]));
            }

            if (valoresN.Equals(""))
            {

            }
            else
            {
                string[] valores_D = valoresN.Split(';');

                if (valores_D.Length > 1)
                {

                    for(int x = 0; x < valores_D.Length; x++)
                    {
                        string[] valores_f = valores_D[x].Split(',');
                        Agregar_Valores(valores_f, x);
                    }

                }
                else
                {
                    string[] valores_f = valores_D[0].Split(',');
                    Agregar_Valores(valores_f, 0);
                    
                }
            }

            arreglo = true;
            this.dimensiones = dimensiones;
            final = f;

        }


        public void Asignar(string va)
        {
            valor = va;
        }

        public string GetValor()
        {
            return valor;

        }

        public void SetValor(string s)
        {
            valor = s;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public string GetTipo()
        {
            return tipo;
        }

        public bool IsFinal()
        {
            return final;
        }

        void Agregar_Valores(string[] datos,int dimension)
        {
            int tope = datos.Length;

            if (tope > 1)
            {
                Valores nuevo = new Valores(datos[0], dimension);
            }
            else
            {
                for(int x = 0; x < tope; x++)
                {
                    Valores nuevo = new Valores(datos[x], dimension);
                }
            }
        }
    }
}