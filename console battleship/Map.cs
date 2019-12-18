using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace battleship
{
    class Map
    {

        public int[,] grille = new int[10, 10];


        public Map()
        {
            Random rnd = new Random();
            #region init

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {

                    
                    //grille[x, y] = rnd.Next(1, 13)  < 10 ? 0 : 1;
                    grille[x, y] = 0;
                }
            }
            #endregion
        }


        public void display()
        {
            Console.Write("    ");

            for (int i = 1; i < 11; i++)
            {
                Console.Write(i + " ");
            }
            Console.WriteLine();
            Console.WriteLine();

            for (int x = 0; x < 10; x++)
            {
                Console.Write(GetExcelColumnName(x+1) + "   ");
                for (int y = 0; y < 10; y++)
                {
                    Console.Write(grille[x, y] + " ");
                }
                Console.WriteLine();
            }
        }



        public static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }



    }
}
