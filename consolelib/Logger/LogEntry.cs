using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;

namespace CoolandonRS.consolelib.Logger;

public struct LogEntry : IPrintable {
    public string Title;
    public string Message;
    public LogIntensity Intensity;
    public LogPurpose Purpose;
    public DateTimeOffset Timestamp;

    public string ToPrintableString(LogConfig config, CultureInfo culture) {
        if (!config.SupportedPurpose.Contains(Purpose) || !config.SupportedIntensity.Contains(Intensity)) return "";
        return string.Format(culture, config.EntryFormat, Timestamp, Title, Enum.GetName(Intensity)!.ToUpper(culture), Message);
    }

    public LogEntry(string? title, string message, LogIntensity intensity, LogPurpose? purpose = null, DateTimeOffset? timestamp = null) {
        Title = title ?? "";
        Message = message;
        Intensity = intensity;
        Purpose = purpose ?? (intensity is LogIntensity.Info or LogIntensity.Debug ? LogPurpose.Info : LogPurpose.Alert);
        Timestamp = timestamp ?? DateTimeOffset.UtcNow;
    }
}