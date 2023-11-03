﻿using Rubberduck.LanguageServer.Model;
using System;

namespace Rubberduck.Editor.Document.Tabs
{
    /// <summary>
    /// A view model for a type of document tab that contains code for a Visual Basic 6.0 module/component.
    /// </summary>
    public class VB6CodeDocumentTabViewModel : CodeDocumentTabViewModel
    {
        public VB6CodeDocumentTabViewModel(Uri documentUri, string title, string content, bool isReadOnly = false)
            : base(documentUri, VisualBasicLanguage.LanguageId, title, content, isReadOnly)
        {
        }
    }
}
