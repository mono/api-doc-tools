using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class InternalizeCommand : ApiCommand
    {

        public override void Run(IEnumerable<string> args)
        {
            string rootdir = "";
            string omitlist = "";
            string processlist = "";
            string pattern = "";
            List<string> extras = CommandUtils.ProcessFileArgs(args,
                                                               ref rootdir,
                                                               ref omitlist,
                                                               ref processlist,
                                                               ref pattern
                                                              );

            string message = "For internal use only.";
            string sigil = "To be added.";
            bool nocheck = false;
            bool nosigil = false;

            var opt = new OptionSet {
                { "m|message=", (m) => message = m },
                { "s|sigil=", (s) => sigil = s },
                { "no-check-browsable", (n) => nocheck = n != null},
                { "no-check-TBA", (t) => nosigil = t != null }
            };

            extras = opt.Parse(extras);

            CommandUtils.ThrowOnFiniteExtras(extras);

            Func<XElement, bool> hassigil;
            Func<XDocument, bool> typehassigil;
            Func<XElement, bool> qualifies;

            if (nosigil)
            {
                // Mark types and members internal, regardless of whether the summaries are filled out
                hassigil = (x) => true;
                typehassigil = (x) => true;
            }
            else
            {
                hassigil = (e) => e.Element("Docs").Element("summary").Value == sigil;
                typehassigil = (t) => t.Element("Type").Element("Docs").Element("summary").Value == sigil;
            }

            if (!nocheck)
            {
                qualifies = (e) =>
                {
                    return e.Elements("Attributes")
                     .Any((XElement child) => child.Elements("Attribute")
                          .Any((XElement name) => name.Value.Contains("EditorBrowsableState.Never")))
                            && hassigil(e);

                };
            }
            else
            {
                qualifies = hassigil;
            }

            foreach (string file in CommandUtils.GetFileList(processlist, omitlist, rootdir, pattern))
            {
                XDocument xdoc = new XDocument(XElement.Load(file));
                // Find every member that has the internal marker and summary="To be added." (or the provided sigil)

                XElement memberRoot = xdoc.Element("Type").Element("Members");
                if (memberRoot == null || !memberRoot.Descendants().Any())
                    continue;

                IEnumerable<XElement> hits = memberRoot.Elements("Member").Where(s => qualifies(s));

                foreach (XElement x in hits)
                {
                    x.Element("Docs").Element("summary").Value = message;
                }

                if (typehassigil(xdoc))
                    xdoc.Element("Type").Element("Docs").Element("summary").Value = message;


                CommandUtils.WriteXDocument(xdoc, file);
            }
        }
    }
}
