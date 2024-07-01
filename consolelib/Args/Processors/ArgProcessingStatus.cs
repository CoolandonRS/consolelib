namespace CoolandonRS.consolelib.Args.Processors;

public enum ArgProcessingStatus {
    /// <summary>
    /// The operation was successful
    /// </summary>
    Success, 
    /// <summary>
    /// Could not parse the argument array into the desired type
    /// </summary>
    FailedCast, 
    /// <summary>
    /// The argument was already set
    /// </summary>
    AlreadySet, 
    /// <summary>
    /// There was not enough data in the argument array
    /// </summary>
    InsufficientData,
    /// <summary>
    /// The data provided was invalid, such as having an unknown argument or a <see cref="Bundleability.LastOnly"/> argument not last in a bundle
    /// </summary>
    InvalidData
}
