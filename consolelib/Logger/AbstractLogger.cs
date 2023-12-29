using System.Globalization;

namespace CoolandonRS.consolelib.Logger;

public abstract class AbstractLogger : ILogger {
    protected TextWriter Out;
    protected CultureInfo Culture;
    protected LogConfig Config;

    public void Write<T>(T obj, CultureInfo? culture) => Out.WritePrintable(obj, Config, culture ?? Culture);
    public void WriteLine<T>(T obj, CultureInfo? culture) => Out.WritePrintableLine(obj, Config, culture ?? Culture);
    public Task WriteAsync<T>(T obj, CultureInfo? culture) => Out.WritePrintableAsync(obj, Config, culture ?? Culture);
    public Task WriteLineAsync<T>(T obj, CultureInfo? culture) => Out.WritePrintableLineAsync(obj, Config, culture ?? Culture);

    public AbstractLogger(TextWriter @out, LogConfig? config = null, CultureInfo? culture = null) {
        Out = @out;
        Config = config ?? new LogConfig();
        Culture = culture ?? CultureInfo.CurrentCulture;
    }
    
    public void Dispose() {
        GC.SuppressFinalize(this);
        Out.Dispose();
    }
    
    public ValueTask DisposeAsync() {
        GC.SuppressFinalize(this);
        return Out.DisposeAsync();
    }
}