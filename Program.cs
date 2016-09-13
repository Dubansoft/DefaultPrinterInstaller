using System;
using System.Windows.Forms;
using System.Management;
using System.Net.NetworkInformation;
using System.Diagnostics;
using Microsoft.Win32;
using System.Drawing.Printing;
using System.Linq;
using System.IO;

namespace DefaultPrinterInstaller
{
    class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0)
            {
                if (args[0].Contains("-c"))
                {
                    Application.Run(new frmSettings());

                    return;
                }
            }

            Application.Run(new Form1());             
         }
    }
    
}
