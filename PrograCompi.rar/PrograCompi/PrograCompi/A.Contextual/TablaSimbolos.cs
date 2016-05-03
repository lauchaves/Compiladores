using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;


//Se utiliza namespace para acceder a todo el contenido del folder A.Contextual
namespace PrograCompi.A.Contextual
{

    class TablaSimbolos
    {


        // Contiene las estructuras que se usan para la validación de tipos e identificadores. 

        //List<Object> t;
        List<Clase> LClases;            //Contiene todas las clases declaradas durante la ejecución.
        List<Metodo> LMetodos;         // Lista de Métodos: contiene las declaraciones de los métodos.
        List<Variable> LVariables;    //  Lista de Varibales: contiene todas las declaraciones de variables.

        public TablaSimbolos()
        {
            LClases = new List<Clase>();
            LMetodos = new List<Metodo>();
            LVariables = new List<Variable>();
            
        }

        public void insertarClase(Clase c) {
            LClases.Add(c);
        }

        public void insertarMetodo(Metodo m)
        {
            LMetodos.Add(m);
        }

        public void insertarVariable(Variable v)
        {
            LVariables.Add(v);
        }


        /*
        public void insertar(String nombre, int tipo)
        {
            IToken token = new CommonToken(tipo, nombre);
            t.Add(token);
        }*/

        // Busca Clase por nombre, retorna true/false
        public bool buscarClase(string Nombre)
        {
            foreach (Clase item in LClases)
            {
                if (item.Nombre.Equals(Nombre))
                {
                    return true;
                }
            }
            return false;
        }

        // Busca Metodo por nombre, retorna true/false
        public bool buscarMetodo(string Nombre)
        {
            foreach (Metodo item in LMetodos)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return true;
                }
            }
            return false;
        }

        // Busca variable por nombre, retorna true/false 
        public bool buscarVariable(string Nombre)
        {
            foreach (Variable item in LVariables)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return true;
                }
            }
            return false;
        }


        // Elimina de la tabla todas las variables de nivel 1
       
        public void eliminarVariables()
        {
            int i;

            for (i = LVariables.Count - 1; i >= 0; i--)
            {
                if (LVariables[i].nivel == 1)
                {
                    LVariables.RemoveAt(i);
                }
            }
        }



        //Busca y retorna una clase en la tabla
        //Por Nombre de la clase

        public Clase retornarClase(string Nombre)
        {
            foreach (Clase item in LClases)
            {
                if (item.Nombre.Equals(Nombre))
                {
                    return item;
                }
            }
            return null;
        }

        //Busca y retorna una clase en la tabla
        //Por Nombre de un Metodo
        public Metodo retornarMetodo(string Nombre)
        {
            foreach (Metodo item in LMetodos)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return item;
                }
            }
            return null;
        }

        //Busca y retorna una clase en la tabla
        // Por Nombre de una variable
        public Variable retornarVariable(string Nombre)
        {
            foreach (Variable item in LVariables)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return item;
                }
            }
            return null;
        }




        /*
        public IToken buscar(String nombre)
        {
            return (IToken)t.Find(item => ((IToken)item).Text.Equals(nombre));
        }

        public void imprimir()
        {
            for (int i = 0; i < t.Count; i++)
            {
                IToken s = (IToken)t.ElementAt(i);
                Console.WriteLine("Nombre: " + s.Text);
                if (s.Type == 0) Console.WriteLine("\tTipo: Indefinido");
                else if (s.Type == 1) Console.WriteLine("\tTipo: Integer\n");
                else if (s.Type == 2) Console.WriteLine("\tTipo: String\n");
            }
        }
         * */

    }
}


