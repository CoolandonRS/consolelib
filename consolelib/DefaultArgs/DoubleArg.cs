namespace CoolandonRS.consolelib.DefaultArgs; 

public class DoubleArg : Arg<double> {
    public DoubleArg(string name, string desc, Args.Syntax syntax, double @default, char[] delims) : base(name, desc, Args.Type.Value, syntax, @default, null, double.Parse, delims) {
    }
}