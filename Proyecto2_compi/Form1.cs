using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto2_compi
{
    public partial class Form1 : Form
    {

        String graph = "";
        bool globales;
        bool parametro_funcion;

        Clases clases;
        Clase clase_n;
        Funcion nuevo_f;
        Parametro nuevo_P;
        Graphics dibujo;


        public Form1()
        {
            InitializeComponent();
            clases = new Clases();
            globales = false;
            dibujo = pictureBox1.CreateGraphics();
        }

        public string esCadenaValida(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);
            
            string a = "";
            if (arbol.HasErrors())
            {

                MessageBox.Show("Errores en la cadena de entrada");


                int elementos = arbol.ParserMessages.Count;

                for (int x = 0; x < elementos; x++)
                {

                    a += "Error en " + arbol.ParserMessages[x].Location + ";" + arbol.ParserMessages[x].Message + "\n";


                }


                if (arbol.Root != null)
                {
                    Genarbol(arbol.Root);
                    GenerateGraph("Entrada.txt", "C:/Fuentes/");

                }



            }
            else
            {
                if (arbol.Root != null)
                {
                    Genarbol(arbol.Root);
                    GenerateGraph("Entrada.txt", "C:/Fuentes/");

                    
                    Actuar(arbol.Root);
                    



                }
            }

            return a;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            Gramatica grammatica = new Gramatica();
            string respuesta = esCadenaValida(this.textBox1.Text, grammatica);
            if (respuesta != "")
            {
                

                MessageBox.Show("Arbol de Analisis Sintactico Constuido !!!");

                textBox2.Text = respuesta;

            }
            else
            {

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        public void Genarbol(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Fuentes/Ejemplo.dot");
            f.Write("digraph lista{ rankdir=TB;node [shape = box, style=rounded]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();

        }
        public void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"red\", style =\"filled\", shape=\"circle\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \n";
                }
            }
        }


        private static void GenerateGraph(string fileName, string path)
        {
            try
            {
                Process.Start("C:\\Fuentes\\Graficar.bat");





            }
            catch (Exception e)
            {

            }
        }


        string Actuar(ParseTreeNode nodo)
        {

            string resultado = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            Actuar(nodo.ChildNodes[0]);
                            Actuar(nodo.ChildNodes[1]);
                        }

                        break;
                    }

                case "Cabeza":
                    {
                        string nombre;
                        bool privacidad = false;
                        bool final = false;
                        string extension;


                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (Actuar(nodo.ChildNodes[0]) == "publico")
                            {
                                privacidad = true;
                            }
                            else if (Actuar(nodo.ChildNodes[0]) == "privado")
                            {
                                privacidad = false;

                            }
                            else if (nodo.ChildNodes[0].Token.Value.ToString() == "Conservar")
                            {

                                final = true;
                            }



                            nombre = nodo.ChildNodes[2].Token.Text;

                            clase_n = new Clase(nombre, privacidad, final);
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            if (Actuar(nodo.ChildNodes[0]) == "publico")
                            {
                                privacidad = true;
                            }
                            else if (Actuar(nodo.ChildNodes[0]) == "privado")
                            {
                                privacidad = false;

                            }


                            if (nodo.ChildNodes[1].Token.Value.ToString() == "Conservar")
                            {

                                final = true;

                                nombre = nodo.ChildNodes[2].Token.Text;
                                clase_n = new Clase(nombre, privacidad, final);

                            }
                            else
                            {
                                nombre = nodo.ChildNodes[2].Token.Text;

                                extension = Actuar(nodo.ChildNodes[3]);

                                clase_n = new Clase(nombre, privacidad, extension);



                            }



                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (Actuar(nodo.ChildNodes[0]) == "publico")
                            {
                                privacidad = true;
                            }
                            else if (Actuar(nodo.ChildNodes[0]) == "privado")
                            {
                                privacidad = false;

                            }

                            final = true;
                            nombre = nodo.ChildNodes[3].Token.Text;

                            extension = Actuar(nodo.ChildNodes[4]);
                            clase_n = new Clase(nombre, privacidad, extension, final);

                        }

                        clases.Insertar(clase_n);


                        break;
                    }

                case "Visibilidad":
                    {
                        if (nodo.ChildNodes[0].Term.Name.ToString() == "publico")
                        {
                            resultado = "publico";
                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "privado")
                        {
                            resultado = "privado";
                        }


                        break;
                    }

                case "Extensiones":
                    {

                        resultado = Actuar(nodo.ChildNodes[1]);


                        break;
                    }

                case "Extension":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado += Actuar(nodo.ChildNodes[0]) + ",";
                            resultado += nodo.ChildNodes[2].Token.Value.ToString();
                        }
                        else
                        {
                            resultado += nodo.ChildNodes[0].Token.Value.ToString();

                        }

                        break;

                    }

                case "Cuerpo":
                    {
                        resultado = Actuar(nodo.ChildNodes[1]);
                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            Actuar(nodo.ChildNodes[0]);
                            Actuar(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Componente":
                    {
                        string nombre;
                        string tipo;
                        bool final;
                        bool privacidad=false;


                        if (nodo.ChildNodes.Count == 1)
                        {
                            globales = true;
                            Actuar(nodo.ChildNodes[0]);
                            globales = false;
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {

                            nombre = nodo.ChildNodes[0].Token.Text;
                            tipo = "void";

                            nuevo_f = new Funcion(tipo, nombre);
                            Actuar(nodo.ChildNodes[4]);

                            clase_n.funciones.Insertar(nuevo_f);
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {
                                final = true;
                                nombre = nodo.ChildNodes[1].Token.Text;

                                tipo = "void";

                                nuevo_f = new Funcion(tipo, nombre, final);
                                Actuar(nodo.ChildNodes[5]);

                                clase_n.funciones.Insertar(nuevo_f);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Visibilidad"))
                            {
                                if (Actuar(nodo.ChildNodes[0]) == "publico")
                                {
                                    privacidad = true;
                                }
                                else if (Actuar(nodo.ChildNodes[0]) == "privado")
                                {
                                    privacidad = false;

                                }

                                nombre = nodo.ChildNodes[1].Token.Text;

                                tipo = "void";

                                nuevo_f = new Funcion(tipo, nombre, false, privacidad);
                                Actuar(nodo.ChildNodes[5]);
                                clase_n.funciones.Insertar(nuevo_f);

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Tipo"))
                            {

                                tipo = Actuar(nodo.ChildNodes[0]);
                                nombre = nodo.ChildNodes[1].Token.Text;

                                nuevo_f = new Funcion(tipo, nombre, false, privacidad);
                                Actuar(nodo.ChildNodes[5]);
                                clase_n.funciones.Insertar(nuevo_f);

                            }
                            else
                            {
                                nombre = nodo.ChildNodes[0].Token.Text;

                                tipo = "void";

                                nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                nuevo_f.parametros = new Parametros();

                                parametro_funcion = true;
                                Actuar(nodo.ChildNodes[2]);
                                parametro_funcion = false;


                                Actuar(nodo.ChildNodes[5]);
                                clase_n.funciones.Insertar(nuevo_f);

                            }

                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {


                                if (nodo.ChildNodes[1].Term.Name.ToString().Equals("Tipo"))
                                {
                                    final = true;
                                    nombre = nodo.ChildNodes[2].Token.Text;

                                    tipo = Actuar(nodo.ChildNodes[1]);

                                    nuevo_f = new Funcion(tipo, nombre, final);


                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }
                                else
                                {
                                    final = true;
                                    nombre = nodo.ChildNodes[1].Token.Text;

                                    tipo = "void";

                                    nuevo_f = new Funcion(tipo, nombre, final);

                                    nuevo_f.parametros = new Parametros();

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[3]);
                                    parametro_funcion = false;


                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Tipo"))
                            {

                                if (nodo.ChildNodes[1].Token.Text.Equals("[]"))
                                {
                                    tipo = Actuar(nodo.ChildNodes[0]);
                                    nombre = nodo.ChildNodes[2].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre);

                                    nuevo_f.SetArreglor(true);

                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);


                                }
                                else
                                {
                                    tipo = Actuar(nodo.ChildNodes[0]);
                                    nombre = nodo.ChildNodes[1].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre);

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[3]);
                                    parametro_funcion = false;

                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }


                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Visibilidad"))
                            {

                                if (Actuar(nodo.ChildNodes[0]) == "publico")
                                {
                                    privacidad = true;
                                }
                                else if (Actuar(nodo.ChildNodes[0]) == "privado")
                                {
                                    privacidad = false;

                                }

                                if (nodo.ChildNodes[1].Term.Name.ToString().Equals("Tipo"))
                                {
                                    tipo = Actuar(nodo.ChildNodes[1]);

                                    nombre = Actuar(nodo.ChildNodes[2]);

                                    nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }
                                else
                                {

                                    tipo = "void";
                                    nombre = nodo.ChildNodes[1].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                    nuevo_f.parametros = new Parametros();
                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[3]);
                                    parametro_funcion = false;

                                    Actuar(nodo.ChildNodes[6]);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }



                            }

                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {
                                final = true;
                                tipo = Actuar(nodo.ChildNodes[1]);

                                if (nodo.ChildNodes[2].Token.Text.Equals("[]"))
                                {
                                    nombre = Actuar(nodo.ChildNodes[3]);

                                    nuevo_f = new Funcion(tipo, nombre, final);

                                    nuevo_f.SetArreglor(true);

                                    Actuar(nodo.ChildNodes[7]);

                                    clase_n.funciones.Insertar(nuevo_f);

                                }
                                else
                                {
                                    nombre = Actuar(nodo.ChildNodes[2]);

                                    nuevo_f = new Funcion(tipo, nombre, final);

                                    nuevo_f.parametros = new Parametros();

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[4]);
                                    parametro_funcion = false;

                                    Actuar(nodo.ChildNodes[7]);

                                    clase_n.funciones.Insertar(nuevo_f);

                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Visibilidad"))
                            {
                                if (Actuar(nodo.ChildNodes[0]) == "publico")
                                {
                                    privacidad = true;
                                }
                                else if (Actuar(nodo.ChildNodes[0]) == "privado")
                                {
                                    privacidad = false;

                                }

                                tipo = Actuar(nodo.ChildNodes[1]);

                                if (nodo.ChildNodes[2].Token.Text.Equals("[]"))
                                {
                                    nombre = Actuar(nodo.ChildNodes[3]);

                                    nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                    nuevo_f.SetArreglor(true);

                                    Actuar(nodo.ChildNodes[7]);

                                    clase_n.funciones.Insertar(nuevo_f);

                                }
                                else
                                {
                                    nombre = Actuar(nodo.ChildNodes[2]);

                                    nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                    nuevo_f.parametros = new Parametros();

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[4]);
                                    parametro_funcion = false;

                                    Actuar(nodo.ChildNodes[7]);

                                    clase_n.funciones.Insertar(nuevo_f);

                                }

                            }
                            else
                            {
                                tipo = Actuar(nodo.ChildNodes[0]);
                                nombre = Actuar(nodo.ChildNodes[2]);

                                nuevo_f = new Funcion(tipo, nombre);

                                nuevo_f.SetArreglor(true);


                                nuevo_f.parametros = new Parametros();

                                parametro_funcion = true;
                                Actuar(nodo.ChildNodes[4]);
                                parametro_funcion = false;

                                Actuar(nodo.ChildNodes[7]);

                                clase_n.funciones.Insertar(nuevo_f);

                            }

                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {
                                final = true;
                                tipo = Actuar(nodo.ChildNodes[1]);
                                nombre = Actuar(nodo.ChildNodes[3]);

                                nuevo_f = new Funcion(tipo, nombre, final);

                                nuevo_f.SetArreglor(true);

                                nuevo_f.parametros = new Parametros();

                                parametro_funcion = true;
                                Actuar(nodo.ChildNodes[5]);
                                parametro_funcion = false;

                                Actuar(nodo.ChildNodes[8]);

                                clase_n.funciones.Insertar(nuevo_f);




                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Visibilidad"))
                            {
                                if (Actuar(nodo.ChildNodes[0]) == "publico")
                                {
                                    privacidad = true;
                                }
                                else if (Actuar(nodo.ChildNodes[0]) == "privado")
                                {
                                    privacidad = false;

                                }

                                if (nodo.ChildNodes[1].Term.Name.ToString().Equals("Tipo"))
                                {
                                    tipo = Actuar(nodo.ChildNodes[1]);
                                    nombre = nodo.ChildNodes[3].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre,false,privacidad);

                                    nuevo_f.SetArreglor(true);

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[5]);
                                    parametro_funcion = false;

                                    Actuar(nodo.ChildNodes[8]);

                                    clase_n.funciones.Insertar(nuevo_f);


                                }
                                else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("Conservar"))
                                {
                                    final = true;

                                    tipo = Actuar(nodo.ChildNodes[2]);
                                    nombre = nodo.ChildNodes[4].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre, final, privacidad);

                                    nuevo_f.SetArreglor(true);

                                    Actuar(nodo.ChildNodes[8]);

                                    clase_n.funciones.Insertar(nuevo_f);


                                }


                            }


                        }
                        else
                        {
                            if (Actuar(nodo.ChildNodes[0]) == "publico")
                            {
                                privacidad = true;
                            }
                            else if (Actuar(nodo.ChildNodes[0]) == "privado")
                            {
                                privacidad = false;

                            }

                            final = true;

                            tipo = Actuar(nodo.ChildNodes[2]);
                            nombre = nodo.ChildNodes[4].Token.Text;

                            nuevo_f = new Funcion(tipo, nombre, final, privacidad);

                            nuevo_f.SetArreglor(true);

                            nuevo_f.parametros = new Parametros();


                            parametro_funcion = true;
                            Actuar(nodo.ChildNodes[6]);
                            parametro_funcion = false;

                            Actuar(nodo.ChildNodes[9]);

                            clase_n.funciones.Insertar(nuevo_f);




                        }

                        break;
                    }

                case "Sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            Actuar(nodo.ChildNodes[0]);
                            Actuar(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Sentencia":
                    {
                        Actuar(nodo.ChildNodes[0]);

                        break;
                    }

                case "Retorno":
                    {
                        break;
                    }

                case "Asignacion":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string variable = nodo.ChildNodes[0].Token.Text;
                            string valor;

                            if (clase_n.variables.Buscar_existe(variable))
                            {
                                clase_n.variables.Buscar(variable);

                                valor = Actuar(nodo.ChildNodes[2]);

                                clase_n.variables.aux.SetValor(valor);

                            }
                            else if (nuevo_f.variables.Buscar_existe(variable))
                            {
                                nuevo_f.variables.Buscar(variable);
                                valor = Actuar(nodo.ChildNodes[2]);
                                nuevo_f.variables.aux.SetValor(valor);
                            }
                            else
                            {
                                textBox2.Text += "(Error en "+nodo.Token.Location.Line+","+ nodo.Token.Location.Column + ") No existe variable :\"" + variable + "\"";
                            }

                        } else if (nodo.ChildNodes.Count == 3)
                        {
                            bool aumentar=false;
                            bool disminuir = false;
                            



                            string variable = nodo.ChildNodes[0].Token.Text;

                            if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "aumentar")
                            {
                                aumentar = true;
                            }
                            else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "disminuir")
                            {
                                disminuir = true;
                            }


                            if (clase_n.variables.Buscar_existe(variable))
                            {
                                clase_n.variables.Buscar(variable);

                                if (aumentar)
                                {
                                    if (clase_n.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(clase_n.variables.aux.GetValor());
                                        valor++;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(clase_n.variables.aux.GetValor());
                                        valor++;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(clase_n.variables.aux.GetValor());
                                        valor++;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                    }

                                }
                                else if (disminuir)
                                {
                                    if (clase_n.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(clase_n.variables.aux.GetValor());
                                        valor--;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(clase_n.variables.aux.GetValor());
                                        valor--;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(clase_n.variables.aux.GetValor());
                                        valor--;
                                        clase_n.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                    }
                                }

                               

                                //clase_n.variables.aux.SetValor(valor);

                            }
                            else if (nuevo_f.variables.Buscar_existe(variable))
                            {
                                nuevo_f.variables.Buscar(variable);
                                if (aumentar)
                                {
                                    if (nuevo_f.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(nuevo_f.variables.aux.GetValor());
                                        valor++;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (nuevo_f.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(nuevo_f.variables.aux.GetValor());
                                        valor++;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (nuevo_f.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(nuevo_f.variables.aux.GetValor());
                                        valor++;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                    }

                                }
                                else if (disminuir)
                                {
                                    if (nuevo_f.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(nuevo_f.variables.aux.GetValor());
                                        valor--;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (nuevo_f.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(nuevo_f.variables.aux.GetValor());
                                        valor--;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (nuevo_f.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(nuevo_f.variables.aux.GetValor());
                                        valor--;
                                        nuevo_f.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                    }
                                }



                                // nuevo_f.variables.aux.SetValor(valor);
                            }
                            else
                            {
                                textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") No existe variable :\"" + variable + "\"";
                            }
                        }


                            break;
                    }

                case "Declaracion":
                    {

                        string nombreV;
                        string valorV;
                        string tipoV;

                        if (nodo.ChildNodes.Count == 4)
                        {
                            tipoV = Actuar(nodo.ChildNodes[1]);
                            nombreV = Actuar(nodo.ChildNodes[2]);
                            string[] nombres = nombreV.Split(',');

                            if (nombres.Length > 1)
                            {
                                for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[cuenta]);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        nuevo_f.variables.Insertar(nuevo);
                                    }
                                }
                            }
                            else
                            {
                                Variable nuevo = new Variable(tipoV, nombres[0]);

                                if (globales)
                                {
                                    clase_n.variables.Insertar(nuevo);
                                }
                                else
                                {
                                    nuevo_f.variables.Insertar(nuevo);
                                }
                            }




                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            tipoV = Actuar(nodo.ChildNodes[2]);
                            nombreV = Actuar(nodo.ChildNodes[3]);
                            string[] nombres = nombreV.Split(',');

                            if (nombres.Length > 1)
                            {
                                for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[cuenta],true);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        nuevo_f.variables.Insertar(nuevo);
                                    }
                                }
                            }
                            else
                            {
                                Variable nuevo = new Variable(tipoV, nombres[0],true);

                                if (globales)
                                {
                                    clase_n.variables.Insertar(nuevo);
                                }
                                else
                                {
                                    nuevo_f.variables.Insertar(nuevo);
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {

                            if (nodo.ChildNodes[3].Token.Text.Equals("="))
                            {

                                tipoV = Actuar(nodo.ChildNodes[1]);
                                nombreV = Actuar(nodo.ChildNodes[2]);
                                string[] nombres = nombreV.Split(',');
                                valorV = Actuar(nodo.ChildNodes[4]);

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta]);
                                        nuevo.SetValor(valorV);
                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                        }
                                        else
                                        {
                                            nuevo_f.variables.Insertar(nuevo);
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0]);
                                    nuevo.SetValor(valorV);
                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        nuevo_f.variables.Insertar(nuevo);
                                    }
                                }
                            }
                            else //arreglo
                            {

                            }
                           

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[4].Token.Text.Equals("="))
                            {
                                tipoV = Actuar(nodo.ChildNodes[2]);
                                nombreV = Actuar(nodo.ChildNodes[3]);
                                string[] nombres = nombreV.Split(',');
                                valorV = Actuar(nodo.ChildNodes[5]);

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], true);
                                        nuevo.SetValor(valorV);
                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                        }
                                        else
                                        {
                                            nuevo_f.variables.Insertar(nuevo);
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], true);
                                    nuevo.SetValor(valorV);
                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        nuevo_f.variables.Insertar(nuevo);
                                    }
                                }
                            }
                        }
                            break;
                    }

                case "Tipo":
                    {

                        resultado = nodo.ChildNodes[0].Token.Value.ToString();
                        break;
                    }

                case "Nombres":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado += Actuar(nodo.ChildNodes[0]) + ",";
                            resultado += Actuar(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado += Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Nombre":
                    {
                        resultado = nodo.ChildNodes[0].Token.Value.ToString();
                        break;
                    }

                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = Convert.ToString(Aritmeticas(nodo));
                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {


                                if (clase_n.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                                {
                                    clase_n.variables.Buscar(nodo.ChildNodes[0].Token.Text);

                                    resultado = clase_n.variables.aux.GetValor();

                                }
                                else if (nuevo_f.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                                {
                                    nuevo_f.variables.Buscar(nodo.ChildNodes[0].Token.Text);
                                    resultado = nuevo_f.variables.aux.GetValor();
                                }
                                else
                                {
                                    textBox2.Text += "No existe variable :\"" + nodo.ChildNodes[0].Token.Text + "\"";
                                }
                            }
                            else
                            {
                                resultado = Actuar(nodo.ChildNodes[0]);
                            }


                        }
                        break;
                    }

                case "Valor":

                    {
                        resultado = nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "Pintar":
                    {
                        Actuar(nodo.ChildNodes[0]);
                        break;
                    }

                case "Punto":
                    {
                        int x;
                        int y;
                        string colorb;
                        string color;
                        int diametro;

                        x = Convert.ToInt32(Actuar(nodo.ChildNodes[1]));
                        y = Convert.ToInt32(Actuar(nodo.ChildNodes[3]));
                        colorb = Actuar(nodo.ChildNodes[5]);
                        color = colorb[1].ToString()+ colorb[2] + colorb[3] + colorb[4] + colorb[5] + colorb[6] + colorb[7] + "";
                        diametro = Convert.ToInt32(Actuar(nodo.ChildNodes[7]));
                        Color _color = System.Drawing.ColorTranslator.FromHtml(color);
                        SolidBrush myBrush = new SolidBrush(_color);


                        dibujo.FillEllipse(myBrush, x, y, diametro, diametro);


                        pictureBox1.Update();
                        
                        
                        break;
                    }

                case "Ovalo_R":
                    {

                        int x;
                        int y;
                        string colorb;
                        string color;
                        string tipoc;
                        int ancho;
                        int alto;
                        char tipo;

                        x = Convert.ToInt32(Actuar(nodo.ChildNodes[1]));
                        y = Convert.ToInt32(Actuar(nodo.ChildNodes[3]));
                        colorb = Actuar(nodo.ChildNodes[5]);
                        color = colorb[1].ToString() + colorb[2] + colorb[3] + colorb[4] + colorb[5] + colorb[6] + colorb[7] + "";
                        Color _color = System.Drawing.ColorTranslator.FromHtml(color);
                        SolidBrush myBrush = new SolidBrush(_color);


                        ancho = Convert.ToInt32(Actuar(nodo.ChildNodes[7]));
                        alto = Convert.ToInt32(Actuar(nodo.ChildNodes[9]));

                        tipoc = Actuar(nodo.ChildNodes[11]);
                        tipo = tipoc[1];
                        if (tipo == 'o')
                        {
                            dibujo.FillEllipse(myBrush, x, y, ancho, alto);
                        }
                        else if (tipo == 'r')
                        {
                            dibujo.FillRectangle(myBrush, new Rectangle(x, y, ancho, alto));

                            
                        }
                        pictureBox1.Update();





                        break;
                    }

                case "Parametros":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = Actuar(nodo.ChildNodes[0]);
                            resultado = Actuar(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Parametro":
                    {
                        string tipo;
                        string nombre;

                        tipo= Actuar(nodo.ChildNodes[0]);
                        nombre = nodo.ChildNodes[1].Token.Value.ToString();


                        nuevo_P = new Parametro(tipo, nombre);

                        if (parametro_funcion)
                        {
                            nuevo_f.parametros.Insertar(nuevo_P);
                        }

                        break;
                    }



            }

            return resultado;
        }

        double Aritmeticas(ParseTreeNode nodo)
        {
            double resultado = 0;

            switch (nodo.Term.Name.ToString())
            {
                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "suma")
                            {

                                double numero1 = Aritmeticas(nodo.ChildNodes[0]);
                                double numero2 = Aritmeticas(nodo.ChildNodes[2]);
                                Console.WriteLine(numero1 + "+" + numero2);
                                resultado = numero1 + numero2;

                            }
                            else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "resta")
                            {
                                double numero1 = Aritmeticas(nodo.ChildNodes[0]);
                                double numero2 = Aritmeticas(nodo.ChildNodes[2]);
                                Console.WriteLine(numero1 + "-" + numero2);
                                resultado = numero1 - numero2;

                            }
                            else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "multi")
                            {
                                double numero1 = Aritmeticas(nodo.ChildNodes[0]);
                                double numero2 = Aritmeticas(nodo.ChildNodes[2]);
                                Console.WriteLine(numero1 + "*" + numero2);
                                resultado = numero1 * numero2;

                            }
                            else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "div")
                            {
                                double numero1 = Aritmeticas(nodo.ChildNodes[0]);
                                double numero2 = Aritmeticas(nodo.ChildNodes[2]);
                                Console.WriteLine(numero1 + "/" + numero2);
                                resultado = numero1 / numero2;


                            }
                            else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "power")
                            {
                                double numero1 = Aritmeticas(nodo.ChildNodes[0]);
                                double numero2 = Aritmeticas(nodo.ChildNodes[2]);
                                Console.WriteLine(numero1 + "^" + numero2);
                                resultado = Math.Pow(numero1, numero2);



                            }


                        }
                        else
                        {
                            resultado = Aritmeticas(nodo.ChildNodes[0]);

                        }
                        break;
                    }

                case "Valor":

                    {
                        resultado = Convert.ToDouble(nodo.ChildNodes[0].Token.Text);
                        break;
                    }

                
            }


            return resultado;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
