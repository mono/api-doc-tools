using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using Mono.Cecil;
using Mono.Documentation.Updater;
using Mono.Documentation.Updater.Frameworks;
using Mono.Documentation.Updater.Statistics;
using Mono.Documentation.Util;
using Mono.Options;

using MyXmlNodeList = System.Collections.Generic.List<System.Xml.XmlNode>;
using StringList = System.Collections.Generic.List<string>;
using StringToXmlNodeMap = System.Collections.Generic.Dictionary<string, System.Xml.XmlNode>;

namespace Mono.Documentation
{
    class MDocUpdater : MDocCommand
    {
        string srcPath;
        List<AssemblySet> assemblies = new List<AssemblySet> ();
        StringList globalSearchPaths = new StringList ();

        string apistyle = string.Empty;
        bool isClassicRun;

        bool delete;
        bool show_exceptions;
        bool no_assembly_versions, ignore_missing_types;
        ExceptionLocations? exceptions;

        internal int additions = 0, deletions = 0;

        List<DocumentationImporter> importers = new List<DocumentationImporter> ();

        DocumentationEnumerator docEnum;

        string since;

        static readonly MemberFormatter docTypeFormatter = new DocTypeMemberFormatter ();
        static readonly MemberFormatter filenameFormatter = new FileNameMemberFormatter ();

        static MemberFormatter[] typeFormatters = new MemberFormatter[]{
        new CSharpMemberFormatter (),
        new ILMemberFormatter (),
    };

        static MemberFormatter[] memberFormatters = new MemberFormatter[]{
        new CSharpFullMemberFormatter (),
        new ILFullMemberFormatter (),
    };

        internal static readonly MemberFormatter slashdocFormatter = new SlashDocMemberFormatter ();

        MyXmlNodeList extensionMethods = new MyXmlNodeList ();

        public static string droppedNamespace = string.Empty;

        private HashSet<string> memberSet;

        public static bool HasDroppedNamespace (TypeDefinition forType)
        {
            return HasDroppedNamespace (forType.Module);
        }

        public static bool HasDroppedNamespace (MemberReference forMember)
        {
            return HasDroppedNamespace (forMember.Module);
        }

        public static bool HasDroppedNamespace (AssemblyDefinition forAssembly)
        {
            return HasDroppedNamespace (forAssembly.MainModule);
        }

        public static bool HasDroppedNamespace (ModuleDefinition forModule)
        {
            return !string.IsNullOrWhiteSpace (droppedNamespace) && droppedAssemblies.Any (da => da == forModule.Name);
        }

        public static bool HasDroppedAnyNamespace ()
        {
            return !string.IsNullOrWhiteSpace (droppedNamespace);
        }

        /// <summary>Logic flag to signify that we should list assemblies at the method level, since there are multiple
        /// assemblies for a given type/method.</summary>
        public bool IsMultiAssembly
        {
            get
            {
                return apistyle == "classic" || apistyle == "unified" || !string.IsNullOrWhiteSpace (FrameworksPath);
            }
        }

        /// <summary>Path which contains multiple folders with assemblies. Each folder contained will represent one framework.</summary>
        string FrameworksPath = string.Empty;
        FrameworkIndex frameworks;
        FrameworkIndex frameworksCache;
        IEnumerable<XDocument> oldFrameworkXmls;

        private StatisticsCollector statisticsCollector = new StatisticsCollector();

        static List<string> droppedAssemblies = new List<string> ();

        public string PreserveTag { get; set; }
        public bool DisableSearchDirectoryRecurse = false;
        private bool statisticsEnabled = false;
        private string statisticsFilePath;
        public static MDocUpdater Instance { get; private set; }
        public static bool SwitchingToMagicTypes { get; private set; }

        public override void Run (IEnumerable<string> args)
        {
            Instance = this;
            show_exceptions = DebugOutput;
            var types = new List<string> ();
            var p = new OptionSet () {
            { "delete",
                "Delete removed members from the XML files.",
                v => delete = v != null },
            { "exceptions:",
              "Document potential exceptions that members can generate.  {SOURCES} " +
                "is a comma-separated list of:\n" +
                "  asm      Method calls in same assembly\n" +
                "  depasm   Method calls in dependent assemblies\n" +
                "  all      Record all possible exceptions\n" +
                "  added    Modifier; only create <exception/>s\n" +
                "             for NEW types/members\n" +
                "If nothing is specified, then only exceptions from the member will " +
                "be listed.",
                v =>
                {
                    exceptions = ParseExceptionLocations(v);
                } },
            { "f=",
                "Specify a {FLAG} to alter behavior.  See later -f* options for available flags.",
                v => {
                    switch (v) {
                        case "ignore-missing-types":
                            ignore_missing_types = true;
                            break;
                        case "no-assembly-versions":
                            no_assembly_versions = true;
                            break;
                        default:
                            throw new Exception ("Unsupported flag `" + v + "'.");
                    }
                } },
            { "fignore-missing-types",
                "Do not report an error if a --type=TYPE type\nwas not found.",
                v => ignore_missing_types = v != null },
            { "fno-assembly-versions",
                "Do not generate //AssemblyVersion elements.",
                v => no_assembly_versions = v != null },
            { "i|import=",
                "Import documentation from {FILE}.",
                v => AddImporter (v) },
            { "L|lib=",
                "Check for assembly references in {DIRECTORY}.",
                v => globalSearchPaths.Add (v) },
            { "library=",
                "Ignored for compatibility with update-ecma-xml.",
                v => {} },
            { "o|out=",
                "Root {DIRECTORY} to generate/update documentation.",
                v => srcPath = v },
            { "r=",
                "Search for dependent assemblies in the directory containing {ASSEMBLY}.\n" +
                "(Equivalent to '-L `dirname ASSEMBLY`'.)",
                v => globalSearchPaths.Add (Path.GetDirectoryName (v)) },
            { "since=",
                "Manually specify the assembly {VERSION} that new members were added in.",
                v => since = v },
            { "type=",
              "Only update documentation for {TYPE}.",
                v => types.Add (v) },
            { "dropns=",
              "When processing assembly {ASSEMBLY}, strip off leading namespace {PREFIX}:\n" +
              "  e.g. --dropns ASSEMBLY=PREFIX",
              v => {
                var parts = v.Split ('=');
                if (parts.Length != 2) { Console.Error.WriteLine ("Invalid dropns input"); return; }
                var assembly = Path.GetFileName (parts [0].Trim ());
                var prefix = parts [1].Trim();
                droppedAssemblies.Add (assembly);
                droppedNamespace = prefix;
            } },
            { "ntypes",
                "If the new assembly is switching to 'magic types', then this switch should be defined.",
                v => SwitchingToMagicTypes = true },
            { "preserve",
                "Do not delete members that don't exist in the assembly, but rather mark them as preserved.",
                v => PreserveTag = "true" },
            { "api-style=",
                "Denotes the apistyle. Currently, only `classic` and `unified` are supported. `classic` set of assemblies should be run first, immediately followed by 'unified' assemblies with the `dropns` parameter.",
                v => apistyle = v.ToLowerInvariant () },
            { "fx|frameworks=",
                "Configuration XML file, that points to directories which contain libraries that span multiple frameworks.",
                v => FrameworksPath = v },
            { "use-docid",
                "Add 'DocId' to the list of type and member signatures",
                v =>
                {
                    typeFormatters = typeFormatters.Union (new MemberFormatter[] { new DocIdFormatter () }).ToArray ();
                    memberFormatters = memberFormatters.Union (new MemberFormatter[] { new DocIdFormatter () }).ToArray ();
                } },
            { "disable-searchdir-recurse",
                "Default behavior for adding search directories ('-L') is to recurse them and search in all subdirectories. This disables that",
                v => DisableSearchDirectoryRecurse = true },
            {
                "statistics=",
                "Save statistics to the specified file",
                v =>
                {
                    statisticsEnabled = true;
                    if (!string.IsNullOrEmpty(v))
                        statisticsFilePath = v;
                } },
        };
            var assemblyPaths = Parse (p, args, "update",
                    "[OPTIONS]+ ASSEMBLIES",
                    "Create or update documentation from ASSEMBLIES.");

            if (!string.IsNullOrWhiteSpace (FrameworksPath))
            {
                var configPath = FrameworksPath;
                var frameworksDir = FrameworksPath;
                if (!configPath.EndsWith ("frameworks.xml", StringComparison.InvariantCultureIgnoreCase))
                    configPath = Path.Combine (configPath, "frameworks.xml");
                else
                    frameworksDir = Path.GetDirectoryName (configPath);

                var fxconfig = XDocument.Load (configPath);
                var fxd = fxconfig.Root
                                  .Elements ("Framework")
                                  .Select (f => new
                                  {
                                      Name = f.Attribute ("Name").Value,
                                      Path = Path.Combine (frameworksDir, f.Attribute ("Source").Value),
                                      SearchPaths = f.Elements ("assemblySearchPath")
                                                   .Select (a => Path.Combine (frameworksDir, a.Value))
                                                   .ToArray (),
                                      Imports = f.Elements ("import")
                                                   .Select (a => Path.Combine (frameworksDir, a.Value))
                                                   .ToArray ()
                                  })
                                  .Where (f => Directory.Exists (f.Path));

                oldFrameworkXmls = fxconfig.Root
                                               .Elements("Framework")
                                               .Select(f => new
                                               {
                                                   Name = f.Attribute("Name").Value,
                                                   Source = f.Attribute("Source").Value,
                                                   XmlPath = Path.Combine(srcPath, "FrameworksIndex", f.Attribute("Source").Value + ".xml"),
                                               })
                                               .Where(f => File.Exists(f.XmlPath))
                                               .Select(f => XDocument.Load(f.XmlPath));

                Func<string, string, IEnumerable<string>> getFiles = (string path, string filters) =>
                {
                    return filters
                        .Split ('|')
                        .SelectMany (v => Directory.GetFiles (path, v));
                };

                var sets = fxd.Select (d => new AssemblySet (
                    d.Name,
                    getFiles (d.Path, "*.dll|*.exe|*.winmd"),
                    this.globalSearchPaths.Union (d.SearchPaths),
                    d.Imports
                ));
                this.assemblies.AddRange (sets);
                assemblyPaths.AddRange (sets.SelectMany (s => s.AssemblyPaths));

                // Create a cache of all frameworks, so we can look up 
                // members that may exist only other frameworks before deleting them
                Console.Write ("Creating frameworks cache: ");
                FrameworkIndex cacheIndex = new FrameworkIndex (FrameworksPath);
                string[] prefixesToAvoid = { "get_", "set_", "add_", "remove_", "raise_" };
                foreach (var assemblySet in this.assemblies)
                {
                    using (assemblySet)
                    {
                        Console.Write (".");
                        foreach (var assembly in assemblySet.Assemblies)
                        {
                            var a = cacheIndex.StartProcessingAssembly (assembly, assemblySet.Importers);
                            foreach (var type in assembly.GetTypes ())
                            {
                                var t = a.ProcessType (type);
                                foreach (var member in type.GetMembers ().Where (m => !prefixesToAvoid.Any (pre => m.Name.StartsWith (pre, StringComparison.Ordinal))))
                                    t.ProcessMember (member);
                            }
                        }
                    }
                }
                Console.WriteLine ($"{Environment.NewLine}done caching.");
                this.frameworksCache = cacheIndex;
            }
            else
            {
                this.assemblies.Add (new AssemblySet ("Default", assemblyPaths, this.globalSearchPaths, null));
            }

            if (assemblyPaths == null)
                return;
            if (assemblyPaths.Count == 0)
                Error ("No assemblies specified.");

            if (!DisableSearchDirectoryRecurse)
            {
                // unless it's been explicitly disabled, let's
                // add all of the subdirectories to the resolver
                // search paths.
                foreach (var assemblySet in this.assemblies)
                    assemblySet.RecurseSearchDirectories ();
            }

            // validation for the api-style parameter
            if (apistyle == "classic")
                isClassicRun = true;
            else if (apistyle == "unified")
            {
                if (!droppedAssemblies.Any ())
                    Error ("api-style 'unified' must also supply the 'dropns' parameter with at least one assembly and dropped namespace.");
            }
            else if (!string.IsNullOrWhiteSpace (apistyle))
                Error ("api-style '{0}' is not currently supported", apistyle);

            // PARSE BASIC OPTIONS AND LOAD THE ASSEMBLY TO DOCUMENT

            if (srcPath == null)
                throw new InvalidOperationException ("The --out option is required.");

            docEnum = docEnum ?? new DocumentationEnumerator ();

            // PERFORM THE UPDATES
            frameworks = new FrameworkIndex (FrameworksPath);

            if (types.Count > 0)
            {
                types.Sort ();
                DoUpdateTypes (srcPath, types, srcPath);
            }
            else
                DoUpdateAssemblies (srcPath, srcPath);

            if (!string.IsNullOrWhiteSpace (FrameworksPath))
                frameworks.WriteToDisk (srcPath);

            if (statisticsEnabled)
            {
                try
                {
                    StatisticsSaver.Save(statisticsCollector, statisticsFilePath);
                }
                catch (Exception exception)
                {
                    Warning($"Unable to save statistics file: {exception.Message}");
                }
            }

            Console.WriteLine ("Members Added: {0}, Members Deleted: {1}", additions, deletions);
        }

        public static bool IsInAssemblies (string name)
        {
            return Instance?.assemblies != null ? Instance.assemblies.Any (a => a.Contains (name)) : true;
        }

        void AddImporter (string path)
        {
            var importer = GetImporter (path, supportsEcmaDoc: true);
            if (importer != null)
                importers.Add (importer);
        }

        internal DocumentationImporter GetImporter (string path, bool supportsEcmaDoc)
        {
            try
            {
                XmlReader r = new XmlTextReader (path);
                if (r.Read ())
                {
                    while (r.NodeType != XmlNodeType.Element)
                    {
                        if (!r.Read ())
                            Error ("Unable to read XML file: {0}.", path);
                    }
                    if (r.LocalName == "doc")
                    {
                        return new MsxdocDocumentationImporter (path);
                    }
                    else if (r.LocalName == "Libraries")
                    {
                        if (!supportsEcmaDoc)
                            throw new NotSupportedException ($"Ecma documentation not supported in this mode: {path}");

                        var ecmadocs = new XmlTextReader (path);
                        docEnum = new EcmaDocumentationEnumerator (this, ecmadocs);
                        return new EcmaDocumentationImporter (ecmadocs);
                    }
                    else
                        Error ("Unsupported XML format within {0}.", path);
                }
                r.Close ();
            }
            catch (Exception e)
            {
                Environment.ExitCode = 1;
                Error ("Could not load XML file: {0}.", e.Message);
            }
            return null;
        }

