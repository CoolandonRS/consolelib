namespace CoolandonRS.consolelib.Arg;

public struct ArgHandlerConfig {
    public enum ArgErrorStyle {
        Print, Throw
    }
    
    // ReSharper disable FieldCanBeMadeReadOnly.Global // To allow for new() { DoErrors = true } and similar. Since structs are passed by copy this should be fine.
    public bool DoErrors;
    public bool UnknownToImplicit;
    public string UnknownArgMessage;
    public string CastFailureMessage;
    public string DuplicateMessage;
    public string InsufficientMessage;
    public string NonTrailingSingleValueMessage;
    public ArgErrorStyle ErrorStyle;
    // ReSharper restore FieldCanBeMadeReadOnly.Global

    public ArgHandlerConfig(bool doErrors = true, bool unknownToImplicit = false, string unknownArgMessage = "Unknown Argument {0}", string castFailureMessage = "Invalid Argument {0}, expected {1}", string duplicateMessage = "Duplicate argument {0}", string insufficientMessage = "No value provided for {0}", string nonTrailingSingleValueMessage = "Single arguments with values must be the last of a chain.", ArgErrorStyle errorStyle = ArgErrorStyle.Print) {
        DoErrors = doErrors;
        UnknownToImplicit = unknownToImplicit;
        UnknownArgMessage = unknownArgMessage;
        CastFailureMessage = castFailureMessage;
        DuplicateMessage = duplicateMessage;
        InsufficientMessage = insufficientMessage;
        NonTrailingSingleValueMessage = nonTrailingSingleValueMessage;
        ErrorStyle = errorStyle;
    }
}