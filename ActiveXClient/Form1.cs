﻿using MSTSCLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ActiveXClient
{
    public partial class Form1 : Form
    {

        private string server;
        private string userName;
        private string domain;
        private string clearTextPassword;
        private int port = 0;
        private int bpp = 0;

        public Form1()
        {
            InitializeComponent();

            ReadSettings(
                Environment.GetEnvironmentVariable("HOMEDRIVE") +
                Path.Combine(
                Environment.GetEnvironmentVariable("HOMEPATH"), 
                @"Dropbox\connect.txt"));

        }

        private void ReadSettings(string path)
        {
            Dictionary<string, string> settings = new Dictionary<string,string>();
            using (StreamReader reader = new StreamReader(path))
            {
                string line = reader.ReadLine();
                while(line != null)
                {
                    if (!line.StartsWith("#")) // commented lines start with #. Ignore.
                    {
                        string[] tokens = line.Split(new char[] { '=' }, 2);
                        if (tokens.Length == 2)
                        {
                            settings[tokens[0].Trim().ToLower()] = tokens[1].Trim();
                        }
                    }
                    line = reader.ReadLine();
                }
            }

            settings.TryGetValue("domain", out domain);
            settings.TryGetValue("domain", out domain);
            settings.TryGetValue("domain", out domain);
            server = settings["server"];
            userName = settings["username"];
            clearTextPassword = settings["password"];
            settings.TryGetValue("domain", out domain);


            txtDomain.Text = domain;
            txtPassword.Text = clearTextPassword;
            txtPort.Text = "";
            txtServer.Text = server;
            txtUsername.Text = userName;
        }

        private void toolStripButtonConnect_Click(object sender, EventArgs e)
        {
            rdpConnection.DesktopWidth = rdpConnection.Width;
            rdpConnection.DesktopHeight = rdpConnection.Height;

            rdpConnection.Server = server;
            rdpConnection.UserName = userName;
            rdpConnection.Domain = domain;

            if (bpp > 0)
            {
                rdpConnection.ColorDepth = bpp;
            }

            //IMsTscNonScriptable secured = (IMsTscNonScriptable) rdpConnection.GetOcx();
            //secured.ClearTextPassword = clearTextPassword;
            rdpConnection.AdvancedSettings9.ClearTextPassword = clearTextPassword;
            //rdpConnection.AdvancedSettings9.SmartSizing = true;

            rdpConnection.Connect();
        }


        private void DoConnect(AwakeCoding.FreeRDPClient.RDPClientFrame rdpClientFrame)
        {
            rdpClientFrame.DesktopWidth = rdpClientFrame1.Width;
            rdpClientFrame.DesktopHeight = rdpClientFrame1.Height;

            rdpClientFrame.Server = server;
            rdpClientFrame.UserName = userName;
            rdpClientFrame.Domain = domain;
            rdpClientFrame.AdvancedSettings.ClearTextPassword = clearTextPassword;

            if (bpp > 0)
            {
                rdpClientFrame.ColorDepth = bpp;
            }

            //rdpClientFrame.AdvancedSettings.SmartSizing = true;

            rdpClientFrame.Connect();
        }

        private void toolStripButtonDisconnect_Click(object sender, EventArgs e)
        {
            rdpConnection.Disconnect();
        }

        private void toolStripButtonDisconnect2_Click(object sender, EventArgs e)
        {
            rdpClientFrame1.Disconnect();
        }

        private void toolStripButtonConnect2_Click(object sender, EventArgs e)
        {
            DoConnect(rdpClientFrame1);
        }

        private void toolStripButtonRemoteProgram_Click(object sender, EventArgs e)
        {
            try
            {
                rdpConnection.RemoteProgram.RemoteProgramMode = true;
                rdpConnection.RemoteProgram.ServerStartProgram(@"c:\windows\system32\cmd.exe", @"c:\windows\system32\", "", false, "", false);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

        }

        private void rdpConnection_OnConnected(object sender, EventArgs e)
        {
            SetConnected(1, true);
        }

        private void rdpConnection_OnDisconnected(object sender, AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEvent e)
        {
            SetConnected(1, false);
        }

        private void rdpClientFrame_Connected(object sender, EventArgs e)
        {
            SetConnected(2, true);
        }

        private void rdpClientFrame_Disconnected(object sender, AwakeCoding.Common.DisconnectedEventArgs e)
        {
            SetConnected(2, false);
        }

        private void toolStripButtonConnect3_Click(object sender, EventArgs e)
        {
            DoConnect(rdpClientFrame2);
        }

        private void toolStripButtonDisconnect3_Click(object sender, EventArgs e)
        {
            rdpClientFrame2.Disconnect();
        }

        private void rdpClientFrame2_Connected(object sender, EventArgs e)
        {
            SetConnected(3, true);
        }

        private void rdpClientFrame2_Disconnected(object sender, AwakeCoding.Common.DisconnectedEventArgs e)
        {
            SetConnected(3, false);
        }

        private void SetConnected(int tabIndex, bool connected)
        {
            switch (tabIndex)
            {
                case 1:
                    toolStripButtonConnect.Enabled = !connected;
                    toolStripButtonDisconnect.Enabled = connected;
                    break;
                case 2:
                    toolStripButtonConnect2.Enabled = !connected;
                    toolStripButtonDisconnect2.Enabled = connected;
                    break;
                case 3:
                    toolStripButtonConnect3.Enabled = !connected;
                    toolStripButtonDisconnect3.Enabled = connected;
                    break;
            }
        }

        private void toolStripButtonSmartSize1_Click(object sender, EventArgs e)
        {
            toolStripButtonSmartSize1.Checked = !toolStripButtonSmartSize1.Checked;
            rdpClientFrame1.AdvancedSettings.SmartSizing = toolStripButtonSmartSize1.Checked;
        }

        private void toolStripButtonSmartSize2_Click(object sender, EventArgs e)
        {
            toolStripButtonSmartSize2.Checked = !toolStripButtonSmartSize2.Checked;
            rdpClientFrame2.AdvancedSettings.SmartSizing = toolStripButtonSmartSize2.Checked;
        }

        private void txtUsername_Validated(object sender, EventArgs e)
        {
            userName = txtUsername.Text;
        }

        private void txtDomain_Validated(object sender, EventArgs e)
        {
            domain = txtDomain.Text;
        }

        private void txtPassword_Validated(object sender, EventArgs e)
        {
            clearTextPassword = txtPassword.Text;
        }

        private void txtServer_Validated(object sender, EventArgs e)
        {
            server = txtServer.Text;
        }

        private void txtPort_Validated(object sender, EventArgs e)
        {
            port = Convert.ToInt32(txtPort.Text);
        }

        private void cbColorDepth_Validated(object sender, EventArgs e)
        {
            bpp = Convert.ToInt32(cbColorDepth.SelectedItem);
        }

    }
}
