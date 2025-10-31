namespace TestInterfaceImplementation
{
    public class ScorableBase<Item, State, Score> : IScorable<Item, Score>
    {
        Score IScorable<Item, Score>.GetScore(Item item, object opaque)
        {
            throw new System.NotImplementedException();
        }

    }
}