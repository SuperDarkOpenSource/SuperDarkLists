using FluentAssertions;
using SuperdarkLists.Common.Extensions;

namespace SuperdarkLists.Test.UnitTests.Util;

[TestFixture]
[Category(TestCategories.UnitTests)]
public class StringExtensionTests
{
    [TestCase(null, true)]
    [TestCase("", true)]
    [TestCase("something", false)]
    public void Works(string? str, bool expectedResult)
    {
        var result = str.IsNullOrEmpty();

        result.Should().Be(expectedResult);
    }

    [Test]
    public void EmptyWorks()
    {
        var str = string.Empty;

        var result = str.IsNullOrEmpty();

        result.Should().BeTrue();
    }
}