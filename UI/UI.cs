using Projekt1.Builder;
using Projekt1.Items;
using System;
using System.Collections.Generic;

namespace Projekt1.UI
{
    public sealed class UI
    {
        private static int previousUILineCount = 0;
        private static int UI_LEFT = 0;

        private static bool mapDrawn = false;
        private static int oldPlayerX = -1;
        private static int oldPlayerY = -1;

        private const int INVENTORY_HEIGHT = 12;
        private const int INSTRUCTIONS_CLEAR_HEIGHT = 10;

        private static List<string> actionLog = new List<string>();
        private const int MaxLogEntries = 5;

        private static UI _instance;

        private UI() { }

        public static UI GetInstance()
        {
            if (_instance == null)
                _instance = new UI();
            return _instance;
        }

        public static void LogAction(string action)
        {
            actionLog.Add(action);
            if (actionLog.Count > MaxLogEntries)
                actionLog.RemoveAt(0);
        }

        public static List<string> GetActionLog()
        {
            return new List<string>(actionLog);
        }

        public static void InitializeUI(Room room)
        {
            int requiredWidth = room.Cols + 40;
            int requiredHeight = room.Rows + INVENTORY_HEIGHT + INSTRUCTIONS_CLEAR_HEIGHT + 5;
            try
            {
                if (Console.WindowWidth < requiredWidth || Console.WindowHeight < requiredHeight)
                    Console.SetWindowSize(Math.Max(Console.WindowWidth, requiredWidth), Math.Max(Console.WindowHeight, requiredHeight));
            }
            catch
            {
            }

            Console.Title = "Gra Konsolowa";
            Console.CursorVisible = false;
        }

        public static void Draw(Room room, Player player)
        {
            InitializeUI(room);

            if (!mapDrawn)
            {
                DrawMapOnce(room);
                mapDrawn = true;
                UI_LEFT = room.Cols + 3;
            }

            UpdatePlayerPosition(room, player);
            DrawUISection(room, player);

            DisplayInstructions(room, player);
        }

