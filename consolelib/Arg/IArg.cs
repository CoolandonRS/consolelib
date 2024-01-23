namespace CoolandonRS.consolelib.Arg; 

public interface IArg {
    public enum SetStatus {
        Set, FailedCast, AlreadySet, Insufficient
    }

    SetStatus Set(string val);

    object Get();

    bool IsDefault();

    Type Type();

    bool IsSingle();

    bool Validate(string str);

    ArgValueSplit? GetSplit();

    string GetName();

    string GetRegex();

    string GetDesc();

    string? GetCall();
}