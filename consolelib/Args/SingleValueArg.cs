namespace CoolandonRS.consolelib.Args; 

public class SingleValueArg<T> : ValueArg<T> {
    public SingleValueArg(string name, string desc, char arg, T @default, Func<string, T> cast) : base(name, desc, $"^{arg}$", @default, cast, [' ']) {
    }

    public override bool IsSingle() => true;
}