using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Extensiones
    {
        Extension cabeza;
        Extension ultimo;
        Extension aux;

        public Extensiones()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Extension nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;

            }else if (ultimo == null)
            {
                ultimo = nuevo;

                cabeza.siguiente = ultimo;
                ultimo.anterior = cabeza;

            }
            else
            {
                aux = nuevo;

                aux.anterior = ultimo;
                ultimo.siguiente = aux;

                ultimo = aux;
            }
        }
    }
}
