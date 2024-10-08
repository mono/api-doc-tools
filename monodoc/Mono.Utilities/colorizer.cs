using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mono.Utilities
{
    public class Colorizer
    {
        //
        // Syntax coloring
        //
        static string[] words = { "abstract","event","new","struct","as","explicit","null","switch","base","extern",
            "object","this","bool","false","operator","throw","break","finally","out","true",
            "byte","fixed","override","try","case","float","params","typeof","catch","for",
            "private","uint","char","foreach","protected","ulong","checked","goto","public",
            "unchecked","class","if","readonly","unsafe","const","implicit","ref","ushort",
            "continue","in","return","using","decimal","int","sbyte","virtual","default",
            "interface","sealed","volatile","delegate","internal","short","void","do","is",
            "sizeof","while","double","lock","stackalloc","else","long","static","enum",
            "namespace","string" };
        static string boundary = @"\b";
        static string keywords_cs = "(" + string.Join("|", words.Select(word => $"{boundary}{word}{boundary}")) + ")";

#if false
// currently not in use
		static string keywords_vb =
			"(\\bAddHandler\\b|\\bAddressOf\\b|\\bAlias\\b|\\bAnd\\b|\\bAndAlso\\b|\\bAnsi\\b|\\bAs\\b|\\bAssembly\\b|"
			+
			"\\bAuto\\b|\\bBoolean\\b|\\bByRef\\b|\\bByte\\b|\\bByVal\\b|\\bCall\\b|\\bCase\\b|\\bCatch\\b|"
			+
			"\\bCBool\\b|\\bCByte\\b|\\bCChar\\b|\\bCDate\\b|\\bCDec\\b|\\bCDbl\\b|\\bChar\\b|\\bCInt\\b|"
			+
			"\\bClass\\b|\\bCLng\\b|\\bCObj\\b|\\bConst\\b|\\bCShort\\b|\\bCSng\\b|\\bCStr\\b|\\bCType\\b|"
			+
			"\\bDate\\b|\\bDecimal\\b|\\bDeclare\\b|\\bDefault\\b|\\bDelegate\\b|\\bDim\\b|\\bDirectCast\\b|\\bDo\\b|"
			+
			"\\bDouble\\b|\\bEach\\b|\\bElse\\b|\\bElseIf\\b|\\bEnd\\b|\\bEnum\\b|\\bErase\\b|\\bError\\b|"
			+
			"\\bEvent\\b|\\bExit\\b|\\bFalse\\b|\\bFinally\\b|\\bFor\\b|\\bFriend\\b|\\bFunction\\b|\\bGet\\b|"
			+
			"\\bGetType\\b|\\bGoSub\\b|\\bGoTo\\b|\\bHandles\\b|\\bIf\\b|\\bImplements\\b|\\bImports\\b|\\bIn\\b|"
			+
			"\\bInherits\\b|\\bInteger\\b|\\bInterface\\b|\\bIs\\b|\\bLet\\b|\\bLib\\b|\\bLike\\b|\\bLong\\b|"
			+
			"\\bLoop\\b|\\bMe\\b|\\bMod\\b|\\bModule\\b|\\bMustInherit\\b|\\bMustOverride\\b|\\bMyBase\\b|\\bMyClass\\b|"
			+
			"\\bNamespace\\b|\\bNew\\b|\\bNext\\b|\\bNot\\b|\\bNothing\\b|\\bNotInheritable\\b|\\bNotOverridable\\b|\\bObject\\b|"
			+
			"\\bOn\\b|\\bOption\\b|\\bOptional\\b|\\bOr\\b|\\bOrElse\\b|\\bOverloads\\b|\\bOverridable\\b|\\bOverrides\\b|"
			+
			"\\bParamArray\\b|\\bPreserve\\b|\\bPrivate\\b|\\bProperty\\b|\\bProtected\\b|\\bPublic\\b|\\bRaiseEvent\\b|\\bReadOnly\\b|"
			+
			"\\bReDim\\b|\\bREM\\b|\\bRemoveHandler\\b|\\bResume\\b|\\bReturn\\b|\\bSelect\\b|\\bSet\\b|\\bShadows\\b|"
			+
			"\\bShared\\b|\\bShort\\b|\\bSingle\\b|\\bStatic\\b|\\bStep\\b|\\bStop\\b|\\bString\\b|\\bStructure\\b|"
			+
			"\\bSub\\b|\\bSyncLock\\b|\\bThen\\b|\\bThrow\\b|\\bTo\\b|\\bTrue\\b|\\bTry\\b|\\bTypeOf\\b|"
			+
			"\\bUnicode\\b|\\bUntil\\b|\\bVariant\\b|\\bWhen\\b|\\bWhile\\b|\\bWith\\b|\\bWithEvents\\b|\\bWriteOnly\\b|\\bXor\\b)";
#endif

        public static string Colorize(string text, string lang)
        {
            lang = lang.Trim().ToLower();
            switch (lang)
            {
                case "xml":
                    return ColorizeXml(text);
                case "cs":
                case "c#":
                case "csharp":
                    return ColorizeCs(text);
                case "vb":
                    return ColorizeVb(text);
            }
            return Escape(text);
        }

        static string ColorizeXml(string text)
        {
            // Order is highly important.

            // s/ /&nbsp;/g must be first, as later substitutions add required spaces
            text = text.Replace(" ", "&nbsp;");

            // Find & mark XML elements
            Regex re = new Regex("<\\s*(\\/?)\\s*([\\s\\S]*?)\\s*(\\/?)\\s*>");
            text = re.Replace(text, "{blue:&lt;$1}{maroon:$2}{blue:$3&gt;}");

            // Colorize attribute strings; must be done before colorizing marked XML
            // elements so that we don't clobber the colorized XML tags.
            re = new Regex("([\"'])(.*?)\\1");
            text = re.Replace(text,
                    "$1<font color=\"purple\">$2</font>$1");

            // Colorize marked XML elements
            re = new Regex("\\{(\\w*):([\\s\\S]*?)\\}");
            //text = re.Replace(text, "<span style='color:$1'>$2</span>");
            text = re.Replace(text, "<font color=\"$1\">$2</font>");

            // Standard Structure
            text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            re = new Regex("\r\n|\r|\n");
            text = re.Replace(text, "<br/>");

            return text;
        }

        static string ColorizeCs(string text)
        {
            text = text.Replace(" ", "&nbsp;");

            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");

            Regex re = new Regex("\"((((?!\").)|\\\")*?)\"");

            text =
                re.Replace(text,
                        "<font color=\"purple\">\"$1\"</font>");
            //"<span style='color:purple'>\"$1\"</span>");

            re = new
                Regex
                ("//(((.(?!\"</font>))|\"(((?!\").)*)\"</font>)*)(\r|\n|\r\n)");
            //("//(((.(?!\"</span>))|\"(((?!\").)*)\"</span>)*)(\r|\n|\r\n)");
            text =
                re.Replace(text,
                        "<font color=\"green\">//$1</font><br/>");
            //	"<span style='color:green'>//$1</span><br/>");

            re = new Regex(keywords_cs);
            text = re.Replace(text, "<font color=\"blue\">$1</font>");
            //text = re.Replace(text, "<span style='color:blue'>$1</span>");

            text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            text = text.Replace("\n", "<br/>");

            return text;
        }

        static string ColorizeVb(string text)
        {
            text = text.Replace(" ", "&nbsp;");

            /*	Regex re = new Regex ("\"((((?!\").)|\\\")*?)\"");
				text = re.Replace (text,"<span style='color:purple'>\"$1\"</span>");

				re = new Regex ("'(((.(?!\"\\<\\/span\\>))|\"(((?!\").)*)\"\\<\\/span\\>)*)(\r|\n|\r\n)");
				text = re.Replace (text,"<span style='color:green'>//$1</span><br/>");

				re = new Regex (keywords_vb);
				text = re.Replace (text,"<span style='color:blue'>$1</span>");
			 */
            text = text.Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
            text = text.Replace("\n", "<br/>");
            return text;
        }

        static string Escape(string text)
        {
            text = text.Replace("&", "&amp;");
            text = text.Replace(" ", "&nbsp;");
            text = text.Replace("<", "&lt;");
            text = text.Replace(">", "&gt;");
            text = text.Replace("\n", "<br/>");
            return text;
        }
    }
}
