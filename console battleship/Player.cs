using System;

namespace battleship
{
    internal class Player
    {
        public int[,] grille = new int[10, 10];
        public int[,] radar = new int[10, 10];
        public string name;
        public int vie;
        public bool win;
        public ConsoleColor color;

        public Player(string name, ConsoleColor color)
        {
            this.name = name;
            this.color = color;
        }

        public override string ToString()
        {
            Console.BackgroundColor = ConsoleColor.Red;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t{0}\t", name);
            Console.ResetColor();

            return name;
        }

        public bool CaseIsEmpty(int x, int y)
        {
            if (grille[x, y] == 0)
            {
                return false;
            }
            return true;
        }
    }
}