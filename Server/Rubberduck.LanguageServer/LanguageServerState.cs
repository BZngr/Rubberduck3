﻿using Microsoft.Extensions.Logging;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.General;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Rubberduck.ServerPlatform;
using Rubberduck.SettingsProvider.Model.LanguageServer;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Rubberduck.LanguageServer
{
    public interface ILanguageServerState : IServerStateWriter
    {
        DocumentUri? RootUri { get; set; }
        void AddWorkspaceFolders(IEnumerable<WorkspaceFolder> workspaceFolders);
    }

    public class LanguageServerState : ServerState<LanguageServerSettings, LanguageServerStartupSettings>, ILanguageServerState
    {
        private readonly IExitHandler _exitHandler;

        public LanguageServerState(ILogger<LanguageServerState> logger, 
            IHealthCheckService<LanguageServerStartupSettings> healthCheck,
            IExitHandler exitHandler)
            : base(logger, healthCheck)
        {
            _exitHandler = exitHandler;
        }

        private Container<WorkspaceFolder>? _workspaceFolders;
        public IEnumerable<WorkspaceFolder> Workspacefolders => _workspaceFolders ?? throw new ServerStateNotInitializedException();

        public DocumentUri? RootUri { get; set; }

        public void AddWorkspaceFolders(IEnumerable<WorkspaceFolder> workspaceFolders)
        {
            _workspaceFolders = _workspaceFolders?.Concat(workspaceFolders).ToContainer() ?? throw new ServerStateNotInitializedException();
        }

        protected override void OnInitialize(InitializeParams param)
        {
            _workspaceFolders = param.WorkspaceFolders!;
            RootUri = param.RootUri ?? param.RootPath ?? throw new InvalidInitializeParamsException(nameof(param.RootUri));
        }

        protected override void OnClientProcessExited()
        {
            _exitHandler.Handle(new ExitParams(), CancellationToken.None).ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}