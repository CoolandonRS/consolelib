using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib; 

/// <summary>
/// Raw class. Recommended to use a helper in <see cref="CoolandonRS.consolelib.Args.ValueArg">CoolandonRS.consolelib.Args</see> unless you know what you're doing.
/// </summary>
public class Arg<T> : IArg {
    protected Regex prefixRgx;
    protected ArgValueSplit? valueSplit;
    protected Func<string, T> cast;
    public T val { get; protected set; }
    public bool isDefault { get; protected set; }
    public readonly string name;
    public readonly string desc;

    public virtual IArg.SetStatus Set(string val) {
        if (!isDefault) return IArg.SetStatus.AlreadySet;
        try {
            this.val = cast(val);
            this.isDefault = false;
            return IArg.SetStatus.Set;
        } catch (SystemException e) when (e is FormatException or InvalidCastException) {
            return IArg.SetStatus.FailedCast;
        }
    }

    public object Get() => val;
    public bool IsDefault() => isDefault;
    public Type Type() => typeof(T);
    public virtual bool IsSingle() => false;
    public bool Validate(string str) => prefixRgx.Match(str).Success;
    public ArgValueSplit? GetSplit() => valueSplit;
    public string GetName() => name;
    public string GetRegex() => prefixRgx.ToString();
    public string GetDesc() => desc;
    public virtual string? GetCall() => null;
    
    public Arg(string name, string desc, Regex validator, ArgValueSplit? split, T @default, Func<string, T> cast) {
        this.name = name;
        this.desc = desc;
        prefixRgx = validator;
        valueSplit = split;
        val = @default;
        isDefault = true;
        this.cast = cast;
    }
}