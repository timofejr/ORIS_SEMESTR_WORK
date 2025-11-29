using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace MiniTemplateEngine;

public class IfNode : Node
{
    public string Condition { get; }
    public List<Node> TrueBranch { get; }
    public List<Node> FalseBranch { get; }

    public IfNode(string condition, List<Node> trueBranch, List<Node> falseBranch)
    {
        Condition = condition;
        TrueBranch = trueBranch;
        FalseBranch = falseBranch;
    }

    public override string Render(object data)
    {
        bool cond = EvalCondition(Condition, data);
        var branch = cond ? TrueBranch : FalseBranch;
        return string.Join("", branch.Select(n => n.Render(data)));
    }

    private static bool EvalCondition(string condition, object data)
    {
        condition = condition.Trim();
        bool negate = false;

        if (condition.StartsWith("!"))
        {
            negate = true;
            condition = condition.Substring(1).Trim();
        }

        // поддержка операторов сравнения
        string[] operators = { "==", "!=", ">=", "<=", ">", "<" };
        foreach (var op in operators)
        {
            int idx = condition.IndexOf(op, StringComparison.Ordinal);
            if (idx > 0)
            {
                var leftExpr = condition[..idx].Trim();
                var rightExpr = condition[(idx + op.Length)..].Trim();

                var leftVal = VariableNode.GetValueFromExpression(leftExpr, data);
                var rightVal = ParseValue(rightExpr, data);

                bool result = CompareValues(leftVal, rightVal, op);
                return negate ? !result : result;
            }
        }

        // fallback на bool
        var val = VariableNode.GetValueFromExpression(condition, data);
        bool baseResult = val switch
        {
            bool b => b,
            null => false,
            _ => true
        };

        return negate ? !baseResult : baseResult;
    }

    private static object ParseValue(string expr, object data)
    {
        // если строка в кавычках
        if ((expr.StartsWith("\"") && expr.EndsWith("\"")) || (expr.StartsWith("'") && expr.EndsWith("'")))
            return expr.Substring(1, expr.Length - 2);

        // если число
        if (double.TryParse(expr, NumberStyles.Any, CultureInfo.InvariantCulture, out double num))
            return num;

        // иначе считаем выражением переменной
        return VariableNode.GetValueFromExpression(expr, data);
    }

    private static bool CompareValues(object left, object right, string op)
    {
        if (left is IComparable lComp && right is IComparable rComp)
        {
            try
            {
                // Преобразуем типы если возможно
                double lNum, rNum;
                if (double.TryParse(left?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out lNum) &&
                    double.TryParse(right?.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out rNum))
                {
                    return op switch
                    {
                        "==" => lNum == rNum,
                        "!=" => lNum != rNum,
                        ">"  => lNum > rNum,
                        "<"  => lNum < rNum,
                        ">=" => lNum >= rNum,
                        "<=" => lNum <= rNum,
                        _ => false
                    };
                }

                // Строковое сравнение
                int cmp = string.Compare(left?.ToString(), right?.ToString(), StringComparison.OrdinalIgnoreCase);
                return op switch
                {
                    "==" => cmp == 0,
                    "!=" => cmp != 0,
                    ">"  => cmp > 0,
                    "<"  => cmp < 0,
                    ">=" => cmp >= 0,
                    "<=" => cmp <= 0,
                    _ => false
                };
            }
            catch { return false; }
        }

        // fallback
        return op switch
        {
            "==" => Equals(left, right),
            "!=" => !Equals(left, right),
            _ => false
        };
    }
}
