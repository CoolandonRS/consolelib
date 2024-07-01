using Metadata = CoolandonRS.consolelib.Args.Processors.ProcessorMetadata;
using Status = CoolandonRS.consolelib.Args.Processors.ArgProcessingStatus;

namespace CoolandonRS.consolelib.Args.Processors;

/// <summary>
/// Acts as a bridge between <see cref="DelimitedValueProcessor{T}"/> and <see cref="SplitValueProcessor{T}"/>
/// </summary>
public class ValueProcessor<T>(Func<string, T> cast, T @default, char[]? delimiters = null): IArgProcessor<T> {
    public readonly char[] Delimiters = delimiters ?? ['=', ':', ' '];
    private bool set = false;


    public Status Process(string[] allArgs, string token, ref int idx) {
        var delimitedProcess = DelimitedValueProcessor<T>.Process(allArgs, token, ref idx, ref set, cast, ref @default, Delimiters);
        if (delimitedProcess != Status.InsufficientData) return delimitedProcess;
        return SplitValueProcessor<T>.Process(allArgs, token, ref idx, ref set, cast, ref @default);
    }

    public Metadata GetCurrentMetadata() => Metadata.New<T>(Bundleability.LastOnly, set);

    public T Extract() => @default;
}
