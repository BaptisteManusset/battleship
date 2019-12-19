﻿using System;

namespace battleship
{
    class Player
    {

        public int[,] grille = new int[10, 10];
        public string name;


        public Player(string name)
        {
            this.name = name;

        }


        public void Turn()
        {
            //map.display();

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
