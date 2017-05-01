using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Arreglo
    {
        Valores cabeza;
        Valores ultimo;
        Valores aux;

        public Arreglo()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void insertar(Valores nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;

            }
            else if (ultimo == null){

                ultimo = nuevo;

                cabeza.siguiente = ultimo;
                ultimo.anterior = cabeza;
            }
            else
            {
                aux = nuevo;

                ultimo.siguiente = aux;
                aux.anterior = ultimo;

                ultimo = aux;
            }
        }
    }
}
