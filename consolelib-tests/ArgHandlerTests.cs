﻿using CoolandonRS.consolelib.Arg;
using CoolandonRS.consolelib.Arg.Builders;

namespace consolelib_tests;

[TestFixture, TestOf(typeof(ArgHandler))]
public class ArgHandlerTests {
    private static readonly ArgHandlerConfig config = new() {
        DoErrors = true,
        ErrorStyle = ArgHandlerConfig.ArgErrorStyle.Throw
    };
    
    [Test]
    public void ParseSingleFlagArg() {
        var argHandler = new ArgHandler(config, new SingleFlagArg("verbose", "print verbose", 'v'), new SingleFlagArg("!verbose", "don't print verbose", 'V', true));
        Assert.Multiple(() => {
            Assert.DoesNotThrow(() => argHandler.Parse(["-v"]), "Parse threw");
            Assert.That(argHandler.IsDefault("verbose"), Is.False, "Default reported when present");
            Assert.That(argHandler.IsDefault("!verbose"), Is.True, "Nondefault reported when not present");
            Assert.That(argHandler.Get<bool>("verbose"), Is.True, "Get passthrough failure");
            Assert.Throws<InvalidCastException>(() => argHandler.Get<HttpClient>("!verbose"), "Non throw on invalid cast");
        });
    }

    [Test]
    public void ParseNonSingleFlagArg() {
        var argHandler = new ArgHandler(config, new FlagArg("verbose", "print verbose"), new FlagArg("fbi-trigger", "notify fbi of suspicious activity", true));
        Assert.Multiple(() => {
            Assert.DoesNotThrow(() => argHandler.Parse(["--verbose"]), "Parse threw");
            Assert.That(argHandler.IsDefault("verbose"), Is.False, "Default reported when present");
            Assert.That(argHandler.IsDefault("fbi-trigger"), Is.True, "Non-default reported when not present");
            Assert.That(argHandler.Get<bool>("verbose"), Is.True, "Get passthrough failure");
            Assert.Throws<InvalidCastException>(() => argHandler.Get<int>("fbi-trigger"), "Non throw on invalid cast");
        });
    }

    [Test]
    public void ParseSingleValueArg() {
        var argHandler = new ArgHandler(config, new SingleValueArg<int>("number", "number", 'n', 0, int.Parse), new SingleValueArg<int>("number2", "number2", 'N', 2, int.Parse));
        Assert.Multiple(() => {
            Assert.Throws<NonTrailingSingleValueArgException>(() => argHandler.Parse(["-nN", "2"]), "Non trailing single value didn't error");
            Assert.Throws<InsufficientDataException>(() => argHandler.Parse(["-n"]), "Success on insufficient data");
            Assert.DoesNotThrow(() => argHandler.Parse(["-n", "3"]), "Parse threw");
            Assert.That(argHandler.IsDefault("number"), Is.False, "Default reported when present");
            Assert.That(argHandler.IsDefault("number2"), Is.True, "Nondefault reported when not present");
            Assert.That(argHandler.Get<int>("number"), Is.EqualTo(3), "Get passthrough failure");
            Assert.Throws<InvalidCastException>(() => argHandler.Get<HttpClient>("number"), "Non throw on invalid cast");
        });
    }

    [Test]
    public void ParseNonSingleValueArg() {
        var argHandler = new ArgHandler(config, new ValueArg<int>("number", "number", 0, int.Parse), new ValueArg<int>("number2", "number2", 2, int.Parse));
        Assert.Multiple(() => {
            Assert.Throws<InsufficientDataException>(() => argHandler.Parse(["--number"]), "Success on insufficient data");
            Assert.DoesNotThrow(() => argHandler.Parse(["--number", "12"]), "Parse threw");
            Assert.That(argHandler.IsDefault("number"), Is.False, "Default reported when present");
            Assert.That(argHandler.IsDefault("number2"), Is.True, "Nondefault reported when not present");
            Assert.That(argHandler.Get<int>("number"), Is.EqualTo(12), "Get passthrough failure");
            Assert.Throws<InvalidCastException>(() => argHandler.Get<bool>("number"), "Non throw on invalid cast");
            Assert.DoesNotThrow(() => argHandler.Parse(["--number2=15"]), "Parse threw");
        });
    }

    [Test]
    public void GenerateHelp() {
        Assert.That(false); //TODO
    }
}