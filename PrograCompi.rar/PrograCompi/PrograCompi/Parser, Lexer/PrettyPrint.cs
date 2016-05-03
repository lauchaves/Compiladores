using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using PrograCompi;
//using System.Windows.Forms;


class PrettyPrint : Parser1BaseVisitor<Object>
{

    //Form1.errores

    //private TextBoxBase contenedor;
    //private Object contenedor;
    private int cont;

    public PrettyPrint(Object container)
    {
        //container traia el objeto
        //contenedor = (TextBoxBase)container;
        //contenedor = null;
        cont = 0;
    }

    public void print( String what, bool nl)
    {
        Form1.errores.Append( what+" " );
        /*
        if (where != null)
        {
            /*    if (where is TextBoxBase)
                {
                    if (nl)
                        ((TextBoxBase) where).AppendText(what+"\n");
                    else
                        ((TextBoxBase) where).AppendText(what);
                }
                else
                    Console.WriteLine("Error al imprimir!!!");*

        }
        else
        {
            if (nl)
                Console.WriteLine(what);
            else
                Console.Write(what);
        }
*/

    }

    public void printtab(int n)
    {

        for (int num = n; num != 0; num--)
            print( "\t|", false);
        print( ">", false);
    }





    public override Object VisitClassDeclAST([NotNull] Parser1.ClassDeclASTContext context) {
        printtab(cont);
        print(context.GetType().Name, true);
        print(context.CLASS().ToString(), true);
        print(context.ID().ToString(), true);
        print("{"+"\n", true);

        cont++;

        if (context.varDecl(0) != null)
            Visit(context.varDecl(0)); //cuando hay ciclos se manda parametro inicia en 0 
        for (int i = 1; i <= context.varDecl().Length - 1; i++)
        {
            Visit(context.varDecl(i));
        }
        cont--;
        printtab(cont);
        print("}"+"\n", true); 


        return null; 
    }

    public override Object VisitCondTermAST([NotNull] Parser1.CondTermASTContext context) {

        //condFact (Y condFact)*	
        Visit(context.condFact(0));

        if (context.Y(0) != null)
        {
            print(context.Y(0).ToString(),true);
            Visit(context.condFact(1));
            for (int i = 1; i <= context.Y().Length - 1; i++)
            {
                print(context.Y(i).ToString(), true);
                Visit(context.condFact(i + 1));
            }
        }
        
        return null;
    
    }

    public override object VisitConstDeclNumAST([NotNull] Parser1.ConstDeclNumASTContext context) {
        printtab(cont);
        print(context.GetType().Name, true);
        print(context.CONST().ToString(), true);
        Visit(context.type());
        print(context.ID().ToString(), true); //Print "ID"
        print(context.ASIGN().ToString(), true); //Print "="

        print(context.NUM().ToString(), true); //Print CHARCONST

        print(context.PyCOMA().ToString() + "\n", true);

        return null;    }

    public override Object VisitConstDeclCharAST([NotNull] Parser1.ConstDeclCharASTContext context) {

        //CONST type ID ASIGN (NUM | CHARCONST) PyCOMA	 

        printtab(cont);
        print(context.GetType().Name, true);
        print(context.CONST().ToString(), true);
        Visit(context.type());
        print(context.ID().ToString(), true); //Print "ID"
        print(context.ASIGN().ToString(), true); //Print "="

        //if (context.NUM() == null)
          //  print(context.CHARCONST().ToString(), true); //Print CHARCONST
        //else
        print(context.CHARCONST().ToString(), true); // Print NUM

        print(context.PyCOMA().ToString()+"\n", true); //Print ";"

        return null;
    }


    public override Object VisitProgramAST([NotNull] Parser1.ProgramASTContext context) {
        printtab(cont);
        print(context.GetType().Name, true); //Print "PROGRAM"
        print( "CLASS", true);               //Print "CLASS"
        print( context.ID().ToString()+"\n", true); //Print "ID"

        cont++;

       // { ConstDecl | VarDecl | ClassDecl } "{" { MethodDecl } "}"

        Visit(context.decla(0)); //cuando hay ciclos se manda parametro inicia en 0 
        for (int i = 1; i <= context.decla().Length - 1; i++)
        {
            Visit(context.decla(i));
        }

        print( "{"+"\n", true);
        if (context.methodDecl(0) != null)
        {
            Visit(context.methodDecl(0)); //cuando hay ciclos se manda parametro inicia en 0 
        }
        for (int i = 1; i <= context.methodDecl().Length - 1; i++)
        {
            Visit(context.methodDecl(i));
        }
        print( "}", true);


        cont--;

        return null;
            
    }

     

