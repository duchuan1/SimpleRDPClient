﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwakeCoding.Common
{
    public delegate void FatalErrorEventHandler(object sender, FatalErrorEventArgs e);
    public class FatalErrorEventArgs
    {
        public int ErrorCode { get; set; }
        public FatalErrorEventArgs(int errorCode)
        {
            ErrorCode = errorCode;
        }
    }

    public delegate void WarningEventHandler(object sender, FatalErrorEventArgs e);
    public class WarningEventArgs
    {
        public int WarningCode { get; set; }
        public WarningEventArgs(int warningCode)
        {
            WarningCode = warningCode;
        }
    }
}
