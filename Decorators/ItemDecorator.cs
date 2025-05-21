using Projekt1.Items;

namespace Projekt1.Decorators
{
    public abstract class ItemDecorator : IItem
    {
        protected IItem WrappedItem { get; set; }

        public ItemDecorator(IItem item)
        {
            WrappedItem = item;
        }

        public virtual string GetName() => WrappedItem.GetName();
        public virtual int GetDamage() => WrappedItem.GetDamage();
        public virtual void OnEquip(Player player) => WrappedItem.OnEquip(player);
        public virtual void OnUnequip(Player player) => WrappedItem.OnUnequip(player);
        public virtual int GetCurrencyAmount() => WrappedItem.GetCurrencyAmount();
        public virtual void IncreaseDamage(int bonus) => WrappedItem.IncreaseDamage(bonus);
        public virtual void DecreaseDamage(int bonus) => WrappedItem.DecreaseDamage(bonus);
        public virtual void ApplyPickup(Player player, Room room)
        {
            player.Backpack.Add(this);
            room.RemoveItem(player.PosY, player.PosX, this);
        }
        public virtual char Symbol => WrappedItem.Symbol;
        public string Name => WrappedItem.Name;
        public int Damage => WrappedItem.Damage;
        public virtual int RequiredHands => WrappedItem.RequiredHands;

        public bool IsElixir => WrappedItem.IsElixir;

        public override string ToString() => GetName();
    }
}
