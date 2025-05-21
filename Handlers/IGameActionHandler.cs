using System;

namespace Projekt1.Handlers
{
    public interface IGameActionHandler
    {
        IGameActionHandler SetNext(IGameActionHandler handler);
        void Handle(ConsoleKeyInfo keyInfo, Player player, Room room);
    }
}
