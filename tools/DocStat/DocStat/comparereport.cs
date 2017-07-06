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
            throw new NotImplementedException();

            string updatedDir = "";
			string omitlist = "";
			string processlist = "";
			string pattern = "";

			List<string> extras = CommandUtils.ProcessFileArgs(args,
															   ref updatedDir,
															   ref omitlist,
															   ref processlist,
															   ref pattern);

            string oldFiles = "";
            bool typeOnly = false;


			if (String.IsNullOrEmpty(oldFiles))
				throw new ArgumentException("You must supply a parallel directory from which to source new content with '[u|using]'=.");


            IEnumerable<string> updated = CommandUtils.GetFileList(processlist, omitlist, updatedDir, pattern);
			HashSet<string> filesToUseAsReference = new HashSet<string>(CommandUtils.GetFileList("", "", oldFiles, ""));

			updated =
				updated.Where((f) =>
								 filesToUseAsReference.Contains(ParallelXmlHelper.GetParallelFilePathFor(f,
																										 oldFiles,
																										 updatedDir)));



			// For each member in the updated files, report if the old one isn't present

		}
    }
}
