﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Parametro
    {
        string tipo;
        string nombre;

        public Parametro siguiente;
        public Parametro anterior;

        public Parametro(string t, string n)
        {
            tipo = t;
            nombre = n;

            siguiente = null;
            anterior = null;
        }
    }
}
