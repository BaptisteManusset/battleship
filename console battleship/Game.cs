using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace battleship
{
    class Game
    {
        bool actualPlayer = false;
        int offset_top = 3;
        int offset_left = 3;
        int vaisseauCaseCount = 0;

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
            players.Add(new Player("Batman"));
            players.Add(new Player("Joker"));

            Placing();
            players.Reverse();
            Placing();
            players.Reverse();

            while (true)
            {
                Console.Clear();
                Turn();
                if (players.First<Player>().win)
                {
                    break;
                }

                players.Reverse();

            }
            Console.Clear();
            Console.WriteLine("Félicitation {0}, vous avez gagnez !!!", players.First<Player>().name);

            Console.ReadKey();

        }

        private void Placing()
        {
            ConsoleKeyInfo cki;
            #region création des différents vaisseaux

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

            for (int i = 0; i < flotte.Count; i++)
            {
                vaisseauCaseCount += flotte[i].quantite;
            }

            #endregion



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



                    #region message d'information pour le placement
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

                    #region gestion des inputs de l'utilisateur
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

                    #endregion
                } while (!((cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Spacebar) && canPlace == true));


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
            } while (vaisseauRestant != 0);      // loop tant qu'il reste un vaisseau a placer
                                                 //} while (false); /////////////-------------------------------------------------------------------------------------------------------Debugage

            Console.SetCursorPosition(0, 0);
        }


        private void Turn()
        {
            ConsoleKeyInfo cki;
            int pos_top = 0;
            int pos_left = 0;


            do
            {

                #region affichage de la grille de placement
                Console.SetCursorPosition(offset_left, offset_top - 1);
                Console.WriteLine("Votre carte");
                #region affichage du fond de la grille

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Cyan;


                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Console.SetCursorPosition(y * 2 + offset_left, x + offset_top);
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
                        Console.SetCursorPosition(y * 2 + offset_left, x + offset_top);
                        if (players.First<Player>().grille[x, y] > 0)
                        {
                            DrawCase(x + offset_top, y * 2 + offset_left, ConsoleColor.Gray, players.First<Player>().grille[x, y].ToString() + players.First<Player>().grille[x, y]);
                        }
                        else if (players.First<Player>().grille[x, y] == -1)
                        {
                            DrawCase(x + offset_top, y * 2 + offset_left, ConsoleColor.Red, "▒▒");

                        }


                    }
                }

                Console.SetCursorPosition(0, 20);
                Console.ResetColor();
                #endregion
                #endregion

                #region affichage de la grille ennemy
                Console.SetCursorPosition(offset_left + 30, offset_top - 1);
                Console.WriteLine("Carte ennemy");
                #region affichage du fond de la grille

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.ForegroundColor = ConsoleColor.Cyan;


                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Console.SetCursorPosition(y * 2 + offset_left + 30, x + offset_top);
                        DrawCase(ConsoleColor.Blue, "  ");
                    }
                    Console.WriteLine();
                }
                Console.ResetColor();
                #endregion
                #region affichage du radar sur la grille
                Console.ForegroundColor = ConsoleColor.Black;
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {
                        Console.SetCursorPosition(y * 2 + offset_left + 30, x + offset_top);
                        if (players.First<Player>().radar[x, y] == 1)
                        {
                            DrawCase(x + offset_top, y * 2 + offset_left + 30, ConsoleColor.Gray);
                        }
                        else if (players.First<Player>().radar[x, y] == -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            DrawCase(x + offset_top, y * 2 + offset_left + 30, ConsoleColor.DarkBlue, "XX");

                        }


                    }
                }
                Console.ResetColor();
                #endregion
                #region affichage du curseur sur la grille
                for (int x = 0; x < 10; x++)
                {
                    for (int y = 0; y < 10; y++)
                    {

                        if (y == pos_left && pos_top == x)
                        {
                            Console.SetCursorPosition(offset_left + pos_left * 2 + 30, offset_top + pos_top);
                            DrawCase(ConsoleColor.Red, "  ");
                        }
                    }
                }




                Console.SetCursorPosition(0, 20);
                #endregion
                Console.ResetColor();
                #endregion



                #region gestion des inputs de l'utilisateur
                cki = Console.ReadKey();
                switch (cki.Key.ToString().ToLower())
                {
                    case "uparrow":
                    case "z":
                        pos_top--;
                        break;
                    case "downarrow":
                    case "s":
                        pos_top++;
                        break;
                    case "leftarrow":
                    case "q":
                        pos_left--;
                        break;
                    case "rightarrow":
                    case "d":
                        pos_left++;
                        break;
                }
                pos_top = Math.Min(Math.Max(pos_top, 0), 9);
                pos_left = Math.Min(Math.Max(pos_left, 0), 9);

                #endregion

            } while (!(cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Spacebar));

            #region verification si touche
            if (players.Last<Player>().grille[pos_top, pos_left] > 0)
            {
                players.First<Player>().radar[pos_top, pos_left] = 1;
                players.Last<Player>().grille[pos_top, pos_left] = -1;
                Console.SetCursorPosition(offset_left + pos_left * 2 + 30, offset_top + pos_top);
                DrawCase(ConsoleColor.Red, "<>");
                Console.SetCursorPosition(0, 20);
                Console.WriteLine("Toucher !!!");
                Console.ResetColor();
            }
            else if (players.Last<Player>().grille[pos_top, pos_left] == 0)
            {
                Console.WriteLine("Plouff !!!");
                players.First<Player>().radar[pos_top, pos_left] = -1;
            }
            #endregion
            #region vie
            int vie = 0;
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (players.Last<Player>().grille[x, y] > 0)
                    {
                        vie++;

                    }
                }
            }
            players.Last<Player>().vie = vie;
            if (players.Last<Player>().vie == 0)
            {
                players.First<Player>().win = true;
            }

            Console.WriteLine("La vie de votre adversaire est de {0} sur {1}", vie, vaisseauCaseCount);
            #endregion
            Console.WriteLine("Appuyer sur une touche pour passer au joueur suivant");
            Console.ReadKey();
        }



        public void DrawCase(ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }
        public void DrawCase(int x, int y, ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }
    }
}
