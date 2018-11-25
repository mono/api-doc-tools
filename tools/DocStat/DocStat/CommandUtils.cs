using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Mono.Options;

namespace DocStat
{
    public static class CommandUtils
    {
        public static List<string> ProcessFileArgs(IEnumerable<string> args,
                                                   ref string rootdir,
                                                   ref string omitlist,
                                                   ref string processlist,
                                                   ref string pattern)
        {
            // Take that, compiler!
            string _rd = "";
            string _ol = "";
            string _pl = "";
            string _pa = "";

            List<string> extras;

            var opt = new OptionSet {

                { "d|dir|directory=",
                                (string d) => _rd =  d },
                // Provide a file that contains a list of files to omit. User may use more than one -o
                { "e|exceptlist=",
                                (m) => _ol = m },
                // List a file that contains a list of files to process
                { "p|processlist=",
                    (f) => _pl = f},
                { "n|namematches=",
                    (n) => _pa = n }
			};
            extras = opt.Parse(args);

            // And that!
            rootdir = String.IsNullOrEmpty(_rd) ? rootdir : _rd;
            omitlist = String.IsNullOrEmpty(_ol) ? omitlist : _ol;
            processlist =String.IsNullOrEmpty(_pl) ? processlist : _pl;
            pattern = String.IsNullOrEmpty(_pa) ? pattern : _pa;

            return extras;
		}

        public static IEnumerable<string> GetFileList(string processListFileName,
                                               string omitListFileName,
                                               string rootDir,
                                               string pattern,
                                               bool recurse = true,
                                               bool skipNsAndIndex = true)
        {
            IEnumerable<string> toProcess = Enumerable.Empty<string>();

			// Build search predicates
            Func<string, bool> fileMatches;
            Func<string, bool> omitFile;

            if (!String.IsNullOrEmpty(pattern))
            {
                Regex fm = new Regex(pattern);
                fileMatches = fm.IsMatch;
            }
            else
            {
                fileMatches = (string s) => true;
            }

            if (String.IsNullOrEmpty(omitListFileName))
			{
				omitFile = (string s) => false;
			}
            else
            {
                if (File.Exists(omitListFileName))
                {
                    IEnumerable<string> toOmit = FileNamesIn(omitListFileName);
                    omitFile = toOmit.Contains;
                }
                else
                    throw new ArgumentException("Omission file does not exist: " + omitListFileName);
            }

            // Process any user-supplied file lists
			if (!String.IsNullOrEmpty(processListFileName))
			{
				if (File.Exists(processListFileName))
				{
                    toProcess = toProcess.Union(
                        FileNamesIn(processListFileName)
                        .Where((p) => fileMatches(Path.GetFileName(p)) && !omitFile(p)));
				}
				else
				{
					throw new FileNotFoundException("Process list file does not exist: " + processListFileName);
				}
			}

            if (String.IsNullOrEmpty(rootDir) && !String.IsNullOrEmpty(processListFileName))
                return toProcess; // they gave us a list only, so they're happy

            if (String.IsNullOrEmpty(rootDir))
                rootDir = Directory.GetCurrentDirectory(); // no list or rootdir, so they want the default

            if (!Directory.Exists(rootDir)) //They gave us something, but it was a boo-boo
                throw new ArgumentException("The provided root directory was required and does not exist: " + rootDir);

            // We have a good root directory, and we want to use it.

            Func<string, bool> isNsOrIndex;

            if (skipNsAndIndex)
            {
                isNsOrIndex = (string fName) => { 
                    string barename = Path.GetFileName(fName); 
                    return barename.StartsWith("ns-") || barename.StartsWith("index");
                };
            }
            else
            {
                isNsOrIndex = (string arg) => false;
            }
            return toProcess.Union(Directory.GetFiles(rootDir,
                                                           "*.xml", 
                                                           recurse ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                            .Where(p => fileMatches(Path.GetFileName(p)) && !omitFile(p) && !isNsOrIndex(p));
			
        }

        public static void ThrowOnFiniteExtras(List<string> extras)
        {
			if (extras.Count > 1)
			{
				StringBuilder s = new StringBuilder("The following options were not recoginzed:\n");
				List<string> sl = new List<string>();
				for (int i = 1; i < extras.Count; i++)
				{
					sl.Add(extras[i]);
				}
				s.Append(String.Join("\n", sl));
				throw new Exception(s.ToString());
			}

		}

		public static XmlDocument ToXmlDocument(XDocument xDocument)
		{
			var xmlDocument = new XmlDocument();
			using (var reader = xDocument.CreateReader())
			{
				xmlDocument.Load(reader);
			}

			var xDeclaration = xDocument.Declaration;
			if (xDeclaration != null)
			{
				var xmlDeclaration = xmlDocument.CreateXmlDeclaration(
					xDeclaration.Version,
					xDeclaration.Encoding,
					xDeclaration.Standalone);
				xmlDocument.InsertBefore(xmlDeclaration, xmlDocument.FirstChild);
			}

			return xmlDocument;
		}

        public static void WriteXDocument(XDocument xdoc, string file)
        {
            if (!File.Exists(file))
                throw new FileNotFoundException("File not found: " + file);
            
            XmlDocument xmldoc = CommandUtils.ToXmlDocument(xdoc);
            TextWriter xdout = new StreamWriter(file);
			// Write back
			XmlTextWriter writer = new XmlTextWriter(xdout)
			{
				Formatting = Formatting.Indented,
				IndentChar = ' ',
				Indentation = 2
			};
			xmldoc.WriteTo(writer);
			xdout.WriteLine();
			xdout.Flush();
        }

        private static IEnumerable<string> FileNamesIn(string fileListPath)
        {
            return File.ReadLines(fileListPath)
                       .Where(s =>
                                  !String.IsNullOrEmpty(s) &&
                                  Uri.IsWellFormedUriString(s, UriKind.Absolute) &&
                                  File.Exists(s)
                             );
        }

        public static string CSVFormatString(int numColumns, int[] order = null)
        {
            if (null != order && order.Length != numColumns)
                throw new ArgumentException(String.Format("Column order array had {0} entries, but {1} columns are needed.",
                                                          order.Length.ToString(),
                                                          numColumns.ToString()));
            Func<int, int> indexFor = null;

            if (null != order)
            {
                indexFor = (int i) => order[i];
            }
            else
            {
                indexFor = (int i) => i;
            }

            string[] cols = new string[numColumns];

            for (int i = 0; i < numColumns; i++)
                cols[i] = "\"{" + indexFor(i) + "}\"";

            return String.Join(",", cols);
        }
    }
}
