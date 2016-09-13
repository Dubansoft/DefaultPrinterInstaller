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
    internal class GlobalSetup
    {
        public static string LogFolderPath
        {
            get { return Application.StartupPath + @"\Log"; }
        }
        public static string LogFileName
        {
            get { return "DefaultPrinterInstaller.txt"; }
        }

        //private static string printServerName = "localhost";
        //public static string PrintServerName
        //{
        //    get { return printServerName; }
        //    set { printServerName = value; }
        //}

        //private static string printServerIp = "0.0.0.0";
        //public static string PrintServerIp
        //{
        //    get { return printServerIp; }
        //    set { printServerIp = value; }
        //}

        //private static int printerWakeUpInterval;
        //public static int PrinterWakeUpInterval
        //{
        //    get { return GlobalSetup.printerWakeUpInterval; }
        //    set { GlobalSetup.printerWakeUpInterval = value; }
        //}

        //private static string defaultPrinterQueueName;
        //public static string DefaultPrinterQueueName
        //{
        //    get { return defaultPrinterQueueName; }
        //    set { defaultPrinterQueueName = value; }
        //}

        private static int internetConnectionCheck = 0;
        public static int InternetConnectionCheck
        {
            get{return internetConnectionCheck;}
            set{internetConnectionCheck = value;}
        }

        public static string[] Months
        {
            get
            {
                return new string[] { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };
            }
        }



        public static void UpdateRunningConfig()
        {
            //Update current program variables


            //try
            //{
            //    string[] lines = System.IO.File.ReadAllLines(GlobalSetup.SettingsFolderPath + "\\" + GlobalSetup.SettingsFileName);

            //    foreach (string line in lines)
            //    {
            //        if (!line.Contains("=") || line.Trim().Length == 0)
            //        {
            //            continue;
            //        }

            //        string[] dataline;
            //        dataline = line.Split('=');

            //        object newLineValue = dataline[1].ToString();

            //        switch (dataline[0].ToString())
            //        {
            //            case "PrintServerIp":
            //                GlobalSetup.PrintServerIp = newLineValue.ToString();
            //                break;
            //            case "PrintServerName":
            //                GlobalSetup.PrintServerName = newLineValue.ToString();
            //                break;
            //            case "DefaultPrinterQueueName":
            //                GlobalSetup.DefaultPrinterQueueName = newLineValue.ToString();
            //                break;
            //            case "PrinterWakeUpInterval":
            //                GlobalSetup.PrinterWakeUpInterval = Convert.ToInt32(newLineValue);
            //                break;
            //            default:
            //                break;
            //        }

            //    }
            //}
            //catch (Exception ee)
            //{
            //    EventLogger.LogEvent(null, ee.Message.ToString(), ee);
            //}
        }

    }
}
