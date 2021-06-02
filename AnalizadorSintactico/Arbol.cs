using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace AnalizadorLexicosEstados.AnalizadorSintactico
{
    class Arbol
    {
        public NodoArbol raiz;
        private NodoArbol trabajo;
        private int i = 0;
        public Arbol()
        {
            raiz = new NodoArbol();
        }

        public NodoArbol Insertar(string pDato, NodoArbol pNodo)
        {
            //Si no hay nodo, tomamos como si fuera la raiz
            if (pNodo == null)
            {
                raiz = new NodoArbol();
                raiz.Dato = pDato;

                //No hay hijo
                raiz.Hijo = null;

                //No hay hermano
                raiz.Hermano = null;

                return raiz;
            }

            //Verificamos si no tiene hijo 
            //Insertamos el dato como hijo
            if (pNodo.Hijo == null)
            {
                NodoArbol temp = new NodoArbol();
                temp.Dato = pDato;

                //Conectamos el nuevo nodo como hijo
                pNodo.Hijo = temp;

                return temp;
            }
            else // ya tiene un hijo tenemos que insertarlo como hermano
            {
                trabajo = pNodo.Hijo;

                //Avanzamos hasta el ultimo hermano
                while (trabajo.Hermano != null)
                {
                    trabajo = trabajo.Hermano;
                }

                //Creamos nodo temporal
                NodoArbol temp = new NodoArbol();

                temp.Dato = pDato;

                //Unimos el temporal al ultimo hermano
                trabajo.Hermano = temp;

                return temp;
            }
        }
        public void PreOrden(NodoArbol pNodo,TextWriter escribeSint)
        {
            if (pNodo == null)
                return;

            //Me proceso primero a mi
            for (int n = 0; n < i; n++)
            {
                Console.Write("  ");
                escribeSint.Write("     ");
            }

            escribeSint.WriteLine(pNodo.Dato);
            Console.WriteLine(pNodo.Dato);

            //Luego proceso a mi hijo
            if (pNodo.Hijo != null)
            {
                i++;
                PreOrden(pNodo.Hijo,escribeSint);
                i--;
            }

            //Si tengo hermanos, los proceso
            if (pNodo.Hermano != null)
                PreOrden(pNodo.Hermano,escribeSint);

        }
        

    }
}
