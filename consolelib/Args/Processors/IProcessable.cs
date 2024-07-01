namespace CoolandonRS.consolelib.Args.Processors;

public interface IProcessable {
    /// <inheritdoc cref="IArgProcessor{T}.Process"/>
    internal ArgProcessingStatus Process(string[] allArgs, string token, ref int idx);
    
    /// <summary>
    /// Whether the token should be passed to the processor
    /// <remarks>Typically determined using regex</remarks>
    /// </summary>
    internal bool ShouldProcess(string token);
}
