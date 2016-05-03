using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograCompi.A.Contextual
{

    // Clase para retornar tipos
    class Tipo
    {
        public String nombre;
        public int linea;
        public int columna;


        public Tipo(String nombre, int linea, int columna)
        {
            this.nombre = nombre;
            this.linea = linea;
            this.columna = columna;
        }
    }
}
