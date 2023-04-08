using System.Text.RegularExpressions;

namespace consolelib;

public static class ConsoleUtil {
    public static void WriteColored(string msg, ConsoleColor? foreground = null, ConsoleColor? background = null) {
        var returnForeground = Console.ForegroundColor;
        var returnBackground = Console.BackgroundColor;
        Console.ForegroundColor = foreground ?? returnForeground;
        Console.BackgroundColor = background ?? returnBackground;
        Console.Write(msg);
        Console.ForegroundColor = returnForeground;
        Console.BackgroundColor = returnBackground;
    }

    public static void WriteColoredLine(string msg, ConsoleColor? foreground = null, ConsoleColor? background = null) {
        WriteColored(msg, foreground, background);
        Console.WriteLine();
    }
        
    public static string RegexQuery(string prompt, string regex, bool newLine = false) {
        for (;;) {
            Console.Write(prompt + (newLine ? "\n" : ""));
            var input = Console.ReadLine()!;
            if (Regex.Match(input, regex).Success) return input;
        }
    }

    public static string StringQuery(string prompt, bool newLine = false) {
        return RegexQuery(prompt, ".*", newLine);
    }

    public static string ChoiceQuery(string prompt, string[] valid, bool newLine = false) {
        var regex = "(" + string.Join(")|(", valid.Select(Regex.Escape).ToArray()) + ")";
        return RegexQuery(prompt, regex, newLine);
    }

    private static readonly string[] agreements = { "y", "yes" };
    private static readonly string[] disagreements = { "n", "no" };
    public static bool BoolQuery(string prompt, bool assume = false, bool newLine = false) {
        var answer = StringQuery(prompt, newLine);
        return (assume ? disagreements : agreements).Contains(answer.ToLower().Trim());
    }

    public static T EnumQuery<T>(string prompt, bool newLine = false) {
        return (T) Enum.Parse(typeof(T), ChoiceQuery(prompt, Enum.GetNames(typeof(T)), newLine));
    }

    public static int IntQuery(string prompt, bool newLine = false) {
        return int.Parse(RegexQuery(prompt, "\\d+", newLine));
    }

    public static int RangedIntQuery(string prompt, int min, int max, bool newLine = false) {
        for (;;) {
            var input = IntQuery(prompt, newLine);
            if (input >= min && input <= max) return input;
        }
    }

    public static int ChoiceIntQuery(string prompt, int[] valid, bool newLine = false) {
        for (;;) {
            var input = IntQuery(prompt, newLine);
            if (valid.Contains(input)) return input;
        }
    }
}