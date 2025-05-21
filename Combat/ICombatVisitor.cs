using Projekt1;
using Projekt1.Items;

namespace Projekt1.Combat
{
    public interface ICombatVisitor
    {
        void VisitPlayer(Player player);
        void VisitEnemy(Enemy enemy);
        void VisitHeavyWeapon(Weapon weapon);
        void VisitLightWeapon(Weapon weapon);
        void VisitMagicWeapon(Weapon weapon);
        void VisitOtherItem(BaseItem item);
    }
}
