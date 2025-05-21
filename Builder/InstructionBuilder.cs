using System;
using System.Collections.Generic;

namespace Projekt1.Builder
{
    public class InstructionsBuilder : IBuilder
    {
        private List<string> instructions = new List<string>();

        public IBuilder BuildMovementInstructions(Room room, Player player)
        {
            instructions.Add("W, A, S, D to move.");
            return this;
        }

        public IBuilder BuildPickupInstructions(Room room, Player player)
        {
            if (room.GetItems(player.PosY, player.PosX).Count > 0)
            {
                instructions.Add("E to pick up items.");
            }
            if (player.Backpack.Count > 0)
            {
                instructions.Add("I to manage inventory.");
            }
            return this;
        }

        public IBuilder BuildEnemyInstructions(Room room, Player player)
        {
            if (room.GetEnemy(player.PosY, player.PosX) != null)
            {
                instructions.Add("Beware of enemies!");
            }
            return this;
        }

        public IBuilder BuildGeneralInstructions(Room room, Player player)
        {
            if (player.LeftHand != null || player.RightHand != null)
            {
                instructions.Add("R or L to unequip items.");
            }
            instructions.Add(". to quit the game.");
            return this;
        }

        public IBuilder DropAllItemsInstructions(Room room, Player player)
        {
            if (player.Backpack.Count > 0)
            {
                instructions.Add("B to drop all items.");
            }
            return this;
        }

        public IBuilder AttackInstructions(Room room, Player player)
        {
            if (room.GetEnemy(player.PosY, player.PosX) != null)
            {
                instructions.Add("Z to Normal Attack.\nX to Stealth Attack.\nC to Magic Attack.");
            }
            return this;
        }

        public List<string> GetInstructions(Room room, Player player)
        {
            return new List<string>(instructions);
        }

        public IBuilder SetEmptyDungeon() { return this; }
        public IBuilder SetFilledDungeon() { return this; }
        public IBuilder AddPaths() { return this; }
        public IBuilder AddChambers() { return this; }
        public IBuilder AddCentralRoom() { return this; }
        public IBuilder AddItems() { return this; }
        public IBuilder AddWeapons() { return this; }
        public IBuilder AddModifiedWeapons() { return this; }
        public IBuilder AddElixirs() { return this; }
        public IBuilder AddEnemies() { return this; }
        public IBuilder AddCurrency() { return this; }
        public Room GetRoom() { return null; }
    }
}
