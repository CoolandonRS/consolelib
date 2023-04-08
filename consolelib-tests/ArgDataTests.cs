namespace consolelib_tests; 

public class ArgDataTests {
    private static ArgDesc desc = new ArgDesc("", "");

    [Test]
    public void NullUnset() {
        Assert.That(GetData().Value.IsSet, Is.False, "Unset Default Failure");
    }

    [Test]
    public void DefaultUnset() {
        Assert.That(new ArgData(desc, "abc").Value.AsString(), Is.EqualTo("abc"), "Default Failure");
    }

    [Test]
    public void SetArgData() {
        var data = GetData();
        data.Set(new ArgValue("def"));
        Assert.That(data.Value.AsString(), Is.EqualTo("def"));
    }

    [Test]
    public void SetString() {
        var data = GetData();
        data.Set("ghi");
        Assert.That(data.Value.AsString(), Is.EqualTo("ghi"));
    }

    [Test]
    public void DoubleSet() {
        Assert.Throws(typeof(InvalidOperationException), () => {
            var data = GetData();
            data.Set(new ArgValue("jkl"));
            data.Set(new ArgValue("mno"));
        }, "Double Set Success");
    }

    private ArgData GetData() {
        return new ArgData(desc);
    }
}