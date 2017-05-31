using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DocStat
{
    public static class ParallelXmlHelper
	{
		public static void Fix(XElement toFix, XElement forReference)
		{
			if (null == forReference)
			{
				return;
			}

			if (XNode.DeepEquals(toFix, forReference))
			{
				return;
			}

			toFix.ReplaceWith(forReference);
		}

		// Sometimes, the identifying attribute lives in a child. This means that
		// we need to return a predicate that finds the piece to match.
        // So:
		// Return a function that takes an XElement with a known unique identifier
		// and return a predicate that gets the XAttribute that identifies it
		public static Func<XElement, XAttribute> IdentifyingAttributePredicateFor(XElement el)
		{
			switch (el.Name.ToString())
			{
				case "typeparam":
				case "param":
					return (XElement e) => e.Attribute("name");
				case "Member":
					// The ILAsm signature is unique, and always present
					return (XElement e) => e.Elements("MemberSignature")
														.First((a) => a.Attribute("Language").Value == "ILAsm")
														.Attribute("Value");
				case "related":
					return (XElement e) => e.Attribute("href"); ;
				default:
					throw new Exception("Encountered plural node of type " + el.Name);
			}
		}

		// Walk the hierarchy to get a selector that can be used to find the current element
		// This selector can then be used on the parallel XDocument to retrieve the equivalent
		// XElement. The selector returns null early if any selector returns null
		public static Func<XDocument, XElement> GetSelectorFor(XElement toSelect)
		{
			List<Func<XElement, XElement>> toRun = new List<Func<XElement, XElement>>();

			XElement current = toSelect;
			Func<XElement, XElement> selector;

			while (current.Parent != null)
			{
				XName currentName = current.Name;
				if (current.Parent.Elements(currentName).Count() > 1)
				{
					// Function that finds the element-specific unique attribute.
					Func<XElement, XAttribute> uniquePredicate
								= IdentifyingAttributePredicateFor(current);

					// Get the unique attribute value in *this* xml tree.
					// (We know this exists.)
					string uniqueAttr = uniquePredicate(current).Value;

					selector = (XElement arg) => arg.Elements(currentName)
													.FirstOrDefault((XElement a) =>
														   uniquePredicate(a).Value == uniqueAttr);
				}
				else
				{
					selector = (XElement arg) => arg.Element(currentName);
				}

				toRun.Add(selector);
				current = current.Parent;
			}

			return (XDocument doc) =>
			{
				XElement _current = doc.Root;

				foreach (var _selector in ((IEnumerable<Func<XElement, XElement>>)toRun).Reverse())
				{

					_current = _selector.Invoke(_current);

					if (_current == null)
						return null;
				}
				return _current;
			};
		}

		public static XElement ParallelElement(XElement sourceElement,
											   string sourcePath,
											   string sourceRoot,
											   string refRoot,
											   HashSet<string> refPaths)
		{
			string parallelPath = GetParallelFilePathFor(sourcePath, refRoot, sourceRoot);

			// bail early if we can

			if (!File.Exists(parallelPath) || !refPaths.Contains(parallelPath))
				return null;

			FileAttributes attr = File.GetAttributes(parallelPath);
			if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
				return null;

			XDocument refToSearch = XDocument.Load(parallelPath);
			Console.WriteLine("Found parallel document");
			var toReturn = GetSelectorFor(sourceElement).Invoke(refToSearch);
			Console.WriteLine("Got the parallel element");
			return toReturn;
		}

		public static string GetParallelFilePathFor(string pathToTypeToFix,
													string rootOfFilesToUse,
													string rootOfFilesToFix)
		{
			string fullFixPath = Path.GetFullPath(pathToTypeToFix);
			string fullFixRoot = Path.GetFullPath(rootOfFilesToFix);
			string fullRefRoot = Path.GetFullPath(rootOfFilesToUse);
			return fullFixPath.Replace(fullFixRoot, fullRefRoot);
		}
	}
}
