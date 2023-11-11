﻿using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System.Threading.Tasks;
using System.Threading;
using OmniSharp.Extensions.LanguageServer.Protocol.Workspace;
using OmniSharp.Extensions.LanguageServer.Protocol;
using System.Collections.Generic;
using System;
using Rubberduck.ServerPlatform;
using System.Linq;

namespace Rubberduck.Main.RPC.EditorServer.Handlers
{
    public class InvalidRequestParamsException : ArgumentException
    {
        public InvalidRequestParamsException(string name, object request) 
            : base($"{request.GetType().Name} is missing a required member.", name)
        {
            Data.Add("request", request);
        }
    }

    public class WorkspaceFoldersHandler : WorkspaceFoldersHandlerBase
    {
        /**
         * EditorServer sends this request once started to load additional workspaces,
         * for example if multiple unlocked projects are loaded in the VBE.
        **/

        private readonly ServerPlatformServiceHelper _service;

        public WorkspaceFoldersHandler(ServerPlatformServiceHelper service)
        {
            _service = service;
        }

        public async override Task<Container<WorkspaceFolder>?> Handle(WorkspaceFolderParams request, CancellationToken cancellationToken)
        {
            var folders = Enumerable.Empty<WorkspaceFolder>();

            _service.TryRunAction(() =>
            {
                throw new NotImplementedException();
                // TODO
            });


            return await Task.FromResult(new Container<WorkspaceFolder>(folders));
        }
    }

    /// <summary>
    /// The VBE handles workspace edits sent from the RDE by synchronizing the specified documents from the workspace into the VBE.
    /// </summary>
    public class ApplyWorkspaceEditHandler : ApplyWorkspaceEditHandlerBase
    {
        private readonly ServerPlatformServiceHelper _service;

        public ApplyWorkspaceEditHandler(ServerPlatformServiceHelper service)
        {
            _service = service;
        }

        public async override Task<ApplyWorkspaceEditResponse> Handle(ApplyWorkspaceEditParams request, CancellationToken cancellationToken)
        {
            /**
             * Depending on the client capability
             * `workspace.workspaceEdit.resourceOperations` document changes are either
             * an array of `TextDocumentEdit`s to express changes to n different text
             * documents where each text document edit addresses a specific version of
             * a text document. Or it can contain above `TextDocumentEdit`s mixed with
             * create, rename and delete file / folder operations.
             *
             * Whether a client supports versioned document edits is expressed via
             * `workspace.workspaceEdit.documentChanges` client capability.
             *
             * If a client neither supports `documentChanges` nor
             * `workspace.workspaceEdit.resourceOperations` then only plain `TextEdit`s
             * using the `changes` property are supported.
             * https://microsoft.github.io/language-server-protocol/specifications/lsp/3.17/specification/#workspaceEdit
            **/

            cancellationToken.ThrowIfCancellationRequested();
            var synchronized = new HashSet<DocumentUri>();

            ApplyWorkspaceEditResponse? response = default;
            int? errorIndex = default;
            string? errorMessage = default;

            _service.TryRunAction(() =>
            {
                var changes = request.Edit.DocumentChanges?.GroupByDocumentUri()
                    ?? throw new InvalidRequestParamsException(nameof(request.Edit.DocumentChanges), request);

                foreach (var uri in changes.Keys)
                {
                    foreach (var (change, index) in changes[uri])
                    {
                        if (change.IsTextDocumentEdit && !synchronized.Contains(uri))
                        {
                            var path = uri.Path;
                            // TODO sync VBE from file

                            synchronized.Add(uri);
                            break;
                        }
                        else if (change.IsRenameFile && !synchronized.Contains(uri))
                        {
                            // TODO remove old-name module from VBA project
                            break;
                        }
                        else if (change.IsCreateFile && !synchronized.Contains(uri))
                        {
                            // TODO sync (straight up import) VBE from file
                            break;
                        }
                    }
                }

                response = new ApplyWorkspaceEditResponse { Applied = true };
            });

            var result = response ?? new ApplyWorkspaceEditResponse
            {
                Applied = false,
                FailedChange = errorIndex,
                FailureReason = errorMessage,
            };

            return await Task.FromResult(result);
        }
    }
}
