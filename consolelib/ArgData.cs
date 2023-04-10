namespace CoolandonRS.consolelib; 

public class ArgData {
    public readonly ArgDesc Desc;
    public ArgValue Value { get; private set; }
    private bool valueChanged = false;

    public void Set(string newValue) {
        Set(new ArgValue(newValue));
    }

    public void Set(ArgValue newValue) {
        if (valueChanged) throw new InvalidOperationException("Already Set");
        this.Value = newValue;
        this.valueChanged = true;
    }
    
    public ArgData(ArgDesc desc, ArgValue? defaultValue = null) {
        this.Desc = desc;
        this.Value = defaultValue ?? ArgValue.OfNull();
    }

    public ArgData(ArgDesc desc, string defaultValue) : this(desc, new ArgValue(defaultValue)) { }
}