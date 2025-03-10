﻿using Antlr4.Runtime;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Tree;
using Rubberduck.Parsing.Exceptions;
using Rubberduck.Parsing.Model;

namespace Rubberduck.Parsing.Abstract;

public abstract class TokenStreamParserBase<TParser> : ITokenStreamParser
    where TParser : Parser
{
    //protected static ILogger Logger = LogManager.GetCurrentClassLogger();

    private readonly IParsePassErrorListenerFactory _sllErrorListenerFactory;
    private readonly IParsePassErrorListenerFactory _llErrorListenerFactory;

    public TokenStreamParserBase(IParsePassErrorListenerFactory sllErrorListenerFactory,
        IParsePassErrorListenerFactory llErrorListenerFactory)
    {
        _sllErrorListenerFactory = sllErrorListenerFactory;
        _llErrorListenerFactory = llErrorListenerFactory;
    }

    protected IParseTree Parse(ITokenStream tokenStream, PredictionMode predictionMode, IParserErrorListener? errorListener, IEnumerable<IParseTreeListener>? parseListeners = null)
    {
        var parser = GetParser(tokenStream);
        parser.Interpreter.PredictionMode = predictionMode;
        if (errorListener != null)
        {
            parser.AddErrorListener(errorListener);
        }
        if (parseListeners != null)
        {
            foreach (var listener in parseListeners)
            {
                parser.AddParseListener(listener);
            }
        }
        return Parse(parser);
    }
    protected abstract TParser GetParser(ITokenStream tokenStream);
    protected abstract IParseTree Parse(TParser parser);
    public IParseTree Parse(string moduleName, string projectId, CommonTokenStream tokenStream, CancellationToken token, CodeKind codeKind = CodeKind.RubberduckEditorModule, ParserMode parserMode = ParserMode.FallBackSllToLl, IEnumerable<IParseTreeListener>? parseListeners = null)
    {
        return parserMode switch
        {
            ParserMode.FallBackSllToLl => ParseWithFallBack(moduleName, tokenStream, codeKind, parseListeners),
            ParserMode.LlOnly => ParseLl(moduleName, tokenStream, codeKind, parseListeners),
            ParserMode.SllOnly => ParseSll(moduleName, tokenStream, codeKind, parseListeners),
            _ => throw new ArgumentException($"Value '{parserMode}' is not supported.", nameof(parserMode)),
        };
    }

    private IParseTree ParseWithFallBack(string moduleName, CommonTokenStream tokenStream, CodeKind codeKind, IEnumerable<IParseTreeListener>? parseListeners = null)
    {
        try
        {
            return ParseSll(moduleName, tokenStream, codeKind, parseListeners);
        }
        catch (ParsePassSyntaxErrorException syntaxErrorException)
        {
            var message = $"SLL mode failed while parsing the {codeKind} version of module {moduleName} at symbol {syntaxErrorException.OffendingSymbol.Text} at L{syntaxErrorException.LineNumber}C{syntaxErrorException.Position}. Retrying using LL.";
            LogAndReset(tokenStream, message, syntaxErrorException);
            return ParseLl(moduleName, tokenStream, codeKind, parseListeners);
        }
        catch (Exception exception)
        {
            var message = $"SLL mode failed while parsing the {codeKind} version of module {moduleName}. Retrying using LL.";
            LogAndReset(tokenStream, message, exception);
            return ParseLl(moduleName, tokenStream, codeKind, parseListeners);
        }
    }

    //This method is virtual only because a CommonTokenStream cannot be mocked in tests
    //and there is no interface for it or a base class that has the Reset member.
    protected virtual void LogAndReset(CommonTokenStream tokenStream, string logWarnMessage, Exception exception)
    {
        //Logger.Warn(logWarnMessage);
        //Logger.Debug(exception);
        tokenStream.Reset();
    }

    private IParseTree ParseLl(string moduleName, ITokenStream tokenStream, CodeKind codeKind, IEnumerable<IParseTreeListener>? parseListeners = null)
    {
        var errorListener = _llErrorListenerFactory.Create(moduleName, codeKind);
        var tree = Parse(tokenStream, PredictionMode.Ll, errorListener, parseListeners);
        if (errorListener.HasPostponedException(out var exception))
        {
            throw exception;
        }
        return tree;
    }

    private IParseTree ParseSll(string moduleName, ITokenStream tokenStream, CodeKind codeKind, IEnumerable<IParseTreeListener>? parseListeners = null)
    {
        var errorListener = _sllErrorListenerFactory.Create(moduleName, codeKind);
        var tree = Parse(tokenStream, PredictionMode.Sll, errorListener, parseListeners);
        if (errorListener.HasPostponedException(out var exception))
        {
            throw exception;
        }
        return tree;
    }
}
