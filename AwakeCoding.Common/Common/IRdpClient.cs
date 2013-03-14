﻿using System;
using System.Windows.Forms;

namespace AwakeCoding.Common
{

    /// <summary>
    /// Common interface used to control a RDP Client component.
    /// </summary>
    public interface IRDPClient
    {
        #region Settings
        IAdvancedSettings AdvancedSettings { get; }
        ISecuredSettings SecuredSettings { get; }
        ITransportSettings TransportSettings { get; }

        string Server { get; set; }
        string UserName { get; set; }
        string Domain { get; set; }

        int DesktopWidth { get; set; }
        int DesktopHeight { get; set; }

        Control GetControl();
        #endregion // Settings

        #region Methods
        void Connect();
        void Disconnect();
        #endregion // Methods

        #region Events

        event EventHandler Connected;
        event DisconnectedEventHandler Disconnected;
        event FatalErrorEventHandler FatalErrorOccurred;
        event WarningEventHandler WarningOccurred;
        
        #endregion // Events
    }
}