        private static void DrawMapOnce(Room room)
        {
            SafeSetCursorPosition(0, 0);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write("┌" + new string('─', room.Cols) + "┐");

            for (int row = 0; row < room.Rows; row++)
            {
                SafeSetCursorPosition(0, row + 1);
                Console.Write("│");
                for (int col = 0; col < room.Cols; col++)
                {
                    if (!room.IsWalkable(row, col))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write("█");
                        Console.ResetColor();
                    }
                    else if (room.GetItems(row, col).Count > 0)
                    {
                        var firstItem = room.GetItems(row, col)[0];
                        Console.Write(firstItem.Symbol);
                    }
                    else if (room.GetEnemy(row, col) != null)
                    {
                        var enemy = room.GetEnemy(row, col);
                        Console.Write(enemy.Symbol);
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("│");
            }
            SafeSetCursorPosition(0, room.Rows + 1);
            Console.Write("└" + new string('─', room.Cols) + "┘");
        }

        private static void UpdatePlayerPosition(Room room, Player player)
        {
            if (oldPlayerX >= 0 && oldPlayerY >= 0)
            {
                SafeSetCursorPosition(oldPlayerX + 1, oldPlayerY + 1);
                var items = room.GetItems(oldPlayerY, oldPlayerX);
                var enemy = room.GetEnemy(oldPlayerY, oldPlayerX);
                if (!room.IsWalkable(oldPlayerY, oldPlayerX))
                {
                    Console.Write("█");
                }
                else if (items.Count > 0)
                {
                    Console.Write(items[0].Symbol);
                }
                else if (enemy != null)
                {
                    Console.Write(enemy.Symbol);
                }
                else
                {
                    Console.Write(" ");
                }
            }

            SafeSetCursorPosition(player.PosX + 1, player.PosY + 1);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("¶");
            Console.ResetColor();

            oldPlayerX = player.PosX;
            oldPlayerY = player.PosY;
        }

        private static void DrawUISection(Room room, Player player)
        {
            var lines = PrepareUILines(room, player);
            int uiTop = 0;

            Console.ForegroundColor = ConsoleColor.Yellow;

            for (int i = 0; i < lines.Count; i++)
            {
                SafeSetCursorPosition(UI_LEFT, uiTop + i);
                int clearLength = Console.WindowWidth - UI_LEFT;
                if (clearLength > 0)
                {
                    Console.Write(new string(' ', clearLength));
                    SafeSetCursorPosition(UI_LEFT, uiTop + i);
                }
                Console.Write(lines[i]);
            }

            if (previousUILineCount > lines.Count)
            {
                for (int i = lines.Count; i < previousUILineCount; i++)
                {
                    SafeSetCursorPosition(UI_LEFT, uiTop + i);
                    int clearLength = Console.WindowWidth - UI_LEFT;
                    if (clearLength > 0)
                    {
                        Console.Write(new string(' ', clearLength));
                    }
                }
            }

            previousUILineCount = lines.Count;
            Console.ResetColor();
        }

        private static List<string> PrepareUILines(Room room, Player player)
        {
            List<string> lines = new List<string>();

            List<string> heroAttributes = new List<string>();
            heroAttributes.Add("=== Hero Attributes ===");
            heroAttributes.Add($" Strength (P):  {player.Strength}");
            heroAttributes.Add($" Agility  (A):  {player.Agility}");
            heroAttributes.Add($" Health   (H):  {player.Health}");
            heroAttributes.Add($" Luck     (L):  {player.Luck}");
            heroAttributes.Add($" Aggression(A): {player.Aggression}");
            heroAttributes.Add($" Wisdom   (W):  {player.Wisdom}");

            List<string> recentActions = new List<string>();
            recentActions.Add("=== Recent Actions ===");
            var actions = GetActionLog();
            if (actions.Count == 0)
            {
                recentActions.Add(" (no actions)");
            }
            else
            {
                foreach (var action in actions)
                {
                    recentActions.Add(" " + action);
                }
            }

            int leftColumnWidth = 30;
            int maxLines = Math.Max(heroAttributes.Count, recentActions.Count);

            for (int i = 0; i < maxLines; i++)
            {
                string leftText = i < heroAttributes.Count ? heroAttributes[i] : "";
                string rightText = i < recentActions.Count ? recentActions[i] : "";
                string combinedLine = leftText.PadRight(leftColumnWidth) + rightText;
                lines.Add(combinedLine);
            }

            lines.Add("");

            lines.Add("=== Equipped Items ===");
            lines.Add($" Left Hand:  {(player.LeftHand != null ? player.LeftHand : "Empty")}");
            lines.Add($" Right Hand: {(player.RightHand != null ? player.RightHand : "Empty")}");
            lines.Add("");

            lines.Add("=== Inventory ===");
            if (player.Backpack.Count == 0)
            {
                lines.Add(" (empty)");
            }
            else
            {
                int i = 1;
                foreach (var item in player.Backpack)
                {
                    lines.Add($" {i}. {item}");
                    i++;
                }
            }
            lines.Add("");

            lines.Add($" Coins: {player.Coins}, Gold: {player.Gold}");
            lines.Add("");

            var groundItems = room.GetItems(player.PosY, player.PosX);
            if (groundItems.Count > 0)
            {
                lines.Add("=== Items on the Ground ===");
                int i = 1;
                foreach (var gi in groundItems)
                {
                    lines.Add($" {i}. {gi}");
                    i++;
                }
                lines.Add("");
            }

            var enemy = room.GetEnemy(player.PosY, player.PosX);
            if (enemy != null)
            {
                lines.Add("=== Enemy Info ===");
                lines.Add($" Max Health:    {enemy.MaxHealth}");
                lines.Add($" Current Health:{enemy.CurrentHealth}");
                lines.Add($" Strength:      {enemy.Strength}");
                lines.Add($" Armor:         {enemy.Armor}");
                lines.Add("");
            }

            lines.Add("=== Active Effects ===");
            var effects = player.GetActiveEffectsDescriptions();
            if (effects.Count == 0)
            {
                lines.Add(" (no active effects)");
            }
            else
            {
                foreach (var effect in effects)
                {
                    lines.Add(" " + effect);
                }
            }
            lines.Add("");

            return lines;
        }


        public static void ShowInventoryBelowMap(Player player, Room room)
        {
            int inventoryTop = room.Rows + 3;
            int left = 0;

            ClearArea(inventoryTop, INVENTORY_HEIGHT, Console.WindowWidth);
            SafeSetCursorPosition(left, inventoryTop);
            Console.WriteLine("=== Inventory Management ===");
            Console.WriteLine();
            Console.Write("Enter item number to manage: ");
        }

        public static void ClearInventoryBelowMap(Room room)
        {
            int inventoryTop = room.Rows + 3;
            ClearArea(inventoryTop, INVENTORY_HEIGHT, Console.WindowWidth);
        }

        public static ConsoleKeyInfo ManageInventory(Player player, Room room)
        {
            Console.Write("'R' Right, 'L' Left, 'D' drop: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Console.WriteLine();
            return keyInfo;
        }

        public static int PromptItemSelection(List<IItem> items, Room room)
        {
            int promptRow = room.Rows + INSTRUCTIONS_CLEAR_HEIGHT + 3;
            SafeSetCursorPosition(0, promptRow);
            Console.Write("Select the number of item to pick up: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int index) && index >= 0 && index <= items.Count)
                return index - 1;
            return -1;
        }

        private static void ClearInstructionsBelowMap(Room room)
        {
            int instructionsTop = room.Rows + 2;
            ClearArea(instructionsTop, INSTRUCTIONS_CLEAR_HEIGHT, room.Cols + 2);
        }

        public static void DisplayInstructions(Room room, Player player)
        {
            ClearInstructionsBelowMap(room);

            IBuilder instructionsBuilder = new InstructionsBuilder();
            Director director = new Director { Builder = instructionsBuilder };
            List<string> instructions = director.BuildInstructions(room, player);

            int maxLineWidth = room.Cols;

            int instructionsLeft = 0;
            int instructionsTop = room.Rows + 2;

            string currentLine = "";
            for (int i = 0; i < instructions.Count; i++)
            {
                string nextPart = (i == 0) ? instructions[i] : ", " + instructions[i];

                if ((currentLine.Length + nextPart.Length) > maxLineWidth)
                {
                    SafeSetCursorPosition(instructionsLeft, instructionsTop++);
                    Console.WriteLine(currentLine);
                    currentLine = instructions[i];
                }
                else
                {
                    currentLine += nextPart;
                }
            }

            if (!string.IsNullOrEmpty(currentLine))
            {
                SafeSetCursorPosition(instructionsLeft, instructionsTop++);
                Console.WriteLine(currentLine);
            }
        }

        private static void ClearArea(int top, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                SafeSetCursorPosition(0, top + i);
                Console.Write(new string(' ', width));
            }
        }

        public static void WriteMessage(string message)
        {
            Console.WriteLine(message);
        }

        private static void SafeSetCursorPosition(int left, int top)
        {
            try
            {
                Console.SetCursorPosition(left, top);
            }
            catch (ArgumentOutOfRangeException)
            {
            }
        }

        public static void GameOver()
        {
            Console.Clear();
            mapDrawn = false;
            oldPlayerX = -1;
            oldPlayerY = -1;
            previousUILineCount = 0;
            Console.WriteLine("Game Over! Press any key to exit.");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
