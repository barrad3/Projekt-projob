using System;

namespace Projekt1.Handlers
{
    public abstract class GameActionHandlerBase : IGameActionHandler
    {
        private IGameActionHandler? _nextHandler;

        public IGameActionHandler SetNext(IGameActionHandler handler)
        {
            _nextHandler = handler;
            return handler;
        }

        public virtual void Handle(ConsoleKeyInfo keyInfo, Player player, Room room)
        {
            if (_nextHandler != null)
            {
                _nextHandler.Handle(keyInfo, player, room);
            }
        }
    }
}
