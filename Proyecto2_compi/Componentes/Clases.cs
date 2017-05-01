using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Clases
    {
        Clase cabeza;
        Clase ultimo;
        Clase aux;
        
        

        public Clases()
        {
            cabeza = null;
            ultimo = null;
            aux = null;

        }

        public void Insertar(Clase nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;

            }
            else if (ultimo == null)
            {
                ultimo = nuevo;

                cabeza.siguiente = nuevo;
                ultimo.anterior = nuevo;
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
