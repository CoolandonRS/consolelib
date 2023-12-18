namespace CoolandonRS.consolelib;

public struct ArgHandlerConfig {
    public enum ArgErrorStyle {
        Print, Throw
    }
    
    public readonly bool DoErrors;
    public readonly bool UnknownToImplicit;
    public readonly string UnknownArgMessage;
    public readonly string CastFailureMessage;
    public readonly string DuplicateMessage;
    public readonly string InsufficientMessage;
    public readonly string NonTrailingSingleValueMessage;
    public readonly ArgErrorStyle ErrorStyle;
    
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