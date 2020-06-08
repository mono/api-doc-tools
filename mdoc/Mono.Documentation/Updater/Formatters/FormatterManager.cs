using Mono.Documentation.Updater.Formatters.CppFormatters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Documentation.Updater.Formatters
{
    static class FormatterManager
    {
        public static List<MemberFormatter> TypeFormatters { get; private set; } = new List<MemberFormatter>{
                        new CSharpMemberFormatter(MDocUpdater.Instance.TypeMap),
                        new ILMemberFormatter(MDocUpdater.Instance.TypeMap),
                    };

        public static List<MemberFormatter> MemberFormatters { get; private set; } = new List<MemberFormatter>{
                        new CSharpFullMemberFormatter (MDocUpdater.Instance.TypeMap),
                        new ILFullMemberFormatter (MDocUpdater.Instance.TypeMap),
                    };

        public static DocIdFormatter DocIdFormatter { get; private set; } = new DocIdFormatter(MDocUpdater.Instance.TypeMap);

        public static void AddFormatter(string langId)
        {
            MemberFormatter memberFormatter;
            MemberFormatter typeFormatter;
            langId = langId.ToLower();
            var map = MDocUpdater.Instance.TypeMap;
            switch (langId)
            {
                case Consts.DocIdLowCase:
                    typeFormatter = DocIdFormatter;
                    memberFormatter = DocIdFormatter;
                    break;
                case Consts.VbNetLowCase:
                    typeFormatter = new VBMemberFormatter(map);
                    memberFormatter = new VBMemberFormatter(map);
                    break;
                case Consts.CppCliLowCase:
                    typeFormatter = new CppMemberFormatter(map);
                    memberFormatter = new CppFullMemberFormatter(map);
                    break;
                case Consts.CppCxLowCase:
                    typeFormatter = new CppCxMemberFormatter(map);
                    memberFormatter = new CppCxFullMemberFormatter(map);
                    break;
                case Consts.CppWinRtLowCase:
                    typeFormatter = new CppWinRtMemberFormatter(map);
                    memberFormatter = new CppWinRtFullMemberFormatter(map);
                    break;
                case Consts.FSharpLowCase:
                case "fsharp":
                    typeFormatter = new FSharpMemberFormatter(map);
                    memberFormatter = new FSharpFullMemberFormatter(map);
                    break;
                case Consts.JavascriptLowCase:
                    typeFormatter = new JsMemberFormatter(map);
                    memberFormatter = new JsMemberFormatter(map);
                    break;
                default:
                    throw new ArgumentException("Unsupported formatter id '" + langId + "'.");
            }
            TypeFormatters.Add(typeFormatter);
            MemberFormatters.Add(memberFormatter);
        }
    }
}
