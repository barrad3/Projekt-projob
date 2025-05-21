using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Builder
{
    public interface IBuilder
    {
        IBuilder SetEmptyDungeon();
        IBuilder SetFilledDungeon();
        IBuilder AddPaths();
        IBuilder AddChambers();
        IBuilder AddCentralRoom();
        IBuilder AddItems();
        IBuilder AddWeapons();
        IBuilder AddModifiedWeapons();
        IBuilder AddElixirs();
        IBuilder AddEnemies();
        IBuilder AddCurrency();
        Room GetRoom();

        IBuilder BuildMovementInstructions(Room room, Player player);
        IBuilder BuildPickupInstructions(Room room, Player player);
        IBuilder BuildEnemyInstructions(Room room, Player player);
        IBuilder BuildGeneralInstructions(Room room, Player player);
        IBuilder DropAllItemsInstructions(Room room, Player player);
        IBuilder AttackInstructions(Room room, Player player);
        List<string> GetInstructions(Room room, Player player);
    }
}
