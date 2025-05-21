namespace Projekt1.Items
{
    public class Currency : BaseItem
    {
        public int Amount { get; set; }

        public Currency(string name, char symbol, int amount)
            : base(name, symbol)
        {
            Amount = amount;
        }

        public override int GetCurrencyAmount() => Amount;
        public override string ToString() => $"{Name} x{Amount}";
        public override int RequiredHands => 0;
    }

    public class Coin : Currency
    {
        public Coin(int amount) : base("Moneta", 'C', amount) { }

        public override void ApplyPickup(Player player, Room room)
        {
            player.Coins += GetCurrencyAmount();
            room.RemoveItem(player.PosY, player.PosX, this);
        }
    }

    public class Gold : Currency
    {
        public Gold(int amount) : base("Złoto", 'Z', amount) { }

        public override void ApplyPickup(Player player, Room room)
        {
            player.Gold += GetCurrencyAmount();
            room.RemoveItem(player.PosY, player.PosX, this);
        }
    }
}
