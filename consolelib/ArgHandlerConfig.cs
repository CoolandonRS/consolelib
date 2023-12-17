namespace CoolandonRS.consolelib;

public struct ArgHandlerConfig {
    public readonly bool DoErrors;
    public readonly bool UnknownToImplicit;
    public readonly string UnknownArgMessage;
    public readonly string CastFailureMessage;
    public readonly string DuplicateMessage;
    public readonly string NonTrailingSingleValueMessage;

    public ArgHandlerConfig(bool doErrors = true, bool unknownToImplicit = false, string unknownArgMessage = "Unknown Argument {0}", string castFailureMessage = "Invalid Argument {0}, expected {1}", string duplicateMessage = "Duplicate argument {0}", string nonTrailingSingleValueMessage = "Single arguments with values must be the last of a chain.") {
        DoErrors = doErrors;
        UnknownToImplicit = unknownToImplicit;
        UnknownArgMessage = unknownArgMessage;
        CastFailureMessage = castFailureMessage;
        DuplicateMessage = duplicateMessage;
        NonTrailingSingleValueMessage = nonTrailingSingleValueMessage;
    }
}