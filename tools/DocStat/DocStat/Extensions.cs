using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DocStat
{
	public static class Extensions
	{
		public static void Iter<T> (this IEnumerable<T> list, Action<T> action)
		{
			foreach (var item in list)
				action (item);
		}

		public static XElement Element (this XDocument element, params string[] querychain)
		{
			if (element.Root.Name.LocalName.Equals (querychain.FirstOrDefault ()))
				return Element (element.Root, querychain.Skip (1).ToArray ());

			return Element (element.Root, querychain);
		}

		public static XElement Element (this XElement element, params string[] querychain)
		{
			XElement current = element;
			foreach (var q in querychain)
			{
				if (current == null) break;
				current = current.Element (q);
			}

			return current == element ? null : current;
		}
	}
}