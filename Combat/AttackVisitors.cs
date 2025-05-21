using Projekt1.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Combat
{
    public class NormalAttackVisitor : ICombatVisitor
    {
        private int _weaponDamage;
        public int DamageToEnemy { get; private set; }

        public void VisitHeavyWeapon(Weapon w) => _weaponDamage = w.Damage;
        public void VisitLightWeapon(Weapon w) => _weaponDamage = w.Damage;
        public void VisitMagicWeapon(Weapon w) => _weaponDamage = 1;
        public void VisitOtherItem(BaseItem _) => _weaponDamage = 0;
        public void VisitEnemy(Enemy e) => DamageToEnemy = Math.Max(0, _weaponDamage - e.Armor);
        public void VisitPlayer(Player _) { }
    }

    public class StealthAttackVisitor : ICombatVisitor
    {
        private int _weaponDamage;
        public int DamageToEnemy { get; private set; }

        public void VisitLightWeapon(Weapon w) => _weaponDamage = w.Damage * 2;
        public void VisitHeavyWeapon(Weapon w) => _weaponDamage = w.Damage / 2;
        public void VisitMagicWeapon(Weapon w) => _weaponDamage = 1;
        public void VisitOtherItem(BaseItem _) => _weaponDamage = 0;
        public void VisitEnemy(Enemy e) => DamageToEnemy = Math.Max(0, _weaponDamage - e.Armor);
        public void VisitPlayer(Player _) { }
    }

    public class MagicAttackVisitor : ICombatVisitor
    {
        private int _weaponDamage;
        public int DamageToEnemy { get; private set; }

        public void VisitMagicWeapon(Weapon w) => _weaponDamage = w.Damage;
        public void VisitHeavyWeapon(Weapon w) => _weaponDamage = 1;
        public void VisitLightWeapon(Weapon w) => _weaponDamage = 1;
        public void VisitOtherItem(BaseItem _) => _weaponDamage = 0;
        public void VisitEnemy(Enemy e) => DamageToEnemy = Math.Max(0, _weaponDamage - e.Armor);
        public void VisitPlayer(Player _) { }
    }
}
