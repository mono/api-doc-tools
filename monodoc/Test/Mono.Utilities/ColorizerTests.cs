using NUnit.Framework;
using Mono.Utilities;
using Monodoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoTests.Mono.Utilities
{
    [TestFixture]
    public class ColorizerTests
    {
        [Test]
        public void ColorizeTest()
        {
            string text = "public int InitialCapacity { get; set; } = 100;";
            string result = Colorizer.Colorize(text, "cs");
            Assert.AreEqual("<font color=\"blue\">public</font>&nbsp;<font color=\"blue\">int</font>&nbsp;InitialCapacity&nbsp;{&nbsp;get;&nbsp;set;&nbsp;}&nbsp;=&nbsp;100;", result);
        }
    }
}
