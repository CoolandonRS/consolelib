namespace CoolandonRS.consolelib.DefaultArgs; 

public class IntArg : Arg<int> {
    public IntArg(string name, string desc, Args.Syntax syntax, int @default, char[] delims) : base(name, desc, Args.Type.Value, syntax, @default, null, int.Parse, delims) {
    }
}