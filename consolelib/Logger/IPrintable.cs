using System.Globalization;

namespace CoolandonRS.consolelib.Logger;

public interface IPrintable {
    public string ToPrintableString(LogConfig config, CultureInfo culture);
}