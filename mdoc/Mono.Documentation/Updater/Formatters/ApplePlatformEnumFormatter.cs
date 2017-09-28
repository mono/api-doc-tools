using System;
using System.Collections.Generic;
using System.Linq;

using Mono.Cecil;

namespace Mono.Documentation.Updater
{
    /// <summary>A custom formatter for the ObjCRuntime.Platform enumeration.</summary>
    class ApplePlatformEnumFormatter : AttributeValueFormatter
    {
        public override bool TryFormatValue (object v, ResolvedTypeInfo type, out string returnvalue)
        {
            TypeReference valueType = type.Reference;
            string typename = MDocUpdater.GetDocTypeFullName (valueType);
            TypeDefinition valueDef = type.Definition;
            if (typename.Contains ("ObjCRuntime.Platform") && valueDef.CustomAttributes.Any (ca => ca.AttributeType.FullName == "System.FlagsAttribute"))
            {

                var values = MDocUpdater.GetEnumerationValues (valueDef);
                long c = MDocUpdater.ToInt64 (v);

                returnvalue = Format (c, values, typename);
                return true;
            }

            returnvalue = null;
            return false;
        }

        string Format (long c, IDictionary<long, string> values, string typename)
        {
            int iosarch, iosmajor, iosminor, iossubminor;
            int macarch, macmajor, macminor, macsubminor;
            GetEncodingiOS (c, out iosarch, out iosmajor, out iosminor, out iossubminor);
            GetEncodingMac ((ulong)c, out macarch, out macmajor, out macminor, out macsubminor);

            if (iosmajor == 0 & iosminor == 0 && iossubminor == 0)
            {
                return FormatValues ("Mac", macarch, macmajor, macminor, macsubminor);
            }

            if (macmajor == 0 & macminor == 0 && macsubminor == 0)
            {
                return FormatValues ("iOS", iosarch, iosmajor, iosminor, iossubminor);
            }

            return string.Format ("(Platform){0}", c);
        }

        string FormatValues (string plat, int arch, int major, int minor, int subminor)
        {
            string archstring = "";
            switch (arch)
            {
                case 1:
                    archstring = "32";
                    break;
                case 2:
                    archstring = "64";
                    break;
            }
            return string.Format ("Platform.{4}_{0}_{1}{2} | Platform.{4}_Arch{3}",
                major,
                minor,
                subminor == 0 ? "" : "_" + subminor.ToString (),
                archstring,
                plat
            );
        }

        void GetEncodingiOS (long entireLong, out int archindex, out int major, out int minor, out int subminor)
        {
            long lowerBits = entireLong & 0xffffffff;
            int lowerBitsAsInt = (int)lowerBits;
            GetEncoding (lowerBitsAsInt, out archindex, out major, out minor, out subminor);
        }

        void GetEncodingMac (ulong entireLong, out int archindex, out int major, out int minor, out int subminor)
        {
            ulong higherBits = entireLong & 0xffffffff00000000;
            int higherBitsAsInt = (int)((higherBits) >> 32);
            GetEncoding (higherBitsAsInt, out archindex, out major, out minor, out subminor);
        }

        void GetEncoding (Int32 encodedBits, out int archindex, out int major, out int minor, out int subminor)
        {
            // format is AAJJNNSS
            archindex = (int)((encodedBits & 0xFF000000) >> 24);
            major = (int)((encodedBits & 0x00FF0000) >> 16);
            minor = (int)((encodedBits & 0x0000FF00) >> 8);
            subminor = (int)((encodedBits & 0x000000FF) >> 0);
        }
    }
}