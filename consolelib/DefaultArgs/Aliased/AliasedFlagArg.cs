namespace CoolandonRS.consolelib.DefaultArgs; 

public class AliasedFlagArg : AliasedArg<bool> {
    public override (bool Found, bool ConsumedNext) ValidateAndSet((string Cur, string Next) strs) {
        var result = validate(strs);
        if (result.Found) val = !val;
        return (result.Found, result.ConsumedNext);
    }
    
    public AliasedFlagArg Create(string desc, bool @default = false, params (string name, Args.Syntax syntax)[] aliases) {
        return new AliasedFlagArg(aliases[0].name, desc, @default, Args.GetAliasValidator(Args.Type.Flag, null, aliases));
    }
    protected AliasedFlagArg(string name, string desc, bool @default, Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)>? validate = null, Func<string, bool>? convert = null, char[]? delims = null) : base(name, desc, Args.Type.Flag, @default, validate, convert, delims) {
    }
}