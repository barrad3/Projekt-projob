using Projekt1.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Combat
{
    public class NormalDefenseVisitor : ICombatVisitor
    {
        private readonly Player _player;
        private int _enemyAttack;
        private int _defense;
        public int DamageToPlayer { get; private set; }

        public NormalDefenseVisitor(Player player) => _player = player;

        public void VisitEnemy(Enemy e) => _enemyAttack = e.Strength;
        public void VisitPlayer(Player _) => DamageToPlayer = Math.Max(0, _enemyAttack - _defense);

        public void VisitHeavyWeapon(Weapon _) => _defense = _player.Strength + _player.Luck;
        public void VisitLightWeapon(Weapon _) => _defense = _player.Agility + _player.Luck;
        public void VisitMagicWeapon(Weapon _) => _defense = _player.Agility + _player.Luck;
        public void VisitOtherItem(BaseItem _) => _defense = _player.Agility;
    }

    public class StealthDefenseVisitor : ICombatVisitor
    {
        private readonly Player _player;
        private int _enemyAttack;
        private int _defense;
        public int DamageToPlayer { get; private set; }

        public StealthDefenseVisitor(Player player) => _player = player;

        public void VisitEnemy(Enemy e) => _enemyAttack = e.Strength;
        public void VisitPlayer(Player _) => DamageToPlayer = Math.Max(0, _enemyAttack - _defense);

        public void VisitHeavyWeapon(Weapon _) => _defense = _player.Strength;
        public void VisitLightWeapon(Weapon _) => _defense = _player.Agility;
        public void VisitMagicWeapon(Weapon _) => _defense = 0;
        public void VisitOtherItem(BaseItem _) => _defense = 0;
    }

    public class MagicDefenseVisitor : ICombatVisitor
    {
        private readonly Player _player;
        private int _enemyAttack;
        private int _defense;
        public int DamageToPlayer { get; private set; }

        public MagicDefenseVisitor(Player player) => _player = player;

        public void VisitEnemy(Enemy e) => _enemyAttack = e.Strength;
        public void VisitPlayer(Player _) => DamageToPlayer = Math.Max(0, _enemyAttack - _defense);

        public void VisitHeavyWeapon(Weapon _) => _defense = _player.Luck;
        public void VisitLightWeapon(Weapon _) => _defense = _player.Luck;
        public void VisitMagicWeapon(Weapon _) => _defense = _player.Wisdom * 2;
        public void VisitOtherItem(BaseItem _) => _defense = _player.Luck;
    }
}
