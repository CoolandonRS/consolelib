using Metadata = CoolandonRS.consolelib.Args.Processors.ProcessorMetadata;
using Status = CoolandonRS.consolelib.Args.Processors.ArgProcessingStatus;

namespace CoolandonRS.consolelib.Args.Processors;

public class CountProcessor(bool alwaysHasValue = true): IArgProcessor<int> {
    private int count = 0;

    public Status Process(string[] allArgs, string token, ref int idx) {
        count += 1;
        return Status.Success;
    }

    public Metadata GetCurrentMetadata() => Metadata.New<int>(Bundleability.Full, alwaysHasValue || count != 0);

    public int Extract() => count;
}
