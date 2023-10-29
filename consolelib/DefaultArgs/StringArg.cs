namespace CoolandonRS.consolelib.DefaultArgs; 

public class StringArg : Arg<string> {
    public StringArg(string name, string desc, Args.Syntax syntax, string @default, char[] delims) : base(name, desc, Args.Type.Value, syntax, @default, null, str => str, delims) {
    }
}