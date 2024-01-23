using System.Runtime.Versioning;

namespace CoolandonRS.consolelib.Arg.Contracts;

[RequiresPreviewFeatures]
public partial interface IArgContract {
    public enum Status {
        Fulfilled, Ignored, Unfulfilled
    }
        
    public Result Eval(ArgHandler handler);
}