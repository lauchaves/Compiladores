using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Antlr4.Runtime;
using PrograCompi;
using Antlr4.Runtime.Misc;
using System.Windows.Forms;
using Antlr4.Runtime.Dfa;
using Antlr4.Runtime.Sharpen;
using Antlr4.Runtime.Atn;


class MyErrorListener : BaseErrorListener
{
    public static MyErrorListener INSTANCE = new MyErrorListener();


    public override void SyntaxError(IRecognizer recognizer,
                            IToken offendingSymbol, 
                            int line, int charPositionInLine, string msg,
                            RecognitionException e)
    {

        String expected = "";
        String found = "";

        int i;
        int j;

        
        if (e != null)
        {

            if (e is NoViableAltException)
                msg = "Error en el parser en la línea: " + line + ", columna: " + (charPositionInLine + 1) + "\n" +
                    "El token: '" + offendingSymbol.Text + "' no es una alternativa viable\n\n";
            else if (e is LexerNoViableAltException)
                msg = "Error en el scanner en la línea: " + line + ", columna: " + (charPositionInLine + 1) + "\n" +
                    "El símbolo: " + offendingSymbol.Text + " no es un token válido\n\n";
            else if (e is FailedPredicateException)
                msg = "Error en el parser en la línea: " + line + ", columna: " + (charPositionInLine + 1) + "\n" +
                    "La semántica de la expresión es inválida";
            else if (e is InputMismatchException)
                msg = "Error en el parser en la línea: " + line + ", columna: " + (charPositionInLine + 1) + "\n" +
                    "Se esperaba: " + e.GetExpectedTokens().ToString(e.Recognizer.TokenNames) + /*AQUÍ TOKENS*/  " y en su lugar se encontró ' " + offendingSymbol.Text + " '\n\n";
            else
                msg = "Error general\n\n";

        }

            //e.GetExpectedTokens().ToString(e.Recognizer.TokenNames)  obtiene todos los tokens en { }

        else
        {

            // ERROR GENERAL  

            // El primer char del error Extraneous input es 'e'
            if(msg[0]=='e'){
                msg = "Error en el parser en la línea: " + line + ", columna: " + (charPositionInLine + 1) + "\n" + "Entrada no esperada: " + tokensEsperados(msg)[0] + " se esperaba: " + tokensEsperados(msg)[1] + "\n"; 
                
                //extraneous input : expecting 
            }
            //errorMissing.ToString().Equals("missing")) ->  El primer char del error missing es 'm'
            else if (msg.Substring(0, 7) == "missing")
            {
                for (i = 7; msg[i] != '\''; i++)
                {

                }
                for (j = i + 1; msg[j] != '\''; j++)
                {
                    expected += msg[j];
                }
                for (i = j + 1; msg[i] != '\''; i++)
                {

                }
                for (j = i + 1; msg[j] != '\''; j++)
                {
                    found += msg[j];
                }
                msg = "Error sintáctico, línea: " + line.ToString() + " columna: " + (charPositionInLine + 1) + "\nFalta '" + expected + "' en '" + found + "'\n";
                
            }

            else if (msg.Substring(0, 10) == "mismatched")
            {
                for (i = 11; msg[i] != '\''; i++)
                {

                }
                for (j = i + 1; msg[j] != '\''; j++)
                {
                    found += msg[j];
                }
                for (i = j + 1; msg[i] != '\''; i++)
                {

                }
                for (j = i + 1; msg[j] != '\''; j++)
                {
                    expected += msg[j];
                }
                msg = "Error sintáctico, línea: " + line.ToString() + " columna: " + (charPositionInLine + 1) + "\nLa entrada no coincide, se encontró '" + found + "' pero se esperaba '" + expected + "'\n";
                
            }

            else if (msg.Substring(0, 30) == "no viable alternative at input")
            {
                for (i = 31; i < msg.Length; i++)
                {
                    found += msg[i];
                }

                msg = "Error sintáctico, línea: " + line.ToString() + " columna: " + (charPositionInLine + 1) + "\nNo hay alternativa viable para la entrada '" + found + "'\n";

            }

            else if (msg.Substring(0, 4) == "rule")
            {
                for (i = 5; msg[i] != ' '; i++)
                {
                    found += msg[i];
                }

                msg = "Error sintáctico, línea: " + line.ToString() + " columna: " + (charPositionInLine + 1) + "\nPredicado fallido '" + found + "'\n";

            }


        }
         


        Form1.errores.Append(msg + "\n");

        //throw new ParserException();
         
        

    }

    // separa el string msg para obtener solo los tokens
    public String[] tokensEsperados(String msg)
    {
        String errorMissingExtraneous = "";
        bool b1 = false;
        String[] mensajeAux = { "", "" }; // en la pos 0, esta el token esperado, y en la pos 1 esta el q se encontro
        int i = 0;
        foreach (char e in msg)
        {
            if (e.Equals('\'') && b1) // Token esperado 
            {
                b1 = false;
                mensajeAux[i] = errorMissingExtraneous + "'";
                i=1;
                errorMissingExtraneous = "";
            }

            if (e.Equals('\'') && !b1)
            {
                b1 = true;
            }
            
            if (b1) {
                errorMissingExtraneous += e;
            }
            
        }

        return mensajeAux;
    }

    // Errores de ambiguedad
    public override void ReportAmbiguity(Parser recognizer, DFA dfa, int startIndex, int stopIndex, bool exact, BitSet ambigAlts, ATNConfigSet configs)
    {
        MessageBox.Show(ambigAlts.ToString() + "\n" + configs.ToString() + "\n");
    }


    /// Metodo para Reportar Problemas de Contexto
    public override void ReportAttemptingFullContext(Parser recognizer, DFA dfa, int startIndex, int stopIndex, BitSet conflictingAlts, SimulatorState conflictState)
    {
        MessageBox.Show(conflictingAlts.ToString() + "\n" + conflictState.ToString() + "\n");
    }
}
