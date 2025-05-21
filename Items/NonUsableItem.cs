namespace Projekt1.Items
{
    public class NonUsableItem : BaseItem
    {
        public NonUsableItem(string name, char symbol)
            : base(name, symbol)
        { }
        public override int RequiredHands => 0;
    }

    public class Statue : NonUsableItem
    {
        public Statue() : base("Posąg", 'P') { }
    }

    public class Painting : NonUsableItem
    {
        public Painting() : base("Obraz", 'O') { }
    }

    public class Vase : NonUsableItem
    {
        public Vase() : base("Wazon", 'W') { }
    }
}
