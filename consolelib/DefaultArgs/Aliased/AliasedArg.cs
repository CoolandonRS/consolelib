using System.Data;

namespace CoolandonRS.consolelib.DefaultArgs; 

public class AliasedArg<T> : Arg<T> {

    public static AliasedArg<T> Create(string desc, Args.Type type, T @default, char[]? delims = null, Func<string, T>? convert = null, params (string name, Args.Syntax syntax)[] aliases) {
        if (type == Args.Type.Verb && aliases.Any(a => a.syntax != Args.Syntax.Implicit)) throw new InvalidOperationException("All aliases of a verb must be implicit");
        return new AliasedArg<T>(aliases[0].name, desc, type, @default, Args.GetAliasValidator(type, delims, aliases), convert, delims);
    }

    protected AliasedArg(string name, string desc, Args.Type type, T @default, Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)>? validate = null, Func<string, T>? convert = null, char[]? delims = null) : base(name, desc, type, Args.Syntax.SelfValidating, @default, validate, convert, delims) {
    }
}