using System;
using System.Collections.Generic;
using System.IO;

namespace AnalizadorLexicosEstados.AnalizadorLexico
{
    public class AnalLex
    {

        private int numLinea = 1;
        private String lexema;
        private String[] reservadas = { "float", "write", "program", "until", "fi", "read", "bool", "if", "while", "do", "for", "class", "else", "true", "double", "final", "not", "and", "or", "String", "int" };
        private List<String> lstPalabrasReservadas;
        private char car;
        private int val;
        private StreamReader file;
        private TextWriter escribeLex;
        private TextWriter escribeSint;
        private List<String> colaEntrada;

        //Arbol sintactico
        private AnalizadorSintactico.Arbol ArbolSintactico = new AnalizadorSintactico.Arbol();
        private AnalizadorSintactico.NodoArbol raiz = new AnalizadorSintactico.NodoArbol();
        private AnalizadorSintactico.NodoArbol nodoActual = new AnalizadorSintactico.NodoArbol();

        public AnalLex(String nombreArchivo)
        {
            file = new StreamReader(nombreArchivo);
            escribeLex = new StreamWriter(@"C:\Users\Diana\Documents\NetBeansProjects\Sharky\resultadoLexico.txt");
            escribeSint = new StreamWriter(@"C:\Users\Diana\Documents\NetBeansProjects\Sharky\resultadoSintactico.txt");
            lstPalabrasReservadas = new List<string>();
            colaEntrada = new List<string>();


            foreach (var item in reservadas)
            {
                lstPalabrasReservadas.Add(item);
            }

            lexema = "";

            val = file.Read();
            car = (Char)val;

            q0();
            AnalSint();
            file.Close();
            escribeLex.Close();
            escribeSint.Close();

            System.Environment.Exit(1);
        }

        private void q0()
        {
            if (car == ' ' || car == '\n' || car == '\r' || car == '\t')
            {
                q6();
            }

            if (Char.IsNumber(car))
            {
                q1();
            }

            if (Char.IsLetter(car))
            {
                q4();
            }

            if (car == '"')
            {
                q7();
            }

            if (car == '$')
            {
                q11();
            }

            switch (car)
            {
                case '\r': break;
                case '=':
                case '+':
                case '.':
                case '-':
                case '*':
                case '/':
                case '&':
                case ';': q9(); break;
                case '!'://///diferente de
                case '|':////igual que
                case '^':////menor igual que
                case '°':////mayor igual que
                case '<':
                case '>':
                case '(':
                case ')':
                case ',':
                case '{':
                case '}': q10(); break;
                default: break;
            }

        }

