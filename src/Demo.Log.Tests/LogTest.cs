using System;
using System.Diagnostics;
using Demo.Log;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Demo.Business.Tests
{
    [TestClass]
    public class LogTest
    {
        [TestMethod]
        public void TraceTest()
        {
            {
                // http://vincentlaine.developpez.com/tuto/dotnet/log/#LII-B-3-d

                /*var textListener = new TextWriterTraceListener("./debug.log");
                textListener.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId | TraceOptions.DateTime;
                Debug.Listeners.Add(textListener); //Création d'un "listener" texte pour sortie dans un fichier texte
                Debug.Listeners.Add(new TraceListenerJson());*/

                //Debug.AutoFlush = true; //On écrit directement, pas de temporisation.
                
                // Log par défault
               // Logger.Default.Info("Une info");

                // Log d'un logger propre au projet à configurer
                //Logger.Get("Business").Info("Une info business");

                try{
                    throw new Exception();
                }
                catch(Exception ex)
                {
                    // Log d'une exception
                   // Logger.Default.Error(ex, "Une exception");
                }

            }
        }
    }
}
