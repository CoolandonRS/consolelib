namespace CoolandonRS.consolelib.DefaultArgs; 

public class LongArg : Arg<long>{
    public LongArg(string name, string desc, Args.Syntax syntax, long @default, char[] delims) : base(name, desc, Args.Type.Value, syntax, @default, null, long.Parse, delims) {
    }
}