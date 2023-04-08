namespace consolelib_tests; 

public class ArgHandlerTests {
    private ArgHandler handler;
    private static string helpVerif = "-f\n\tFlag\n-T\n\tTest\n-?\n\tShows help\n--val=[int]\n\tTest Value";
    
    [SetUp]
    public void SetUp() {
        handler = new ArgHandler(new Dictionary<string, ArgData>() {
            { "val", new ArgData(new ArgDesc("--val=[int]", "Test Value")) }
        }, new Dictionary<char, FlagData>() {
            { 'f', new FlagData(new ArgDesc("-f", "Flag")) },
            { 'T', new FlagData(new ArgDesc("-T", "Test"))}
        });
    }

    [Test]
    public void GetHelpString() {
        
        Assert.That(handler.GetHelpString(), Is.EqualTo(helpVerif), "Incorrect HelpString");
    }

    [Test]
    public void GetValue() {
        Assert.That(handler.GetValue("val").IsSet, Is.False, "GetValue Failure");
    }

    [Test]
    public void GetFlag() {
        Assert.That(handler.GetFlag('f'), Is.False, "GetFlag Failure");
    }

    [Test]
    public void ParseArgFlagFail() {
        handler.ParseArgs(new[] {"-a"});
        Assert.That(handler.HasErrors, Is.True, "Flag Error Unrecognized");
    }

    [Test]
    public void ParseArgValueFail() {
        handler.ParseArgs(new[] {"--abc=def"});
        Assert.That(handler.HasErrors, Is.True, "value Error Unrecognized");
    }

    [Test]
    public void ParseArgFlag() {
        handler.ParseArgs(new[] {"-f"});
        Assert.That(handler.GetFlag('f'), Is.True, "Flag Parse Failure");
    }

    [Test]
    public void ParseArgMultiFlag() {
        handler.ParseArgs(new[] {"-fT"});
        Assert.That(handler.GetFlag('f') && handler.GetFlag('T'), Is.True, "Flag Parse Failure");
    }

    [Test]
    public void ParseArgValue() {
        handler.ParseArgs(new[] {"--val=123"});
        Assert.That(handler.GetValue("val").AsInt(), Is.EqualTo(123), "Value Parse Failure");
    }

    [Test]
    public void ParseArgBoth() {
        handler.ParseArgs(new[] {"-T", "--val=456"});
        Assert.Multiple(() => {
            Assert.That(handler.GetFlag('T'), Is.True, "Flag Parse Failure");
            Assert.That(handler.GetValue("val").AsInt(), Is.EqualTo(456), "Value Parse Failure");
        });
    }
}