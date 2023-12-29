using System.ComponentModel;
using System.Globalization;
using System.Xml;

namespace CoolandonRS.consolelib.Logger;

public interface ILogger : IDisposable, IAsyncDisposable {
    public void Write<T>(T obj, CultureInfo? culture);
    public void WriteLine<T>(T obj, CultureInfo? culture);
    public Task WriteAsync<T>(T obj, CultureInfo? culture);
    public Task WriteLineAsync<T>(T obj, CultureInfo? culture);
}