using System.Collections.Generic;

namespace Projekt1.Builder
{
    public class Director
    {
        public IBuilder Builder { get; set; }

        public void BuildBasicDungeon(char c)
        {
            switch (c)
            {
                case 'f':
                    Builder.SetFilledDungeon();
                    break;
                case 'e':
                    Builder.SetEmptyDungeon();
                    break;
            }
        }

        public void BuildDungeon()
        {
            Builder.AddPaths();
            Builder.AddChambers();
            Builder.AddCentralRoom();
            Builder.AddItems();
            Builder.AddItems();
            Builder.AddWeapons();
            Builder.AddWeapons();
            Builder.AddModifiedWeapons();
            Builder.AddElixirs();
            Builder.AddEnemies();
            Builder.AddCurrency();
        }

        public List<string> BuildInstructions(Room room, Player player)
        {
            return Builder
                .BuildMovementInstructions(room,player)
                .BuildPickupInstructions(room,player)
                .DropAllItemsInstructions(room, player)
                .BuildEnemyInstructions(room,player)
                .BuildGeneralInstructions(room,player)
                .AttackInstructions(room, player)
                .GetInstructions(room,player);
        }
    }
}
