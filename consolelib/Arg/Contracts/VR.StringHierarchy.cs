using System.Runtime.Versioning;

namespace CoolandonRS.consolelib.Arg.Contracts;

/// <summary>
/// Not guaranteed to work since writing unit tests for string manipulation sounds like a pain.
/// </summary>
public partial struct ValidationResult {
    public readonly struct StringHierarchy(string? val = null, StringHierarchy[]? children = null) {
        public readonly string? Val = val;
        public readonly StringHierarchy[]? Children = children;

        public static StringHierarchy? From(IArgContract.Message message) {
            if (message.Children is null) return message.Main is null ? new StringHierarchy(message.Main) : null;
            // use OfType as a strange sort of null filtering.
            return new StringHierarchy(message.Main, message.Children.Select(From).OfType<StringHierarchy>().ToArray());
        }

        /// <seealso cref="ToString(string,string,string,string,string)"/>
        public override string ToString() => ToString("-", "", "  ", "\n", "\n");

        /// <param name="blankRepresentation">What to show when Val is null</param>
        /// <param name="prefix">A prefix that appears before each child</param>
        /// <param name="depthStr">A string that gets added to prefix for each recursion</param>
        /// <param name="parentDelim">The delimiter between a parent and its children</param>
        /// <param name="childDelim">The delimiter between children of the same parent</param>
        /// <example><code>ToString("-", "", "  ", "\n", "\n")</code></example>
        /// <seealso cref="ToString()"/>
        public string ToString(string blankRepresentation, string prefix, string depthStr, string parentDelim, string childDelim) {
            if (Children is null) return Val!;
            return (Val ?? blankRepresentation) + parentDelim + string.Join(childDelim, Children.Select(c => prefix + c.ToString(blankRepresentation, prefix + depthStr, depthStr, parentDelim, childDelim)));
        }
    }
}