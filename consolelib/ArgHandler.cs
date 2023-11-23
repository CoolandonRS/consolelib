using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Net.Security;

namespace CoolandonRS.consolelib; 

public class ArgHandler {
    private readonly Dictionary<string, IArg> namedArgs;
    private readonly IArg[] singleArgs;
    private readonly IArg[] nonSingleArgs;
    private readonly bool doErrors;
    private readonly bool unknownToImplicit;
    private readonly string unknown;
    private readonly string castFailure;
    private readonly string duplicate;
    private readonly string nonTrailingSingleValue;
    private string[] implicitArgs = Array.Empty<string>();


    #region Parse arguments
    
    public void Parse(string[] args) {
        if (args.Any(s => s is "--help" or "-h" or "-?")) {
            // not WriteLine since there is a trailing \n
            Console.Write(GenerateHelp());
            return;
        }
        List<string> implicits = new();
        var allImplicit = false;
        for (var i = 0; i < args.Length; i++) {
            var argStr = args[i];
            if (allImplicit || !argStr.StartsWith("-")) { implicits.Add(argStr); continue; }
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
                    if (doErrors) Console.WriteLine(nonTrailingSingleValue, j);
                    continue;
                }
                try {
                    setStatus = arg.Set(args[++i]);
                } catch (IndexOutOfRangeException) {
                    setStatus = IArg.SetStatus.FailedCast;
                }
                break;
            }
            PrintError($"{c}", setStatus, argMeta, null, true);
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
                        setStatus = IArg.SetStatus.FailedCast;
                    }
                    break;
                default: continue;
            }
            argMeta = (splitResult.prefix, arg.GetType());
            break;
        }
        PrintError(argStr, setStatus, argMeta, implicits, false);
    }
    
    private void PrintError(string argStr, IArg.SetStatus? setStatus, (string, Type)? argMeta, List<string>? implicits, bool single) {
        Contract.Requires((implicits == null) == single);
        if (unknownToImplicit && setStatus == null && !single) {
            implicits!.Add(argStr);
            return;
        }
        if (!doErrors) return;
        switch (setStatus) {
            case null:
                Console.WriteLine(unknown, argStr);
                break;
            case IArg.SetStatus.FailedCast:
                Console.WriteLine(castFailure, argMeta!.Value.Item1, argMeta!.Value.Item2.Name);
                break;
            case IArg.SetStatus.AlreadySet:
                Console.WriteLine(duplicate, argMeta!.Value.Item1);
                break;
            case IArg.SetStatus.Set: break;
            default: throw new InvalidEnumArgumentException();
        }
    }
    
    #endregion Parse arguments


    private const string helpPrefix = "--help, -h, -?\n  Print help\n";
    private readonly char[] regexTrim = { '-', '^', '$' };
    public string GenerateHelp() => namedArgs.Values.Aggregate(helpPrefix, (current, arg) => current + $"{(arg.IsSingle() ? "-" : "--")}{arg.GetRegex().Trim(regexTrim)}\n  {arg.GetDesc()}\n");

    public string[] GetImplicits() => implicitArgs;
    public string GetImplicit(int n) => implicitArgs[n];

    public T Get<T>(string name) {
        var arg = namedArgs[name];
        Contract.Requires(arg.GetType().IsAssignableTo(typeof(T)));
        return (T)arg.Get();
    }

    public bool IsDefault(string name) {
        return namedArgs[name].IsDefault();
    }
    
    public ArgHandler(params IArg[] args) : this(args, true) { }

    public ArgHandler(IArg[] args, bool doErrors, bool unknownToImplicit = false, string unknown = "Unknown Argument {0}", string castFailure = "Invalid Argument {0}, expected {1}", string duplicate = "Duplicate argument {0}", string nonTrailingSingleValue = "Single arguments with values must be the last of a chain.") {
        List<IArg> single = new(), nonSingle = new();
        foreach (var arg in args) { (arg.IsSingle() ? single : nonSingle).Add(arg); }
        this.namedArgs = args.ToDictionary(a => a.GetName(), a => a);
        this.singleArgs = single.ToArray();
        this.nonSingleArgs = nonSingle.ToArray();
        this.doErrors = doErrors;
        this.unknownToImplicit = unknownToImplicit;
        this.unknown = unknown;
        this.castFailure = castFailure;
        this.duplicate = duplicate;
        this.nonTrailingSingleValue = nonTrailingSingleValue;
    }
}