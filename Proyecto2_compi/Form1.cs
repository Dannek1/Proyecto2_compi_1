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
using ScintillaNET;

namespace Proyecto2_compi
{
    public partial class Form1 : Form
    {

        String graph = "";
        bool globales;
        bool parametro_funcion;
        int dimension;
        string tipo_Temp;
        string fun_actual;
        string clase_actual;
        bool retorna;
        string componentes = "";
        string errores = "";

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
            retorna = false;
            dibujo = pictureBox1.CreateGraphics();

            textBox1.Margins[0].Width = 40;
            textBox1.Styles[Style.LineNumber].Font = "Consolas";
            textBox1.Margins[0].Type = MarginType.Number;
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

                    errores += arbol.ParserMessages[x].Location.Line + ";" + arbol.ParserMessages[x].Location.Column + ";" + arbol.ParserMessages[x].Message+"@";

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

                textBox2.Text += respuesta;
                Reporte_Errores();
                button2.Visible = true;

                

            }
            else
            {

                MessageBox.Show("Arbol de Analisis Sintactico Constuido !!!");

                textBox2.Text += respuesta;
                Reporte_Errores();
                button2.Visible = true;
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
                            clase_n.nodo = nodo.ChildNodes[1];
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
                            clase_actual = nombre;

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
                                clase_actual = nombre;
                                clase_n = new Clase(nombre, privacidad, final);

                            }
                            else
                            {
                                nombre = nodo.ChildNodes[2].Token.Text;
                                clase_actual = nombre;

                                extension = Actuar(nodo.ChildNodes[3]);

                                clase_n = new Clase(nombre, privacidad, extension);

                                string[] extender = extension.Split(',');

                                for (int x = 0; x < extender.Length; x++)
                                {
                                    Heredar(clase_n, clases.Existe(extender[x]));
                                }
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
                            clase_actual = nombre;

                            extension = Actuar(nodo.ChildNodes[4]);


                            clase_n = new Clase(nombre, privacidad, extension, final);

                            string[] extender = extension.Split(',');

                            for(int x = 0; x < extender.Length; x++)
                            {
                                Heredar(clase_n, clases.Existe(extender[x]));
                            }

                        }

