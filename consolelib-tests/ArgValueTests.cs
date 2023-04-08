namespace consolelib_tests; 

public class ArgValueTests {
    [Test]
    public void OfNull() {
        Assert.That(ArgValue.OfNull().IsSet, Is.False, "IsSet Success");
    }

    [Test]
    public void OfVal() {
        var val = new ArgValue("abc");
        Assert.Multiple(() => {
            Assert.That(val.IsSet, Is.True, "IsSet Failure");
            Assert.That(val.AsString(), Is.EqualTo("abc"), "AsString Failure");
        });
    }

    [Test]
    public void AsInt() {
        Assert.Multiple(() => {
            Assert.Throws(typeof(FormatException), () => {
                new ArgValue("abc").AsInt();
            }, "Invalid Parse Success");
            Assert.That(new ArgValue("1").AsInt(), Is.EqualTo(1), "AsInt Failure");
        });
    }

    [Test]
    public void AsAddress() {
        Assert.Multiple(() => {
            Assert.Throws(typeof(FormatException), () => { 
                new ArgValue("abc").AsAddress();
            }, "Invalid Address Success");
            Assert.Throws(typeof(FormatException), () => { 
                new ArgValue("abc:def").AsAddress();
            }, "Invalid Port Success");
            var addr = new ArgValue("abc:123").AsAddress();
            Assert.That(addr.ip, Is.EqualTo("abc"));
            Assert.That(addr.port, Is.EqualTo(123));
        });
    }

    [Test]
    public void AssertSet() {
        Assert.Multiple(() => {
            Assert.Throws(typeof(ArgumentNullException), () => {
                ArgValue.OfNull().AsString();
            }, "AssertSet Unset Success");
            Assert.DoesNotThrow(() => {
                new ArgValue("abc").AsString();
            }, "AssertSet Set Failure");
        });
    }
}