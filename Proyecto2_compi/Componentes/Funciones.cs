﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Funciones
    {
        Funcion cabeza;
        Funcion ultimo;
        public Funcion aux;

        public Funciones()
        {
            cabeza = null;
            ultimo = null;
            aux = null;

        }

        public void Insertar(Funcion nuevo)
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

                ultimo = aux;

            }
        }

        public bool ExisteF(string nombre)
        {
            bool respuesta = false;

            if (cabeza != null)
            {
                aux = cabeza;

                bool seguir = true;


                while (seguir)
                {
                    if (aux.GetNombre().Equals(nombre))
                    {
                        respuesta = true;
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
                            respuesta = false;
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

        public Funcion Existe(string nombre)
        {
            

            aux = cabeza;

            bool seguir = true;

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
                        
                        seguir = false;
                    }
                }
            }

            return aux;
        }

        
    }
}
