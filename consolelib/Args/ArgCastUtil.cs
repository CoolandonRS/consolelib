using System.Globalization;
using System.Net;
using System.Net.Sockets;

namespace CoolandonRS.consolelib.Args; 

/// <summary>
/// For most common types, type.Parse will work fine. <br/> Ex: int.Parse(); <br/> <br/>
/// A few less common cases that may or may not be useful are stored here.
/// </summary>
public static class ArgCastUtil {
    private static Dictionary<Type, Func<string, object>> lookup = new();
    private static Type[] strTypeArr = { typeof(string) };
    /// <summary>
    /// <b> NOT RECOMMENDED </b> <br/>
    /// Checks the given type for a Parse(string) method or a Type(string) constructor, caches it, and uses it on future calls. <br/> <br/>
    /// You should avoid using this. 
    /// </summary>
    /// <exception cref="MissingMethodException">If neither a matching method nor constructor were found.</exception>
    public static T TryFindParse<T>(string str) {
        var type = typeof(T);
        if (lookup.TryGetValue(type, out var value)) return (T)value.Invoke(str);
        // Search for Parse(string) method
        var parse = type.GetMethod("Parse", strTypeArr);
        if (parse != null) {
            lookup[type] = s => parse.Invoke(null, [s])!;
            return (T)lookup[type].Invoke(str);
        }
        // Search for Type(string) constructor
        var constructor = type.GetConstructor(strTypeArr);
        if (constructor != null) {
            lookup[type] = s => constructor.Invoke([s]);
            return (T)lookup[type].Invoke(str);
        }
        throw new MissingMethodException();
    }
    /// <summary>
    /// Reads an int as hexadecimal input
    /// </summary>
    public static int Hex(string str) => int.Parse(str.StartsWith("0x") ? str[2..] : str, NumberStyles.HexNumber);

    /// <summary>
    /// Casts to an IP or performs a DNS lookup on the provided string
    /// </summary>
    public static IPAddress HostnameOrIP(string str) {
        try {
            return Dns.GetHostAddresses(str)[0];
        } catch (Exception e) when (e is ArgumentOutOfRangeException or SocketException or ArgumentException) {
            throw new InvalidCastException(e.Message, e);
        }
    }
    /// <summary>
    /// Given a cast for a type, provides a cast to an array of that type.
    /// </summary>
    public static Func<string, T[]> GetArrayCast<T>(Func<string, T> cast, char[]? delim = null) {
        delim ??= new[] { ',' };
        return str => str.Split(delim).Select(cast).ToArray();
    }
}