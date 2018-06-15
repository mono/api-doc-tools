using System;
using System.Collections.Generic;

namespace Mono.Documentation.Updater.Frameworks
{
    public class FrameworkItemEntry
    {
        List<string> attributes = null;
        static string[] emptyStringList = new string[0];

        public string Name { get; set; }
        public string Id { get; set; }

        public FrameworkItemEntry() {}
        public FrameworkItemEntry (string id) => Id = id;
        public FrameworkItemEntry (string id, string name) : this (id) => Name = name;


        public IEnumerable<string> Attributes {
            get => attributes == null ? (emptyStringList as IEnumerable<string>): (attributes as IEnumerable<string>);
        }

        public void AddAttribute (string attributeValue)
        {
            if (attributes == null)
                attributes = new List<string> ();

            attributes.Add (attributeValue);
        }
    }
}
