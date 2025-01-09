using Mono.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace mdoc.Test
{
    [TestFixture]
    public class StringCodaTests
    {
        [Test]
        public void WrappedLines_SingleWidth_WrapsCorrectly()
        {
            string input = "This is a test string that should be wrapped.";
            int width = 10;
            var expected = new List<string> { "This is a", "test", "string", "that", "should be", "wrapped." };

            var result = StringCoda.WrappedLines(input, width);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WrappedLines_MultipleWidths_WrapsCorrectly()
        {
            string input = "This is a test string that should be wrapped.";
            int[] widths = { 10, 5, 15 };
            var expected = new List<string> { "This is a", "test", "string that", "should be", "wrapped." };

            var result = StringCoda.WrappedLines(input, widths);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WrappedLines_EmptyString_ReturnsEmpty()
        {
            string input = "";
            int width = 10;
            var expected = new List<string> { "" };

            var result = StringCoda.WrappedLines(input, width);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void WrappedLines_NullWidths_ThrowsArgumentNullException()
        {
            string input = "This is a test string.";
            IEnumerable<int> widths = null;

            _ = Assert.Throws<ArgumentNullException>(() => StringCoda.WrappedLines(input, widths));
        }
    }
}
