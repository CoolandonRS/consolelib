using System.Runtime.Versioning;
#pragma warning disable CA2252 // preview features

namespace CoolandonRS.consolelib.Arg.Contracts;

public readonly partial struct ValidationResult {
    public readonly bool Success;
    public readonly StringHierarchy? Message;


    internal ValidationResult(IArgContract.Result result) : this(result.Status, result.Msg) { }
    internal ValidationResult(IArgContract.Status status, IArgContract.Message message) {
        Success = status is IArgContract.Status.Fulfilled or IArgContract.Status.Ignored;
        Message = StringHierarchy.From(message);
    }
}