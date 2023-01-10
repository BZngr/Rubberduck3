﻿using System.Text.Json.Serialization;

namespace Rubberduck.InternalApi.RPC.LSP.Response
{
    public class CompletionItem
    {
        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("labelDetails")]
        public CompletionItemLabelDetails LabelDetails { get; set; }

        [JsonPropertyName("kind")]
        public int Kind { get; set; } = Constants.CompletionItemKind.Text;

        [JsonPropertyName("tags")]
        public int[] Tags { get; set; }

        /// <summary>
        /// A human-readable string with additional information about this item, like type or symbol information.
        /// </summary>
        [JsonPropertyName("detail")]
        public string Detail { get; set; }

        /// <summary>
        /// A human-readable string that represents a doc-comment.
        /// </summary>
        [JsonPropertyName("documentation")]
        public MarkupContent Documentation { get; set; }

        /// <summary>
        /// If <c>true</c> (and <c>false</c> for all other items in the list), item will be pre-selected when shown.
        /// </summary>
        /// <remarks>
        /// Client has the final say.
        /// </remarks>
        [JsonPropertyName("preselect")]
        public bool PreSelect { get; set; }

        /// <summary>
        /// A string that should be used when comparing this item with other items.
        /// </summary>
        /// <remarks>
        /// If unespecified, Label is used instead.
        /// </remarks>
        [JsonPropertyName("sortText")]
        public string SortText { get; set; }

        /// <summary>
        /// A string that should be used when filtering a set of completion items.
        /// </summary>
        /// <remarks>
        /// If unespecified, Label is used instead.
        /// </remarks>
        [JsonPropertyName("filterText")]
        public string FilterText { get; set; }

        /// <summary>
        /// A string that should be inserted into a document when selecting this completion.
        /// </summary>
        /// <remarks>
        /// If unespecified, Label is used instead.
        /// </remarks>
        [JsonPropertyName("insertText")]
        public string InsertText { get; set; }

        [JsonPropertyName("insertTextMode")]
        public int? InsertTextMode { get; set; } = Constants.InsertTextMode.AdjustIndentation;

        /// <summary>
        /// An edit that is applied to a document when selecting this completion.
        /// </summary>
        /// <remarks>
        /// If specified, then the value of <c>InsertText</c> is ignored.
        /// </remarks>
        [JsonPropertyName("textEdit")]
        public TextEdit TextEdit { get; set; }

        /// <summary>
        /// An optional array of edits that are applied when selecting this completion.
        /// </summary>
        /// <remarks>
        /// Additional text edits should be used to change text <em>unrelated</em> to the current cursor position.
        /// </remarks>
        [JsonPropertyName("additionalTextEdits")]
        public TextEdit[] AdditionalTextEdits { get; set; }

        /// <summary>
        /// An optional set of characters that when pressed while this completion is active, will accept it first and then type that character.
        /// </summary>
        /// <remarks>
        /// All commit character strings should have a length of 1; superfluous characters will be ignored.
        /// </remarks>
        [JsonPropertyName("commitCharacters")]
        public string[] CommitCharacters { get; set; }

        /// <summary>
        /// An optional command that is executed <em>after</em> inserting this completion.
        /// </summary>
        [JsonPropertyName("command")]
        public Command Command { get; set; }

        /// <summary>
        /// A data entry field that is preserved on a completion item between a completion and a subsequent completion resolve request.
        /// </summary>
        [JsonPropertyName("data")]
        public LSPAny Data { get; set; }
    }
}
