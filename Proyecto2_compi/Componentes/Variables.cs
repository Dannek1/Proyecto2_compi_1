﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Variables
    {
        Variable cabeza;
        Variable ultimo;
        public Variable aux;

        public Variables()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Variable nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else if (ultimo == null)
            {
                ultimo = nuevo;

                cabeza.siguiente = ultimo;
                ultimo.anterior = cabeza;

            }
            else
            {
                aux = nuevo;

                ultimo.siguiente = aux;
                aux.anterior = ultimo;

                ultimo = nuevo;
            }

        }

        public bool Buscar_existe(string nombre)
        {

            bool respuesta = false;
            bool seguir = true;


            if (cabeza != null)
            {
                aux = cabeza;

                while (seguir)
                {
                    if (aux.GetNombre().Equals(nombre))
                    {
                        seguir = false;
                        respuesta = true;
                    }
                    else
                    {
                        if (aux.siguiente != null)
                        {
                            aux = aux.siguiente;
                        }
                        else
                        {
                            seguir = false;
                        }
                    }
                }
            }
            else
            {
                respuesta = false;
            }
            


            return respuesta;
        }

        public Variable Buscar(string nombre)
        {
            
            bool seguir = true;
            aux = cabeza;

            while (seguir)
            {
                if (aux.GetNombre().Equals(nombre))
                {
                    seguir = false;
                    
                }
                else
                {
                    if (aux.siguiente != null)
                    {
                        aux = aux.siguiente;
                    }
                    else
                    {
                        aux = null;
                        seguir = false;
                    }
                }
            }


            return aux;
        }
    }
}
