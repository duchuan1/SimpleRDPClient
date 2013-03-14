﻿using AwakeCoding.Common;
using AxMSTSCLib;
using MSTSCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IMsTscAxEvents_OnDisconnectedEventHandler = AxMSTSCLib.IMsTscAxEvents_OnDisconnectedEventHandler;
using IMsTscAxEvents_OnFatalErrorEventHandler = AxMSTSCLib.IMsTscAxEvents_OnFatalErrorEventHandler;
using IMsTscAxEvents_OnWarningEventHandler = AxMSTSCLib.IMsTscAxEvents_OnWarningEventHandler;

namespace AwakeCoding.MsRdpClient
{
    public class MsRdpClientAdapter : IRDPClient, IDisposable
    {
        public enum MsRdpClientVersion
        {
            Unknown,
            MsClient50,
            MsClient60,
            MsClient61,
            MsClient70,
            MsClient80
        }

        // Last detected version of the ActiveX control
        private static MsRdpClientVersion lastDetectedVersion = MsRdpClientVersion.Unknown;

        private IMsRDPClient client;

        public MsRdpClientAdapter()
        {
            TryVersion(MsRdpClientVersion.MsClient80, () => { client = new MsRDPClient80(); });
            TryVersion(MsRdpClientVersion.MsClient70, () => { client = new MsRDPClient70(); });
            TryVersion(MsRdpClientVersion.MsClient61, () => { client = new MsRDPClient61(); });
            TryVersion(MsRdpClientVersion.MsClient60, () => { client = new MsRDPClient60(); });
            TryVersion(MsRdpClientVersion.MsClient50, () => { client = new MsRDPClient50(); });

            if (lastDetectedVersion == MsRdpClientVersion.Unknown)
            {
                throw new NotSupportedException("MsRrdpClient could not be instanciated");
            }

            System.Diagnostics.Debug.WriteLine("AxRDPClient version instanciated: " + lastDetectedVersion);

            SecuredSettings = new MsSecuredSettings(client);
            AdvancedSettings = new MsAdvancedSettings(client);
            TransportSettings = new MsTransportSettings(client);

            RegisterEvents();
        }

        public Control GetControl()
        {
            return client as Control;
        }

        private void RegisterEvents()
        {
            if (client != null)
            {
                client.OnConnected += client_OnConnected;
                client.OnDisconnected += client_OnDisconnected;
                client.OnFatalError += client_OnFatalError;
                client.OnWarning += client_OnWarning;
            }
        }

        void client_OnConnected(object sender, EventArgs e)
        {
            if (Connected != null)
            {
                Connected(this, e);
            }
        }

        void client_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
        {
            if (WarningOccurred != null)
            {
                WarningOccurred(this, new WarningEventArgs(e.warningCode));
            }
        }

        void client_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
        {
            if (FatalErrorOccurred != null)
            {
                FatalErrorOccurred(this, new FatalErrorEventArgs(e.errorCode));
            }
        }

        void client_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            if (Disconnected != null)
            {
                Disconnected(this, new DisconnectedEventArgs(e.discReason));
            }
        }

        private void UnregisterEvents()
        {
        }


        private void TryVersion(MsRdpClientVersion clientVersion, MethodInvoker doApplyVersion)
        {
            try
            {
                if (lastDetectedVersion == MsRdpClientVersion.Unknown || lastDetectedVersion == clientVersion)
                {
                    doApplyVersion();
                    lastDetectedVersion = clientVersion;
                }
            }
            catch
            {

                lastDetectedVersion = MsRdpClientVersion.Unknown;
            }
        }

        public static MsRdpClientVersion LastDetectedVersion
        {
            get
            {
                return lastDetectedVersion;
            }
        }

        public void Dispose()
        {
            UnregisterEvents();
        }

        public IAdvancedSettings AdvancedSettings
        {
            get;
            private set;
        }

        public ISecuredSettings SecuredSettings
        {
            get;
            private set;
        }

        public ITransportSettings TransportSettings
        {
            get;
            private set;
        }

        public string Server
        {
            get
            {
                return client.Server;
            }
            set
            {
                client.Server = value;
            }
        }

        public string UserName
        {
            get
            {
                return client.UserName;
            }
            set
            {
                client.UserName = value;
            }
        }

        public string Domain
        {
            get
            {
                return client.Domain;
            }
            set
            {
                client.Domain = value;
            }
        }

        public int DesktopWidth
        {
            get
            {
                return client.DesktopWidth;
            }
            set
            {
                client.DesktopWidth = value;
            }
        }

        public int DesktopHeight
        {
            get
            {
                return client.DesktopHeight;
            }
            set
            {
                client.DesktopHeight = value;
            }
        }

        /// <summary>
        /// Apply the values of the current RDP Client internal RDP ActiveX component
        /// Override in derived classes to extend the settings that apply to a given version. Always call
        /// the ancestor version.
        /// </summary>
        protected virtual void ApplySettings()
        {
        }

        /// <summary>
        /// Apply the values of AdvancedSettings to the internal RDP ActiveX component
        /// Override in derived classes to extend the settings that apply to a given version. Always call
        /// the ancestor version.
        /// </summary>
        protected virtual void ApplyAdvancedSettings()
        {
        }

        /// <summary>
        /// Apply the values of SecuredSettings to the internal RDP ActiveX component
        /// Override in derived classes to extend the settings that apply to a given version. Always call
        /// the ancestor version.
        /// </summary>
        protected virtual void ApplySecuredSettings()
        {
            IMsTscNonScriptable secured = (IMsTscNonScriptable) client.GetOcx();

            //secured.ClearTextPassword = SecuredSettings.ClearTextPassword;
            //secured.BinaryPassword = SecuredSettings.BinaryPassword;
            //secured.BinarySalt = SecuredSettings.BinarySalt;
            //secured.PortablePassword = SecuredSettings.PortablePassword;
            //secured.PortableSalt = SecuredSettings.PortableSalt;
        }

        /// <summary>
        /// Apply the values of TransportSettings to the internal RDP ActiveX component
        /// Override in derived classes to extend the settings that apply to a given version. Always call
        /// the ancestor version.
        /// </summary>
        protected virtual void ApplyTransportSettings()
        {
        }


        public void Connect()
        {
            ApplySettings();
            ApplyAdvancedSettings();
            ApplySecuredSettings();
            ApplyTransportSettings();

            client.Connect();
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public event EventHandler Connected;

        public event DisconnectedEventHandler Disconnected;

        public event FatalErrorEventHandler FatalErrorOccurred;

        public event WarningEventHandler WarningOccurred;
    }
}
