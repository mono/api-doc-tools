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

            reportStream.WriteLine(String.Format(CommandUtils.CSVFormatString(5), "File Name", "Type", "Member","Need file summary", "Need member summary"));

			Func<XElement, bool> hasSigil = null;

            if (nosigil)
            {
                hasSigil = (XElement e) => true;
            }
            else
            {
                hasSigil = (XElement e) => e.Element("Docs").Element("summary").Value == "To be added.";
            }

            //Func<XElement, bool> needSummary = (XElement e) => e.Element("Docs").Element("summary").Value == "To be added.";

            Func<XElement, string> MemberLine = (XElement e) => {
				return string.Format(
                    CommandUtils.CSVFormatString(5), 
                    "",
                    "",
                    e.Attribute("MemberName").Value,
                    "",
                    hasSigil(e) ? "y" : "n");
            };



            List<string> toWrite = new List<string>();
			foreach (string updatedXMLFile in updated)
			{
                
                bool fileLineAdded = false;

                XDocument updatedXDoc = XDocument.Load(updatedXMLFile);

                Func<string> FileLine = () =>
                {
                    return string.Format(
                        CommandUtils.CSVFormatString(5),
                        updatedXMLFile,
                        updatedXDoc.Element("Type").Attribute("FullName").Value,
                        "",
                        hasSigil(updatedXDoc.Element("Type")) ? "y" : "n",
                        "");
                };

				
                string oldXMLFile = EcmaXmlHelper.GetParallelFilePathFor(updatedXMLFile, oldFilesDir, updatedDir);
                XDocument oldXDoc = File.Exists(oldXMLFile) ? XDocument.Load(oldXMLFile) : null;
                if (null == oldXDoc && hasSigil(updatedXDoc.Element("Type")))
                {
                    toWrite.Add(FileLine());
                    fileLineAdded = true;
                }

                IEnumerable<XElement> newMembers = EcmaXmlHelper.NewMembers(updatedXDoc, oldXDoc);
                if (null != newMembers && newMembers.Where((f) => hasSigil(f)).Any())
                {
                    if (!fileLineAdded)
                        toWrite.Add(FileLine());

                    foreach (XElement e in newMembers.Where((f) => hasSigil(f)))
                    {
                        toWrite.Add(MemberLine(e));
                    }
                }

                // If toWrite isn't empty, write all lines
                if (toWrite.Any())
                {
                    foreach (string s in toWrite)
                        reportStream.WriteLine(s);
                }
                toWrite.Clear();
			}
            reportStream.Flush();
            reportStream.Close();

		}

	}


}
