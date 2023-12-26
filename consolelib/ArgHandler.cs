using System.ComponentModel;
using System.Diagnostics.Contracts;
using CoolandonRS.consolelib.ArgContract;

namespace CoolandonRS.consolelib; 

public class ArgHandler {
    private readonly Dictionary<string, IArg> namedArgs;
    private readonly IArg[] singleArgs;
    private readonly IArg[] nonSingleArgs;
    private string[] implicitArgs = [];
    private readonly ArgHandlerConfig config;

    #region Parse arguments
    
    public void Parse(string[] args) {
        if (args.Any(s => s is "--help" or "-h" or "-?")) {
            // not WriteLine since there is a trailing \n
            Console.Write(GenerateHelp());
            return;
        }
        List<string> implicits = [];
        var allImplicit = false;
        for (var i = 0; i < args.Length; i++) {
            var argStr = args[i];
            if (allImplicit || !argStr.StartsWith('-')) { implicits.Add(argStr); continue; }
            if (argStr == "--") { allImplicit = true; continue; }
            if (!argStr.StartsWith("--")) { ParseSingle(args, argStr, ref i); continue; }
            ParseNonSingle(args, argStr, ref i, implicits);
        }
        implicitArgs = implicits.ToArray();
    }

    private void ParseSingle(string[] args, string argStr, ref int i) {
        var chars = argStr[1..].ToCharArray();
        var last = chars.Length - 1;
        IArg.SetStatus? setStatus = null;
        (string, Type)? argMeta = null;
        for (var j = 0; j < chars.Length; j++) {
            var c = chars[j];
            foreach (var arg in singleArgs) {
                if (!arg.Validate($"{c}")) continue;
                if (arg.GetSplit() == null) {
                    setStatus = arg.Set("");
                    break;
                }
                if (j != last) {
                    if (config.DoErrors) {
                        if (config.ErrorStyle == ArgHandlerConfig.ArgErrorStyle.Throw) throw new NonTrailingSingleValueArgException();
                        Console.WriteLine(config.NonTrailingSingleValueMessage, j);
                    }
                    continue;
                }
                try {
                    setStatus = arg.Set(args[++i]);
                } catch (IndexOutOfRangeException) {
                    setStatus = IArg.SetStatus.Insufficient;
                }
                break;
            }
            DoError($"{c}", setStatus, argMeta, null, true);
        }
    }

    private void ParseNonSingle(string[] args, string argStr, ref int i, List<string> implicits) {
        IArg.SetStatus? setStatus = null;
        (string, Type)? argMeta = null;
        foreach (var arg in nonSingleArgs) {
            var split = arg.GetSplit();
            if (split == null) {
                if (arg.Validate(argStr)) { setStatus = arg.Set(""); argMeta = (argStr, arg.GetType()); break; }
                continue;
            }
            var splitResult = split.Parse(argStr);
            if (!arg.Validate(splitResult.prefix)) continue;
            switch (splitResult.status) {
                case ArgValueSplit.Status.Success:
                    setStatus = arg.Set(splitResult.postfix!);
                    break;
                case ArgValueSplit.Status.Advance:
                    try {
                        setStatus = arg.Set(args[++i]);
                    } catch (IndexOutOfRangeException) {
                        setStatus = IArg.SetStatus.Insufficient;
                    }
                    break;
                default: continue;
            }
            argMeta = (splitResult.prefix, arg.GetType());
            break;
        }
        DoError(argStr, setStatus, argMeta, implicits, false);
    }
    
    private void DoError(string argStr, IArg.SetStatus? setStatus, (string, Type)? argMeta, List<string>? implicits, bool single) {
        if ((implicits == null) != single) throw new InvalidOperationException();
        if (config.UnknownToImplicit && setStatus == null && !single) {
            implicits!.Add(argStr);
            return;
        }
        if (!config.DoErrors) return;
        switch (setStatus) {
            case null:
                if (config.ErrorStyle == ArgHandlerConfig.ArgErrorStyle.Throw) throw new UnknownArgException();
                Console.WriteLine(config.UnknownArgMessage, argStr);
                break;
            case IArg.SetStatus.FailedCast:
                if (config.ErrorStyle == ArgHandlerConfig.ArgErrorStyle.Throw) throw new CastFailureException();
                Console.WriteLine(config.CastFailureMessage, argMeta!.Value.Item1, argMeta!.Value.Item2.Name);
                break;
            case IArg.SetStatus.AlreadySet:
                if (config.ErrorStyle == ArgHandlerConfig.ArgErrorStyle.Throw) throw new DuplicateArgException();
                Console.WriteLine(config.DuplicateMessage, argMeta!.Value.Item1);
                break;
            case IArg.SetStatus.Insufficient:
                if (config.ErrorStyle == ArgHandlerConfig.ArgErrorStyle.Throw) throw new InsufficientDataException();
                Console.WriteLine(config.InsufficientMessage, argMeta!.Value.Item1);
                break;
            case IArg.SetStatus.Set: break;
            default: throw new InvalidEnumArgumentException();
        }
    }
    
    #endregion Parse arguments

    private const string helpPrefix = "--help, -h, -?\n  Print help\n";
    private readonly char[] regexTrim = { '-', '^', '$' };
    public virtual string GenerateHelp() => namedArgs.Values.Aggregate(helpPrefix, (current, arg) => current + $"{arg.GetCall() ?? (arg.IsSingle() ? "-" : "--") + arg.GetRegex().Trim(regexTrim)}\n  {arg.GetDesc()}\n");

    public string[] GetImplicits() => implicitArgs;
    public string GetImplicit(int n) => implicitArgs[n];

    public T Get<T>(string name) {
        var arg = namedArgs[name];
        if (!arg.Type().IsAssignableTo(typeof(T))) throw new InvalidCastException();
        return (T)arg.Get();
    }

    public bool IsDefault(string name) => namedArgs[name].IsDefault();

    /// <summary>
    /// Validates given a set of ArgContracts.
    /// </summary>
    /// <seealso cref="ArgContracts"/>
    public bool Validate(params IArgContract[] contracts) => ArgContracts.And(contracts).Eval(this);

    public ArgHandler(params IArg[] args) : this(new ArgHandlerConfig(), args) { }
    public ArgHandler(ArgHandlerConfig config, params IArg[] args) : this(args, config) { }
    public ArgHandler(IArg[] args, ArgHandlerConfig config = new()) {
        List<IArg> single = [], nonSingle = [];
        foreach (var arg in args) { (arg.IsSingle() ? single : nonSingle).Add(arg); }
        this.namedArgs = args.ToDictionary(a => a.GetName(), a => a);
        this.singleArgs = single.ToArray();
        this.nonSingleArgs = nonSingle.ToArray();
        this.config = config;
    }
}