namespace CoolandonRS.consolelib.Arg.Builders; 

public class SingleFlagArg : FlagArg {
    public SingleFlagArg(string name, string desc, char flag, bool @default = false) : base(name, desc, $"^{flag}$", @default) {
    }

    public override bool IsSingle() => true;
}