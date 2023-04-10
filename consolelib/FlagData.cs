namespace CoolandonRS.consolelib; 

public class FlagData {
    public readonly ArgDesc Desc;
    public bool IsSet { get; private set; }
    private bool wasSet = false;

    public void Set() {
        if (wasSet) throw new InvalidOperationException("Already Set");
        IsSet = !IsSet;
        this.wasSet = true;
    }

    public FlagData(ArgDesc desc, bool defaultVal = false) {
        this.Desc = desc;
        this.IsSet = defaultVal;
    }
}