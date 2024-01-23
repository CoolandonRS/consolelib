using System.ComponentModel;

namespace CoolandonRS.consolelib.Arg.Contracts;

public partial interface IArgContract {
    public readonly struct Result(Status status, Message msg = new()) {
        public readonly Status Status = status;
        public readonly Message Msg = msg;
        
        public Result Invert(string? newMsg) => new(Status.Invert(), newMsg);
        public Result Invert(Message? newMsg = null) => new(Status.Invert(), newMsg ?? Msg);
        public Result Invert(ConditionalString? newMsg) {
            var newStatus = Status.Invert();
            return new Result(newStatus, newMsg?.GetAsMessage(newStatus) ?? Msg);
        }

        public bool WasSuccess(bool strict = false) => Status switch {
            Status.Fulfilled => true,
            Status.Ignored => strict,
            Status.Unfulfilled => false,
            _ => throw new InvalidEnumArgumentException()
        };

        public Result(Status status, string? msg) : this(status, new Message(msg)) { }
        public Result(bool status, string? msg) : this(status, new Message(msg)) { }
        public Result(bool status, Message? msg = null) : this(status ? Status.Fulfilled : Status.Unfulfilled, msg ?? new Message()) { }
        public Result(bool status, ConditionalString? msg) : this(status, msg?.Get(status ? Status.Fulfilled : Status.Unfulfilled)) { }
        public Result(bool status, ConditionalString? msg, Message[]? children) : this(status, new Message(msg?.Get(status ? Status.Fulfilled : Status.Unfulfilled), children)) { }
        public Result(Status status, ConditionalString? msg) : this(status, msg?.Get(status)) { }
        public Result(Status status, ConditionalString? msg, Message[]? children) : this(status, new Message(msg?.Get(status), children)) { }
        public Result(Status status, ConditionalString? msg, string? child) : this(status, new Message(msg?.Get(status), child)) { }
        public Result(Status status, ConditionalString? msg, Message child) : this(status, new Message(msg?.Get(status), child)) { }
    }
}