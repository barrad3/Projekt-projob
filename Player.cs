using Projekt1.Combat;
using Projekt1.Effects;
using Projekt1.Items;
using Projekt1.UI;
using System;
using System.Collections.Generic;

namespace Projekt1
{
    public class Player : ICombatElement
    {
        public int PosX { get; set; }
        public int PosY { get; set; }

        public int Strength { get; set; }
        public int Agility { get; set; }
        public int Health { get; set; }
        public int Luck { get; set; }
        public int Aggression { get; set; }
        public int Wisdom { get; set; }

        public const int BaseStrength = 10;
        public const int BaseAgility = 10;
        public const int BaseHealth = 100;
        public const int BaseLuck = 10;
        public const int BaseAggression = 10;
        public const int BaseWisdom = 10;

        public int Coins { get; set; }
        public int Gold { get; set; }

        public IItem? LeftHand { get; set; }
        public IItem? RightHand { get; set; }
        public List<IItem> Backpack { get; set; }

        public Player()
        {
            PosX = 0;
            PosY = 0;

            Strength = BaseStrength;
            Agility = BaseAgility;
            Health = BaseHealth;
            Luck = BaseLuck;
            Aggression = BaseAggression;
            Wisdom = BaseWisdom;

            Coins = 0;
            Gold = 0;

            Backpack = new List<IItem>();
        }

        public void Move(char direction, Room room)
        {
            int newX = PosX;
            int newY = PosY;

            switch (direction)
            {
                case 'W':
                case 'w':
                    newY--;
                    break;
                case 'S':
                case 's':
                    newY++;
                    break;
                case 'A':
                case 'a':
                    newX--;
                    break;
                case 'D':
                case 'd':
                    newX++;
                    break;
            }

            if (room.IsWalkable(newY, newX))
            {
                PosX = newX;
                PosY = newY;
            }
        }

        public void Pickup(Room room)
        {
            var cellItems = room.GetItems(PosY, PosX);
            if (cellItems.Count == 0) return;

            IItem chosenItem = null;
            if (cellItems.Count > 1)
            {
                int selectedIndex = UI.UI.PromptItemSelection(cellItems, room);
                if (selectedIndex < 0 || selectedIndex >= cellItems.Count)
                {
                    UI.UI.ClearInventoryBelowMap(room);
                    return;
                }
                chosenItem = cellItems[selectedIndex];
                UI.UI.ClearInventoryBelowMap(room);
            }
            else
            {
                chosenItem = cellItems[0];
            }

            UI.UI.LogAction($"Picked up {chosenItem}.");
            chosenItem.ApplyPickup(this, room);
        }


        public string EquipItemFromInventory(int backpackIndex, char targetHand)
        {
            if (backpackIndex < 0 || backpackIndex >= Backpack.Count)
                return "Invalid item selection.";

            IItem item = Backpack[backpackIndex];
            bool success = item.Equip(this, targetHand);
            if (success)
            {
                Backpack.RemoveAt(backpackIndex);
                return $"Item equipped in {targetHand} hand.";
            }
            else
            {
                return "Could not equip item.";
            }
        }

        public string DropItemFromInventory(int backpackIndex, Room room)
        {
            if (backpackIndex < 0 || backpackIndex >= Backpack.Count)
                return "Invalid item selection.";

            IItem item = Backpack[backpackIndex];
            room.AddItem(PosY, PosX, item);
            Backpack.RemoveAt(backpackIndex);
            return "Item dropped on the ground.";
        }

        public void UnequipFromHand(char hand)
        {
            if (hand == 'L' || hand == 'l')
            {
                if (LeftHand != null)
                {
                    LeftHand.OnUnequip(this);
                    if (RightHand == LeftHand)
                        RightHand = null;
                    Backpack.Add(LeftHand);
                    LeftHand = null;
                }
            }
            else if (hand == 'R' || hand == 'r')
            {
                if (RightHand != null)
                {
                    RightHand.OnUnequip(this);
                    if (LeftHand == RightHand)
                        LeftHand = null;
                    Backpack.Add(RightHand);
                    RightHand = null;
                }
            }
        }

        public void DrinkElixir(int index)
        {
            if (index < 0 || index >= Backpack.Count || !Backpack[index].IsElixir)
                return;
            Backpack[index].OnEquip(this);
            Backpack.RemoveAt(index);
        }

        public List<IEffect> activeEffects = new List<IEffect>();

        public void AddEffect(IEffect effect)
        {
            effect.Apply(this);
            activeEffects.Add(effect);
        }

        public void RemoveEffect(IEffect effect)
        {
            effect.Remove(this);
            activeEffects.Remove(effect);
        }

        public void RemoveAllEffects()
        {
            foreach (var effect in activeEffects.ToList())
            {
                effect.Remove(this);
            }
            activeEffects.Clear();
        }

        public void UpdateEffects()
        {
            foreach (var effect in activeEffects.ToList())
            {
                effect.Update(this);
                if (effect.IsExpired)
                {
                    activeEffects.Remove(effect);
                }
            }
        }

        public List<string> GetActiveEffectsDescriptions()
        {
            return activeEffects.Select(e => e.Description).ToList();
        }

        public void Accept(ICombatVisitor visitor) => visitor.VisitPlayer(this);

        public void Attack(Enemy enemy, IAttackStrategy strategy, Room room)
        {
            var atkV = strategy.CreateAttackVisitor();
            (RightHand as ICombatElement ?? LeftHand as ICombatElement
             ?? new BaseItem("Fists", ' ', 0))
             .Accept(atkV);
            enemy.Accept(atkV);
            enemy.CurrentHealth = System.Math.Max(0, enemy.CurrentHealth - ((dynamic)atkV).DamageToEnemy);

            var defV = strategy.CreateDefenseVisitor(this);
            enemy.Accept(defV);
            (RightHand as ICombatElement ?? LeftHand as ICombatElement
             ?? new BaseItem("Fists", ' ', 0))
             .Accept(defV);
            this.Accept(defV);
            Health = System.Math.Max(0, Health - ((dynamic)defV).DamageToPlayer);

            if (enemy.CurrentHealth == 0)
            {
                UI.UI.LogAction("Enemy defeated!");
                room.RemoveEnemy(PosY, PosX);
            }
            if (Health == 0)
            {
                UI.UI.LogAction("You died!");
                UI.UI.GameOver();
            }     
        }
    }
}
