﻿/**
 * FreeRDP: A Remote Desktop Protocol Implementation
 *
 * Copyright 2013 Benoit LeBlanc <benoit.leblanc@awakecoding.com>
 * Copyright 2013 Awake Coding Consulting Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using AwakeCoding.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AwakeCoding.FreeRDPClient
{
	public class RDPClientStub : IRDPClient
	{
		private Panel panel = new Panel();
		private TextBox textBox = new TextBox();
		private bool smartSizing = false;

		public RDPClientStub()
		{
			AdvancedSettings = new AdvancedSettingsStub();
			TransportSettings = new TransportSettingsStub();
			SecuredSettings = new SecuredSettingsStub();

			textBox.Dock = DockStyle.Fill;
			textBox.ReadOnly = true;
			textBox.Multiline = true;
			textBox.BackColor = System.Drawing.Color.White;
			panel.Controls.Add(textBox);

			panel.HandleCreated += panel_HandleCreated;

			((AdvancedSettingsStub)AdvancedSettings).SettingsChanged += RDPClientStub_SettingsChanged;
		}

		void RDPClientStub_SettingsChanged(object sender, SettingsChangedEventArgs args)
		{
			if (SettingsChanged != null)
			{
				UpdateDisplay();
				SettingsChanged(this, args);
			}
		}

		void panel_HandleCreated(object sender, EventArgs e)
		{
			UpdateDisplay();
		}

		public RDPClientVersion ClientVersion
		{
			get
			{
				return RDPClientVersion.Stub;
			}
		}

		public IAdvancedSettings AdvancedSettings
		{
			get;
			set;
		}

		public ITransportSettings TransportSettings
		{
			get;
			set;
		}

		public ISecuredSettings SecuredSettings
		{
			get;
			set;
		}

		public string Server
		{
			get;
			set;
		}

		public string UserName
		{
			get;
			set;
		}
		public string Domain
		{
			get;
			set;
		}

		public int DesktopWidth
		{
			get;
			set;
		}

		public int DesktopHeight
		{
			get;
			set;
		}

		public int ColorDepth
		{
			get;
			set;
		}

		public void Connect()
		{
			IsConnected = true;
			UpdateDisplay();

			if (Connected != null)
			{
				Connected(this, EventArgs.Empty);
			}
		}

		public void Disconnect()
		{
			IsConnected = false;
			UpdateDisplay();

			if (Disconnected != null)
			{
				Disconnected(this, new DisconnectedEventArgs(0));
			}
		}

		public void Attach(Control parent)
		{
			parent.Controls.Add(panel);
		}

		public event EventHandler Connected;

		public event DisconnectedEventHandler Disconnected;

		public event FatalErrorEventHandler FatalErrorOccurred;

		public event WarningEventHandler WarningOccurred;

		public event SettingsChangedEventHandler SettingsChanged;

		private void UpdateDisplay()
		{
			textBox.Text = String.Format(
			    "Dummy remote control" + Environment.NewLine +
				Environment.NewLine +
			    "Connection: {0}" + Environment.NewLine +
			    "DesktopWidth: {1}" + Environment.NewLine +
			    "DesktopHeight: {2}" + Environment.NewLine +
				"SmartSizing: {3}",
			    IsConnected ? Server + " as " + UserName : "(disconnected)",
			    DesktopWidth,
			    DesktopHeight,
				AdvancedSettings.SmartSizing);
		}



		public bool ContainsFocus
		{
			get;
			set;
		}

		public string ConnectedStatusText
		{
			get;
			set;
		}

		public string ConnectingText
		{
			get;
			set;
		}

		public string DisconnectedText
		{
			get;
			set;
		}

		public bool FullScreen
		{
			get;
			set;
		}

		public string FullScreenTitle
		{
			get;
			set;
		}

		public bool HorizontalScrollBarVisible
		{
			get;
			set;
		}

		public bool IsConnected
		{
			get;
			set;
		}

		public bool VerticalScrollBarVisible
		{
			get;
			set;
		}


		public string GetErrorDescription(uint discReason, uint extendedDisconnectReason)
		{
			return "";
		}


		public int Width
		{
			get { return panel.Width; }
		}

		public int Height
		{
			get { return panel.Height; }
		}

		public System.Drawing.Point Location
		{
			get
			{
				return panel.Location;
			}
			set
			{
				panel.Location = value;
			}
		}

		public void SetSize(int width, int height)
		{
			panel.Width = width;
			panel.Height = height;
		}

	}
}
