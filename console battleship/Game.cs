using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace battleship
{
    class Game
    {
        bool actualPlayer = false;
        int offset_x = 3;
        int offset_y = 3;


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

        public struct Vaisseau
        {
            public int quantite;
            public int width;
            public string name;
        }




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
                Turn();
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
            int actualVaisseau = 0;
            int vaisseauRestant = 100;
            int vaisseauNumber = 1;

            List<Vaisseau> flotte = new List<Vaisseau>();
            Vaisseau vaisseau = new Vaisseau();
            vaisseau.name = "Porte avion";
            vaisseau.width = 5;
            vaisseau.quantite = 1;
            flotte.Add(vaisseau);

            vaisseau.name = "Croiseur";
            vaisseau.width = 4;
            vaisseau.quantite = 1;
            flotte.Add(vaisseau);

            vaisseau.name = "Contre-torpilleurs";
            vaisseau.width = 3;
            vaisseau.quantite = 2;
            flotte.Add(vaisseau);

            vaisseau.name = "Torpilleur";
            vaisseau.width = 2;
            vaisseau.quantite = 1;
            flotte.Add(vaisseau);


            ConsoleKeyInfo cki;
            do
            {
                int pos_x = 0;
                int pos_y = 0;
                Console.Clear();
                players.First<Player>().ToString();
                Console.WriteLine();
                int bateau_width = 1;
                int bateau_height = flotte[actualVaisseau].width;

                bool canPlace = true;
                do
                {
                    canPlace = true;
                    //#region affichage


                    Console.Clear();
                    #region affichage de la grille de placement
                    #region affichage du fond de la grille

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
                    #region affichage des bateaux sur la grille
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
                    #region affichage du curseur sur la grille
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
                    #endregion
                    Console.SetCursorPosition(26, 1);
                    #region affichage de la liste des bateaux
                    Console.SetCursorPosition(26, 0);
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" [{0}] ", players.First<Player>().name);
                    Console.ResetColor();
                    Console.SetCursorPosition(26, 1);
                    Console.Write("Qnt\tTaille\tNom");
                    for (int i = 0; i < flotte.Count; i++)
                    {
                        // Verification que le curseur n'est pas sur un emplacement avec aucun bateau restant
                        if (i == actualVaisseau && flotte[i].quantite == 0)
                        {
                            actualVaisseau++;
                        }
                        Console.SetCursorPosition(26, 2 + i);
                        if (flotte[i].quantite <= 0)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkRed;
                        }
                        if (actualVaisseau == i) Console.Write(">");
                        Console.WriteLine("[{0}]\t{1}\t{2}", flotte[i].quantite, flotte[i].width, flotte[i].name);
                        Console.ResetColor();
                    }
                    #endregion
                    Console.SetCursorPosition(0, 11);


                    Console.WriteLine("Utiliser ZQSD pour vous deplacer et R pour tourner");
                    Console.WriteLine("{0}:{1} La case actuel contient {2}", pos_x + 1, pos_y + 1, players.First<Player>().grille[pos_x, pos_y]);



                    //#region message d'information pour le placement
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

                    //#endregion

                    //#region gestion des inputs de l'utilisateur
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

                            do
                            {
                                actualVaisseau++;
                                if (actualVaisseau == flotte.Count) actualVaisseau = 0;
                            } while (flotte[actualVaisseau].quantite == 0);

                            bateau_width = 1;
                            bateau_height = flotte[actualVaisseau].width;

                            break;
                        case "r":
                            int temp = bateau_width;
                            bateau_width = bateau_height;
                            bateau_height = temp;
                            break;
                    }

                    pos_x = Math.Min(Math.Max(pos_x, 0), 9);
                    pos_y = Math.Min(Math.Max(pos_y, 0), 9);

                    //#endregion
                } while (!(cki.Key == ConsoleKey.Enter && canPlace == true));


                if (flotte[actualVaisseau].quantite > 0)
                {
                    Vaisseau tempVaisseau = flotte[actualVaisseau];
                    tempVaisseau.quantite = Math.Max(tempVaisseau.quantite - 1, 0);
                    flotte[actualVaisseau] = tempVaisseau;


                    #region sauvegarde du placement
                    for (int i = pos_x; i < pos_x + bateau_width; i++)
                    {
                        for (int y = pos_y; y < pos_y + bateau_height; y++)
                        {
                            players.First<Player>().grille[i, y] = vaisseauNumber;
                        }
                    }
                    vaisseauNumber++;
                    #endregion
                }

                #region verification du nombre de bateaux restants 
                vaisseauRestant = 0;
                for (int i = 0; i < flotte.Count; i++)
                {
                    vaisseauRestant += flotte[i].quantite;
                }
                #endregion

                Console.WriteLine("Sauvegarde...");
                Thread.Sleep(200);
            } while (vaisseauRestant != 0);

            Console.SetCursorPosition(0, 0);
        }


        private void Turn()
        {

            //for (int x = 0; x < 10; x++)
            //{
            //    for (int y = 0; y < 10; y++)
            //    {
            //        Console.Write(players.First<Player>().grille[x, y]);
            //    }
            //    Console.WriteLine();
            //}



            #region affichage de la grille de placement
            #region affichage du fond de la grille

            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.Cyan;


            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Console.SetCursorPosition(y * 2 + offset_y, x + offset_x);
                    DrawCase(ConsoleColor.Blue, "  ");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
            #endregion
            #region affichage des bateaux sur la grille
            Console.ForegroundColor = ConsoleColor.Black;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    Console.SetCursorPosition(y * 2 + offset_y, x + offset_x);
                    if (players.First<Player>().CaseIsEmpty(x, y))
                        DrawCase(ConsoleColor.Gray, 
                            
                            players.First<Player>().grille[x, y].ToString() + players.First<Player>().grille[x, y].ToString());


                }
            }
            Console.ResetColor();
            #endregion
            #region affichage du curseur sur la grille
            for (int x = 0; x < 10; x++)
            {
                //for (int y = 0; y < 10; y++)
                //{

                //    if (y == pos_y && pos_x == x)
                //    {
                //        Console.SetCursorPosition((y + index_y) * 2, x + index_x);
                //        if ((x + index_x >= 0 && x + index_x <= 9) && (y + index_y >= 0 && y + index_y <= 9))
                //        {
                //            if (players.First<Player>().CaseIsEmpty(x + index_x, y + index_y))
                //            {
                //                DrawCase(ConsoleColor.Red, "  ");
                //                canPlace = false;
                //            }
                //            else
                //            {
                //                DrawCase(ConsoleColor.Green, "  ");
                //            }
                //        }
                //        else
                //        {
                //            canPlace = false;
                //        }
                //    }
                //}
                Console.WriteLine();
            }
            #endregion
            Console.ResetColor();
            #endregion
        }



        public void DrawCase(ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }
    }
}
