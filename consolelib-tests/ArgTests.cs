using System.Text.RegularExpressions;
using CoolandonRS.consolelib.Arg;
using CoolandonRS.consolelib.Arg.Builders;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(Arg<>)), TestOf(typeof(FlagArg)), TestOf(typeof(SingleFlagArg)), TestOf(typeof(SingleValueArg<>)), TestOf(typeof(ValueArg<>))]
public class ArgTests {
    private Arg<int> testArg;
    private const string name = "test";
    private const string desc = "testDesc";
    private readonly Regex prefixRgx = new("--abc");
    private readonly ArgValueSplit split = new([]);
    private const int val = 1;

    [SetUp]
    public void Setup() {
        testArg = new Arg<int>(name, desc, prefixRgx, split, val, int.Parse);
    }
    
    [Test, TestOf(typeof(Arg<>))]
    public void NoncomplexMethods() { 
        Assert.Multiple(() => {
            Assert.That(testArg.GetName(), Is.EqualTo(name), "Name mismatch");
            Assert.That(testArg.GetDesc(), Is.EqualTo(desc), "Desc mismatch");
            Assert.That(testArg.GetRegex(), Is.EqualTo(prefixRgx.ToString()), "Regex mismatch");
            Assert.That(testArg.GetSplit(), Is.EqualTo(split), "Split mismatch");
            Assert.That(testArg.Get(), Is.EqualTo(val), "Value mismatch");
            Assert.That(testArg.IsDefault(), Is.True, "Reported non default");
            Assert.That(testArg.Validate("--abc"), Is.True, "Validate truth failure");
            Assert.That(testArg.Validate("-def"), Is.False, "Validate false success");
            Assert.That(testArg.Type(), Is.EqualTo(typeof(int)));
            Assert.That(testArg.GetPrefixType(), Is.EqualTo(IArg.PrefixType.Double), "Nondouble prefix on noncomplex Arg<> call");
            Assert.That(testArg.GetCall(), Is.Null, "Non null Call on noncomplex Arg<> call");
        });
    }

    [Test, TestOf(typeof(Arg<>))]
    public void NoncomplexSet() {
        Assert.Multiple(() => {
            Assert.That(testArg.Set("george"), Is.EqualTo(IArg.SetStatus.FailedCast), "Non respective cast fail status");
            Assert.That(testArg.IsDefault(), Is.True, "Nondefault after cast failure");
            Assert.That(testArg.Set("1"), Is.EqualTo(IArg.SetStatus.Set), "Non respective set status");
            Assert.That(testArg.IsDefault(), Is.False, "Default after success");
            Assert.That(testArg.Set("1"), Is.EqualTo(IArg.SetStatus.AlreadySet), "Non respective already set status");
            Assert.That(testArg.IsDefault(), Is.False, "Default after repeated set");
        });
    }

    [Test, TestOf(typeof(ValueArg<>)), TestOf(typeof(SingleValueArg<>))]
    public void ValueArgs() {
        var value = new ValueArg<int>("monkeys", "Number of monkeys", 0, int.Parse);
        var singleValue = new SingleValueArg<string?>("name", "Name of primary monkey", 'n', null, s => s);
        Assert.Multiple(() => {
            Assert.That(value.GetPrefixType(), Is.EqualTo(IArg.PrefixType.Double), "Double reported incorrectlty");
            Assert.That(singleValue.GetPrefixType(), Is.EqualTo(IArg.PrefixType.Single), "Single reported incorrectly");
            // While technically we should test Set(string) here to make sure that the constructor initialized the arg correctly since it doesn't overload I don't care enough to.
        });
    }

    [Test, TestOf(typeof(FlagArg)), TestOf(typeof(SingleFlagArg))]
    public void FlagArgs() {
        var flag = new FlagArg("monkey", "are monkeys present?");
        var singleFlag = new SingleFlagArg("no cookies", "are no cookies present?", 'c', true);
        Assert.Multiple(() => {
            Assert.That(flag.GetPrefixType(), Is.EqualTo(IArg.PrefixType.Double), "Nonsingle reported incorrectly");
            Assert.That(singleFlag.GetPrefixType(), Is.EqualTo(IArg.PrefixType.Single), "Single reported incorrectly");
            Assert.That(flag.IsDefault, Is.True, "default false reported nondefault");
            Assert.That(singleFlag.IsDefault, Is.True, "default true reported nondefault");
            Assert.That(flag.Get(), Is.False, "Default false returned true");
            Assert.That(singleFlag.Get(), Is.True, "Default true returned false");
            Assert.That(flag.Set("sadhisodfuoiuosif"), Is.EqualTo(IArg.SetStatus.Set), "Non set status");
            Assert.That(flag.Set("zcxv78dyu"), Is.EqualTo(IArg.SetStatus.AlreadySet), "Non already set status");
            Assert.That(flag.Get(), Is.True, "Default false returned false after set");
            // Don't need to test singleFlag for AlreadySet, they use the same overload. We test it for set anyway, since it needs to be set and might as well be reconfirmed
            Assert.That(singleFlag.Set(""), Is.EqualTo(IArg.SetStatus.Set), "Non set status");
            Assert.That(singleFlag.Get(), Is.False, "Default true returned true after set");
            Assert.That(flag.IsDefault, Is.False, "Default false reported default");
            Assert.That(singleFlag.IsDefault, Is.False, "Default true reported default");
        });
    }
}