namespace TestInterfaceImplementation
{
    public interface IScorable<Item, Score>
    {
        Score GetScore(Item item, object state);
    }
}