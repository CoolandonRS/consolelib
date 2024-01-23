using System.ComponentModel;
using System.Runtime.Versioning;

namespace CoolandonRS.consolelib.Arg.Contracts;

[RequiresPreviewFeatures]
public static class Extensions {
    public static IArgContract.Status Invert(this IArgContract.Status s) => s switch {
        IArgContract.Status.Fulfilled => IArgContract.Status.Unfulfilled,
        IArgContract.Status.Ignored => IArgContract.Status.Ignored,
        IArgContract.Status.Unfulfilled => IArgContract.Status.Fulfilled,
        _ => throw new InvalidEnumArgumentException()
    };

}