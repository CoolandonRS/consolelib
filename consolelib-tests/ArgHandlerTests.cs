using CoolandonRS.consolelib.Args;
using NUnit.Framework.Interfaces;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(ArgHandler))]
public class ArgHandlerTests {
    [Test]
    public void ParseSingleFlag() {
        var argHandler = new ArgHandler(new SingleFlagArg("verbose", "print verbose", 'v'), new SingleFlagArg("!verbose", "don't print verbose", 'V', true));
        argHandler.Parse(["-v"]);
        Assert.Multiple(() => {
            Assert.That(argHandler.IsDefault("verbose"), Is.False, "Default reported when present");
            Assert.That(argHandler.IsDefault("!verbose"), Is.True, "Nondefault reported when not present");
            Assert.That(argHandler.Get<bool>("verbose"), Is.True, "Get passthrough failure");
            Assert.Throws<InvalidCastException>(() => argHandler.Get<HttpClient>("!verbose"), "Non throw on invalid cast");
        });
    }
}