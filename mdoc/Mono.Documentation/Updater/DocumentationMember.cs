using System;
using System.Xml;
using StringList = System.Collections.Generic.List<string>;
using StringToStringMap = System.Collections.Generic.Dictionary<string, string>;

namespace Mono.Documentation.Updater
{
    class DocumentationMember
    {
        public StringToStringMap MemberSignatures = new StringToStringMap ();
        public string ReturnType;
        public StringList Parameters;
        public StringList TypeParameters;
        public string MemberName;
        public string MemberType;

        /// <summary>Removes modreq and modopt from ReturnType, Parameters, and TypeParameters</summary>
        private void CleanTypes ()
        {
            ReturnType = MemberFormatter.RemoveMod (ReturnType);
            MemberType = MemberFormatter.RemoveMod (MemberType);

            if (Parameters != null)
            {
                for (var i = 0; i < Parameters.Count; i++)
                    Parameters[i] = MemberFormatter.RemoveMod (Parameters[i]);
            }

            if (TypeParameters != null)
            {
                for (var i = 0; i < TypeParameters.Count; i++)
                    TypeParameters[i] = MemberFormatter.RemoveMod (TypeParameters[i]);
            }
        }

        public DocumentationMember (XmlReader reader)
        {
            MemberName = reader.GetAttribute ("MemberName");
            int depth = reader.Depth;
            bool go = true;
            StringList p = new StringList ();
            StringList tp = new StringList ();
            do
            {
                if (reader.NodeType != XmlNodeType.Element)
                    continue;

                bool shouldUse = true;
                try
                {
                    string apistyle = reader.GetAttribute ("apistyle");
                    shouldUse = string.IsNullOrWhiteSpace (apistyle) || apistyle == "classic"; // only use this tag if it's an 'classic' style node
                }
                catch (Exception ex) { }
                switch (reader.Name)
                {
                    case "MemberSignature":
                        if (shouldUse)
                        {
                            MemberSignatures[reader.GetAttribute ("Language")] = reader.GetAttribute ("Value");
                        }
                        break;
                    case "MemberType":
                        MemberType = reader.ReadElementString ();
                        break;
                    case "ReturnType":
                        if (reader.Depth == depth + 2 && shouldUse)
                            ReturnType = reader.ReadElementString ();
                        break;
                    case "Parameter":
                        if (reader.Depth == depth + 2 && shouldUse)
                            p.Add (reader.GetAttribute ("Type"));
                        break;
                    case "TypeParameter":
                        if (reader.Depth == depth + 2 && shouldUse)
                            tp.Add (reader.GetAttribute ("Name"));
                        break;
                    case "Docs":
                        if (reader.Depth == depth + 1)
                            go = false;
                        break;
                }
            } while (go && reader.Read () && reader.Depth >= depth);
            if (p.Count > 0)
            {
                Parameters = p;
            }
            if (tp.Count > 0)
            {
                TypeParameters = tp;
            }
            else
            {
                DiscernTypeParameters ();
            }

            CleanTypes ();
        }

        public DocumentationMember (XmlNode node)
        {
            MemberName = node.Attributes["MemberName"].Value;
            foreach (XmlNode n in node.SelectNodes ("MemberSignature"))
            {
                XmlAttribute l = n.Attributes["Language"];
                XmlAttribute v = n.Attributes["Value"];
                XmlAttribute apistyle = n.Attributes["apistyle"];
                bool shouldUse = apistyle == null || apistyle.Value == "classic";
                if (l != null && v != null && shouldUse)
                    MemberSignatures[l.Value] = v.Value;
            }
            MemberType = node.SelectSingleNode ("MemberType").InnerText;
            XmlNode rt = node.SelectSingleNode ("ReturnValue/ReturnType[not(@apistyle) or @apistyle='classic']");
            if (rt != null)
                ReturnType = rt.InnerText;
            XmlNodeList p = node.SelectNodes ("Parameters/Parameter[not(@apistyle) or @apistyle='classic']");
            if (p.Count > 0)
            {
                Parameters = new StringList (p.Count);
                for (int i = 0; i < p.Count; ++i)
                    Parameters.Add (p[i].Attributes["Type"].Value);
            }
            XmlNodeList tp = node.SelectNodes ("TypeParameters/TypeParameter[not(@apistyle) or @apistyle='classic']");
            if (tp.Count > 0)
            {
                TypeParameters = new StringList (tp.Count);
                for (int i = 0; i < tp.Count; ++i)
                    TypeParameters.Add (tp[i].Attributes["Name"].Value);
            }
            else
            {
                DiscernTypeParameters ();
            }

            CleanTypes ();
        }

        void DiscernTypeParameters ()
        {
            // see if we can discern the param list from the name
            if (MemberName.Contains ("<") && MemberName.EndsWith (">"))
            {
                var starti = MemberName.IndexOf ("<") + 1;
                var endi = MemberName.LastIndexOf (">");
                var paramlist = MemberName.Substring (starti, endi - starti);
                var tparams = paramlist.Split (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                TypeParameters = new StringList (tparams);
            }
        }
    }
}