namespace CoolandonRS.consolelib.DefaultArgs; 

public class AliasedStringArg : AliasedArg<string> {

    public static AliasedStringArg Create(string desc, string @default, char[] delim, params (string name, Args.Syntax syntax)[] aliases) {
        return new AliasedStringArg(aliases[0].name, desc, @default, Args.GetAliasValidator(Args.Type.Value, delim, aliases));
    }
    protected AliasedStringArg(string name, string desc, string @default, Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)>? validate = null, Func<string, bool>? convert = null, char[]? delims = null) : base(name, desc, Args.Type.Value, @default, validate, str => str, delims) {
    }
}