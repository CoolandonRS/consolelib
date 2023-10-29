namespace CoolandonRS.consolelib.DefaultArgs; 

public class FloatArg : Arg<float> {
    public FloatArg(string name, string desc, Args.Syntax syntax, float @default, char[] delims) : base(name, desc, Args.Type.Value, syntax, @default, null, float.Parse, delims) {
    }
}