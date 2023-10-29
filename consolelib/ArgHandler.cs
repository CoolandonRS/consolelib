namespace CoolandonRS.consolelib; 

public class ArgHandler {
    private Dictionary<string, Arg<object>> args;
    public readonly string[] implicitArgs;
    
    public void Parse(string[] givenArgs) {
        
    }

    public ArgHandler(params Arg<object>[] args) {
        this.args = args.ToDictionary(arg => arg.name, arg => arg);
    }
}