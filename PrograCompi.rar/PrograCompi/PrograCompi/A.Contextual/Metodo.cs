using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograCompi.A.Contextual
{
    class Metodo
    {
        public String nombre;
        public Tipo tipo;
        public int nivel;

        List<Variable> parametros;

        public Metodo(String nombre, Tipo tipo, int nivel)
        {
            this.nombre = nombre;
            this.tipo = tipo;
            this.nivel = nivel;
            this.parametros = new List<Variable>();
        }


        //inserta parametro

        public void insertarParametro(Variable v)
        {
            this.parametros.Add(v);
        }

        // Devuelve el parametro segun la pos
        public Variable retornarParamPos(int Indice)
        {
            return parametros[Indice];
        }


       
        /// Recibe un nombre y retorna true si el método tiene un método con dicho método
        //El nombre de el parámetro a buscar
        
        public bool buscarParametro(string Nombre)
        {
            foreach (Variable item in parametros)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return true;
                }
            }
            return false;
        }



        //Recibe un nombre y retorna el parámetro en esa posición
        //El nombre del parámetro a retornar

        public Variable retornarParametroPorNombre(string Nombre)
        {
            foreach (Variable item in parametros)
            {
                if (item.nombre.Equals(Nombre))
                {
                    return item;
                }
            }
            return null;
        }


        //Retorna el número de parámetros que posee el método
        //Número de parámetros

        public int cantidadParametros()
        {
            return parametros.Count;
        }

    }
}
