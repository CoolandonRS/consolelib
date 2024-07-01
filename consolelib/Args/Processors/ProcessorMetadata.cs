namespace CoolandonRS.consolelib.Args.Processors;

public readonly record struct ProcessorMetadata(Type Type, Bundleability Bundleability, bool HasValue) {
    public static ProcessorMetadata New<T>(Bundleability bundleability, bool hasValue) => new(typeof(T), bundleability, hasValue);
}
