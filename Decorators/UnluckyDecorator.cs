using Projekt1.Items;

namespace Projekt1.Decorators
{
    public class UnluckyDecorator : ItemDecorator
    {
        private int luckPenalty = 5;
        private string decoratedName;

        public UnluckyDecorator(IItem item)
            : base(item)
        {
            decoratedName = $"{WrappedItem.GetName()} (Pechowy)";
        }
        public override string GetName() => decoratedName;

        public override void OnEquip(Player player)
        {
            base.OnEquip(player);
            player.Luck -= luckPenalty;
        }

        public override void OnUnequip(Player player)
        {
            base.OnUnequip(player);
            player.Luck += luckPenalty;
        }

        public override string ToString() => $"{WrappedItem.ToString()} (Pechowy: -{luckPenalty} LUCK)";
    }
}
