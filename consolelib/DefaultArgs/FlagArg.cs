namespace CoolandonRS.consolelib.DefaultArgs; 

public class FlagArg : Arg<bool> {
    public override (bool Found, bool ConsumedNext) ValidateAndSet((string Cur, string Next) strs) {
        var result = validate(strs);
        if (result.Found) val = !val;
        return (result.Found, result.ConsumedNext);
    }

    public FlagArg(string name, string desc, Args.Syntax syntax, bool @default = false) : base(name, desc, Args.Type.Flag, syntax, @default) {
    }
}