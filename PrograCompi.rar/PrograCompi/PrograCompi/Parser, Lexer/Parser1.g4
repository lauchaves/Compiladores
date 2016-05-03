parser grammar Parser1;

options {
language = CSharp;
tokenVocab = Lexer1;
}


/*
* Parser Rules
*/


program 
: 
	CLASS ID (decla)* CORCHETEIZQ (methodDecl)* CORCHETEDER									#programAST
;
decla 
:		
	  constDecl																				#constDeclaAST
	| varDecl																				#varDeclaAuxAST
	| classDecl																				#classDeclaAuxAST
;

constDecl
:
	CONST type ID ASIGN NUM PyCOMA													#constDeclNumAST
	|  CONST type ID ASIGN CHARCONST PyCOMA											#constDeclCharAST
;

varDecl
:
	type ID (COMA ID)* PyCOMA																#varDeclAST
;

classDecl
:
	CLASS ID CORCHETEIZQ (varDecl)* CORCHETEDER												#classDeclAST
;

methodDecl
:	
	(type | VOID) ID PIZQ (formPars | ) PDER (varDecl)* block							    #methodDeclAST
;

formPars
:
	type ID (COMA type ID)*																	#formParsAST
;

type
:
	ID ( PCIZQ PCDER | )																	#typeAST
;

// foreach (int element in fibarray) 

statement
:
	designator ( ASIGN expr | PIZQ ( actPars | ) PDER  | MASMAS | MENOSMENOS ) PyCOMA		 #designatorstaAST

		 |  IF PIZQ condition PDER statement ( ELSE statement | )							 #ifstaAST

		 //“for” “(“ Expr “;” [Condition] “;” [Statement] ) Statement

		 |  FOR PIZQ expr PyCOMA (condition | ) PyCOMA (statement | ) PDER statement		 #forstaAST

		 |  WHILE PIZQ condition PDER statement												 #whilestaAST 
		 |  FOREACH PIZQ type ID IN expr PDER	statement										 #foreachstaAST
		 |  BREAK PyCOMA																	 #breakAST
		 |  RETURN ( expr | ) PyCOMA														 #returnstaAST			
		 |  READ PIZQ designator PDER PyCOMA												 #readstaAST
		 |  WRITE PIZQ expr ( COMA NUM | ) PDER PyCOMA									 #writestaAST
		 |  block																			 #blockstaAST
		 |  PyCOMA																			 #pycomastaAST

;

block
:
	CORCHETEIZQ (statement)* CORCHETEDER					 #blockAST
;

actPars
:
	expr (COMA expr)*										 #actParsAST
;

condition
:
	condTerm (O condTerm)*									 #conditionAST
;

condTerm
:
	condFact (Y condFact)*									#condTermAST
;

condFact
:
	expr relop expr											#condFactAST
;

expr
:
// - q 12  true false 
	(MENOS | ) term ( addop term )*							#exprAST
;



term
: 
	factor ( mulop factor)*									#termAST

;								
factor
:
	designator (PIZQ (actPars | ) PDER | )					#designatorfactorAST
	|	NUM													#numFactorAST
	|	CHARCONST											#charconstFactorAST
	|  (TRUE | FALSE)										#truefalseFactorAST
	|  NEW ID ( PCIZQ expr PCDER | )						#newidFactorAST
	|  PIZQ expr PDER										#pizqExpreFactorAST
	|  STRING												#stringAST


;

designator 	 //locals [string Tipo]
:
	 // X.j  || X(asdds).j().

	ID (desig2)*											#designatorAST
;

desig2 //locals [string Tipo]
:
//asd.asd   asd[]

	(PUNTO ID | PCIZQ expr PCDER)									#desig2AST
;

relop
:
	(IGUALIGUAL | DIFERENTE | MAYOR | MAYORIGUAL | MENOR | MENORIGUAL)	#relopAST
;
addop
:		
	(SUMA | MENOS)														#addopAST
;
mulop
:
	(MUL | SLASH | PORCENT)												#mulopAST
;

