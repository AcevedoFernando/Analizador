using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalizadorLexicosEstados.AnalizadorSintactico
{
    class NodoArbol
    {
        private string dato = "";
        private NodoArbol hijo=null;
        private NodoArbol hermano=null;
        private int AuxIt = 0;
        public string Dato { get => dato; set => dato = value; }
        public NodoArbol Hijo { get => hijo; set => hijo = value; }
        public NodoArbol Hermano { get => hermano; set => hermano = value; }
        public int AuxIteracion { get => AuxIt; set => AuxIt= value; }
        public NodoArbol()
        {
            dato = "";
            hijo = null;
            hermano = null;
            AuxIteracion = 0;
        }
    }
}
