﻿using Rubberduck.SettingsProvider.Model.ServerStartup;

namespace Rubberduck.SettingsProvider
{
    public static class ServerPlatformSettings
    {
        public const TransportType DefaultTransportType = TransportType.StdIO;
        public const string DefaultTraceLevel = "Verbose";

        public const string DatabaseServerConnectionString = "Data Source=Rubberduck.db;";

        // NOTE: the editor is a LSP client for the language server, but also a LSP server for the add-in client.

        public const string EditorServerExecutable = "Rubberduck.Editor.exe";
        public const string DatabaseServerExecutable = "Rubberduck.DatabaseServer.exe";
        public const string LanguageServerExecutable = "Rubberduck.LanguageServer.exe";
        public const string TelemetryServerExecutable = "Rubberduck.TelemetryServer.exe";
        public const string UpdateServerExecutable = "Rubberduck.UpdateServer.exe";

        public const string EditorServerName = "Rubberduck.Editor";
        public const string DatabaseServerName = "Rubberduck.DatabaseServer";
        public const string LanguageServerName = "Rubberduck.LanguageServer";
        public const string TelemetryServerName = "Rubberduck.TelemetryServer";
        public const string UpdateServerName = "Rubberduck.UpdateServer";

        public const string EditorServerDefaultPipeName = "Rubberduck.Editor.Pipe";
        public const string DatabaseServerDefaultPipeName = "Rubberduck.DatabaseServer.Pipe";
        public const string LanguageServerDefaultPipeName = "Rubberduck.LanguageServer.Pipe";
        public const string TelemetryServerDefaultPipeName = "Rubberduck.TelemetryServer.Pipe";
        public const string UpdateServerDefaultPipeName = "Rubberduck.UpdateServer.Pipe";
    }
}