    public override Object VisitMethodDeclAST([NotNull] Parser1.MethodDeclASTContext context)
    {
        //(type | VOID) ID PIZQ (formPars | ) PDER (varDecl)* block
        printtab(cont);
        print(context.GetType().Name, true);
        if (context.type() != null) 
            Visit(context.type()); // Print type
        else
            print(context.VOID().ToString(), true); // Print void

        print(context.ID().ToString(), true); //Print "ID"
        print("(", true); //Print "("


        if (context.formPars() != null)
            Visit(context.formPars());
        print(")"+"\n", true); //Print ")"

        cont++;


        if (context.varDecl(0) != null)
        {
            Visit(context.varDecl(0));
            for (int i = 1; i <= context.varDecl().Length - 1; i++)
                {
                    Visit(context.varDecl(i));
                }
        }

        

        cont--;

        cont++;
        printtab(cont);
        Visit(context.block());
        cont--;

        return null;
    }
    public override Object VisitVarDeclaAuxAST([NotNull] Parser1.VarDeclaAuxASTContext context)
    {
        
        VisitChildren(context);

        cont++;
        cont--; 

        return null;
    }


    public override object VisitConstDeclaAST([NotNull] Parser1.ConstDeclaASTContext context) {

        VisitChildren(context);

        cont++;
        cont--;

        return null;
    
    
    }

    public override object VisitClassDeclaAuxAST([NotNull] Parser1.ClassDeclaAuxASTContext context) {
        VisitChildren(context);

        cont++;
        cont--;

        return null;
    }


    public override Object VisitTypeAST([NotNull] Parser1.TypeASTContext context) {


        //ID ( PCIZQ PCDER | ) 
        //print(context.GetType().Name, true);
        print(context.ID().ToString(), true); //Print "ID"

        if (context.PCIZQ() != null)
        {
            print(context.PCIZQ().ToString(), true); //Print "["
            print(context.PCDER().ToString(), true); //Print "]"
        }
        return null;
    }

    public override Object VisitFormParsAST([NotNull] Parser1.FormParsASTContext context) {
        //type ID (COMA type ID)*	
        print(context.GetType().Name, true);
        Visit(context.type(0));
        print(context.ID(0).ToString(), true); //Print "ID"
        if (context.COMA(0) != null)
        {
            print(context.COMA(0).ToString(), true);
            if (context.type(1) != null)
            {
                Visit(context.type(1));
                if (context.ID(1) != null)
                {
                    print(context.ID(1).ToString(), true);
                    for (int i = 1; i < context.COMA(0).ToString().Length - 1; i++)
                    {
                        print(context.COMA(i + 1).ToString() + " ", true); //Print ","
                        Visit(context.type(i + 1));
                        print(context.ID(i + 1).ToString(), true); //Print "ID"
                    }
                }
            }
        }

        return null;
    }

    public override Object VisitActParsAST([NotNull] Parser1.ActParsASTContext context) {

        // 	expr (COMA expr)*	 

        Visit(context.expr(0));
        if (context.COMA() != null)
        {
            print(context.COMA().ToString(), true);
            if (context.expr(1) != null)
                Visit(context.expr(1));
            for (int i = 1; i <= context.COMA().Length - 1; i++)
            {
                print(context.COMA(i).ToString(), true);
                Visit(context.expr(i + 1));
            }
        }

        return null;
    }


