using System.Text.RegularExpressions;
using CoolandonRS.consolelib.Args.Processors;

namespace CoolandonRS.consolelib.Args.Bundles;

/// <summary>
/// A bundle where each argument is a single character
/// </summary>
public partial class CharBundle: IArgBundle {
    private static Regex rgx = CharRegex();
    public IArg[] Args => args.ToArray();
    private List<IArg> args = [];
    public void Add(IArg arg) {
        if (arg.Metadata.Bundleability is Bundleability.None) throw new InvalidOperationException();
        args.Add(arg);
    }
    
    public ArgProcessingStatus Process(string[] allArgs, string token, ref int idx) {
        var max = token.Length - 1;
        for (var i = 0; i <= max; i++) {
            var str = token[i].ToString();
            var processed = false;
            foreach (var arg in args) {
                if (!arg.ShouldProcess(str)) continue;
                if (arg.Metadata.Bundleability is Bundleability.LastOnly && i != max) return ArgProcessingStatus.InvalidData;
                var result = arg is IBundleProcessable bundleArg ? bundleArg.BundleProcess(allArgs, str, ref i, ref idx) : arg.Process(allArgs, str, ref idx);
                if (result is not ArgProcessingStatus.Success) return result;
                processed = true;
                break;
            }
            if (!processed) return ArgProcessingStatus.InvalidData;
        }
        return ArgProcessingStatus.Success;
    }

    public bool ShouldProcess(string token) => rgx.IsMatch(token);

    [GeneratedRegex(@"[\w\d]+")]
    private static partial Regex CharRegex();
}
