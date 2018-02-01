using System;
using System.Linq;
using System.Collections.Generic;
namespace Mono.Documentation.Updater.Frameworks
{
    public static class FXUtils
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

        /// <summary>Returns a list of all previously processed frameworks (not including the current)</summary>
        internal static string PreviouslyProcessedFXString (FrameworkTypeEntry typeEntry) 
        {
            return string.Join (";", typeEntry
                .PreviouslyProcessedFrameworkTypes
                .Select (previous => previous.Framework.Name)
                .ToArray ());
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