        static ExceptionLocations ParseExceptionLocations (string s)
        {
            ExceptionLocations loc = ExceptionLocations.Member;
            if (s == null)
                return loc;
            foreach (var type in s.Split (','))
            {
                switch (type)
                {
                    case "added": loc |= ExceptionLocations.AddedMembers; break;
                    case "all": loc |= ExceptionLocations.Assembly | ExceptionLocations.DependentAssemblies; break;
                    case "asm": loc |= ExceptionLocations.Assembly; break;
                    case "depasm": loc |= ExceptionLocations.DependentAssemblies; break;
                    default: throw new NotSupportedException ("Unsupported --exceptions value: " + type);
                }
            }
            return loc;
        }

        internal void Warning (string format, params object[] args)
        {
            Message (TraceLevel.Warning, "mdoc: " + format, args);
        }

        internal AssemblyDefinition LoadAssembly (string name, IAssemblyResolver assemblyResolver)
        {
            AssemblyDefinition assembly = null;
            try
            {
                assembly = AssemblyDefinition.ReadAssembly (name, new ReaderParameters { AssemblyResolver = assemblyResolver });
            }
            catch (Exception ex)
            {
                Warning ($"Unable to load assembly '{name}': {ex.Message}");
            }

            return assembly;
        }

        private static void WriteXml (XmlElement element, System.IO.TextWriter output)
        {
            OrderTypeAttributes (element);
            XmlTextWriter writer = new XmlTextWriter (output);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 2;
            writer.IndentChar = ' ';
            element.WriteTo (writer);
            output.WriteLine ();
        }

        private static void WriteFile (string filename, FileMode mode, Action<TextWriter> action)
        {
            Action<string> creator = file =>
            {
                using (var writer = OpenWrite (file, mode))
                    action (writer);
            };

            MdocFile.UpdateFile (filename, creator);
        }

        private static void OrderTypeAttributes (XmlElement e)
        {
            foreach (XmlElement type in e.SelectNodes ("//Type"))
            {
                OrderTypeAttributes (type.Attributes);
            }
        }

        static readonly string[] TypeAttributeOrder = {
        "Name", "FullName", "FullNameSP", "Maintainer"
    };

        private static void OrderTypeAttributes (XmlAttributeCollection c)
        {
            XmlAttribute[] attrs = new XmlAttribute[TypeAttributeOrder.Length];
            for (int i = 0; i < c.Count; ++i)
            {
                XmlAttribute a = c[i];
                for (int j = 0; j < TypeAttributeOrder.Length; ++j)
                {
                    if (a.Name == TypeAttributeOrder[j])
                    {
                        attrs[j] = a;
                        break;
                    }
                }
            }
            for (int i = attrs.Length - 1; i >= 0; --i)
            {
                XmlAttribute n = attrs[i];
                if (n == null)
                    continue;
                XmlAttribute r = null;
                for (int j = i + 1; j < attrs.Length; ++j)
                {
                    if (attrs[j] != null)
                    {
                        r = attrs[j];
                        break;
                    }
                }
                if (r == null)
                    continue;
                if (c[n.Name] != null)
                {
                    c.RemoveNamedItem (n.Name);
                    c.InsertBefore (n, r);
                }
            }
        }

        private XmlDocument CreateIndexStub ()
        {
            XmlDocument index = new XmlDocument ();

            XmlElement index_root = index.CreateElement ("Overview");
            index.AppendChild (index_root);

            if (assemblies.Count == 0)
                throw new Exception ("No assembly");

            XmlElement index_assemblies = index.CreateElement ("Assemblies");
            index_root.AppendChild (index_assemblies);

            XmlElement index_remarks = index.CreateElement ("Remarks");
            index_remarks.InnerText = "To be added.";
            index_root.AppendChild (index_remarks);

            XmlElement index_copyright = index.CreateElement ("Copyright");
            index_copyright.InnerText = "To be added.";
            index_root.AppendChild (index_copyright);

            XmlElement index_types = index.CreateElement ("Types");
            index_root.AppendChild (index_types);

            return index;
        }

        private static void WriteNamespaceStub (string ns, string outdir)
        {
            XmlDocument index = new XmlDocument ();

            XmlElement index_root = index.CreateElement ("Namespace");
            index.AppendChild (index_root);

            index_root.SetAttribute ("Name", ns);

            XmlElement index_docs = index.CreateElement ("Docs");
            index_root.AppendChild (index_docs);

            XmlElement index_summary = index.CreateElement ("summary");
            index_summary.InnerText = "To be added.";
            index_docs.AppendChild (index_summary);

            XmlElement index_remarks = index.CreateElement ("remarks");
            index_remarks.InnerText = "To be added.";
            index_docs.AppendChild (index_remarks);

            WriteFile (outdir + "/ns-" + ns + ".xml", FileMode.CreateNew,
                    writer => WriteXml (index.DocumentElement, writer));
        }

