using Projekt1.Items;

namespace Projekt1.Decorators
{
    public class StrongDecorator : ItemDecorator
    {
        private int bonusDamage = 5;
        private string decoratedName;

        public StrongDecorator(IItem item)
            : base(item)
        {
            decoratedName = $"{item.GetName()} (Silny)";
        }
        public override string GetName() => decoratedName;

        public override void OnEquip(Player player)
        {
            base.OnEquip(player);
            WrappedItem.IncreaseDamage(bonusDamage);
        }

        public override void OnUnequip(Player player)
        {
            base.OnUnequip(player);
            WrappedItem.DecreaseDamage(bonusDamage);
        }

        public override int GetDamage() => WrappedItem.GetDamage();

        public override string ToString() => $"{WrappedItem.ToString()} (Silny +{bonusDamage} DMG)";
    }
}
