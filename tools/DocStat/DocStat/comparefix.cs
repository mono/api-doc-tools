using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class CompareFixCommand : ApiCommand
    {

        public override void Run(IEnumerable<string> args)
        {
            string filesToFixDir = "";
            string omitlist = "";
            string processlist = "";
            string pattern = "";

            List<string> extras = CommandUtils.ProcessFileArgs(args,
                                                               ref filesToFixDir,
                                                               ref omitlist,
                                                               ref processlist,
                                                               ref pattern);
            // must have
            string filesToUseDir = "";
            // should have
            bool doSummaries = true;
            bool doParameters = true;
            bool doReturns = true;
            bool doRemarks = true;
            bool doTypes = true;
            // nice to have
            bool dryRun = false;
            bool reportChanges = false;

            var opts = new OptionSet {
                {"f|fix=", (f) => filesToFixDir = f},
                {"u|using=", (u) => filesToUseDir = u},
                {"s|summaries", (s) => doSummaries = s != null},
                {"a|params", (p) => doParameters = p != null },
                {"r|retvals", (r) => doReturns = r != null },
                {"m|remarks", (r) => doRemarks = r != null },
                {"t|typesummaries", (t) => doTypes = t != null },
                {"y|dryrun", (d) => dryRun = d != null },
                {"c|reportchanges", (c) => reportChanges = c != null },
            };

            extras = opts.Parse(extras);
            CommandUtils.ThrowOnFiniteExtras(extras);
            if (String.IsNullOrEmpty(filesToUseDir))
                throw new ArgumentException("You must supply a parallel directory from which to source new content with '[u|using]'=.");


            IEnumerable<string> toFix = CommandUtils.GetFileList(processlist, omitlist, filesToFixDir, pattern);
            HashSet<string> toUse = new HashSet<string>(CommandUtils.GetFileList("", "", filesToUseDir, ""));

            toFix = toFix.Where((f) => toUse.Contains(ParallelXmlHelper.GetParallelFilePathFor(f, filesToUseDir, filesToFixDir)));

            // closure for lexical brevity in loop below
            Action<XElement, string> Fix = (XElement e, string f) =>
            {
                Console.WriteLine(e.Name);
                ParallelXmlHelper.Fix(e, ParallelXmlHelper.ParallelElement(e,
                                                        f,
                                                        filesToFixDir,
                                                        filesToUseDir,
                                                        toUse));
            };

            foreach (var f in toFix)
            {
                bool changed = false;
                XDocument fixie = XDocument.Load(f);

                EventHandler<XObjectChangeEventArgs> SetTrueIfChanged = null;
                SetTrueIfChanged =
                    new EventHandler<XObjectChangeEventArgs>((sender, e) => { fixie.Changed -= SetTrueIfChanged; changed = true; });
                fixie.Changed += SetTrueIfChanged;

                // (1) Fix ype-level summary and remarks:
                XElement typeSummaryToFix = fixie.Element("Type").Element("Docs").Element("summary");
                Fix(typeSummaryToFix, f);

                XElement typeRemarksToFix = fixie.Element("Type").Element("Docs").Element("remarks");
                Fix(typeRemarksToFix, f);

                var members = fixie.Element("Type").Element("Members");
                if (null != members)
                {
                    foreach (XElement m in members.Elements().
                             Where((XElement e) => ParallelXmlHelper.ParallelElement(e,
                                                                    f,
                                                                    filesToFixDir,
                                                                    filesToUseDir,
                                                                    toUse) != null))
                    {
                        // (2) Fix summary, remarks, return values, parameters, and typeparams
                        XElement summary = m.Element("Docs").Element("summary");
                        Fix(summary, f);

                        XElement remarks = m.Element("Docs").Element("remarks");
                        if (null != remarks)
                            Fix(remarks, f);

                        XElement returns = m.Element("Docs").Element("returns");
                        if (null != returns)
                            Fix(returns, f);

                        if (m.Element("Docs").Elements("param").Any())
                        {
                            IEnumerable<XElement> _params = m.Element("Docs").Elements("param");
                            foreach (XElement p in _params)
                            {
                                Fix(p, f);
                            }
                        }

                        if (m.Element("Docs").Elements("typeparam").Any())
                        {
                            IEnumerable<XElement> typeparams = m.Element("Docs").Elements("typeparam");
                            foreach (XElement p in typeparams)
                            {
                                Fix(p, f);
                            }
                        }
                    }
                }

                if (changed)
                {
                    CommandUtils.WriteXDocument(fixie, f);
                }
            }

        }
    }
}
