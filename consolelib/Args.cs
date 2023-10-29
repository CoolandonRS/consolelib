
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib; 

public static class Args {
    public enum Type {
        Value, Flag, Verb
    }

    public enum Syntax {
        Single, Double, SelfValidating, Implicit
    }

    public static Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)> GetValidator(string name, Type type, Syntax syntax, char[] delims) {
        if (delims == new[] { ' ' }) {
            return GetSpaceDelimitedValueValidator(name, syntax);
        }
        var plainValidator = GetPlainValidator(name, type, syntax, delims);
        if (type != Type.Value || !delims.Contains(' ')) return plainValidator;
        var spaceDelimitedValidator = GetSpaceDelimitedValueValidator(name, syntax);
        return strs => {
            var plain = plainValidator(strs);
            return plain.Found ? plain : spaceDelimitedValidator(strs);
        };
    }

    internal static Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)> GetPlainValidator(string name, Type type, Syntax syntax, char[] delims) {
        var indicator = $"{GetPrefix(syntax)}{name}";
        if (type == Type.Value) {
            var delimString = GetDelimString(delims);
            indicator += $"[{delimString}]([^{delimString}][^ ]+)";
        }
        var rgx = new Regex(indicator)!;
        return strs => {
            var matches = rgx.Match(strs.Cur);
            return matches.Success == false ? (false, false, null) : (true, false, type != Type.Value ? null : matches.Groups[1].Value);
        };
    }

    internal static Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)> GetSpaceDelimitedValueValidator(string name, Syntax syntax) {
        var indicator = $"{GetPrefix(syntax)}{name}";
        var rgx = new Regex(indicator);
        return strs => rgx.IsMatch(strs.Cur) && strs.Cur[0] != '-' ? (true, true, strs.Nxt) : (false, false, null);
    }

    internal static Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)> GetAliasValidator(Args.Type type, char[]? delims, (string name, Args.Syntax syntax)[] aliases) {
        var validators = aliases.Select(a => GetValidator(a.name, type, a.syntax, delims ?? Array.Empty<char>())).ToArray();
        return Validator;
        // This code is so cursed to my naive eyes, but Rider says to do it.
        // And I'd trust Rider with my life.
        
        (bool Found, bool ConsumedNext, string? Value) Validator((string Cur, string Nxt) strs) {
            foreach (var validator in validators) {
                var dat = validator(strs);
                if (dat.Found) return dat;
            }

            return (false, false, null);
        }
    }

    private static string GetDelimString(char[] delims) {
        var delimString = string.Join("", delims.Distinct());
        if (delimString[0] == '^') delimString = $@"\{delimString}";
        return delimString.Replace("]", @"\]").Replace(@"\", @"\\").Replace(" ", "");
    }

    private static string GetPrefix(Syntax syntax) {
        return syntax switch {
            Syntax.Single => "-",
            Syntax.Double => "--",
            Syntax.Implicit => "",
            Syntax.SelfValidating => throw new InvalidOperationException("Attempted to get validator on a self validating argument"),
            _ => throw new InvalidEnumArgumentException()
        };
    }
}