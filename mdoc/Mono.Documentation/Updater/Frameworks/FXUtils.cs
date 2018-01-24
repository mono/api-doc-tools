using System;
using System.Linq;
using System.Collections.Generic;
namespace Mono.Documentation.Updater.Frameworks
{
    public class FXUtils
    {
        public static string AddFXToList (string existingValue, string newFX)
        {
            var splitValue = SplitList (existingValue);
            if (!splitValue.Contains (newFX)) splitValue.Add (newFX);
            return JoinList (splitValue);
        }

        public static string RemoveFXFromList (string existingValue, string FXToRemove)
        {
            var splitValue = SplitList (existingValue);
            splitValue.Remove (FXToRemove);
            return JoinList (splitValue);
        }

        static List<string> SplitList (string existingValue)
        {
            return existingValue.Split (new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList ();
        }

        static string JoinList (List<string> splitValue)
        {
            return string.Join (";", splitValue.ToArray ());
        }
    }
}
