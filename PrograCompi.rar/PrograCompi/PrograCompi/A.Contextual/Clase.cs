using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograCompi.A.Contextual
{
    class Clase
    {
        //Contiene nombre de la clase y sus atributos -> variables
        public string Nombre;
        List<Variable> Variables;

        public Clase(string Nombre)
        {
            this.Nombre = Nombre;
            Variables = new List<Variable>();
        }

        public void insertarVar(Variable v)
        {
            Variables.Add(v);
        }


        // Recibe un nombre y retorna verdadero si la clase posee una variable con
        // dicho nombre, de lo contrario falso.
        public bool buscarVar(string Nombre)
        {
            foreach (Variable item in Variables)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return true;
                }
            }
            return false;
        }

        // Recibe un nombre y retorna la variable de la clase con dicho nombre
        public Variable retornarVar(string Nombre)
        {
            foreach (Variable item in Variables)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return item;
                }
            }
            return null;
        }

    }

}

