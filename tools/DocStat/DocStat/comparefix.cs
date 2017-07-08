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
            string filesToUseAsRefDir = "";
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
                {"u|using=", (u) => filesToUseAsRefDir = u},
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
            if (String.IsNullOrEmpty(filesToUseAsRefDir))
                throw new ArgumentException("You must supply a parallel directory from which to source new content with '[u|using]'=.");


            IEnumerable<string> filesToFix = CommandUtils.GetFileList(processlist, omitlist, filesToFixDir, pattern);
            HashSet<string> filesToUseAsReference = new HashSet<string>(CommandUtils.GetFileList("", "", filesToUseAsRefDir, ""));

            filesToFix = 
                filesToFix.Where((f) =>
                                 filesToUseAsReference.Contains(EcmaXmlHelper.GetParallelFilePathFor(f,
                                                                                                         filesToUseAsRefDir,
                                                                                                         filesToFixDir)));


            foreach (var f in filesToFix)
            {
				XDocument currentRefXDoc = EcmaXmlHelper.GetParallelXDocFor(
					EcmaXmlHelper.GetParallelFilePathFor(f, filesToUseAsRefDir, filesToFixDir),
					filesToUseAsReference
				);

                if (null == currentRefXDoc)
                    continue;

                Action<XElement> fix = 
                    (XElement e) => EcmaXmlHelper.Fix(e, EcmaXmlHelper.GetSelectorFor(e)(currentRefXDoc));
                
				bool changed = false;
                XDocument currentXDocToFix = XDocument.Load(f);

                EventHandler<XObjectChangeEventArgs> SetTrueIfChanged = null;
                SetTrueIfChanged =
                    new EventHandler<XObjectChangeEventArgs>((sender, e) => { currentXDocToFix.Changed -= SetTrueIfChanged; changed = true; });
                currentXDocToFix.Changed += SetTrueIfChanged;

                foreach (XElement e in EcmaXmlHelper.ElementsOfInterest(currentXDocToFix))
                    fix(e);

                if (changed)
                {
                    CommandUtils.WriteXDocument(currentXDocToFix, f);
                }
            }

        }
    }
}
