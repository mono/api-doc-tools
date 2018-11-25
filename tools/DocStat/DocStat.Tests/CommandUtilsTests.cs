using NUnit.Framework;
using System;
namespace DocStat.Tests
{
    [TestFixture]
    public class CommandUtilsTests
    {
        [Test]
        public void CSVStringWith2()
        {
            Assert.AreEqual(@"""{0}"",""{1}""", CommandUtils.CSVFormatString(2));
        }

        [Test]
        public void CSVStringWithIndexArray()
        {
            Assert.AreEqual(@"""{1}"",""{0}""",
                            CommandUtils.CSVFormatString(2, new int[] { 1, 0 }));
        }
    }
}
