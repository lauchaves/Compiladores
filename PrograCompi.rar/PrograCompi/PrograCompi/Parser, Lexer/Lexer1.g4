lexer grammar Lexer1;

/*
WS
: ' ' -> channel(HIDDEN)
;

*/

ASIGN : '=' ;
PyCOMA : ';' ;


IN 
:
	'in'
;
BREAK 
: 'break'
;

CLASS 
: 'class'
;


CONST
: 'const'
;

ELSE
: 'else'
;

IF
: 'if'
;

NEW
: 'new'
;

READ 
: 'read'
;

RETURN
: 'return'
;

VOID
: 'void'
;

WHILE 
: 'while'
;

WRITE 
: 'write'
;

FOR 
:
 'for'
;

FOREACH
:
 'foreach'
;

TRUE: 'true';

FALSE: 'false';

/*
LEN
:
	'len'
;
*/

//BOOLCONST		:	'true' | 'false';

NUM:	'0' | (DIGIT (DIGIT0)*);

fragment
DIGIT0			:	[0-9];
fragment
DIGIT			:	[1-9];


CHARCONST
: 
	'\u0027' ( PRINTABLECHAR | '\\n' | '\\r' ) '\u0027'
;




COMA : ',' ;
PIZQ : '(' ;
PDER : ')' ;
SUMA : '+' ;
MUL : '*' ;
ADMIRACION: '!';
NUMERAL: '#';
//COMILLAS: '"';
DOLAR: '$';
PORCENT: '%';
AMPERSAND: '&';
COMILLASIMPLE: '\u0027'; 
PUNTO: '.'; 
MENOS: '-';
SLASH: '/';
DOSPUNTOS: ':';
MENOR: '<';
MAYOR: '>'; 
PREG: '?';
ARROA: '@';
IGUALIGUAL: '=='; 
DIFERENTE: '!='; 
MAYORIGUAL: '>='; 
MENORIGUAL: '<=';
Y: '&&';
O: '||';
MASMAS: '++'; 
MENOSMENOS: '--';
CORCHETEIZQ:'{';
CORCHETEDER:'}';
PCIZQ: '[';
PCDER: ']';

ID : L (L | N )* ;
fragment L : 'A'..'Z'|'a'..'z'|'_' ; 
fragment N : '0'..'9' ;

PRINTABLECHAR
: LETTERS | NUM | ADMIRACION | /*COMILLAS |*/ NUMERAL  | DOLAR |	
  PORCENT | AMPERSAND | COMILLASIMPLE | PIZQ | PDER	| MUL |
  SUMA | COMA | MENOS | PUNTO | SLASH | DOSPUNTOS | PyCOMA |
  MAYOR | MENOR	| ASIGN | PREG | ARROA
;


LETTERS
: 'A'..'Z' | 'a'..'z' | '_'
;

NEWLINE 
: ('\n'|'\r' | '\t') -> channel(HIDDEN)
;

WS : ' '+ -> skip ;  

// Comentarios 

COMMENT : '//' ~[\r\n]* '\r'? '\n' -> skip ;
CMT : '/*' .*? '*/' -> skip;


LQUOTE     : '"' -> more, mode(MSTRING) ;
mode MSTRING ;
STRING : '"' -> mode(DEFAULT_MODE) ;
TEXT: . -> more ;




