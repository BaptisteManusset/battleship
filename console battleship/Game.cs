using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace battleship
{
    internal class Game
    {
        private bool actualPlayer = false;
        private int offset_top = 3;
        private int offset_left = 3;
        private int vaisseauCaseCount = 0;

        private List<Player> players = new List<Player>();

        public struct Vaisseau
        {
            public int quantite;
            public int width;
            public string name;
        }

        public Game()
        {
            Rect(2, 1, 70, 8);
            Rect(3, 2, 70, 8);
            Console.SetCursorPosition(0, 0);
            Console.SetCursorPosition(4, 3);
            Console.WriteLine(" __________         __    __  .__                .__    .__        ");
            Console.SetCursorPosition(4, 4);
            Console.WriteLine(" \\______   \\_____ _/  |__/  |_|  |   ____   _____|  |__ |__|_____  ");
            Console.SetCursorPosition(4, 5);
            Console.WriteLine("  |    |  _/\\__  \\\\   __\\   __\\  | _/ __ \\ /  ___/  |  \\|  \\____ \\ ");
            Console.SetCursorPosition(4, 6);
            Console.WriteLine("  |    |   \\ / __ \\|  |  |  | |  |_\\  ___/ \\___ \\|   Y  \\  |  |_> >");
            Console.SetCursorPosition(4, 7);
            Console.WriteLine("  |______  /(____  /__|  |__| |____/\\___  >____  >___|  /__|   __/ ");
            Console.SetCursorPosition(4, 8);
            Console.WriteLine("         \\/      \\/                     \\/     \\/     \\/   |__|    ");
            Console.SetCursorPosition(4, 9);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("                                             |__                                              ");
            Console.WriteLine("                                             |\\/                                              ");
            Console.WriteLine("                                             ---                                              ");
            Console.WriteLine("                                             / | [                                            ");
            Console.WriteLine("                                      !      | |||                                            ");
            Console.WriteLine("                                    _/|     _/|-++'                                           ");
            Console.WriteLine("                                +  +--|    |--|--|_ |-                                        ");
            Console.WriteLine("                             { /|__|  |/\\__|  |--- |||__/                                     ");
            Console.WriteLine("                            +---------------___[}-_===_.'____                 /\\              ");
            Console.WriteLine("                        ____`-' ||___-{]_| _[}-  |     |_[___\\==--            \\/   _          ");
            Console.WriteLine("         __..._____--==/___]_|__|_____________________________[___\\==--____,------' .7        ");
            Console.WriteLine("        |                                                                           /          ");
            Console.WriteLine("        |                                                       USS Constitution   /          ");
            Console.WriteLine("         \\________________________________________________________________________|          ");
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine("                                                                                                      ");
            Console.WriteLine("                                                                                                      ");
            Console.WriteLine("                                                                                                      ");
            Console.WriteLine();
            Console.SetCursorPosition(75, 26);
            Console.WriteLine("[[Press to Play]]");
            Console.ResetColor();

            Console.SetCursorPosition(0, 28);
            Console.ReadKey();

            for (int i = 0; i < 2; i++)
            {
                string name;
                Console.Clear();
                Console.WriteLine("Joueur {0}", i + 1);
                Console.WriteLine("Entrez votre pseudo:");

                name = Console.ReadLine();
                //name = name.Substring(0, 10);
                Console.Write("Votre pseudo est : ");
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.Write(" {0} ", name);
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Appuyer pour passer au joueur suivant");

                players.Add(new Player(name, i == 0 ? ConsoleColor.Red : ConsoleColor.Cyan));
                Console.ReadKey();
            }

            Transition("Placement des Bateaux par les joueurs", ConsoleColor.Yellow);
            Transition("Placement des Bateaux de [" + players.First<Player>().name + "]", players.First<Player>().color);
            Placing();
            players.Reverse();
            Transition("Placement des Bateaux de [" + players.First<Player>().name + "]", players.First<Player>().color);
            Placing();
            players.Reverse();

            Transition("Phase de Jeu", ConsoleColor.Yellow);
            while (true)
            {
                Console.Clear();
                Turn();
                if (players.First<Player>().win)
                {
                    break;
                }
                else
                {
                    Transition("Au joueur suivant", players.Last<Player>().color);
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

            #endregion création des différents vaisseaux

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

                    #endregion affichage du fond de la grille

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

                    #endregion affichage des bateaux sur la grille

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

                    #endregion affichage du curseur sur la grille

                    Console.ResetColor();

                    #endregion affichage de la grille de placement

                    Console.SetCursorPosition(26, 1);

                    #region affichage de la liste des bateaux

                    Console.SetCursorPosition(26, 0);
                    Console.BackgroundColor = players.First<Player>().color;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write(" [{0}] ", players.First<Player>().name);
                    Console.ResetColor();
                    Console.SetCursorPosition(26, 1);
                    Console.Write(" Qnt\tTaille\tNom");
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
                        if (actualVaisseau == i)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        Console.WriteLine(" {0}\t{1}\t{2}\t", flotte[i].quantite, flotte[i].width, flotte[i].name);
                        Console.ResetColor();
                    }

                    #endregion affichage de la liste des bateaux

                    Console.SetCursorPosition(0, 12);

                    Console.WriteLine("\tZQSD pour vous deplacer");
                    Console.WriteLine("\tR pour tourner");
                    Console.WriteLine("\ttab pour changer de vaisseau");
                    //Console.WriteLine("{0}:{1} La case actuel contient {2}", pos_x + 1, pos_y + 1, players.First<Player>().grille[pos_x, pos_y]);

                    #region message d'information pour le placement

                    Console.WriteLine();
                    Console.Write("\t");
                    if (canPlace == false)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Impossible de placer ce bateau ici");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Appuyer sur entrer plus valider l'emplacement");
                        Console.ResetColor();
                    }

                    #endregion message d'information pour le placement

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

                                actualVaisseau = Math.Max(Math.Min(flotte.Count - 1, actualVaisseau), 0);
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

                    #endregion gestion des inputs de l'utilisateur
                } while (!((cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Spacebar) && canPlace == true));

                actualVaisseau = Math.Max(Math.Min(flotte.Count - 1, actualVaisseau), 0);
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

                    #endregion sauvegarde du placement
                }

                #region verification du nombre de bateaux restants

                vaisseauRestant = 0;
                for (int i = 0; i < flotte.Count; i++)
                {
                    vaisseauRestant += flotte[i].quantite;
                }

                #endregion verification du nombre de bateaux restants

                Console.WriteLine("Sauvegarde...");
                Thread.Sleep(200);
            } while (vaisseauRestant != 0);      // loop tant qu'il reste un vaisseau a placer
                                                 //} while (false); /////////////-------------------------------------------------------------------------------------------------------Debugage

            Transition("Au tour du joueur suivant ", players.Last<Player>().color);
        }

        private void Turn()
        {
            ConsoleKeyInfo cki;
            int pos_top = 0;
            int pos_left = 0;

            do
            {
                Console.SetCursorPosition(0, 0);
                players.First<Player>().ToString();

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

                #endregion affichage du fond de la grille

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

                #endregion affichage des bateaux sur la grille

                #endregion affichage de la grille de placement

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

                #endregion affichage du fond de la grille

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

                #endregion affichage du radar sur la grille

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

                #endregion affichage du curseur sur la grille

                Console.ResetColor();

                #endregion affichage de la grille ennemy

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

                #endregion gestion des inputs de l'utilisateur
            } while (!(cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.Spacebar));

            #region verification si touche

            if (players.Last<Player>().grille[pos_top, pos_left] > 0)
            {
                players.First<Player>().radar[pos_top, pos_left] = 1;
                players.Last<Player>().grille[pos_top, pos_left] = -1;
                Console.SetCursorPosition(offset_left + pos_left * 2 + 30, offset_top + pos_top);
                DrawCase(ConsoleColor.Red, "<>");
                Console.SetCursorPosition(0, 20);
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Toucher !!!");
                Console.ResetColor();
            }
            else if (players.Last<Player>().grille[pos_top, pos_left] == 0)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine("Plouff !!!");
                Console.ResetColor();
                players.First<Player>().radar[pos_top, pos_left] = -1;
            }

            #endregion verification si touche

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

            Console.WriteLine("Il reste {0} cases avant de vaincre la flotte ennemie.", vie, vaisseauCaseCount);

            #endregion vie

            Console.WriteLine("Appuyer sur une touche pour passer au joueur suivant");
            Console.ReadKey();
        }

        //Affiche une case au placement actuel du curseur
        public void DrawCase(ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }

        //modifie l'emplacement du curseur et affiche une case
        public void DrawCase(int x, int y, ConsoleColor color, string content = "  ")
        {
            Console.BackgroundColor = color;
            Console.Write(content);
        }

        private void Rect(int left, int top, int width, int height)
        {
            for (int i = 0; i < width; i++)
            {
                for (int y = 0; y < height; y++)
                {
                    Console.SetCursorPosition(left + i, top + y);
                    Console.Write(" ");
                }
            }

            for (int i = 0; i < width; i++)
            {
                Console.SetCursorPosition(left + i, top);
                Console.Write("═");
                Console.SetCursorPosition(left + i, top + height);
                Console.Write("═");
            }

            for (int i = 0; i < height; i++)
            {
                Console.SetCursorPosition(left, top + i);
                Console.Write("║");
                Console.SetCursorPosition(left + width, top + i);
                Console.Write("║");
            }

            Console.SetCursorPosition(left, top);
            Console.Write("╔");
            Console.SetCursorPosition(left, top + height);
            Console.Write("╚");
            Console.SetCursorPosition(left + width, top);
            Console.Write("╗");
            Console.SetCursorPosition(left + width, top + height);
            Console.Write("╝");
        }

        private void Transition(string text, ConsoleColor color)
        {
            Console.Clear();
            Console.BackgroundColor = color;
            Console.ForegroundColor = ConsoleColor.Black;
            for (int i = 0; i < 25; i++)
            {
                Console.SetCursorPosition(6, i);
                for (int y = 0; y < 50; y++)
                {
                    Console.Write(" ");
                }
            }
            Console.SetCursorPosition(6, 23);
            Console.Write("     {0}     ", text);

            Console.ResetColor();
            Console.SetCursorPosition(75, 26);
            Console.WriteLine("[[Press to Play]]");
            Console.ReadKey();
        }
    }
}