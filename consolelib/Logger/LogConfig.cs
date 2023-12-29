using System.Diagnostics.CodeAnalysis;

namespace CoolandonRS.consolelib.Logger;

public struct LogConfig {
    /// <summary>
    /// {0} is timestamp, {1} title, {2} intensity, and {3} message.
    /// </summary>
    [StringSyntax("CompositeFormat")] 
    public string EntryFormat = "[{0}] {1} {2}: {3}";
    public HashSet<LogPurpose> SupportedPurpose = [ LogPurpose.Info, LogPurpose.Alert ];
    public HashSet<LogIntensity> SupportedIntensity = [LogIntensity.Info, LogIntensity.Warning, LogIntensity.Error, LogIntensity.Fatal];
    
    public LogConfig(string? format = null, HashSet<LogPurpose>? purpose = null, HashSet<LogIntensity>? intensity = null) {
        EntryFormat = format ?? EntryFormat;
        SupportedPurpose = purpose ?? SupportedPurpose;
        SupportedIntensity = intensity ?? SupportedIntensity;
    }
}