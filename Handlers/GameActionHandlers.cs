using Projekt1.Combat;
using Projekt1.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Projekt1.Handlers
{
    public class MovementHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            char key = char.ToLower(keyInfo.KeyChar);
            if ("wasd".Contains(key))
            {
                player.Move(key, room);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class PickupHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            char key = char.ToLower(keyInfo.KeyChar);
            if (key == 'e' && room.GetItems(player.PosY, player.PosX).Count > 0)
            {
                player.Pickup(room);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }


    public class InventoryHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'i' && player.Backpack.Count > 0)
            {
                UI.UI.ShowInventoryBelowMap(player, room);
                string? input = Console.ReadLine();
                if (!int.TryParse(input, out int selectedIndex) || selectedIndex <= 0 || selectedIndex > player.Backpack.Count)
                {
                    UI.UI.ClearInventoryBelowMap(room);
                    return;
                }
                ConsoleKeyInfo subKey = UI.UI.ManageInventory(player, room);
                IGameActionHandler subChain = new EquipRightHandler(selectedIndex);
                subChain.SetNext(new EquipLeftHandler(selectedIndex))
                        .SetNext(new DropItemHandler(selectedIndex));

                subChain.Handle(subKey, player, room);
                UI.UI.ClearInventoryBelowMap(room);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }


    public class InventorySubActionHandler : GameActionHandlerBase
    {
        protected int SelectedIndex { get; }

        public InventorySubActionHandler(int selectedIndex)
        {
            SelectedIndex = selectedIndex;
        }
    }

    public class EquipRightHandler : InventorySubActionHandler
    {
        public EquipRightHandler(int selectedIndex) : base(selectedIndex)
        {
        }

        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'r')
            {
                string message = player.EquipItemFromInventory(SelectedIndex - 1, 'R');
                Console.WriteLine(message);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class EquipLeftHandler : InventorySubActionHandler
    {
        public EquipLeftHandler(int selectedIndex) : base(selectedIndex)
        {
        }

        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'l')
            {
                string message = player.EquipItemFromInventory(SelectedIndex - 1, 'L');
                UI.UI.LogAction(message);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class DropItemHandler : InventorySubActionHandler
    {
        public DropItemHandler(int selectedIndex) : base(selectedIndex)
        {
        }

        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'd')
            {
                string message = player.DropItemFromInventory(SelectedIndex - 1, room);
                UI.UI.LogAction(message);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class UnEquipFromRightHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (keyInfo.KeyChar == 'r')
            {
                player.UnequipFromHand(keyInfo.KeyChar);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class UnEquipFromLeftHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (keyInfo.KeyChar == 'l')
            {
                player.UnequipFromHand(keyInfo.KeyChar);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }


    public class DrinkElixirHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            char key = char.ToLower(keyInfo.KeyChar);
            if (key == 'p' && player.Backpack.Any(item => item.IsElixir))
            {
                UI.UI.WriteMessage("Podaj indeks eliksiru do wypicia: ");
                string? input = Console.ReadLine();
                if (int.TryParse(input, out int index))
                {
                    player.DrinkElixir(index - 1);
                }
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class DropAllItemsHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (keyInfo.KeyChar == 'b' && player.Backpack.Count > 0)
            {
                foreach (var item in player.Backpack.ToList())
                {
                    player.DropItemFromInventory(player.Backpack.IndexOf(item), room);
                }
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }


    public class WrongKeyHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            string message = "Naciśnięto niepoprawny klawisz.";
            UI.UI.LogAction(message);
        }
    }

    public class ExitGameHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (keyInfo.KeyChar == '.')
            {
                Environment.Exit(0);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class NormalAttackHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'z')
            {
                var enemy = room.GetEnemy(player.PosY, player.PosX);
                player.Attack(enemy, new NormalAttackStrategy(), room);
                UI.UI.LogAction($"Atak zwykły! HP wroga: {enemy.CurrentHealth}/{enemy.MaxHealth}");
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class StealthAttackHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'x')
            {
                var enemy = room.GetEnemy(player.PosY, player.PosX);
                player.Attack(enemy, new StealthAttackStrategy(), room);
                UI.UI.LogAction($"Atak skryty! HP wroga: {enemy.CurrentHealth}/{enemy.MaxHealth}");
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class MagicAttackHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (char.ToLower(keyInfo.KeyChar) == 'c')
            {
                var enemy = room.GetEnemy(player.PosY, player.PosX);
                player.Attack(enemy, new MagicAttackStrategy(), room);
                UI.UI.LogAction($"Atak magiczny! HP wroga: {enemy.CurrentHealth}/{enemy.MaxHealth}");
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }

    public class AttackHandler : GameActionHandlerBase
    {
        public override void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            char key = char.ToLower(keyInfo.KeyChar);
            var enemy = room.GetEnemy(player.PosY, player.PosX);

            if ((key == 'z' || key == 'x' || key == 'c') && enemy != null)
            {
                var head = new NormalAttackHandler();
                head
                    .SetNext(new StealthAttackHandler())
                    .SetNext(new MagicAttackHandler())
                    .SetNext(new WrongKeyHandler());

                head.Handle(keyInfo, player, room);
            }
            else
            {
                base.Handle(keyInfo, player, room);
            }
        }
    }
}
