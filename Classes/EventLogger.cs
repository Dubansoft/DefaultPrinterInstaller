//  Copyright 2015 Jhorman Duban Rodríguez Pulgarín
//  
//  This file is part of InkAlert.
//  
//  InkAlert is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//  
//  InkAlert is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//  
//  You should have received a copy of the GNU General Public License
//  along with InkAlert.  If not, see <http://www.gnu.org/licenses/>.
//  
//  Jhorman Duban Rodríguez., hereby disclaims all copyright interest in 
//  the program "InkAlert" (which makes passes at 
//  compilers) written by Jhorman Duban Rodríguez.
//  
//  Jhorman Duban Rodríguez,
//  5 January 2016
//  For more information, visit <http://www.codigoinnovador.com/projects/inkalert/>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DefaultPrinterInstaller
{
    public static class EventLogger
    {

        /// <summary>
        /// Writes a new line to the InkalertLog.txt log file
        /// </summary>
        /// <param name="sender">The form or object that sends the event</param>
        /// <param name="ErrorMessage">The message to be added to the InkalertLog.txt log file</param>
        /// <param name="e">Exception reference e</param>
        public static void LogEvent(object sender, string ErrorMessage, object e)
        {
            Exception renderedException = null;
            
            try { renderedException = (Exception)e; }
            catch (Exception) { }

            


            string activeControl = "<no control set>";
            string senderFormText = "<no form set>";

            if (sender is Form)
            {
                Form senderForm = new Form();
                senderForm = ((Form)sender);
                senderFormText = senderForm.Name;

                if (senderForm.ActiveControl != null)
                {
                    activeControl = senderForm.ActiveControl.Name;
                }
            }

                FileManager myFileManager = new FileManager();
                FileManager.TextToAppend = DateEngine.CurrentDateTimeShort + " :: " + senderFormText + " :: " + activeControl + " :: " + ErrorMessage;

                if (renderedException is Exception)
                {
                    Exception myException = (Exception)e;

                    FileManager.TextToAppend += "\n\t\tException type is System.Exception";

                    FileManager.TextToAppend += "\n\t\tSource: " + myException.Source.ToString();

                    if (renderedException.InnerException != null)
                    {
                        FileManager.TextToAppend += "\n\t\tInnerException: " + myException.InnerException.ToString();
                    }

                    FileManager.TextToAppend += "\n\t\tStackTrace: " + myException.StackTrace.ToString();
                    FileManager.TextToAppend += "\n\t\tType: " + e.GetType().ToString();
                }

                
                FileManager.TextToAppend += "\n\n"; 

                FileManager.WriteFileDelegate = new Action(myFileManager.WriteToFile);

                if (FileManager.WriteFileDelegate != null)
                {
                    FileManager.WriteFileDelegate();
                }

                e = null;
        }
    }
}
