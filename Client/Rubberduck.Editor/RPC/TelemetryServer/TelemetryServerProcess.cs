﻿using Microsoft.Extensions.Logging;
using Rubberduck.ServerPlatform;
using Rubberduck.SettingsProvider;

namespace Rubberduck.Editor.RPC.TelemetryServer
{
    public class TelemetryServerProcess : RubberduckServerProcess
    {
        public TelemetryServerProcess(ILogger logger)
            : base(logger)
        {
        }

        protected override string ExecutableFileName { get; } = ServerPlatformSettings.TelemetryServerExecutable;
    }
}