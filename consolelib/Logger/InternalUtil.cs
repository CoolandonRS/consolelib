using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace CoolandonRS.consolelib.Logger;


/// <summary>
/// Random assortment of internal utilities 
/// </summary>
internal static class InternalUtil {
    
    public static string ToPrintableString<T>(T obj, CultureInfo culture, LogConfig? config = null) {
        return obj switch {
            string str => str,
            IPrintable print => print.ToPrintableString(config ?? new LogConfig(), culture),
            IFormattable format => format.ToString(null, culture),
            IConvertible convert => convert.ToString(culture),
            null => "\0",
            _ => obj.ToString() ?? typeof(T).FullName ?? typeof(T).Name
        };
    }

    public static void WritePrintable<T>(this TextWriter w, T obj, LogConfig config, CultureInfo culture) => w.Write(ToPrintableString(obj, culture, config));
    public static void WritePrintableLine<T>(this TextWriter w, T obj, LogConfig config, CultureInfo culture) => w.WriteLine(ToPrintableString(obj, culture, config));
    public static Task WritePrintableAsync<T>(this TextWriter w, T obj, LogConfig config, CultureInfo culture) => w.WriteAsync(ToPrintableString(obj, culture, config));
    public static Task WritePrintableLineAsync<T>(this TextWriter w, T obj, LogConfig config, CultureInfo culture) => w.WriteLineAsync(ToPrintableString(obj, culture, config));
}