        private void q1()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q1();
            }
            else if (car == '.')
            {
                q2();
            }
            else
            {
                Mostrar("NUMERO");
                q0();
            }
        }

        private void q2()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q3();
            }
        }

        private void q3()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car))
            {
                q3();
            }
            else
            {
                Mostrar("NUMERO");
                q0();
            }
        }

        private void q4()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (Char.IsNumber(car) || Char.IsLetter(car) || car == '$' || car == '_')
            {
                q4();
            }
            else
            {
                q5();
            }
        }

        private void q5()
        {
            if (lstPalabrasReservadas.Contains(lexema))
            {
                Mostrar("RESERVADA");
                q0();
            }
            else
            {
                Mostrar("Identificador");
                q0();
            }
        }

        public void q6()
        {
            val = file.Read();
            car = (Char)val;

            if (car == ' ' || car == '\n' || car == '\r' || car == '\t')
            {
                if (car == '\n')
                {
                    numLinea++;
                }

                q6();
            }
            else
            {
                q0();
            }

        }

        public void q7()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '"')
            {
                q8();
            }
            else
            {
                q7();
            }
        }

        private void q8()
        {
            lexema = lexema + car;

            Mostrar("CADENA");

            val = file.Read();
            car = (Char)val;

            q0();
        }

        private void q9()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            Mostrar("OPERADOR");
            q0();
        }

        private void q10()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            Mostrar("SIMBOLO");
            q0();
        }

        private void q11()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '/')
            {
                q12();
            }

            if (car == '*')
            {
                q13();
            }
        }

        private void q12()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '\n' || car == '\r')
            {
                Mostrar("COMENTARIO SENCILLO");
                q0();
            }
            else
            {
                q12();
            }
        }


        private void q13()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '\n')
            {
                numLinea++;
            }

            if (car == '*')
            {
                q14();
            }
            else
            {
                q13();
            }
        }

        private void q14()
        {
            lexema = lexema + car;

            val = file.Read();
            car = (Char)val;

            if (car == '/')
            {
                Mostrar("COMENTARIO MULTIPLE");

                val = file.Read();
                car = (Char)val;

                q0();
            }
            else
            {
                q13();
            }

        }

        private void Mostrar(String msg)
        {
            colaEntrada.Add(lexema);
            escribeLex.WriteLine($"{numLinea} : {msg} ({lexema})");
            lexema = "";
        }
        private void AnalSint()
        {
            raiz = null;

            for (int i = 0; i < colaEntrada.Count; i++)
            {
                if (raiz == null)
                {
                    raiz = ArbolSintactico.Insertar(colaEntrada[i], null);
                    nodoActual = raiz;
                    i++;
                    i = listaDeclaracion(ArbolSintactico, nodoActual, colaEntrada, i);
                }
                else
                {
                    i = listaSentencia(ArbolSintactico, nodoActual, colaEntrada, i);
                }

            }
            ArbolSintactico.PreOrden(raiz,escribeSint);
            Console.WriteLine("End of sintax");
        }
        private int listaDeclaracion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            bool errorFlag = false;
            do
            {
                //Definir TIPO() segun regla gramatical
                if (arrayValores[i] == "int" || arrayValores[i] == "float" || arrayValores[i] == "boolean")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodo);

                }
                ////////////////
                //Lista-ID
                else if (arrayValores[i] != "," && arrayValores[i] != "{" && arrayValores[i] != "}" && arrayValores[i] != ";")
                    arbol.Insertar(arrayValores[i], nodo);
                //////////////////////////
                else if (colaEntrada[i] == "{")
                {

                }
                else if (colaEntrada[i] == "}")
                {

                }
                i++;
            } while (arrayValores[i] != ";" && errorFlag == false && (colaEntrada[i] != "if" && colaEntrada[i] != "else" && colaEntrada[i] != "write"));

            if (colaEntrada[i] == "if" || colaEntrada[i] == "do" || colaEntrada[i] == "write")
                return i - 1;
            else
                return i;
        }

        private int listaSentencia(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            switch (colaEntrada[i])
            {
                case "{":
                    if (colaEntrada[i - 1] != "program")///BLOQUE
                    {
                        //i = listaDeclaracion(ArbolSintactico, nodo, colaEntrada, i);
                    }
                    else
                    {
                        i++;
                    }
                    break;
                case "}":
                    i++;
                    break;
                case "if":
                    i = seleccion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "else":
                    nodo = arbol.Insertar(arrayValores[i], nodo);
                    i += 2;
                    i = listaSentencia(arbol, nodo, colaEntrada, i);
                    break;
                case "while":
                    i = iteracion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "do":
                    i = repeticion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "read":
                    i = sentRead(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                case "write":
                    i = sentWrite(ArbolSintactico, nodo, colaEntrada, i);
                    break;
                default:
                    i = asignacion(ArbolSintactico, nodo, colaEntrada, i);
                    break;
            }
            return i;
        }
        private int seleccion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol nodoAux = new AnalizadorSintactico.NodoArbol();
            AnalizadorSintactico.NodoArbol nodoRaiz = new AnalizadorSintactico.NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            bool errorFlag = false;
            ///Leer B-Expresion
            if (colaEntrada[i] == "(")
            {
                i++;
                while (colaEntrada[i] != ")")
                {
                    if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);
                    }
                    else
                    {
                        nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                        arbol.Insertar(arrayValores[i], nodo);
                        arbol.Insertar(arrayValores[i + 2], nodo);
                        i += 2;
                    }
                    i++;
                }
            }
            else
                errorFlag = true;
            ////Leer THEN y luego BLOQUE
            i++;
            while (colaEntrada[i] != "fi")
            {
                if (colaEntrada[i] == "then")
                {
                    i++;
                    ////Iniciamos BLOQUE
                    if (colaEntrada[i] == "{")
                    {
                        i++;
                        i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
                    }
                    else
                        errorFlag = true;
                }
                else
                    errorFlag = true;
                if (colaEntrada[i] == "else")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodoRaiz);
                    nodoAux = nodo;
                    i++;
                    ////Llamamos otro BLOQUE
                    if (colaEntrada[i] == "{")
                    {
                        i++;
                        i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
                    }
                    else
                        errorFlag = true;
                }

                if (i > colaEntrada.Count) { // INSERTAR ERROR DE FI FALTANTE
                }
                i++;
            }

            return i;
        }
        private int iteracion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol nodoAux = new AnalizadorSintactico.NodoArbol();
            AnalizadorSintactico.NodoArbol nodoRaiz = new AnalizadorSintactico.NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            ///Leer b-expresion
            bool errorFlag = false;
            if (colaEntrada[i] == "(")
            {
                i++;
                while (colaEntrada[i] != ")")
                {
                    if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);
                    }
                    else
                    {
                        nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                        arbol.Insertar(arrayValores[i], nodo);
                        arbol.Insertar(arrayValores[i + 2], nodo);
                        i += 2;
                    }
                    i++;
                }
            }
            else
                errorFlag = true;
            i++;
            ////Llamamos a un bloque
            if (colaEntrada[i] == "{")
            {
                i++;
                i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
            }
            else
                errorFlag = true;
            return i;
        }
        private int repeticion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol nodoAux = new AnalizadorSintactico.NodoArbol();
            AnalizadorSintactico.NodoArbol nodoRaiz = new AnalizadorSintactico.NodoArbol();
            nodoRaiz = nodo;
            nodo = arbol.Insertar(arrayValores[i], nodo);
            nodoAux = nodo;
            i++;
            bool errorFlag = false;
            ///Leer b-expresion
            if (colaEntrada[i] == "{")
            {
                i++;
                i = listaSentencia(arbol, nodoAux, colaEntrada, i);////Llamamos nuevamente la lista-sentencias
            }
            else
                errorFlag = true;
            i += 2;
            if (colaEntrada[i] == "}")
                i++;
            else
                errorFlag = true;
            if (colaEntrada[i] == "until")
            {
                nodo = arbol.Insertar(arrayValores[i], nodoRaiz);
                i++;
                while (colaEntrada[i] != ";")
                {
                    if (colaEntrada[i] == "(")
                    {
                        i++;
                        while (colaEntrada[i] != ")")
                        {
                            if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                            {
                                nodo = arbol.Insertar(arrayValores[i], nodo);
                            }
                            else
                            {
                                nodo = arbol.Insertar(arrayValores[i + 1], nodo);
                                arbol.Insertar(arrayValores[i], nodo);
                                arbol.Insertar(arrayValores[i + 2], nodo);
                                i += 2;
                            }
                            i++;
                        }
                    }
                    else
                        errorFlag = true;
                    i++;
                }
            }


            return i;


        }
        private int sentRead(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            bool errorFlag = false;
            do
            {
                //Definir TIPO() segun regla gramatical
                if (arrayValores[i] == "read")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodo);

                }
                ////////////////
                //Lista-ID
                else if (arrayValores[i] != "," && arrayValores[i] != "{" && arrayValores[i] != "}" && arrayValores[i] != ";")
                    arbol.Insertar(arrayValores[i], nodo);
                //////////////////////////
                else if (colaEntrada[i] == "{")
                {

                }
                else if (colaEntrada[i] == "}")
                {

                }
                i++;
            } while (arrayValores[i] != ";" && errorFlag == false);
            return i;
        }
        private int sentWrite(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {

            AnalizadorSintactico.NodoArbol nodoAux = new AnalizadorSintactico.NodoArbol();
            List<string> listaSimbolos = new List<string>();
            nodo = arbol.Insertar(arrayValores[i], nodo);
            i++;
            bool errorFlag = false;
            ///Leer B-Expresion
            while (colaEntrada[i] != ";")
            {
                if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                {
                    nodo = arbol.Insertar(arrayValores[i], nodo);
                }
                else
                {
                    ///Llamamos expresion
                    nodoAux = expresion(arbol, nodo, arrayValores, i);
                    nodo.Hijo = nodoAux;
                    i = nodoAux.AuxIteracion;
                }
            }


            return i;
        }

        private AnalizadorSintactico.NodoArbol expresion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.Arbol arbolSec = new AnalizadorSintactico.Arbol();
            AnalizadorSintactico.NodoArbol trabajo = new AnalizadorSintactico.NodoArbol(),temp = new AnalizadorSintactico.NodoArbol();
           
            nodo.Hijo = termino(arbolSec, arrayValores, i);
            nodo.AuxIteracion = nodo.Hijo.AuxIteracion;
            i = nodo.AuxIteracion;

            ///Reinicializamos variables
            AnalizadorSintactico.NodoArbol nuevo = new AnalizadorSintactico.NodoArbol();
            temp = new AnalizadorSintactico.NodoArbol(); 
            while (arrayValores[i] == "+" || arrayValores[i] == "-")
            {
                AnalizadorSintactico.NodoArbol Aux = new AnalizadorSintactico.NodoArbol();
                Aux.Dato = arrayValores[i];
                Aux.Hijo = arbolSec.raiz;
                arbolSec.raiz = Aux;
                nuevo = Aux;
                i++;
                temp = factor(arrayValores, i);
                i++;
                arbolSec.Insertar(temp.Dato, nuevo);
                temp = nuevo;

            }
            temp.AuxIteracion = i;
            return temp;
        }
        private AnalizadorSintactico.NodoArbol expresion2(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.Arbol arbolSec = new AnalizadorSintactico.Arbol();
            AnalizadorSintactico.NodoArbol temp = new AnalizadorSintactico.NodoArbol();
            temp = termino(arbolSec, arrayValores, i);
            i = temp.AuxIteracion;

            ///Reinicializamos variables
            AnalizadorSintactico.NodoArbol nuevo = new AnalizadorSintactico.NodoArbol();
            while (arrayValores[i] == "+" || arrayValores[i] == "-")
            {
                AnalizadorSintactico.NodoArbol Aux = new AnalizadorSintactico.NodoArbol();
                Aux.Dato = arrayValores[i];
                if(Aux.Hijo!=null)
                    Aux.Hijo = arbolSec.raiz;
                arbolSec.raiz = Aux;
                if (arbolSec.raiz.Hijo == null)
                    arbolSec.Insertar(temp.Dato, arbolSec.raiz);
                nuevo = Aux;
                i++;
                temp = factor(arrayValores, i);
                i++;
                arbolSec.Insertar(temp.Dato, nuevo);
                temp = nuevo;

            }
            temp.AuxIteracion = i;
            return temp;
        }

        private AnalizadorSintactico.NodoArbol termino(AnalizadorSintactico.Arbol arbolSec, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol temp = new AnalizadorSintactico.NodoArbol(), nuevo = new AnalizadorSintactico.NodoArbol();////Variables temporales

            temp = factor(arrayValores, i);
            i++;
            while (arrayValores[i] == "*" || arrayValores[i] == "/")
            {
                if (arbolSec.raiz.Dato == "")
                {
                    nuevo = arbolSec.Insertar(arrayValores[i], null);
                    arbolSec.Insertar(temp.Dato, nuevo);
                    i++;
                    arbolSec.Insertar(factor(arrayValores, i).Dato, nuevo);
                    i++;
                    temp = nuevo;
                }
                else
                {
                    AnalizadorSintactico.NodoArbol Aux = new AnalizadorSintactico.NodoArbol();
                    Aux.Dato = arrayValores[i];
                    Aux.Hijo = arbolSec.raiz;
                    arbolSec.raiz = Aux;
                    nuevo = Aux;
                    i++;
                    temp = factor(arrayValores, i);
                    i++;
                    arbolSec.Insertar(temp.Dato, nuevo);
                    temp = nuevo;
                }
            }
            temp.AuxIteracion = i;
            return temp;
        }
        private AnalizadorSintactico.NodoArbol factor(List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol temp = new AnalizadorSintactico.NodoArbol();////Variables temporales
            if (arrayValores[i] == "(")
            {

            }
            else
            {
                temp.Dato = arrayValores[i];////Insertamos valor en nodo
                i++;
            }
            return temp;
        }
        private int asignacion(AnalizadorSintactico.Arbol arbol, AnalizadorSintactico.NodoArbol nodo, List<string> arrayValores, int i)
        {
            AnalizadorSintactico.NodoArbol nodoAux = new AnalizadorSintactico.NodoArbol(),trabajo;
            i++;
            bool errorFlag = false;
            if (colaEntrada[i] == "=")
            {
                nodo = arbol.Insertar(arrayValores[i], nodo);
                arbol.Insertar(arrayValores[i - 1], nodo);
                i++;
                ///Leer B-Expresion
                while (colaEntrada[i] != ";")
                {
                    if (colaEntrada[i] == "true" || colaEntrada[i] == "false")
                    {
                        nodo = arbol.Insertar(arrayValores[i], nodo);
                    }
                    else
                    {
                        ///Llamamos expresion
                        nodoAux = expresion2(arbol, nodo, arrayValores, i);
                        if(nodo.Hijo==null)
                            nodo.Hijo = nodoAux;
                        else
                        {
                            trabajo = new AnalizadorSintactico.NodoArbol();
                            trabajo = nodo.Hijo;
                            //Avanzamos hasta el ultimo hermano
                            while (trabajo.Hermano != null)
                            {
                                trabajo = trabajo.Hermano;
                            }
                            //Unimos el temporal al ultimo hermano
                            trabajo.Hermano = nodoAux;
                        }
                        i = nodoAux.AuxIteracion;
                    }
                }
            }
            else
            {
                errorFlag = true;
            }

            return i;
        }


    }
}