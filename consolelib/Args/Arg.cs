using System.Text.RegularExpressions;
using CoolandonRS.consolelib.Args.Help;
using CoolandonRS.consolelib.Args.Processors;

namespace CoolandonRS.consolelib.Args;

/// <param name="regex">The regex that determines if the argument should be processed</param>
/// <param name="helpEntry">The messages shown when --help is used</param>
/// <param name="processor">The argument processor</param>
public class Arg<T>(Regex regex, int prefixCount, HelpEntry helpEntry, IArgProcessor<T?> processor): IArg {
    public int PrefixCount => prefixCount;
    public ProcessorMetadata Metadata => processor.GetCurrentMetadata();
    public HelpEntry HelpEntry => helpEntry;
    
    public ArgProcessingStatus Process(string[] allArgs, string token, ref int idx) => processor.Process(allArgs, token, ref idx);
    
    public bool ShouldProcess(string token) => regex.IsMatch(token);
    
    public object? Get() => processor.Extract();
    
    public static Arg<T> New(char c, HelpEntry helpEntry, IArgProcessor<T> processor, int prefixCount = 1) => new(new Regex(Regex.Escape(c.ToString())), prefixCount, helpEntry, processor);
    public static Arg<T> New(string str, HelpEntry helpEntry, IArgProcessor<T> processor, int prefixCount = 2) => new(new Regex(Regex.Escape(str)), prefixCount, helpEntry, processor);
}
