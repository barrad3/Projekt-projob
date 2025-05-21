using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Combat
{
    public class NormalAttackStrategy : IAttackStrategy
    {
        public ICombatVisitor CreateAttackVisitor()
            => new NormalAttackVisitor();

        public ICombatVisitor CreateDefenseVisitor(Player player)
            => new NormalDefenseVisitor(player);
    }

    public class StealthAttackStrategy : IAttackStrategy
    {
        public ICombatVisitor CreateAttackVisitor()
            => new StealthAttackVisitor();

        public ICombatVisitor CreateDefenseVisitor(Player player)
            => new StealthDefenseVisitor(player);
    }

    public class MagicAttackStrategy : IAttackStrategy
    {
        public ICombatVisitor CreateAttackVisitor()
            => new MagicAttackVisitor();

        public ICombatVisitor CreateDefenseVisitor(Player player)
            => new MagicDefenseVisitor(player);
    }
}
