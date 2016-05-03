using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograCompi.A.Contextual
{
    // Nodo de la tabla de variables, representa la declaración de una variable.
    class Variable
    {
        public String  nombre;
        public Tipo    tipo;
        public int     nivel;
        public bool     Const = false;
        public bool    Arreglo = false;  //determinar si es arreglo o no. 


        public Variable(String nombre, Tipo tipo, int nivel)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.nivel = nivel;
        }
    }
}
