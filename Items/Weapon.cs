using Projekt1.Combat;
using System;

namespace Projekt1.Items
{
    public class Weapon : BaseItem
    {
        public Weapon(string name, char symbol, int damage)
            : base(name, symbol, damage)
        {
        }

        public override void IncreaseDamage(int bonus)
        {
            Damage += bonus;
        }

        public override void DecreaseDamage(int bonus)
        {
            Damage -= bonus;
        }
        public override string ToString() => $"{Name} DMG: {Damage}";
    }

    public class TwoHandedWeapon : Weapon
    {
        public TwoHandedWeapon() : base("Topór Dwuręczny", 'T', 15) { }
        public override int RequiredHands => 2;
        public override void Accept(ICombatVisitor visitor) => visitor.VisitHeavyWeapon(this);
    }

    public class Sword : Weapon
    {
        public Sword() : base("Miecz", 'M', 10) { }
        public override void Accept(ICombatVisitor visitor) => visitor.VisitLightWeapon(this);
    }

    public class Axe : Weapon
    {
        public Axe() : base("Siekiera", 'S', 12) { }
        public override void Accept(ICombatVisitor visitor) => visitor.VisitHeavyWeapon(this);
    }
    public class MagicWand : Weapon
    {
        public MagicWand() : base("Różdżka", 'W', 8) { }

        public override void Accept(ICombatVisitor visitor) => visitor.VisitMagicWeapon(this);
    }
}
