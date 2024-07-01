namespace CoolandonRS.consolelib.Args.Processors;

using Status = ArgProcessingStatus;
using Metadata = ProcessorMetadata;

public interface IArgProcessor<out T> {
    /// <summary>
    /// Processes the argument array to figure out what value an argument should be set to
    /// </summary>
    /// <param name="allArgs">The argument array as passed into the programs entrypoint</param>
    /// <param name="token">The processed token of your argument (without irrelevant data such as the prefix)</param>
    /// <param name="idx">An index of the argument array; assumed to be currently set to the first value matching the args precondition</param>
    /// <remarks><c>idx</c>should only be incremented to skip elements in the array</remarks>
    /// <returns>The <see cref="Status"/></returns>
    public Status Process(string[] allArgs, string token, ref int idx);

    public Metadata GetCurrentMetadata();
    
    /// <summary>
    /// Returns the result of all the processing operations
    /// </summary>
    /// <returns>The value, or the default if <see cref="GetCurrentMetadata"/>.<see cref="ProcessorMetadata.HasValue"/> is false</returns>
    public T Extract();
}
