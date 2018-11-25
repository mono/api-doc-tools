using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class ObsoleteCommand : ApiCommand
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
                                                               ref pattern);
            string obsoleteMarker = "System.Obsolete";
            string sigil = "To be added.";
            bool skipSigil = false;
            string message = "Deprecated. Do not use.";
            var opt = new OptionSet {
                {"a|attribute",
                    (x) => obsoleteMarker = x },
                { "s|sigil=", (s) => sigil = s },
                { "no-check-TBA", (s) => skipSigil = s != null},
                { "m|message=", (m) => message = m}
            };

            extras = opt.Parse(extras);
            CommandUtils.ThrowOnFiniteExtras(extras);

            Func<XElement, bool> sigilCheck;
            Func<XElement, bool> obsoleteCheck;

            if (skipSigil)
            {
                sigilCheck = (e) => true;
            }
            else
            {
                sigilCheck = (e) => e.Element("Docs").Element("summary").Value == sigil;
            }

            obsoleteCheck = (e) => e.Elements("Attribute").Any((arg) =>
                                                                  arg.Elements("Attribute").Any((arg2) =>
                                                                                                arg2.Value.StartsWith(obsoleteMarker))); ; ;

            foreach (string file in CommandUtils.GetFileList(processlist, omitlist, rootdir, pattern))
            {
                // find all the ones that have attributes that start with the provided attribute
                XDocument xdoc = XDocument.Load(file);

                XElement memberRoot = xdoc.Element("Type").Element("Members");
                if (memberRoot == null || !memberRoot.Descendants().Any())
                    continue;

                foreach (XElement toMark in memberRoot.Elements("Member")
                         .Where((e) => obsoleteCheck(e) && sigilCheck(e)))
                {
                    toMark.Element("Docs").Element("summary").Value = message;
                }
                CommandUtils.WriteXDocument(xdoc, file);
            }
        }
    }
}
