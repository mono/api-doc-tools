﻿using System;
using System.IO;
using System.Linq;
using System.Xml;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    class MsxdocDocumentationImporter : DocumentationImporter
    {

        XmlDocument slashdocs;

        public MsxdocDocumentationImporter (string file)
        {
            try
            {
                char oppositeSlash = Path.DirectorySeparatorChar == '/' ? '\\' : '/';
                if (file.Contains (oppositeSlash))
                    file = file.Replace (oppositeSlash, Path.DirectorySeparatorChar);

                var xml = File.ReadAllText (file);

                // Ensure Unix line endings
                xml = xml.Replace ("\r", "");

                slashdocs = new XmlDocument ();

                slashdocs.LoadXml (xml);
            }
            catch (IOException ex)
            {
                Console.WriteLine ($"Importer Error: {ex.Message}");
            }
        }

        public override void ImportDocumentation (DocsNodeInfo info)
        {
            XmlNode elem = GetDocs (info.Member ?? info.Type);

            if (elem == null)
                return;

            XmlElement e = info.Node;

            if (elem.SelectSingleNode ("summary") != null)
                MDocUpdater.ClearElement (e, "summary");
            if (elem.SelectSingleNode ("remarks") != null)
                MDocUpdater.ClearElement (e, "remarks");
            if (elem.SelectSingleNode ("value") != null || elem.SelectSingleNode ("returns") != null)
            {
                MDocUpdater.ClearElement (e, "value");
                MDocUpdater.ClearElement (e, "returns");
            }

            foreach (XmlNode child in elem.ChildNodes)
            {
                switch (child.Name)
                {
                    case "param":
                    case "typeparam":
                        {
                            XmlAttribute name = child.Attributes["name"];
                            if (name == null)
                                break;
                            XmlElement p2 = (XmlElement)e.SelectSingleNode (child.Name + "[@name='" + name.Value + "']");
                            if (p2 != null)
                                p2.InnerXml = child.InnerXml;
                            break;
                        }
                    // Occasionally XML documentation will use <returns/> on
                    // properties, so let's try to normalize things.
                    case "value":
                    case "returns":
                        {
                            XmlElement v = e.OwnerDocument.CreateElement (info.ReturnIsReturn ? "returns" : "value");
                            v.InnerXml = child.InnerXml;
                            e.AppendChild (v);
                            break;
                        }
                    case "altmember":
                    case "exception":
                    case "permission":
                        {
                            XmlAttribute cref = child.Attributes["cref"] ?? child.Attributes["name"];
                            if (cref == null)
                                break;
                            XmlElement a = (XmlElement)e.SelectSingleNode (child.Name + "[@cref='" + cref.Value + "']");
                            if (a == null)
                            {
                                a = e.OwnerDocument.CreateElement (child.Name);
                                a.SetAttribute ("cref", cref.Value);
                                e.AppendChild (a);
                            }
                            a.InnerXml = child.InnerXml;
                            break;
                        }
                    case "seealso":
                        {
                            XmlAttribute cref = child.Attributes["cref"];
                            if (cref == null)
                                break;
                            XmlElement a = (XmlElement)e.SelectSingleNode ("altmember[@cref='" + cref.Value + "']");
                            if (a == null)
                            {
                                a = e.OwnerDocument.CreateElement ("altmember");
                                a.SetAttribute ("cref", cref.Value);
                                e.AppendChild (a);
                            }
                            break;
                        }
                    default:
                        {
                            bool add = true;
                            if (child.NodeType == XmlNodeType.Element &&
                                    e.SelectNodes (child.Name).Cast<XmlElement> ().Any (n => n.OuterXml == child.OuterXml))
                                add = false;
                            if (add)
                                MDocUpdater.CopyNode (child, e);
                            break;
                        }
                }
            }
        }

        private XmlNode GetDocs (MemberReference member)
        {
            string slashdocsig = MDocUpdater.slashdocFormatter.GetDeclaration (member);
            if (slashdocsig != null && slashdocs != null)
                return slashdocs.SelectSingleNode ("doc/members/member[@name='" + slashdocsig + "']");
            return null;
        }
    }
}