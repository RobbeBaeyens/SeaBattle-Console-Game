using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;
using System.Resources;
using System.Threading;

namespace Zeeslag1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ZEESLAG by robbe baeyens";
            Console.WindowHeight = 40;
            Console.WindowWidth = 130;
            Console.BufferWidth = 250;

            //declareren
            string[,] spelBord1 = new string[10, 10];
            string[,] spelBord2 = new string[spelBord1.GetLength(0), spelBord1.GetLength(1)];
            string[,] spelBord3 = new string[spelBord1.GetLength(0), spelBord1.GetLength(1)];
            string[,] spelBord4 = new string[spelBord1.GetLength(0), spelBord1.GetLength(1)];
            for (int i = 0; i < spelBord1.GetLength(0); i++)
            {
                for (int j = 0; j < spelBord1.GetLength(1); j++)
                {
                    spelBord1[i, j] = " ";
                    spelBord2[i, j] = " ";
                    spelBord3[i, j] = " ";
                    spelBord4[i, j] = " ";
                }
            }
            string[] zettenOpslag = new string[10] { "", "", "", "", "", "", "", "", "", "" };
            string[] schepen = new string[5] { "Vliegdek Moederschip (5)", "Slagschip (4)", "Onderzeëer (3)", "Torpedojager (3)", "Patroeilleschip (2)" };
            int[] schiplengte = new int[5] { 5, 4, 3, 3, 2 };
            int aantalSchepenGezet1 = 0, aantalSchepenGezet2 = 0, opnieuw2 = 0, cancelSetShip = 0, aantalRaakpunten = 17;
            int rijNummer = 0, kolomNummer = 0, spelerBeurt, opnieuw = 0, iemandIsGewonnen = 0, speler1Raakt = 0, speler2Raakt = 0, slaagNogEens = 0, slaagNogEens2 = 0, isMusltiplayer = 1, horOrVerInt;
            string ErrorMessage = "", multiplayer, horOrVer = "";

            //multiplayer of singleplayer + LOGO
            Console.WriteLine("\n\n\n\t" +
                "\n\t████████   ███████   ███████    ██████   ██          ████      ██████          gemaakt door Robbe Baeyens" +
                "\n\t     ██    ██        ██        ██        ██        ███  ███   ██               bevat Multiplayer en singleplayer modus" +
                "\n\t   ██      █████     █████      █████    ██        ██    ██   ██  ███          bevat geluid" +
                "\n\t ██        ██        ██             ██   ██        ████████   ██    ██         bestaat uit 1350 regels C# code" +
                "\n\t████████   ███████   ███████   ██████    ███████   ██    ██    ██████          ");
            Console.WriteLine("\n\n" +
"\n\t                                      |__                                      /" +
"\n\t                                      |\\/                                  / *  /" +
"\n\t                                      ---                                /   /" +
"\n\t                                     / | [                            /**/**" +
"\n\t                              !      | |||                         ******/**" +
"\n\t                            _ /| _ /| -++'                          **/****" +
"\n\t                        + +--|    | --| --| _ | -                    **" +
"\n\t                     { /| __ |  |/\\__ |  | --- ||| __ /" +
"\n\t                    +---------------___[}-_ === _.'____                         /\\ " +
"\n\t                 ____`-' ||___-{]_| _[}-  |      |_[___\\==--                    \\/   _" +
"\n\t __..._____-- ==/ ___]_ | __ | _____________________________[___\\== --____, ------' .7" +
"\n\t  |                                                                        BB - 61 /" +
"\n\t   \\_________________________________________________________________________ |***");

            opnieuw = 1;
            Console.Write("\n\n");
            while (opnieuw == 1)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t(1)Multiplayer / (2)Singleplayer ? : ");
                multiplayer = Console.ReadLine();
                opnieuw = 0;
                switch (multiplayer)
                {
                    case "1":
                        isMusltiplayer = 1;
                        break;
                    case "2":
                        isMusltiplayer = 0;
                        break;
                    default:
                        opnieuw = 1;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\tError...");
                        break;
                }
            }


            //setup1
            while (aantalSchepenGezet1 < 5)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   SPELER 1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelBord1[rijen, kolommen] == "#")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write("  {0}", spelBord1[rijen, kolommen]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" #");
                }
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("\n\t{0}", ErrorMessage);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\n\t" + schepen[aantalSchepenGezet1]);
                Console.Write("\n\tPlaats schip horizontaal (H) / verticaal (V)  : ");
                horOrVer = Console.ReadLine().ToUpper();
                while (horOrVer != "V" && horOrVer != "H")
                {
                    Console.Write("\n\tPlaats schip horizontaal (H) / verticaal (V)  : ");
                    horOrVer = Console.ReadLine().ToUpper();
                }
                Console.Write("\n\tPlaats schip op rij   : ");
                while (!int.TryParse(Console.ReadLine(), out rijNummer) | (horOrVer == "V" && rijNummer > 10 - schiplengte[aantalSchepenGezet1]))
                {
                    Console.Write("\n\tPlaats schip op rij   : ");
                }
                Console.Write("\tPlaats schip op kolom : ");
                while (!int.TryParse(Console.ReadLine(), out kolomNummer) | (horOrVer == "H" && kolomNummer > 10 - schiplengte[aantalSchepenGezet1]))
                {
                    Console.Write("\tPlaats schip op kolom : ");
                }
                if (rijNummer < 0 || rijNummer > spelBord1.GetUpperBound(0))
                {
                    rijNummer = 0;
                }
                if (kolomNummer < 0 || kolomNummer > spelBord1.GetUpperBound(1))
                {
                    kolomNummer = 0;
                }
                //test correct ship placement
                cancelSetShip = 0;
                if (horOrVer == "V")
                {
                    for (int i = 0; i < schiplengte[aantalSchepenGezet1]; i++)
                    {
                        if (spelBord1[rijNummer + i, kolomNummer] == "#")
                        {
                            cancelSetShip = 1;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < schiplengte[aantalSchepenGezet1]; i++)
                    {
                        if(spelBord1[rijNummer, kolomNummer + i] == "#")
                        {
                            cancelSetShip = 1;
                        }
                    }
                }

                if (cancelSetShip == 1)
                {
                    ErrorMessage = "Hier staat al een schip!";
                    SoundPlayer error = new SoundPlayer();
                    error.SoundLocation = @"error.wav";
                    error.Play();
                }
                else
                {
                    ErrorMessage = "";
                    if (horOrVer == "V")
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet1]; i++)
                        {
                            spelBord1[rijNummer + i, kolomNummer] = "#";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet1]; i++)
                        {
                            spelBord1[rijNummer, kolomNummer + i] = "#";
                        }
                    }
                    aantalSchepenGezet1 += 1;
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n   SPELER 1");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelBord1[rijen, kolommen] == "#")
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }
                        Console.Write("  {0}", spelBord1[rijen, kolommen]);
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" #");
                }
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }
            }

            Console.ReadLine();

            if (isMusltiplayer == 1)
            {
                Console.Clear();
                Console.WriteLine("\n\n" +
                "\n\t █████   ██        ██   ██   ████████     █████   ██   ██" +
                "\n\t██       ██        ██           ██      ███       ██   ██" +
                "\n\t ████    ██   ██   ██   ██      ██      ██        ███████" +
                "\n\t    ██    ██ ████ ██    ██      ██      ███       ██   ██" +
                "\n\t█████      ███  ███     ██      ██        █████   ██   ██" +
                "\n\n\n" +
                "\n\t █████    ██        █████    ██    ██   ██████   ██████  " +
                "\n\t ██  ██   ██       ██   ██    ██  ██    ██       ██   ██ " +
                "\n\t █████    ██       ██   ██     ████     █████    ██████  " +
                "\n\t ██       ██       ███████      ██      ██       ██  ██  " +
                "\n\t ██       ██████   ██   ██      ██      ██████   ██   ██ ");
                Console.ReadLine();
                Console.Write("\n\t.");
                Console.ReadLine();
                Console.Write("\t.");
                Console.ReadLine();
                Console.Write("\t.");
                Console.ReadLine();

                //setup2 + computer setup
                while (aantalSchepenGezet2 < 5)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n   SPELER 2");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\t##");
                    for (int i = 0; i < spelBord2.GetLength(0); i++)
                    {
                        if (i > 9)
                        {
                            Console.Write(" {0}", i);
                        }
                        else
                        {
                            Console.Write("  {0}", i);
                        }
                    }
                    Console.WriteLine(" #");
                    for (int rijen = 0; rijen < spelBord2.GetLength(0); rijen++)
                    {
                        if (rijen > 9)
                        {
                            Console.Write("\t" + rijen);
                        }
                        else
                        {
                            Console.Write("\t " + rijen);
                        }
                        for (int kolommen = 0; kolommen < spelBord2.GetLength(1); kolommen++)
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            Console.Write("  {0}", spelBord2[rijen, kolommen]);
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" #");
                    }
                    Console.Write("\t#");
                    for (int i = 0; i < spelBord2.GetLength(0) + 1; i++)
                    {
                        Console.Write(" ##");
                    }
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("\n\t{0}", ErrorMessage);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\n\t" + schepen[aantalSchepenGezet2]);
                    Console.Write("\n\tPlaats schip horizontaal (H) / verticaal (V)  : ");
                    horOrVer = Console.ReadLine().ToUpper();
                    while (horOrVer != "V" && horOrVer != "H")
                    {
                        Console.Write("\n\tPlaats schip horizontaal (H) / verticaal (V)  : ");
                        horOrVer = Console.ReadLine().ToUpper();
                    }
                    Console.Write("\n\tPlaats schip op rij   : ");
                    while (!int.TryParse(Console.ReadLine(), out rijNummer) | (horOrVer == "V" && rijNummer > 10 - schiplengte[aantalSchepenGezet2]))
                    {
                        Console.Write("\n\tPlaats schip op rij   : ");
                    }
                    Console.Write("\tPlaats schip op kolom : ");
                    while (!int.TryParse(Console.ReadLine(), out kolomNummer) | (horOrVer == "H" && kolomNummer > 10 - schiplengte[aantalSchepenGezet2]))
                    {
                        Console.Write("\tPlaats schip op kolom : ");
                    }
                    if (rijNummer < 0 || rijNummer > spelBord2.GetUpperBound(0))
                    {
                        rijNummer = 0;
                    }
                    if (kolomNummer < 0 || kolomNummer > spelBord2.GetUpperBound(1))
                    {
                        kolomNummer = 0;
                    }
                    //test correct ship placement
                    cancelSetShip = 0;
                    if (horOrVer == "V")
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                        {
                            if (spelBord2[rijNummer + i, kolomNummer] == "#")
                            {
                                cancelSetShip = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                        {
                            if (spelBord2[rijNummer, kolomNummer + i] == "#")
                            {
                                cancelSetShip = 1;
                            }
                        }
                    }

                    if (cancelSetShip == 1)
                    {
                        ErrorMessage = "Hier staat al een schip!";
                        SoundPlayer error = new SoundPlayer();
                        error.SoundLocation = @"error.wav";
                        error.Play();
                    }
                    else
                    {
                        ErrorMessage = "";
                        if (horOrVer == "V")
                        {
                            for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                            {
                                spelBord2[rijNummer + i, kolomNummer] = "#";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                            {
                                spelBord2[rijNummer, kolomNummer + i] = "#";
                            }
                        }
                        aantalSchepenGezet2 += 1;
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n   SPELER 2");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\t##");
                    for (int i = 0; i < spelBord2.GetLength(0); i++)
                    {
                        if (i > 9)
                        {
                            Console.Write(" {0}", i);
                        }
                        else
                        {
                            Console.Write("  {0}", i);
                        }
                    }
                    Console.WriteLine(" #");
                    for (int rijen = 0; rijen < spelBord2.GetLength(0); rijen++)
                    {
                        if (rijen > 9)
                        {
                            Console.Write("\t" + rijen);
                        }
                        else
                        {
                            Console.Write("\t " + rijen);
                        }
                        for (int kolommen = 0; kolommen < spelBord2.GetLength(1); kolommen++)
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            Console.Write("  {0}", spelBord2[rijen, kolommen]);
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine(" #");
                    }
                    Console.Write("\t#");
                    for (int i = 0; i < spelBord2.GetLength(0) + 1; i++)
                    {
                        Console.Write(" ##");
                    }
                }
                Console.ReadLine();
            }
            else
            {
                while (aantalSchepenGezet2 < 5)
                {
                    Random random = new Random();
                    horOrVerInt = random.Next(0, 2);
                    if (horOrVerInt == 0)
                    {
                        horOrVer = "H";
                    }
                    else
                    {
                        horOrVer = "V";
                    }
                    if (horOrVer == "V")
                    {
                        kolomNummer = random.Next(0, 10);
                        rijNummer = random.Next(0, 10 - schiplengte[aantalSchepenGezet2]);
                    }
                    if (horOrVer == "H")
                    {
                        rijNummer = random.Next(0, 10);
                        kolomNummer = random.Next(0, 10 - schiplengte[aantalSchepenGezet2]);
                    }
                    cancelSetShip = 0;
                    if (horOrVer == "V")
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                        {
                            if (spelBord2[rijNummer + i, kolomNummer] == "#")
                            {
                                cancelSetShip = 1;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                        {
                            if (spelBord2[rijNummer, kolomNummer + i] == "#")
                            {
                                cancelSetShip = 1;
                            }
                        }
                    }

                    if (cancelSetShip != 1)
                    {
                        ErrorMessage = "";
                        if (horOrVer == "V")
                        {
                            for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                            {
                                spelBord2[rijNummer + i, kolomNummer] = "#";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < schiplengte[aantalSchepenGezet2]; i++)
                            {
                                spelBord2[rijNummer, kolomNummer + i] = "#";
                            }
                        }
                        aantalSchepenGezet2 += 1;
                    }
                }
            }

            //game
            spelerBeurt = 1;
            while (iemandIsGewonnen == 0)
            {
                if (isMusltiplayer == 1)
                {
                    if (slaagNogEens == 0)
                    {
                        Console.Clear();
                        Console.WriteLine("\n\n" +
                        "\n\t █████   ██        ██   ██   ████████     █████   ██   ██" +
                        "\n\t██       ██        ██           ██      ███       ██   ██" +
                        "\n\t ████    ██   ██   ██   ██      ██      ██        ███████" +
                        "\n\t    ██    ██ ████ ██    ██      ██      ███       ██   ██" +
                        "\n\t█████      ███  ███     ██      ██        █████   ██   ██" +
                        "\n\n\n" +
                        "\n\t █████    ██        █████    ██    ██   ██████   ██████  " +
                        "\n\t ██  ██   ██       ██   ██    ██  ██    ██       ██   ██ " +
                        "\n\t █████    ██       ██   ██     ████     █████    ██████  " +
                        "\n\t ██       ██       ███████      ██      ██       ██  ██  " +
                        "\n\t ██       ██████   ██   ██      ██      ██████   ██   ██ ");
                        Console.ReadLine();
                        Console.Write("\n\t.");
                        Console.ReadLine();
                        Console.Write("\t.");
                        Console.ReadLine();
                        Console.Write("\t.");
                        Console.ReadLine();
                    }
                }

                Console.Clear();
                if (spelerBeurt == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n   Bord SPELER 1");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n   Bord SPELER 2");
                }
                //spelbord1
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #\t\t\tLaatste zetten:");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelerBeurt == 1)
                        {
                            if (spelBord1[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else
                            {
                                if (spelBord1[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord1[rijen, kolommen]);
                        }
                        else
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else
                            {
                                if (spelBord2[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord2[rijen, kolommen]);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" #");
                    Console.WriteLine("\t\t\t{0}", zettenOpslag[rijen]);//zetteninfo
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }

                Console.WriteLine("\n");

                //spelbord2
                if (spelerBeurt == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    if (isMusltiplayer == 1)
                    {
                        Console.WriteLine("\n   Zicht op SPELER 2");
                    }
                    else
                    {
                        Console.WriteLine("\n   Zicht op Computer");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n   Zicht op SPELER 1");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelerBeurt == 1)
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                if (spelBord2[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord4[rijen, kolommen]);
                        }
                        else
                        {
                            if (spelBord1[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                if (spelBord1[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord3[rijen, kolommen]);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" #");
                }
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }
                opnieuw = 1;
                while (opnieuw == 1)
                {
                    Thread.Sleep(1000);
                    opnieuw = 0;
                    if (spelerBeurt == 1)
                    {
                        if (isMusltiplayer == 1)
                        {
                            Console.Write("\n\n\tVal SPELER 2 aan!\n" +
                                          "\t  Val aan op rij   : ");
                        }
                        else
                        {
                            Console.Write("\n\n\tVal Computer aan!\n" +
                                          "\t  Val aan op rij   : ");
                        }
                    }
                    else
                    {
                        Console.Write("\n\n\tVal SPELER 1 aan!\n" +
                                      "\t  Val aan op rij   : ");
                    }

                    while (!int.TryParse(Console.ReadLine(), out rijNummer))
                    {
                        Console.Write("\t  Val aan op rij   : ");
                    }
                    Console.Write("\t  Val aan op kolom : ");
                    while (!int.TryParse(Console.ReadLine(), out kolomNummer))
                    {
                        Console.Write("\t  Val aan op kolom : ");
                    }
                    //zet op 0 als buiten spelbord
                    if (rijNummer < 0 || rijNummer > spelBord1.GetUpperBound(0))
                    {
                        rijNummer = 0;
                    }
                    if (kolomNummer < 0 || kolomNummer > spelBord1.GetUpperBound(1))
                    {
                        kolomNummer = 0;
                    }

                    //kijk of het raak is en registreer dit
                    if (spelerBeurt == 1)
                    {
                        slaagNogEens = 0;
                        switch (spelBord2[rijNummer, kolomNummer])
                        {
                            case "#":
                                spelBord2[rijNummer, kolomNummer] = "X";
                                spelBord4[rijNummer, kolomNummer] = "X";
                                speler1Raakt++;
                                for (int i = 9; i >= 1; i--)
                                {
                                    zettenOpslag[i] = zettenOpslag[i - 1];
                                }
                                if (isMusltiplayer == 1)
                                {
                                    zettenOpslag[0] = "X Speler 1 raakt Speler 2 op : (" + rijNummer + ", " + kolomNummer + ")";
                                }
                                else
                                {
                                    zettenOpslag[0] = "X Speler 1 raakt Computer op :  (" + rijNummer + ", " + kolomNummer + ")";
                                }
                                slaagNogEens = 1;
                                SoundPlayer explosion = new SoundPlayer();
                                explosion.SoundLocation = @"explosion.wav";
                                explosion.Play();
                                break;
                            case " ":
                                spelBord2[rijNummer, kolomNummer] = "@";
                                spelBord4[rijNummer, kolomNummer] = "@";
                                for (int i = 9; i >= 1; i--)
                                {
                                    zettenOpslag[i] = zettenOpslag[i - 1];
                                }
                                if (isMusltiplayer == 1)
                                {
                                    zettenOpslag[0] = "@ Speler 1 mist Speler 2 op :  (" + rijNummer + ", " + kolomNummer + ")";
                                }
                                else
                                {
                                    zettenOpslag[0] = "@ Speler 1 mist Computer op :  (" + rijNummer + ", " + kolomNummer + ")";
                                }
                                SoundPlayer splash = new SoundPlayer();
                                splash.SoundLocation = @"splash.wav";
                                splash.Play();
                                break;
                            default:
                                opnieuw = 1;
                                SoundPlayer error = new SoundPlayer();
                                error.SoundLocation = @"error.wav";
                                error.Play();
                                break;
                        }
                    }
                    else
                    {
                        slaagNogEens = 0;
                        switch (spelBord1[rijNummer, kolomNummer])
                        {
                            case "#":
                                spelBord1[rijNummer, kolomNummer] = "X";
                                spelBord3[rijNummer, kolomNummer] = "X";
                                speler2Raakt++;
                                for (int i = 9; i >= 1; i--)
                                {
                                    zettenOpslag[i] = zettenOpslag[i - 1];
                                }
                                zettenOpslag[0] = "X Speler 2 raakt Speler 1 op : (" + rijNummer + ", " + kolomNummer + ")";
                                slaagNogEens = 1;
                                SoundPlayer explosion = new SoundPlayer();
                                explosion.SoundLocation = @"explosion.wav";
                                explosion.Play();
                                break;
                            case " ":
                                spelBord1[rijNummer, kolomNummer] = "@";
                                spelBord3[rijNummer, kolomNummer] = "@";
                                for (int i = 9; i >= 1; i--)
                                {
                                    zettenOpslag[i] = zettenOpslag[i - 1];
                                }
                                zettenOpslag[0] = "@ Speler 2 mist Speler 1 op :  (" + rijNummer + ", " + kolomNummer + ")";
                                SoundPlayer splash = new SoundPlayer();
                                splash.SoundLocation = @"splash.wav";
                                splash.Play();
                                break;
                            default:
                                opnieuw = 1;
                                SoundPlayer error = new SoundPlayer();
                                error.SoundLocation = @"error.wav";
                                error.Play();
                                break;
                        }
                    }
                }

                //laat de zetten zien
                Console.Clear();
                if (spelerBeurt == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n   Bord SPELER 1");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\n   Bord SPELER 2");
                }
                //spelbord1
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #\t\t\tLaatste zetten:");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelerBeurt == 1)
                        {
                            if (spelBord1[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else
                            {
                                if (spelBord1[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord1[rijen, kolommen]);
                        }
                        else
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                            }
                            else
                            {
                                if (spelBord2[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord2[rijen, kolommen]);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(" #");
                    Console.WriteLine("\t\t\t{0}", zettenOpslag[rijen]);//zetteninfo
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }

                Console.WriteLine("\n");

                //spelbord2
                if (spelerBeurt == 1)
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    if (isMusltiplayer == 1)
                    {
                        Console.WriteLine("\n   Zicht op SPELER 2");
                    }
                    else
                    {
                        Console.WriteLine("\n   Zicht op Computer");
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n   Zicht op SPELER 1");
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\t##");
                for (int i = 0; i < spelBord1.GetLength(0); i++)
                {
                    if (i > 9)
                    {
                        Console.Write(" {0}", i);
                    }
                    else
                    {
                        Console.Write("  {0}", i);
                    }
                }
                Console.WriteLine(" #");
                for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                {
                    if (rijen > 9)
                    {
                        Console.Write("\t" + rijen);
                    }
                    else
                    {
                        Console.Write("\t " + rijen);
                    }
                    for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                    {
                        if (spelerBeurt == 1)
                        {
                            if (spelBord2[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                if (spelBord2[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord4[rijen, kolommen]);
                        }
                        else
                        {
                            if (spelBord1[rijen, kolommen] == "#")
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                            }
                            else
                            {
                                if (spelBord1[rijen, kolommen] == "@")
                                {
                                    Console.ForegroundColor = ConsoleColor.Cyan;
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                }
                            }
                            Console.Write("  {0}", spelBord3[rijen, kolommen]);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(" #");
                }
                Console.Write("\t#");
                for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                {
                    Console.Write(" ##");
                }

                if (isMusltiplayer == 1)
                {
                    if (slaagNogEens == 0)
                    {
                        Console.ReadLine();
                        if (spelerBeurt == 1)
                        {
                            spelerBeurt = 2;
                        }
                        else
                        {
                            spelerBeurt = 1;
                        }
                    }
                }
                else
                {
                    // computer beurt
                    slaagNogEens2 = 1;
                    if (slaagNogEens == 0)
                    {
                        while (slaagNogEens2 == 1 || opnieuw2 == 1)
                        {
                            Thread.Sleep(1000);
                            opnieuw2 = 0;
                            slaagNogEens2 = 0;
                            Random random = new Random();
                            rijNummer = random.Next(0, spelBord1.GetUpperBound(0)+1);
                            kolomNummer = random.Next(0, spelBord1.GetUpperBound(1)+1);
                            if (spelBord1[rijNummer, kolomNummer] == " ")
                            {
                                spelBord1[rijNummer, kolomNummer] = "@";
                                for (int i = 9; i >= 1; i--)
                                {
                                    zettenOpslag[i] = zettenOpslag[i - 1];
                                }
                                zettenOpslag[0] = "@ Computer mist Speler 1 op : (" + rijNummer + ", " + kolomNummer + ")";
                                SoundPlayer splash = new SoundPlayer();
                                splash.SoundLocation = @"splash.wav";
                                splash.Play();
                            }
                            else
                            {
                                if (spelBord1[rijNummer, kolomNummer] == "#")
                                {
                                    spelBord1[rijNummer, kolomNummer] = "X";
                                    for (int i = 9; i >= 1; i--)
                                    {
                                        zettenOpslag[i] = zettenOpslag[i - 1];
                                    }
                                    zettenOpslag[0] = "X Computer raakt Speler 1 op : (" + rijNummer + ", " + kolomNummer + ")";
                                    speler2Raakt++;
                                    slaagNogEens2 = 1;
                                    SoundPlayer explosion = new SoundPlayer();
                                    explosion.SoundLocation = @"pchit.wav";
                                    explosion.Play();
                                }
                                else
                                {
                                    opnieuw2 = 1;
                                }
                            }
                            Console.Clear();
                            if (spelerBeurt == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n   Bord SPELER 1");
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                Console.WriteLine("\n   Bord SPELER 2");
                            }
                            //spelbord1
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("\t##");
                            for (int i = 0; i < spelBord1.GetLength(0); i++)
                            {
                                if (i > 9)
                                {
                                    Console.Write(" {0}", i);
                                }
                                else
                                {
                                    Console.Write("  {0}", i);
                                }
                            }
                            Console.WriteLine(" #\t\t\tLaatste zetten:");
                            for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                            {
                                if (rijen > 9)
                                {
                                    Console.Write("\t" + rijen);
                                }
                                else
                                {
                                    Console.Write("\t " + rijen);
                                }
                                for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                                {
                                    if (spelerBeurt == 1)
                                    {
                                        if (spelBord1[rijen, kolommen] == "#")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                        }
                                        else
                                        {
                                            if (spelBord1[rijen, kolommen] == "@")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                            }
                                        }
                                        Console.Write("  {0}", spelBord1[rijen, kolommen]);
                                    }
                                    else
                                    {
                                        if (spelBord2[rijen, kolommen] == "#")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Green;
                                        }
                                        else
                                        {
                                            if (spelBord2[rijen, kolommen] == "@")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                            }
                                        }
                                        Console.Write("  {0}", spelBord2[rijen, kolommen]);
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.Write(" #");
                                Console.WriteLine("\t\t\t{0}", zettenOpslag[rijen]);//zetteninfo
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("\t#");
                            for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                            {
                                Console.Write(" ##");
                            }

                            Console.WriteLine("\n");

                            //spelbord2
                            if (spelerBeurt == 1)
                            {
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                if (isMusltiplayer == 1)
                                {
                                    Console.WriteLine("\n   Zicht op SPELER 2");
                                }
                                else
                                {
                                    Console.WriteLine("\n   Zicht op Computer");
                                }
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n   Zicht op SPELER 1");
                            }
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write("\t##");
                            for (int i = 0; i < spelBord1.GetLength(0); i++)
                            {
                                if (i > 9)
                                {
                                    Console.Write(" {0}", i);
                                }
                                else
                                {
                                    Console.Write("  {0}", i);
                                }
                            }
                            Console.WriteLine(" #");
                            for (int rijen = 0; rijen < spelBord1.GetLength(0); rijen++)
                            {
                                if (rijen > 9)
                                {
                                    Console.Write("\t" + rijen);
                                }
                                else
                                {
                                    Console.Write("\t " + rijen);
                                }
                                for (int kolommen = 0; kolommen < spelBord1.GetLength(1); kolommen++)
                                {
                                    if (spelerBeurt == 1)
                                    {
                                        if (spelBord2[rijen, kolommen] == "#")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        else
                                        {
                                            if (spelBord2[rijen, kolommen] == "@")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                            }
                                        }
                                        Console.Write("  {0}", spelBord4[rijen, kolommen]);
                                    }
                                    else
                                    {
                                        if (spelBord1[rijen, kolommen] == "#")
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red;
                                        }
                                        else
                                        {
                                            if (spelBord1[rijen, kolommen] == "@")
                                            {
                                                Console.ForegroundColor = ConsoleColor.Cyan;
                                            }
                                            else
                                            {
                                                Console.ForegroundColor = ConsoleColor.Red;
                                            }
                                        }
                                        Console.Write("  {0}", spelBord3[rijen, kolommen]);
                                    }
                                }
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine(" #");
                            }
                            Console.Write("\t#");
                            for (int i = 0; i < spelBord1.GetLength(0) + 1; i++)
                            {
                                Console.Write(" ##");
                            }
                        }
                    }
                }

                if (speler1Raakt >= aantalRaakpunten)
                {
                    iemandIsGewonnen = 1;
                }
                else
                {
                    if (speler2Raakt >= aantalRaakpunten)
                    {
                        iemandIsGewonnen = 1;
                    }
                    else
                    {

                    }
                }
            }

            //einde
            Console.Clear();
            if (speler1Raakt >= aantalRaakpunten || isMusltiplayer == 1)
            {
                Console.WriteLine("\n\n" +
            "\n\t█████    ██        █████    ██    ██   ██████   ██████  " +
            "\n\t██  ██   ██       ██   ██    ██  ██    ██       ██   ██ " +
            "\n\t█████    ██       ██   ██     ████     █████    ██████  " +
            "\n\t██       ██       ███████      ██      ██       ██  ██  " +
            "\n\t██       ██████   ██   ██      ██      ██████   ██   ██ " +
            "\n\n\n");
            }
            if (speler1Raakt >= aantalRaakpunten)
            {
                Console.WriteLine("" +
                      "\t  ██  " +
                    "\n\t████  " +
                    "\n\t  ██  " +
                    "\n\t  ██  " +
                    "\n\t██████" +
                    "\n\n\n");
            }
            else
            {
                if (isMusltiplayer == 1)
                {
                    Console.WriteLine("" +
                      "\t ████ " +
                    "\n\t█   ██" +
                    "\n\t   ██ " +
                    "\n\t ██  " +
                    "\n\t██████" +
                    "\n\n\n");
                }
                else
                {
                    Console.WriteLine("" +
                      "\t█████     █████" +
                    "\n\t██  ██   ██" +
                    "\n\t█████    ██" +
                    "\n\t██       ██" +
                    "\n\t██        █████" +
                    "\n\n\n");
                }
            }
            Console.WriteLine("" +
              "\t██        ██   ██   ██    ██    ██████" +
            "\n\t██        ██        ████  ██   ██" +
            "\n\t██   ██   ██   ██   ██  ████    █████" +
            "\n\t ██ ████ ██    ██   ██    ██        ██" +
            "\n\t  ███  ███     ██   ██    ██   ██████");

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
            Thread.Sleep(15000);
        }
    }
}
