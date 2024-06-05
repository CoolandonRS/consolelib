using System.ComponentModel;
using System.Runtime.Versioning;

namespace CoolandonRS.consolelib.Arg.Contracts;

using IAC = IArgContract;
using LAC = LambdaArgContract;
using Result = IArgContract.Result;
using Status = IArgContract.Status;
using Message = IArgContract.Message;
using CondStr = IArgContract.ConditionalString;

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
/// <seealso cref="NotOneOf{T}"/>
/// <seealso cref="If"/>
/// <seealso cref="Relations"/>
/// <seealso cref="Lambda"/>
/// </summary>
public static class ArgContracts {
    /// <summary> Returns true. </summary>
    public static IAC Always(string? msg = null) => new LAC(_ => new Result(Status.Fulfilled, msg));

    /// <summary> Returns false. </summary>
    public static IAC Never(string? msg = null) => new LAC(_ => new Result(Status.Unfulfilled, msg));

    /// <inheritdoc cref="All(System.Nullable{CondStr},IAC[])"/>
    public static IAC All(params IAC[] contracts) => All(null, contracts);
    /// <summary> Returns true if all contained contracts are true. </summary>
    /// <remarks> Always terminates when the first failure is found. </remarks>
    public static IAC All(CondStr? msg, params IAC[] contracts) => new LAC(ah => {
        List<Message> msgs = [];
        return new Result(contracts.All(c => {
            var res = c.Eval(ah);
            msgs.Add(res.Msg);
            return res.WasSuccess();
        }), msg, msgs.ToArray());
    });

    /// <inheritdoc cref="Any(System.Nullable{CondStr},IAC[])"/>
    public static IAC Any(params IAC[] contracts) => Any(null, contracts);
    /// <summary> Returns true if at least one of the contained contracts are true. </summary>
    /// <remarks> Always terminates when the first success is found. </remarks>
    public static IAC Any(CondStr? msg, params IAC[] contracts) => new LAC(ah => {
        List<Message> msgs = [];
        return new Result(contracts.Any(c => {
            var res = c.Eval(ah);
            msgs.Add(res.Msg);
            return res.Status == Status.Fulfilled;
        }), msg, msgs.ToArray());
    });

    /// <inheritdoc cref="None(CondStr?,IAC[])"/>
    public static IAC None(params IAC[] contracts) => None(null, contracts);
    /// <summary> Returns true if none of the contained contracts are true. </summary>
    /// <remarks> Always terminates when the first success is found. </remarks>
    public static IAC None(CondStr? msg, params IAC[] contracts) => Not(Any(msg, contracts), null);

    /// <summary> Negates an ArgContract. </summary>
    public static IAC Not(IAC contract, CondStr? msg = null) => new LAC(ah => contract.Eval(ah).Invert(msg));

    /// <summary> Checks if an argument is not set. </summary>
    public static IAC NotPresent(string name, CondStr? msg = null) => new LAC(ah => new Result(ah.IsDefault(name), msg));

    /// <summary> Checks if an argument is set. </summary>
    public static IAC Present(string name, CondStr? msg = null) => new LAC(ah => new Result(!ah.IsDefault(name), msg));

    /// <summary> Checks if an argument is a specific value. Throws if the cast fails. </summary>
    public static IAC Is<T>(string name, T value, CondStr? msg = null) where T : IEquatable<T> => new LAC(ah => new Result(ah.Get<T>(name).Equals(value), msg));

    /// <summary> Checks if an argument is not a value. Throws if the cast fails. </summary>
    public static IAC IsNot<T>(string name, T value, CondStr? msg = null) where T : IEquatable<T> => new LAC(ah => new Result(!ah.Get<T>(name).Equals(value), msg));

    /// <summary> Checks if two arguments are equal given their type. Throws if either cast fails. </summary>
    public static IAC Equal<T>(string name1, string name2, CondStr? msg = null) where T : IEquatable<T> => new LAC(ah => new Result(ah.Get<T>(name1).Equals(ah.Get<T>(name2)), msg));

    /// <summary> Checks if two arguments are not equal given their type. Throws if either cast fails. </summary>
    public static IAC NotEqual<T>(string name1, string name2, CondStr? msg = null) where T : IEquatable<T> => new LAC(ah => new Result(!ah.Get<T>(name1).Equals(ah.Get<T>(name2)), msg));

    /// <inheritdoc cref="OneOf{T}(string,UsrMsg?,T[])"/>
    public static IAC OneOf<T>(string name, params T[] vals) where T : IEquatable<T> => OneOf(name, null, vals);
    /// <summary> Returns true if the variable is one of vals. Throws on cast failure. </summary>
    public static IAC OneOf<T>(string name, CondStr? msg, params T[] vals) where T : IEquatable<T> => new LAC(ah => new Result(vals.Contains(ah.Get<T>(name)), msg));

    /// <inheritdoc cref="NotOneOf{T}(string,UsrMsg?,T[])"/>
    public static IAC NotOneOf<T>(string name, params T[] vals) where T : IEquatable<T> => NotOneOf(name, null, vals);
    /// <summary> Returns true if the variable is not one of vals. Throws on cast failure. </summary>
    public static IAC NotOneOf<T>(string name, CondStr? msg, params T[] vals) where T : IEquatable<T> => new LAC(ah => new Result(!vals.Contains(ah.Get<T>(name)), msg));

    /// <summary> Transforms the given lambda into an ArgContract </summary>
    public static IAC Lambda(Func<ArgHandler, Result> func) => new LAC(func); // Do this instead of publicizing LAC for consistency and since I like it more.

    /// <summary> Returns the result of @true if cond is true, and @false if cond is false. @false is by default ArgContracts.Never() </summary>
    public static IAC If(IAC cond, IAC @true, IAC? @false = null, CondStr? msg = null) {
        return new LAC(ah => {
            var res = cond.Eval(ah);
            return res.Status switch {
                Status.Ignored => new Result(Status.Ignored, msg, res.Msg),
                Status.Fulfilled => IfResult(res.Msg, @true.Eval(ah)),
                Status.Unfulfilled => IfResult(res.Msg, (@false ?? Never()).Eval(ah)),
                _ => throw new InvalidEnumArgumentException()
            };
        });
        Result IfResult(Message condMsg, Result result) => new(result.Status, msg, [condMsg, result.Msg]);
    }

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
    /// <remarks>Terminates early on failure</remarks>
    public static IAC Relations(string arg, string[][] depends, string[] conflicts, CondStr? msg = null) => new LAC(ah => {
        if (ah.IsDefault(arg)) return new Result(Status.Ignored, msg);
        foreach (var depend in depends) {
            if (depend.All(ah.IsDefault)) return new Result(Status.Unfulfilled, msg, $"None found: {string.Join(", ", depend)}");
        }
        foreach (var conflict in conflicts) {
            if (!ah.IsDefault(conflict)) return new Result(Status.Unfulfilled, msg, $"Found: {conflict}");
        }
        return new Result(Status.Fulfilled, msg);
    });
}