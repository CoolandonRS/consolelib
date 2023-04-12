using System.Text;
using System.Text.RegularExpressions;

namespace CoolandonRS.consolelib; 

public class ArgHandler {
    private Dictionary<string, ArgData> valueArgs;

    private Dictionary<char, FlagData> flagArgs;
    public bool HasErrors { get; private set; } = false; 
    public string GetHelpString() {
        var builder = new StringBuilder();
        foreach (var data in flagArgs.Values) {
            builder.Append(data.Desc.Template + "\n\t" + data.Desc.Desc + "\n");
        }
        foreach (var data in  valueArgs.Values) {
            builder.Append(data.Desc.Template + "\n\t" + data.Desc.Desc + "\n");
        }
        return builder.ToString().TrimEnd();
    }
    
    /// <summary>
    /// ? is always considered the "help" flag <br/>
    /// Values Cannot contain '='
    /// </summary>
    /// <param name="args"></param>
    public void ParseArgs(string[] args) {
        var newArgs = new List<string>();
        var tempArg = "";
        var inQuote = false;
        foreach (var t in args) {
            if (t.Contains('"')) {
                if (inQuote) {
                    inQuote = false;
                    newArgs.Add(tempArg);
                    tempArg = "";
                } else {
                    inQuote = true;
                }
            }
            if (inQuote) {
                tempArg += t;
            } else if(!t.Contains('"')) {
                newArgs.Add(t);
            }
        }
        Console.WriteLine(newArgs);
        var errs = new List<string>();
        var flagRegex = new Regex("^-[a-zA-Z?]+$");
        var valueRegex = new Regex("^--[\\.\\\\/a-zA-Z0-9:_-]+=[\\.\\\\/a-zA-Z0-9:_-]+$");
        
        var flags = args.Where(str => flagRegex.IsMatch(str));
        var values = args.Where(str => valueRegex.IsMatch(str));
        
        foreach (var flag in flags) {
            var chars = flag.Replace("-", "").ToCharArray();
            foreach (var c in chars) {
                if (!flagArgs.ContainsKey(c)) {
                    errs.Add("Flag '" + c + "' is unknown");
                    continue;
                }
                flagArgs[c].Set();
            }
        }

        if (GetFlag('?')) {
            Console.WriteLine(GetHelpString());
            Environment.Exit(0);
        } else {
            foreach (var err in errs) Console.Error.WriteLine(err);
            if (errs.Count > 0) HasErrors = true;
        }

        foreach (var valuePair in values) {
            var split = valuePair.Replace("--", "").Split("=");
            if (!valueArgs.ContainsKey(split[0])) {
                Console.Error.WriteLine("Value '" + split[0] + "' is unknown");
                HasErrors = true;
                continue;
            }
            valueArgs[split[0]].Set(split[1]);
        }
    }

    public bool GetFlag(char flag) {
        return flagArgs[flag].IsSet;
    }

    public ArgValue GetValue(string value) {
        return valueArgs[value].Value;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">Names must match [\.\\/a-zA-Z0-9:_-]</param>
    /// <param name="flags">? is considered the help flag, and cannot be used</param>
    public ArgHandler(Dictionary<string, ArgData> args, Dictionary<char, FlagData> flags) {
        if (flags.ContainsKey('?')) throw new InvalidOperationException("Cannot set flag ?; reserved for help");
        this.valueArgs = args;
        this.flagArgs = flags;
        flagArgs.Add('?', new FlagData(new ArgDesc("-?", "Shows help")));
    }
}