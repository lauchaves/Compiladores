using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PrograCompi;
using Antlr4.Runtime;

using IToken = Antlr4.Runtime.IToken;
using ParserRuleContext = Antlr4.Runtime.ParserRuleContext;

using PrograCompi.A.Contextual;

namespace PrograCompi
{

    // Comentario prueba github

    /// <summary>
    /// This class provides an empty implementation of <see cref="IParser1Visitor{Result}"/>,
    /// which can be extended to create a visitor which only needs to handle a subset
    /// of the available methods.
    /// </summary>
    /// <typeparam name="Result">The return type of the visit operation.</typeparam>
    [System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.5-SNAPSHOT")]
    [System.CLSCompliant(false)]

public class AContextual : Parser1BaseVisitor<Object>
{
    

    //------------------------------------------ NUESTRO CODIGO VA AQUI --------------------------------------


        TablaSimbolos Tabla;  
        int ContextoActual = -1;
        int ContadorCiclos = 0;

        string ClaseActual = null;
        string MetodoActual = null;

        bool ReturnMayor = false;
        bool Return = false;
        bool Arreglo = false;
        bool Arreglo2 = false;
        bool designatorExpression = false;

        int designatorLinea;
        int designatorColumna;


        public AContextual()
        {
            Tabla = new TablaSimbolos(); // Instancia de la tabla de simbolos
            
            // para declarar variables
            Clase _char = new Clase("char");
            Clase _int = new Clase("int");
            Clase _float = new Clase("float");
            Clase _bool = new Clase("bool");
            

            Tabla.insertarClase(_char);
            Tabla.insertarClase(_int);
            Tabla.insertarClase(_float);
            Tabla.insertarClase(_bool);

            /*
             * La tabla de símbolos debe considerar los métodos pre-establecidos
             * (ord, chr y len) y constantes, como “null”, etc.
             * 
             * */


            // Se crea el metodo ord.
            // Se crea variable ch que sera el parametro de ord que es de tipo char. 
            Metodo ord = new Metodo("ord", new Tipo("int", 0, 0), -1);
            Variable ch = new Variable("ch", new Tipo("char", 0, 0), 1);
            ord.insertarParametro(ch);

            // Se crea el metodo chr.
            // Se crea variable i que sera el parametro de chr que es de tipo int.
            Metodo chr = new Metodo("chr", new Tipo("char", 0, 0), -1);
            Variable i = new Variable("i", new Tipo("int", 0, 0), 1);
            chr.insertarParametro(i);

            // Se crea el metodo len *para ints
            // Se crea variable a1 que sera el parametro de len para tipos int.
            Metodo len1 = new Metodo("len", new Tipo("int", 0, 0), -1);
            Variable a1 = new Variable("a", new Tipo("int", 0, 0), 1);
            len1.insertarParametro(a1);


            // Se crea el metodo len *para chars
            // Se crea variable a2 que sera el parametro de len para tipos char.
            Metodo len2 = new Metodo("len", new Tipo("int", 0, 0), -1);
            Variable a2 = new Variable("a", new Tipo("char", 0, 0), 1);
            len2.insertarParametro(a2);
            
            //La tabla de símbolos debe considerar los métodos pre-establecidos, se insertan los metodos
            //a la tabla de simbolos
            Tabla.insertarMetodo(ord);
            Tabla.insertarMetodo(chr);
            Tabla.insertarMetodo(len1);
            Tabla.insertarMetodo(len2);

            //Se crea la variable null.
            Variable _null = new Variable("null", new Tipo("null", 0, 0), -1);
            Tabla.insertarVariable(_null);  // Inserta null a la tabla de simbolos.

            ContextoActual++;
        }






	/// <summary>
	/// Visit a parse tree produced by the <c>classDeclAST</c>
	/// labeled alternative in <see cref="Parser1.classDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
        public override object VisitClassDeclAST([NotNull] Parser1.ClassDeclASTContext context) { 

            // CLASS ID CORCHETEIZQ (varDecl)* CORCHETEDER	

            ClaseActual = context.ID().Symbol.Text;  // Clase Actual -> para declaracion de variables
            ContextoActual = 1; // Contexto nivel 1: bloques 

            // No permite clases repetidas
            if (Tabla.buscarClase(ClaseActual))
            {
                throw new Exception("Error: Ya existe una definición para '" + ClaseActual + "línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"'\n");
            }

            //Crea la nueva clase, la agrega a la tabla de simbolos
            Clase Clase = new Clase(ClaseActual);  
            Tabla.insertarClase(Clase);

            int vardeclLen = context.varDecl().Length;
            int i;

            // Para (varDecl)*
            for (i = 0; i < vardeclLen; i++) {
                Visit(context.varDecl(i));
            }

            // Vuelve a inicializar los valores de claseactual y contextoactual
            ClaseActual = null;
            ContextoActual = 0;

            return null;
 
        }

	/// <summary>
	/// Visit a parse tree produced by the <c>declaAST</c>
	/// labeled alternative in <see cref="Parser1.decla"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    //public override object VisitDeclaAST([NotNull] Parser1.DeclaASTContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by the <c>addopAST</c>
	/// labeled alternative in <see cref="Parser1.addop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitAddopAST([NotNull] Parser1.AddopASTContext context) {

        //(SUMA | MENOS)
        if (context.SUMA() != null)
        {

            //Crea el nuevo tipo -> + , almacena la linea y columna donde se encuentra
            return new Tipo(context.SUMA().Symbol.Text, context.SUMA().Symbol.Line, context.SUMA().Symbol.Column);
        }
        else {
            //Crea el nuevo tipo -> - , almacena la linea y columna donde se encuentra
            return new Tipo(context.MENOS().Symbol.Text, context.MENOS().Symbol.Line, context.MENOS().Symbol.Column);
        
        }
        
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>condTermAST</c>
	/// labeled alternative in <see cref="Parser1.condTerm"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitCondTermAST([NotNull] Parser1.CondTermASTContext context) { 
        
        //condFact (Y condFact)*	
        int i;
        // en caso condfact -> )*
        for (i = 0; i < context.condFact().Length; i++) {
            Visit(context.condFact(i));
        }
            return null; 
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>constDeclCharAST</c>
	/// labeled alternative in <see cref="Parser1.constDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitConstDeclCharAST([NotNull] Parser1.ConstDeclCharASTContext context) {


        //CONST type ID ASIGN CHARCONST PyCOMA	 

        Tipo tipo = (Tipo)Visit(context.type());
        String id = context.ID().Symbol.Text;

        // CHARCONST -> char
        if (!tipo.nombre.Equals("char"))
        {
            throw new Exception("Error: \nNo se puede convertir implícitamente el tipo '" + tipo.nombre +
                "' en 'char'  línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
        }

        //Ningún identificador debe ser declarados dos veces en el mismo ámbito.
        if (Tabla.buscarVariable(id))
        {
            throw new Exception("Error : \n" + "El tipo '" + ClaseActual + "' ya contiene una definición para '" + id + "'" +
                "línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
        }


        Variable Const = new Variable(id, tipo, 0);
        Const.Arreglo = Arreglo;
        Const.Const = true;
        Tabla.insertarVariable(Const);


        return null;


    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>constDeclNumAST</c>
	/// labeled alternative in <see cref="Parser1.constDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitConstDeclNumAST([NotNull] Parser1.ConstDeclNumASTContext context) {


        //CONST type ID ASIGN NUM PyCOMA

        Tipo tipo = (Tipo) Visit(context.type());
        String id = context.ID().Symbol.Text;

        // NUM -> INT
        if (!tipo.nombre.Equals("int"))
        {
            throw new Exception("Error: \nNo se puede convertir implícitamente el tipo '" + tipo.nombre +
                "' en 'int'  línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"\n");
        }

        //Ningún identificador debe ser declarados dos veces en el mismo ámbito.
        if (Tabla.buscarVariable(id))
        {
            throw new Exception("Error : \n"+"El tipo '" + ClaseActual + "' ya contiene una definición para '" + id + "'"+
                "línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"\n");
        }


        Variable Const = new Variable(id, tipo, 0);
        Const.Arreglo = Arreglo;
        Const.Const = true;
        Tabla.insertarVariable(Const);


        return null;
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>programAST</c>
	/// labeled alternative in <see cref="Parser1.program"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitProgramAST([NotNull] Parser1.ProgramASTContext context) { 
        
      //  CLASS ID (decla)* CORCHETEIZQ (methodDecl)* CORCHETEDER

        Clase Program = new Clase(context.ID().Symbol.Text);// Crea instancia de la calse
        Tabla.insertarClase(Program);

        int declaCount = context.decla().Length;
        int methodCount = context.methodDecl().Length;

        int i;

        for (i = 0; i < declaCount; i++)
        {
            Visit(context.decla(i));
        }

        for (i = 0; i < methodCount; i++)
        {
            Visit(context.methodDecl(i));
        }

        //Caso error contextual TIENE QUE TENER MAIN
        if (!Tabla.buscarMetodo("Main"))
        {
            throw new Exception("Error: No existe ningún método 'Main' void!\n");  //Manejo de errores con exceptions
        }

        //Caso error contextual EL MAIN TIENE QUE SER TIPO VOID
        Metodo Main = Tabla.retornarMetodo("Main");
        if (!Main.tipo.nombre.Equals("void"))
        {
            throw new Exception("Error: No existe ningún método 'Main' void\n");
        }

        // Caso error contextual EL MAIN NO TIENE PARAMS
        if (Main.cantidadParametros() != 0)
        {
            throw new Exception("Error: El método 'Main' no debe tener ningún parámetro\n");
        }

        return null;
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>methodDeclAST</c>
	/// labeled alternative in <see cref="Parser1.methodDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitMethodDeclAST([NotNull] Parser1.MethodDeclASTContext context) {


        //(type | VOID) ID PIZQ (formPars | ) PDER (varDecl)* block		


        //                                                                                              ENTRA TYPE!!!!!!!!! 
        MetodoActual = context.ID().Symbol.Text; //  ID !!!! 

        if (context.type() != null)
        {
            // TYPE

            Tipo type = (Tipo)Visit(context.type());

            if (Tabla.buscarClase(type.nombre))
            {
                
                ContextoActual = 2; // Contexto actual 2 -> Params

                // No permite metodos repetidos
                if (Tabla.buscarClase(MetodoActual) ||
                    Tabla.buscarMetodo(MetodoActual) ||
                    Tabla.buscarVariable(MetodoActual))
                {
                    throw new Exception("Error: Ya existe una definición para '" + MetodoActual + "', línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"\n");
                }

                //Crea el metodo, lo inserta en la tabla de simbolos
                Metodo Metodo = new Metodo(MetodoActual, type, 0);
                Tabla.insertarMetodo(Metodo);

                // Si tiene FormPars
                if (context.formPars() != null)
                {
                    //FORMPARS

                    Visit(context.formPars()); 

                    int i;
                    int vardeclCount = context.varDecl().Length;

                    // para (varDecl)*
                    for (i = 0; i < vardeclCount; i++)
                    {
                        Visit(context.varDecl(i));
                    }

                    Visit(context.block());

                    // Los metodos tienen que retornar algo
                    if (!Return)
                    {
                        throw new Exception("Error: '"+ MetodoActual + "': No todas las rutas de código devuelven un valor"+", línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"\n");
                    }

                    if (!ReturnMayor)
                    {
                        throw new Exception("Error: '" + MetodoActual + "': No todas las rutas de código devuelven un valor" + ", línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
                    }
                     

                    
                    Tabla.eliminarVariables(); // Limpia las variable para poder utilizarlas de nuevo en otros metodos
                    //Inicializa los valores
                    MetodoActual = null;
                    ContextoActual = 0;
                    Return = false;
                    ReturnMayor = false;

                    return null;
                }
                else { 
                    // VACIO

                    // lo mismo que el anterior pero sin formpars :P 
                    int i;
                    int vardeclCount = context.varDecl().Length;

                    for (i = 0; i < vardeclCount; i++)
                    {
                        Visit(context.varDecl(i));
                    }

                    Visit(context.block());

                    if (!Return)
                    {
                        throw new Exception("Error: '" + MetodoActual + "': No todas las rutas de código devuelven un valor" + ", línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
                    }
                    

                    if (!ReturnMayor)
                    {
                        throw new Exception("Error: '" + MetodoActual + "': No todas las rutas de código devuelven un valor" + ", línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
                    }
                    

                    Tabla.eliminarVariables();
                    MetodoActual = null;
                    ContextoActual = 0;
                    Return = false;
                    ReturnMayor = false;

                    return null;
                
                }
            }
            else
            {
                throw new Exception("Error, línea: " + type.linea + " columna: " + type.columna +
                                    "\nNo se puede encontrar el tipo '" + type.nombre + "'\n");
            }
        }

        //                                                                                          ENTRA VOID!!!!!!!!!!!!!! 
        else
        {
            // VOID
            MetodoActual = context.ID().Symbol.Text; // ID!!!!
            ContextoActual = 2;  // Contexto actual 2 -> Params

            // No permite metodos repetidos
            if (Tabla.buscarClase(MetodoActual) ||
                Tabla.buscarMetodo(MetodoActual) ||
                Tabla.buscarVariable(MetodoActual))
            {
                throw new Exception("Error: Ya existe una definición para '" + MetodoActual + "'" + " línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column + "\n");
            }


            //Crea el metodo VOID!! 
            // lo inserta en la tabla de simbolos
            Metodo Metodo = new Metodo(MetodoActual, new Tipo("void", context.VOID().Symbol.Line, context.VOID().Symbol.Column), 0);
            Tabla.insertarMetodo(Metodo);


            // Si tiene FormPars
            if (context.formPars() != null)
            {
                //FORMPARS
                Visit(context.formPars());

                int i;
                int vardeclCount = context.varDecl().Length;

                for (i = 0; i < vardeclCount; i++)
                {
                    Visit(context.varDecl(i));
                }

                Visit(context.block());

                Tabla.eliminarVariables(); // Limpia las variable para poder utilizarlas de nuevo en otros metodos
                //Inicializa los valores
                MetodoActual = null;
                ContextoActual = 0;
                Return = false;

                return null;


            }
            else { 
                //VACIO

                //lo mismo de arriba pero sin formpars :P
                int i;
                int vardeclCount = context.varDecl().Length;

                for (i = 0; i < vardeclCount; i++)
                {
                    Visit(context.varDecl(i));
                }

                Visit(context.block());

                Tabla.eliminarVariables();
                MetodoActual = null;
                ContextoActual = 0;

                return null;
            
            }

            
        }

  
    
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>desig2AST</c>
	/// labeled alternative in <see cref="Parser1.desig2"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitDesig2AST([NotNull] Parser1.Desig2ASTContext context) {

        // (PUNTO ID | PCIZQ expr PCDER)

        if (context.PUNTO() != null)
        {
            //context.Tipo = 
            return context.ID().Symbol.Text;

        }
        else {

            Tipo expr = (Tipo)Visit(context.expr());
            if (expr.nombre.Equals("int"))
            {
                return expr;
            }

            throw new Exception("Error: Solo se aceptan expresiones de tipo 'int'.  Línea: " + expr.linea + " columna: " + expr.columna +"\n");
        
        }


        return null;
    
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>typeAST</c>
	/// labeled alternative in <see cref="Parser1.type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitTypeAST([NotNull] Parser1.TypeASTContext context) {

        //ID ( PCIZQ PCDER | )	  ARREGLOS

        if (context.PCIZQ() != null)
        {
            Tipo type = new Tipo(context.ID().Symbol.Text, context.ID().Symbol.Line, context.ID().Symbol.Column);


            if (type.nombre != "int" && type.nombre != "char")
            {
                throw new Exception("Error, línea: " + type.linea + " columna: " + type.columna +
                                    "\nSolo se admiten arreglos de tipo 'int' y 'char'\n");
            }

            Arreglo = true;  // Es un arreglo bandera true

            return type;
        }
        else { 
            //VACIO
            return new Tipo(context.ID().Symbol.Text, context.ID().Symbol.Line, context.ID().Symbol.Column);
        }
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>relopAST</c>
	/// labeled alternative in <see cref="Parser1.relop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitRelopAST([NotNull] Parser1.RelopASTContext context) {


        // (IGUALIGUAL | DIFERENTE | MAYOR | MAYORIGUAL | MENOR | MENORIGUAL) 

        /*
         * Crea el nuevo tipo -> (==|!=|>|>=|<|<=) , almacena la linea y columna donde se encuentra
         * 
         * */

        if (context.IGUALIGUAL() != null) {

            return new Tipo(context.IGUALIGUAL().Symbol.Text, context.IGUALIGUAL().Symbol.Line, context.IGUALIGUAL().Symbol.Column);
        }
        else if (context.DIFERENTE() != null)
        {
            return new Tipo(context.DIFERENTE().Symbol.Text, context.DIFERENTE().Symbol.Line, context.DIFERENTE().Symbol.Column);
        }
        else if (context.MAYOR() != null) 
        {
            return new Tipo(context.MAYOR().Symbol.Text, context.MAYOR().Symbol.Line, context.MAYOR().Symbol.Column);
        }
        else if (context.MAYORIGUAL() != null)
        {
            return new Tipo(context.MAYORIGUAL().Symbol.Text, context.MAYORIGUAL().Symbol.Line, context.MAYORIGUAL().Symbol.Column);
        }
        else if (context.MENOR() != null)
        {
            return new Tipo(context.MENOR().Symbol.Text, context.MENOR().Symbol.Line, context.MENOR().Symbol.Column);
        }
        else
            return new Tipo(context.MENORIGUAL().Symbol.Text, context.MENORIGUAL().Symbol.Line, context.MENORIGUAL().Symbol.Column);

    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>formParsAST</c>
	/// labeled alternative in <see cref="Parser1.formPars"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitFormParsAST([NotNull] Parser1.FormParsASTContext context) {


        //type ID (COMA type ID)*	

        int i;
        int idcount = context.ID().Length;

        for (i = 0; i < idcount; i++)
        {
            Tipo type = (Tipo)Visit(context.type(i));

            // Busca el tipo -> char int bool float
            if (!Tabla.buscarClase(type.nombre))
            {
                throw new Exception("Error: No se puede encontrar el tipo '" + type.nombre + "'"+"línea: " + type.linea + " columna: " + type.columna +"\n");
            }

            string ID = context.ID(i).Symbol.Text; // Obtiene el nombre ID!! 

            //No permite parametros repetidos
            if (Tabla.retornarMetodo(MetodoActual).buscarParametro(ID))
            {
                throw new Exception("Error:El nombre de parámetro '" + ID + "' está duplicado"+ " línea: " + context.ID(i).Symbol.Line + " columna: " + context.ID(i).Symbol.Column +"\n");
            }

            //Crea la nueva variable y lo inserta en la tabla de simbolos
            Variable Parametro = new Variable(ID, type, 1);  // Nivel 1: variable local
            Tabla.retornarMetodo(MetodoActual).insertarParametro(Parametro);
        }

        return null;
    
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>actParsAST</c>
	/// labeled alternative in <see cref="Parser1.actPars"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitActParsAST([NotNull] Parser1.ActParsASTContext context) {


        // expr (COMA expr)*	 
        List<Tipo> Lista;
        Lista = new List<Tipo>();

        int i;
        for (i = 0; i < context.expr().Length; i++)
        {
            Lista.Add((Tipo)Visit(context.expr(i)));
        }
        return Lista;  
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>designatorAST</c>
	/// labeled alternative in <see cref="Parser1.designator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	 public override object VisitDesignatorAST([NotNull] Parser1.DesignatorASTContext context) {

         // ID (desig2)*


         designatorLinea = context.ID().Symbol.Line;  // para manejo de errores
         designatorColumna = context.ID().Symbol.Column; // para manejo de errores

         if (context.desig2().Length > 1)
         {
             throw new Exception("Error: El miembro que se intenta acceder no se encuentra dentro del rango permitido. Línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
         }

         int designatorauxCount = context.desig2().Length;

         // Obtiene el id
         string ID = context.ID().Symbol.Text;

         string[] designators = new string[designatorauxCount + 1];
         designators[0] = ID;


         if (Tabla.buscarMetodo(ID))
         {
             //context.Tipo = ((Metodo)Tabla.retornarMetodo(ID)).tipo.nombre;

             return designators;
         }
         if (!Tabla.buscarVariable(ID) && !((Metodo)Tabla.retornarMetodo(MetodoActual)).buscarParametro(ID))
         {
             throw new Exception("Error: El nombre '" + ID + "' no existe en el contexto actual. Línea: " + context.ID().Symbol.Line + " columna: " + context.ID().Symbol.Column +"\n");
         }
         if (Tabla.buscarVariable(ID))
         {
             //context.Tipo = ((Variable)Tabla.retornarVariable(ID)).tipo.nombre;

             bool expr = false;
             int i;
             for (i = 0; i < designatorauxCount; i++)
             { 

                 if ((Visit(context.desig2(i))).GetType() == (Type.GetType("System.String")))//.ID
                 {
                     Variable Variable = Tabla.retornarVariable(designators[i]);
                     Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);
                     if (!Clase.buscarVar((string)Visit(context.desig2(i))))
                     {
                         throw new Exception("Error: '" + Clase.Nombre + "' no contiene una definición de '" + (string)Visit(context.desig2(i)) + "' línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                     }
                     designators[i + 1] = (string)Visit(context.desig2(i));
                     //context.desig2(i).Tipo = ((Variable)Clase.retornarVar((string)Visit(context.desig2(i)))).tipo.nombre;
                 }
                 else//[expr]
                 {
                     designatorExpression = true;
                     if (i != designatorauxCount - 1)
                     {
                         throw new Exception("Error, línea: " + designatorLinea + " columna: " + designatorColumna +
                                             "\nPara acceder a una poscición del arreglo, este debe ser el último en accederse\n");
                     }
                     expr = true;
                 }
             }
             if (expr)
             {
                 string[] aux = new string[designatorauxCount];
                 for (i = 0; i < aux.Length; i++)
                 {
                     aux[i] = designators[i];
                 }
                 return aux;
             }
             return designators;
         }
         else
         {
             //context.Tipo = ((Variable)((Metodo)Tabla.retornarMetodo(MetodoActual)).retornarParametroPorNombre(ID)).tipo.nombre;
             bool expr = false;
             int i;
             for (i = 0; i < designatorauxCount; i++)
             {


                 if ((Visit(context.desig2(i))).GetType() == (Type.GetType("System.String")))//.ID
                 {
                     Clase Clase = Tabla.retornarClase(Tabla.retornarMetodo(MetodoActual).retornarParametroPorNombre(designators[0]).tipo.nombre);
                     if (!Clase.buscarVar((string)Visit(context.desig2(i))))
                     {
                         throw new Exception("Error, línea: " + designatorLinea + " columna: " + designatorColumna +
                                             "\n'" + Clase.Nombre + "' no contiene una definición de '" + (string)Visit(context.desig2(i)) + "'\n");
                     }
                     designators[i + 1] = (string)Visit(context.desig2(i));
                     //context.desig2(i).Tipo = ((Variable)Clase.retornarVar((string)Visit(context.desig2(i)))).tipo.nombre;
                 }
                 else//[expr]
                 {
                     designatorExpression = true;
                     if (i != designatorauxCount - 1)
                     {
                         throw new Exception("Error, línea: " + designatorLinea + " columna: " + designatorColumna +
                                             "\nPara acceder a una poscición del arreglo, este debe ser el último en accederse\n");
                     }
                     expr = true;
                 }
             }
             if (expr)
             {
                 string[] aux = new string[designatorauxCount];
                 for (i = 0; i < aux.Length; i++)
                 {
                     aux[i] = designators[i];
                 }
                 return aux;
             }
             return designators;
         }
    
     }

	/// <summary>
	/// Visit a parse tree produced by the <c>condFactAST</c>
	/// labeled alternative in <see cref="Parser1.condFact"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitCondFactAST([NotNull] Parser1.CondFactASTContext context) {

        // expr relop expr		

        //visita a vcondfact:

        Tipo operador = (Tipo)Visit(context.relop()); // obtiene el tipo relop
        Tipo cero = (Tipo)Visit(context.expr(0)); // obtiene tipo expr0
        Tipo uno = (Tipo)Visit(context.expr(1)); // obtiene tipo expr1

        // verifica que sean de tipos de datos compatibles
        if (cero.nombre.Equals(uno.nombre))
        {
            return null;
        }
        else
        {
            throw new Exception("Error: El operador '" + operador.nombre + "' no se puede aplicar a operandos del tipo '" + cero.nombre + "' y '" + uno.nombre + "'. Línea: " + operador.linea + " columna: " + operador.columna +"\n");
        }

 
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>conditionAST</c>
	/// labeled alternative in <see cref="Parser1.condition"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitConditionAST([NotNull] Parser1.ConditionASTContext context) {

        //condTerm (O condTerm)*	

        int i;
        // en caso condTerm -> )*
        for (i = 0; i < context.condTerm().Length; i++)
        {
            Visit(context.condTerm(i));
        }
        return null; 
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>mulopAST</c>
	/// labeled alternative in <see cref="Parser1.mulop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitMulopAST([NotNull] Parser1.MulopASTContext context)
    {
        // (MUL | SLASH | PORCENT)	 

        //Crea el nuevo tipo -> * , almacena la linea y columna donde se encuentra
        if (context.MUL() != null) {
            return new Tipo(context.MUL().Symbol.Text, context.MUL().Symbol.Line, context.MUL().Symbol.Column);
        
        }
        //Crea el nuevo tipo -> / , almacena la linea y columna donde se encuentra
        else if (context.SLASH() != null)
        {
            return new Tipo(context.SLASH().Symbol.Text, context.SLASH().Symbol.Line, context.SLASH().Symbol.Column);
        
        }
        //Crea el nuevo tipo -> % , almacena la linea y columna donde se encuentra
        else
            return new Tipo(context.PORCENT().Symbol.Text, context.PORCENT().Symbol.Line, context.PORCENT().Symbol.Column);

            return null;
    }
	/// <summary>
	/// Visit a parse tree produced by the <c>forstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitForstaAST([NotNull] Parser1.ForstaASTContext context) {

        // FOR PIZQ expr PyCOMA (condition | ) PyCOMA (statement | ) PDER statement		 #forstaAST 

        // FOR PIZQ expr PYCOMA condition PCOMA statment PDER statment


        if (context.condition() != null && context.statement(0) != null)
        {

            Tipo expr = (Tipo)Visit(context.expr());

            //Verfica que la expresion sea de tipo int

            // (int=0; int<int; int++ )
            if (expr.nombre == "int")
            {
                Visit(context.condition());
                Visit(context.statement(0));

                // ---------------------------------- for ()  ___ 

                ContadorCiclos++;

                Visit(context.statement(1));
                ReturnMayor = false;
                
                ContadorCiclos--;

                return null;
            }
            else
            {
                throw new Exception("Error: Solo se aceptan expresiones de tipo 'int' Línea: " + expr.linea + " columna: " + expr.columna +"\n");
            }  

        
        }

        // FOR PIZQ expr PYCOMA  PCOMA  PDER statment

        else if (context.condition() == null && context.statement(0) == null)
        {
            Tipo expr = (Tipo)Visit(context.expr());

            //Verfica que la expresion sea de tipo int

            // (int=0; int<int; int++ )
            if (expr.nombre == "int")
            {
                //Visit(context.condition());
                //Visit(context.statement(0));


                // ---------------------------------- for ()  ___ 

                ContadorCiclos++;

                Visit(context.statement(1));

                ReturnMayor = false;
                ContadorCiclos--;

                return null;
            }
            else
            {
                throw new Exception("Error: Solo se aceptan expresiones de tipo 'int' Línea: " + expr.linea + " columna: " + expr.columna + "\n");
            } 
        
        }


        
        // FOR PIZQ expr PYCOMA condition PCOMA  PDER statment

        else if (context.condition() != null && context.statement(0) == null)
        {

            Tipo expr = (Tipo)Visit(context.expr());

            //Verfica que la expresion sea de tipo int

            // (int=0; int<int; int++ )
            if (expr.nombre == "int")
            {
                Visit(context.condition());
                //Visit(context.statement(0));


                // ---------------------------------- for ()  ___ 


                ContadorCiclos++;

                Visit(context.statement(1));

                ReturnMayor = false;
                ContadorCiclos--;

                return null;
            }
            else
            {
                throw new Exception("Error: Solo se aceptan expresiones de tipo 'int' Línea: " + expr.linea + " columna: " + expr.columna + "\n");
            } 



        }
        // FOR PIZQ expr PYCOMA  PCOMA statment PDER statment
        else if (context.condition() == null && context.statement(0) != null)
        {
            Tipo expr = (Tipo)Visit(context.expr());

            //Verfica que la expresion sea de tipo int

            // (int=0; int<int; int++ )
            if (expr.nombre == "int")
            {
                //Visit(context.condition());
                Visit(context.statement(0));


                // ---------------------------------- for ()  ___ 


                ContadorCiclos++;

                Visit(context.statement(1));


                ContadorCiclos--;

                return null;
            }
            else
            {
                throw new Exception("Error: Solo se aceptan expresiones de tipo 'int' Línea: " + expr.linea + " columna: " + expr.columna + "\n");
            } 

        }

        return null; 
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>designatorstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitDesignatorstaAST([NotNull] Parser1.DesignatorstaASTContext context) {

        //designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA	 

        //-----------------------------------------------------------------------------------------------  designator ASIGN expr PYC	

        // expr -> (MENOS | ) term ( addop term )*

        if (context.ASIGN() != null)
        {

            string[] designator = (string[])Visit(context.designator());  // visita designator, guarda lo q visito 

            Metodo Metodo;


            if (designator.Length == 1)
            {
                // No permite grupos de metodos
                if (Tabla.buscarMetodo(designator[0]))
                {
                    Metodo = Tabla.retornarMetodo(designator[0]);

                    throw new Exception("Error: No se puede asignar a '" + Metodo.nombre + "' porque es 'grupo de métodos' " + ", línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }
            }


            //int a = 8; -> int   

            Variable Variable;
            Tipo expr;   // el tipo que tendra expresion
            int i;

            // Asignación de variables 

            if (Tabla.buscarVariable(designator[0]))
            {
                Variable = Tabla.retornarVariable(designator[0]);

                for (i = 1; i < designator.Length; i++)
                {
                    Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);  // Obtiene la clase por el nombre -> Tipo!! 
                    Variable = Clase.retornarVar(designator[i]);  // Obtiene la variable  de acuerdo int <- a 
                }

                expr = (Tipo)Visit(context.expr());  // Castea el expr a tipo 

                // No permite asignaciones entre arreglos  [int a=1]
                if (Variable.Arreglo && Arreglo && !designatorExpression)
                {
                    throw new Exception("Error: No se pueden hacer asignaciones entre arreglos, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }

                if (Variable.Const == true)
                {
                    throw new Exception("Error: No se pueden modificar las variables const, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }

                // Tipos arreglos incompatibles
                if (!Variable.Arreglo && Arreglo2)
                {
                    throw new Exception("Error: Tipos incompatibles, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }

                // Inicializa las banderas de nuevo en false
                Arreglo = false;
                Arreglo2 = false;
                designatorExpression = false;

                // Verifica que la asignacion de variables sean de tipos compatibles 

                if ((Variable.tipo.nombre) == (expr.nombre) ||  // verifica nombres tipos iguales
                    (expr.nombre.Equals("null") && !Variable.tipo.nombre.Equals("int") && !Variable.tipo.nombre.Equals("char") &&
                    !Variable.tipo.nombre.Equals("bool") && !Variable.tipo.nombre.Equals("float")) ||  // si es null el exp la variable no puede ser de ningun tipo bool, char etc
                    (Variable.tipo.nombre.Equals("float") && expr.nombre.Equals("int")) )  // verifica que float no es lo mismo que int
                {
    
                    if (!Variable.nombre.Equals("const"))
                        return null;
                    throw new Exception("Error: no se puede asignar a const, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                
                }
                else  // si el if falla es xq no son iguales
                {
                    throw new Exception("Error: Asignación entre tipos incompatibles, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }
            }

            // Asignacion de variables en metodos

            // Obtiene el metodo y su parametro
            Metodo = Tabla.retornarMetodo(MetodoActual);
            Variable = Metodo.retornarParametroPorNombre(designator[0]);

            for (i = 1; i < designator.Length; i++)
            {
                Clase Clase = Tabla.retornarClase(Variable.tipo.nombre); // para obtener las variables
                Variable = Clase.retornarVar(designator[i]);
            }

            expr = (Tipo)Visit(context.expr());  

            // No permite asignaciones entre arreglos
            if (Variable.Arreglo && Arreglo && !designatorExpression)
            {
                throw new Exception("Error: No se pueden hacer asignaciones entre arreglos, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
            }

            // No iguala arreglo a otro arreglo
            if (!Variable.Arreglo && Arreglo)
            {
                throw new Exception("Error: Tipos incompatibles, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
            }

            // Inicializa las banderas
            Arreglo = false;
            designatorExpression = false;

            if ((Variable.tipo.nombre) == (expr.nombre) ||
                (expr.nombre.Equals("null") &&
                !Variable.tipo.nombre.Equals("int") &&
                !Variable.tipo.nombre.Equals("char") &&
                !Variable.tipo.nombre.Equals("bool") &&
                !Variable.tipo.nombre.Equals("float")) ||
                (Variable.tipo.nombre.Equals("float") &&
                expr.nombre.Equals("int")))
            {
                if (!Variable.nombre.Equals("const"))
                    return null;
                throw new Exception("Error: no se puede asignar a const, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                
            }
            else // si el if falla es xq no son iguales
            {
                throw new Exception("Error: Asignación entre tipos incompatibles,  línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
            }
        }


             //designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA	 

            //------------------------------------------------------------------------------>>>>  designator PIZQ ( actPars | ) PDER PYC	

            // j.y(actpars); 

            else if (context.PIZQ() != null){


                string[] designator = (string[])Visit(context.designator());

                if (designator.Length > 1)
                {
                    throw new Exception("Error: El método no existe en el contexto actual,  línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                }


                string ID = designator[0];

                // Si el metodo no esta en la tabla, error
                if (!Tabla.buscarMetodo(ID))
                {
                    throw new Exception("Error: El nombre '" + ID + "' no existe en el contexto actual  línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                }



                if (context.actPars() != null)
                {
                    //ACTPARS

                    //obtiene el metodo
                    Metodo Metodo = Tabla.retornarMetodo(ID);
                    List<Tipo> Parametros = (List<Tipo>)Visit(context.actPars()); // obtiene los parametros en lista si son varios

                    // Tiene mas parametros que los establecidos en el metodo
                    if (Parametros.Count != Metodo.cantidadParametros())
                    {
                        throw new Exception("Error Ninguna sobrecarga para el método '" + ID + "' toma '" + Parametros.Count + "' argumentos. línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                    }

                    int i;

                    for (i = 0; i < Parametros.Count; i++)
                    {
                        // Los parametros no sean tipos -> (String String)
                        if (Parametros[i].nombre != Metodo.retornarParamPos(i).tipo.nombre)
                        {
                            if (!ID.Equals("len"))
                            {
                                throw new Exception("Error: La mejor coincidencia de método sobrecargado para '" + ID + "' tiene algunos argumentos no válidos, línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                            }
                        }
                    }

                    return null;
                }

                
                else {

                    //vacio 


                    if (Tabla.buscarMetodo(ID))
                    {
                        // sin parametros
                        if (Tabla.retornarMetodo(ID).cantidadParametros() != 0)
                        {
                            throw new Exception("Error: Ninguna sobrecarga para el método '" + designator[0] + "' toma '" + 0 + "' argumentos. Línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                        }
                        return null;
                    }
                    else
                    {
                        throw new Exception("Error: El nombre '" + ID + "' no existe en el contexto actual, línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                    }
                }
  
            }  // cierra else if 

        //designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA	 

        //------------------------------------------------------------------------------>>>>  designator MASMAS PYC	

        else if (context.MASMAS() != null)
        {
            string[] designator = (string[])Visit(context.designator());

            if (designator.Length == 1)
            {
                // Busca que exista el metodo, para que no le haga ++ al parametro de ese metodo :D
                if (Tabla.buscarMetodo(designator[0]))

                    // asd.asd+++ // asd(ex++)
                {
                    throw new Exception("Error: El operador '++' no se puede aplicar al operando del tipo 'grupo de métodos'.  Línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                }
            }
            // Si no es metodo, es variable, obtiene la variable de la tabla
            Variable Variable = Tabla.retornarVariable(designator[0]);

            int i;

            // Obtiene el tipo de la variable, devuelve la variable de ese tipo
            for (i = 1; i < designator.Length; i++)
            {
                Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);
                Variable = Clase.retornarVar(designator[i]);
            }

            // Solo puede hacer ++ a tipos int
            if (Variable.tipo.nombre == "int")
            {
                return null;
            }
            else
            {
                throw new Exception("Error: El operando de un operador de incremento o decremento debe ser una variable de tipo 'int', línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
            }

        } // cierra else if 

        //designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA	 

        //------------------------------------------------------------------------------>>>>  designator MENOSMENOS PYC	

        else if (context.MENOSMENOS()!=null){

            string[] designator = (string[])Visit(context.designator());

            if (designator.Length == 1)
            {
                // Busca que exista el metodo, para que no le haga -- al parametro de ese metodo :D
                if (Tabla.buscarMetodo(designator[0]))

                // asd.as-- // asd(ex--)
                {
                    throw new Exception("Error: El operador '--' no se puede aplicar al operando del tipo 'grupo de métodos'.  Línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }
            }
            // Si no es metodo, es variable, obtiene la variable de la tabla
            Variable Variable = Tabla.retornarVariable(designator[0]);

            int i;

            // Obtiene el tipo de la variable, devuelve la variable de ese tipo
            for (i = 1; i < designator.Length; i++)
            {
                Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);
                Variable = Clase.retornarVar(designator[i]);
            }

            // Solo puede hacer ++ a tipos int
            if (Variable.tipo.nombre == "int")
            {
                return null;
            }
            else
            {
                throw new Exception("Error: El operando de un operador de incremento o decremento debe ser una variable de tipo 'int', línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
            }

        }// cierre else if



        return null;
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>whilestaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitWhilestaAST([NotNull] Parser1.WhilestaASTContext context) {

        //WHILE PIZQ condition PDER statement	 

        Visit(context.condition());
        ContadorCiclos++;
        Visit(context.statement());
        ContadorCiclos--;
        ReturnMayor = false;

        return null; 
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>returnstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitReturnstaAST([NotNull] Parser1.ReturnstaASTContext context) {

        //RETURN ( expr | ) PyCOMA

        Metodo Metodo = Tabla.retornarMetodo(MetodoActual);

        Tipo expr = (Tipo)Visit(context.expr());

        if (context.expr() != null)
        {


            if ((Metodo.tipo.nombre.Equals("int") ||
                Metodo.tipo.nombre.Equals("char") ||
                Metodo.tipo.nombre.Equals("bool") ||
                Metodo.tipo.nombre.Equals("float")) &&
                "null".Equals(expr.nombre) ||
                (Metodo.tipo.nombre.Equals("void") ||
                (!Metodo.tipo.nombre.Equals(expr.nombre) &&
                !"null".Equals(expr.nombre))))
            {
                throw new Exception("Error: El tipo de retorno es diferente al de método actual. Línea: " + context.RETURN().Symbol.Line + " columna: " + context.RETURN().Symbol.Column + "\n");
            }
            Return = true; /// esq este es el del if o.o 
            ReturnMayor = true;  // esta es la otra variable para ver q tenga return el metodo sin los ifs xD

            return null;


        }// cierra if

        else {


            if (Metodo.tipo.nombre != ("void"))
            {
                throw new Exception("Error:El tipo de retorno es diferente al de método actual. Línea: " + context.RETURN().Symbol.Line + " columna: " + context.RETURN().Symbol.Column +"\n");
            }
            Return = true;  // para manejo de errores en return 
            ReturnMayor = true;

            return null;
        }



    }

	/// <summary>
	/// Visit a parse tree produced by the <c>pycomastaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitPycomastaAST([NotNull] Parser1.PycomastaASTContext context) { 
        

        return null; 
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>foreachstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitForeachstaAST([NotNull] Parser1.ForeachstaASTContext context) {

        //  FOREACH PIZQ type ID IN expr PDER	statement										 #foreachstaAST

        Tipo tipo1 = (Tipo)Visit(context.type());
        Tipo tipo2 = (Tipo)Visit(context.expr());

        // verifica el type del id sea del mismo tipo que el expre
        if (!tipo1.nombre.Equals(tipo2.nombre))
        {

            throw new Exception("Error: Tipos incompatibles en: '"+context.ID().Symbol.Text+"', línea: " + tipo1.linea + " columna: " + tipo1.columna + "\n");
        }
        ContadorCiclos++;
        Visit(context.statement());
        ContadorCiclos--;

        return null; 
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>breakAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitBreakAST([NotNull] Parser1.BreakASTContext context)
    {

        // BREAK PyCOMA

        if (ContadorCiclos > 0)
        {
            return null;
        }
        else
        {
            throw new Exception("Error: No hay ningún bucle envolvente desde el que interrumpir o continuar. Línea: " + context.BREAK().Symbol.Line + " columna: " + context.BREAK().Symbol.Column +"\n");
        }

        //return null;
    }
	/// <summary>
	/// Visit a parse tree produced by the <c>readstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitReadstaAST([NotNull] Parser1.ReadstaASTContext context) {

        //READ PIZQ designator PDER PyCOMA												 #readstaAST

        string[] designator = (string[])Visit(context.designator());

        //metodo
        if (designator.Length == 1)
        {
            //Si el read esta un metodo, error
            if (Tabla.buscarMetodo(designator[0]))
            {
                throw new Exception("Error: No se puede hacer read en un metodo. Línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
            }
        }

        return null;  
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>ifstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitIfstaAST([NotNull] Parser1.IfstaASTContext context) {

        //IF PIZQ condition PDER statement ( ELSE statement | )							 #ifstaAST

        
        Visit(context.condition());
        Visit(context.statement(0));


        //-------------------------------------------------------------------------->>> IF PIZQ condition PDER statement (ELSE statment)
        if (context.ELSE() != null)
        {
            //ELSE

            Visit(context.statement(1));
            return null;


        }

        //-------------------------------------------------------------------------->>> IF PIZQ condition PDER statement ()
        else { 
            //VACIO 
            ReturnMayor = false;
            return null;
 
        }
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>writestaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitWritestaAST([NotNull] Parser1.WritestaASTContext context) {

        //WRITE PIZQ expr ( COMA NUM | ) PDER PyCOMA									 #writestaAST 

        // ------------------------------------------------------------------->>>>>> WRITE PIZQ expr  COMA NUM PDER PyComa

        if (context.COMA() != null)
        {

            Visit(context.expr());
            return null;
        }

        // ------------------------------------------------------------------->>>>>> WRITE PIZQ expr PDER PyComa

        else {
            Visit(context.expr());
            return null;
        
        }
        
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>blockstaAST</c>
	/// labeled alternative in <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitBlockstaAST([NotNull] Parser1.BlockstaASTContext context) {
        Visit(context.block());
        return null;

    }

	/// <summary>
	/// Visit a parse tree produced by the <c>blockAST</c>
	/// labeled alternative in <see cref="Parser1.block"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitBlockAST([NotNull] Parser1.BlockASTContext context) {

        //CORCHETEIZQ (statement)* CORCHETEDER 

        int i;
        for (i = 0; i < context.statement().Length; i++)
        {
            Visit(context.statement(i));
        }

        return null;
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>exprAST</c>
	/// labeled alternative in <see cref="Parser1.expr"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitExprAST([NotNull] Parser1.ExprASTContext context) {


        //(MENOS | ) term ( addop term )*		

        if (context.MENOS() != null)
        {
            //MENOS
            int i;
            for (i = 0; i < context.term().Length; i++)
            {
                Tipo term = (Tipo)Visit(context.term(i));
                // Term tiene q ser int o float para el addop 
                if (term.nombre != "int" && term.nombre != "float")
                {
                    throw new Exception("Error: El operador '" + (string)Visit(context.addop(i)) + "' solo se puede aplicar a operandos de tipos numéricos, línea: " + term.linea + " columna: " + term.columna + "\n");
                }
            }

            // Crea el nuevo tipo, le asigna la linea y columna donde se encuentra
            return new Tipo("int", context.MENOS().Symbol.Line, context.MENOS().Symbol.Column);

        }
        else
        {
            //VACIO

            Tipo term = (Tipo)Visit(context.term(0));
            if (context.term().Length > 1)
            {
                Tipo termp;
                int i;
                for (i = 0; i < context.term().Length; i++)
                {
                    termp = (Tipo)Visit(context.term(i));
                    if (termp.nombre != "int" && termp.nombre != "float")
                    {
                        throw new Exception("Error, línea: " + termp.linea + " columna: " + termp.columna +
                                            "\nEl operador '" + (string)Visit(context.addop(i)) + "' solo se puede aplicar a operandos de tipos numéricos\n");
                    }
                }
            }
            return term;
        }

        //return null;
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>termAST</c>
	/// labeled alternative in <see cref="Parser1.term"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitTermAST([NotNull] Parser1.TermASTContext context) { 
        
        //factor ( mulop factor)*	

        Tipo factor = (Tipo)Visit(context.factor(0));
        if (context.factor().Length > 1)
        {
            Tipo factomp;
            int i;

            // En caso de -> (mulop factor)*
            for (i = 0; i < context.factor().Length; i++)
            {
                factomp = (Tipo)Visit(context.factor(i));
                // Verifica para mulop que el factor sea de tipo int o float
                if (factomp.nombre != "int" && factomp.nombre != "float")
                {
                    throw new Exception("Error:El operador '" + (string)Visit(context.mulop(i)) + "' solo se puede aplicar a operandos de tipos numéricos. Línea: " + factomp.linea + " columna: " + factomp.columna +"\n");
                }
            }
        }
        return factor;
 
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>lenAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    //public override object VisitLenAST([NotNull] Parser1.LenASTContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by the <c>truefalseFactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitTruefalseFactorAST([NotNull] Parser1.TruefalseFactorASTContext context) {

        // num -> retorna el tipo
        if (context.TRUE() != null)
        {
            return new Tipo("bool", context.TRUE().Symbol.Line, context.TRUE().Symbol.Column);
        }
        else
            return new Tipo("bool", context.FALSE().Symbol.Line, context.FALSE().Symbol.Column);
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>stringAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitStringAST([NotNull] Parser1.StringASTContext context) { 
        
        
          // string -> retorna el tipo
        return new Tipo("string", context.STRING().Symbol.Line, context.STRING().Symbol.Column);
        
        }

	/// <summary>
	/// Visit a parse tree produced by the <c>designatorfactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitDesignatorfactorAST([NotNull] Parser1.DesignatorfactorASTContext context) {

        //designator (PIZQ (actPars | ) PDER | )	


        // ---------------------------------------------------------------------------->>>>>> designator PIZQ actpars PDER


        string[] designator = (string[])Visit(context.designator());

        if (context.PIZQ()!=null && context.actPars()!=null){

            // metodo
            if(designator.Length==1){

                if (Tabla.buscarMetodo(designator[0]))
                {
                    Metodo Metodo = Tabla.retornarMetodo(designator[0]);
                    List<Tipo> Parametros = (List<Tipo>)Visit(context.actPars());  // ---> visita actpars
                    // Tiene que tener la misma cantidad de parametros que el metodo
                    if (Parametros.Count != Metodo.cantidadParametros())
                    {
                        throw new Exception("Error: Ninguna sobrecarga para el método '" + designator[0] + "' toma '" + Parametros.Count + "' argumentos. Línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                    }
                    int i;
                    for (i = 0; i < Parametros.Count; i++)
                    {
                        if (Parametros[i].nombre != Metodo.retornarParamPos(i).tipo.nombre)
                        {
                            //No puede hacerle len
                            if (!designator[0].Equals("len"))
                            {
                                throw new Exception("Error: La mejor coincidencia de método sobrecargado para '" + designator[0] + "' tiene algunos argumentos no válidos línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                            }
                        }
                    }
                    return Metodo.tipo;
                }
            }

            throw new Exception("Error: Debe ser un metodo. Línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
            }

            // ---------------------------------------------------------------------------->>>>>> designator PIZQ PDER	
        else if(context.PIZQ()!=null && context.actPars()==null){

            //metodo
            if (designator.Length == 1)
            {
                // busca en la tabla de simbolos que exista el metodo
                if (Tabla.buscarMetodo(designator[0]))
                {
                    Metodo Metodo = Tabla.retornarMetodo(designator[0]);
                    // metodo () -> parametros ==0
                    if (Metodo.cantidadParametros() != 0)
                    {
                        throw new Exception("Error: Ninguna sobrecarga para el método '" + designator[0] + "' toma '" + 0 + "' argumentos. línea: " + designatorLinea + " columna: " + designatorColumna +"\n");
                    }
                    return Metodo.tipo;
                }
            }

            throw new Exception("Error: Debe ser un metodo línea: " + designatorLinea + " columna: " + designatorColumna +"\n");  
        }
            // ---------------------------------------------------------------------------->>>>>> designator 
        else if (context.PIZQ()==null&&context.actPars()==null){
           // nuevo designator
            int i;
            Variable Variable;

            //metodo
            if (designator.Length == 1)
            {
                // no metodo
                if (Tabla.buscarMetodo(designator[0]))
                {
                    throw new Exception("Error: Debe ser una variable o el miembro de una variable línea: " + designatorLinea + " columna: " + designatorColumna + "\n");
                }
            }

            // busca la var en la tabla de simbolos y devuelve el tipo
            if (Tabla.buscarVariable(designator[0]))
            {
                // obtiene la variable
                Variable = Tabla.retornarVariable(designator[0]);

                for (i = 1; i < designator.Length; i++)
                {
                    Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);
                    Variable = Clase.retornarVar(designator[i]);
                }

                Arreglo = Variable.Arreglo;  // No arreglo!! 

                return Variable.tipo;
            }

            // busca la variable en los params del metodo, devuelve el tipo
            Metodo Metodo = Tabla.retornarMetodo(MetodoActual);
            Variable = Metodo.retornarParametroPorNombre(designator[0]);

            for (i = 1; i < designator.Length; i++)
            {
                Clase Clase = Tabla.retornarClase(Variable.tipo.nombre);
                Variable = Clase.retornarVar(designator[i]);
            }

            Arreglo = Variable.Arreglo; // NO es arreglo!! 

            return Variable.tipo;
        
        }
        else 
            return null; 
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>numFactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitNumFactorAST([NotNull] Parser1.NumFactorASTContext context) {

        // num -> retorna el tipo
        return new Tipo("int", context.NUM().Symbol.Line, context.NUM().Symbol.Column);
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>newidFactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitNewidFactorAST([NotNull] Parser1.NewidFactorASTContext context) {


        //NEW ID ( PCIZQ expr PCDER | )

        // ------------------------------------------------------------------------------->>>>>>>>>>>> NEW ID PCIZQ expr PCDER

        if (context.PCIZQ() != null) {

             // PCIZQ -> [ 
            Arreglo2 = true; // ES ARREGLO!! 

            //Obtiene el tipo del id
            Tipo ID = new Tipo(context.ID().Symbol.Text, context.ID().Symbol.Line, context.ID().Symbol.Column);

            //Obtiene el tipo del expr
            Tipo expr = (Tipo)Visit(context.expr());

            // los expr tienen q ser ints
            if (!expr.nombre.Equals("int"))
            {
                throw new Exception("Error: Solo se aceptan expresiones del tipo entero línea: " + expr.linea + " columna: " + expr.columna +"\n");
            }
            // [] chars o int 
            if ((ID.nombre == "int") || (ID.nombre == "char"))
            {
                return ID;
            }
            else
            {
                throw new Exception("Error: El nombre '" + ID.nombre + "' no existe en el contexto actual. Línea: " + ID.linea + " columna: " + ID.columna +"\n");
            }
        
        }// cierra if

        else if (context.PCIZQ()==null){
            //Obtiene el tipo del id
            // ------------------------------------------------------------------------------->>>>>>>>>>>> NEW ID PCIZQ expr PCDER

            Tipo ID = new Tipo(context.ID().Symbol.Text, context.ID().Symbol.Line, context.ID().Symbol.Column);
            if (Tabla.buscarClase(ID.nombre))  // hace new id, el id tiene q estar en la tabla de simbolos
            {
                return ID;
            }
            else
            {
                throw new Exception("Error: No se puede encontrar el tipo '" + ID.nombre + "'. Línea: " + ID.linea + " columna: " + ID.columna +"\n");
            }

        }

        return null;
    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>charconstFactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitCharconstFactorAST([NotNull] Parser1.CharconstFactorASTContext context) {


        // char -> retorna el tipo
        return new Tipo("char", context.CHARCONST().Symbol.Line, context.CHARCONST().Symbol.Column);

    
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>pizqExpreFactorAST</c>
	/// labeled alternative in <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitPizqExpreFactorAST([NotNull] Parser1.PizqExpreFactorASTContext context) {
        //PIZQ expr PDER 
        // solo hace el visit a expr
        return Visit(context.expr());
    }

	/// <summary>
	/// Visit a parse tree produced by the <c>varDeclAST</c>
	/// labeled alternative in <see cref="Parser1.varDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitVarDeclAST([NotNull] Parser1.VarDeclASTContext context) { 
        
        //type ID (COMA ID)* PyCOMA

        

        Tipo tipo = (Tipo)Visit(context.type());

        int idCount = context.ID().Length;

        if (Tabla.buscarClase(tipo.nombre)) {
            

            // ContextoActual = Niveles, ubicarse entre bloques  

            // 0 globles, 1 bloques, 2 params
            switch (ContextoActual) 
            { 
            
                // Variable global -> nivel 0
                case 0:
                    
                        int i;

                        for (i = 0; i < idCount; i++)
                        {
                            //char char () a, b;
                            //ClaseActual = context.ID(i).Symbol.Text;  // Clase Actual -> para declaracion de variables

                            string ID = context.ID(i).Symbol.Text;

                            if (!Tabla.buscarClase(ID) &&  // para que no venga char char 
                                !Tabla.buscarMetodo(ID) && // para que no declare un metodo
                                !Tabla.buscarVariable(ID)) // no este repetida la variable
                            {
                                Variable Variable = new Variable(ID, tipo, 0); // Variable nivel 0
                                Variable.Arreglo = Arreglo;
                                Tabla.insertarVariable(Variable); // Inserta la nueva variable a la tabla de simbolos
                                Arreglo = false;
                            }
                            else
                            {
                                throw new Exception("Error: El tipo '" + ClaseActual + "' ya contiene una definición para '" + ID + " , Línea: " + context.ID(i).Symbol.Line + " columna: " + context.ID(i).Symbol.Column + "'\n");
                            }
                        }
                       // ClaseActual = null;
                    
                    break;
                    
                //Variable local (dentro de un metodo) 
                    /*public void a (){
                        int ASD;
                    };
                     */
                case 1:
                    int j;

                    for (j = 0; j < idCount; j++)
                    {
                        string ID = context.ID(j).Symbol.Text;
                        //ClaseActual = context.ID(j).Symbol.Text;  // Clase Actual -> para declaracion de variables

                        // pregunta si clase clase actual es del mismo tipo de ID 
                        //Clase actual puede ser: int, char, bool, float

                        if (ClaseActual.Equals(ID) || // evita ejm: char char
                            Tabla.retornarClase(ClaseActual).buscarVar(ID))  // Obtiene la claseActual y si ya exista la variable error
                        {
                            throw new Exception("Error: tipo '" + ClaseActual + "' ya contiene una definición para '" + ID +", línea: " + context.ID(j).Symbol.Line + " columna: " + context.ID(j).Symbol.Column + "'\n");
                        }

                        Variable var = new Variable(ID, tipo, 1); // Variable nivel 1. 

                        // Además los arreglos solamente pueden ser de enteros o caracteres; NO DE CLASES              

                        if (Arreglo)
                        {
                            throw new Exception("Error:No se permiten declaraciones de arreglos dentro de una clase" + ", línea: " + context.ID(j).Symbol.Line + " columna: " + context.ID(j).Symbol.Column + "\n");
                        }

                        var.Arreglo = Arreglo;
                        Tabla.retornarClase(ClaseActual).insertarVar(var);  //Inserta la variable a la tabla de simbolos
                        Arreglo = false;
                    }
                   // ClaseActual = null;
                    break;


                // Variables dentro de metodos -> params  public void asd (String asd2){};
                case 2:
                    int k;

                    for (k = 0; k < idCount; k++)
                    {
                        string ID = context.ID(k).Symbol.Text;
                      //  ClaseActual = context.ID(k).Symbol.Text;  // Clase Actual -> para declaracion de variables

                        if (MetodoActual.Equals(ID) ||   // Metodo repetido 
                            Tabla.retornarMetodo(MetodoActual).buscarParametro(ID))  //Parametro repetido
                        {
                            throw new Exception("Error: El tipo '" + ClaseActual + "' ya contiene una definición para '" + ID + ", línea: " + context.ID(k).Symbol.Line + " columna: " + context.ID(k).Symbol.Column + "'\n");
                        }

                        if (Tabla.buscarVariable(ID))
                        {
                            throw new Exception("Error, línea: " + context.ID(k).Symbol.Line + " columna: " + context.ID(k).Symbol.Column +
                                                "\n Ya contiene una definición para '" + ID + "'\n");
                        }

                        Variable Variable = new Variable(ID, tipo, 1); // Variable nivel 1
                        Variable.Arreglo = Arreglo;
                        Tabla.insertarVariable(Variable);
                        Arreglo = false;
                    }
                    //ClaseActual=null;
                    break;
            }
        }
        else
        {
            throw new Exception("Error: línea: " + tipo.linea + " columna: " + tipo.columna +
                                "\nNo se puede encontrar el tipo '" + tipo.nombre + "'\n");
        }

        return null;
        
    }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.program"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitProgram([NotNull] Parser1.ProgramContext context) { return VisitChildren(context); }

    /// <summary>
    /// Visit a parse tree produced by the <c>varDeclaAuxAST</c>
    /// labeled alternative in <see cref="Parser1.decla"/>.
    /// <para>
    /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
    /// on <paramref name="context"/>.
    /// </para>
    /// </summary>
    /// <param name="context">The parse tree.</param>
    /// <return>The visitor result.</return>
    public override object VisitVarDeclaAuxAST([NotNull] Parser1.VarDeclaAuxASTContext context) {

        Visit(context.varDecl());

        return null; 
    }

    /// <summary>
    /// Visit a parse tree produced by the <c>classDeclaAuxAST</c>
    /// labeled alternative in <see cref="Parser1.decla"/>.
    /// <para>
    /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
    /// on <paramref name="context"/>.
    /// </para>
    /// </summary>
    /// <param name="context">The parse tree.</param>
    /// <return>The visitor result.</return>
    public override object VisitClassDeclaAuxAST([NotNull] Parser1.ClassDeclaAuxASTContext context) {
        
        Visit(context.classDecl());

        return null; 
    }


    /// <summary>
    /// Visit a parse tree produced by the <c>constDeclaAST</c>
    /// labeled alternative in <see cref="Parser1.decla"/>.
    /// <para>
    /// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
    /// on <paramref name="context"/>.
    /// </para>
    /// </summary>
    /// <param name="context">The parse tree.</param>
    /// <return>The visitor result.</return>
    public override object VisitConstDeclaAST([NotNull] Parser1.ConstDeclaASTContext context) {
        
        Visit(context.constDecl());

        return null; 
    }


	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.constDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitConstDecl([NotNull] Parser1.ConstDeclContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.varDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitVarDecl([NotNull] Parser1.VarDeclContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.classDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitClassDecl([NotNull] Parser1.ClassDeclContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.methodDecl"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitMethodDecl([NotNull] Parser1.MethodDeclContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.formPars"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitFormPars([NotNull] Parser1.FormParsContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.type"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitType([NotNull] Parser1.TypeContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.statement"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitStatement([NotNull] Parser1.StatementContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.block"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitBlock([NotNull] Parser1.BlockContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.actPars"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	 public override object VisitActPars([NotNull] Parser1.ActParsContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.condition"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	 public override object VisitCondition([NotNull] Parser1.ConditionContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.condTerm"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	 public override object VisitCondTerm([NotNull] Parser1.CondTermContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.condFact"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitCondFact([NotNull] Parser1.CondFactContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.expr"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitExpr([NotNull] Parser1.ExprContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.term"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitTerm([NotNull] Parser1.TermContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.factor"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitFactor([NotNull] Parser1.FactorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.designator"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitDesignator([NotNull] Parser1.DesignatorContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.desig2"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	 public override object VisitDesig2([NotNull] Parser1.Desig2Context context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.relop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitRelop([NotNull] Parser1.RelopContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.addop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitAddop([NotNull] Parser1.AddopContext context) { return VisitChildren(context); }

	/// <summary>
	/// Visit a parse tree produced by <see cref="Parser1.mulop"/>.
	/// <para>
	/// The default implementation returns the result of calling <see cref="AbstractParseTreeVisitor{Result}.VisitChildren(IRuleNode)"/>
	/// on <paramref name="context"/>.
	/// </para>
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
    public override object VisitMulop([NotNull] Parser1.MulopContext context) { return VisitChildren(context); }
}
} // namespace PrograCompi


