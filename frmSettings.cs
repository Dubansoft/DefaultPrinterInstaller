using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using Microsoft.VisualBasic;
using System.Drawing.Printing;
using System.Printing;
using System.Xml;
using SnmpSharpNet;

namespace DefaultPrinterInstaller
{
    public partial class frmSettings : Form
    {
        string configFile = Application.StartupPath.ToString() + @"\DefaultPrinterInstaller.exe.config";

        public frmSettings()
        {
            InitializeComponent();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            txtIpAddress.Text = getIpAddress(txtServerName.Text);
        }

        private void loadPrintList()
        {

            try
            {
                
                cmbPrinters.Items.Clear();

                if (!pingIp(txtServerName.Text)) { MessageBox.Show("Dispositivo no disponible en la red"); return; }

                // "theServer" must be a print server to which the user has full print access.
                PrintServer myPrintServer = new PrintServer(@"\\" + txtServerName.Text + @"");

                // List the print server's queues
                PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();

                foreach (PrintQueue pq in myPrintQueues)
                {
                    cmbPrinters.Items.Add(pq.Name);
                }
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);

                try
                {
                    cmbPrinters.Items.Clear();

                    if (!pingIp(txtServerName.Text)) { MessageBox.Show("Disponistivo no disponible en la red"); return; }

                    string networkAdmin = Interaction.InputBox("Por favor ingrese el nombre de usuario administrador de red");
                    string networkDomain = Interaction.InputBox("Por favor ingrese el nombre del dominio de red. Ejemplo: miempresa.loc");
                    string networkPassword = Interaction.InputBox("Por favor ingrese la contraseña del administrador de red");

                    using (NetworkShareAccesser.Access(txtServerName.Text, networkDomain.Trim(), networkAdmin.Trim(), networkPassword))
                    {
                        PrintServer myPrintServer = new PrintServer(@"\\" + txtServerName.Text + @"");

                        // List the print server's queues
                        PrintQueueCollection myPrintQueues = myPrintServer.GetPrintQueues();
                        foreach (PrintQueue pq in myPrintQueues)
                        {
                            cmbPrinters.Items.Add(pq.Name);
                        }
                        cmbPrinters.Enabled = true;
                    }
                }
                catch (Exception eee)
                {
                    EventLogger.LogEvent(this, eee.Message.ToString(), eee);
                    MessageBox.Show(eee.Message.ToString());
                }
            }

        }

        private string getIpAddress(string address)
        {
            IpAddress myIpAddress = new IpAddress("0.0.0.0");

            try
            {
                myIpAddress = new IpAddress(address.Trim());
                return myIpAddress.ToString();
            }
            catch (Exception ee)
            {
                DialogResult dgResult = MessageBox.Show(string.Format("No se ha podido convertir el nombre de host {0} en una IPv4 válida. ¿Desea reemplazar la dirección actual por la dirección 0.0.0.0 por defecto?", txtServerName.Text.Trim()), "Conversión nombre de Host en IPv4", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dgResult == DialogResult.Yes)
                {
                    return "0.0.0.0";
                }
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
                return "";
            }
        }

        private bool pingIp(string printerIp)
        {
            if (printerIp == string.Empty)
            {
                MessageBox.Show("Dirección IP o nombre de host no válidos", "Dirección ip no válida", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool updateXmlFile(string xmlFile, string node, string value)
        {
            try
            {
                //Here is the variable with which you assign a new value to the attribute
                string newValue = value;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                XmlNode nodeName = xmlDoc.SelectSingleNode(node);
                nodeName.InnerText = newValue;
                xmlDoc.Save(xmlFile);

                return true;
            }
            catch (Exception ee)
            {
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(txtIpAddress.Text.Trim().Length > 0 && txtServerName.Text.Trim().Length > 0 && cmbPrinters.Text.Trim().Length > 0) { } else
            {
                MessageBox.Show("No ha completado todos los campos","Error",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            try
            {

                updateXmlFile(configFile, "configuration/userSettings/DefaultPrinterInstaller.Properties.Settings/setting[@name='PrintServerName']/value", txtServerName.Text);
                updateXmlFile(configFile, "configuration/userSettings/DefaultPrinterInstaller.Properties.Settings/setting[@name='PrintServerIp']/value", txtIpAddress.Text);
                updateXmlFile(configFile, "configuration/userSettings/DefaultPrinterInstaller.Properties.Settings/setting[@name='DefaultPrintQueue']/value", cmbPrinters.Text);
                MessageBox.Show("Se ha guardado correctamente la nueva configuración.", "Configuración guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Error:" + ee.Message.ToString());
                EventLogger.LogEvent(this, ee.Message.ToString(), ee);
            }
        }

        private void cmdFindPrinters_Click(object sender, EventArgs e)
        {
            loadPrintList();
            cmbPrinters.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                cmbPrinters.DropDownStyle = ComboBoxStyle.Simple;
                cmbPrinters.Enabled = true;
            }
            else
            {
                cmbPrinters.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbPrinters.Enabled = true;
            }
        }
    }
}
