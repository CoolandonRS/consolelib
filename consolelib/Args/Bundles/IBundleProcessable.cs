using CoolandonRS.consolelib.Args.Processors;

namespace CoolandonRS.consolelib.Args.Bundles;

/// <summary>
/// A Processable that behaves differently when processed by a bundle
/// </summary>
public interface IBundleProcessable: IProcessable {
    /// <summary>
    /// If this processable can only exist within a bundle
    /// </summary>
    public bool MustBeBundled { get; }
    
    
    internal ArgProcessingStatus BundleProcess(string[] allArgs, string token, ref int bundleIdx, ref int argIdx);
}
