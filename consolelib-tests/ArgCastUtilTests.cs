using System.Net;
using CoolandonRS.consolelib.Arg;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(ArgCastUtil))]
public class ArgCastUtilTests {
    private struct TestStruct(string str) {
        public string str = str;
    }
    
    [Test]
    public void TryFindParse() {
        Assert.Multiple(() => {
            Assert.That(ArgCastUtil.TryFindParse<int>("1"), Is.EqualTo(1), "Parse usage failure");
            Assert.That(ArgCastUtil.TryFindParse<int>("2"), Is.EqualTo(2), "Parse repeated use failure");
            Assert.That(ArgCastUtil.TryFindParse<TestStruct>("abc").str, Is.EqualTo("abc"), "Constructor usage failure");
            Assert.That(ArgCastUtil.TryFindParse<TestStruct>("def").str, Is.EqualTo("def"), "Constructor repeated use failure");
            Assert.Throws<MissingMethodException>(() => ArgCastUtil.TryFindParse<List<string>>("abc"), "No throw on missing methods");
        });
    }
    
    [Test]
    public void Hex([Random(1, 255, 10)] int val, [Values(true, false)] bool prefix) => Assert.That(ArgCastUtil.Hex((prefix ? "0x" : "") + Convert.ToString(val, 16)), Is.EqualTo(val));

    [Test]
    public void HostnameOrIp() {
        Assert.Multiple(() => {
            Assert.That(ArgCastUtil.HostnameOrIP("192.168.1.1"), Is.EqualTo(IPAddress.Parse("192.168.1.1")), "IP failure");
            Assert.DoesNotThrow(() => ArgCastUtil.HostnameOrIP("google.com"), "Google.com dns lookup failure");
            Assert.Throws<InvalidCastException>(() => ArgCastUtil.HostnameOrIP("abckdjsfoiu.asdkjl"), "Garbage domain lookup success");
        });
    }

    [Test]
    public void GetArrayCast() {
        Assert.That(ArgCastUtil.GetArrayCast(int.Parse)("1,2,3"), Is.EqualTo(new[] {1, 2, 3}), "Array cast failure");
    }
}