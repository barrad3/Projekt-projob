using Projekt1.Builder;
using Projekt1.UI;
using Projekt1.Handlers;
using System;

namespace Projekt1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.CursorVisible = false;
            Console.SetWindowSize(120, 40);
            Console.SetBufferSize(120, 40);

            var roomBuilder = new RoomBuilder();
            var director = new Director { Builder = roomBuilder };
            director.BuildBasicDungeon('f');
            director.BuildDungeon();
            Room room = roomBuilder.GetRoom();

            Player player = new Player();

            IGameActionHandler movementHandler = new MovementHandler();
            IGameActionHandler pickupHandler = new PickupHandler();
            IGameActionHandler inventoryHandler = new InventoryHandler();
            IGameActionHandler unEquipFromRightHandler = new UnEquipFromRightHandler();
            IGameActionHandler unEquipFromLeftHandler = new UnEquipFromLeftHandler();
            IGameActionHandler drinkElixirandler = new DrinkElixirHandler();
            IGameActionHandler dropAllItemsHandler = new DropAllItemsHandler();
            IGameActionHandler exitGameHandler = new ExitGameHandler();
            IGameActionHandler attackHandler = new AttackHandler();
            IGameActionHandler wrongKeyHandler = new WrongKeyHandler();

            movementHandler.SetNext(pickupHandler)
                           .SetNext(inventoryHandler)
                           .SetNext(unEquipFromRightHandler)
                           .SetNext(unEquipFromLeftHandler)
                           .SetNext(drinkElixirandler)
                           .SetNext(dropAllItemsHandler)
                           .SetNext(exitGameHandler)
                           .SetNext(attackHandler)
                           .SetNext(wrongKeyHandler);

            while (true)
            {
                UI.UI.Draw(room, player);
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                movementHandler.Handle(keyInfo, player, room);
                player.UpdateEffects();
            }
        }
    }
}
