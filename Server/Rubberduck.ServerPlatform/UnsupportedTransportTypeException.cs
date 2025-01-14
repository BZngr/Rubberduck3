﻿using Rubberduck.SettingsProvider.Model.ServerStartup;
using System;

namespace Rubberduck.ServerPlatform
{
    public class UnsupportedTransportTypeException : NotSupportedException
    {
        public UnsupportedTransportTypeException(TransportType transportType)
            : base($"Transport type '{transportType}' is not supported.") { }

        public TransportType UnsupportedTransportType { get; }
    }
}