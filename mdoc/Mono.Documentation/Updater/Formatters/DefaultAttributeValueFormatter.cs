namespace Mono.Documentation.Updater
{
    /// <summary>The final value formatter in the pipeline ... if no other formatter formats the value,
    /// then this one will serve as the default implementation.</summary>
    class DefaultAttributeValueFormatter : AttributeValueFormatter
    {
        public override bool TryFormatValue (object v, ResolvedTypeInfo type, out string returnvalue)
        {
            returnvalue = "(" + MDocUpdater.GetDocTypeFullName (type.Reference) + ") " + MDocUpdater.FilterSpecialChars(v.ToString ());
            return true;
        }
    }
}