    public override Object VisitDesignatorAST([NotNull] Parser1.DesignatorASTContext context) {

        //ID (desig2)* 

        //printtab(cont);
        //print(context.GetType().Name, true);
        print(context.ID().ToString(), true);
        if(context.desig2(0)!=null){
            Visit(context.desig2(0));
            for (int i = 1; i <= context.desig2().Length - 1; i++)
            {
                Visit(context.desig2(i));
            }
        }
        
        return null; 
    }

    
    public override Object VisitDesig2AST([NotNull] Parser1.Desig2ASTContext context) {
        // (PUNTO ID | PCIZQ expr PCDER)


        if (context.PUNTO() != null)
        {
            print(context.PUNTO().ToString(), true);
            print(context.ID().ToString(), true);
        }
        else
        {
            print(context.PCIZQ().ToString(), true);
            Visit(context.expr());
            print(context.PCDER().ToString(), true);
        }



        return null;
    }

    public override Object VisitCondFactAST([NotNull] Parser1.CondFactASTContext context) {
        //expr relop expr	
        Visit(context.expr(0));
        Visit(context.relop());
        Visit(context.expr(1));
        
        return null;
    }

    public override Object VisitConditionAST([NotNull] Parser1.ConditionASTContext context) { 
        
        //condTerm (O condTerm)*	


        Visit(context.condTerm(0));

        if (context.O(0) != null)
        {
            print(context.O(0).ToString(),true);
            Visit(context.condTerm(1));
            for (int i = 1; i <= context.O().Length - 1; i++)
            {
                print(context.O(i).ToString(), true);
                Visit(context.condTerm(i+1));
            }
        }


        return null; 
    
    }

    public override Object VisitForstaAST([NotNull] Parser1.ForstaASTContext context) { 
        
        //FOR PIZQ expr PyCOMA (condition | ) PyCOMA (statement | ) PDER statement
        printtab(cont);
        print(context.GetType().Name, true);

        print(context.FOR().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.expr());
        print(context.PyCOMA(0).ToString(), true);

        cont++;
        if (context.condition() != null) {
            Visit(context.condition());
        }
        print(context.PyCOMA(1).ToString(), true);

        if (context.statement(0) != null)
        {
            Visit(context.statement(0));
        }

        print(context.PDER().ToString(), true);
        Visit(context.statement(1));
        cont--;

        return null; 
    
    }

    public override Object VisitDesignatorstaAST([NotNull] Parser1.DesignatorstaASTContext context) {


        //designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA
        
        printtab(cont); 

        print(context.GetType().Name, true);

        Visit(context.designator());

        if (context.ASIGN() != null)
        {
            print(context.ASIGN().ToString(), true);
            Visit(context.expr());
        }
        else if (context.PIZQ() != null)
        {
            print("(" + "\n", true);
            if (context.actPars() != null)
            {
                Visit(context.actPars());
            }
            print(")" + "\n", true);

        }
        else if (context.MASMAS() != null)
            print(context.MASMAS().ToString(), true);

        else if (context.MENOSMENOS() != null)
            print(context.MENOSMENOS().ToString(), true);

        print(";" + "\n",true);

        return null; 
    }

    public override Object VisitWhilestaAST([NotNull] Parser1.WhilestaASTContext context) {
        //WHILE PIZQ condition PDER statement	 

        printtab(cont);
        print(context.GetType().Name, true);

        print(context.WHILE().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.condition());
        print(context.PDER().ToString(), true);
        Visit(context.statement());

        return null;
    }

    public override Object VisitReturnstaAST([NotNull] Parser1.ReturnstaASTContext context) { 
        
        
        //RETURN ( expr | ) PyCOMA	

        printtab(cont);
        print(context.GetType().Name, true);

        print(context.RETURN().ToString(), true);

        if (context.expr() != null) {

            Visit(context.expr());
        }

        print(context.PyCOMA().ToString()+"\n", true);

        return null; 
    
    }

    public override Object VisitPycomastaAST([NotNull] Parser1.PycomastaASTContext context) { 
        print(context.PyCOMA().ToString()+"\n", true);
        return null; }

    public override Object VisitForeachstaAST([NotNull] Parser1.ForeachstaASTContext context) {
        //FOREACH PIZQ type ID IN ID PDER	 
        printtab(cont);
        print(context.GetType().Name, true);

        print(context.FOREACH().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.type());
        print(context.ID().ToString(), true);
        print(context.IN().ToString(), true);
        Visit(context.expr());
        print(context.PDER().ToString(), true);

        cont++;
        Visit(context.statement());
        cont--;
        
        return null;
    }

