using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class FixSummariesCommand : ApiCommand
    {
        public FixSummariesCommand()
        {
        }

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
            CommandUtils.ThrowOnFiniteExtras(extras);

            foreach (string file in CommandUtils.GetFileList(processlist, omitlist, rootdir, pattern))
            {
                bool changed = false;
                XDocument xdoc = new XDocument(XElement.Load(file));

                XElement memberRoot = xdoc.Element("Type").Element("Members");
                if (memberRoot == null || !memberRoot.Descendants().Any())
                {
                    continue;
                }


                foreach (XElement m in memberRoot.Elements("Member"))
                {
                    XElement summary = m.Element("Docs").Element("summary");

                    if (null == summary)
                    {
                        continue;
                    }

                    if (summary.IsEmpty || (summary.Value.Length == 0 && summary.Descendants().Count() == 0))
                    {
                        summary.Value = "To be added.";
                        changed = true;
                        continue;
                    }

                    IEnumerable<XElement> mistakeParams = summary.Descendants("param");

                    if (mistakeParams.Count() == 0)
                    {
                        continue;
                    }

                    mistakeParams.ToList().ForEach(e => e.Name = "paramref");

                    changed = true;

                }

				if (changed)
				{
					CommandUtils.WriteXDocument(xdoc, file);
				}

            }
			
                
        }
    }
}
