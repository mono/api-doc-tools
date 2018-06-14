using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using System;
using System.Linq;

namespace mdoc.Test
{
    [TestFixture ()]
    public class FrameworkAlternateTests
    {
        [Test ()]
        public void AddToEmptyList()
        {
            string newValue = FXUtils.AddFXToList ("", "One");

            Assert.AreEqual ("One", newValue);
        }


        [Test ()]
        public void RemoveFromList()
        {
            string newValue = FXUtils.RemoveFXFromList ("One", "One");
            Assert.AreEqual ("", newValue);
        }
    }
}
