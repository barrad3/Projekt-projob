using System;
using Projekt1.Decorators;
using Projekt1.Items;

namespace Projekt1.Builder
{
    public class RoomBuilder : IBuilder
    {
        private Room room = new Room(20, 40);

        public RoomBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this.room = new Room(20, 40);
        }

        public IBuilder SetEmptyDungeon()
        {
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    room.SetCell(i, j, ' ');
                }
            }
            return this;
        }

        public IBuilder SetFilledDungeon()
        {
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    room.SetCell(i, j, '█');
                }
            }
            return this;
        }

        public IBuilder AddPaths()
        {
            int rCount = room.Rows;
            int cCount = room.Cols;
            Random rnd = new Random();

            bool[,] visited = new bool[rCount, cCount];

            int startR = 1;
            int startC = 1;

            room.SetCell(startR, startC, ' ');
            visited[startR, startC] = true;

            Stack<(int r, int c)> stack = new Stack<(int r, int c)>();
            stack.Push((startR, startC));

            while (stack.Count > 0)
            {
                var (r, c) = stack.Pop();

                var neighbors = new List<(int nr, int nc)>();
                if (r - 2 > 0 && !visited[r - 2, c]) neighbors.Add((r - 2, c));
                if (r + 2 < rCount - 1 && !visited[r + 2, c]) neighbors.Add((r + 2, c));
                if (c - 2 > 0 && !visited[r, c - 2]) neighbors.Add((r, c - 2));
                if (c + 2 < cCount - 1 && !visited[r, c + 2]) neighbors.Add((r, c + 2));

                if (neighbors.Count > 0)
                {
                    stack.Push((r, c));

                    var (nr, nc) = neighbors[rnd.Next(neighbors.Count)];

                    int wallR = r + (nr - r) / 2;
                    int wallC = c + (nc - c) / 2;
                    room.SetCell(wallR, wallC, ' ');
                    room.SetCell(nr, nc, ' ');

                    visited[nr, nc] = true;
                    stack.Push((nr, nc));
                }
            }

            room.SetCell(0, 0, ' ');

            if (rCount > 1)
            {
                room.SetCell(1, 0, ' ');
            }

            return this;
        }



        public IBuilder AddChambers()
        {
            Random rnd = new Random();
            int chambers = 3;
            for (int ch = 0; ch < chambers; ch++)
            {
                int startRow = rnd.Next(room.Rows - 5);
                int startCol = rnd.Next(room.Cols - 5);
                int height = rnd.Next(3, 6);
                int width = rnd.Next(3, 6);
                for (int i = startRow; i < startRow + height; i++)
                {
                    for (int j = startCol; j < startCol + width; j++)
                    {
                        room.SetCell(i, j, ' ');
                    }
                }
            }
            return this;
        }

        public IBuilder AddCentralRoom()
        {
            int height = room.Rows / 3;
            int width = room.Cols / 3;
            int startRow = room.Rows / 2 - height / 2;
            int startCol = room.Cols / 2 - width / 2;
            for (int i = startRow; i < startRow + height; i++)
            {
                for (int j = startCol; j < startCol + width; j++)
                {
                    room.SetCell(i, j, ' ');
                }
            }
            return this;
        }

        public IBuilder AddItems()
        {
            Random rnd = new Random();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.05)
                    {
                        IItem item = rnd.Next(2) == 0 ? new Vase() : new Statue();
                        item = rnd.Next(3) == 0 ? item : new Painting();
                        room.AddItem(i, j, item);
                    }
                }
            }
            return this;
        }

        public IBuilder AddWeapons()
        {
            Random rnd = new Random();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.05)
                    {
                        IItem weapon = rnd.Next(2) == 0 ? new Sword() : new Axe();
                        room.AddItem(i, j, weapon);
                    }
                }
            }
            return this;
        }

        public IBuilder AddModifiedWeapons()
        {
            Random rnd = new Random();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.05)
                    {
                        IItem weapon = rnd.Next(2) == 0 ? new Sword() : new Axe();
                        if (rnd.NextDouble() < 0.5)
                            weapon = new StrongDecorator(weapon);
                        else
                            weapon = new UnluckyDecorator(weapon);
                        room.AddItem(i, j, weapon);
                    }
                }
            }
            return this;
        }
        public IBuilder AddElixirs()
        {
            Random rnd = new Random();
            List<Func<IItem>> elixirs = new List<Func<IItem>>()
            {
                () => new HealthElixir(),
                () => new AntidotumElixir(),
                () => new StrengthElixir(),
                () => new LuckElixir()
            };

            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.03)
                    {
                        IItem elixir = elixirs[rnd.Next(elixirs.Count)]();
                        room.AddItem(i, j, elixir);
                    }
                }
            }
            return this;
        }

        public IBuilder AddEnemies()
        {
            Random rnd = new Random();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.04)
                    {
                        room.AddEnemy(i, j, new Enemy());
                    }
                }
            }
            return this;
        }

        public IBuilder AddCurrency()
        {
            Random rnd = new Random();
            Random amount = new Random();
            for (int i = 0; i < room.Rows; i++)
            {
                for (int j = 0; j < room.Cols; j++)
                {
                    if (room.GetCell(i, j) == ' ' && rnd.NextDouble() < 0.05)
                    {
                        IItem currency = rnd.Next(2) == 0 ? new Coin(amount.Next(1, 10)) : new Gold(amount.Next(1, 10));
                        room.AddItem(i, j, currency);
                    }
                }
            }
            return this;
        }

        public Room GetRoom()
        {
            Room result = this.room;
            this.Reset();
            return result;
        }

        public IBuilder BuildMovementInstructions(Room room, Player player) { return this; }
        public IBuilder BuildPickupInstructions(Room room, Player player) { return this; }
        public IBuilder BuildEnemyInstructions(Room room, Player player) { return this; }
        public IBuilder BuildGeneralInstructions(Room room, Player player) { return this; }
        public IBuilder DropAllItemsInstructions(Room room, Player player) { return this; }
        public IBuilder AttackInstructions(Room room, Player player) { return this; }
        public List<string> GetInstructions(Room room, Player player) { return null; }
    }
}
