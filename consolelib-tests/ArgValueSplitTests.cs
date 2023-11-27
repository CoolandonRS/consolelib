using System.Globalization;
using NUnit.Framework.Internal;

namespace consolelib_tests; 

[TestFixture, TestOf(typeof(ArgValueSplit))]
public class ArgValueSplitTests {
    
    [Test]
    public static void SingleSuccess([Range(33, 126)] byte b) {
        var c = (char)b;
        Console.WriteLine(c);
        var split = new ArgValueSplit(new[] { c });
        string s1 = RandomString(c), s2 = RandomString(c);
        var o = split.Parse($"{s1}{c}{s2}");
        Assert.Multiple(() => {
            Assert.That(o.status, Is.EqualTo(ArgValueSplit.Status.Success), "Non success return status");
            Assert.That(o.prefix, Is.EqualTo(s1), "Prefix failure");
            Assert.That(o.postfix, Is.EqualTo(s2), "Postfix failure");
        });
    }

    [Test]
    public static void MultiSuccess([Random(33, 126, 2)] byte b1, [Random(33, 126, 2)] byte b2, [Random(33, 126, 2)] byte b3, [Range(1, 3)] int use) {
        var arr = new[] { b1, b2, b3 }[..use].Select(b => (char)b).ToArray();
        var split = new ArgValueSplit(arr);
        string s1 = RandomString(arr), s2 = RandomString(arr);
        var o = split.Parse($"{s1}{arr.MinBy(_ => Guid.NewGuid())}{s2}");
        Assert.Multiple(() => {
            Assert.That(o.status, Is.EqualTo(ArgValueSplit.Status.Success), "Non success return status");
            Assert.That(o.prefix, Is.EqualTo(s1), "Prefix failure");
            Assert.That(o.postfix, Is.EqualTo(s2), "Postfix failure");
        });
    }

    [Test]
    public static void Failure([Random(33, 126, 5)] byte b) {
        var c = (char)b;
        Console.WriteLine(c);
        var split = new ArgValueSplit(new[] { c });
        var s = RandomString(c);
        var o = split.Parse(s);
        Assert.Multiple(() => {
            Assert.That(o.status, Is.EqualTo(ArgValueSplit.Status.Failure), "Non failure return status");
            Assert.That(o.prefix, Is.EqualTo(s), "Prefix passthrough failure");
            Assert.That(o.postfix, Is.Null, "Non null postfix");
        });
    }
    
    [Test]
    public static void SpaceDelimited() {
        var split = new ArgValueSplit(new[] { ' ' });
        var s = RandomString(' ');
        var o = split.Parse(s);
        Assert.Multiple(() => {
            Assert.That(o.status, Is.EqualTo(ArgValueSplit.Status.Advance), "Non advance return status");
            Assert.That(o.prefix, Is.EqualTo(s), "Prefix passthrough failure");
            Assert.That(o.postfix, Is.Null, "Non null postfix");
        });
    }
    
    [Test]
    public static void ReoccurringDelimiter([Random(33, 126, 5)] byte b) {
        var c = (char)b;
        Console.WriteLine(c);
        var split = new ArgValueSplit(new[] { c });
        string s1 = RandomString(c), s2 = RandomString(c);
        var edited = $"{s2[..2]}{c}{s2[2..5]}{c}{s2[5..]}";
        var o = split.Parse($"{s1}{c}{edited}");
        Assert.Multiple(() => {
            Assert.That(o.status, Is.EqualTo(ArgValueSplit.Status.Success), "Non success return status");
            Assert.That(o.prefix, Is.EqualTo(s1), "Prefix failure");
            Assert.That(o.postfix, Is.EqualTo(edited), "Postfix failure");
        });
    }

    private static string RandomString(char exclude) {
        var str = Path.GetRandomFileName();
        return str.Contains(exclude) ? str.Replace(exclude, (char)((byte)exclude + 1)) : str;
    }
    private static string RandomString(char[] exclude) {
        var str = Path.GetRandomFileName();
        return new string(str.Select(c => {
            while (exclude.Contains(c)) {
                c = (char)((byte)c + 1);
            }
            return c;
        }).ToArray());
    }
}