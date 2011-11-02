using NUnit.Framework;
using SharpTestsEx;

namespace RazorCandle.Tests
{
    [TestFixture]
    public class ArgumentsTests
    {
        [Test]
        public void WhenDestinationIsNullThenReturnSourceWithHtmlExtension()
        {
            var arg = new Arguments();
            arg.Source = "c:\\test\\foo\\bar.cshtml";
            arg.Destination.Should().Be.EqualTo("c:\\test\\foo\\bar.html");
        }
    }
}
