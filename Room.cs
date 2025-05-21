using System;
using System.Collections.Generic;
using Projekt1.Items;

namespace Projekt1
{
    public class Room
    {
        public int Rows { get; private set; }
        public int Cols { get; private set; }
        private char[,] grid;
        private List<IItem>[,] itemsGrid;
        private Enemy[,] enemies;

        public Room(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            grid = new char[Rows, Cols];
            itemsGrid = new List<IItem>[Rows, Cols];
            enemies = new Enemy[Rows, Cols];

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    itemsGrid[i, j] = new List<IItem>();
                }
            }
        }

        public void SetCell(int row, int col, char c)
        {
            grid[row, col] = c;
        }

        public char GetCell(int row, int col)
        {
            return grid[row, col];
        }

        public void AddItem(int row, int col, IItem item)
        {
            itemsGrid[row, col].Add(item);
        }

        public void AddEnemy(int row, int col, Enemy enemy)
        {
            enemies[row, col] = enemy;
        }

        public List<IItem> GetItems(int row, int col)
        {
            return itemsGrid[row, col];
        }

        public void RemoveItem(int row, int col, IItem item)
        {
            itemsGrid[row, col].Remove(item);
        }

        public bool IsWalkable(int row, int col)
        {
            if (row < 0 || row >= Rows || col < 0 || col >= Cols)
                return false;
            return grid[row, col] != '█';
        }

        public Enemy GetEnemy(int row, int col)
        {
            return enemies[row, col];
        }

        public void RemoveEnemy(int row, int col)
        {
            enemies[row, col] = null;
        }
    }
}