                        clases.Insertar(clase_n);
                        componentes += clase_n.GetNombre() + "," + "lienzo" + "," + "lienzo" + "," + "Principal"+";";
                        


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
                        string clase="";
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado += Actuar(nodo.ChildNodes[0]) + ",";
                            clase= nodo.ChildNodes[2].Token.Value.ToString();
                            if (clases.Existe_c(clase))
                            {
                                resultado += clase;
                                
;                            }
                            else
                            {
                                textBox2.Text += "(Error en " + nodo.ChildNodes[2].Token.Location.Line + "," + nodo.ChildNodes[2].Token.Location.Column + ") No existe la clase :\"" + clase + "\"";
                                errores += nodo.ChildNodes[2].Token.Location.Line + ";" + nodo.ChildNodes[2].Token.Location.Column+";"+"semantico"+";" +" No existe la clase:\"" + clase + "\"@";
                            }
                            
                        }
                        else
                        {
                           clase = nodo.ChildNodes[0].Token.Value.ToString();

                            if (clases.Existe_c(clase))
                            {
                                resultado += clase;
                                
                            }
                            else
                            {
                                textBox2.Text += "(Error en " + nodo.ChildNodes[0].Token.Location.Line + "," + nodo.ChildNodes[0].Token.Location.Column + ") No existe la clase :\"" + clase + "\"";

                                errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + " No existe la clase:\"" + clase + "\"@";
                            }

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
                            nuevo_f.nodo=nodo.ChildNodes[4];

                            clase_n.funciones.Insertar(nuevo_f);

                            componentes += nuevo_f.GetNombre() + "," +nuevo_f.GetTipo()+","+"metodo"+","+clase_n.GetNombre()+";";
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {
                                final = true;
                                nombre = nodo.ChildNodes[1].Token.Text;

                                tipo = "void";

                                nuevo_f = new Funcion(tipo, nombre, final);
                                nuevo_f.nodo = nodo.ChildNodes[5];

                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
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
                                nuevo_f.nodo = nodo.ChildNodes[5];
                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Tipo"))
                            {

                                tipo = Actuar(nodo.ChildNodes[0]);

                                if (tipo != "void")
                                {
                                    retorna = true;
                                }
                                nombre = nodo.ChildNodes[1].Token.Text;

                                nuevo_f = new Funcion(tipo, nombre, false, privacidad);
                                nuevo_f.nodo = nodo.ChildNodes[5];
                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

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


                                nuevo_f.nodo = nodo.ChildNodes[5];
                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

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

                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
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

                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Tipo"))
                            {

                                if (nodo.ChildNodes[1].Token.Text.Equals("[]"))
                                {
                                    tipo = Actuar(nodo.ChildNodes[0]);
                                    nombre = nodo.ChildNodes[2].Token.Text;

                                    if (tipo != "void")
                                    {
                                        retorna = true;
                                    }

                                    nuevo_f = new Funcion(tipo, nombre);

                                    nuevo_f.SetArreglor(true);

                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";


                                }
                                else
                                {
                                    tipo = Actuar(nodo.ChildNodes[0]);
                                    nombre = nodo.ChildNodes[1].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre);

                                    if (tipo != "void")
                                    {
                                        retorna = true;
                                    }

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[3]);
                                    parametro_funcion = false;

                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
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

                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
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


                                    nuevo_f.nodo = nodo.ChildNodes[6];
                                    

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
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

                                    nuevo_f.nodo = nodo.ChildNodes[7];

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

                                }
                                else
                                {
                                    nombre = Actuar(nodo.ChildNodes[2]);

                                    nuevo_f = new Funcion(tipo, nombre, final);

                                    nuevo_f.parametros = new Parametros();

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[4]);
                                    parametro_funcion = false;

                                    nuevo_f.nodo = nodo.ChildNodes[7];
                                    
                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

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

                                    nuevo_f.nodo = nodo.ChildNodes[7];

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

                                }
                                else
                                {
                                    nombre = Actuar(nodo.ChildNodes[2]);

                                    nuevo_f = new Funcion(tipo, nombre, false, privacidad);

                                    nuevo_f.parametros = new Parametros();

                                    parametro_funcion = true;
                                    Actuar(nodo.ChildNodes[4]);
                                    parametro_funcion = false;

                                    nuevo_f.nodo = nodo.ChildNodes[7];

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

                                }

                            }
                            else
                            {
                                tipo = Actuar(nodo.ChildNodes[0]);
                                nombre = Actuar(nodo.ChildNodes[2]);

                                nuevo_f = new Funcion(tipo, nombre);

                                nuevo_f.SetArreglor(true);

                                if (tipo != "void")
                                {
                                    retorna = true;
                                }

                                nuevo_f.parametros = new Parametros();

                                parametro_funcion = true;
                                Actuar(nodo.ChildNodes[4]);
                                parametro_funcion = false;

                                nuevo_f.nodo = nodo.ChildNodes[7];

                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

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

                                nuevo_f.nodo = nodo.ChildNodes[8];

                                clase_n.funciones.Insertar(nuevo_f);
                                componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";


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

                                    nuevo_f.nodo = nodo.ChildNodes[8];

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";


                                }
                                else if (nodo.ChildNodes[1].Term.Name.ToString().Equals("Conservar"))
                                {
                                    final = true;

                                    tipo = Actuar(nodo.ChildNodes[2]);
                                    nombre = nodo.ChildNodes[4].Token.Text;

                                    nuevo_f = new Funcion(tipo, nombre, final, privacidad);

                                    nuevo_f.SetArreglor(true);

                                    nuevo_f.nodo = nodo.ChildNodes[8];

                                    clase_n.funciones.Insertar(nuevo_f);
                                    componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";


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

                            nuevo_f.nodo = nodo.ChildNodes[9];

                            clase_n.funciones.Insertar(nuevo_f);
                            componentes += nuevo_f.GetNombre() + "," + nuevo_f.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";




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
                        if (retorna)
                        {

                            clase_n.funciones.aux.SetRetorno(Actuar(nodo.ChildNodes[1]));
                        }

                        break;
                    }

                case "Asignacion":
                    {
                        clase_n.funciones.Existe(fun_actual);
                        
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string variable = nodo.ChildNodes[0].Token.Text;
                            string valor;

                            
                           if (clase_n.funciones.aux.variables.Buscar_existe(variable))
                            {
                                clase_n.funciones.aux.variables.Buscar(variable);
                                valor = Actuar(nodo.ChildNodes[2]);
                                clase_n.funciones.aux.variables.aux.SetValor(valor);
                            }
                            else if (clase_n.variables.Buscar_existe(variable))
                            {
                                clase_n.variables.Buscar(variable);

                                tipo_Temp = clase_n.variables.aux.GetTipo();

                                valor = Actuar(nodo.ChildNodes[2]);

                                clase_n.variables.aux.SetValor(valor);

                            }                            
                            else
                            {
                                textBox2.Text += "(Error en "+ nodo.ChildNodes[0].Token.Location.Line+","+ nodo.ChildNodes[0].Token.Location.Column + ") No existe variable :\"" + variable + "\"";
                                errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + " No existe la variable:\"" + variable + "\"@";
                            }

                        }
                        else if (nodo.ChildNodes.Count == 3)
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


                            
                            if (clase_n.funciones.aux.variables.Buscar_existe(variable))
                            {
                                clase_n.funciones.aux.variables.Buscar(variable);
                                if (aumentar)
                                {
                                    if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor++;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor++;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor++;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.ChildNodes[0].Token.Location.Line + "," + nodo.ChildNodes[0].Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";

                                        errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + " la variable:\"" + variable + "\" no permite este tipo de operacion @";

                                    }

                                }
                                else if (disminuir)
                                {
                                    if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("entero"))
                                    {
                                        int valor = Convert.ToInt32(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor--;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("doble"))
                                    {
                                        double valor = Convert.ToDouble(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor--;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else if (clase_n.funciones.aux.variables.aux.GetTipo().Equals("caracter"))
                                    {
                                        char valor = Convert.ToChar(clase_n.funciones.aux.variables.aux.GetValor());
                                        valor--;
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(valor));
                                    }
                                    else
                                    {
                                        textBox2.Text += "(Error en " + nodo.ChildNodes[0].Token.Location.Line + "," + nodo.ChildNodes[0].Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";

                                        errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + "la variable:\"" + variable + "\"no permite este tipo de operacion@";
                                    }
                                }



                                // nuevo_f.variables.aux.SetValor(valor);
                            }
                            else if (clase_n.variables.Buscar_existe(variable))
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
                                        textBox2.Text += "(Error en " + nodo.ChildNodes[0].Token.Location.Line + "," + nodo.ChildNodes[0].Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                        errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + "la variable:\"" + variable + "\" no permite este tipo de operacion@";
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
                                        textBox2.Text += "(Error en " + nodo.ChildNodes[0].Token.Location.Line + "," + nodo.ChildNodes[0].Token.Location.Column + ") La Variable:\"" + variable + "\" no permite este tipo de operacion";
                                        errores += nodo.ChildNodes[0].Token.Location.Line + ";" + nodo.ChildNodes[0].Token.Location.Column + ";" + "semantico" + ";" + "la variable:\"" + variable + "\" no permite este tipo de operacion@";
                                    }
                                }



                                //clase_n.variables.aux.SetValor(valor);

                            }
                            else
                            {
                                textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") No existe variable :\"" + variable + "\"";
                                errores += nodo.Token.Location.Line + ";" + nodo.Token.Location.Column + ";" + "semantico" + ";" + " No existe la variable:\"" + variable + "\"@";
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
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
                            }
                            else
                            {
                                Variable nuevo = new Variable(tipoV, nombres[0]);

                                if (globales)
                                {
                                    clase_n.variables.Insertar(nuevo);
                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                }
                                else
                                {
                                    clase_n.funciones.Existe(fun_actual);
                                    clase_n.funciones.aux.variables.Insertar(nuevo);
                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
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
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
                            }
                            else
                            {
                                Variable nuevo = new Variable(tipoV, nombres[0],true);

                                if (globales)
                                {
                                    clase_n.variables.Insertar(nuevo);
                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                }
                                else
                                {
                                    clase_n.funciones.Existe(fun_actual);
                                    clase_n.funciones.aux.variables.Insertar(nuevo);
                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
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
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
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
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
                            }
                            else
                            {

                                dimension = 0;

                                tipoV = Actuar(nodo.ChildNodes[1]);

                                nombreV = Actuar(nodo.ChildNodes[3]);
                                string[] nombres = nombreV.Split(',');

                                string dimeniones=Actuar(nodo.ChildNodes[4]);

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta],"",dimension,dimeniones);
                                        
                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "variable" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], "", dimension, dimeniones);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
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
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
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
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
                            }
                            else
                            {
                                dimension = 0;
                                tipoV = Actuar(nodo.ChildNodes[2]);
                                nombreV = Actuar(nodo.ChildNodes[4]);
                                string[] nombres = nombreV.Split(',');

                                string dimeniones = Actuar(nodo.ChildNodes[5]);


                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta],"",dimension, dimeniones, true);
                                        
                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], "", dimension, dimeniones, true);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            dimension = 0;
                            tipoV = Actuar(nodo.ChildNodes[1]);
                            nombreV = Actuar(nodo.ChildNodes[3]);
                            string[] nombres = nombreV.Split(',');

                            string dimeniones = Actuar(nodo.ChildNodes[4]);
                            clase_n.funciones.Existe(fun_actual);

                            if (clase_n.variables.Buscar_existe(nodo.ChildNodes[6].Token.Text))
                            {
                                clase_n.variables.Buscar(nodo.ChildNodes[6].Token.Text);

                                valorV = clase_n.variables.aux.GetValor();

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta],valorV, dimension,dimeniones,false);
                                        
                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                             componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);
                                    
                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }



                            }
                            else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.ChildNodes[6].Token.Text))
                            {
                                clase_n.funciones.aux.variables.Buscar(nodo.ChildNodes[6].Token.Text);
                                valorV = clase_n.funciones.aux.variables.aux.GetValor();

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, false);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }

                            }
                            else
                            {
                                textBox2.Text += "No existe variable :\"" + nodo.ChildNodes[6].Token.Text + "\"";
                                errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " No existe la variable:\"" + nodo.ChildNodes[6].Token.Text + "\"@";
                            }

                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            dimension = 0;
                            tipoV = Actuar(nodo.ChildNodes[2]);
                            nombreV = Actuar(nodo.ChildNodes[4]);
                            string[] nombres = nombreV.Split(',');

                            string dimeniones = Actuar(nodo.ChildNodes[5]);
                            clase_n.funciones.Existe(fun_actual);

                            if (clase_n.variables.Buscar_existe(nodo.ChildNodes[7].Token.Text))
                            {
                                clase_n.variables.Buscar(nodo.ChildNodes[7].Token.Text);

                                valorV = clase_n.variables.aux.GetValor();

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, true);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            nuevo_f.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, true);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        nuevo_f.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }



                            }
                            else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.ChildNodes[7].Token.Text))
                            {
                                clase_n.funciones.aux.variables.Buscar(nodo.ChildNodes[7].Token.Text);
                                valorV = clase_n.funciones.aux.variables.aux.GetValor();

                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, true);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, true);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }

                            }
                            else
                            {
                                textBox2.Text += "No existe variable :\"" + nodo.ChildNodes[7].Token.Text + "\"";
                                errores += nodo.ChildNodes[7].Token.Location.Line + ";" + nodo.ChildNodes[7].Token.Location.Column + ";" + "semantico" + ";" + " No existe la variable:\"" + nodo.ChildNodes[7].Token.Text + "\"@";
                            }

                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            if (nodo.ChildNodes[6].Token.Text.Equals("{"))
                            {
                                if (nodo.ChildNodes[7].Term.Name.ToString().Equals("AsignacionesArreglo"))
                                {
                                    dimension = 0;
                                    tipoV = Actuar(nodo.ChildNodes[1]);
                                    nombreV = Actuar(nodo.ChildNodes[3]);
                                    string[] nombres = nombreV.Split(',');

                                    string dimeniones = Actuar(nodo.ChildNodes[4]);

                                    valorV = Actuar(nodo.ChildNodes[7]);


                                    if (nombres.Length > 1)
                                    {
                                        for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                        {
                                            Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, false);

                                            if (globales)
                                            {
                                                clase_n.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";

                                            }
                                            else
                                            {
                                                clase_n.funciones.Existe(fun_actual);
                                                clase_n.funciones.aux.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }


                                }
                                else if (nodo.ChildNodes[7].Term.Name.ToString().Equals("AsignacionArreglo"))
                                {
                                    dimension = 0;
                                    tipoV = Actuar(nodo.ChildNodes[1]);
                                    nombreV = Actuar(nodo.ChildNodes[3]);
                                    string[] nombres = nombreV.Split(',');

                                    string dimeniones = Actuar(nodo.ChildNodes[4]);

                                    valorV= Actuar(nodo.ChildNodes[7]);


                                    if (nombres.Length > 1)
                                    {
                                        for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                        {
                                            Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, false);

                                            if (globales)
                                            {
                                                clase_n.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                            }
                                            else
                                            {
                                                clase_n.funciones.Existe(fun_actual);
                                                clase_n.funciones.aux.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }

                                }

                            }
                            else
                            {
                                dimension = 0;
                                tipoV = Actuar(nodo.ChildNodes[1]);
                                nombreV = Actuar(nodo.ChildNodes[3]);
                                string[] nombres = nombreV.Split(',');

                                string dimeniones = Actuar(nodo.ChildNodes[4]);

                                string funcion = nodo.ChildNodes[6].Token.Text;

                                if (clase_n.funciones.ExisteF(funcion))
                                {
                                    Funcion aux = clase_n.funciones.Existe(funcion);
                                    string temp=fun_actual;
                                    fun_actual = aux.GetNombre();
                                    Actuar(aux.nodo);
                                    fun_actual = temp;
                                    if (aux.GetTipo().Equals(tipoV))
                                    {
                                        valorV = aux.GetRetorno();


                                        if (nombres.Length > 1)
                                        {
                                            for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                            {
                                                Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, false);

                                                if (globales)
                                                {
                                                    clase_n.variables.Insertar(nuevo);
                                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                                }
                                                else
                                                {
                                                    clase_n.funciones.Existe(fun_actual);
                                                    clase_n.funciones.aux.variables.Insertar(nuevo);
                                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);

                                            if (globales)
                                            {
                                                clase_n.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                            }
                                            else
                                            {
                                                clase_n.funciones.Existe(fun_actual);
                                                clase_n.funciones.aux.variables.Insertar(nuevo);
                                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        textBox2.Text += "Tipos no Compatibles";
                                        errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + "tipos no compatibles @";
                                    }
                                }
                                else
                                {
                                    textBox2.Text += "No existe la funcion :\"" + nodo.ChildNodes[6].Token.Text + "\"";
                                    errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " No existe la funcion:\"" + nodo.ChildNodes[6].Token.Text + "\"@";
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 11)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Conservar"))
                            {
                                dimension = 0;
                                tipoV = Actuar(nodo.ChildNodes[1]);
                                nombreV = Actuar(nodo.ChildNodes[3]);
                                string[] nombres = nombreV.Split(',');

                                string dimeniones = Actuar(nodo.ChildNodes[4]);

                                valorV = Actuar(nodo.ChildNodes[7]);


                                if (nombres.Length > 1)
                                {
                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                    {
                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, true);

                                        if (globales)
                                        {
                                            clase_n.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                        }
                                        else
                                        {
                                            clase_n.funciones.Existe(fun_actual);
                                            clase_n.funciones.aux.variables.Insertar(nuevo);
                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                        }
                                    }
                                }
                                else
                                {
                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, true);

                                    if (globales)
                                    {
                                        clase_n.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                    }
                                    else
                                    {
                                        clase_n.funciones.Existe(fun_actual);
                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                    }
                                }
                            }
                            else
                            {
                                dimension = 0;
                                tipoV = Actuar(nodo.ChildNodes[1]);
                                nombreV = Actuar(nodo.ChildNodes[3]);
                                string[] nombres = nombreV.Split(',');
                                

                                string dimeniones = Actuar(nodo.ChildNodes[4]);

                                string valoresp= Actuar(nodo.ChildNodes[7]);

                                string funcion = nodo.ChildNodes[6].Token.Text;

                                if (clase_n.funciones.ExisteF(funcion))
                                {
                                    Funcion aux = clase_n.funciones.Existe(funcion);

                                    if (aux.GetTipo().Equals(tipoV))
                                    {
                                        if (aux.TieneParametros())
                                        {

                                            string[] cantparametros = funcion.Split(',');

                                            if(cantparametros.Length==aux.nParametros)
                                            {
                                                valorV = aux.GetRetorno();
                                                if (nombres.Length > 1)
                                                {
                                                    for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                                    {
                                                        Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, false);

                                                        if (globales)
                                                        {
                                                            clase_n.variables.Insertar(nuevo);
                                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                                        }
                                                        else
                                                        {
                                                            nuevo_f.variables.Insertar(nuevo);
                                                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + nuevo_f.GetNombre() + ";";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, false);

                                                    if (globales)
                                                    {
                                                        clase_n.variables.Insertar(nuevo);
                                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                                    }
                                                    else
                                                    {
                                                        nuevo_f.variables.Insertar(nuevo);
                                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + nuevo_f.GetNombre() + ";";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                textBox2.Text += "Cantidad de Parametros errornea en  funcion :\"" + funcion + "\"";
                                                errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + "cantidad de parametros erronea @";
                                            }    


                                            
                                        }
                                        else
                                        {
                                            textBox2.Text += "La funcion :\"" + funcion + "\" no lleva parametros";
                                            errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " la funcion:\"" + funcion + "\" no lleva parametros@";
                                        }

                                        

                                    }
                                    else
                                    {
                                        textBox2.Text += "Tipos no Compatibles";
                                        errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + "tipos no compatibles@";
                                    }
                                }
                                else
                                {
                                    textBox2.Text += "No existe la funcion :\"" + nodo.ChildNodes[6].Token.Text + "\"";
                                    errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " la funcion:\"" + nodo.ChildNodes[6].Token.Text + "\" no lleva parametros@";
                                }


                            }
                        }
                        else if (nodo.ChildNodes.Count == 12)
                        {
                            dimension = 0;
                            tipoV = Actuar(nodo.ChildNodes[2]);
                            nombreV = Actuar(nodo.ChildNodes[4]);
                            string[] nombres = nombreV.Split(',');


                            string dimeniones = Actuar(nodo.ChildNodes[5]);

                            string valoresp = Actuar(nodo.ChildNodes[9]);

                            string funcion = nodo.ChildNodes[7].Token.Text;

                            if (clase_n.funciones.ExisteF(funcion))
                            {
                                Funcion aux = clase_n.funciones.Existe(funcion);

                                if (aux.GetTipo().Equals(tipoV))
                                {
                                    if (aux.TieneParametros())
                                    {

                                        string[] cantparametros = funcion.Split(',');

                                        if (cantparametros.Length == aux.nParametros)
                                        {
                                            valorV = aux.GetRetorno();
                                            if (nombres.Length > 1)
                                            {
                                                for (int cuenta = 0; cuenta < nombres.Length; cuenta++)
                                                {
                                                    Variable nuevo = new Variable(tipoV, nombres[cuenta], valorV, dimension, dimeniones, true);
                                                    
                                                    if (globales)
                                                    {
                                                        clase_n.variables.Insertar(nuevo);
                                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                                    }
                                                    else
                                                    {
                                                        clase_n.funciones.Existe(fun_actual);
                                                        clase_n.funciones.aux.variables.Insertar(nuevo);
                                                        componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Variable nuevo = new Variable(tipoV, nombres[0], valorV, dimension, dimeniones, true);

                                                if (globales)
                                                {
                                                    clase_n.variables.Insertar(nuevo);
                                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.GetNombre() + ";";
                                                }
                                                else
                                                {
                                                    clase_n.funciones.Existe(fun_actual);
                                                    clase_n.funciones.aux.variables.Insertar(nuevo);
                                                    componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "metodo" + "," + clase_n.funciones.aux.GetNombre() + ";";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            textBox2.Text += "Cantidad de Parametros errornea en  funcion :\"" + funcion + "\"";
                                            errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " Cantidad de Parametros errornea en  funcion:\"" + funcion + "\" @";
                                        }



                                    }
                                    else
                                    {
                                        textBox2.Text += "La funcion :\"" + funcion + "\" no lleva parametros";
                                        errores += nodo.ChildNodes[6].Token.Location.Line + ";" + nodo.ChildNodes[6].Token.Location.Column + ";" + "semantico" + ";" + " La funcion:\"" + funcion + "\"no lleva parametros @";
                                    }



                                }
                                else
                                {
                                    textBox2.Text += "Tipos no Compatibles";

                                }
                            }
                            else
                            {
                                textBox2.Text += "No existe la funcion :\"" + nodo.ChildNodes[6].Token.Text + "\"";
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

                case "Operaciones":
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

                case "Funciones":
                        {

                        string funcion;

                        funcion= nodo.ChildNodes[0].Token.Value.ToString();
                        string temp = "Principal";
                        fun_actual=funcion;
                        if (clase_n.funciones.ExisteF(funcion))
                        {
                            clase_n.funciones.Existe(funcion);

                            if (nodo.ChildNodes.Count == 4)
                            {
                                Actuar(clase_n.funciones.aux.nodo);
                            }
                            else
                            {
                                fun_actual = temp;
                                string param = Actuar(nodo.ChildNodes[2]);

                                string[] variables = param.Split(',');

                                fun_actual = funcion;
                                clase_n.funciones.Existe(funcion);
                                for (int x = 0; x < variables.Length; x++)
                                {
                                    string n = clase_n.funciones.aux.parametros.GetNOmbreP(x + 1);

                                    clase_n.funciones.aux.variables.Buscar(n);

                                    clase_n.funciones.aux.variables.aux.SetValor(variables[x]);
                                    
                                }

                                Actuar(clase_n.funciones.aux.nodo);

                            }
                        }
                        else
                        {
                            textBox2.Text += "No existe Funcion :\"" + nodo.ChildNodes[0].Token.Text + "\"";
                        }

                        break;
                    }

                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {

                            if (nodo.ChildNodes[0].Term.Name!= "Operacion")
                            {
                                resultado = Actuar(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                resultado = Convert.ToString(Aritmeticas(nodo));
                            }

                                
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            clase_n.funciones.Existe(fun_actual);

                            if (clase_n.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                            {
                                clase_n.variables.Buscar(nodo.ChildNodes[0].Token.Text);

                                if (clase_n.variables.aux.IsArreglo())
                                {
                                    Double lugar= Aritmeticas(nodo.ChildNodes[2]);

                                    resultado = clase_n.variables.aux.GetValor_Arr(0, Convert.ToInt32(lugar));
                                }
                                else
                                {
                                    textBox2.Text += "La variable :\"" + nodo.ChildNodes[0].Token.Text + "\" no es Arreglo";
                                }

                            }
                            else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                            {
                                clase_n.funciones.aux.variables.Buscar(nodo.ChildNodes[0].Token.Text);
                                if (clase_n.funciones.aux.variables.aux.IsArreglo())
                                {
                                    Double lugar = Aritmeticas(nodo.ChildNodes[2]);

                                    resultado = clase_n.funciones.aux.variables.aux.GetValor_Arr(0, Convert.ToInt32(lugar));
                                }
                                else
                                {
                                    textBox2.Text += "La variable :\"" + nodo.ChildNodes[0].Token.Text + "\" no es Arreglo";
                                }
                            }
                            else
                            {
                                textBox2.Text += "No existe variable :\"" + nodo.ChildNodes[0].Token.Text + "\"";
                            }
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
                                else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                                {


                                    clase_n.funciones.aux.variables.Buscar(nodo.ChildNodes[0].Token.Text);


                                    resultado = clase_n.funciones.aux.variables.aux.GetValor();
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

                        Variable nuevo = new Variable(tipo, nombre);
                        nuevo_P = new Parametro(tipo, nombre);

                        if (parametro_funcion)
                        {
                            nuevo_f.parametros.Insertar(nuevo_P);
                            nuevo_f.AumentarParametros();
                            nuevo_f.variables.Insertar(nuevo);
                            componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "Parametro" + "," + nuevo_f.GetNombre() + ";";
                        }

                        break;
                    }

                case "Dimensiones":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado += Actuar(nodo.ChildNodes[0])+",";
                            resultado += Actuar(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado += Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Dimension":
                    {
                        dimension++;
                        resultado = Convert.ToString(Aritmeticas(nodo.ChildNodes[1]));

                        break;
                    }

                case "AsignacionArreglo":
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

                case "AsignacionesArreglo":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            resultado += Actuar(nodo.ChildNodes[0]) + ";";
                            resultado += Actuar(nodo.ChildNodes[3]);
                        }
                        else
                        {
                            resultado += Actuar(nodo.ChildNodes[1]);

                        }


                        break;
                    }

                case "Si":
                    {
                        string logica;
                        bool hacer = false;
                        logica = Actuar(nodo.ChildNodes[1]);

                        if (logica.Equals("true"))
                        {
                            hacer = true;
                        }
                        else
                        {
                            hacer = false;
                        }

                        if (nodo.ChildNodes.Count == 6)
                        {
                            if (hacer)
                            {
                                resultado = Actuar(nodo.ChildNodes[4]);
                            }
                        }
                        else
                        {
                            if (hacer)
                            {
                                resultado = Actuar(nodo.ChildNodes[4]);
                            }
                            else
                            {
                                resultado = Actuar(nodo.ChildNodes[7]);
                            }

                        }


                        break;
                    }


                case "Condicion":
                    {
                        resultado = Actuar(nodo.ChildNodes[0]);
                        break;
                    }

                case "Logica":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if(nodo.ChildNodes[0].Term.Name != "Logica")
                            {
                                resultado = Actuar(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                string operador1 = Actuar(nodo.ChildNodes[0]);
                                string operador2 = Actuar(nodo.ChildNodes[2]);

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "or")
                                {
                                    if(operador1.Equals("true") || operador2.Equals("true"))
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "and")
                                {
                                    if (operador1.Equals("true") && operador2.Equals("true"))
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "Nand")
                                {
                                    if (operador1.Equals("true") && operador2.Equals("true"))
                                    {
                                        resultado = "false";
                                    }
                                    else
                                    {
                                        resultado = "true";
                                    }
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "Nor")
                                {
                                    if (operador1.Equals("false") && operador2.Equals("false"))
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "xor")
                                {
                                    if (operador1.Equals("false") || operador2.Equals("false"))
                                    {
                                        resultado = "false";
                                    }
                                    else if (operador1.Equals("true") || operador2.Equals("true"))
                                    {
                                        resultado = "false";
                                    }
                                    else
                                    {
                                        resultado = "true";
                                    }
                                }
                                
                            }
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            string operador1 = Actuar(nodo.ChildNodes[1]);

                            if (operador1.Equals("false"))
                            {
                                resultado = "true";
                            }
                            else
                            {
                                resultado = "false";
                            }
                        }
                        else
                        {
                            resultado = Actuar(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Relacional":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name!= "Relacional")
                            {
                                resultado = Actuar(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                double operador1 = Convert.ToDouble(Actuar(nodo.ChildNodes[0]));
                                double operador2 = Convert.ToDouble(Actuar(nodo.ChildNodes[2]));

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "igual")
                                {
                                    if (operador1==operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "diferente")
                                {
                                    if (operador1 != operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor")
                                {
                                    if (operador1 < operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor")
                                {
                                    if (operador1 > operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor_que")
                                {
                                    if (operador1 <= operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor_que")
                                {
                                    if (operador1 > operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                            }
                        }
                        else
                        {
                            resultado = Actuar(nodo.ChildNodes[0]);

                        }
                        break;
                    }

                case "Para":
                    {
                        string nombre;

                        if (nodo.ChildNodes.Count == 13)
                        {
                            nombre=nodo.ChildNodes[1].Token.Text;

                            if (clase_n.variables.Buscar_existe(nombre))
                            {
                                Variable aux=clase_n.variables.Buscar(nombre);

                                aux.SetValor(Convert.ToString(Aritmeticas(nodo.ChildNodes[3])));
                                double control = Aritmeticas(nodo.ChildNodes[3]);
                                string logica_inicial = "true";

                                logica_inicial= Actuar(nodo.ChildNodes[5]);

                                while (logica_inicial.Equals("true"))
                                {
                                    Actuar(nodo.ChildNodes[11]);

                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    logica_inicial = Actuar(nodo.ChildNodes[5]);
                                    aux.SetValor(Convert.ToString(control));
                                }

                            }
                            else if (nuevo_f.variables.Buscar_existe(nombre))
                            {
                                Variable aux = nuevo_f.variables.Buscar(nombre);

                                aux.SetValor(Convert.ToString(Aritmeticas(nodo.ChildNodes[3])));
                                double control = Aritmeticas(nodo.ChildNodes[3]);
                                string logica_inicial = "true";

                                logica_inicial = Actuar(nodo.ChildNodes[5]);

                                while (logica_inicial.Equals("true"))
                                {
                                    Actuar(nodo.ChildNodes[11]);

                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    logica_inicial = Actuar(nodo.ChildNodes[5]);
                                    aux.SetValor(Convert.ToString(control));
                                }
                            }
                            else
                            {
                                textBox2.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") No existe variable :\"" + nombre + "\"";
                            }

                        }
                        else if (nodo.ChildNodes.Count == 15)
                        {
                            nombre = nodo.ChildNodes[3].Token.Text;

                            if (clase_n.variables.Buscar_existe(nombre))
                            {
                                textBox2.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                            }
                            else if (nuevo_f.variables.Buscar_existe(nombre))
                            {
                                textBox2.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                            }
                            else
                            {
                                string tipo = Actuar(nodo.ChildNodes[2]);
                                Variable nuevo = new Variable(tipo, nombre);

                                nuevo_f.variables.Insertar(nuevo);
                                componentes += nuevo.GetNombre() + "," + nuevo.GetTipo() + "," + "Variable" + "," + nuevo_f.GetNombre() + ";";

                                double control = Aritmeticas(nodo.ChildNodes[5]);
                                nuevo.SetValor(Convert.ToString(control));

                                string logica_inicial = "true";

                                logica_inicial = Actuar(nodo.ChildNodes[7]);

                                while (logica_inicial.Equals("true"))
                                {
                                    Actuar(nodo.ChildNodes[13]);

                                    if (nodo.ChildNodes[10].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[10].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    nuevo.SetValor(Convert.ToString(control));
                                    logica_inicial = Actuar(nodo.ChildNodes[7]);
                                    
                                }

                            }

                        }
                        break;
                    }


                case "Mientras":
                    {

                        string condicion = Actuar(nodo.ChildNodes[1]);

                        while (condicion.Equals("true"))
                        {
                            Actuar(nodo.ChildNodes[4]);

                            condicion = Actuar(nodo.ChildNodes[1]);
                        }
                        break;
                    }

                case "Hacer":
                    {
                        Actuar(nodo.ChildNodes[2]);

                        string condicion = Actuar(nodo.ChildNodes[6]);

                        while (condicion.Equals("true"))
                        {
                            Actuar(nodo.ChildNodes[2]);
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
                            else
                            {

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

                case "ID":
                    {
                        clase_n.funciones.Existe(fun_actual);
                        

                        if (clase_n.variables.Buscar_existe(nodo.Token.Text))
                        {
                            clase_n.variables.Buscar(nodo.Token.Text);


                            resultado = Convert.ToDouble( clase_n.variables.aux.GetValor());

                        }
                        else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.Token.Text))
                        {
                            clase_n.funciones.aux.variables.Buscar(nodo.Token.Text);
                            resultado = Convert.ToDouble( clase_n.funciones.aux.variables.aux.GetValor());
                        }
                        else
                        {
                            textBox2.Text += "No existe variable :\"" + nodo.Token.Text + "\"";
                        }

                        break;
                    }

                

                


            }


            return resultado;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select a Cursor File";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.
           

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog1.FileName);
                string archivo=sr.ReadToEnd();
                sr.Close();
                textBox1.Text = archivo;
            }

            
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string codigo = textBox1.Text;
            SaveFileDialog saveFileDialog1;
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Guardar Archivo de Texto";
            

            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.RestoreDirectory = true;


            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ruta = saveFileDialog1.FileName;

                FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.Write);

                StreamWriter fichero = new StreamWriter(fs);
                fichero.Write(codigo);
                fichero.Close();
                fs.Close();
                MessageBox.Show("Se guardo el archivo: " + saveFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show("Has cancelado.");
            }
            saveFileDialog1.Dispose();
            saveFileDialog1 = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clase_n.funciones.Existe("Principal");
            fun_actual = "Principal";
            string respuesta = Actuar(clase_n.funciones.aux.nodo);

            textBox2.Text += respuesta;
            Reporte_Componentes();
            Reporte_Errores();
        }

        void Heredar(Clase nueva,Clase Padre)
        {
            if (Padre != null)
            {


                Padre.funciones.SeekCabeza();

                bool seguir = true;

                while (seguir)
                {
                    if (Padre.funciones.aux.GetNombre() != "Principal")
                    {
                        nueva.funciones.Insertar(Padre.funciones.aux);
                    }

                    if (Padre.funciones.aux.siguiente != null)
                    {
                        Padre.funciones.aux = Padre.funciones.aux.siguiente;
                    }
                    else
                    {
                        seguir = false;
                    }
                }

            }
        }

        public void Reporte_Componentes()
        {
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("dd-MM-yyyy");

            string html= "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> \n";
            html += "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n";
            html += "<head >\n";
            html += "<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\" />\n";
            html += "<title>Simbolos</title>\n";
            html += "</head >\n";

            html +="<body >\n";
            html += "<h1>Lista de Simbolos</h1>\n";
            html += "<p>Dia de Ejecucion "+ fecha_actual+"</p>\n";
            html += "<p>Hora de Ejecucion "+ DateTime.Now.ToShortTimeString()+"</p>\n";
            html += "<table width=\"500\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n";
            html += " <tr>\n";
            html += "<th scope=\"col\">ID</th>\n";
            html += "<th scope=\"col\">TIPO</th>\n";
            html += "<th scope=\"col\">Rol</th>\n";
            html += "<th scope=\"col\">Ambito</th>\n";
            html += " </tr>\n";

            string[] filas = componentes.Split(';');


            for(int x = 0; x < filas.Length; x++)
            {
                if (filas[x] != "")
                {
                    string[] dato = filas[x].Split(',');
                    html += " <tr>\n";

                    html += "<th>" + dato[0] + "</th>\n";
                    html += "<th>" + dato[1] + "</th>\n";
                    html += "<th>" + dato[2] + "</th>\n";
                    html += "<th>" + dato[3] + "</th>\n";
                    html += " </tr>\n";
                }
                

            }
            html += "</table>\n";
            html += "</body>\n";
            html += "</html>\n";



            FileStream fs = new FileStream("C:\\Fuentes\\Reporte_Simbolos.html", FileMode.Create, FileAccess.Write);

            StreamWriter fichero = new StreamWriter(fs);
            fichero.Write(html);
            fichero.Close();
            fs.Close();


            //html += "\n";


        }

        public void Reporte_Errores()
        {
            DateTime Hoy = DateTime.Today;
            string fecha_actual = Hoy.ToString("dd-MM-yyyy");

            string html = "<!DOCTYPE html PUBLIC \" -//W3C//DTD XHTML 1.0 Transitional//EN\" http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"> \n";
            html += "<html xmlns=\"http://www.w3.org/1999/xhtml\">\n";
            html += "<head >\n";
            html += "<meta http-equiv=\"Content - Type\" content=\"text / html; charset = utf - 8\" />\n";
            html += "<title>Simbolos</title>\n";
            html += "</head >\n";

            html += "<body >\n";
            html += "<h1>Lista de Simbolos</h1>\n";
            html += "<p>Dia de Ejecucion " + fecha_actual + "</p>\n";
            html += "<p>Hora de Ejecucion " + DateTime.Now.ToShortTimeString() + "</p>\n";
            html += "<table width=\"500\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\">\n";
            html += " <tr>\n";
            html += "<th scope=\"col\">Linea</th>\n";
            html += "<th scope=\"col\">Columna</th>\n";
            html += "<th scope=\"col\">Tipo</th>\n";
            html += "<th scope=\"col\">Detalles</th>\n";
            html += " </tr>\n";

            string[] filas = errores.Split('@');


            for (int x = 0; x < filas.Length; x++)
            {
                if (filas[x] != "")
                {
                    string[] dato = filas[x].Split(';');
                    html += " <tr>\n";

                    html += "<th>" + dato[0] + "</th>\n";
                    html += "<th>" + dato[1] + "</th>\n";
                    html += "<th>" + dato[2] + "</th>\n";
                    if (dato.Length == 4)
                    {
                        html += "<th>" + dato[3] + "</th>\n";
                    }
                    
                    
                    html += " </tr>\n";
                }


            }
            html += "</table>\n";
            html += "</body>\n";
            html += "</html>\n";



            FileStream fs = new FileStream("C:\\Fuentes\\Reporte_Errores.html", FileMode.Create, FileAccess.Write);

            StreamWriter fichero = new StreamWriter(fs);
            fichero.Write(html);
            fichero.Close();
            fs.Close();


            //html += "\n";


        }
    }
}
