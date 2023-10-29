using System.Reflection;
using System.Runtime.ConstrainedExecution;

namespace CoolandonRS.consolelib;

public class Arg<T> {
    protected T val;
    public readonly string name;
    public readonly string desc;
    public readonly Args.Type type;
    public readonly Args.Syntax syntax;
    protected readonly Func<string, T>? convert;
    protected readonly Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)> validate;
    public readonly char[] delims;

    public T Get() => val;

    public virtual (bool Found, bool ConsumedNext) ValidateAndSet((string Cur, string Next) strs) {
        var result = validate(strs);
        if (!result.Found) return (false, false);
        if (type != Args.Type.Value) return (true, result.ConsumedNext);
        try {
            val = convert!(result.Value!);
        } catch (Exception e) {
            throw new ArgParseException(name, e);
        }
        return (true, result.ConsumedNext);
    }

public Arg(string name, string desc, Args.Type type, Args.Syntax syntax, T @default, Func<(string Cur, string Nxt), (bool Found, bool ConsumedNext, string? Value)>? validate = null, Func<string, T>? convert = null, char[]? delims = null) {
        if (name.Length < 1) throw new InvalidOperationException("Length must be > 1");
        if (syntax == Args.Syntax.Single && name.Length != 1) throw new InvalidOperationException("When syntax is single, length must be 1");
        if (name[0] == '-') throw new InvalidOperationException("First character of name must not be -");
        if ((type == Args.Type.Value) != (delims == null || delims.Length == 0)) throw new InvalidOperationException("Delims must be specified if and only if type is value");
        if ((type == Args.Type.Value) != (convert != null)) throw new InvalidOperationException("Convert must be specified if and only if type is value");
        if ((syntax == Args.Syntax.SelfValidating) != (validate == null)) throw new InvalidOperationException("Validate must be specified if and only if syntax is self-validating");
        if (this.type == Args.Type.Verb && this.syntax != Args.Syntax.Implicit) throw new InvalidOperationException("Verbs must be implicit");
        val = @default;
        this.name = name;
        this.desc = desc;
        this.type = type;
        this.syntax = syntax;
        this.delims = delims ?? Array.Empty<char>();
        this.convert = convert;
        this.validate = validate ?? Args.GetPlainValidator(name, type, syntax, this.delims);
    }
}