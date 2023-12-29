namespace CoolandonRS.consolelib.Arg.Contracts;

using IAC = IArgContract;
using LAC = LambdaArgContract;

/// <summary>
/// A system to validate the way a user passed in arguments.
/// <seealso cref="Always"/>
/// <seealso cref="Never"/>
/// <seealso cref="And"/>
/// <seealso cref="Or"/>
/// <seealso cref="None"/>
/// <seealso cref="Not"/>
/// <seealso cref="Present"/>
/// <seealso cref="NotPresent"/>
/// <seealso cref="Is{T}"/>
/// <seealso cref="IsNot{T}"/>
/// <seealso cref="Equal{T}"/>
/// <seealso cref="NotEqual{T}"/>
/// <seealso cref="OneOf{T}"/>
/// <seealso cref="NotOneOf{T}"/>\
/// <seealso cref="If"/>
/// <seealso cref="Relations"/>
/// <seealso cref="Lambda"/>
/// </summary>
public static class ArgContracts {
    /// <summary> Returns true. </summary>
    public static IAC Always() => new LAC(_ => true);

    /// <summary> Returns false. </summary>
    public static IAC Never() => new LAC(_ => false);

    /// <summary> Returns true if all contained contracts are true. </summary>
    public static IAC And(params IAC[] contracts) => new LAC(ah => contracts.All(c => c.Eval(ah)));

    /// <summary> Returns true if at least one of the contained contracts are true. </summary>
    public static IAC Or(params IAC[] contracts) => new LAC(ah => contracts.Any(c => c.Eval(ah)));

    /// <summary> Returns true if none of the contained contracts are true. </summary>
    public static IAC None(params IAC[] contracts) => new LAC(ah => !contracts.Any(c => c.Eval(ah)));

    /// <summary> Negates an ArgContract. </summary>
    public static IAC Not(IAC contract) => new LAC(ah => !contract.Eval(ah));

    /// <summary> Checks if an argument is set. </summary>
    public static IAC Present(string name) => new LAC(ah => !ah.IsDefault(name));

    /// <summary> Checks if an argument is not set. </summary>
    public static IAC NotPresent(string name) => new LAC(ah => ah.IsDefault(name));

    /// <summary> Checks if an argument is a specific value. Throws if the cast fails. </summary>
    public static IAC Is<T>(string name, T value) where T : IEquatable<T> => new LAC(ah => ah.Get<T>(name).Equals(value));

    /// <summary> Checks if an argument is not a value. Throws if the cast fails. </summary>
    public static IAC IsNot<T>(string name, T value) where T : IEquatable<T> => new LAC(ah => !ah.Get<T>(name).Equals(value));

    /// <summary> Checks if two arguments are equal given their type. Throws if either cast fails. </summary>
    public static IAC Equal<T>(string name1, string name2) where T : IEquatable<T> => new LAC(ah => ah.Get<T>(name1).Equals(ah.Get<T>(name2)));

    /// <summary> Checks if two arguments are not equal given their type. Throws if either cast fails. </summary>
    public static IAC NotEqual<T>(string name1, string name2) where T : IEquatable<T> => new LAC(ah => !ah.Get<T>(name1).Equals(ah.Get<T>(name2)));

    /// <summary> Returns true if the variable is one of vals. Throws on cast failure. </summary>
    public static IAC OneOf<T>(string name, params T[] vals) where T : IEquatable<T> => new LAC(ah => vals.Contains(ah.Get<T>(name)));

    /// <summary> Returns true if the variable is not one of vals. Throws on cast failure. </summary>
    public static IAC NotOneOf<T>(string name, params T[] vals) where T : IEquatable<T> => new LAC(ah => !vals.Contains(ah.Get<T>(name)));

    /// <summary> Transforms the given lambda into an ArgContract </summary>
    public static IAC Lambda(Func<ArgHandler, bool> func) => new LAC(func); // Do this instead of publicizing LAC for consistency and since I like it more.

    /// <summary> Returns the result of @true if cond is true, and @false if cond is false. @false is by default ArgContracts.Never() </summary>
    public static IAC If(IAC cond, IAC @true, IAC? @false = null) { @false ??= Never(); return new LAC(ah => cond.Eval(ah) ? @true.Eval(ah) : @false.Eval(ah)); }

    /// <summary>
    /// Easily set up relationships for a given argument.
    /// </summary>
    /// <param name="arg">Name of the argument</param>
    /// <param name="depends">A two dimensional array indicating dependencies, where the first dimension is multiple requirements, and the second dimension is multiple satisfactions </param>
    /// <param name="conflicts">An array indicating conflicts</param>
    /// <example>
    /// A depends of [["a"], ["b"]] requires both "a" and "b" to be present. <br/>
    /// A depends of [["a", "b"]] requires either "a" or "b" to be present.
    /// </example>
    public static IAC Relations(string arg, string[][] depends, string[] conflicts) => new LAC(ah => ah.IsDefault(arg) || (!depends.Any(arr => arr.All(ah.IsDefault)) && conflicts.All(ah.IsDefault)));
}