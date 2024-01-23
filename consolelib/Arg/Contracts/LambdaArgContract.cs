using System.Runtime.Versioning;

namespace CoolandonRS.consolelib.Arg.Contracts;

[RequiresPreviewFeatures]
internal class LambdaArgContract : IArgContract {
    private readonly Func<ArgHandler, IArgContract.Result> func;

    public IArgContract.Result Eval(ArgHandler handler) => func(handler);

    public LambdaArgContract(Func<ArgHandler, IArgContract.Result> func, string? msg = null) => this.func = func;
}