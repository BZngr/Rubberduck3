﻿using Rubberduck.Parsing.Abstract;

namespace Rubberduck.Parsing.Expressions;

public sealed class LogicalEqualsExpression : Expression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public LogicalEqualsExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public override IValue Evaluate()
    {
        var left = _left.Evaluate();
        var right = _right.Evaluate();
        if (left == null || right == null)
        {
            return null;
        }
        if (
            (left.ValueType == ValueType.String || left.ValueType == ValueType.Empty)
            && (right.ValueType == ValueType.String || right.ValueType == ValueType.Empty))
        {
            var leftValue = left.AsString;
            var rightValue = right.AsString;
            return new BoolValue(leftValue.CompareTo(rightValue) == 0);
        }
        else
        {
            var leftValue = left.AsDecimal;
            var rightValue = right.AsDecimal;
            return new BoolValue(leftValue == rightValue);
        }
    }
}