    public override Object VisitBreakAST([NotNull] Parser1.BreakASTContext context) {

        //BREAK PyCOMA	

        printtab(cont);
        print(context.GetType().Name, true);

        print(context.BREAK().ToString(), true);
        print(context.PyCOMA().ToString()+"\n", true);

        return null; 
    }

    public override Object VisitReadstaAST([NotNull] Parser1.ReadstaASTContext context) { 
         //READ PIZQ designator PDER PyCOMA		
        printtab(cont);
        print(context.GetType().Name, true);

        print(context.READ().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.designator());
        print(context.PDER().ToString(), true);
        print(context.PyCOMA().ToString()+"\n", true);


        return null; 
    
    }

    public override Object VisitIfstaAST([NotNull] Parser1.IfstaASTContext context) {

        //IF PIZQ condition PDER statement ( ELSE statement | ) 
        printtab(cont);
        print(context.GetType().Name, true);

        print(context.IF().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.condition());
        print(context.PDER().ToString(), true);
        Visit(context.statement(0));


        if (context.ELSE() != null) {
            print("\n",true);
            printtab(cont);
            print(context.ELSE().ToString() , true);
            Visit(context.statement(1));
        }

        return null; 
    }

    public override Object VisitWritestaAST([NotNull] Parser1.WritestaASTContext context) {
        //WRITE PIZQ expr ( COMA NUM | ) PDER PyCOMA	
        printtab(cont);
        print(context.GetType().Name, true);

        print(context.WRITE().ToString(), true);
        print(context.PIZQ().ToString(), true);
        Visit(context.expr());

        if (context.COMA() != null)
        {
            print(context.COMA().ToString() + "\n", true);
            print(context.NUM().ToString(), true);
            print(context.PDER().ToString(), true);
        }

        print(context.PDER().ToString(), true);
        print(context.PyCOMA().ToString() + "\n", true);

        return null;
    }

    public override Object VisitBlockstaAST([NotNull] Parser1.BlockstaASTContext context) {
        Visit(context.block());
        return null; }

    public override Object VisitBlockAST([NotNull] Parser1.BlockASTContext context) { 
        
        //CORCHETEIZQ (statement)* CORCHETEDER
        print(context.GetType().Name, true);
        print("{"+"\n", true);

        cont++;
        if(context.statement()!=null){
            Visit(context.statement(0));
            for (int i = 1; i <= context.statement().Length - 1; i++)
            {
                 Visit(context.statement(i));
            }
        }
        cont--;
        printtab(cont);
        print("}" + "\n", true);

        return null; 
    
    }

    public override Object VisitExprAST([NotNull] Parser1.ExprASTContext context) { 
        
        
        //(MENOS | ) term ( addop term )*

        if (context.MENOS() != null)
        {
            print(context.MENOS().ToString(), true);
        }

        Visit(context.term(0));

        if (context.addop(0) != null)
        {
            Visit(context.addop(0));
            Visit(context.term(1));
            for (int i = 1; i <= context.addop().Length - 1; i++)
            {
                Visit(context.addop(i));
                Visit(context.term(i+1));
            }
        }


        return null;
    
    }

    public override Object VisitTermAST([NotNull] Parser1.TermASTContext context) {

        //factor ( mulop factor)*

        Visit(context.factor(0));

        if (context.mulop(0) != null) {
            Visit(context.mulop(0));
            Visit(context.factor(1));

            for (int i = 1; i <= context.mulop().Length - 1; i++) {

                Visit(context.mulop(i));
                Visit(context.factor(i+1));
            
            }
        }
        
        return null; 
    }

    /*
    public override Object VisitLenAST([NotNull] Parser1.LenASTContext context)
    {
        //LEN PIZQ (ID | STRING ) PDER

        print(context.LEN().ToString(), true);
        print(context.PIZQ().ToString(), true);

        if (context.ID() != null)
        {
            print(context.ID().ToString(), true);

        }
        else { 
            print(context.STRING().ToString(), true); 
        }

        print(context.PDER().ToString(), true);
        return null;
    }
    */
    public override Object VisitStringAST([NotNull] Parser1.StringASTContext context) {

        print(context.STRING().ToString(), true);
        return null; 
    }

