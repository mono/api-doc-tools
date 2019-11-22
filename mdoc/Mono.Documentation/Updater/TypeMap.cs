using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Mono.Documentation.Updater
{
    public class TypeMap
    {
        /// <summary>This is the core lookup data structure</summary>
        Dictionary<string, Dictionary<string,TypeMapItem>> map;

        public List<TypeMapItem> Items { get; private set; }

        public string GetTypeName(string lang, string typename)
        {
            if (map == null) InitializeMap();

            Dictionary<string, TypeMapItem> val;
            if (map.TryGetValue(lang, out val))
            {
                TypeMapItem itemMap;
                if (val.TryGetValue(typename, out itemMap))
                {
                    return itemMap.To;
                }
            }

            return typename;
        }

        private void InitializeMap()
        {
            map = new Dictionary<string, Dictionary<string, TypeMapItem>>();

            // init the map
            foreach (var item in Items)
            {

                // for each language that this item applies to, make a separate entry in the map for that language
                foreach (var itemLang in item.LangList)
                {
                    Dictionary<string, TypeMapItem> langList;
                    if (!map.TryGetValue(itemLang, out langList))
                    {
                        langList = new Dictionary<string, TypeMapItem>();
                        map.Add(itemLang, langList);
                    }

                    if (!langList.ContainsKey(item.From))
                        langList.Add(item.From, item);
                }
            }
        }

        public static TypeMap FromXml(string path)
        {
            var doc = XDocument.Load(path);

            return FromXDocument(doc);
        }

        public static TypeMap FromXDocument(XDocument doc)
        {
            TypeMap map = new TypeMap
            {
                Items = doc.Root.Elements()
                   .Select(e =>
                   {
                       switch (e.Name.LocalName)
                       {
                           case "TypeReplace":
                               return ItemFromElement<TypeMapItem>(e);
                           case "InterfaceReplace":
                               var item = ItemFromElement<TypeMapInterfaceItem>(e);
                               item.Members = e.Element("Members");
                               return item;
                           default:
                               Console.WriteLine($"\tUnknown element: {e.Name.LocalName}");
                               break;
                       }
                       return new TypeMapItem();
                   })
                   .ToList()
            };

            return map;
        }

        private static T ItemFromElement<T>(XElement e) where T: TypeMapItem, new()
        {
            return new T
            {
                From = e.Attribute("From").Value,
                To = e.Attribute("To").Value,
                Langs = e.Attribute("Langs").Value
            };
        }

        public TypeMapInterfaceItem HasInterfaceReplace(string lang, string facename)
        {
            Dictionary<string, TypeMapItem> typemap;
            if (map.TryGetValue(lang, out typemap))
            {
                TypeMapItem item;
                if (typemap.TryGetValue(facename, out item))
                {
                    var ifaceItem = item as TypeMapInterfaceItem;
                    if (ifaceItem != null)
                        return ifaceItem;
                }
            }

            return null;
        }
    }

    public class TypeMapItem
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Langs { get; set; }

        public IEnumerable<string> LangList { get
            {
                foreach (var l in Langs.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()))
                    yield return l;
            }
        }
    }

    public class TypeMapInterfaceItem : TypeMapItem
    {
        public XElement Members { get; set; }

        public XmlElement ToXmlElement(XElement el)
        {
            var doc = new XmlDocument();
            doc.Load(el.CreateReader());
            var xel = doc.DocumentElement;
            xel.ParentNode.RemoveChild(xel);
            return xel;
        }
    }
}
