﻿﻿using System;
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

            StreamWriter reportStream = new StreamWriter(reportFile);
            reportStream.WriteLine(@"""File Name"",""Type"",""Member""");

			string threeColumnFormatString = @"""{0}"",""{1}"",""{2}""";
			string twoColumnFormatString = @"""{0}"",""{1}""";

			Action<XElement> Write = null;

            Action<XElement> WriteSubsequent = (XElement e) =>
			{
                reportStream.WriteLine(threeColumnFormatString, "", "", e.Attribute("MemberName").Value);
			};

			foreach (string updatedXMLFile in updated)
			{
                XDocument updatedXDoc = XDocument.Load(updatedXMLFile);

                Action<string> WriteFileLine = (string fname) =>
                {
                    reportStream.WriteLine(twoColumnFormatString,
                                           fname,
                                           updatedXDoc.Element("Type").Attribute("FullName").Value);
                    Write = WriteSubsequent;
                };

                Write = (XElement e) =>
                {
					reportStream.WriteLine(twoColumnFormatString,
										   updatedXMLFile,
                                           updatedXDoc.Element("Type").Attribute("FullName").Value);
                    WriteSubsequent(e);
                    Write = WriteSubsequent;
                };
				
                string oldXMLFile = EcmaXmlHelper.GetParallelFilePathFor(updatedXMLFile, oldFilesDir, updatedDir);
                XDocument oldXDoc = File.Exists(oldXMLFile) ? XDocument.Load(oldXMLFile) : null;
                if (null == oldXDoc)
                    WriteFileLine(updatedXMLFile);
				
                foreach (XElement e in EcmaXmlHelper.NewMembers(updatedXDoc, oldXDoc))
                {
                    Write(e);
                }
			}
            reportStream.Flush();
            reportStream.Close();
		}

	}


}