    public override Object VisitTruefalseFactorAST([NotNull] Parser1.TruefalseFactorASTContext context) { 
        
        // (TRUE | FALSE)	

        if (context.TRUE() != null) {
            print(context.TRUE().ToString(), true);
        }
        else
            print(context.FALSE().ToString(), true);

        return null; 
    }

    public override Object VisitDesignatorfactorAST([NotNull] Parser1.DesignatorfactorASTContext context) {

        Visit(context.designator());

        if (context.PIZQ() != null) {
            print(context.PIZQ().ToString(), true);
            if (context.actPars() != null) {
                Visit(context.actPars());
            }
            print(context.PDER().ToString(), true);
        }

        
        return null;
    }

    public override Object VisitNumFactorAST([NotNull] Parser1.NumFactorASTContext context) {
        print(context.NUM().ToString(), true);
        return null; }

    public override Object VisitNewidFactorAST([NotNull] Parser1.NewidFactorASTContext context) {

        // NEW ID ( PCIZQ expr PCDER | )						#newidFactorAST

        print(context.NEW().ToString(), true);
        print(context.ID().ToString(), true);

        if (context.PCIZQ() != null)
        {
            print(context.PCIZQ().ToString(), true);
            Visit(context.expr());
            print(context.PCDER().ToString(), true);
        }

        
        return null;
    }

    public override Object VisitCharconstFactorAST([NotNull] Parser1.CharconstFactorASTContext context) {
        print(context.CHARCONST().ToString(), true);
        return null; }

    public override Object VisitPizqExpreFactorAST([NotNull] Parser1.PizqExpreFactorASTContext context) {
        
        //PIZQ expr PDER
        print(context.PIZQ().ToString(), true);
        Visit(context.expr());
        print(context.PDER().ToString(), true);
        return null; 
    
    }

    public override Object VisitVarDeclAST([NotNull] Parser1.VarDeclASTContext context) {

        //type ID (COMA ID)* PyCOMA	 

        printtab(cont);
        print(context.GetType().Name, true);
        Visit(context.type());
        print(context.ID(0).ToString(), true); //Print "ID"

        cont++;
        if (context.COMA(0) != null) {
            print(context.COMA(0).ToString() + " ", true);
            print(context.ID(1).ToString(), true);
            for (int i = 1; i <= context.COMA().Length - 1; i++)
            {
                print(context.COMA(i).ToString()+" ", true); //Print ","
                print(context.ID(i+1).ToString(), true); //Print "ID"
                
            }
            
        }
        print(context.PyCOMA().ToString()+"\n", true);//Print ";"

        cont--;
        
        return null; 
    

    }


    public override Object VisitAddopAST([NotNull] Parser1.AddopASTContext context) {

        //(SUMA | MENOS)	 

        if (context.SUMA() != null)
        {
            print(context.SUMA().ToString(), true);
        }
        else 
        {
            print(context.MENOS().ToString(), true);
        }
        return null; 
    }

    public override Object VisitMulopAST([NotNull] Parser1.MulopASTContext context){
        //(MUL | SLASH | PORCENT)

        print(context.children[0].ToString(),true);

        /*
        if (context.MUL() != null) {
            print(context.MUL().ToString(), true);
        }
        else if (context.SLASH() != null)
        {
            print(context.SLASH().ToString(), true);
        }
        else {
            print(context.PORCENT().ToString(), true);
        }
         * */

        return null;
    }


    public override Object VisitRelopAST([NotNull] Parser1.RelopASTContext context)
    {
        //(IGUALIGUAL | DIFERENTE | MAYOR | MAYORIGUAL | MENOR | MENORIGUAL) 
        if (context.IGUALIGUAL() != null)
        {
            print(context.IGUALIGUAL().ToString(), true);
        }
        else if (context.DIFERENTE() != null)
        {
            print(context.DIFERENTE().ToString(), true);
        }
        else if (context.MAYOR() != null)
        {
            print(context.MAYOR().ToString(), true);
        }
        else if (context.MAYORIGUAL() != null)
        {
            print(context.MAYORIGUAL().ToString(), true);
        }
        else if (context.MENOR() != null)
        {
            print(context.MENOR().ToString(), true);
        }
        else
        {
            print(context.MENORIGUAL().ToString(), true);
        }


        return null;
    }


}


