namespace CoolandonRS.consolelib; 

public class ArgParseException : InvalidOperationException {
    public ArgParseException(string name, Exception inner) : base($"Argument {name}: {inner.Message}", inner) {
        
    }
}