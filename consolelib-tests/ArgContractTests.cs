using CoolandonRS.consolelib.Arg;
using CoolandonRS.consolelib.Arg.Builders;
using CoolandonRS.consolelib.Arg.Contracts;
using NUnit.Framework.Interfaces;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(ArgContracts))]
public class ArgContractTests {
    private ArgHandler handler;

    [SetUp]
    public void InitHandler() => handler = new ArgHandler(
        new ValueArg<string>("arg1", "", "def1", str => str),
        new ValueArg<string>("arg2", "", "def2", str => str),
        new ValueArg<string>("arg3", "", "def3", str => str),
        new ValueArg<string>("arg4", "", "def4", str => str)
    );
    
    [Test]
    public void ContractConstants() {
        Assert.Multiple(() => {
            Assert.That(IsSuccess(ArgContracts.Always()), Is.True, "Always returned failure");
            Assert.That(IsSuccess(ArgContracts.Never()), Is.False, "Never return success");
        });
    }

    [Test]
    public void GroupContracts() {
        Assert.Multiple(() => {
            // None
            Assert.That(IsSuccess(ArgContracts.None(ArgContracts.Always())), Is.False, "None returned success with succeeding child");
            Assert.That(IsSuccess(ArgContracts.None(ArgContracts.Never())), Is.True, "None returned failure with no succeeding child");
            // All
            Assert.That(IsSuccess(ArgContracts.All(ArgContracts.Always())), Is.True, "All returned failure with no failing child");
            Assert.That(IsSuccess(ArgContracts.All(ArgContracts.Never())), Is.False, "All returned success with failing child");
            // Any
            Assert.That(IsSuccess(ArgContracts.Any(ArgContracts.Always())), Is.True, "Any returned failure with succeeding child");
            Assert.That(IsSuccess(ArgContracts.Any(ArgContracts.Never())), Is.False, "Any returned success with no succeeding child");
        });
    }

    [Test]
    public void PresenceContracts() {
        Assert.Multiple(() => {
            Assert.That(IsSuccess(ArgContracts.Present("arg1")), Is.False, "Present returned success when not present");
            Assert.That(IsSuccess(ArgContracts.NotPresent("arg1")), Is.True, "NotPresent returned failure when not present");
            handler.Parse(["--arg1=something"]);
            Assert.That(IsSuccess(ArgContracts.Present("arg1")), Is.True, "Present returned failure when present");
            Assert.That(IsSuccess(ArgContracts.NotPresent("arg1")), Is.False, "NotPresent returned success when present");
        });
    }

    [Test]
    public void ConstantEqualityContracts() {
        handler.Parse(["--arg1=something"]);
        Assert.Multiple(() => {
            // Is
            Assert.That(IsSuccess(ArgContracts.Is("arg1", "something")), Is.True, "Is returned failure when values are equal");
            Assert.That(IsSuccess(ArgContracts.Is("arg1", "somethingElse")), Is.False, "Is returned success when values aren't equal");
            Assert.That(IsSuccess(ArgContracts.IsNot("arg1", "something")), Is.False, "IsNot returned success when values are equal");
            Assert.That(IsSuccess(ArgContracts.IsNot("arg1", "somethingElse")), Is.True, "IsNot returned failure when values aren't equal");
            // OneOf
            Assert.That(IsSuccess(ArgContracts.OneOf("arg1", "something")), Is.True, "OneOf returned failure when set contains value");
            Assert.That(IsSuccess(ArgContracts.OneOf("arg1", "somethingElse")), Is.False, "OneOf returned success when set doesn't contain value");
            Assert.That(IsSuccess(ArgContracts.NotOneOf("arg1", "something")), Is.False, "NotOneOf returned success when set contains value");
            Assert.That(IsSuccess(ArgContracts.NotOneOf("arg1", "somethingElse")), Is.True, "NotOneOf returned failure when set doesn't contain value");
        });
    }

    [Test]
    public void DynamicEqualityContracts() {
        handler.Parse(["--arg1=something","--arg2=something","--arg3=somethingElse"]);
        Assert.Multiple(() => {
            Assert.That(IsSuccess(ArgContracts.Equal<string>("arg1", "arg2")), Is.True, "Equal returned failure when values are equal");
            Assert.That(IsSuccess(ArgContracts.Equal<string>("arg1", "arg3")), Is.False, "Equal returned success when values aren't equal");
            Assert.That(IsSuccess(ArgContracts.NotEqual<string>("arg1", "arg2")), Is.False, "NotEqual returned success when values are equal");
            Assert.That(IsSuccess(ArgContracts.NotEqual<string>("arg1", "arg3")), Is.True, "NotEqual returned failure when values aren't equal");
        });
    }

    [Test]
    public void IfContract() {
        Assert.Multiple(() => {
            // True condition
            Assert.That(IsSuccess(ArgContracts.If(ArgContracts.Always(), ArgContracts.Always(), ArgContracts.Never())), Is.True, "True condition didn't return True value (True)");
            Assert.That(IsSuccess(ArgContracts.If(ArgContracts.Always(), ArgContracts.Never(), ArgContracts.Always())), Is.False, "True condition didn't return True value (False)");
            // False condition
            Assert.That(IsSuccess(ArgContracts.If(ArgContracts.Never(), ArgContracts.Never(), ArgContracts.Always())), Is.True, "False condition didn't return False value (True)");
            Assert.That(IsSuccess(ArgContracts.If(ArgContracts.Never(), ArgContracts.Always(), ArgContracts.Never())), Is.False, "False condition didn't return False value (False)");
            // Default False value is Never
            Assert.That(IsSuccess(ArgContracts.If(ArgContracts.Never(), ArgContracts.Always())), Is.False, "False condition returned True when no False value was specified");
        });
    }

    [Test]
    public void RelationsContract() {
        handler.Parse(["--arg1=a", "--arg2=b", "--arg3=c"]);
        Assert.Multiple(() => {
            // Ignored when not present
            Assert.That(ArgContracts.Relations("arg4", [], []).Eval(handler).Status, Is.EqualTo(IArgContract.Status.Ignored), "Relations not ignored when parent isn't present"); 
            // Succeed with no depends/conflicts
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [], [])), Is.True, "Relations failed with empty arguments");
            // Succeeding Dependencies
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg2"]], [])), Is.True, "Single-Single dependency failed when dependency was present");
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg2", "arg4"]], [])), Is.True, "Single-Multi dependency failed when at least one option present");
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg2", "arg3"]], [])), Is.True, "Single-Multi dependency failed when all options present");
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg2"], ["arg3"]], [])), Is.True, "Multi-Single dependency failed when all dependencies present");
            // Failing Dependencies
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg4"]], [])), Is.False, "Single-Single dependency succeeded when dependency wasn't present");
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [["arg2"], ["arg4"]], [])), Is.False, "Multi-Single dependency succeeded when all dependencies weren't present");
            // Conflicts
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [], ["arg4"])), Is.True, "Conflict failed when not present");
            Assert.That(IsSuccess(ArgContracts.Relations("arg1", [], ["arg2"])), Is.False, "Conflict succeeded when present");
        });
    }

    private bool IsSuccess(IArgContract contract) => contract.Eval(handler).WasSuccess();
}
