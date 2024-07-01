using CoolandonRS.consolelib.Args.Processors;

namespace CoolandonRS.consolelib.Args.Bundles;

/// <summary>
/// A bundle handles situations when multiple arguments can be found in a single token
/// </summary>
public interface IArgBundle: IProcessable {
    public IArg[] Args { get; }
    
    public void Add(IArg arg);
}
