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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AwakeCoding.Common
{
	public delegate void DisconnectedEventHandler(object sender, DisconnectedEventArgs e);
	public class DisconnectedEventArgs
	{
		public int DiscReason { get; set; }

		public DisconnectedEventArgs(int discReason)
		{
			DiscReason = discReason;
		}
	}

	public delegate void FatalErrorEventHandler(object sender, FatalErrorEventArgs e);
	public class FatalErrorEventArgs
	{
		public int ErrorCode { get; set; }
		public FatalErrorEventArgs(int errorCode)
		{
			ErrorCode = errorCode;
		}
	}

	public delegate void WarningEventHandler(object sender, WarningEventArgs e);
	public class WarningEventArgs
	{
		public int WarningCode { get; set; }
		public WarningEventArgs(int warningCode)
		{
			WarningCode = warningCode;
		}
	}
}