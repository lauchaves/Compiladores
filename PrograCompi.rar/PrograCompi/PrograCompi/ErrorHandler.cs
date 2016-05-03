using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrograCompi
{
    /// <summary>
    /// Clase que implementa Metodos del DefaultErrorStrategy
    /// </summary>
    class ErrorHandler : DefaultErrorStrategy
    {
        /// <summary>
        /// Instancia de esta misma clase para ser utilizada en proceso de compilacion
        /// </summary>
        public static ErrorHandler INSTANCE = new ErrorHandler();

        /// <summary>
        /// Metodo para Reportar Errores
        /// </summary>
        /// <param name="recognizer"></param>
        /// <param name="e"></param>
        public override void ReportError(Parser recognizer, RecognitionException e)
        {
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }

            BeginErrorCondition(recognizer);

            if (e is NoViableAltException)
            {
                ReportNoViableAlternative(recognizer, (NoViableAltException)e);
            }

            else
            {
                if (e is InputMismatchException)
                {
                    ReportInputMismatch(recognizer, (InputMismatchException)e);
                }

                else
                {
                    if (e is FailedPredicateException)
                    {
                        ReportFailedPredicate(recognizer, (FailedPredicateException)e);
                    }

                    else
                    {
#if !PORTABLE
                        Program.errores.Append("Error de reconocimiento desconocido de tipo: " + e.GetType().FullName + "\n");
#endif
                        NotifyErrorListeners(recognizer, e.Message, e);
                    }
                }
            }
        }

        /// <summary>
        /// Metodo para Reportar perdida de algun token
        /// </summary>
        /// <param name="recognizer"></param>
        protected override void ReportMissingToken(Parser recognizer)
        {
            if (InErrorRecoveryMode(recognizer))
            {
                return;
            }

            BeginErrorCondition(recognizer);

            IToken t = recognizer.CurrentToken;

            IntervalSet expecting = GetExpectedTokens(recognizer);

            string msg = "missing " + expecting.ToString(recognizer.TokenNames) + " at " + GetTokenErrorDisplay(t);

            recognizer.NotifyErrorListeners(t, msg, null);
        }
    }
}
