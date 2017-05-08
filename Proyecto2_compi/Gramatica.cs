using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_compi
{
    class Gramatica:Grammar
    {

        public Gramatica():base(true)
        {
            CommentTerminal comentarioSimple = new CommentTerminal("comentarioSimple", ">>", "\n", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "<-", "->");

            base.NonGrammarTerminals.Add(comentarioMulti);
            base.NonGrammarTerminals.Add(comentarioSimple);
            

            RegexBasedTerminal iniCuerpo = new RegexBasedTerminal("iniCuerpo", "¿");
            RegexBasedTerminal finCuerpo = new RegexBasedTerminal("finCuerpo", "\\?");
            RegexBasedTerminal finSentencia = new RegexBasedTerminal("finSentencia", "\\$");

            //Reservadas
            
            RegexBasedTerminal Lienzo = new RegexBasedTerminal("lienzo", "Lienzo");
            RegexBasedTerminal Conservar = new RegexBasedTerminal("Conservar", "Conservar");
            RegexBasedTerminal Extender = new RegexBasedTerminal("Extender", "extiende");
            RegexBasedTerminal Pintar_OR = new RegexBasedTerminal("RPintar_OR", "Pintar_OR\\(");
            RegexBasedTerminal Pintar_P = new RegexBasedTerminal("RPintar_P", "Pintar_P\\(");
            RegexBasedTerminal Rretorna = new RegexBasedTerminal("Rretorna", "retorna");
            RegexBasedTerminal Rsi = new RegexBasedTerminal("Rsi", "si\\(");
            RegexBasedTerminal Rsino = new RegexBasedTerminal("Rsino", "sino¿");
            RegexBasedTerminal Rpara = new RegexBasedTerminal("Rpara", "para\\(");
            RegexBasedTerminal Rmientras = new RegexBasedTerminal("Rmientras", "mientras\\(");
            RegexBasedTerminal Rhacer = new RegexBasedTerminal("Rhacer", "hacer");
            RegexBasedTerminal Rvar = new RegexBasedTerminal("Rvar", "var ");
            RegexBasedTerminal Rarreglo = new RegexBasedTerminal("Rarreglo", "arreglo ");







            //Visibilidad 
            RegexBasedTerminal publico = new RegexBasedTerminal("publico", "publico ");
            RegexBasedTerminal privado = new RegexBasedTerminal("privado", "privado ");




            IdentifierTerminal ID = new IdentifierTerminal("ID");

            //Tipo de Datos
            RegexBasedTerminal REntero = new RegexBasedTerminal("REntero", "entero");
            NumberLiteral Entero = new NumberLiteral("entero");

            RegexBasedTerminal Rvoid = new RegexBasedTerminal("Rvoid", "void");

            RegexBasedTerminal RDoble = new RegexBasedTerminal("RDoble", "doble");
            RegexBasedTerminal Doble = new RegexBasedTerminal("Doble", "[0-9]+\\.[0-9]{6}");

            RegexBasedTerminal Rboolean = new RegexBasedTerminal("Rboolean", "boolean");
            RegexBasedTerminal Verdadero = new RegexBasedTerminal("verdadero", "verdadero|true");
            RegexBasedTerminal Falso = new RegexBasedTerminal("falso", "falso|false");

            RegexBasedTerminal RCaracter = new RegexBasedTerminal("RCaracter", "caracter");
            RegexBasedTerminal Caracter = new RegexBasedTerminal("Caracter", "\'([a-zA-Z0-9]|#(n|f|t)|#|\\[|\\])\'");

            RegexBasedTerminal RCadena = new RegexBasedTerminal("RCadena", "cadena");
            RegexBasedTerminal Cadena = new RegexBasedTerminal("Cadena", "\\\"([a-zA-Z0-9]|#\\\"|#|\\[|\\])+\\\"");


            //Operadores


            //Relaciones
            RegexBasedTerminal Igual = new RegexBasedTerminal("igual", "==");
            RegexBasedTerminal Diferente = new RegexBasedTerminal("diferente", "!=");
            RegexBasedTerminal Menor = new RegexBasedTerminal("menor", "<");
            RegexBasedTerminal Mayor = new RegexBasedTerminal("mayor", ">");
            RegexBasedTerminal MenorQue = new RegexBasedTerminal("menor_que", "<=");
            RegexBasedTerminal MayorQue = new RegexBasedTerminal("mayor_que", ">=");

            //Logicos
            RegexBasedTerminal Or = new RegexBasedTerminal("or", "\\|\\|");
            RegexBasedTerminal and = new RegexBasedTerminal("and", "&&");
            RegexBasedTerminal Nand = new RegexBasedTerminal("Nand", "!&&");
            RegexBasedTerminal Nor = new RegexBasedTerminal("Nor", "!\\|\\|");
            RegexBasedTerminal Xor = new RegexBasedTerminal("xor", "&\\|");
            RegexBasedTerminal not = new RegexBasedTerminal("not", "!");

          

            //Artimeticos
            RegexBasedTerminal suma = new RegexBasedTerminal("suma", "\\+");
            RegexBasedTerminal resta = new RegexBasedTerminal("resta", "-");
            RegexBasedTerminal multiplicacion = new RegexBasedTerminal("multi", "\\*");
            RegexBasedTerminal division = new RegexBasedTerminal("div", "\\/");
            RegexBasedTerminal potencia = new RegexBasedTerminal("power", "\\^");

            RegexBasedTerminal aumentar = new RegexBasedTerminal("aumentar", "\\+\\+");
            RegexBasedTerminal disminuir = new RegexBasedTerminal("disminuir", "--");


            this.RegisterOperators(0, suma, resta);
            this.RegisterOperators(1, division, multiplicacion);
            this.RegisterOperators(2, Menor, Mayor,MenorQue,MayorQue,Igual,Diferente);       
            this.RegisterOperators(3, Associativity.Right, not);
            this.RegisterOperators(4, Associativity.Left, and, Nand);
            this.RegisterOperators(5, Associativity.Left, Or, Nor, Xor);




            NonTerminal S = new NonTerminal("S"),
                        Visibilidad = new NonTerminal("Visibilidad"),
                        Extensiones = new NonTerminal("Extensiones"),
                        Extension = new NonTerminal("Extension"),
                        Cabeza = new NonTerminal("Cabeza"),
                        Tipo = new NonTerminal("Tipo"),
                        Componentes = new NonTerminal("Componentes"),
                        Componente = new NonTerminal("Componente"),
                        Sentencias = new NonTerminal("Sentencias"),
                        Parametros = new NonTerminal("Parametros"),
                        Parametro = new NonTerminal("Parametro"),
                        ParametrosV = new NonTerminal("ParamterosV"),
                        Sentencia = new NonTerminal("Sentencia"),
                        Declaracion = new NonTerminal("Declaracion"),
                        Asignacion = new NonTerminal("Asignacion"),
                        AsignacionesArreglo = new NonTerminal("AsignacionesArreglo"),
                        AsignacionArreglo = new NonTerminal("AsignacionArreglo"),
                        Nombres = new NonTerminal("Nombres"),
                        Nombre = new NonTerminal("Nombre"),
                        Dimensiones = new NonTerminal("Dimensiones"),
                        Dimension = new NonTerminal("Dimension"),
                        Valor = new NonTerminal("Valor"),
                        Operacion = new NonTerminal("Operacion"),
                        Operaciones = new NonTerminal("Operaciones"),
                        Logica = new NonTerminal("Logica"),
                        Relacional = new NonTerminal("Relacional"),
                        Funciones = new NonTerminal("Funciones"),
                        Si = new NonTerminal("Si"),
                        Para = new NonTerminal("Para"),
                        Mientras = new NonTerminal("Mientras"),
                        Pintar = new NonTerminal("Pintar"),
                        Punto = new NonTerminal("Punto"),
                        Ovalo_R = new NonTerminal("Ovalo_R"),
                        Retorno = new NonTerminal("Retorno"),
                        Hacer = new NonTerminal("Hacer"),   
                        Condicion = new NonTerminal("Condicion"),
                        Cuerpo = new NonTerminal("Cuerpo");


            //1
            S.Rule = Cabeza + Cuerpo;

            //2
            Cabeza.Rule=Visibilidad + Lienzo + ID + Extensiones
                        | Visibilidad + Lienzo + ID
                        | Conservar + Lienzo + ID
                        | Visibilidad + Conservar + Lienzo + ID
                        | Visibilidad + Conservar + Lienzo + ID + Extensiones;

            //4
            Cuerpo.Rule = iniCuerpo + Componentes + finCuerpo;
                        

            //5
            Componentes.Rule = Componentes + Componente
                            | Componente;

            //7
            Componente.Rule = Conservar + ID +"("+ Parametros + ")"+iniCuerpo + Sentencias + finCuerpo //8
                            | Conservar + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo //7
                            | ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //7
                            | ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//6
                            | Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //8
                            | Tipo + ID + "("  + ")" + iniCuerpo + Sentencias + finCuerpo //7
                            | Conservar + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //9
                            | Conservar + Tipo + ID + "(" +")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Tipo + "[]" + ID + "("  + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Conservar + Tipo +"[]"+ ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Conservar + Tipo + "[]" + ID + "("  + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Visibilidad + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Visibilidad + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//7
                            | Visibilidad + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Visibilidad + Tipo + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Visibilidad + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Visibilidad + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Visibilidad + Conservar + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//11
                            | Visibilidad + Conservar + Tipo + "[]" + ID + "("  + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Sentencias;//1

            Componente.ErrorRule = SyntaxError + finCuerpo;

            //13
            Parametros.Rule = Parametros + "," + Parametro
                            | Parametro;
                           

            ParametrosV.Rule = Operacion;
            //14
            Parametro.Rule = Tipo + ID;   

            //15
            Sentencias.Rule = Sentencias + Sentencia
                            | Sentencia;

            //17
            Sentencia.Rule = Retorno
                            | Asignacion
                            | Declaracion
                            | Pintar
                            | Funciones
                            | Si
                            | Para
                            | Mientras
                            | Hacer;


            //24
            Declaracion.Rule = Conservar +Rvar+ Tipo + Nombres + "=" + Operacion + finSentencia//7
                            | Conservar +Rvar+ Tipo + Nombres + finSentencia//5
                            | Rvar+ Tipo + Nombres + finSentencia//4
                            | Rvar+ Tipo + Nombres + "=" + Operacion + finSentencia//6
                            | Conservar +Rvar+ Tipo + Rarreglo + Nombres + Dimensiones + finSentencia//7
                            | Rvar+ Tipo + Rarreglo + Nombres + Dimensiones + finSentencia//6
                            | Rvar+ Tipo + Rarreglo + Nombres + Dimensiones + "=" + "{" + AsignacionesArreglo + "}" + finSentencia//10
                            | Rvar+ Tipo + Rarreglo + Nombres + Dimensiones + "=" + "{" + AsignacionArreglo + "}" + finSentencia//10
                            | Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + ID + finSentencia//8
                            | Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + ID +"("+ ")" + finSentencia//10
                            | Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + ID + "(" + Operaciones + ")" + finSentencia//11
                            | Conservar + Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + "{" + AsignacionesArreglo + " }" + finSentencia//11
                            | Conservar + Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + "{" + AsignacionArreglo + "}" + finSentencia//11
                            | Conservar + Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" + ID + finSentencia//9
                            | Conservar + Rvar + Tipo + Rarreglo + Nombres + Dimensiones + "=" +ID + "(" + Operaciones + ")" + finSentencia;//12

            Declaracion.ErrorRule = SyntaxError + finSentencia;
            //31
            AsignacionesArreglo.Rule= AsignacionesArreglo +","+"{"+ AsignacionArreglo+"}" 
                                    | "{"+AsignacionArreglo+"}" ;

            //33
            AsignacionArreglo.Rule = AsignacionArreglo + "," + Operacion
                                    | Operacion;


            Funciones.Rule = ID + "(" + Operaciones  + ")" + finSentencia
                           | ID + "(" + ")" + finSentencia;

            Funciones.ErrorRule = SyntaxError + finSentencia;


            Operaciones.Rule = Operaciones + "," + Operacion
                            |Operacion;
            //35
            Asignacion.Rule = ID + "=" + Operacion + finSentencia//4
                            | ID + aumentar + finSentencia//3
                            | ID + disminuir + finSentencia//3
                            | ID + Dimension+ "="+ Operacion + finSentencia;//5

            Asignacion.ErrorRule = SyntaxError + finSentencia;
            //39
            Retorno.Rule = Rretorna + Operacion + finSentencia;

            Retorno.ErrorRule = SyntaxError + finSentencia;

            //40
            Pintar.Rule = Punto
                        | Ovalo_R;

            //42
            Punto.Rule = Pintar_P +Operacion+","+Operacion+","+Operacion+","+Operacion+")"+finSentencia;

            Punto.ErrorRule = SyntaxError + finSentencia;

            //43
            Ovalo_R.Rule = Pintar_OR+Operacion+","+Operacion+","+ Operacion + ","+Operacion+","+Operacion+","+ Operacion + ")"+ finSentencia;

            Ovalo_R.ErrorRule = SyntaxError + finSentencia;

            //44
            Si.Rule = Rsi+ Condicion + ")" + iniCuerpo + Sentencias + finCuerpo
                     | Rsi+ Condicion + ")" + iniCuerpo + Sentencias + finCuerpo +Rsino+ Sentencias+finCuerpo;

            Si.ErrorRule = SyntaxError + finCuerpo;

            //45
            Para.Rule = Rpara + ID +"=" +Operacion + ";"+Condicion+";"+ID + aumentar+")"+iniCuerpo + Sentencias + finCuerpo
                        | Rpara + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + Sentencias + finCuerpo
                        | Rpara + Rvar+ Tipo+ ID + "=" + Operacion + ";" + Condicion + ";" + ID + aumentar + ")" + iniCuerpo + Sentencias + finCuerpo
                        | Rpara + Rvar + Tipo + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + Sentencias + finCuerpo;

            Para.ErrorRule= SyntaxError + finCuerpo;
            
            //46
            Mientras.Rule = Rmientras+Condicion+")"+iniCuerpo+Sentencias+finCuerpo;

            Mientras.ErrorRule= SyntaxError + finCuerpo;
            //47
            Hacer.Rule = Rhacer +iniCuerpo+Sentencias+finCuerpo+Rmientras+"("+Condicion+")"+finSentencia;

            Hacer.ErrorRule= SyntaxError + finCuerpo;
            //48
            Condicion.Rule = Logica;


            //49
            Logica.Rule = Logica + Or + Logica
                        |Logica + and + Logica
                        |Logica + Nand + Logica
                        |Logica + Nor + Logica
                        |Logica + Xor + Logica
                        |not + Logica
                        |"("+ Logica +")"
                        |Relacional;

            //57
            Relacional.Rule =Relacional + Igual + Relacional
                            |Relacional + Diferente + Relacional
                            |Relacional + Menor + Relacional
                            |Relacional + MenorQue + Relacional
                            |Relacional + Mayor + Relacional
                            |Relacional + MayorQue + Relacional
                            |"("+Relacional+")"
                            |Operacion;

            //65
            Operacion.Rule = Operacion + suma + Operacion
                            | Operacion + resta + Operacion
                            | Operacion + division + Operacion
                            | Operacion + multiplicacion + Operacion
                            | Operacion + potencia + Operacion
                            | "(" + Operacion + ")"
                            | ID
                            | ID + "[" + Operacion + "]"
                            | Valor;

            //26            
            Dimensiones.Rule = Dimensiones + Dimension
                              |Dimension;

            //27
            Dimension.Rule = "[" + Operacion+"]";

            //28
            Nombres.Rule = Nombres + "," + Nombre
                         | Nombre;

            //29
            Nombre.Rule = ID;

            //30
            Visibilidad.Rule = publico
                            | privado;

            Extensiones.Rule = Extender + Extension;

            Extension.Rule = Extension + "," + ID
                            | ID;

            Tipo.Rule = REntero
                      |Rboolean
                      |RCadena
                      |RDoble
                      |RCaracter
                      |Rvoid;

            Valor.Rule = Entero
                 |Verdadero
                 |Falso
                 |Caracter
                 |Doble
                 |Cadena;

            this.Root = S;



        }





    }
}

