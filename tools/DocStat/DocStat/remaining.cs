using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public class RemainingCommand : ApiCommand
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
            string sigil = "To be added.";
            string outputfile = "";

            var opt = new OptionSet
            {
                {"s|sigil=", (s) => sigil = s},
                {"o|output|ofile=", (o) => outputfile = o.EndsWith(@"csv") ? o : outputfile}
            };

            extras = opt.Parse(extras);

            if (String.IsNullOrEmpty(outputfile))
                throw new ArgumentException("You must supply an output file, and it must end with '.csv'");

            CommandUtils.ThrowOnFiniteExtras(extras);

            List<XElement> results = new List<XElement>();

            IEnumerable<string> files = CommandUtils.GetFileList(processlist, omitlist, rootdir, pattern);
            List<string> fileList = files.ToList();
            foreach (string file in fileList)
            {
                AddResults(file, results, sigil);
            }

            WriteResults(results, outputfile);

        }

        internal void AddResults(string file, List<XElement> results, string sigil)
        {
            // Add results
            // <QueryResults>
            //     <Type name="..." filename="....">
            //        <Member name="...">
            //        ....
            //     </Type ... >
            //     <Type>
            //     ....
            // <QueryResults>

            XElement top = XElement.Load(file);
            if (top.Name == "Type")
            {
                // We got a live one!
                IEnumerable<XElement> qres =
                    from member in top.Descendants("Member")
                    where (string)member.Element("Docs").Element("summary") == sigil
                    select member;
                List<XElement> le = new List<XElement>(qres);
                if (le.Any())
                {
                    string typeName = top.Attribute("FullName").Value;
                    XElement t = new XElement("Type");
                    t.Add(new XAttribute("name", typeName));
                    t.Add(new XAttribute("filename", file));
                    // add member name node for each node

                    foreach (XElement m in le)
                    {
                        XElement mres = new XElement("Member");
                        mres.Add(new XAttribute("name", m.Attribute("MemberName").Value));
                        t.Add(mres);
                    }

                    results.Add(t);
                }
            }

        }

        internal void WriteResults(List<XElement> results, string outputFileName)
        {
            if (null == results || results.Count == 0)
            {
                return;
            }

            StreamWriter ofile = new StreamWriter(outputFileName);
            int typeCount = 0;
            int memberCount = 0;
            ofile.WriteLine("Type,Count,File Name,Member");

            string typeFormat = "\"{0}\",\"{1}\",\"{2}\",";
            string memberFormat = ",,,\"{0}\"";
            string rollupFormat = "Types:,\"{0}\",Members:,\"{1}\"";
            foreach (XElement e in results)
            {
                //List<XElement> countable = new List<XElement>(e.Elements());
                ofile.WriteLine(typeFormat,
                                e.Attribute("name").Value,
                                e.Elements().Count(),
                                e.Attribute("filename").Value.Replace("`", "\\`"));
                typeCount++;
                foreach (XElement x in e.Elements())
                {
                    ofile.WriteLine(memberFormat, x.Attribute("name").Value);
                    memberCount++;
                }


            }

            ofile.WriteLine(rollupFormat, typeCount, memberCount);
            ofile.Flush();
            ofile.Close();
        }
    }
}
