namespace CoolandonRS.consolelib.Arg.Contracts;

public partial interface IArgContract {
    public readonly struct Message(string? main = null, Message[]? children = null) {
        public readonly string? Main = main;
        public readonly Message[]? Children = children;

        public bool IsNull() {
            var token = false;
            return IsNull(ref token);
        }
        
        private bool IsNull(ref bool token) {
            if (token) return false;
            var isNull = Main is null && (Children is null || Recurse(Children, ref token));
            if (!isNull) token = true;
            return isNull;
            // This function exists so it can early return between every child, and since ref vars cannot be passed to a lambda.
            // This also seems to trick rider into thinking this function isn't recursive, interestingly enough.
            bool Recurse(IEnumerable<Message> myChildren, ref bool token) {
                foreach (var child in myChildren) {
                    if (token || !child.IsNull(ref token)) return false;
                }
                return true;
            }
        }
        
        public Message(string? main, Message child) : this(main, (Message[])[child]) { }
        public Message(string? main, string? child) : this(main, new Message(child)) { }
        public Message(string? main, List<Message> children) : this(main, children.ToArray()) { }
    }
}