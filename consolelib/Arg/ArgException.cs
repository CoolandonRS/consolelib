namespace CoolandonRS.consolelib.Arg;

public class ArgException : ArgumentException;

internal class UnknownArgException : ArgException;
internal class CastFailureException : ArgException;
internal class DuplicateArgException : ArgException;
internal class NonTrailingSingleValueArgException : ArgException;
internal class InsufficientDataException : ArgException;