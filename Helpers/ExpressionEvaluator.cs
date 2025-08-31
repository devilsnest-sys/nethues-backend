using System.Globalization;
namespace ArithmeticChat.Helpers;
public sealed class ExpressionEvaluator
{
    // Shunting-yard implementation (supports + - * / ( ) and decimals)
    public decimal Evaluate(string expr)
    {
        if (string.IsNullOrWhiteSpace(expr)) throw new ArgumentException("Expression empty");
        var tokens = Tokenize(expr);
        var rpn = ToRpn(tokens);
        return EvalRpn(rpn);
    }
    private static IEnumerable<string> Tokenize(string s)
    {
        int i = 0;
        while (i < s.Length)
        {
            char c = s[i];
            if (char.IsWhiteSpace(c)) { i++; continue; }
            if ("+-*/()".IndexOf(c) >= 0) { yield return c.ToString(); i++; continue; }
            if (char.IsDigit(c) || c == '.')
            {
                int start = i; while (i < s.Length && (char.IsDigit(s[i]) || s[i] == '.')) i++;
                yield return s[start..i]; continue;
            }
            throw new ArgumentException($"Invalid character '{c}' in expression.");
        }
    }
    private static int Prec(string op) => op switch { "+" or "-" => 1, "*" or "/" => 2, _ => 0 };
    private static IEnumerable<string> ToRpn(IEnumerable<string> tokens)
    {
        var outq = new List<string>(); var ops = new Stack<string>();
        foreach (var t in tokens)
        {
            if (decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out _)) outq.Add(t);
            else if ("+-*/".Contains(t))
            {
                while (ops.Count > 0 && "+-*/".Contains(ops.Peek()) && Prec(ops.Peek()) >= Prec(t)) outq.Add(ops.Pop());
                ops.Push(t);
            }
            else if (t == "(") ops.Push(t);
            else if (t == ")") { while (ops.Count > 0 && ops.Peek() != "(") outq.Add(ops.Pop()); if (ops.Count == 0 || ops.Pop() != "(") throw new ArgumentException("Mismatched parentheses"); }
        }
        while (ops.Count > 0) { var op = ops.Pop(); if (op == "(" || op == ")") throw new ArgumentException("Mismatched parentheses"); outq.Add(op); }
        return outq;
    }
    private static decimal EvalRpn(IEnumerable<string> rpn)
    {
        var st = new Stack<decimal>();
        foreach (var t in rpn)
        {
            if (decimal.TryParse(t, NumberStyles.Number, CultureInfo.InvariantCulture, out var n)) { st.Push(n); }
            else
            {
                if (st.Count < 2) throw new ArgumentException("Invalid expression");
                var b = st.Pop(); var a = st.Pop();
                st.Push(t switch { "+" => a + b, "-" => a - b, "*" => a * b, "/" => b == 0 ? throw new DivideByZeroException() : a / b, _ => throw new ArgumentException($"Unknown op {t}") });
            }
        }
        if (st.Count != 1) throw new ArgumentException("Invalid expression");
        return st.Pop();
    }
}
