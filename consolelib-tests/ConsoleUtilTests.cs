namespace consolelib_tests;

[TestFixture, TestOf(typeof(ConsoleUtil))]
public class ConsoleUtilTests {
    // I have no clue how to cover WriteColored(Line)... So, I'm not.

    [Test]
    public void RegexQuery() {
        var str = "AbcDef\n26";
        Input(str);
        Assert.That(ConsoleUtil.RegexQuery("", "[0-9]+"), Is.EqualTo("26"), "Regex Fail");
    }

    [Test]
    public void StringQuery() {
        var str = "TestString";
        Input(str);
        Assert.That(ConsoleUtil.StringQuery(""), Is.EqualTo(str), "Passthrough Fail");
    }

    [Test]
    public void BoolQuery()
    {
        Input("y\nabc\nn\nabc");
        Assert.Multiple(() => {
            Assert.That(ConsoleUtil.BoolQuery(""), Is.True);
            Assert.That(ConsoleUtil.BoolQuery(""), Is.False);
            Assert.That(ConsoleUtil.BoolQuery("", true), Is.True);
            Assert.That(ConsoleUtil.BoolQuery("", true), Is.False);
        });
    }

    private enum TestEnum { ABC, HIJ }
    
    [Test]
    public void EnumQuery() {
        Input("ABC\nDEF\nHIJ");
        Assert.Multiple(() => {
            Assert.That(ConsoleUtil.EnumQuery<TestEnum>(""), Is.EqualTo(TestEnum.ABC), "Passthrough Error");
            Assert.That(ConsoleUtil.EnumQuery<TestEnum>(""), Is.EqualTo(TestEnum.HIJ), "Validation Error");
        });
    }

    [Test]
    public void ChoiceQuery() {
        var valid = new[] { "abc", "hij", ".23" };
        Input("abc\ndef\nhij\n123\n.23");
        Assert.Multiple(() => {
            Assert.That(ConsoleUtil.ChoiceQuery("", valid), Is.EqualTo(valid[0]), "Passthrough Fail");
            Assert.That(ConsoleUtil.ChoiceQuery("", valid), Is.EqualTo(valid[1]), "Validation Fail");
            Assert.That(ConsoleUtil.ChoiceQuery("", valid), Is.EqualTo(valid[2]), "Regex Escape Fail");
        });
    }

    [Test]
    public void IntQuery() {
        Input("abc\n123");
        Assert.That(ConsoleUtil.IntQuery(""), Is.EqualTo(123), "Regex Fail");
    }

    [Test]
    public void RangedIntQuery() {
        Input("1\n31\n26");
        Assert.That(ConsoleUtil.RangedIntQuery("", 2, 30), Is.EqualTo(26), "Boundary Fail");
    }

    [Test]
    public void ChoiceIntQuery() {
        var valid = new[] { 1, 3 };
        Input("1\n2\n3");
        Assert.Multiple(() => {
            Assert.That(ConsoleUtil.ChoiceIntQuery("", valid), Is.EqualTo(1), "Passthrough Fail");
            Assert.That(ConsoleUtil.ChoiceIntQuery("", valid), Is.EqualTo(3), "Validation Fail");
        });
    }

    private static void Input(params string[] str) {
        Console.SetIn(new StringReader(string.Join('\n', str)));
    }
}