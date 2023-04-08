namespace consolelib_tests; 

public class FlagDataTests {
    private static ArgDesc desc = new ArgDesc("", "");
    
    [Test]
    public void FlagData() {
        Assert.Multiple(() => {
            var defFalse = new FlagData(desc);
            var defTrue = new FlagData(desc, true);
            Assert.That(defFalse.IsSet, Is.False, "Default False Failure");
            Assert.That(defTrue.IsSet, Is.True, "Default True Failure");
            defFalse.Set();
            defTrue.Set();
            Assert.That(defTrue.IsSet, Is.False, "Invert True Failure");
            Assert.That(defFalse.IsSet, Is.True, "Invert False Failure");
            Assert.Throws(typeof(InvalidOperationException), () => {
                defTrue.Set();
            }, "Double Set Success");
        });
    }
}