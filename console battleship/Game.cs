using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace battleship
{
    class Game
    {
        bool actualPlayer = false;

        Player p1 = new Player("joueur 1");
        Player p2 = new Player("joueur 2");

        List<Player> players = new List<Player>();


        enum GameState
        {
            placing,
            playing,
            gameover
        };
        GameState gameState = GameState.placing;

        public Game()
        {
            players.Add(new Player("joueur 1"));
            players.Add(new Player("joueur 2"));

            Placing();
            players.Reverse();
            Placing();
            players.Reverse();



            while (true)
            {
                Console.Clear();
                players.First<Player>().ToString();
                Console.WriteLine();
                Console.WriteLine("grille du joueur");
                players.First<Player>().Turn();
                Console.WriteLine();
                Console.WriteLine("grille adverse");
                players.Last<Player>().Turn();


                Console.ReadLine();
                players.Reverse();

            }

        }

        private void Placing()
        {

            //Console.TreatControlCAsInput = true;
            ConsoleKeyInfo cki;

            do
            {
                int pos_x = 0;
                int pos_y = 0;
                Console.Clear();
                players.First<Player>().ToString();
                Console.WriteLine();
                int bateau_width = 1;
                int bateau_height = 5;

                bool canPlace = true;
                do
                {
                    canPlace = true;
                    #region affichage
                    Console.Clear();
                    #region background

                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    Console.ForegroundColor = ConsoleColor.Cyan;


                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            DrawCase(ConsoleColor.Blue, "  ");
                        }
                        Console.WriteLine();
                    }
                    Console.ResetColor();
                    #endregion
                    #region bateaux
                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {
                            Console.SetCursorPosition(y * 2, x);
                            if (players.First<Player>().CaseIsEmpty(x, y))
                                DrawCase(ConsoleColor.Gray);


                        }
                    }
                    #endregion
                    #region curseur
                    for (int x = 0; x < 10; x++)
                    {
                        for (int y = 0; y < 10; y++)
                        {

                            if (y == pos_y && pos_x == x)
                            {
                                for (int index_x = 0; index_x < bateau_width; index_x++)
                                {
                                    for (int index_y = 0; index_y < bateau_height; index_y++)
                                    {
                                        Console.SetCursorPosition((y + index_y) * 2, x + index_x);
                                        if ((x + index_x >= 0 && x + index_x <= 9) && (y + index_y >= 0 && y + index_y <= 9))
                                        {
                                            if (players.First<Player>().CaseIsEmpty(x + index_x, y + index_y))
                                            {
                                                DrawCase(ConsoleColor.Red, "  ");
                                                canPlace = false;
                                            }
                                            else
                                            {
                                                DrawCase(ConsoleColor.Green, "  ");
                                            }
                                        }
                                        else
                                        {
                                            canPlace = false;
                                        }
                                    }
                                }
                            }

                        }
                        Console.WriteLine();
                    }
                    #endregion
                    Console.ResetColor();
                    Console.SetCursorPosition(0, 11);
                    Console.WriteLine("Utiliser ZQSD pour vous deplacer et R pour tourner");
                    Console.WriteLine("{0}:{1} La case actuel contient {2}", pos_x + 1, pos_y + 1, players.First<Player>().map.grille[pos_x, pos_y]);




                    if (canPlace == false)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Impossible de placer ce bateau ici");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Appuyer sur entrer plus valider l'emplacement");
                        Console.ResetColor();
                    }

                    #endregion
                    #region input
                    cki = Console.ReadKey();
                    switch (cki.Key.ToString().ToLower())
                    {
                        case "uparrow":
                        case "z":
                            pos_x--;
                            break;
                        case "downarrow":
                        case "s":
                            pos_x++;
                            break;
                        case "leftarrow":
                        case "q":
                            pos_y--;
                            break;
                        case "rightarrow":
                        case "d":
                            pos_y++;
                            break;
                        case "tab":




                            break;
                        case "r":
                            int temp = bateau_width;
                            bateau_width = bateau_height;
                            bateau_height = temp;
                            break;
                    }

                    pos_x = Math.Min(Math.Max(pos_x, 0), 9);
                    pos_y = Math.Min(Math.Max(pos_y, 0), 9);

                    #endregion
                } while (!(cki.Key == ConsoleKey.Enter && canPlace == true));

                for (int i = pos_x; i < pos_x + bateau_width; i++)
                {
                    for (int y = pos_y; y < pos_y + bateau_height; y++)
                    {
                        players.First<Player>().map.grille[i, y] = 1;
                    }
                }

                Console.WriteLine("Sauvegarde...");
                Thread.Sleep(200);
            } while (true);


        }

        public void DrawCase(ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }
    }
}
