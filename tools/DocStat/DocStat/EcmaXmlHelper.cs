﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DocStat
{
    public static class EcmaXmlHelper
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

					_current = _selector(_current);

					if (_current == null)
						return null;
				}
				return _current;
			};
		}

		public static string GetParallelFilePathFor(string pathToTypeToFix,
                                                    string rootOfReferenceFiles,
													string rootOfFilesToFix,
                                                    Func<string, string> referenceRootTransform = null,
                                                    Func<string, string> referencePathTransform = null)
		{
            
			string fullFixPath = Path.GetFullPath(pathToTypeToFix);

			string fullFixRoot = Path.GetFullPath(rootOfFilesToFix);

			rootOfReferenceFiles =
				null == referenceRootTransform ? rootOfReferenceFiles : referenceRootTransform(rootOfReferenceFiles);
            string fullRefRoot = Path.GetFullPath(rootOfReferenceFiles);

            string fullReferencePath = fullFixPath.Replace(fullFixRoot, fullRefRoot);

            fullReferencePath = 
                null == referencePathTransform ? fullReferencePath : referencePathTransform(fullReferencePath);
            return fullReferencePath;
		}

        public static XDocument GetParallelXDocFor(string parallelFilePath,
                                                HashSet<string> refPaths = null)
        {

            if (!File.Exists(parallelFilePath))
                return null;
            
            if ((null != refPaths) && !refPaths.Contains(parallelFilePath))
                return null;
            
            return XDocument.Load(parallelFilePath);
        }

		public static IEnumerable<XElement> ElementsOfInterest(XDocument ecmaXmlDoc)
		{
			// (1) Yield type-level summary and remarks:
			yield return ecmaXmlDoc.Element("Type").Element("Docs").Element("summary");

			yield return ecmaXmlDoc.Element("Type").Element("Docs").Element("remarks");


			var members = ecmaXmlDoc.Element("Type").Element("Members");

			if (null != members)
			{

				foreach (XElement m in members.Elements())
				{
					// (2) Yield summary, remarks, return values, parameters, and typeparams
					XElement docsElement = m.Element("Docs");

					yield return docsElement.Element("summary");


					XElement remarks = docsElement.Element("remarks");
					if (null != remarks)
						yield return remarks;

					XElement returns = docsElement.Element("returns");
					if (null != returns)
						yield return returns;

					if (docsElement.Elements("param").Any())
					{
						IEnumerable<XElement> _params = docsElement.Elements("param");
						foreach (XElement p in _params)
						{
							yield return p;
						}
					}

					if (docsElement.Elements("typeparam").Any())
					{
						IEnumerable<XElement> typeparams = docsElement.Elements("typeparam");
						foreach (XElement p in typeparams)
						{
							yield return p;
						}
					}
				}
			}
		}

        public static IEnumerable<XElement> Members(XDocument ecmaXmlDoc)
        {
            var members = ecmaXmlDoc.Element("Type").Element("Members");
            if (null != members)
            {
                foreach (var m in members.Elements("Member"))
                        yield return m;
            }
            yield break;
        }

		public static IEnumerable<XElement> NewMembers(XDocument newXml, XDocument oldXml)
		{

            if (null == Members(newXml))
            { yield break; }
            if (null == oldXml)
			{
                
				foreach (var e in Members(newXml))
					yield return e;
			}
			else
			{
                foreach (var e in Members(newXml))
                {
                    if (null == GetSelectorFor(e)(oldXml))
                        yield return e;
                }

			}
		}
	}
}
