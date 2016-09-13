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
using System.Net.NetworkInformation;
using System.Windows.Forms;
using System.Management;
using System.Threading;

namespace DefaultPrinterInstaller
{
    public partial class Form1 : Form
    {
        Properties.Settings AppProperties = Properties.Settings.Default;

        public Form1()
        {
            InitializeComponent();
            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {

            //this.Hide();
            //this.Width = 0;
            //this.Height = 0;

            Thread.Sleep(15000);
            
            try
            {
                lblMessage.Text = "Validando configuración local...";
                if (AppProperties.PrintServerName == Environment.MachineName)
                {
                    var printerQuery = new ManagementObjectSearcher("SELECT * from Win32_Printer");
                    
                    foreach (var printer in printerQuery.Get())
                    {
                        var name = printer.GetPropertyValue("Name");
                        var isDefault = printer.GetPropertyValue("Default");

                        if ((Boolean)isDefault == true && ((String)name).ToUpper().Contains("OUT"))
                        {
                            MessageBox.Show("La impresora OUT no debe estar definida como predeterminada. Por favor póngase en contacto en el Administrador de impresión.", "Error de configuración", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            break;
                        }
                    }

                    return;
                }

                //If the current host is not the server (or pc with the usb printer connected)
                //enable the rest of the checks.
                this.timerServerConnectionValidation.Enabled = true;
                this.timerServerConnectionValidation.Interval = 1000; //this will call the timer immediatly
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool pingIp(string printerIp)
        {
            if (printerIp == string.Empty || printerIp == "0.0.0.0")
            {

                EventLogger.LogEvent(this,"Dirección IP no válida: " + printerIp, null);
                return false;
            }

            try
            {
                int timeout = 120;
                Ping pingSender = new Ping();

                PingReply reply = pingSender.Send(printerIp, timeout);

                if (reply.Status == IPStatus.Success)
                {
                    return true;
                }

                return false;

            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
                return false;
            }
        }

        private void timerServerConnectionValidation_Tick(object sender, EventArgs e)
        {
            timerServerConnectionValidation.Enabled = false;

            //Validate if the program can connect to the print server set in config file
            //Try to get ip from servername using dns

            string myHost = AppProperties.PrintServerName;
            string myIP = null ;
            string serverAddress = null;

            try
            {
                for (int i = 0; i <= System.Net.Dns.GetHostEntry(myHost).AddressList.Length - 1; i++)
                {
                    if (System.Net.Dns.GetHostEntry(myHost).AddressList[i].IsIPv6LinkLocal == false)
                    {
                        myIP = System.Net.Dns.GetHostEntry(myHost).AddressList[i].ToString();
                        AppProperties.PrintServerIp = myIP;
                        AppProperties.Save();
                        serverAddress = AppProperties.PrintServerName;
                    }
                }
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
                myIP = AppProperties.PrintServerIp;
                serverAddress = myIP;
            }

            if (myIP != null)
            {
                if (!pingIp(myIP))
                {
                    resetNetworkConnectionCheck();
                    return;
                }
            }
            //If conversion from hostname to ip failed,  try to use stored ip
            else
            {
                if (!pingIp(AppProperties.PrintServerIp))
                {
                    resetNetworkConnectionCheck();
                    return;
                }
                else
                {
                    serverAddress = myIP;
                }
            }

            if (!installPrinterOnHost(serverAddress))
            {
                MessageBox.Show("La instalación de la impresora " + AppProperties.DefaultPrintQueue + " ha fallado. Por favor contacte al administrador de impresión", "Error al instalar impresora. Por favor valide la configuración.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Application.Exit();
            }
            else
            {
                //Set printer as default
                setDefaultPrinterOnHost();
                Application.Exit();
            }
            
        }

        private void resetNetworkConnectionCheck()
        {
            try
            {
                timerServerConnectionValidation.Enabled = false;

                switch (GlobalSetup.InternetConnectionCheck)
                {
                    case 0:
                        this.Hide();
                        timerServerConnectionValidation.Interval = 20 * 60 * 1000;
                        //timerServerConnectionValidation.Interval = 10000;
                        GlobalSetup.InternetConnectionCheck = 1;
                        break;
                    case 1:
                        timerServerConnectionValidation.Interval = 120 * 60 * 1000;
                        break;
                    default:
                        break;
                }

                timerServerConnectionValidation.Enabled = true;
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
            }

        }

        private bool installPrinterOnHost(string serverAddress)
        {
            lblMessage.Text = "Instalando impresora predeterminada en equipo local...";


            try
            {
                using (ManagementClass win32Printer = new ManagementClass("Win32_Printer"))
                {
                    using (ManagementBaseObject inputParam =
                       win32Printer.GetMethodParameters("AddPrinterConnection"))
                    {
                        // Replace <server_name> and <printer_name> with the actual server and
                        // printer names.
                        
                        inputParam.SetPropertyValue("Name", @"\\"+serverAddress+@"\"+AppProperties.DefaultPrintQueue);

                        lblMessage.Text = "La impresora predeterminada es " + @"\\" + serverAddress + @"\" + AppProperties.DefaultPrintQueue;

                        using (ManagementBaseObject result =
                            (ManagementBaseObject)win32Printer.InvokeMethod("AddPrinterConnection", inputParam, null))
                        {

                            uint errorCode = (uint)result.Properties["returnValue"].Value;

                            switch (errorCode)
                            {
                                case 0:
                                    lblMessage.Text = ("La impresora se ha instalado correctamente.");
                                    Thread.Sleep(2000);
                                    return true;
                                case 5:
                                    lblMessage.Text = ("Se ha denegado el acceso al servidor. Compruebe sus credenciales.");
                                    Thread.Sleep(2000);
                                    return false;
                                case 123:
                                    lblMessage.Text = ("El archivo, directorio o sintaxis de volumen de red son incorrectos.");
                                    Thread.Sleep(2000);
                                    return false;
                                case 1801:
                                    lblMessage.Text = ("Nombre de impresora inválido: no se ha encontrado la impresora en el servidor.");
                                    Thread.Sleep(2000);
                                    return false;
                                case 1930:
                                    lblMessage.Text = ("El controlador de la impresora no es compatible.");
                                    Thread.Sleep(2000);
                                    return false;
                                case 3019:
                                    lblMessage.Text = ("El controlador de impresión especificado no fue encontrado en el sistema y necesita ser descargado.");
                                    Thread.Sleep(2000);
                                    return false;
                            }
                        }
                    }
                }

                return false;
                

            }
            catch (Exception e)
            {
                EventLogger.LogEvent(this, e.Message.ToString(), e);
                return false;
            }
        }

        private void setDefaultPrinterOnHost()
        {
            //Set default printer
            try
            {
                lblMessage.Text = ("Estableciendo la impresora como predeterminada...");

                object printerName = AppProperties.DefaultPrintQueue;

                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject currentObject in collection)
                {
                    if (currentObject["name"].ToString().Contains(printerName.ToString()))
                    {
                        currentObject.InvokeMethod("SetDefaultPrinter", new object[] { printerName });
                        break;
                    }
                }
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
            }
        }
        
    }
}
