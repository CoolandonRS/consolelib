using Metadata = CoolandonRS.consolelib.Args.Processors.ProcessorMetadata;
using Status = CoolandonRS.consolelib.Args.Processors.ArgProcessingStatus;

namespace CoolandonRS.consolelib.Args.Processors;

/// <summary>
/// Processes arguments that are seperated by a space (unless surrounded by quotes)
/// </summary>
public class SplitValueProcessor<T>(Func<string, T> cast, T @default): IArgProcessor<T> {
    private bool set = false;
    
    public Status Process(string[] allArgs, string token, ref int idx) {
        return Process(allArgs, token, ref idx, ref set, cast, ref @default);
    }

    public Metadata GetCurrentMetadata() => Metadata.New<T>(Bundleability.LastOnly, set);

    public T Extract() => @default;
    
    internal static Status Process(string[] allArgs, string token, ref int idx, ref bool set, Func<string, T> cast, ref T @default) {
        if (set) return Status.AlreadySet;
        try {
            @default = cast(allArgs[idx + 1]);
            idx += 1;
            set = true;
            return Status.Success;
        } catch (IndexOutOfRangeException) {
            return Status.InsufficientData;
        } catch (InvalidCastException) {
            return Status.FailedCast;
        } catch (InvalidDataException) {
            return Status.FailedCast;
        } catch (InvalidOperationException) {
            return Status.FailedCast;
        }
    }
}
