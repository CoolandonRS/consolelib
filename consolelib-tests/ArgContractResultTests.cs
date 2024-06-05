using CoolandonRS.consolelib.Arg.Contracts;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(IArgContract.Result))]
public class ArgContractResultTests {
    [Test]
    public void BoolCoercion() {
        var fulfilled = new IArgContract.Result(IArgContract.Status.Fulfilled);
        var ignored = new IArgContract.Result(IArgContract.Status.Ignored);
        var unfulfilled = new IArgContract.Result(IArgContract.Status.Unfulfilled);
        Assert.Multiple(() => {
            Assert.That(fulfilled.WasSuccess(true), Is.True, "Fulfilled -> false when strict");
            Assert.That(fulfilled.WasSuccess(false), Is.True, "Fulfilled -> false when not strict");
            Assert.That(ignored.WasSuccess(true), Is.False, "Ignored -> true when strict");
            Assert.That(ignored.WasSuccess(false), Is.True, "Ignored -> false when not strict");
            Assert.That(unfulfilled.WasSuccess(true), Is.False, "Unfulfilled -> true when strict");
            Assert.That(unfulfilled.WasSuccess(false), Is.False, "Unfulfilled -> true when not strict");
        });
    }
}
