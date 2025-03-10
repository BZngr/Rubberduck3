using Rubberduck.Unmanaged.Model;

namespace Rubberduck.Parsing.Model.Symbols;

/// <summary>
/// Represents a comment.
/// </summary>
public class CommentNode
{
    /// <summary>
    /// Creates a new comment node.
    /// </summary>
    /// <param name="comment">The comment line text, without the comment marker.</param>
    /// <param name="qualifiedSelection">The information required to locate and select this node in its VBE code pane.</param>
    public CommentNode(string comment, string marker, QualifiedSelection qualifiedSelection)
    {
        CommentText = comment;
        Marker = marker;
        QualifiedSelection = qualifiedSelection;
    }

    public CommentNode(string comment, string marker, QualifiedDocumentOffset qualifiedOffset)
    {
        CommentText = comment;
        Marker = marker;
        QualifiedOffset = qualifiedOffset;
    }

    /// <summary>
    /// Gets the comment text, without the comment marker.
    /// </summary>
    public string CommentText { get; }

    /// <summary>
    /// The token used to indicate a comment.
    /// </summary>
    public string Marker { get; }

    /// <summary>
    /// Gets the information required to locate and select this node in its VBE code pane.
    /// </summary>
    public QualifiedSelection QualifiedSelection { get; }

    /// <summary>
    /// Gets the information required to locate and select this node in its RDE tab.
    /// </summary>
    public QualifiedDocumentOffset QualifiedOffset { get; }

    /// <summary>
    /// Returns the comment text.
    /// </summary>
    public override string ToString() => CommentText;
}
