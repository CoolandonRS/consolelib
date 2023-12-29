using System.Globalization;

namespace CoolandonRS.consolelib.Logger;

public class ConsoleLogger(LogConfig? config = null, CultureInfo? culture = null) : AbstractLogger(Console.Out, config, culture);