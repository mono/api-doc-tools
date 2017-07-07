using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class CompareReportCommand : ApiCommand
    {
        
        public override void Run(IEnumerable<string> args)
        {
            // throw new NotImplementedException();

            string updatedDir = "";
			string omitlist = "";
			string processlist = "";
			string pattern = "";

			List<string> extras = CommandUtils.ProcessFileArgs(args,
															   ref updatedDir,
															   ref omitlist,
															   ref processlist,
															   ref pattern);

            string oldFilesDir = "";
            bool typeOnly = false;
            string reportFile = "";

            var options = new OptionSet {
                {"previous=", (p) => oldFilesDir = p},
                {"typeonly", (t) => typeOnly = t != null},
                {"reportfile=", (r) => reportFile = r}
            };

            extras = options.Parse(extras);

            CommandUtils.ThrowOnFiniteExtras(extras);

			if (String.IsNullOrEmpty(oldFilesDir))
				throw new ArgumentException("You must supply a parallel directory from which to source new content with 'previous='.");

            if (String.IsNullOrEmpty(reportFile) || !reportFile.EndsWith(".csv"))
                throw new ArgumentException("'reportfile=' must be used, and its value must end with '.csv'.");

            string bareReportDir = Path.GetDirectoryName(Path.GetFullPath(reportFile));

            if (!Directory.Exists(bareReportDir))
                throw new ArgumentException(bareReportDir + " does not exist.");

            IEnumerable<string> updated = CommandUtils.GetFileList(processlist, omitlist, updatedDir, pattern);
			HashSet<string> filesToUseAsReference = new HashSet<string>(CommandUtils.GetFileList("", "", oldFilesDir, ""));

			updated =
				updated.Where((f) =>
								 filesToUseAsReference.Contains(ParallelXmlHelper.GetParallelFilePathFor(f,
																										 oldFilesDir,
																										 updatedDir)));


            foreach (string updatedXMLFile in updated)
            {
                // For each member in the updated files, report if the old one isn't present

                string oldXMLFile = ParallelXmlHelper.GetParallelFilePathFor(updatedXMLFile, oldFilesDir, updatedDir);

                bool completelyNew = !File.Exists(oldXMLFile);

                XDocument updatedXDoc = XDocument.Load(updatedXMLFile);
                XDocument oldXDoc = XDocument.Load(oldXMLFile);


            }
		}
    }
}
