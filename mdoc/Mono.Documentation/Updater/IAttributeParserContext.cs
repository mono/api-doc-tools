namespace Mono.Documentation.Updater
{
    public interface IAttributeParserContext
    {
        void MoveToNextDynamicFlag();
        bool IsDynamic();
        bool IsNullable();
    }
}
