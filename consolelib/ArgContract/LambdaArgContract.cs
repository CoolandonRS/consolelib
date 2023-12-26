namespace CoolandonRS.consolelib.ArgContract;

internal class LambdaArgContract : IArgContract {
    private readonly Func<ArgHandler, bool> func;

    public bool Eval(ArgHandler handler) => func(handler);

    public LambdaArgContract(Func<ArgHandler, bool> func, string? msg = null) => this.func = func;
}