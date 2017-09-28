namespace Mono.Documentation.Updater
{
    class CSharpNativeTypeMemberFormatter : CSharpFullMemberFormatter
    {
        protected override string GetCSharpType (string t)
        {
            string moddedType = base.GetCSharpType (t);

            switch (moddedType)
            {
                case "int": return "nint";
                case "uint":
                    return "nuint";
                case "float":
                    return "nfloat";
                case "System.Drawing.SizeF":
                    return "CoreGraphics.CGSize";
                case "System.Drawing.PointF":
                    return "CoreGraphics.CGPoint";
                case "System.Drawing.RectangleF":
                    return "CoreGraphics.CGPoint";
            }
            return null;
        }
    }
}