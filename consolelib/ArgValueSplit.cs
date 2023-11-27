namespace CoolandonRS.consolelib; 

public class ArgValueSplit {
    internal enum Status {
        Failure, Success, Advance
    }

    private char[] delimiters;
    private bool spaceDelimited;

    internal (Status status, string prefix, string? postfix) Parse(string val) {
        var split = val.Split(delimiters);
        if (split.Length < 2) return (spaceDelimited ? Status.Advance : Status.Failure, val, null);
        return (Status.Success, split[0], string.Join("", split[1..]));
    }
    
    public ArgValueSplit(char[] delimiters) {
        if (delimiters.Contains(' ')) {
            this.delimiters = delimiters.Where(c => c != ' ').ToArray();
            this.spaceDelimited = true;
        } else {
            this.delimiters = delimiters;
            this.spaceDelimited = false;
        }
    }
}