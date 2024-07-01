namespace CoolandonRS.consolelib.Args.Processors;

/// <summary>
/// Bundleability assumes that there is not an unambiguous delimiter between the bundled items. As such processors that require additional data are not typically bundleable.
/// </summary>
public enum Bundleability {
    /// <summary>
    /// A processor is bundleable under any circumstance
    /// </summary>
    Full, 
    /// <summary>
    /// A processor is bundleable if and only if it's token is the last in the bundle
    /// </summary>
    LastOnly, 
    /// <summary>
    /// A processor is never bundleable
    /// </summary>
    None
}
