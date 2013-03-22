﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AwakeCoding.Common;

namespace AwakeCoding.FreeRDPClient
{
    public class SecuredSettingsStub : ISecuredSettings
    {
        private string clearPassword;

        public void SetClearTextPassword(string value)
        {
            
        }

        public string ClearTextPassword
        {
            set
            {
                //Console.WriteLine("SecuredSettingsStub.set_ClearTextPassword " + value);
                clearPassword = value;
            }
            get
            {
                return clearPassword;
            }
        }

        public string BinaryPassword
        {
            get;
            set;
        }

        public string BinarySalt
        {
            get;
            set;
        }

        public string PortablePassword
        {
            get;
            set;
        }

        public string PortableSalt
        {
            get;
            set;
        }
    }
}