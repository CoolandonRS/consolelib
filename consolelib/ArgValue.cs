namespace consolelib; 

public class ArgValue {
    private string? val;

    public static ArgValue OfNull() {
        return new ArgValue();
    }

    public bool IsSet() => val != null;

    private void AssertSet() {
        if (!IsSet()) throw new ArgumentNullException();
    }

    public string AsString() {
        AssertSet();
        return val!;
    }

    public int AsInt() {
        AssertSet();
        return int.Parse(val!);
    }

    public (string ip, int port) AsAddress() {
        AssertSet();
        var split = val!.Split(':');
        if (split.Length != 2) throw new FormatException();
        return (split[0], int.Parse(split[1]));
    }

    private ArgValue() {
        this.val = null;
    }
    public ArgValue(string val) {
        this.val = val;
    }
}