using Mono.Documentation.Updater.Frameworks;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
        public void AddToExistingList ()
        {
            string newValue = FXUtils.AddFXToList ("One", "Two");

            Assert.AreEqual ("One;Two", newValue);
        }

        [Test ()]
        public void AddDupeToExistingList ()
        {
            string newValue = FXUtils.AddFXToList ("One", "One");

            Assert.AreEqual ("One", newValue);
        }


        [Test ()]
        public void RemoveFromList()
        {
            string newValue = FXUtils.RemoveFXFromList ("One", "One");
            Assert.AreEqual ("", newValue);
        }


        [Test ()]
        public void RemoveFromMultiList ()
        {
            string existingValue = "Pre;One;Post";
            string fxToRemove = "One";
            string postValue = "Pre;Post";

            string newValue = FXUtils.RemoveFXFromList (existingValue, fxToRemove);
            Assert.AreEqual (postValue, newValue);

            // make sure the cache returns the same value
            newValue = FXUtils.RemoveFXFromList (existingValue, fxToRemove);
            Assert.AreEqual (postValue, newValue);
        }

        [Test ()]
        public void AddToMultiList ()
        {
            string existingValue = "Pre;Post";
            string fxToAdd = "One";
            string postValue = "Pre;Post;One";

            string newValue = FXUtils.AddFXToList (existingValue, fxToAdd);
            Assert.AreEqual (postValue, newValue);

            // make sure the cache returns the same value
            newValue = FXUtils.AddFXToList (existingValue, fxToAdd);
            Assert.AreEqual (postValue, newValue);
        }

        [Test]
        public void LastFramework()
        {
            List<FrameworkEntry> entries = new List<FrameworkEntry>();
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));

            Assert.IsFalse (entries[0].IsLastFramework);
            Assert.IsFalse (entries[1].IsLastFramework);
            Assert.IsFalse (entries[2].IsLastFramework);
            Assert.IsTrue (entries[3].IsLastFramework);
        }

        [Test]
        public void FirstFramework ()
        {
            List<FrameworkEntry> entries = new List<FrameworkEntry> ();
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));
            entries.Add (new FrameworkEntry (entries, entries));

            Assert.IsTrue (entries[0].IsFirstFramework);
            Assert.IsFalse (entries[1].IsFirstFramework);
            Assert.IsFalse (entries[2].IsFirstFramework);
            Assert.IsFalse (entries[3].IsFirstFramework);
        }
    }
}
