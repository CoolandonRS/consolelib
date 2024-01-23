using System.ComponentModel;

namespace CoolandonRS.consolelib.Arg.Contracts;

public static class Extensions {
    public static IArgContract.Status Invert(this IArgContract.Status s) => s switch {
        IArgContract.Status.Fulfilled => IArgContract.Status.Unfulfilled,
        IArgContract.Status.Ignored => IArgContract.Status.Ignored,
        IArgContract.Status.Unfulfilled => IArgContract.Status.Fulfilled,
        _ => throw new InvalidEnumArgumentException()
    };

}