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
            bool nosigil = false;

            var options = new OptionSet {
                {"previous=", (p) => oldFilesDir = p},
                {"typeonly", (t) => typeOnly = t != null},
                {"reportfile=", (r) => reportFile = r},
                { "no-check-TBA", (t) => nosigil = t != null }
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

            reportStream.WriteLine(String.Format(CommandUtils.CSVFormatString(3), "File Name", "Type", "Member"));

			Action<XElement> Write = null;

            Action<XElement> WriteSubsequent = (XElement e) =>
			{
                reportStream.WriteLine(CommandUtils.CSVFormatString(3), "", "", e.Attribute("MemberName").Value);
			};

            Func<XElement, bool> hasSigil = null;

            if (nosigil)
            {
                hasSigil = (XElement e) => true;
            }
            else
            {
                hasSigil = (XElement e) => e.Element("Docs").Element("summary").Value == "To be added.";
            }

			foreach (string updatedXMLFile in updated)
			{
                XDocument updatedXDoc = XDocument.Load(updatedXMLFile);

                Action<string> WriteFileLine = (string fname) =>
                {
                    reportStream.WriteLine(CommandUtils.CSVFormatString(2),
                                           fname,
                                           updatedXDoc.Element("Type").Attribute("FullName").Value);
                    Write = WriteSubsequent;
                };

                Write = (XElement e) =>
                {
					reportStream.WriteLine(CommandUtils.CSVFormatString(2),
										   updatedXMLFile,
                                           updatedXDoc.Element("Type").Attribute("FullName").Value);
                    WriteSubsequent(e);
                    Write = WriteSubsequent;
                };
				
                string oldXMLFile = EcmaXmlHelper.GetParallelFilePathFor(updatedXMLFile, oldFilesDir, updatedDir);
                XDocument oldXDoc = File.Exists(oldXMLFile) ? XDocument.Load(oldXMLFile) : null;
                if (null == oldXDoc)
                    WriteFileLine(updatedXMLFile);

                IEnumerable<XElement> newMembers = EcmaXmlHelper.NewMembers(updatedXDoc, oldXDoc);
                if (null != newMembers && newMembers.Count() > 0)
                {
                    foreach (XElement e in newMembers.Where((f) => hasSigil(f)))
                    {
                        Write(e);
                    }
                }
			}
            reportStream.Flush();
            reportStream.Close();
		}

	}


}
