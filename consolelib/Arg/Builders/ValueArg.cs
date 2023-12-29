using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib.Arg.Builders; 

public class ValueArg<T> : Arg<T> {
    /// <summary>
    /// Recommended to use <see cref="ValueArg{T}(string,T,Func{string,T},char[])"/> unless you know what you're doing.
    /// </summary>
    public ValueArg(string name, string desc, string regex, T @default, Func<string, T> cast, char[]? delims = null) : base(name, desc, new Regex(regex), new ArgValueSplit(delims ?? new[] { ' ', '=', ':' }), @default, cast) {
    }

    public ValueArg(string name, string desc, T @default, Func<string, T> cast, char[]? delims = null) : this(name, desc, $"^--{name}$", @default, cast, delims) {
    }
}