        public void DoUpdateTypes (string basepath, List<string> typenames, string dest)
        {
            var index = CreateIndexForTypes (dest);

            var found = new HashSet<string> ();
            foreach (var assemblySet in this.assemblies)
            {
                using (assemblySet)
                {
                    foreach (AssemblyDefinition assembly in assemblySet.Assemblies)
                    {
                        var typeSet = new HashSet<string> ();
                        var namespacesSet = new HashSet<string> ();
                        memberSet = new HashSet<string> ();

                        var frameworkEntry = frameworks.StartProcessingAssembly (assembly, assemblySet.Importers);

                        foreach (TypeDefinition type in docEnum.GetDocumentationTypes (assembly, typenames))
                        {
                            var typeEntry = frameworkEntry.ProcessType (type);

                            string relpath = DoUpdateType (type, typeEntry, basepath, dest);
                            if (relpath == null)
                                continue;

                            found.Add (type.FullName);

                            if (index == null)
                                continue;

                            index.Add (assembly);
                            index.Add (type);

                            namespacesSet.Add (type.Namespace);
                            typeSet.Add (type.FullName);
                        }

                        statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Types, StatisticsMetrics.Total, typeSet.Count);
                        statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Namespaces, StatisticsMetrics.Total, namespacesSet.Count);
                        statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Members, StatisticsMetrics.Total, memberSet.Count);
                    }
                }
            }

            if (index != null)
                index.Write ();


            if (ignore_missing_types)
                return;

            var notFound = from n in typenames where !found.Contains (n) select n;
            if (notFound.Any ())
                throw new InvalidOperationException ("Type(s) not found: " + string.Join (", ", notFound.ToArray ()));
        }

        class IndexForTypes
        {

            MDocUpdater app;
            string indexFile;

            XmlDocument index;
            XmlElement index_types;
            XmlElement index_assemblies;

            public IndexForTypes (MDocUpdater app, string indexFile, XmlDocument index)
            {
                this.app = app;
                this.indexFile = indexFile;
                this.index = index;

                index_types = WriteElement (index.DocumentElement, "Types");
                index_assemblies = WriteElement (index.DocumentElement, "Assemblies");
            }

            public void Add (AssemblyDefinition assembly)
            {
                if (index_assemblies.SelectSingleNode ("Assembly[@Name='" + assembly.Name.Name + "']") != null)
                    return;

                app.AddIndexAssembly (assembly, index_assemblies);
            }

            public void Add (TypeDefinition type)
            {
                app.AddIndexType (type, index_types);
            }

            public void Write ()
            {
                SortIndexEntries (index_types);
                WriteFile (indexFile, FileMode.Create,
                        writer => WriteXml (index.DocumentElement, writer));
            }
        }

        IndexForTypes CreateIndexForTypes (string dest)
        {
            string indexFile = Path.Combine (dest, "index.xml");
            if (File.Exists (indexFile))
                return null;
            return new IndexForTypes (this, indexFile, CreateIndexStub ());
        }

        /// <summary>Constructs the presumed path to the type's documentation file</summary>
        /// <returns><c>true</c>, if the type file was found, <c>false</c> otherwise.</returns>
        /// <param name="result">A typle that contains 1) the 'reltypefile', 2) the 'typefile', and 3) the file info</param>
        bool TryFindTypeFile (string nsname, string typename, string basepath, out Tuple<string, string, FileInfo> result)
        {
            string reltypefile = DocUtils.PathCombine (nsname, typename + ".xml");
            string typefile = Path.Combine (basepath, reltypefile);
            System.IO.FileInfo file = new System.IO.FileInfo (typefile);

            result = new Tuple<string, string, FileInfo> (reltypefile, typefile, file);

            return file.Exists;
        }

        public string DoUpdateType (TypeDefinition type, FrameworkTypeEntry typeEntry, string basepath, string dest)
        {
            if (type.Namespace == null)
                Warning ("warning: The type `{0}' is in the root namespace.  This may cause problems with display within monodoc.",
                        type.FullName);
            if (!IsPublic (type))
                return null;

            // Must get the A+B form of the type name.
            string typename = GetTypeFileName (type);
            string nsname = DocUtils.GetNamespace (type);

            // Find the file, if it exists
            string[] searchLocations = new string[] {
                nsname
            };

            if (MDocUpdater.HasDroppedNamespace (type))
            {
                // If dropping namespace, types may have moved into a couple of different places.
                var newSearchLocations = searchLocations.Union (new string[] {
                string.Format ("{0}.{1}", droppedNamespace, nsname),
                nsname.Replace (droppedNamespace + ".", string.Empty),
                MDocUpdater.droppedNamespace
            });

                searchLocations = newSearchLocations.ToArray ();
            }

            string reltypefile = "", typefile = "";
            System.IO.FileInfo file = null;

            foreach (var f in searchLocations)
            {
                Tuple<string, string, FileInfo> result;
                bool fileExists = TryFindTypeFile (f, typename, basepath, out result);

                if (fileExists)
                {
                    reltypefile = result.Item1;
                    typefile = result.Item2;
                    file = result.Item3;

                    break;
                }
            }

            if (file == null || !file.Exists)
            {
                // we were not able to find a file, let's use the original type informatio.
                // so that we create the stub in the right place.
                Tuple<string, string, FileInfo> result;
                TryFindTypeFile (nsname, typename, basepath, out result);

                reltypefile = result.Item1;
                typefile = result.Item2;
                file = result.Item3;
            }

            string output = null;
            if (dest == null)
            {
                output = typefile;
            }
            else if (dest == "-")
            {
                output = null;
            }
            else
            {
                output = Path.Combine (dest, reltypefile);
            }

            if (file != null && file.Exists)
            {
                // Update
                XmlDocument basefile = new XmlDocument ();
                try
                {
                    basefile.Load (typefile);
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException ("Error loading " + typefile + ": " + e.Message, e);
                }

                DoUpdateType2 ("Updating", basefile, type, typeEntry, output, false);
            }
            else
            {
                // Stub
                XmlElement td = StubType (type, output, typeEntry.Framework.Importers);
                if (td == null)
                    return null;
            }
            return reltypefile;
        }

        private static string GetTypeFileName (TypeReference type)
        {
            return filenameFormatter.GetName (type);
        }

        public static string GetTypeFileName (string typename)
        {
            StringBuilder filename = new StringBuilder (typename.Length);
            int numArgs = 0;
            int numLt = 0;
            bool copy = true;
            for (int i = 0; i < typename.Length; ++i)
            {
                char c = typename[i];
                switch (c)
                {
                    case '<':
                        copy = false;
                        ++numLt;
                        break;
                    case '>':
                        --numLt;
                        if (numLt == 0)
                        {
                            filename.Append ('`').Append ((numArgs + 1).ToString ());
                            numArgs = 0;
                            copy = true;
                        }
                        break;
                    case ',':
                        if (numLt == 1)
                            ++numArgs;
                        break;
                    default:
                        if (copy)
                            filename.Append (c);
                        break;
                }
            }
            return filename.ToString ();
        }

        private void AddIndexAssembly (AssemblyDefinition assembly, XmlElement parent)
        {
            XmlElement index_assembly = null;
            if (IsMultiAssembly)
                index_assembly = (XmlElement)parent.SelectSingleNode ("Assembly[@Name='" + assembly.Name.Name + "']");

            if (index_assembly == null)
                index_assembly = parent.OwnerDocument.CreateElement ("Assembly");

            index_assembly.SetAttribute ("Name", assembly.Name.Name);
            index_assembly.SetAttribute ("Version", assembly.Name.Version.ToString ());

            AssemblyNameDefinition name = assembly.Name;
            if (name.HasPublicKey)
            {
                XmlElement pubkey = parent.OwnerDocument.CreateElement ("AssemblyPublicKey");
                var key = new StringBuilder (name.PublicKey.Length * 3 + 2);
                key.Append ("[");
                foreach (byte b in name.PublicKey)
                    key.AppendFormat ("{0,2:x2} ", b);
                key.Append ("]");
                pubkey.InnerText = key.ToString ();
                index_assembly.AppendChild (pubkey);
            }

            if (!string.IsNullOrEmpty (name.Culture))
            {
                XmlElement culture = parent.OwnerDocument.CreateElement ("AssemblyCulture");
                culture.InnerText = name.Culture;
                index_assembly.AppendChild (culture);
            }

            MakeAttributes (index_assembly, GetCustomAttributes (assembly.CustomAttributes, ""));
            parent.AppendChild (index_assembly);
        }

        private void AddIndexType (TypeDefinition type, XmlElement index_types)
        {
            string typename = GetTypeFileName (type);

            // Add namespace and type nodes into the index file as needed
            string ns = DocUtils.GetNamespace (type);
            XmlElement nsnode = (XmlElement)index_types.SelectSingleNode ("Namespace[@Name='" + ns + "']");
            if (nsnode == null)
            {
                nsnode = index_types.OwnerDocument.CreateElement ("Namespace");
                nsnode.SetAttribute ("Name", ns);
                index_types.AppendChild (nsnode);
            }
            string doc_typename = GetDocTypeName (type);
            XmlElement typenode = (XmlElement)nsnode.SelectSingleNode ("Type[@Name='" + typename + "']");
            if (typenode == null)
            {
                typenode = index_types.OwnerDocument.CreateElement ("Type");
                typenode.SetAttribute ("Name", typename);
                nsnode.AppendChild (typenode);
            }
            if (typename != doc_typename)
                typenode.SetAttribute ("DisplayName", doc_typename);
            else
                typenode.RemoveAttribute ("DisplayName");

            typenode.SetAttribute ("Kind", GetTypeKind (type));
        }

        private void DoUpdateAssemblies (string source, string dest)
        {
            string indexfile = dest + "/index.xml";
            XmlDocument index;
            if (System.IO.File.Exists (indexfile))
            {
                index = new XmlDocument ();
                index.Load (indexfile);

                // Format change
                ClearElement (index.DocumentElement, "Assembly");
                ClearElement (index.DocumentElement, "Attributes");
            }
            else
            {
                index = CreateIndexStub ();
            }

            XmlElement index_types = WriteElement (index.DocumentElement, "Types");
            XmlElement index_assemblies = WriteElement (index.DocumentElement, "Assemblies");
            if (!IsMultiAssembly)
                index_assemblies.RemoveAll ();


            HashSet<string> goodfiles = new HashSet<string> (StringComparer.OrdinalIgnoreCase);

            int processedAssemblyCount = 0;
            foreach (var assemblySet in assemblies)
            {
                using (assemblySet)
                {
                    foreach (AssemblyDefinition assm in assemblySet.Assemblies)
                    {
                        AddIndexAssembly (assm, index_assemblies);
                        DoUpdateAssembly (assemblySet, assm, index_types, source, dest, goodfiles);
                        processedAssemblyCount++;
                    }
                }
            }

            string defaultTitle = "Untitled";
            if (processedAssemblyCount == 1)
                defaultTitle = assemblies[0].Assemblies.First ().Name.Name;
            WriteElementInitialText (index.DocumentElement, "Title", defaultTitle);

            SortIndexEntries (index_types);

            CleanupFiles (dest, goodfiles);
            CleanupIndexTypes (index_types, goodfiles);
            CleanupExtensions (index_types);

            WriteFile (indexfile, FileMode.Create,
                    writer => WriteXml (index.DocumentElement, writer));
        }

        private static char[] InvalidFilenameChars = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

        private void DoUpdateAssembly (AssemblySet assemblySet, AssemblyDefinition assembly, XmlElement index_types, string source, string dest, HashSet<string> goodfiles)
        {
            var namespacesSet = new HashSet<string> ();
            var typeSet = new HashSet<string> ();
            memberSet = new HashSet<string> ();

            var frameworkEntry = frameworks.StartProcessingAssembly (assembly, assemblySet.Importers);
            foreach (TypeDefinition type in docEnum.GetDocumentationTypes (assembly, null))
            {
                string typename = GetTypeFileName (type);
                if (!IsPublic (type) || typename.IndexOfAny (InvalidFilenameChars) >= 0)
                    continue;

                var typeEntry = frameworkEntry.ProcessType (type);

                string reltypepath = DoUpdateType (type, typeEntry, source, dest);
                if (reltypepath == null)
                    continue;

                // Add namespace and type nodes into the index file as needed
                AddIndexType (type, index_types);

                // Ensure the namespace index file exists
                string namespaceToUse = type.Namespace;
                if (HasDroppedNamespace (assembly))
                {
                    namespaceToUse = string.Format ("{0}.{1}", droppedNamespace, namespaceToUse);
                }
                string onsdoc = DocUtils.PathCombine (dest, namespaceToUse + ".xml");
                string nsdoc = DocUtils.PathCombine (dest, "ns-" + namespaceToUse + ".xml");
                namespacesSet.Add (namespaceToUse);
                if (File.Exists (onsdoc))
                {
                    File.Move (onsdoc, nsdoc);
                }

                if (!File.Exists (nsdoc))
                {
                    statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Namespaces, StatisticsMetrics.Added);
                    Console.WriteLine ("New Namespace File: " + type.Namespace);
                    WriteNamespaceStub (namespaceToUse, dest);
                }

                goodfiles.Add (reltypepath);
                typeSet.Add (type.FullName);
            }
            statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Types, StatisticsMetrics.Total, typeSet.Count);
            statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Namespaces, StatisticsMetrics.Total, namespacesSet.Count);
            statisticsCollector.AddMetric (frameworkEntry.Name, StatisticsItem.Members, StatisticsMetrics.Total, memberSet.Count);
        }

        private static void SortIndexEntries (XmlElement indexTypes)
        {
            XmlNodeList namespaces = indexTypes.SelectNodes ("Namespace");
            XmlNodeComparer c = new AttributeNameComparer ();
            SortXmlNodes (indexTypes, namespaces, c);

            for (int i = 0; i < namespaces.Count; ++i)
                SortXmlNodes (namespaces[i], namespaces[i].SelectNodes ("Type"), c);
        }

        private static void SortXmlNodes (XmlNode parent, XmlNodeList children, XmlNodeComparer comparer)
        {
            MyXmlNodeList l = new MyXmlNodeList (children.Count);
            for (int i = 0; i < children.Count; ++i)
                l.Add (children[i]);
            l.Sort (comparer);
            for (int i = l.Count - 1; i > 0; --i)
            {
                parent.InsertBefore (parent.RemoveChild ((XmlNode)l[i - 1]), (XmlNode)l[i]);
            }
        }

        abstract class XmlNodeComparer : IComparer, IComparer<XmlNode>
        {
            public abstract int Compare (XmlNode x, XmlNode y);

            public int Compare (object x, object y)
            {
                return Compare ((XmlNode)x, (XmlNode)y);
            }
        }

        class AttributeNameComparer : XmlNodeComparer
        {
            string attribute;

            public AttributeNameComparer ()
                : this ("Name")
            {
            }

            public AttributeNameComparer (string attribute)
            {
                this.attribute = attribute;
            }

            public override int Compare (XmlNode x, XmlNode y)
            {
                return x.Attributes[attribute].Value.CompareTo (y.Attributes[attribute].Value);
            }
        }

        class VersionComparer : XmlNodeComparer
        {
            public override int Compare (XmlNode x, XmlNode y)
            {
                // Some of the existing docs use e.g. 1.0.x.x, which Version doesn't like.
                string a = GetVersion (x.InnerText);
                string b = GetVersion (y.InnerText);
                return new Version (a).CompareTo (new Version (b));
            }

            static string GetVersion (string v)
            {
                int n = v.IndexOf ("x");
                if (n < 0)
                    return v;
                return v.Substring (0, n - 1);
            }
        }

        private static string GetTypeKind (TypeDefinition type)
        {
            if (type.IsEnum)
                return "Enumeration";
            if (type.IsValueType)
                return "Structure";
            if (type.IsInterface)
                return "Interface";
            if (DocUtils.IsDelegate (type))
                return "Delegate";
            if (type.IsClass || type.FullName == "System.Enum") // FIXME
                return "Class";
            throw new ArgumentException ("Unknown kind for type: " + type.FullName);
        }

        [Obsolete("Use DocUtils.IsPublic instead")]
        public static bool IsPublic (TypeDefinition type)
        {
            return DocUtils.IsPublic (type);
        }

        private void CleanupFiles (string dest, HashSet<string> goodfiles)
        {
            // Look for files that no longer correspond to types
            foreach (System.IO.DirectoryInfo nsdir in new System.IO.DirectoryInfo (dest).GetDirectories ("*").Where (d => Path.GetFileName (d.FullName) != "FrameworksIndex"))
            {
                foreach (System.IO.FileInfo typefile in nsdir.GetFiles ("*.xml"))
                {
                    string relTypeFile = Path.Combine (nsdir.Name, typefile.Name);
                    if (!goodfiles.Contains (relTypeFile))
                    {
                        XmlDocument doc = new XmlDocument ();
                        doc.Load (typefile.FullName);
                        XmlElement e = doc.SelectSingleNode ("/Type") as XmlElement;
                        var typeFullName = e.GetAttribute("FullName");
                        var assemblyNameNode = doc.SelectSingleNode ("/Type/AssemblyInfo/AssemblyName");
                        if (assemblyNameNode == null)
                        {
                            Warning ("Did not find /Type/AssemblyInfo/AssemblyName on {0}", typefile.FullName);
                            continue;
                        }
                        string assemblyName = assemblyNameNode.InnerText;


                        Action saveDoc = () =>
                        {
                            using (TextWriter writer = OpenWrite (typefile.FullName, FileMode.Truncate))
                                WriteXml (doc.DocumentElement, writer);
                        };

                        if (!IsMultiAssembly)
                        { // only do this for "regular" runs
                            AssemblyDefinition assembly = assemblies
                                .SelectMany (aset => aset.Assemblies)
                                .FirstOrDefault (a => a.Name.Name == assemblyName);
                            if (e != null && !no_assembly_versions && assembly != null && assemblyName != null && UpdateAssemblyVersions (e, assembly, GetAssemblyVersions (assemblyName), false))
                            {
                                saveDoc ();
                                goodfiles.Add (relTypeFile);
                                continue;
                            }
                        }

                        Action actuallyDelete = () =>
                        {
                            string newname = typefile.FullName + ".remove";
                            try { System.IO.File.Delete (newname); } catch (Exception) { Warning ("Unable to delete existing file: {0}", newname); }
                            try { typefile.MoveTo (newname); } catch (Exception) { Warning ("Unable to rename to: {0}", newname); }
                            Console.WriteLine ("Class no longer present; file renamed: " + Path.Combine (nsdir.Name, typefile.Name));

                            // Here we don't know the framwork which contained the removed type. So, we determine it by the old frameworks XML-file
                            // If there is only one framework, use it as a default value
                            var defaultFramework = frameworks.Frameworks.SingleOrDefault();
                            // If there is no frameworks (no frameworks mode) or there is more than one framework
                            if (defaultFramework == null)
                                // Use FrameworkEntry.Empty as the default value (as well as in FrameworkIndex/StartProcessingAssembly)
                                defaultFramework = FrameworkEntry.Empty;
                            var frameworkName = defaultFramework.Name;
                            // Try to find the removed type in the old frameworks XML-file
                            var frameworkXml = oldFrameworkXmls?.FirstOrDefault
                                (i => i.XPathSelectElements($"Framework/Namespace/Type[@Name='{typeFullName}']").Any());
                            var frameworkNameAttribute = frameworkXml?.Root?.Attribute ("Name");
                            // If the removed type is found in the old frameworks XML-file, use this framework name
                            if (frameworkNameAttribute != null)
                                frameworkName = frameworkNameAttribute.Value;
                            statisticsCollector.AddMetric (frameworkName, StatisticsItem.Types, StatisticsMetrics.Removed);
                        };

                        if (string.IsNullOrWhiteSpace (PreserveTag))
                        { // only do this if there was not a -preserve
                            saveDoc ();

                            var unifiedAssemblyNode = doc.SelectSingleNode ("/Type/AssemblyInfo[@apistyle='unified']");
                            var classicAssemblyNode = doc.SelectSingleNode ("/Type/AssemblyInfo[not(@apistyle) or @apistyle='classic']");
                            var unifiedMembers = doc.SelectNodes ("//Member[@apistyle='unified']|//Member/AssemblyInfo[@apistyle='unified']");
                            var classicMembers = doc.SelectNodes ("//Member[@apistyle='classic']|//Member/AssemblyInfo[@apistyle='classic']");
                            bool isUnifiedRun = HasDroppedAnyNamespace ();
                            bool isClassicOrNormalRun = !isUnifiedRun;

                            Action<XmlNode, ApiStyle> removeStyles = (x, style) =>
                            {
                                var styledNodes = doc.SelectNodes ("//*[@apistyle='" + style.ToString ().ToLowerInvariant () + "']");
                                if (styledNodes != null && styledNodes.Count > 0)
                                {
                                    foreach (var node in styledNodes.Cast<XmlNode> ())
                                    {
                                        node.ParentNode.RemoveChild (node);
                                    }
                                }
                                saveDoc ();
                            };
                            if (isClassicOrNormalRun)
                            {
                                if (unifiedAssemblyNode != null || unifiedMembers.Count > 0)
                                {
                                    Warning ("*** this type is marked as unified, not deleting during this run: {0}", typefile.FullName);
                                    // if truly removed from both assemblies, it will be removed fully during the unified run
                                    removeStyles (doc, ApiStyle.Classic);
                                    continue;
                                }
                                else
                                {
                                    // we should be safe to delete here because it was not marked as a unified assembly
                                    actuallyDelete ();
                                }
                            }
                            if (isUnifiedRun)
                            {
                                if (classicAssemblyNode != null || classicMembers.Count > 0)
                                {
                                    Warning ("*** this type is marked as classic, not deleting {0}", typefile.FullName);
                                    continue;
                                }
                                else
                                {
                                    // safe to delete because it wasn't marked as a classic assembly, so the type is gone in both.
                                    actuallyDelete ();
                                }
                            }
                        }
                    }
                }
            }
        }

        private static TextWriter OpenWrite (string path, FileMode mode)
        {
            var w = new StreamWriter (
                new FileStream (path, mode),
                new UTF8Encoding (false)
            );
            w.NewLine = "\n";
            return w;
        }

        private string[] GetAssemblyVersions (string assemblyName)
        {
            return (from a in assemblies.SelectMany (aset => aset.Assemblies)
                    where a.Name.Name == assemblyName
                    select GetAssemblyVersion (a)).ToArray ();
        }

        private static void CleanupIndexTypes (XmlElement index_types, HashSet<string> goodfiles)
        {
            // Look for type nodes that no longer correspond to types
            MyXmlNodeList remove = new MyXmlNodeList ();
            foreach (XmlElement typenode in index_types.SelectNodes ("Namespace/Type"))
            {
                string fulltypename = Path.Combine (((XmlElement)typenode.ParentNode).GetAttribute ("Name"), typenode.GetAttribute ("Name") + ".xml");
                if (!goodfiles.Contains (fulltypename))
                {
                    remove.Add (typenode);
                }
            }
            foreach (XmlNode n in remove)
                n.ParentNode.RemoveChild (n);
        }

        private void CleanupExtensions (XmlElement index_types)
        {
            XmlNode e = index_types.SelectSingleNode ("/Overview/ExtensionMethods");
            if (extensionMethods.Count == 0)
            {
                if (e == null)
                    return;
                index_types.SelectSingleNode ("/Overview").RemoveChild (e);
                return;
            }
            if (e == null)
            {
                e = index_types.OwnerDocument.CreateElement ("ExtensionMethods");
                index_types.SelectSingleNode ("/Overview").AppendChild (e);
            }
            else
                e.RemoveAll ();
            extensionMethods.Sort (DefaultExtensionMethodComparer);
            foreach (XmlNode m in extensionMethods)
            {
                e.AppendChild (index_types.OwnerDocument.ImportNode (m, true));
            }
        }

        class ExtensionMethodComparer : XmlNodeComparer
        {
            public override int Compare (XmlNode x, XmlNode y)
            {
                XmlNode xLink = x.SelectSingleNode ("Member/Link");
                XmlNode yLink = y.SelectSingleNode ("Member/Link");

                int n = xLink.Attributes["Type"].Value.CompareTo (
                        yLink.Attributes["Type"].Value);
                if (n != 0)
                    return n;
                n = xLink.Attributes["Member"].Value.CompareTo (
                        yLink.Attributes["Member"].Value);
                if (n == 0 && !object.ReferenceEquals (x, y))
                    throw new InvalidOperationException ("Duplicate extension method found!");
                return n;
            }
        }

        static readonly XmlNodeComparer DefaultExtensionMethodComparer = new ExtensionMethodComparer ();

        public void DoUpdateType2 (string message, XmlDocument basefile, TypeDefinition type, FrameworkTypeEntry typeEntry, string output, bool insertSince)
        {
            Console.WriteLine (message + ": " + type.FullName);
            StringToXmlNodeMap seenmembers = new StringToXmlNodeMap ();

            // Update type metadata
            UpdateType (basefile.DocumentElement, type, typeEntry);

            // Update existing members.  Delete member nodes that no longer should be there,
            // and remember what members are already documented so we don't add them again.

            MyXmlNodeList todelete = new MyXmlNodeList ();

            foreach (DocsNodeInfo info in docEnum.GetDocumentationMembers (basefile, type))
            {
                XmlElement oldmember = info.Node;
                MemberReference oldmember2 = info.Member;

                if (info.Member != null && info.Node != null)
                {
                    // Check for an error condition where the xml MemberName doesn't match the matched member
                    var memberName = GetMemberName (info.Member);
                    var memberAttribute = info.Node.Attributes["MemberName"];
                    if (memberAttribute == null || (memberAttribute.Value != memberName && memberAttribute.Value.Split (',').Length != memberName.Split (',').Length))
                    {
                        oldmember.SetAttribute ("MemberName", memberName);
                    }
                }

                string sig = oldmember2 != null ? memberFormatters[1].GetDeclaration (oldmember2) : null;

                // Interface implementations and overrides are deleted from the docs
                // unless the overrides option is given.
                if (oldmember2 != null && sig == null)
                    oldmember2 = null;

                // Deleted (or signature changed)
                if (oldmember2 == null)
                {
                    if (!string.IsNullOrWhiteSpace (FrameworksPath))
                    {
                        // verify that this member wasn't seen in another framework and is indeed valid
                        var sigFromXml = oldmember
                            .GetElementsByTagName ("MemberSignature")
                            .Cast<XmlElement> ()
                            .FirstOrDefault (x => x.GetAttribute ("Language").Equals ("ILAsm"));

                        if (sigFromXml != null)
                        {
                            var sigvalue = sigFromXml.GetAttribute ("Value");
                            Func<FrameworkEntry, bool> findTypes = fx =>
                            {
                                var tInstance = fx.Types.FirstOrDefault (t => t.Equals (typeEntry));
                                if (tInstance != null)
                                {
                                    return tInstance.ContainsCSharpSig (sigvalue);
                                }
                                return false;
                            };
                            if (frameworksCache.Frameworks.Any (findTypes))
                                continue;
                        }
                    }

                    if (!no_assembly_versions && UpdateAssemblyVersions (oldmember, type.Module.Assembly, new string[] { GetAssemblyVersion (type.Module.Assembly) }, false))
                        continue;

                    DeleteMember ("Member Removed", output, oldmember, todelete, type);
                    statisticsCollector.AddMetric(typeEntry.Framework.Name, StatisticsItem.Members, StatisticsMetrics.Removed);
                    continue;
                }

                // Duplicated
                if (seenmembers.ContainsKey (sig))
                {
                    if (object.ReferenceEquals (oldmember, seenmembers[sig]))
                    {
                        // ignore, already seen
                    }
                    else
                    {
                        DeleteMember ("Duplicate Member Found", output, oldmember, todelete, type);
                        statisticsCollector.AddMetric(typeEntry.Framework.Name, StatisticsItem.Members, StatisticsMetrics.Removed);
                    }
                    continue;
                }

                // Update signature information
                UpdateMember (info, typeEntry);
                memberSet.Add (info.Member.FullName);

                // get all apistyles of sig from info.Node
                var styles = oldmember.GetElementsByTagName ("MemberSignature").Cast<XmlElement> ()
                    .Where (x => x.GetAttribute ("Language") == "ILAsm" && !seenmembers.ContainsKey (x.GetAttribute ("Value")))
                    .Select (x => x.GetAttribute ("Value"));


                typeEntry.ProcessMember (info.Member);

                foreach (var stylesig in styles)
                {
                    seenmembers.Add (stylesig, oldmember);
                }
            }
            foreach (XmlElement oldmember in todelete)
                oldmember.ParentNode.RemoveChild (oldmember);


            if (!DocUtils.IsDelegate (type))
            {
                XmlNode members = WriteElement (basefile.DocumentElement, "Members");
                var typemembers = type.GetMembers ()
                        .Where (m =>
                        {
                            if (m is TypeDefinition) return false;
                            string cssig = memberFormatters[0].GetDeclaration (m);
                            if (cssig == null) return false;
                            string sig = memberFormatters[1].GetDeclaration (m);
                            if (seenmembers.ContainsKey (sig)) return false;

                            // Verify that the member isn't an explicitly implemented 
                            // member of an internal interface, in which case we shouldn't return true.
                            MethodDefinition methdef = null;
                            if (m is MethodDefinition)
                                methdef = m as MethodDefinition;
                            else if (m is PropertyDefinition)
                            {
                                var prop = m as PropertyDefinition;
                                methdef = prop.GetMethod ?? prop.SetMethod;
                            }
                            else if (m is EventDefinition)
                            {
                                var ev = m as EventDefinition;
                                methdef = ev.AddMethod ?? ev.RemoveMethod;
                            }

                            if (methdef != null)
                            {
                                TypeReference iface;
                                MethodReference imethod;

                                if (methdef.Overrides.Count == 1 && !methdef.IsPublic)
                                {
                                    DocUtils.GetInfoForExplicitlyImplementedMethod (methdef, out iface, out imethod);
                                    if (!DocUtils.IsPublic (iface.Resolve ())) return false;
                                }
                            }

                            return true;
                        })
                        .ToArray ();
                foreach (MemberReference m in typemembers)
                {
                    XmlElement mm = MakeMember (basefile, new DocsNodeInfo (null, m), members, typeEntry);
                    if (mm == null) continue;

                    if (MDocUpdater.SwitchingToMagicTypes || MDocUpdater.HasDroppedNamespace (m))
                    {
                        // this is a unified style API that obviously doesn't exist in the classic API. Let's mark
                        // it with apistyle="unified", so that it's not displayed for classic style APIs
                        mm.AddApiStyle (ApiStyle.Unified);
                    }

                    statisticsCollector.AddMetric (typeEntry.Framework.Name, StatisticsItem.Members, StatisticsMetrics.Added);
                    memberSet.Add (m.FullName);
                    Console.WriteLine ("Member Added: " + mm.SelectSingleNode ("MemberSignature/@Value").InnerText);
                    additions++;
                }
            }

            // Import code snippets from files
            foreach (XmlNode code in basefile.GetElementsByTagName ("code"))
            {
                if (!(code is XmlElement)) continue;
                string file = ((XmlElement)code).GetAttribute ("src");
                string lang = ((XmlElement)code).GetAttribute ("lang");
                if (file != "")
                {
                    string src = GetCodeSource (lang, Path.Combine (srcPath, file));
                    if (src != null)
                        code.InnerText = src;
                }
            }

            if (insertSince && since != null)
            {
                XmlNode docs = basefile.DocumentElement.SelectSingleNode ("Docs");
                docs.AppendChild (CreateSinceNode (basefile));
            }

            do
            {
                XmlElement d = basefile.DocumentElement["Docs"];
                XmlElement m = basefile.DocumentElement["Members"];
                if (d != null && m != null)
                    basefile.DocumentElement.InsertBefore (
                            basefile.DocumentElement.RemoveChild (d), m);
                SortTypeMembers (m);
            } while (false);

            if (output == null)
                WriteXml (basefile.DocumentElement, Console.Out);
            else
            {
                FileInfo file = new FileInfo (output);
                if (!file.Directory.Exists)
                {
                    Console.WriteLine ("Namespace Directory Created: " + type.Namespace);
                    file.Directory.Create ();
                }
                WriteFile (output, FileMode.Create,
                        writer => WriteXml (basefile.DocumentElement, writer));
            }
        }

        private string GetCodeSource (string lang, string file)
        {
            int anchorStart;
            if (lang == "C#" && (anchorStart = file.IndexOf (".cs#")) >= 0)
            {
                // Grab the specified region
                string region = "#region " + file.Substring (anchorStart + 4);
                file = file.Substring (0, anchorStart + 3);
                try
                {
                    using (StreamReader reader = new StreamReader (file))
                    {
                        string line;
                        StringBuilder src = new StringBuilder ();
                        int indent = -1;
                        while ((line = reader.ReadLine ()) != null)
                        {
                            if (line.Trim () == region)
                            {
                                indent = line.IndexOf (region);
                                continue;
                            }
                            if (indent >= 0 && line.Trim ().StartsWith ("#endregion"))
                            {
                                break;
                            }
                            if (indent >= 0)
                                src.Append (
                                        (line.Length > 0 ? line.Substring (indent) : string.Empty) +
                                        "\n");
                        }
                        return src.ToString ();
                    }
                }
                catch (Exception e)
                {
                    Warning ("Could not load <code/> file '{0}' region '{1}': {2}",
                            file, region, show_exceptions ? e.ToString () : e.Message);
                    return null;
                }
            }
            try
            {
                using (StreamReader reader = new StreamReader (file))
                    return reader.ReadToEnd ();
            }
            catch (Exception e)
            {
                Warning ("Could not load <code/> file '" + file + "': " + e.Message);
            }
            return null;
        }

        void DeleteMember (string reason, string output, XmlNode member, MyXmlNodeList todelete, TypeDefinition type)
        {
            string format = output != null
                ? "{0}: File='{1}'; Signature='{4}'"
                : "{0}: XPath='/Type[@FullName=\"{2}\"]/Members/Member[@MemberName=\"{3}\"]'; Signature='{4}'";
            string signature = member.SelectSingleNode ("MemberSignature[@Language='C#']/@Value").Value;
            Warning (format,
                    reason,
                    output,
                    member.OwnerDocument.DocumentElement.GetAttribute ("FullName"),
                    member.Attributes["MemberName"].Value,
                    signature);

            // Identify all of the different states that could affect our decision to delete the member
            bool shouldPreserve = !string.IsNullOrWhiteSpace (PreserveTag);
            bool hasContent = MemberDocsHaveUserContent (member);
            bool shouldDelete = !shouldPreserve && (delete || !hasContent);

            bool unifiedRun = HasDroppedNamespace (type);

            var classicAssemblyInfo = member.SelectSingleNode ("AssemblyInfo[not(@apistyle) or @apistyle='classic']");
            bool nodeIsClassic = classicAssemblyInfo != null || member.HasApiStyle (ApiStyle.Classic);
            var unifiedAssemblyInfo = member.SelectSingleNode ("AssemblyInfo[@apistyle='unified']");
            bool nodeIsUnified = unifiedAssemblyInfo != null || member.HasApiStyle (ApiStyle.Unified);

            Action actuallyDelete = () =>
            {
                todelete.Add (member);
                deletions++;
            };

            if (!shouldDelete)
            {
                // explicitly not deleting
                string message = shouldPreserve ?
                        "Not deleting '{0}' due to --preserve." :
                        "Not deleting '{0}'; must be enabled with the --delete option";
                Warning (message, signature);
            }
            else if (unifiedRun && nodeIsClassic)
            {
                // this is a unified run, and the member doesn't exist, but is marked as being in the classic assembly.
                member.RemoveApiStyle (ApiStyle.Unified);
                member.AddApiStyle (ApiStyle.Classic);
                Warning ("Not removing '{0}' since it's still in the classic assembly.", signature);
            }
            else if (unifiedRun && !nodeIsClassic)
            {
                // unified run, and the node is not classic, which means it doesn't exist anywhere.
                actuallyDelete ();
            }
            else
            {
                if (!isClassicRun || (isClassicRun && !nodeIsClassic && !nodeIsUnified))
                { // regular codepath (ie. not classic/unified)
                    actuallyDelete ();
                }
                else
                { // this is a classic run
                    Warning ("Removing classic from '{0}' ... will be removed in the unified run if not present there.", signature);
                    member.RemoveApiStyle (ApiStyle.Classic);
                    if (classicAssemblyInfo != null)
                    {
                        member.RemoveChild (classicAssemblyInfo);
                    }
                }
            }
        }

        class MemberComparer : XmlNodeComparer
        {
            public override int Compare (XmlNode x, XmlNode y)
            {
                int r;
                string xMemberName = x.Attributes["MemberName"].Value;
                string yMemberName = y.Attributes["MemberName"].Value;

                // generic methods *end* with '>'
                // it's possible for explicitly implemented generic interfaces to
                // contain <...> without being a generic method
                if ((!xMemberName.EndsWith (">") || !yMemberName.EndsWith (">")) &&
                        (r = xMemberName.CompareTo (yMemberName)) != 0)
                    return r;

                int lt;
                if ((lt = xMemberName.IndexOf ("<")) >= 0)
                    xMemberName = xMemberName.Substring (0, lt);
                if ((lt = yMemberName.IndexOf ("<")) >= 0)
                    yMemberName = yMemberName.Substring (0, lt);
                if ((r = xMemberName.CompareTo (yMemberName)) != 0)
                    return r;

                // Handle MemberGroup sorting
                var sc = StringComparison.InvariantCultureIgnoreCase;
                if (x.Name.Equals ("MemberGroup", sc) || y.Name.Equals ("MemberGroup", sc))
                {
                    if (x.Name.Equals ("MemberGroup", sc) && y.Name.Equals ("Member", sc))
                        return -1;
                    else if (x.Name.Equals ("Member", sc) && y.Name.Equals ("MemberGroup", sc))
                        return 1;
                    else
                        return xMemberName.CompareTo (yMemberName);
                }

                // if @MemberName matches, then it's either two different types of
                // members sharing the same name, e.g. field & property, or it's an
                // overloaded method.
                // for different type, sort based on MemberType value.
                r = x.SelectSingleNode ("MemberType").InnerText.CompareTo (
                        y.SelectSingleNode ("MemberType").InnerText);
                if (r != 0)
                    return r;

                // same type -- must be an overloaded method.  Sort based on type 
                // parameter count, then parameter count, then by the parameter 
                // type names.
                XmlNodeList xTypeParams = x.SelectNodes ("TypeParameters/TypeParameter");
                XmlNodeList yTypeParams = y.SelectNodes ("TypeParameters/TypeParameter");
                if (xTypeParams.Count != yTypeParams.Count)
                    return xTypeParams.Count <= yTypeParams.Count ? -1 : 1;
                for (int i = 0; i < xTypeParams.Count; ++i)
                {
                    r = xTypeParams[i].Attributes["Name"].Value.CompareTo (
                            yTypeParams[i].Attributes["Name"].Value);
                    if (r != 0)
                        return r;
                }

                XmlNodeList xParams = x.SelectNodes ("Parameters/Parameter");
                XmlNodeList yParams = y.SelectNodes ("Parameters/Parameter");
                if (xParams.Count != yParams.Count)
                    return xParams.Count <= yParams.Count ? -1 : 1;
                for (int i = 0; i < xParams.Count; ++i)
                {
                    r = xParams[i].Attributes["Type"].Value.CompareTo (
                            yParams[i].Attributes["Type"].Value);
                    if (r != 0)
                        return r;
                }
                // all parameters match, but return value might not match if it was
                // changed between one version and another.
                XmlNode xReturn = x.SelectSingleNode ("ReturnValue/ReturnType");
                XmlNode yReturn = y.SelectSingleNode ("ReturnValue/ReturnType");
                if (xReturn != null && yReturn != null)
                {
                    r = xReturn.InnerText.CompareTo (yReturn.InnerText);
                    if (r != 0)
                        return r;
                }

                return 0;
            }
        }

        static readonly MemberComparer DefaultMemberComparer = new MemberComparer ();

        private static void SortTypeMembers (XmlNode members)
        {
            if (members == null)
                return;
            SortXmlNodes (members, members.SelectNodes ("Member|MemberGroup"), DefaultMemberComparer);
        }

        private static bool MemberDocsHaveUserContent (XmlNode e)
        {
            e = (XmlElement)e.SelectSingleNode ("Docs");
            if (e == null) return false;
            foreach (XmlElement d in e.SelectNodes ("*"))
                if (d.InnerText != "" && !d.InnerText.StartsWith ("To be added"))
                    return true;
            return false;
        }

        // UPDATE HELPER FUNCTIONS

        // CREATE A STUB DOCUMENTATION FILE	

        public XmlElement StubType (TypeDefinition type, string output, IEnumerable<DocumentationImporter> importers)
        {
            string typesig = typeFormatters[0].GetDeclaration (type);
            if (typesig == null) return null; // not publicly visible

            XmlDocument doc = new XmlDocument ();
            XmlElement root = doc.CreateElement ("Type");
            doc.AppendChild (root);

            var frameworkEntry = frameworks.StartProcessingAssembly (type.Module.Assembly, importers);
            var typeEntry = frameworkEntry.ProcessType (type);
            DoUpdateType2 ("New Type", doc, type, typeEntry, output, true);
            statisticsCollector.AddMetric (typeEntry.Framework.Name, StatisticsItem.Types, StatisticsMetrics.Added);

            return root;
        }

        private XmlElement CreateSinceNode (XmlDocument doc)
        {
            XmlElement s = doc.CreateElement ("since");
            s.SetAttribute ("version", since);
            return s;
        }

        // STUBBING/UPDATING FUNCTIONS

        public void UpdateType (XmlElement root, TypeDefinition type, FrameworkTypeEntry typeEntry)
        {
            root.SetAttribute ("Name", GetDocTypeName (type));
            root.SetAttribute ("FullName", GetDocTypeFullName (type));

            foreach (MemberFormatter f in typeFormatters)
            {
                string element = "TypeSignature[@Language='" + f.Language + "']";
                string valueToUse = f.GetDeclaration (type);

                AddXmlNode (
                    root.SelectNodes (element).Cast<XmlElement> ().ToArray (),
                    x => x.GetAttribute ("Value") == valueToUse,
                    x => x.SetAttribute ("Value", valueToUse),
                    () =>
                    {
                        var node = WriteElementAttribute (root, element, "Language", f.Language, forceNewElement: true);
                        var newnode = WriteElementAttribute (root, node, "Value", valueToUse);
                        return newnode;
                    },
                    type);
            }

            AddAssemblyNameToNode (root, type);

            string assemblyInfoNodeFilter = MDocUpdater.HasDroppedNamespace (type) ? "[@apistyle='unified']" : "[not(@apistyle) or @apistyle='classic']";
            Func<XmlElement, bool> assemblyFilter = x => x.SelectSingleNode ("AssemblyName").InnerText == type.Module.Assembly.Name.Name;
            foreach (var ass in root.SelectNodes ("AssemblyInfo" + assemblyInfoNodeFilter).Cast<XmlElement> ().Where (assemblyFilter))
            {
                WriteElementText (ass, "AssemblyName", type.Module.Assembly.Name.Name);
                if (!no_assembly_versions)
                {
                    UpdateAssemblyVersions (ass, type, true);
                }
                else
                {
                    var versions = ass.SelectNodes ("AssemblyVersion").Cast<XmlNode> ().ToList ();
                    foreach (var version in versions)
                        ass.RemoveChild (version);
                }
                if (!string.IsNullOrEmpty (type.Module.Assembly.Name.Culture))
                    WriteElementText (ass, "AssemblyCulture", type.Module.Assembly.Name.Culture);
                else
                    ClearElement (ass, "AssemblyCulture");


                // Why-oh-why do we put assembly attributes in each type file?
                // Neither monodoc nor monodocs2html use them, so I'm deleting them
                // since they're outdated in current docs, and a waste of space.
                //MakeAttributes(ass, type.Assembly, true);
                XmlNode assattrs = ass.SelectSingleNode ("Attributes");
                if (assattrs != null)
                    ass.RemoveChild (assattrs);

                NormalizeWhitespace (ass);
            }

            if (type.IsGenericType ())
            {
                MakeTypeParameters (root, type.GenericParameters, type, MDocUpdater.HasDroppedNamespace (type));
            }
            else
            {
                ClearElement (root, "TypeParameters");
            }

            if (type.BaseType != null)
            {
                XmlElement basenode = WriteElement (root, "Base");

                string basetypename = GetDocTypeFullName (type.BaseType);
                if (basetypename == "System.MulticastDelegate") basetypename = "System.Delegate";

                if (string.IsNullOrWhiteSpace (FrameworksPath))
                    WriteElementText (root, "Base/BaseTypeName", basetypename);
                else
                {
                    // Check for the possibility of an alternate inheritance chain in different frameworks
                    var typeElements = basenode.GetElementsByTagName ("BaseTypeName");

                    if (typeElements.Count == 0) // no existing elements, just add
                        WriteElementText (root, "Base/BaseTypeName", basetypename);
                    else
                    {
                        // There's already a BaseTypeName, see if it matches
                        if (typeElements[0].InnerText != basetypename)
                        {
                            // Add a framework alternate if one doesn't already exist
                            var existing = typeElements.Cast<XmlNode> ().Where (n => n.InnerText == basetypename);
                            if (!existing.Any ())
                            {
                                var newNode = WriteElementText (basenode, "BaseTypeName", basetypename, forceNewElement: true);
                                WriteElementAttribute (basenode, newNode, "FrameworkAlternate", typeEntry.Framework.Name);
                            }
                        }
                    }
                }

                // Document how this type instantiates the generic parameters of its base type
                TypeReference origBase = type.BaseType.GetElementType ();
                if (origBase.IsGenericType ())
                {
                    ClearElement (basenode, "BaseTypeArguments");
                    GenericInstanceType baseInst = type.BaseType as GenericInstanceType;
                    IList<TypeReference> baseGenArgs = baseInst == null ? null : baseInst.GenericArguments;
                    IList<GenericParameter> baseGenParams = origBase.GenericParameters;
                    if (baseGenArgs.Count != baseGenParams.Count)
                        throw new InvalidOperationException ("internal error: number of generic arguments doesn't match number of generic parameters.");
                    for (int i = 0; baseGenArgs != null && i < baseGenArgs.Count; i++)
                    {
                        GenericParameter param = baseGenParams[i];
                        TypeReference value = baseGenArgs[i];

                        XmlElement bta = WriteElement (basenode, "BaseTypeArguments");
                        XmlElement arg = bta.OwnerDocument.CreateElement ("BaseTypeArgument");
                        bta.AppendChild (arg);
                        arg.SetAttribute ("TypeParamName", param.Name);
                        arg.InnerText = GetDocTypeFullName (value);
                    }
                }
            }
            else
            {
                ClearElement (root, "Base");
            }

            if (!DocUtils.IsDelegate (type) && !type.IsEnum)
            {
                IEnumerable<TypeReference> userInterfaces = DocUtils.GetUserImplementedInterfaces (type);
                List<string> interface_names = userInterfaces
                        .Select (iface => GetDocTypeFullName (iface))
                        .OrderBy (s => s)
                        .ToList ();

                XmlElement interfaces = WriteElement (root, "Interfaces");
                interfaces.RemoveAll ();
                foreach (string iname in interface_names)
                {
                    XmlElement iface = root.OwnerDocument.CreateElement ("Interface");
                    interfaces.AppendChild (iface);
                    WriteElementText (iface, "InterfaceName", iname);
                }
            }
            else
            {
                ClearElement (root, "Interfaces");
            }

            MakeAttributes (root, GetCustomAttributes (type), type);

            if (DocUtils.IsDelegate (type))
            {
                MakeTypeParameters (root, type.GenericParameters, type, MDocUpdater.HasDroppedNamespace (type));
                var member = type.GetMethod ("Invoke");
                MakeParameters (root, member, member.Parameters);
                MakeReturnValue (root, member);
            }

            DocsNodeInfo typeInfo = new DocsNodeInfo (WriteElement (root, "Docs"), type);
            MakeDocNode (typeInfo, typeEntry.Framework.Importers);

            if (!DocUtils.IsDelegate (type))
                WriteElement (root, "Members");

            OrderTypeNodes (root, root.ChildNodes);
            NormalizeWhitespace (root);
        }

        /// <summary>Adds an AssemblyInfo with AssemblyName node to an XmlElement.</summary>
        /// <returns>The assembly that was either added, or was already present</returns>
        XmlElement AddAssemblyNameToNode (XmlElement root, TypeDefinition type)
        {
            return AddAssemblyNameToNode (root, type.Module);
        }

        /// <summary>Adds an AssemblyInfo with AssemblyName node to an XmlElement.</summary>
        /// <returns>The assembly that was either added, or was already present</returns>
        XmlElement AddAssemblyNameToNode (XmlElement root, ModuleDefinition module)
        {
            Func<XmlElement, bool> assemblyFilter = x =>
            {
                var existingName = x.SelectSingleNode ("AssemblyName");

                bool apiStyleMatches = true;
                string currentApiStyle = x.GetAttribute ("apistyle");
                if ((HasDroppedNamespace (module) && !string.IsNullOrWhiteSpace (currentApiStyle) && currentApiStyle != "unified") ||
                        (isClassicRun && (string.IsNullOrWhiteSpace (currentApiStyle) || currentApiStyle != "classic")))
                {
                    apiStyleMatches = false;
                }
                return apiStyleMatches && (existingName == null || (existingName != null && existingName.InnerText == module.Assembly.Name.Name));
            };

            return AddAssemblyXmlNode (
                root.SelectNodes ("AssemblyInfo").Cast<XmlElement> ().ToArray (),
                assemblyFilter, x => WriteElementText (x, "AssemblyName", module.Assembly.Name.Name),
                () =>
                {
                    XmlElement ass = WriteElement (root, "AssemblyInfo", forceNewElement: true);

                    if (MDocUpdater.HasDroppedNamespace (module))
                        ass.AddApiStyle (ApiStyle.Unified);
                    if (isClassicRun)
                        ass.AddApiStyle (ApiStyle.Classic);
                    return ass;
                }, module);
        }

        static readonly string[] TypeNodeOrder = {
        "TypeSignature",
        "MemberOfLibrary",
        "AssemblyInfo",
        "ThreadingSafetyStatement",
        "ThreadSafetyStatement",
        "TypeParameters",
        "Base",
        "Interfaces",
        "Attributes",
        "Parameters",
        "ReturnValue",
        "Docs",
        "Members",
        "TypeExcluded",
    };

        static void OrderTypeNodes (XmlNode member, XmlNodeList children)
        {
            ReorderNodes (member, children, TypeNodeOrder);
        }

        internal static IEnumerable<T> Sort<T> (IEnumerable<T> list)
        {
            List<T> l = new List<T> (list);
            l.Sort ();
            return l;
        }

        private void UpdateMember (DocsNodeInfo info, FrameworkTypeEntry typeEntry)
        {
            XmlElement me = (XmlElement)info.Node;
            MemberReference mi = info.Member;
            typeEntry.ProcessMember (mi);
            foreach (MemberFormatter f in memberFormatters)
            {
                string element = "MemberSignature[@Language='" + f.Language + "']";

                var valueToUse = f.GetDeclaration (mi);

                AddXmlNode (
                    me.SelectNodes (element).Cast<XmlElement> ().ToArray (),
                    x => x.GetAttribute ("Value") == valueToUse,
                    x => x.SetAttribute ("Value", valueToUse),
                    () =>
                    {
                        var node = WriteElementAttribute (me, element, "Language", f.Language, forceNewElement: true);
                        var newNode = WriteElementAttribute (me, node, "Value", valueToUse);
                        return newNode;
                    },
                    mi);
            }

            WriteElementText (me, "MemberType", GetMemberType (mi));

            if (!no_assembly_versions)
            {
                if (!IsMultiAssembly)
                    UpdateAssemblyVersions (me, mi, true);
                else
                {
                    var node = AddAssemblyNameToNode (me, mi.Module);

                    UpdateAssemblyVersionForAssemblyInfo (node, me, new[] { GetAssemblyVersion (mi.Module.Assembly) }, add: true);
                }
            }
            else
            {
                ClearElement (me, "AssemblyInfo");
            }

            MakeAttributes (me, GetCustomAttributes (mi), mi.DeclaringType);

            MakeReturnValue (me, mi, MDocUpdater.HasDroppedNamespace (mi));
            if (mi is MethodReference)
            {
                MethodReference mb = (MethodReference)mi;
                if (mb.IsGenericMethod ())
                    MakeTypeParameters (me, mb.GenericParameters, mi, MDocUpdater.HasDroppedNamespace (mi));
            }
            MakeParameters (me, mi, MDocUpdater.HasDroppedNamespace (mi));

            string fieldValue;
            if (mi is FieldDefinition && GetFieldConstValue ((FieldDefinition)mi, out fieldValue))
                WriteElementText (me, "MemberValue", fieldValue);

            info.Node = WriteElement (me, "Docs");
            MakeDocNode (info, typeEntry.Framework.Importers);
            OrderMemberNodes (me, me.ChildNodes);
            UpdateExtensionMethods (me, info);
        }

        static void AddXmlNode (XmlElement[] relevant, Func<XmlElement, bool> valueMatches, Action<XmlElement> setValue, Func<XmlElement> makeNewNode, MemberReference member)
        {
            AddXmlNode (relevant, valueMatches, setValue, makeNewNode, member.Module);
        }

        static void AddXmlNode (XmlElement[] relevant, Func<XmlElement, bool> valueMatches, Action<XmlElement> setValue, Func<XmlElement> makeNewNode, TypeDefinition type)
        {
            AddXmlNode (relevant, valueMatches, setValue, makeNewNode, type.Module);
        }

        static XmlElement AddAssemblyXmlNode (XmlElement[] relevant, Func<XmlElement, bool> valueMatches, Action<XmlElement> setValue, Func<XmlElement> makeNewNode, ModuleDefinition module)
        {
            bool isUnified = MDocUpdater.HasDroppedNamespace (module);
            XmlElement thisAssemblyNode = relevant.FirstOrDefault (valueMatches);
            if (thisAssemblyNode == null)
            {
                thisAssemblyNode = makeNewNode ();
            }
            setValue (thisAssemblyNode);

            if (isUnified)
            {
                thisAssemblyNode.AddApiStyle (ApiStyle.Unified);

                foreach (var otherNodes in relevant.Where (n => n != thisAssemblyNode && n.DoesNotHaveApiStyle (ApiStyle.Unified)))
                {
                    otherNodes.AddApiStyle (ApiStyle.Classic);
                }
            }
            return thisAssemblyNode;
        }

        /// <summary>Adds an xml node, reusing the node if it's available</summary>
        /// <param name="relevant">The existing set of nodes</param>
        /// <param name="valueMatches">Checks to see if the node's value matches what you're trying to write.</param>
        /// <param name="setValue">Sets the node's value</param>
        /// <param name="makeNewNode">Creates a new node, if valueMatches returns false.</param>
        static void AddXmlNode (XmlElement[] relevant, Func<XmlElement, bool> valueMatches, Action<XmlElement> setValue, Func<XmlElement> makeNewNode, ModuleDefinition module)
        {
            bool shouldDuplicate = MDocUpdater.HasDroppedNamespace (module);
            var styleToUse = shouldDuplicate ? ApiStyle.Unified : ApiStyle.Classic;
            var existing = relevant;
            bool done = false;
            bool addedOldApiStyle = false;

            if (shouldDuplicate)
            {
                existing = existing.Where (n => n.HasApiStyle (styleToUse)).ToArray ();
                foreach (var n in relevant.Where (n => n.DoesNotHaveApiStyle (styleToUse)))
                {
                    if (valueMatches (n))
                    {
                        done = true;
                    }
                    else
                    {
                        n.AddApiStyle (ApiStyle.Classic);
                        addedOldApiStyle = true;
                    }
                }
            }
            if (!done)
            {
                if (!existing.Any ())
                {
                    var newNode = makeNewNode ();
                    if (shouldDuplicate && addedOldApiStyle)
                    {
                        newNode.AddApiStyle (ApiStyle.Unified);
                    }
                }
                else
                {
                    var itemToReuse = existing.First ();
                    setValue (itemToReuse);

                    if (shouldDuplicate && addedOldApiStyle)
                    {
                        itemToReuse.AddApiStyle (styleToUse);
                    }
                }
            }
        }

        static readonly string[] MemberNodeOrder = {
        "MemberSignature",
        "MemberType",
        "AssemblyInfo",
        "Attributes",
        "ReturnValue",
        "TypeParameters",
        "Parameters",
        "MemberValue",
        "Docs",
        "Excluded",
        "ExcludedLibrary",
        "Link",
    };

        static void OrderMemberNodes (XmlNode member, XmlNodeList children)
        {
            ReorderNodes (member, children, MemberNodeOrder);
        }

        static void ReorderNodes (XmlNode node, XmlNodeList children, string[] ordering)
        {
            MyXmlNodeList newChildren = new MyXmlNodeList (children.Count);
            for (int i = 0; i < ordering.Length; ++i)
            {
                for (int j = 0; j < children.Count; ++j)
                {
                    XmlNode c = children[j];
                    if (c.Name == ordering[i])
                    {
                        newChildren.Add (c);
                    }
                }
            }
            if (newChildren.Count >= 0)
                node.PrependChild ((XmlNode)newChildren[0]);
            for (int i = 1; i < newChildren.Count; ++i)
            {
                XmlNode prev = (XmlNode)newChildren[i - 1];
                XmlNode cur = (XmlNode)newChildren[i];
                node.RemoveChild (cur);
                node.InsertAfter (cur, prev);
            }
        }

        IEnumerable<string> GetCustomAttributes (MemberReference mi)
        {
            IEnumerable<string> attrs = Enumerable.Empty<string> ();

            ICustomAttributeProvider p = mi as ICustomAttributeProvider;
            if (p != null)
                attrs = attrs.Concat (GetCustomAttributes (p.CustomAttributes, ""));

            PropertyDefinition pd = mi as PropertyDefinition;
            if (pd != null)
            {
                if (pd.GetMethod != null)
                    attrs = attrs.Concat (GetCustomAttributes (pd.GetMethod.CustomAttributes, "get: "));
                if (pd.SetMethod != null)
                    attrs = attrs.Concat (GetCustomAttributes (pd.SetMethod.CustomAttributes, "set: "));
            }

            EventDefinition ed = mi as EventDefinition;
            if (ed != null)
            {
                if (ed.AddMethod != null)
                    attrs = attrs.Concat (GetCustomAttributes (ed.AddMethod.CustomAttributes, "add: "));
                if (ed.RemoveMethod != null)
                    attrs = attrs.Concat (GetCustomAttributes (ed.RemoveMethod.CustomAttributes, "remove: "));
            }

            return attrs;
        }

        IEnumerable<string> GetCustomAttributes (IList<CustomAttribute> attributes, string prefix)
        {
            foreach (CustomAttribute attribute in attributes.OrderBy (ca => ca.AttributeType.FullName))
            {
                TypeDefinition attrType = attribute.AttributeType as TypeDefinition;
                if (attrType != null && !IsPublic (attrType))
                    continue;
                if (slashdocFormatter.GetName (attribute.AttributeType) == null)
                    continue;

                if (Array.IndexOf (IgnorableAttributes, attribute.AttributeType.FullName) >= 0)
                    continue;

                StringList fields = new StringList ();

                for (int i = 0; i < attribute.ConstructorArguments.Count; ++i)
                {
                    CustomAttributeArgument argument = attribute.ConstructorArguments[i];
                    fields.Add (MakeAttributesValueString (
                            argument.Value,
                            argument.Type));
                }
                var namedArgs =
                    (from namedArg in attribute.Fields
                     select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value })
                    .Concat (
                            (from namedArg in attribute.Properties
                             select new { Type = namedArg.Argument.Type, Name = namedArg.Name, Value = namedArg.Argument.Value }))
                    .OrderBy (v => v.Name);
                foreach (var d in namedArgs)
                    fields.Add (string.Format ("{0}={1}", d.Name,
                            MakeAttributesValueString (d.Value, d.Type)));

                string a2 = String.Join (", ", fields.ToArray ());
                if (a2 != "") a2 = "(" + a2 + ")";

                string name = attribute.GetDeclaringType ();
                if (name.EndsWith ("Attribute")) name = name.Substring (0, name.Length - "Attribute".Length);
                yield return prefix + name + a2;
            }
        }

        static readonly string[] ValidExtensionMembers = {
        "Docs",
        "MemberSignature",
        "MemberType",
        "Parameters",
        "ReturnValue",
        "TypeParameters",
    };

        static readonly string[] ValidExtensionDocMembers = {
        "param",
        "summary",
        "typeparam",
    };

        private void UpdateExtensionMethods (XmlElement e, DocsNodeInfo info)
        {
            MethodDefinition me = info.Member as MethodDefinition;
            if (me == null)
                return;
            if (info.Parameters.Count < 1)
                return;
            if (!DocUtils.IsExtensionMethod (me))
                return;

            XmlNode em = e.OwnerDocument.CreateElement ("ExtensionMethod");
            XmlNode member = e.CloneNode (true);
            em.AppendChild (member);
            RemoveExcept (member, ValidExtensionMembers);
            RemoveExcept (member.SelectSingleNode ("Docs"), ValidExtensionDocMembers);
            WriteElementText (member, "MemberType", "ExtensionMethod");
            XmlElement link = member.OwnerDocument.CreateElement ("Link");
            var linktype = slashdocFormatter.GetName (me.DeclaringType);
            var linkmember = slashdocFormatter.GetDeclaration (me);
            link.SetAttribute ("Type", linktype);
            link.SetAttribute ("Member", linkmember);
            member.AppendChild (link);
            AddTargets (em, info);

            if (!IsMultiAssembly || (IsMultiAssembly && !extensionMethods.Any (ex => ex.SelectSingleNode ("Member/Link/@Type").Value == linktype && ex.SelectSingleNode ("Member/Link/@Member").Value == linkmember)))
            {
                extensionMethods.Add (em);
            }
        }

        private static void RemoveExcept (XmlNode node, string[] except)
        {
            if (node == null)
                return;
            MyXmlNodeList remove = null;
            foreach (XmlNode n in node.ChildNodes)
            {
                if (Array.BinarySearch (except, n.Name) < 0)
                {
                    if (remove == null)
                        remove = new MyXmlNodeList ();
                    remove.Add (n);
                }
            }
            if (remove != null)
                foreach (XmlNode n in remove)
                    node.RemoveChild (n);
        }

        private static void AddTargets (XmlNode member, DocsNodeInfo info)
        {
            XmlElement targets = member.OwnerDocument.CreateElement ("Targets");
            member.PrependChild (targets);
            if (!(info.Parameters[0].ParameterType is GenericParameter))
            {
                var reference = info.Parameters[0].ParameterType;
                TypeReference typeReference = reference as TypeReference;
                var declaration = reference != null ?
                    slashdocFormatter.GetDeclaration (typeReference) :
                    slashdocFormatter.GetDeclaration (reference);

                AppendElementAttributeText (targets, "Target", "Type", declaration);
            }
            else
            {
                GenericParameter gp = (GenericParameter)info.Parameters[0].ParameterType;
                IList<TypeReference> constraints = gp.Constraints;
                if (constraints.Count == 0)
                    AppendElementAttributeText (targets, "Target", "Type", "System.Object");
                else
                    foreach (TypeReference c in constraints)
                        AppendElementAttributeText (targets, "Target", "Type",
                            slashdocFormatter.GetDeclaration (c));
            }
        }

        private static bool GetFieldConstValue (FieldDefinition field, out string value)
        {
            value = null;
            TypeDefinition type = field.DeclaringType.Resolve ();
            if (type != null && type.IsEnum) return false;

            if (type != null && type.IsGenericType ()) return false;
            if (!field.HasConstant)
                return false;
            if (field.IsLiteral)
            {
                object val = field.Constant;
                if (val == null) value = "null";
                else if (val is Enum) value = val.ToString ();
                else if (val is IFormattable)
                {
                    value = ((IFormattable)val).ToString (null, CultureInfo.InvariantCulture);
                    if (val is string)
                        value = "\"" + value + "\"";
                }
                if (value != null && value != "")
                    return true;
            }
            return false;
        }

        // XML HELPER FUNCTIONS

        internal static XmlElement WriteElement (XmlNode parent, string element, bool forceNewElement = false)
        {
            XmlElement ret = (XmlElement)parent.SelectSingleNode (element);
            if (ret == null || forceNewElement)
            {
                string[] path = element.Split ('/');
                foreach (string p in path)
                {
                    ret = (XmlElement)parent.SelectSingleNode (p);
                    if (ret == null || forceNewElement)
                    {
                        string ename = p;
                        if (ename.IndexOf ('[') >= 0) // strip off XPath predicate
                            ename = ename.Substring (0, ename.IndexOf ('['));
                        ret = parent.OwnerDocument.CreateElement (ename);
                        parent.AppendChild (ret);
                        parent = ret;
                    }
                    else
                    {
                        parent = ret;
                    }
                }
            }
            return ret;
        }
        private static XmlElement WriteElementText (XmlNode parent, string element, string value, bool forceNewElement = false)
        {
            XmlElement node = WriteElement (parent, element, forceNewElement: forceNewElement);
            node.InnerText = value;
            return node;
        }

        static XmlElement AppendElementText (XmlNode parent, string element, string value)
        {
            XmlElement n = parent.OwnerDocument.CreateElement (element);
            parent.AppendChild (n);
            n.InnerText = value;
            return n;
        }

        static XmlElement AppendElementAttributeText (XmlNode parent, string element, string attribute, string value)
        {
            XmlElement n = parent.OwnerDocument.CreateElement (element);
            parent.AppendChild (n);
            n.SetAttribute (attribute, value);
            return n;
        }

        internal static XmlNode CopyNode (XmlNode source, XmlNode dest)
        {
            XmlNode copy = dest.OwnerDocument.ImportNode (source, true);
            dest.AppendChild (copy);
            return copy;
        }

        private static void WriteElementInitialText (XmlElement parent, string element, string value)
        {
            XmlElement node = (XmlElement)parent.SelectSingleNode (element);
            if (node != null)
                return;
            node = WriteElement (parent, element);
            node.InnerText = value;
        }
        private static XmlElement WriteElementAttribute (XmlElement parent, string element, string attribute, string value, bool forceNewElement = false)
        {
            XmlElement node = WriteElement (parent, element, forceNewElement: forceNewElement);
            return WriteElementAttribute (parent, node, attribute, value);
        }
        private static XmlElement WriteElementAttribute (XmlElement parent, XmlElement node, string attribute, string value)
        {
            if (node.GetAttribute (attribute) != value)
            {
                node.SetAttribute (attribute, value);
            }
            return node;
        }
        internal static void ClearElement (XmlElement parent, string name)
        {
            XmlElement node = (XmlElement)parent.SelectSingleNode (name);
            if (node != null)
                parent.RemoveChild (node);
        }

        // DOCUMENTATION HELPER FUNCTIONS

        private void MakeDocNode (DocsNodeInfo info, IEnumerable<DocumentationImporter> setimporters)
        {
            List<GenericParameter> genericParams = info.GenericParameters;
            IList<ParameterDefinition> parameters = info.Parameters;
            TypeReference returntype = info.ReturnType;
            bool returnisreturn = info.ReturnIsReturn;
            XmlElement e = info.Node;
            bool addremarks = info.AddRemarks;

            WriteElementInitialText (e, "summary", "To be added.");

            if (parameters != null)
            {
                string[] values = new string[parameters.Count];
                for (int i = 0; i < values.Length; ++i)
                    values[i] = parameters[i].Name;
                UpdateParameters (e, "param", values);
            }

            if (genericParams != null)
            {
                string[] values = new string[genericParams.Count];
                for (int i = 0; i < values.Length; ++i)
                    values[i] = genericParams[i].Name;
                UpdateParameters (e, "typeparam", values);
            }

            string retnodename = null;
            if (returntype != null && returntype.FullName != "System.Void")
            { // FIXME
                retnodename = returnisreturn ? "returns" : "value";
                string retnodename_other = !returnisreturn ? "returns" : "value";

                // If it has a returns node instead of a value node, change its name.
                XmlElement retother = (XmlElement)e.SelectSingleNode (retnodename_other);
                if (retother != null)
                {
                    XmlElement retnode = e.OwnerDocument.CreateElement (retnodename);
                    foreach (XmlNode node in retother)
                        retnode.AppendChild (node.CloneNode (true));
                    e.ReplaceChild (retnode, retother);
                }
                else
                {
                    WriteElementInitialText (e, retnodename, "To be added.");
                }
            }
            else
            {
                if (DocUtils.NeedsOverwrite(e["returns"]))
                    ClearElement(e, "returns");
                if (DocUtils.NeedsOverwrite(e["value"]))
                    ClearElement(e, "value");
            }

            if (addremarks)
                WriteElementInitialText (e, "remarks", "To be added.");

            if (exceptions.HasValue && info.Member != null &&
                    (exceptions.Value & ExceptionLocations.AddedMembers) == 0)
            {
                UpdateExceptions (e, info.Member);
            }

            foreach (DocumentationImporter importer in importers)
            {
                importer.ImportDocumentation (info);
            }
            if (setimporters != null)
            {
                foreach (var i in setimporters)
                    i.ImportDocumentation (info);
            }

            OrderDocsNodes (e, e.ChildNodes);
            NormalizeWhitespace (e);
        }

        static readonly string[] DocsNodeOrder = {
        "typeparam", "param", "summary", "returns", "value", "remarks",
    };

        private static void OrderDocsNodes (XmlNode docs, XmlNodeList children)
        {
            ReorderNodes (docs, children, DocsNodeOrder);
        }


        private void UpdateParameters (XmlElement e, string element, string[] values)
        {
            if (values != null)
            {
                XmlNode[] paramnodes = new XmlNode[values.Length];

                // Some documentation had param nodes with leading spaces.
                foreach (XmlElement paramnode in e.SelectNodes (element))
                {
                    paramnode.SetAttribute ("name", paramnode.GetAttribute ("name").Trim ());
                }

                // If a member has only one parameter, we can track changes to
                // the name of the parameter easily.
                if (values.Length == 1 && e.SelectNodes (element).Count == 1)
                {
                    UpdateParameterName (e, (XmlElement)e.SelectSingleNode (element), values[0]);
                }

                bool reinsert = false;

                // Pick out existing and still-valid param nodes, and
                // create nodes for parameters not in the file.
                Hashtable seenParams = new Hashtable ();
                for (int pi = 0; pi < values.Length; pi++)
                {
                    string p = values[pi];
                    seenParams[p] = pi;

                    paramnodes[pi] = e.SelectSingleNode (element + "[@name='" + p + "']");
                    if (paramnodes[pi] != null) continue;

                    XmlElement pe = e.OwnerDocument.CreateElement (element);
                    pe.SetAttribute ("name", p);
                    pe.InnerText = "To be added.";
                    paramnodes[pi] = pe;
                    reinsert = true;
                }

                // Remove parameters that no longer exist and check all params are in the right order.
                int idx = 0;
                MyXmlNodeList todelete = new MyXmlNodeList ();
                foreach (XmlElement paramnode in e.SelectNodes (element))
                {
                    string name = paramnode.GetAttribute ("name");
                    if (!seenParams.ContainsKey (name))
                    {
                        if (!delete && !paramnode.InnerText.StartsWith ("To be added"))
                        {
                            Warning ("The following param node can only be deleted if the --delete option is given: ");
                            if (e.ParentNode == e.OwnerDocument.DocumentElement)
                            {
                                // delegate type
                                Warning ("\tXPath=/Type[@FullName=\"{0}\"]/Docs/param[@name=\"{1}\"]",
                                        e.OwnerDocument.DocumentElement.GetAttribute ("FullName"),
                                        name);
                            }
                            else
                            {
                                Warning ("\tXPath=/Type[@FullName=\"{0}\"]//Member[@MemberName=\"{1}\"]/Docs/param[@name=\"{2}\"]",
                                        e.OwnerDocument.DocumentElement.GetAttribute ("FullName"),
                                        e.ParentNode.Attributes["MemberName"].Value,
                                        name);
                            }
                            Warning ("\tValue={0}", paramnode.OuterXml);
                        }
                        else
                        {
                            todelete.Add (paramnode);
                        }
                        continue;
                    }

                    if ((int)seenParams[name] != idx)
                        reinsert = true;

                    idx++;
                }

                foreach (XmlNode n in todelete)
                {
                    n.ParentNode.RemoveChild (n);
                }

                // Re-insert the parameter nodes at the top of the doc section.
                if (reinsert)
                    for (int pi = values.Length - 1; pi >= 0; pi--)
                        e.PrependChild (paramnodes[pi]);
            }
            else
            {
                // Clear all existing param nodes
                foreach (XmlNode paramnode in e.SelectNodes (element))
                {
                    if (!delete && !paramnode.InnerText.StartsWith ("To be added"))
                    {
                        Console.WriteLine ("The following param node can only be deleted if the --delete option is given:");
                        Console.WriteLine (paramnode.OuterXml);
                    }
                    else
                    {
                        paramnode.ParentNode.RemoveChild (paramnode);
                    }
                }
            }
        }

        private static void UpdateParameterName (XmlElement docs, XmlElement pe, string newName)
        {
            string existingName = pe.GetAttribute ("name");
            pe.SetAttribute ("name", newName);
            if (existingName == newName)
                return;
            foreach (XmlElement paramref in docs.SelectNodes (".//paramref"))
                if (paramref.GetAttribute ("name").Trim () == existingName)
                    paramref.SetAttribute ("name", newName);
        }

        class CrefComparer : XmlNodeComparer
        {

            public CrefComparer ()
            {
            }

            public override int Compare (XmlNode x, XmlNode y)
            {
                string xType = x.Attributes["cref"].Value;
                string yType = y.Attributes["cref"].Value;
                string xNamespace = GetNamespace (xType);
                string yNamespace = GetNamespace (yType);

                int c = xNamespace.CompareTo (yNamespace);
                if (c != 0)
                    return c;
                return xType.CompareTo (yType);
            }

            static string GetNamespace (string type)
            {
                int n = type.LastIndexOf ('.');
                if (n >= 0)
                    return type.Substring (0, n);
                return string.Empty;
            }
        }

        private void UpdateExceptions (XmlNode docs, MemberReference member)
        {
            string indent = new string (' ', 10);
            foreach (var source in new ExceptionLookup (exceptions.Value)[member])
            {
                string cref = slashdocFormatter.GetDeclaration (source.Exception);
                var node = docs.SelectSingleNode ("exception[@cref='" + cref + "']");
                if (node != null)
                    continue;
                XmlElement e = docs.OwnerDocument.CreateElement ("exception");
                e.SetAttribute ("cref", cref);
                e.InnerXml = "To be added; from:\n" + indent + "<see cref=\"" +
                    string.Join ("\" />,\n" + indent + "<see cref=\"",
                            source.Sources.Select (m => slashdocFormatter.GetDeclaration (m))
                            .OrderBy (s => s)) +
                    "\" />";
                docs.AppendChild (e);
            }
            SortXmlNodes (docs, docs.SelectNodes ("exception"),
                    new CrefComparer ());
        }

        private static void NormalizeWhitespace (XmlElement e)
        {
            // Remove all text and whitespace nodes from the element so it
            // is outputted with nice indentation and no blank lines.
            ArrayList deleteNodes = new ArrayList ();
            foreach (XmlNode n in e)
                if (n is XmlText || n is XmlWhitespace || n is XmlSignificantWhitespace)
                    deleteNodes.Add (n);
            foreach (XmlNode n in deleteNodes)
                n.ParentNode.RemoveChild (n);
        }

        private bool UpdateAssemblyVersions (XmlElement root, MemberReference member, bool add)
        {
            TypeDefinition type = member as TypeDefinition;
            if (type == null)
                type = member.DeclaringType as TypeDefinition;

            var versions = new string[] { GetAssemblyVersion (type.Module.Assembly) };

            if (root.LocalName == "AssemblyInfo")
                return UpdateAssemblyVersionForAssemblyInfo (root, root.ParentNode as XmlElement, versions, add: true);
            else
                return UpdateAssemblyVersions (root, type.Module.Assembly, versions, add);
        }

        private static string GetAssemblyVersion (AssemblyDefinition assembly)
        {
            return assembly.Name.Version.ToString ();
        }

        private bool UpdateAssemblyVersions (XmlElement root, AssemblyDefinition assembly, string[] assemblyVersions, bool add)
        {
            if (IsMultiAssembly)
                return false;

            XmlElement av = (XmlElement)root.SelectSingleNode ("AssemblyVersions");
            if (av != null)
            {
                // AssemblyVersions is not part of the spec
                root.RemoveChild (av);
            }

            string oldNodeFilter = "AssemblyInfo[not(@apistyle) or @apistyle='classic']";
            string newNodeFilter = "AssemblyInfo[@apistyle='unified']";
            string thisNodeFilter = MDocUpdater.HasDroppedNamespace (assembly) ? newNodeFilter : oldNodeFilter;
            string thatNodeFilter = MDocUpdater.HasDroppedNamespace (assembly) ? oldNodeFilter : newNodeFilter;

            XmlElement e = (XmlElement)root.SelectSingleNode (thisNodeFilter);
            if (e == null)
            {
                e = root.OwnerDocument.CreateElement ("AssemblyInfo");

                if (MDocUpdater.HasDroppedNamespace (assembly))
                {
                    e.AddApiStyle (ApiStyle.Unified);
                }

                root.AppendChild (e);
            }

            var thatNode = (XmlElement)root.SelectSingleNode (thatNodeFilter);
            if (MDocUpdater.HasDroppedNamespace (assembly) && thatNode != null)
            {
                // there's a classic node, we should add apistyles
                e.AddApiStyle (ApiStyle.Unified);
                thatNode.AddApiStyle (ApiStyle.Classic);
            }

            return UpdateAssemblyVersionForAssemblyInfo (e, root, assemblyVersions, add);
        }

        static bool UpdateAssemblyVersionForAssemblyInfo (XmlElement e, XmlElement root, string[] assemblyVersions, bool add)
        {
            List<XmlNode> matches = e.SelectNodes ("AssemblyVersion").Cast<XmlNode> ().Where (v => Array.IndexOf (assemblyVersions, v.InnerText) >= 0).ToList ();
            // matches.Count > 0 && add: ignore -- already present
            if (matches.Count > 0 && !add)
            {
                foreach (XmlNode c in matches)
                    e.RemoveChild (c);
            }
            else if (matches.Count == 0 && add)
            {
                foreach (string sv in assemblyVersions)
                {
                    XmlElement c = root.OwnerDocument.CreateElement ("AssemblyVersion");
                    c.InnerText = sv;
                    e.AppendChild (c);
                }
            }

            // matches.Count == 0 && !add: ignore -- already not present
            XmlNodeList avs = e.SelectNodes ("AssemblyVersion");
            SortXmlNodes (e, avs, new VersionComparer ());

            bool anyNodesLeft = avs.Count != 0;
            if (!anyNodesLeft)
            {
                e.ParentNode.RemoveChild (e);
            }
            return anyNodesLeft;
        }

        // FIXME: get TypeReferences instead of string comparison?
        private static string[] IgnorableAttributes = {
		// Security related attributes
		"System.Reflection.AssemblyKeyFileAttribute",
        "System.Reflection.AssemblyDelaySignAttribute",
		// Present in @RefType
		"System.Runtime.InteropServices.OutAttribute",
		// For naming the indexer to use when not using indexers
		"System.Reflection.DefaultMemberAttribute",
		// for decimal constants
		"System.Runtime.CompilerServices.DecimalConstantAttribute",
		// compiler generated code
		"System.Runtime.CompilerServices.CompilerGeneratedAttribute",
		// more compiler generated code, e.g. iterator methods
		"System.Diagnostics.DebuggerHiddenAttribute",
        "System.Runtime.CompilerServices.FixedBufferAttribute",
        "System.Runtime.CompilerServices.UnsafeValueTypeAttribute",
		// extension methods
		"System.Runtime.CompilerServices.ExtensionAttribute",
		// Used to differentiate 'object' from C#4 'dynamic'
		"System.Runtime.CompilerServices.DynamicAttribute",
    };

        private IEnumerable<char> FilterSpecialChars (string value)
        {
            foreach (char c in value)
            {
                switch (c)
                {
                    case '\0':
                        yield return '\\';
                        yield return '0';
                        break;
                    case '\t':
                        yield return '\\';
                        yield return 't';
                        break;
                    case '\n':
                        yield return '\\';
                        yield return 'n';
                        break;
                    case '\r':
                        yield return '\\';
                        yield return 'r';
                        break;
                    case '\f':
                        yield return '\\';
                        yield return 'f';
                        break;
                    case '\b':
                        yield return '\\';
                        yield return 'b';
                        break;
                    default:
                        yield return c;
                        break;
                }
            }
        }

        private void MakeAttributes (XmlElement root, IEnumerable<string> attributes, TypeReference t = null)
        {
            if (!attributes.Any ())
            {
                ClearElement (root, "Attributes");
                return;
            }

            XmlElement e = (XmlElement)root.SelectSingleNode ("Attributes");
            if (e != null)
                e.RemoveAll ();
            else if (e == null)
                e = root.OwnerDocument.CreateElement ("Attributes");


            foreach (string attribute in attributes)
            {
                XmlElement ae = root.OwnerDocument.CreateElement ("Attribute");
                e.AppendChild (ae);
                var value = new String (FilterSpecialChars (attribute).ToArray ());
                WriteElementText (ae, "AttributeName", value);
            }

            if (e.ParentNode == null)
                root.AppendChild (e);

            NormalizeWhitespace (e);
        }

        public static string MakeAttributesValueString (object v, TypeReference valueType)
        {
            var formatters = new[] {
            new AttributeValueFormatter (),
            new ApplePlatformEnumFormatter (),
            new StandardFlagsEnumFormatter (),
            new DefaultAttributeValueFormatter (),
        };

            ResolvedTypeInfo type = new ResolvedTypeInfo (valueType);
            foreach (var formatter in formatters)
            {
                string formattedValue;
                if (formatter.TryFormatValue (v, type, out formattedValue))
                {
                    return formattedValue;
                }
            }

            // this should never occur because the DefaultAttributeValueFormatter will always
            // successfully format the value ... but this is needed to satisfy the compiler :)
            throw new InvalidDataException (string.Format ("Unable to format attribute value ({0})", v.ToString ()));
        }

        internal static IDictionary<long, string> GetEnumerationValues (TypeDefinition type)
        {
            var values = new Dictionary<long, string> ();
            foreach (var f in
                    (from f in type.Fields
                     where !(f.IsRuntimeSpecialName || f.IsSpecialName)
                     select f))
            {
                values[ToInt64 (f.Constant)] = f.Name;
            }
            return values;
        }

        internal static long ToInt64 (object value)
        {
            if (value is ulong)
                return (long)(ulong)value;
            return Convert.ToInt64 (value);
        }

        private void MakeParameters (XmlElement root, MemberReference member, IList<ParameterDefinition> parameters, bool shouldDuplicateWithNew = false)
        {
            XmlElement e = WriteElement (root, "Parameters");

            int i = 0;
            foreach (ParameterDefinition p in parameters)
            {
                XmlElement pe;

                // param info
                var ptype = GetDocParameterType (p.ParameterType);
                var newPType = ptype;

                if (MDocUpdater.SwitchingToMagicTypes)
                {
                    newPType = NativeTypeManager.ConvertFromNativeType (ptype);
                }

                // now find the existing node, if it's there so we can reuse it.
                var nodes = root.SelectSingleNode ("Parameters").SelectNodes ("Parameter")
                    .Cast<XmlElement> ().Where (x => x.GetAttribute ("Name") == p.Name)
                    .ToArray ();

                if (nodes.Count () == 0)
                {
                    // wasn't found, let's make sure it wasn't just cause the param name was changed
                    nodes = root.SelectSingleNode ("Parameters").SelectNodes ("Parameter")
                        .Cast<XmlElement> ()
                        .Skip (i) // this makes sure we don't inadvertently "reuse" nodes when adding new ones
                        .Where (x => x.GetAttribute ("Name") != p.Name && (x.GetAttribute ("Type") == ptype || x.GetAttribute ("Type") == newPType))
                        .Take (1) // there might be more than one that meets this parameter ... only take the first.
                        .ToArray ();
                }

                AddXmlNode (nodes,
                    x => x.GetAttribute ("Type") == ptype,
                    x => x.SetAttribute ("Type", ptype),
                    () =>
                    {
                        pe = root.OwnerDocument.CreateElement ("Parameter");
                        e.AppendChild (pe);

                        pe.SetAttribute ("Name", p.Name);
                        pe.SetAttribute ("Type", ptype);
                        if (p.ParameterType is ByReferenceType)
                        {
                            if (p.IsOut)
                                pe.SetAttribute ("RefType", "out");
                            else
                                pe.SetAttribute ("RefType", "ref");
                        }

                        MakeAttributes (pe, GetCustomAttributes (p.CustomAttributes, ""));
                        return pe;
                    },
                    member);

                i++;
            }
        }

        private void MakeTypeParameters (XmlElement root, IList<GenericParameter> typeParams, MemberReference member, bool shouldDuplicateWithNew)
        {
            if (typeParams == null || typeParams.Count == 0)
            {
                XmlElement f = (XmlElement)root.SelectSingleNode ("TypeParameters");
                if (f != null)
                    root.RemoveChild (f);
                return;
            }
            XmlElement e = WriteElement (root, "TypeParameters");

            var nodes = e.SelectNodes ("TypeParameter").Cast<XmlElement> ().ToArray ();

            foreach (GenericParameter t in typeParams)
            {

                IList<TypeReference> constraints = t.Constraints;
                GenericParameterAttributes attrs = t.Attributes;


                AddXmlNode (
                    nodes,
                    x =>
                    {
                        var baseType = e.SelectSingleNode ("BaseTypeName");
                        // TODO: should this comparison take into account BaseTypeName?
                        return x.GetAttribute ("Name") == t.Name;
                    },
                    x => { }, // no additional action required
                    () =>
                    {

                        XmlElement pe = root.OwnerDocument.CreateElement ("TypeParameter");
                        e.AppendChild (pe);
                        pe.SetAttribute ("Name", t.Name);
                        MakeAttributes (pe, GetCustomAttributes (t.CustomAttributes, ""), t.DeclaringType);
                        XmlElement ce = (XmlElement)e.SelectSingleNode ("Constraints");
                        if (attrs == GenericParameterAttributes.NonVariant && constraints.Count == 0)
                        {
                            if (ce != null)
                                e.RemoveChild (ce);
                            return pe;
                        }
                        if (ce != null)
                            ce.RemoveAll ();
                        else
                        {
                            ce = root.OwnerDocument.CreateElement ("Constraints");
                        }
                        pe.AppendChild (ce);
                        if ((attrs & GenericParameterAttributes.Contravariant) != 0)
                            AppendElementText (ce, "ParameterAttribute", "Contravariant");
                        if ((attrs & GenericParameterAttributes.Covariant) != 0)
                            AppendElementText (ce, "ParameterAttribute", "Covariant");
                        if ((attrs & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
                            AppendElementText (ce, "ParameterAttribute", "DefaultConstructorConstraint");
                        if ((attrs & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0)
                            AppendElementText (ce, "ParameterAttribute", "NotNullableValueTypeConstraint");
                        if ((attrs & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
                            AppendElementText (ce, "ParameterAttribute", "ReferenceTypeConstraint");
                        foreach (TypeReference c in constraints)
                        {
                            TypeDefinition cd = c.Resolve ();
                            AppendElementText (ce,
                                    (cd != null && cd.IsInterface) ? "InterfaceName" : "BaseTypeName",
                                    GetDocTypeFullName (c));
                        }

                        return pe;
                    },
                member);
            }
        }

        private void MakeParameters (XmlElement root, MemberReference mi, bool shouldDuplicateWithNew)
        {
            if (mi is MethodDefinition && ((MethodDefinition)mi).IsConstructor)
                MakeParameters (root, mi, ((MethodDefinition)mi).Parameters, shouldDuplicateWithNew);
            else if (mi is MethodDefinition)
            {
                MethodDefinition mb = (MethodDefinition)mi;
                IList<ParameterDefinition> parameters = mb.Parameters;
                MakeParameters (root, mi, parameters, shouldDuplicateWithNew);
                if (parameters.Count > 0 && DocUtils.IsExtensionMethod (mb))
                {
                    XmlElement p = (XmlElement)root.SelectSingleNode ("Parameters/Parameter[position()=1]");
                    p.SetAttribute ("RefType", "this");
                }
            }
            else if (mi is PropertyDefinition)
            {
                IList<ParameterDefinition> parameters = ((PropertyDefinition)mi).Parameters;
                if (parameters.Count > 0)
                    MakeParameters (root, mi, parameters, shouldDuplicateWithNew);
                else
                    return;
            }
            else if (mi is FieldDefinition) return;
            else if (mi is EventDefinition) return;
            else throw new ArgumentException ();
        }

        internal static string GetDocParameterType (TypeReference type)
        {
            return GetDocTypeFullName (type).Replace ("@", "&");
        }

        private void MakeReturnValue (XmlElement root, TypeReference type, IList<CustomAttribute> attributes, bool shouldDuplicateWithNew = false)
        {
            XmlElement e = WriteElement (root, "ReturnValue");
            var valueToUse = GetDocTypeFullName (type);

            AddXmlNode (e.SelectNodes ("ReturnType").Cast<XmlElement> ().ToArray (),
                x => x.InnerText == valueToUse,
                x => x.InnerText = valueToUse,
                () =>
                {
                    var newNode = WriteElementText (e, "ReturnType", valueToUse, forceNewElement: true);
                    if (attributes != null)
                        MakeAttributes (e, GetCustomAttributes (attributes, ""), type);

                    return newNode;
                },
            type);
        }

        private void MakeReturnValue (XmlElement root, MemberReference mi, bool shouldDuplicateWithNew = false)
        {
            if (mi is MethodDefinition && ((MethodDefinition)mi).IsConstructor)
                return;
            else if (mi is MethodDefinition)
                MakeReturnValue (root, ((MethodDefinition)mi).ReturnType, ((MethodDefinition)mi).MethodReturnType.CustomAttributes, shouldDuplicateWithNew);
            else if (mi is PropertyDefinition)
                MakeReturnValue (root, ((PropertyDefinition)mi).PropertyType, null, shouldDuplicateWithNew);
            else if (mi is FieldDefinition)
                MakeReturnValue (root, ((FieldDefinition)mi).FieldType, null, shouldDuplicateWithNew);
            else if (mi is EventDefinition)
                MakeReturnValue (root, ((EventDefinition)mi).EventType, null, shouldDuplicateWithNew);
            else
                throw new ArgumentException (mi + " is a " + mi.GetType ().FullName);
        }

        private XmlElement MakeMember (XmlDocument doc, DocsNodeInfo info, XmlNode members, FrameworkTypeEntry typeEntry)
        {
            MemberReference mi = info.Member;
            if (mi is TypeDefinition) return null;

            string sigs = memberFormatters[0].GetDeclaration (mi);
            if (sigs == null) return null; // not publicly visible

            // no documentation for property/event accessors.  Is there a better way of doing this?
            if (mi.Name.StartsWith ("get_", StringComparison.Ordinal)) return null;
            if (mi.Name.StartsWith ("set_", StringComparison.Ordinal)) return null;
            if (mi.Name.StartsWith ("add_", StringComparison.Ordinal)) return null;
            if (mi.Name.StartsWith ("remove_", StringComparison.Ordinal)) return null;
            if (mi.Name.StartsWith ("raise_", StringComparison.Ordinal)) return null;

            XmlElement me = doc.CreateElement ("Member");
            members.AppendChild (me);
            me.SetAttribute ("MemberName", GetMemberName (mi));

            info.Node = me;
            UpdateMember (info, typeEntry);
            if (exceptions.HasValue &&
                    (exceptions.Value & ExceptionLocations.AddedMembers) != 0)
                UpdateExceptions (info.Node, info.Member);

            if (since != null)
            {
                XmlNode docs = me.SelectSingleNode ("Docs");
                docs.AppendChild (CreateSinceNode (doc));
            }

            return me;
        }

        internal static string GetMemberName (MemberReference mi)
        {
            MethodDefinition mb = mi as MethodDefinition;
            if (mb == null)
            {
                PropertyDefinition pi = mi as PropertyDefinition;
                if (pi == null)
                    return mi.Name;
                return DocUtils.GetPropertyName (pi);
            }
            StringBuilder sb = new StringBuilder (mi.Name.Length);
            if (!DocUtils.IsExplicitlyImplemented (mb))
                sb.Append (mi.Name);
            else
            {
                TypeReference iface;
                MethodReference ifaceMethod;
                DocUtils.GetInfoForExplicitlyImplementedMethod (mb, out iface, out ifaceMethod);
                sb.Append (GetDocTypeFullName (iface));
                sb.Append ('.');
                sb.Append (ifaceMethod.Name);
            }
            if (mb.IsGenericMethod ())
            {
                IList<GenericParameter> typeParams = mb.GenericParameters;
                if (typeParams.Count > 0)
                {
                    sb.Append ("<");
                    sb.Append (typeParams[0].Name);
                    for (int i = 1; i < typeParams.Count; ++i)
                        sb.Append (",").Append (typeParams[i].Name);
                    sb.Append (">");
                }
            }
            return sb.ToString ();
        }

        /// SIGNATURE GENERATION FUNCTIONS
        internal static bool IsPrivate (MemberReference mi)
        {
            return memberFormatters[0].GetDeclaration (mi) == null;
        }

        internal static string GetMemberType (MemberReference mi)
        {
            if (mi is MethodDefinition && ((MethodDefinition)mi).IsConstructor)
                return "Constructor";
            if (mi is MethodDefinition)
                return "Method";
            if (mi is PropertyDefinition)
                return "Property";
            if (mi is FieldDefinition)
                return "Field";
            if (mi is EventDefinition)
                return "Event";
            throw new ArgumentException ();
        }

        private static string GetDocTypeName (TypeReference type)
        {
            return docTypeFormatter.GetName (type);
        }

        internal static string GetDocTypeFullName (TypeReference type)
        {
            return DocTypeFullMemberFormatter.Default.GetName (type);
        }

        internal static string GetXPathForMember (DocumentationMember member)
        {
            StringBuilder xpath = new StringBuilder ();
            xpath.Append ("//Members/Member[@MemberName=\"")
                .Append (member.MemberName)
                .Append ("\"]");
            if (member.Parameters != null && member.Parameters.Count > 0)
            {
                xpath.Append ("/Parameters[count(Parameter) = ")
                    .Append (member.Parameters.Count);
                for (int i = 0; i < member.Parameters.Count; ++i)
                {
                    xpath.Append (" and Parameter [").Append (i + 1).Append ("]/@Type=\"");
                    xpath.Append (member.Parameters[i]);
                    xpath.Append ("\"");
                }
                xpath.Append ("]/..");
            }
            return xpath.ToString ();
        }

        public static string GetXPathForMember (XPathNavigator member)
        {
            StringBuilder xpath = new StringBuilder ();
            xpath.Append ("//Type[@FullName=\"")
                .Append (member.SelectSingleNode ("../../@FullName").Value)
                .Append ("\"]/");
            xpath.Append ("Members/Member[@MemberName=\"")
                .Append (member.SelectSingleNode ("@MemberName").Value)
                .Append ("\"]");
            XPathNodeIterator parameters = member.Select ("Parameters/Parameter");
            if (parameters.Count > 0)
            {
                xpath.Append ("/Parameters[count(Parameter) = ")
                    .Append (parameters.Count);
                int i = 0;
                while (parameters.MoveNext ())
                {
                    ++i;
                    xpath.Append (" and Parameter [").Append (i).Append ("]/@Type=\"");
                    xpath.Append (parameters.Current.Value);
                    xpath.Append ("\"");
                }
                xpath.Append ("]/..");
            }
            return xpath.ToString ();
        }

        public static string GetXPathForMember (MemberReference member)
        {
            StringBuilder xpath = new StringBuilder ();
            xpath.Append ("//Type[@FullName=\"")
                .Append (member.DeclaringType.FullName)
                .Append ("\"]/");
            xpath.Append ("Members/Member[@MemberName=\"")
                .Append (GetMemberName (member))
                .Append ("\"]");

            IList<ParameterDefinition> parameters = null;
            if (member is MethodDefinition)
                parameters = ((MethodDefinition)member).Parameters;
            else if (member is PropertyDefinition)
            {
                parameters = ((PropertyDefinition)member).Parameters;
            }
            if (parameters != null && parameters.Count > 0)
            {
                xpath.Append ("/Parameters[count(Parameter) = ")
                    .Append (parameters.Count);
                for (int i = 0; i < parameters.Count; ++i)
                {
                    xpath.Append (" and Parameter [").Append (i + 1).Append ("]/@Type=\"");
                    xpath.Append (GetDocParameterType (parameters[i].ParameterType));
                    xpath.Append ("\"");
                }
                xpath.Append ("]/..");
            }
            return xpath.ToString ();
        }
    }
}