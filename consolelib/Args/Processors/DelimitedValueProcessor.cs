using Metadata = CoolandonRS.consolelib.Args.Processors.ProcessorMetadata;
using Status = CoolandonRS.consolelib.Args.Processors.ArgProcessingStatus;

namespace CoolandonRS.consolelib.Args.Processors;

/// <summary>
/// Processes arguments that are seperated by a non-space character (or a space if surrounded by quotes)
/// </summary>
public class DelimitedValueProcessor<T>(Func<string, T> cast, T @default, char[]? delimiters = null): IArgProcessor<T> {
    public readonly char[] Delimiters = delimiters ?? [':', '=', ' '];
    private bool set = false;

    public Status Process(string[] allArgs, string token, ref int idx) {
        return Process(allArgs, token, ref idx, ref set, cast, ref @default, Delimiters);
    }

    public Metadata GetCurrentMetadata() => Metadata.New<T>(Bundleability.None, set);

    public T Extract() => @default;
    
    internal static Status Process(string[] allArgs, string token, ref int idx, ref bool set, Func<string, T> cast, ref T @default, char[] delimiters) {
        if (set) return Status.AlreadySet;
        var splitIdx = token.IndexOfAny(delimiters);
        if (splitIdx is -1) return Status.InsufficientData;
        try {
            @default = cast(token[(splitIdx + 1)..]);
            set = true;
            return Status.Success;
        } catch (InvalidCastException) {
            return Status.FailedCast;
        } catch (InvalidDataException) {
            return Status.FailedCast;
        } catch (InvalidOperationException) {
            return Status.FailedCast;
        }
    }
}
