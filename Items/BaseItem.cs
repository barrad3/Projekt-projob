using Projekt1.Combat;
using System;

namespace Projekt1.Items
{
    public class BaseItem : IItem, ICombatElement
    {
        public string Name { get; set; }
        public char Symbol { get; set; }
        public int Damage { get; set; }
        public bool IsElixir { get; set; } = false;

        public BaseItem(string name, char symbol, int damage = 0)
        {
            Name = name;
            Symbol = symbol;
            Damage = damage;
        }

        public virtual string GetName() => Name;
        public virtual int GetDamage() => Damage;
        public virtual void OnEquip(Player player) { }
        public virtual void OnUnequip(Player player) { }
        public virtual int GetCurrencyAmount() => 0;

        public virtual int RequiredHands => 1;


        public override string ToString() => $"{Name} ({Symbol})";

        public virtual void IncreaseDamage(int bonus) { }
        public virtual void DecreaseDamage(int bonus) { }
        public virtual void ApplyPickup(Player player, Room room) 
        {
            player.Backpack.Add(this);
            room.RemoveItem(player.PosY, player.PosX, this);
        }

        public virtual void Accept(ICombatVisitor visitor) => visitor.VisitOtherItem(this);
    }
}
