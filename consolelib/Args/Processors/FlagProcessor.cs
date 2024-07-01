using Metadata = CoolandonRS.consolelib.Args.Processors.ProcessorMetadata;
using Status = CoolandonRS.consolelib.Args.Processors.ArgProcessingStatus;

namespace CoolandonRS.consolelib.Args.Processors;

public class FlagProcessor(bool @default = false): IArgProcessor<bool> {
    private bool set = false;

    public Status Process(string[] allArgs, string token, ref int idx) {
        if (set) return Status.AlreadySet;
        set = true;
        return Status.Success;
    }

    public Metadata GetCurrentMetadata() => Metadata.New<bool>(Bundleability.Full, set);

    public bool Extract() => set != @default;
}
