using CoolandonRS.consolelib.Args.Help;
using CoolandonRS.consolelib.Args.Processors;

namespace CoolandonRS.consolelib.Args;

// Not typed since it would require a List<AbstractArg> to be of homogenous typing.
public interface IArg: IProcessable {
    int PrefixCount { get; }
    
    /// <summary>
    /// The current metadata of the processor.
    /// </summary>
    public ProcessorMetadata Metadata { get; }
    public HelpEntry HelpEntry { get; }
    
    public object? Get();
}
