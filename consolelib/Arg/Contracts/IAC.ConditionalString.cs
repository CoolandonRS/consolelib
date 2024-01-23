using System.ComponentModel;

namespace CoolandonRS.consolelib.Arg.Contracts;

public partial interface IArgContract {
    public readonly struct ConditionalString {
        private readonly Func<Status, string?> func;
    
        public string? Get(Status status) => func(status);
        internal Message GetAsMessage(Status status) => new(func(status));

        public static ConditionalString Always(string? msg = null) => new(_ => msg);

        public static ConditionalString From(string? fulfilled, string? unfulfilled) => From(fulfilled, null, unfulfilled);
        public static ConditionalString From(string? fulfilled, string? ignored, string? unfulfilled) => new(s => s switch {
            Status.Fulfilled => fulfilled,
            Status.Ignored => ignored,
            Status.Unfulfilled => unfulfilled,
            _ => throw new InvalidEnumArgumentException()
        });
    
        public static ConditionalString Lambda(Func<Status, string?> lambda) => new(lambda);

        private ConditionalString(Func<Status, string?> func) => this.func = func;

        /// <remarks>Equivalent to <see cref="Always(string)">ConditionalString.Always(string)</see></remarks>
        public ConditionalString(string? msg = null) => func = _ => msg;
    }
}