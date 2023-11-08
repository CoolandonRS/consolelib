using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib; 

public class Arg<T> {
    protected Regex prefixRgx;
    protected ArgValueSplit? valueSplit;
    protected Func<string, T> cast;
    public T val { get; protected set; }
    public bool isDefault { get; protected set; }

    internal enum SetStatus {
        Set, NoCast, AlreadySet
    }

    internal SetStatus Set(string val) {
        if (!isDefault) return SetStatus.AlreadySet;
        try {
            this.val = cast(val);
            return SetStatus.Set;
        } catch (InvalidCastException) {
            return SetStatus.NoCast;
        }
    }
    
    public Arg(Regex validator, ArgValueSplit? split, T @default, Func<string, T> cast) {
        prefixRgx = validator;
        valueSplit = split;
        val = @default;
        isDefault = true;
        this.cast = cast;
    }
}