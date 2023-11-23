using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib.Args; 

public class FlagArg : Arg<bool> {
    // while cast is a non-nullable field, we can get away with it since the only use of `cast` is in `Set(string)` which is overridden.
    
    /// <summary>
    /// Recommended to use <see cref="FlagArg(string,bool)"/> unless you know what you're doing.
    /// </summary>
    public FlagArg(string name, string desc, string rgx, bool @default = false) : base(name, desc, new Regex(rgx), null, @default, null) {
    }

    public FlagArg(string name, string desc, bool @default = false) : this(name, desc, $"^--{name}$", @default) {
    }
    
    public override IArg.SetStatus Set(string val) {
        if (!isDefault) return IArg.SetStatus.AlreadySet;
        this.isDefault = false;
        this.val = !this.val;
        return IArg.SetStatus.Set;
    }
}