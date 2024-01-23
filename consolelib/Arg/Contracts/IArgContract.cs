namespace CoolandonRS.consolelib.Arg.Contracts;

public partial interface IArgContract {
    public enum Status {
        Fulfilled, Ignored, Unfulfilled
    }
        
    public Result Eval(ArgHandler handler);
}