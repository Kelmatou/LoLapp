using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace LoLapp
{
    public static class Data_library
    {
        #region APIs
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hwnd);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SetFocus(IntPtr hwnd);
        #endregion APIs //fonctions win32

        public static List<string> get_champion_list()
        {
            return (new List<string>() { "Aatrox", "Ahri", "Akali", "Alistar", "Anivia", "Ashe", "Aurelion Sol", "Azir", "Bard", "Blitzcrank", "Brand", "Braum", "Caitlyn", "Camille", "Cassiopeia", "Cho'Gath", "Corki", "Darius", "Diana", "Dr. Mundo", "Draven", "Ekko", "Elise", "Evelynn", "Ezreal", "Fiddlesticks", "Fiora", "Fizz", "Galio", "Gangplank", "Garen", "Gnar", "Gragas", "Graves", "Hecarim", "Heimerdinger", "Illaoi", "Irelia", "Ivern", "Janna", "Jarvan IV", "Jax", "Jayce", "Jhin", "Jinx", "Kalista", "Karma", "Karthus", "Kassadin", "Katarina", "Kayle", "Kennen", "Kha'Zix", "Kindred", "Kled", "Kog'Maw", "Leblanc", "Lee Sin", "Leona", "Lissandra", "Lucian", "Lulu", "Lux", "Malphite", "Malzahar", "Maokai", "Master Yi", "Miss Fortune", "Mordekaiser", "Morgana", "Nami", "Nasus", "Nautilus", "Nidalee", "Nocturne", "Nunu", "Olaf", "Orianna", "Pantheon", "Poppy", "Quinn", "Rammus", "Rek'Sai", "Renekton", "Rengar", "Riven", "Rumble", "Ryze", "Sejuani", "Shaco", "Shen", "Shyvana", "Singed", "Sion", "Sivir", "Skarner", "Sona", "Soraka", "Swain", "Syndra", "Tahm Kench", "Taliyah", "Talon", "Taric", "Teemo", "Thresh", "Tristana", "Trundle", "Tryndamere", "Twisted Fate", "Twitch", "Udyr", "Urgot", "Varus", "Vayne", "Veigar", "Vel'Koz", "Vi", "Viktor", "Vladimir", "Volibear", "Warwick", "Wukong", "Xerath", "Xin Zhao", "Yasuo", "Yorick", "Zac", "Zed", "Ziggs", "Zilean", "Zyra" });
        }

        public static string get_champion_from_id(double id_champion)
        {
            switch (Convert.ToInt32(id_champion))
            {
                case (1):
                    return ("Annie");
                case (2):
                    return ("Olaf");
                case (3):
                    return ("Galio");
                case (4):
                    return ("Twisted Fate");
                case (5):
                    return ("Xin Zhao");
                case (6):
                    return ("Urgot");
                case (7):
                    return ("Leblanc");
                case (8):
                    return ("Vladimir");
                case (9):
                    return ("Fiddlesticks");
                case (10):
                    return ("Kayle");
                case (11):
                    return ("Master Yi");
                case (12):
                    return ("Alistar");
                case (13):
                    return ("Ryze");
                case (14):
                    return ("Sion");
                case (15):
                    return ("Sivir");
                case (16):
                    return ("Soraka");
                case (17):
                    return ("Teemo");
                case (18):
                    return ("Tristana");
                case (19):
                    return ("Warwick");
                case (20):
                    return ("Nunu");
                case (21):
                    return ("Miss Fortune");
                case (22):
                    return ("Ashe");
                case (23):
                    return ("Tryndamere");
                case (24):
                    return ("Jax");
                case (25):
                    return ("Morgana");
                case (26):
                    return ("Zilean");
                case (27):
                    return ("Singed");
                case (28):
                    return ("Evelynn");
                case (29):
                    return ("Twitch");
                case (30):
                    return ("Karthus");
                case (31):
                    return ("Cho'Gath");
                case (32):
                    return ("Amumu");
                case (33):
                    return ("Rammus");
                case (34):
                    return ("Anivia");
                case (35):
                    return ("Shaco");
                case (36):
                    return ("Dr. Mundo");
                case (37):
                    return ("Sona");
                case (38):
                    return ("Kassadin");
                case (39):
                    return ("Irelia");
                case (40):
                    return ("Janna");
                case (41):
                    return ("Gangplank");
                case (42):
                    return ("Corki");
                case (43):
                    return ("Karma");
                case (44):
                    return ("Taric");
                case (45):
                    return ("Veigar");
                case (48):
                    return ("Trundle");
                case (50):
                    return ("Swain");
                case (51):
                    return ("Caitlyn");
                case (53):
                    return ("Blitzcrank");
                case (54):
                    return ("Malphite");
                case (55):
                    return ("Katarina");
                case (56):
                    return ("Nocturne");
                case (57):
                    return ("Maokai");
                case (58):
                    return ("Renekton");
                case (59):
                    return ("Jarvan IV");
                case (60):
                    return ("Elise");
                case (61):
                    return ("Orianna");
                case (62):
                    return ("Wukong");
                case (63):
                    return ("Brand");
                case (64):
                    return ("Lee Sin");
                case (67):
                    return ("Vayne");
                case (68):
                    return ("Rumble");
                case (69):
                    return ("Cassiopeia");
                case (72):
                    return ("Skarner");
                case (74):
                    return ("Heimerdinger");
                case (75):
                    return ("Nasus");
                case (76):
                    return ("Nidalee");
                case (77):
                    return ("Udyr");
                case (78):
                    return ("Poppy");
                case (79):
                    return ("Gragas");
                case (80):
                    return ("Pantheon");
                case (81):
                    return ("Ezreal");
                case (82):
                    return ("Mordekaiser");
                case (83):
                    return ("Yorick");
                case (84):
                    return ("Akali");
                case (85):
                    return ("Kennen");
                case (86):
                    return ("Garen");
                case (89):
                    return ("Leona");
                case (90):
                    return ("Malzahar");
                case (91):
                    return ("Talon");
                case (92):
                    return ("Riven");
                case (96):
                    return ("Kog'Maw");
                case (98):
                    return ("Shen");
                case (99):
                    return ("Lux");
                case (101):
                    return ("Xerath");
                case (102):
                    return ("Shyvana");
                case (103):
                    return ("Ahri");
                case (104):
                    return ("Graves");
                case (105):
                    return ("Fizz");
                case (106):
                    return ("Volibear");
                case (107):
                    return ("Rengar");
                case (110):
                    return ("Varus");
                case (111):
                    return ("Nautilus");
                case (112):
                    return ("Viktor");
                case (113):
                    return ("Sejuani");
                case (114):
                    return ("Fiora");
                case (115):
                    return ("Ziggs");
                case (117):
                    return ("Lulu");
                case (119):
                    return ("Draven");
                case (120):
                    return ("Hecarim");
                case (121):
                    return ("Kha'Zix");
                case (122):
                    return ("Darius");
                case (126):
                    return ("Jayce");
                case (127):
                    return ("Lissandra");
                case (131):
                    return ("Diana");
                case (133):
                    return ("Quinn");
                case (134):
                    return ("Syndra");
                case (136):
                    return ("Aurelion Sol");
                case (143):
                    return ("Zyra");
                case (150):
                    return ("Gnar");
                case (154):
                    return ("Zac");
                case (157):
                    return ("Yasuo");
                case (161):
                    return ("Vel'Koz");
                case (163):
                    return ("Taliyah");
                case (164):
                    return ("Camille");
                case (201):
                    return ("Braum");
                case (202):
                    return ("Jhin");
                case (203):
                    return ("Kindred");
                case (222):
                    return ("Jinx");
                case (223):
                    return ("Tahm Kench");
                case (236):
                    return ("Lucian");
                case (238):
                    return ("Zed");
                case (240):
                    return ("Kled");
                case (245):
                    return ("Ekko");
                case (254):
                    return ("Vi");
                case (266):
                    return ("Aatrox");
                case (267):
                    return ("Nami");
                case (268):
                    return ("Azir");
                case (412):
                    return ("Thresh");
                case (420):
                    return ("Illaoi");
                case (421):
                    return ("Rek'Sai");
                case (427):
                    return ("Ivern");
                case (429):
                    return ("Kalista");
                case (432):
                    return ("Bard");
                default:
                    return ("Unknown"); //UNKNOWN = next champion (anticipated update)
            }
        }

        public static double get_id_from_champion(string ChampionName)
        {
            switch (ChampionName)
            {
                case ("Annie"):
                    return (1);
                case ("Olaf"):
                    return (2);
                case ("Galio"):
                    return (3);
                case ("Twisted Fate"):
                    return (4);
                case ("Xin Zhao"):
                    return (5);
                case ("Urgot"):
                    return (6);
                case ("Leblanc"):
                    return (7);
                case ("Vladimir"):
                    return (8);
                case ("Fiddlesticks"):
                    return (9);
                case ("Kayle"):
                    return (10);
                case ("Master Yi"):
                    return (11);
                case ("Alistar"):
                    return (12);
                case ("Ryze"):
                    return (13);
                case ("Sion"):
                    return (14);
                case ("Sivir"):
                    return (15);
                case ("Soraka"):
                    return (16);
                case ("Teemo"):
                    return (17);
                case ("Tristana"):
                    return (18);
                case ("Warwick"):
                    return (19);
                case ("Nunu"):
                    return (20);
                case ("Miss Fortune"):
                    return (21);
                case ("Ashe"):
                    return (22);
                case ("Tryndamere"):
                    return (23);
                case ("Jax"):
                    return (24);
                case ("Morgana"):
                    return (25);
                case ("Zilean"):
                    return (26);
                case ("Singed"):
                    return (27);
                case ("Evelynn"):
                    return (28);
                case ("Twitch"):
                    return (29);
                case ("Karthus"):
                    return (30);
                case ("Cho'Gath"):
                    return (31);
                case ("Amumu"):
                    return (32);
                case ("Rammus"):
                    return (33);
                case ("Anivia"):
                    return (34);
                case ("Shaco"):
                    return (35);
                case ("Dr. Mundo"):
                    return (36);
                case ("Sona"):
                    return (37);
                case ("Kassadin"):
                    return (38);
                case ("Irelia"):
                    return (39);
                case ("Janna"):
                    return (40);
                case ("Gangplank"):
                    return (41);
                case ("Corki"):
                    return (42);
                case ("Karma"):
                    return (43);
                case ("Taric"):
                    return (44);
                case ("Veigar"):
                    return (45);
                case ("Trundle"):
                    return (48);
                case ("Swain"):
                    return (50);
                case ("Caitlyn"):
                    return (51);
                case ("Blitzcrank"):
                    return (53);
                case ("Malphite"):
                    return (54);
                case ("Katarina"):
                    return (55);
                case ("Nocturne"):
                    return (56);
                case ("Maokai"):
                    return (57);
                case ("Renekton"):
                    return (58);
                case ("Jarvan IV"):
                    return (59);
                case ("Elise"):
                    return (60);
                case ("Orianna"):
                    return (61);
                case ("Wukong"):
                    return (62);
                case ("Brand"):
                    return (63);
                case ("Lee Sin"):
                    return (64);
                case ("Vayne"):
                    return (67);
                case ("Rumble"):
                    return (68);
                case ("Cassiopeia"):
                    return (69);
                case ("Skarner"):
                    return (72);
                case ("Heimerdinger"):
                    return (74);
                case ("Nasus"):
                    return (75);
                case ("Nidalee"):
                    return (76);
                case ("Udyr"):
                    return (77);
                case ("Poppy"):
                    return (78);
                case ("Gragas"):
                    return (79);
                case ("Pantheon"):
                    return (80);
                case ("Ezreal"):
                    return (81);
                case ("Mordekaiser"):
                    return (82);
                case ("Yorick"):
                    return (83);
                case ("Akali"):
                    return (84);
                case ("Kennen"):
                    return (85);
                case ("Garen"):
                    return (86);
                case ("Leona"):
                    return (89);
                case ("Malzahar"):
                    return (90);
                case ("Talon"):
                    return (91);
                case ("Riven"):
                    return (92);
                case ("Kog'Maw"):
                    return (96);
                case ("Shen"):
                    return (98);
                case ("Lux"):
                    return (99);
                case ("Xerath"):
                    return (101);
                case ("Shyvana"):
                    return (102);
                case ("Ahri"):
                    return (103);
                case ("Graves"):
                    return (104);
                case ("Fizz"):
                    return (105);
                case ("Volibear"):
                    return (106);
                case ("Rengar"):
                    return (107);
                case ("Varus"):
                    return (110);
                case ("Nautilus"):
                    return (111);
                case ("Viktor"):
                    return (112);
                case ("Sejuani"):
                    return (113);
                case ("Fiora"):
                    return (114);
                case ("Ziggs"):
                    return (115);
                case ("Lulu"):
                    return (117);
                case ("Draven"):
                    return (119);
                case ("Hecarim"):
                    return (120);
                case ("Kha'Zix"):
                    return (121);
                case ("Darius"):
                    return (122);
                case ("Jayce"):
                    return (126);
                case ("Lissandra"):
                    return (127);
                case ("Diana"):
                    return (131);
                case ("Quinn"):
                    return (133);
                case ("Syndra"):
                    return (134);
                case ("Aurelion Sol"):
                    return (136);
                case ("Zyra"):
                    return (143);
                case ("Gnar"):
                    return (150);
                case ("Zac"):
                    return (154);
                case ("Yasuo"):
                    return (157);
                case ("Vel'Koz"):
                    return (161);
                case ("Taliyah"):
                    return (163);
                case ("Camille"):
                    return (164);
                case ("Braum"):
                    return (201);
                case ("Jhin"):
                    return (202);
                case ("Kindred"):
                    return (203);
                case ("Jinx"):
                    return (222);
                case ("Tahm Kench"):
                    return (223);
                case ("Lucian"):
                    return (236);
                case ("Zed"):
                    return (238);
                case ("Kled"):
                    return (240);
                case ("Ekko"):
                    return (245);
                case ("Vi"):
                    return (254);
                case ("Aatrox"):
                    return (266);
                case ("Nami"):
                    return (267);
                case ("Azir"):
                    return (268);
                case ("Thresh"):
                    return (412);
                case ("Illaoi"):
                    return (420);
                case ("Rek'Sai"):
                    return (421);
                case ("Ivern"):
                    return (427);
                case ("Kalista"):
                    return (429);
                case ("Bard"):
                    return (432);
                default:
                    return (0);
            }
        }

        public static int[] get_champion_role(string champion)
        {
            /*
             * 0 = top
             * 1 = jungle
             * 2 = mid
             * 3 = adc
             * 4 = support
             * 5 = Unknown role or champ
            */
            switch (champion)
            {
                case ("Annie"):
                    return (new int[] {4, 2, 3, 1, 0});
                case ("Olaf"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Galio"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Twisted Fate"):
                    return (new int[] { 2, 3, 4, 1, 0 });
                case ("Xin Zhao"):
                    return (new int[] { 1, 0, 3, 4, 2 });
                case ("Urgot"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Leblanc"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Vladimir"):
                    return (new int[] { 0, 2, 1, 4, 3 });
                case ("Fiddlesticks"):
                    return (new int[] { 1, 2, 4, 0, 3 });
                case ("Kayle"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Master Yi"):
                    return (new int[] { 1, 0, 2, 3, 4 });
                case ("Alistar"):
                    return (new int[] { 4, 0, 1, 2, 3 });
                case ("Ryze"):
                    return (new int[] { 0, 2, 1, 4, 3 });
                case ("Sion"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Sivir"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Soraka"):
                    return (new int[] { 4, 2, 0, 3, 1 });
                case ("Teemo"):
                    return (new int[] { 0, 2, 4, 3, 1 });
                case ("Tristana"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Warwick"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Nunu"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Miss Fortune"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Ashe"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Tryndamere"):
                    return (new int[] { 0, 1, 3, 4, 2 });
                case ("Jax"):
                    return (new int[] { 0, 1, 4, 3, 2 });
                case ("Morgana"):
                    return (new int[] { 4, 2, 0, 1, 3 });
                case ("Zilean"):
                    return (new int[] { 4, 2, 1, 0, 3 });
                case ("Singed"):
                    return (new int[] { 0, 1, 4, 2, 3 });
                case ("Evelynn"):
                    return (new int[] { 1, 2, 0, 4, 3 });
                case ("Twitch"):
                    return (new int[] { 3, 1, 2, 0, 4 });
                case ("Karthus"):
                    return (new int[] { 2, 1, 0, 4, 3 });
                case ("Cho'Gath"):
                    return (new int[] { 0, 2, 1, 4, 3 });
                case ("Amumu"):
                    return (new int[] { 1, 4, 0, 2, 3 });
                case ("Rammus"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Anivia"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Shaco"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Dr. Mundo"):
                    return (new int[] { 0, 1, 4, 2, 3 });
                case ("Sona"):
                    return (new int[] { 4, 2, 0, 1, 3 });
                case ("Kassadin"):
                    return (new int[] { 2, 0, 1, 4, 3 });
                case ("Irelia"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Janna"):
                    return (new int[] { 4, 2, 3, 1, 0 });
                case ("Gangplank"):
                    return (new int[] { 0, 1, 2, 3, 4 });
                case ("Corki"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Karma"):
                    return (new int[] { 4, 2, 0, 1, 3 });
                case ("Taric"):
                    return (new int[] { 4, 0, 1, 2, 3 });
                case ("Veigar"):
                    return (new int[] { 2, 0, 4, 1, 3 });
                case ("Trundle"):
                    return (new int[] { 0, 1, 4, 2, 3 });
                case ("Swain"):
                    return (new int[] { 0, 2, 1, 4, 3 });
                case ("Caitlyn"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Blitzcrank"):
                    return (new int[] { 4, 2, 3, 0, 1 });
                case ("Malphite"):
                    return (new int[] { 0, 2, 1, 4, 3 });
                case ("Katarina"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Nocturne"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Maokai"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Renekton"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Jarvan IV"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Elise"):
                    return (new int[] { 1, 2, 0, 4, 3 });
                case ("Orianna"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Wukong"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Brand"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Lee Sin"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Vayne"):
                    return (new int[] { 3, 0, 1, 2, 4 });
                case ("Rumble"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Cassiopeia"):
                    return (new int[] { 2, 0, 4, 1, 3 });
                case ("Skarner"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Heimerdinger"):
                    return (new int[] { 2, 0, 4, 1, 3 });
                case ("Nasus"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Nidalee"):
                    return (new int[] { 1, 2, 0, 4, 3 });
                case ("Udyr"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Poppy"):
                    return (new int[] { 0, 1, 4, 2, 3 });
                case ("Gragas"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Pantheon"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Ezreal"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Mordekaiser"):
                    return (new int[] { 0, 3, 2, 1, 4 });
                case ("Yorick"):
                    return (new int[] { 0, 1, 4, 2, 3 });
                case ("Akali"):
                    return (new int[] { 2, 0, 1, 4, 3 });
                case ("Kennen"):
                    return (new int[] { 0, 2, 3, 1, 4 });
                case ("Garen"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Leona"):
                    return (new int[] { 4, 2, 0, 1, 3 });
                case ("Malzahar"):
                    return (new int[] { 2, 0, 4, 1, 3 });
                case ("Talon"):
                    return (new int[] { 2, 0, 1, 4, 3 });
                case ("Riven"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Kog'Maw"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Shen"):
                    return (new int[] { 0, 4, 1, 2, 3 });
                case ("Lux"):
                    return (new int[] { 2, 4, 0, 3, 1 });
                case ("Xerath"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Shyvana"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Ahri"):
                    return (new int[] { 2, 0, 4, 3, 1 });
                case ("Graves"):
                    return (new int[] { 3, 1, 2, 0, 4 });
                case ("Fizz"):
                    return (new int[] { 2, 0, 1, 4, 3 });
                case ("Volibear"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Rengar"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Varus"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Nautilus"):
                    return (new int[] { 1, 4, 0, 2, 3 });
                case ("Viktor"):
                    return (new int[] { 2, 0, 4, 3, 1 });
                case ("Sejuani"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Fiora"):
                    return (new int[] { 0, 1, 2, 3, 4 });
                case ("Ziggs"):
                    return (new int[] { 2, 4, 0, 3, 1 });
                case ("Lulu"):
                    return (new int[] { 4, 2, 0, 3, 1 });
                case ("Draven"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Hecarim"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Kha'Zix"):
                    return (new int[] { 1, 2, 0, 4, 3 });
                case ("Darius"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Jayce"):
                    return (new int[] { 0, 2, 3, 4, 1 });
                case ("Lissandra"):
                    return (new int[] { 2, 0, 4, 1, 3 });
                case ("Diana"):
                    return (new int[] { 2, 1, 0, 4, 3 });
                case ("Quinn"):
                    return (new int[] { 0, 3, 1, 2, 4 });
                case ("Syndra"):
                    return (new int[] { 2, 0, 4, 3, 1 });
                case ("Aurelion Sol"):
                    return (new int[] { 2, 1, 0, 4, 3 });
                case ("Taliyah"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Camille"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Zyra"):
                    return (new int[] { 4, 2, 0, 1, 3 });
                case ("Gnar"):
                    return (new int[] { 0, 1, 3, 2, 4 });
                case ("Zac"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Yasuo"):
                    return (new int[] { 2, 0, 3, 1, 4 });
                case ("Vel'Koz"):
                    return (new int[] { 2, 4, 0, 1, 3 });
                case ("Braum"):
                    return (new int[] { 4, 0, 1, 2, 3 });
                case("Jhin"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Kindred"):
                    return (new int[] { 1, 3, 0, 2, 4 });
                case ("Jinx"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Tahm Kench"):
                    return (new int[] { 4, 1, 0, 2, 3 });
                case ("Lucian"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Zed"):
                    return (new int[] { 2, 0, 1, 3, 4 });
                case ("Kled"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Ekko"):
                    return (new int[] { 2, 1, 0, 4, 3 });
                case ("Vi"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Aatrox"):
                    return (new int[] { 1, 0, 2, 4, 3 });
                case ("Nami"):
                    return (new int[] { 4, 2, 0, 3, 1 });
                case ("Azir"):
                    return (new int[] { 2, 0, 4, 3, 1 });
                case ("Thresh"):
                    return (new int[] { 4, 3, 2, 1, 0 });
                case ("Illaoi"):
                    return (new int[] { 0, 1, 2, 4, 3 });
                case ("Rek'Sai"):
                    return (new int[] { 1, 0, 4, 2, 3 });
                case ("Ivern"):
                    return (new int[] { 1, 4, 0, 2, 3 });
                case ("Kalista"):
                    return (new int[] { 3, 2, 0, 4, 1 });
                case ("Bard"):
                    return (new int[] { 4, 2, 0, 3, 1 });
                default:
                    return (new int[] { 5, 5, 5, 5, 5 }); //UNKNOWN = next champion (anticipated update)
            }
        }

        public static string get_profileicon_details(double profileIcon)
        {
            switch (Convert.ToInt32(profileIcon))
            {
                case (0):
                    return ("(the super minion icon)");
                case (1):
                    return ("(the blue warrior minion icon)");
                case (2):
                    return ("(the blue canon minion icon)");
                case (3):
                    return ("(the blue mage minion icon)");
                case (4):
                    return ("(the blue mountain icon)");
                case (5):
                    return ("(the blue super minion icon)");
                case (6):
                    return ("(the tibber paw icon)");
                case (7):
                    return ("(the red rose icon)");
                case (8):
                    return ("(the golem icon)");
                case (9):
                    return ("(the 2 crossed swords icon)");
                case (10):
                    return ("(the sword and wings icon)");
                case (11):
                    return ("(the lezard icon)");
                case (12):
                    return ("(the magic book icon)");
                case (13):
                    return ("(the red canon minion icon)");
                case (14):
                    return ("(the red minion icon)");
                case (15):
                    return ("(the red warrior minion icon)");
                case (16):
                    return ("(the red mage minion icon)");
                case (17):
                    return ("(the red super minion icon)");
                case (18):
                    return ("(the green potion icon)");
                case (19):
                    return ("(the rock icon)");
                case (20):
                    return ("(the pyramid icon)");
                case (21):
                    return ("(the apple tree icon)");
                case (22):
                    return ("(the resurection icon)");
                case (23):
                    return ("(the plant icon)");
                case (24):
                    return ("(the magic shield icon)");
                case (25):
                    return ("(the wolf on right icon)");
                case (26):
                    return ("(the wolf on left icon)");
                case (27):
                    return ("(the deadly minions icon)");
                case (28):
                    return ("(the tibber icon)");
                case (501):
                    return ("(the Place Holder icon)");
                case (502):
                    return ("(the Original Pulse Fire icon)");
                case (503):
                    return ("(the Spiteful Specter icon)");
                case (504):
                    return ("(the Death Mask icon)");
                case (505):
                    return ("(the Vengeful Wraith icon)");
                case (506):
                    return ("(the Deadfall Treant icon)");
                case (507):
                    return ("(the Baleful Grasp icon)");
                case (508):
                    return ("(the Season 2 World Championship icon)");
                case (509):
                    return ("(the season 2 3v3 bronze icon)");
                case (510):
                    return ("(the season 2 5v5 bronze icon)");
                case (511):
                    return ("(the season 2 all queue bronze icon)");
                case (512):
                    return ("(the season 1 bronze icon)");
                case (513):
                    return ("(the season 2 bronze icon)");
                case (514):
                    return ("(the season 2 3v3 silver icon)");
                case (515):
                    return ("(the season 2 5v5 silver icon)");
                case (516):
                    return ("(the season 2 all queue silver icon)");
                case (517):
                    return ("(the season 1 silver icon)");
                case (518):
                    return ("(the season 2 silver icon)");
                case (519):
                    return ("(the season 2 3v3 gold icon)");
                case (520):
                    return ("(the season 2 5v5 gold icon)");
                case (521):
                    return ("(the season 2 all queue gold icon)");
                case (522):
                    return ("(the season 1 gold icon)");
                case (523):
                    return ("(the season 2 gold icon)");
                case (524):
                    return ("(the season 2 3v3 platinum icon)");
                case (525):
                    return ("(the season 2 5v5 platinum icon)");
                case (526):
                    return ("(the season 2 all queue platinum icon)");
                case (527):
                    return ("(the season 1 platinum icon)");
                case (528):
                    return ("(the season 2 platinum icon)");
                case (529):
                    return ("(the season 2 3v3 diamond icon)");
                case (530):
                    return ("(the season 2 5v5 diamond icon)");
                case (531):
                    return ("(the season 2 all queue diamond icon)");
                case (532):
                    return ("(the season 1 diamond icon)");
                case (533):
                    return ("(the Master Beta Tester icon)");
                case (534):
                    return ("(the Grand Master Beta Tester icon)");
                case (535):
                    return ("(the Holiday Shopkeeper icon)");
                case (536):
                    return ("(the Doran's Wreath icon)");
                case (537):
                    return ("(the Trimmed Turret icon)");
                case (538):
                    return ("(the Reindeer Urf icon)");
                case (539):
                    return ("(the Holiday Nashor icon)");
                case (540):
                    return ("(the Stocking Blades icon)");
                case (541):
                    return ("(the chinese new year ticket icon)");
                case (543):
                    return ("(the chinese new year urf icon)");
                case (544):
                    return ("(the chinese new year lantern icon)");
                case (545):
                    return ("(the chinese new year cobra icon)");
                case (546):
                    return ("(the freljord war ashe icon)");
                case (547):
                    return ("(the freljord war sejuani icon)");
                case (548):
                    return ("(the freljord war lissandra icon)");
                case (549):
                    return ("(the udyr spirit guard tiger icon)");
                case (550):
                    return ("(the udyr spirit guard bear icon)");
                case (551):
                    return ("(the udyr spirit guard turtle icon)");
                case (552):
                    return ("(the udyr spirit guard phoenix icon)");
                case (553):
                    return ("(the turret with sun icon)");
                case (554):
                    return ("(the season 3 Cloud 9 icon)");
                case (555):
                    return ("(the season 3 Curse icon)");
                case (556):
                    return ("(the season 3 Team Dignitas icon)");
                case (557):
                    return ("(the season 3 Team Coast icon)");
                case (558):
                    return ("(the season 3 TSM icon)");
                case (559):
                    return ("(the season 3 Velocity icon)");
                case (560):
                    return ("(the season 3 Vulcun icon)");
                case (561):
                    return ("(the season 3 CLG icon)");
                case (562):
                    return ("(the season 3 EG icon)");
                case (563):
                    return ("(the season 3 Fnatic icon)");
                case (564):
                    return ("(the season 3 Gambit icon)");
                case (565):
                    return ("(the season 3 Lemondogs icon)");
                case (566):
                    return ("(the season 3 MYM icon)");
                case (567):
                    return ("(the season 3 NIP icon)");
                case (568):
                    return ("(the season 3 SK Gaming icon)");
                case (569):
                    return ("(the season 3 Alternate icon)");
                case (570):
                    return ("(the season 3 Gaming Gear EU icon)");
                case (571):
                    return ("(the season 3 Pain Gaming icon)");
                case (572):
                    return ("(the season 3 Lyon Gaming icon)");
                case (573):
                    return ("(the season 3 Team Immunity icon)");
                case (574):
                    return ("(the season 3 Dark Passage icon)");
                case (575):
                    return ("(the Iceborn Keeper icon)");
                case (576):
                    return ("(the Spirit of the Altar icon)");
                case (577):
                    return ("(the Tomb Angel icon)");
                case (578):
                    return ("(the Vilemaw icon)");
                case (579):
                    return ("(the Morellonomicon icon)");
                case (580):
                    return ("(the old mythic skin owner icon)");
                case (581):
                    return ("(the season 3 bronze icon)");
                case (582):
                    return ("(the season 3 silver icon)");
                case (583):
                    return ("(the season 3 gold icon)");
                case (584):
                    return ("(the season 3 platinium icon)");
                case (585):
                    return ("(the season 3 diamond icon)");
                case (586):
                    return ("(the season 3 challenger icon)");
                case (588):
                    return ("(the poro present icon)");
                case (589):
                    return ("(the santa claus veigar icon)");
                case (590):
                    return ("(the candy cane icon)");
                case (591):
                    return ("(the snowman teemo icon)");
                case (592):
                    return ("(the santa claus gragas icon)");
                case (593):
                    return ("(the SK Telecom OGN 2014 icon)");
                case (594):
                    return ("(the Team NB OGN 2014 icon)");
                case (595):
                    return ("(the CJ Entus OGN 2014 icon)");
                case (596):
                    return ("(the NaJin e-mFire OGN 2014 icon)");
                case (597):
                    return ("(the Samsung Galaxy Ozone OGN 2014 icon)");
                case (598):
                    return ("(the Jin Air Greenwings OGN 2014 icon)");
                case (599):
                    return ("(the KT Rolster OGN 2014 icon)");
                case (600):
                    return ("(the Incredible Miracle OGN 2014 icon)");
                case (601):
                    return ("(the Alienware Arena OGN 2014 icon)");
                case (602):
                    return ("(the Xenics Storm OGN 2014 icon)");
                case (603):
                    return ("(the Year of the Horse icon)");
                case (604):
                    return ("(the Lunar Goddess icon)");
                case (605):
                    return ("(the Warring Kingdoms icon)");
                case (606):
                    return ("(the Dragonblade icon)");
                case (607):
                    return ("(the warrior path truth winner icon)");
                case (608):
                    return ("(the warrior path power winner icon)");
                case (609):
                    return ("(the warrior path freedom winner icon)");
                case (610):
                    return ("(the warrior path truth icon)");
                case (611):
                    return ("(the warrior path power icon)");
                case (612):
                    return ("(the warrior path freedom icon)");
                case (613):
                    return ("(the Alliance LCS 2014 icon)");
                case (614):
                    return ("(the Copenhagen Wolves LCS 2014 icon)");
                case (615):
                    return ("(the Fnatic LCS 2014 icon)");
                case (616):
                    return ("(the Gambit Gaming LCS 2014 icon)");
                case (617):
                    return ("(the Millenium LCS 2014 icon)");
                case (618):
                    return ("(the Roccat LCS 2014 icon)");
                case (619):
                    return ("(the SK Gaming LCS 2014 icon)");
                case (620):
                    return ("(the Supa Hot Crew LCS 2014 icon)");
                case (621):
                    return ("(the Counter Logic Gaming LCS 2014 icon)");
                case (622):
                    return ("(the Cloud9 LCS 2014 icon)");
                case (623):
                    return ("(the Team Coast LCS 2014 icon)");
                case (624):
                    return ("(the Curse LCS 2014 icon)");
                case (625):
                    return ("(the Team Dignitas LCS 2014 icon)");
                case (626):
                    return ("(the Evil Genuises LCS 2014 icon)");
                case (627):
                    return ("(the Team SoloMid LCS 2014 icon)");
                case (628):
                    return ("(the XDG LCS 2014 icon)");
                case (629):
                    return ("(the Saigon Jokers GPL 2014 icon)");
                case (630):
                    return ("(the Insidious Gaming GPL 2014 icon)");
                case (631):
                    return ("(the Imperium Pro Team GPL 2014 icon)");
                case (632):
                    return ("(the Neolution Esports Full Louis GPL 2014 icon)");
                case (633):
                    return ("(the Insidious Gaming Rebirth GPL 2014 icon)");
                case (634):
                    return ("(the Saigon Fantastic Five GPL 2014 icon)");
                case (657):
                    return ("(the team builder assassin icon)");
                case (658):
                    return ("(the team builder fighter icon)");
                case (659):
                    return ("(the team builder mage icon)");
                case (660):
                    return ("(the team builder marksman icon)");
                case (661):
                    return ("(the team builder support icon)");
                case (662):
                    return ("(the team builder tank icon)");
                case (663):
                    return ("(the all star 2014 icon)");
                case (664):
                    return ("(the one for all ziggs icon)");
                case (665):
                    return ("(the world football championship 2014 icon)");
                case (666):
                    return ("(the pentakill icon)");
                case (682):
                    return ("(the meca icon)");
                case (700):
                    return ("(the assassin taipei icon)");
                case (701):
                    return ("(the TSM icon)");
                case (702):
                    return ("(the OMG icon)");
                case (704):
                    return ("(the Fnatic icon)");
                case (709):
                    return ("(the Shadow Isles Cres icon)");
                case (710):
                    return ("(the Harrowed Puppet icon)");
                case (711):
                    return ("(the Mark of the Betrayer icon)");
                case (712):
                    return ("(the Blue Team Stag icon)");
                case (713):
                    return ("(the Red Team Owl icon)");
                case (714):
                    return ("(the solo queue bronze S4 icon)");
                case (715):
                    return ("(the solo queue silver S4 icon)");
                case (716):
                    return ("(the solo queue gold S4 icon)");
                case (717):
                    return ("(the solo queue platinium S4 icon)");
                case (718):
                    return ("(the solo queue diamond S4 icon)");
                case (719):
                    return ("(the solo queue master S4 icon)");
                case (720):
                    return ("(the solo queue challenger S4 icon)");
                case (721):
                    return ("(the solo queue challenger S4 icon)");
                case (722):
                    return ("(the solo queue challenger S4 icon)");
                case (723):
                    return ("(the 3v3 queue bronze S4 icon)");
                case (724):
                    return ("(the 3v3 queue silver S4 icon)");
                case (725):
                    return ("(the 3v3 queue gold S4 icon)");
                case (726):
                    return ("(the 3v3 queue platinium S4 icon)");
                case (727):
                    return ("(the 3v3 queue diamond S4 icon)");
                case (728):
                    return ("(the 3v3 queue master S4 icon)");
                case (729):
                    return ("(the 3v3 queue challenger S4 icon)");
                case (730):
                    return ("(the 3v3 queue challenger S4 icon)");
                case (731):
                    return ("(the 3v3 queue challenger S4 icon)");
                case (732):
                    return ("(the 5v5 queue bronze S4 icon)");
                case (733):
                    return ("(the 5v5 queue silver S4 icon)");
                case (734):
                    return ("(the 5v5 queue gold S4 icon)");
                case (735):
                    return ("(the 5v5 queue platinium S4 icon)");
                case (736):
                    return ("(the 5v5 queue diamond S4 icon)");
                case (737):
                    return ("(the 5v5 queue master S4 icon)");
                case (738):
                    return ("(the 5v5 queue challenger S4 icon)");
                case (739):
                    return ("(the 5v5 queue challenger S4 icon)");
                case (740):
                    return ("(the 5v5 queue challenger S4 icon)");
                case (741):
                    return ("(the meca icon)");
                case (742):
                    return ("(the meca confrontation icon)");
                case (743):
                    return ("(the meca poro icon)");
                case (744):
                    return ("(the poro gentleman icon)");
                case (745):
                    return ("(the dark poro icon)");
                case (746):
                    return ("(the astro poro icon)");
                case (747):
                    return ("(the dragon poro icon)");
                case (748):
                    return ("(the king poro icon)");
                case (749):
                    return ("(the thrown poro icon)");
                case (750):
                    return ("(the blood moon icon)");
                case (751):
                    return ("(the Winterfox LCS 2015 icon)");
                case (752):
                    return ("(the TSM LCS 2015 icon)");
                case (753):
                    return ("(the Team Liquid LCS 2015 icon)");
                case (754):
                    return ("(the TiP LCS 2015 icon)");
                case (755):
                    return ("(the Dignitas LCS 2015 icon)");
                case (756):
                    return ("(the Team Coast LCS 2015 icon)");
                case (757):
                    return ("(the Team8 LCS 2015 icon)");
                case (758):
                    return ("(the CLG LCS 2015 icon)");
                case (759):
                    return ("(the Cloud9 LCS 2015 icon)");
                case (760):
                    return ("(the SK LCS 2015 icon)");
                case (761):
                    return ("(the Unicorn of Love LCS 2015 icon)");
                case (762):
                    return ("(the Roccat LCS 2015 icon)");
                case (763):
                    return ("(the mYm LCS 2015 icon)");
                case (764):
                    return ("(the H2K LCS 2015 icon)");
                case (765):
                    return ("(the Giants LCS 2015 icon)");
                case (766):
                    return ("(the M5 LCS 2015 icon)");
                case (767):
                    return ("(the Fnatic LCS 2015 icon)");
                case (768):
                    return ("(the Element LCS 2015 icon)");
                case (769):
                    return ("(the Copennagen Wolves LCS 2015 icon)");
                case (770):
                    return ("(the dragon rocket icon)");
                case (771):
                    return ("(the fizz urf icon)");
                case (772):
                    return ("(the Gravity LCS 2015 icon)");
                case (774):
                    return ("(the honorable player 2015 icon)");
                case (775):
                    return ("(the Sweetheart icon)");
                case (776):
                    return ("(the Party Teemo icon)");
                case (777):
                    return ("(the Masked Teemo icon)");
                case (778):
                    return ("(the DJ Sona icon)");
                case (779):
                    return ("(the Red DJ Sona icon)");
                case (780):
                    return ("(the Blue DJ Sona icon)");
                case (781):
                    return ("(the Nemesis icon)");
                case (782):
                    return ("(the Heartsplosion icon)");
                case (783):
                    return ("(the Orbital Laser icon)");
                case (784):
                    return ("(the Spooky Urf icon)");
                case (785):
                    return ("(the Thinking Manatee icon)");
                case (786):
                    return ("(the 99,999 RP icon)");
                case (787):
                    return ("(the Omega Squad Teemo icon)");
                case (788):
                    return ("(the Zone esport 2015 icon)");
                case (789):
                    return ("(the TCL 2015 Team TurQuality icon)");
                case (790):
                    return ("(the TCL 2015 NumberOne icon)");
                case (791):
                    return ("(the TCL 2015 HWA Gaming icon)");
                case (792):
                    return ("(the TCL 2015 Dark Passage icon)");
                case (793):
                    return ("(the TCL 2015 Big Plays Incorporated icon)");
                case (794):
                    return ("(the TCL 2015 BJK icon)");
                case (795):
                    return ("(the TCL 2015 ARLAS icon)");
                case (796):
                    return ("(the TCL 2015 Rebirth icon)");
                case (897):
                    return ("(the Bilgewater 1 - Graves icon)");
                case (898):
                    return ("(the Bilgewater 1 - Twisted Fate icon)");
                case (899):
                    return ("(the Bilgewater 2 - Graves icon)");
                case (900):
                    return ("(the Bilgewater 2 - Twisted Fate icon)");
                case (901):
                    return ("(the Bilgewater 3 - Twisted Fate & Graves icon)");
                case (902):
                    return ("(the Bilgewater 3 - Gangplank icon)");
                case (903):
                    return ("(the Bilgewater flag icon)");
                case (904):
                    return ("(the arcade singer poro icon)");
                case (905):
                    return ("(the arcade sona poro icon)");
                case (906):
                    return ("(the dragon fire icon)");
                case (907):
                    return ("(the blitzcrank's poro roundup icon)");
                case (908):
                    return ("(the arcade final boss icon)");
                case (909):
                    return ("(the PROJECT icon)");
                case (910):
                    return ("(the PROJECT: YI icon)");
                case (911):
                    return ("(the PROJECT: ZED icon)");
                case (912):
                    return ("(the PROJECT: LEONA icon)");
                case (913):
                    return ("(the PROJECT: FIORA icon)");
                case (914):
                    return ("(the PROJECT: YASUO icon)");
                case (915):
                    return ("(the CLG Worlds 2015 icon)");
                case (916):
                    return ("(the TSM Worlds 2015 icon)");
                case (917):
                    return ("(the Cloud9 Worlds 2015 icon)");
                case (918):
                    return ("(the Fnatic Worlds 2015 icon)");
                case (919):
                    return ("(the H2K Worlds 2015 icon)");
                case (920):
                    return ("(the Origen Worlds 2015 icon)");
                case (921):
                    return ("(the LGD Gaming Worlds 2015 icon)");
                case (922):
                    return ("(the EDG Worlds 2015 icon)");
                case (923):
                    return ("(the Invictus Gaming Worlds 2015 icon)");
                case (924):
                    return ("(the SKT T1 Worlds 2015 icon)");
                case (925):
                    return ("(the KOO Tigers Worlds 2015 icon)");
                case (926):
                    return ("(the KT Rolster Worlds 2015 icon)");
                case (927):
                    return ("(the Flash Wolves Worlds 2015 icon)");
                case (928):
                    return ("(the AHQ Worlds 2015 icon)");
                case (929):
                    return ("(the Bangkok Titans Worlds 2015 icon)");
                case (930):
                    return ("(the Pain Gaming Worlds 2015 icon)");
                case (931):
                    return ("(the World Championship 2015 icon)");
                case (932):
                    return ("(the scuttler icon)");
                case (933):
                    return ("(the wolves icon)");
                case (934):
                    return ("(the raptor icon)");
                case (935):
                    return ("(the blue sentinel icon)");
                case (936):
                    return ("(the poro pick'em group stage icon)");
                case (937):
                    return ("(the poro pick'em icon)");
                case (938):
                    return ("(the Kindred Lamb icon)");
                case (939):
                    return ("(the Kindred Wolf icon)");
                case (940):
                    return ("(the SKT T1 2015 final icon");
                case (941):
                    return ("(the Origen 2015 final icon");
                case (942):
                    return ("(the Fnatic 2015 final icon");
                case (943):
                    return ("(the KOO Tigers 2015 final icon");
                case (944):
                    return ("(the All-Star 2015 Fire icon");
                case (945):
                    return ("(the All-Star 2015 Ice icon");
                case (946):
                    return ("(the All-Star 2015 icon");
                case (948):
                    return ("(the zombie Brand icon");
                case (949):
                    return ("(the slayer Pantheon icon");
                case (950):
                    return ("(the solo queue bronze S5 icon)");
                case (951):
                    return ("(the 3v3 queue bronze S5 icon)");
                case (952):
                    return ("(the 5v5 queue bronze S5 icon)");
                case (953):
                    return ("(the solo queue silver S5 icon)");
                case (954):
                    return ("(the 3v3 queue silver S5 icon)");
                case (955):
                    return ("(the 5v5 queue silver S5 icon)");
                case (956):
                    return ("(the solo queue gold S5 icon)");
                case (957):
                    return ("(the 3v3 queue gold S5 icon)");
                case (958):
                    return ("(the 5v5 queue gold S5 icon)");
                case (959):
                    return ("(the solo queue platinium S5 icon)");
                case (960):
                    return ("(the 3v3 queue platinium S5 icon)");
                case (961):
                    return ("(the 5v5 queue platinium S5 icon)");
                case (962):
                    return ("(the solo queue diamond S5 icon)");
                case (963):
                    return ("(the 3v3 queue diamond S5 icon)");
                case (964):
                    return ("(the 5v5 queue diamond S5 icon)");
                case (965):
                    return ("(the solo queue master S5 icon)");
                case (966):
                    return ("(the 3v3 queue master S5 icon)");
                case (967):
                    return ("(the 5v5 queue master S5 icon)");
                case (968):
                    return ("(the solo queue challenger S5 icon)");
                case (969):
                    return ("(the 3v3 queue challenger S5 icon)");
                case (970):
                    return ("(the 5v5 queue challenger S5 icon)");
                case (971):
                    return ("(the solo queue top 10 challenger S5 icon)");
                case (972):
                    return ("(the 3v3 queue top 10 challenger S5 icon)");
                case (973):
                    return ("(the 5v5 queue top 10 challenger S5 icon)");
                case (974):
                    return ("(the solo queue top 3 challenger S5 icon)");
                case (975):
                    return ("(the 3v3 queue top 3 challenger S5 icon)");
                case (976):
                    return ("(the 5v5 queue top 3 challenger S5 icon)");
                case (977):
                    return ("(the frog icon)");
                case (978):
                    return ("(the duck icon)");
                case (979):
                    return ("(the icy minion icon)");
                case (980):
                    return ("(the bundled minion icon)");
                case (981):
                    return ("(the snowball 4 icon)");
                case (982):
                    return ("(the snowball 2 icon)");
                case (983):
                    return ("(the snowball 3 icon)");
                case (984):
                    return ("(the snowball 1 icon)");
                case (985):
                    return ("(the PROJECT: poro icon)");
                case (986):
                    return ("(the Shadow Wolf icon)");
                case (987):
                    return ("(the Nightshade Serpent icon)");
                case (988):
                    return ("(the Monkey King icon)");
                case (989):
                    return ("(the Lunar Guardian icon)");
                case (990):
                    return ("(the 2016 OPL Avant Garde icon)");
                case (991):
                    return ("(the 2016 OPL The Chiefs eSports Club icon)");
                case (992):
                    return ("(the 2016 OPL Dire Wolves icon)");
                case (993):
                    return ("(the 2016 OPL Hellions e-Sports Club icon)");
                case (994):
                    return ("(the 2016 OPL Infernum icon)");
                case (995):
                    return ("(the 2016 OPL Legacy eSports icon)");
                case (996):
                    return ("(the 2016 OPL Sin Gaming icon)");
                case (997):
                    return ("(the 2016 OPL Trident Esports icon)");
                case (998):
                    return ("(the 2016 OPL Team Differencial icon)");
                case (999):
                    return ("(the 2016 OPL Team Empire icon)");
                case (1000):
                    return ("(the 2016 OPL Hard Random icon)");
                case (1001):
                    return ("(the 2016 LCL Team Just icon)");
                case (1002):
                    return ("(the 2016 LCL Natus Vincere icon)");
                case (1003):
                    return ("(the 2016 LCL RoX icon)");
                case (1004):
                    return ("(the 2016 LCL Vega Squadron icon)");
                case (1005):
                    return ("(the 2016 LCL Vaevictis eSports icon)");
                case (1006):
                    return ("(the 2016 LMS ahq e-Sports Club icon)");
                case (1007):
                    return ("(the 2016 LMS Flash Wolves icon)");
                case (1008):
                    return ("(the 2016 LMS Hong Kong Esports icon)");
                case (1009):
                    return ("(the 2016 LMS Machi E-Sports icon)");
                case (1010):
                    return ("(the 2016 LMS Midnight Sun icon)");
                case (1011):
                    return ("(the 2016 LMS Taipei Assassins icon)");
                case (1012):
                    return ("(the 2016 NA LCS Cloud9 icon)");
                case (1013):
                    return ("(the 2016 NA LCS CLG icon)");
                case (1014):
                    return ("(the 2016 NA LCS Team Dignitas icon)");
                case (1015):
                    return ("(the 2016 NA LCS Echo Fox icon)");
                case (1016):
                    return ("(the 2016 NA LCS Immortals icon)");
                case (1017):
                    return ("(the 2016 NA LCS NRG eSports icon)");
                case (1018):
                    return ("(the 2016 NA LCS Renegades icon)");
                case (1019):
                    return ("(the 2016 NA LCS Team Impulse icon)");
                case (1020):
                    return ("(the 2016 NA LCS Team Liquid icon)");
                case (1021):
                    return ("(the 2016 NA LCS Team SoloMid icon)");
                case (1022):
                    return ("(the 2016 EU LCS Elements icon)");
                case (1023):
                    return ("(the 2016 EU LCS Fnatic icon)");
                case (1024):
                    return ("(the 2016 EU LCS Giants Gaming icon)");
                case (1025):
                    return ("(the 2016 EU LCS G2 Esports icon)");
                case (1026):
                    return ("(the 2016 EU LCS H2k-Gaming icon)");
                case (1027):
                    return ("(the 2016 EU LCS Origen icon)");
                case (1028):
                    return ("(the 2016 EU LCS Team ROCCAT icon)");
                case (1029):
                    return ("(the 2016 EU LCS Splyce icon)");
                case (1030):
                    return ("(the 2016 EU LCS Unicornes Of Love icon)");
                case (1031):
                    return ("(the 2016 EU LCS Team Vitality icon)");
                case (1032):
                    return ("(the 2016 LAN Tesla E-Sports icon)");
                case (1033):
                    return ("(the 2016 LAN Lyon Gaming icon)");
                case (1034):
                    return ("(the 2016 LAN Revenge eSports icon)");
                case (1035):
                    return ("(the 2016 LAN Havoks Gaming icon)");
                case (1036):
                    return ("(the 2016 LAN Galactic Gamers icon)");
                case (1037):
                    return ("(the 2016 LAN Dash9 Gamers icon)");
                case (1038):
                    return ("(the 2016 LAS Furious Gaming icon)");
                case (1039):
                    return ("(the 2016 LAS Hafnet eSports icon)");
                case (1040):
                    return ("(the 2016 LAS Isurus Gaming icon)");
                case (1041):
                    return ("(the 2016 LAS Kaos Latin Gamers icon)");
                case (1042):
                    return ("(the 2016 LAS Last Kings icon)");
                case (1043):
                    return ("(the 2016 LAS Rebirth eSports icon)");
                case (1044):
                    return ("(the 2016 LCK CJ Entus icon)");
                case (1045):
                    return ("(the 2016 LCK Jin Air Green Wings icon)");
                case (1046):
                    return ("(the 2016 LCK Incredible Miracle icon)");
                case (1047):
                    return ("(the 2016 LCK ROX Tigers icon)");
                case (1048):
                    return ("(the 2016 LCK KT Rolster icon)");
                case (1049):
                    return ("(the 2016 LCK Rebels Anarchy icon)");
                case (1050):
                    return ("(the 2016 LCK e-mFire icon)");
                case (1051):
                    return ("(the 2016 LCK Samsung Galaxy icon)");
                case (1052):
                    return ("(the 2016 LCK SBENU Soniboom icon)");
                case (1053):
                    return ("(the 2016 LCK SK Telecom T1 icon)");
                case (1054):
                    return ("(the 2016 Edward Gaming icon)");
                case (1055):
                    return ("(the 2016 Energy pacemaker icon)");
                case (1065):
                    return ("(the 2016 CNB e-Sports Club icon)");
                case (1066):
                    return ("(the 2016 g3enerationX icon)");
                case (1067):
                    return ("(the 2016 INTZ e-Sports icon)");
                case (1068):
                    return ("(the 2016 JAYOB e-Sports icon)");
                case (1069):
                    return ("(the 2016 KaBuM! e-Sports icon)");
                case (1070):
                    return ("(the 2016 Keyd Stars icon)");
                case (1071):
                    return ("(the 2016 paiN Gaming icon)");
                case (1072):
                    return ("(the 2016 RED Canids icon)");
                case (1073):
                    return ("(the 2016 CILEKLER icon)");
                case (1074):
                    return ("(the 2016 Dark Passage icon)");
                case (1075):
                    return ("(the 2016 HWA Gaming icon)");
                case (1076):
                    return ("(the 2016 NumberOne icon)");
                case (1077):
                    return ("(the 2016 Oyun Hizmetleri icon)");
                case (1078):
                    return ("(the 2016 SuperMassive eSports icon)");
                case (1079):
                    return ("(the 2016 Team AURORA icon)");
                case (1080):
                    return ("(the 2016 Team Turquality icon)");
                case (1081):
                    return ("(the 2016 Snake e-Sports icon)");
                case (1082):
                    return ("(the 2016 Edward Gaming icon)");
                case (1083):
                    return ("(the 2016 Energy Pacemaker icon)");
                case (1084):
                    return ("(the 2016 HY Gaming icon)");
                case (1085):
                    return ("(the 2016 Invictus Gaming icon)");
                case (1086):
                    return ("(the 2016 LGD Gaming icon)");
                case (1087):
                    return ("(the 2016 Master3 icon)");
                case (1088):
                    return ("(the 2016 Royal Never Give Up icon)");
                case (1089):
                    return ("(the 2016 Snake eSports icon)");
                case (1090):
                    return ("(the 2016 Vici Gaming icon)");
                case (1091):
                    return ("(the 2016 OMG icon)");
                case (1092):
                    return ("(the 2016 Qiao Gu icon)");
                case (1093):
                    return ("(the 2016 Team WE icon)");
                case (1094):
                    return ("(the 2016 BJK icon)");
                case (1095):
                    return ("(the 2016 Besiktas e-Sports Club icon)");
                case (1096):
                    return ("(the 2016 Cougar eSports icon)");
                case (1097):
                    return ("(the 2016 Hong Kong eSports icon)");
                case (1098):
                    return ("(the 2016 eXtreme Gamers eSports Club icon)");
                case (1099):
                    return ("(the 2016 ROX Tigers icon)");
                case (1100):
                    return ("(the 2016 e-mFire icon)");
                case (1101):
                    return ("(the 2016 Afreeca Freecs icon)");
                case (1102):
                    return ("(the 2016 Longzhu Gaming icon)");
                case (1103):
                    return ("(the OCean Week 2016 icon)");
                case (1104):
                    return ("(the Heart Key icon)");
                case (1105):
                    return ("(the Sweetheart icon)");
                case (1106):
                    return ("(the Love Song icon)");
                case (1107):
                    return ("(the Blood Moon Kennen icon)");
                case (1108):
                    return ("(the Blood Moon Yasuo icon)");
                case (1109):
                    return ("(the Blood Moon Seal icon)");
                case (1110):
                    return ("(the Dominion 100 wins icon)");
                case (1111):
                    return ("(the Draven gold icon)");
                case (1112):
                    return ("(the Draven minion icon)");
                case (1113):
                    return ("(the Draven Draven icon icon)");
                case (1114):
                    return ("(the Maokai urf 2016 icon)");
                case (1115):
                    return ("(the Definitly not Vel'koz icon)");
                case (1116):
                    return ("(the Urf 2016 icon)");
                case (1117):
                    return ("(the 2016 MSI NA icon)");
                case (1118):
                    return ("(the 2016 MSI BR icon)");
                case (1119):
                    return ("(the 2016 MSI CIS icon)");
                case (1120):
                    return ("(the 2016 MSI LPL icon)");
                case (1121):
                    return ("(the 2016 MSI EU icon)");
                case (1122):
                    return ("(the 2016 MSI Japan icon)");
                case (1123):
                    return ("(the 2016 MSI LCK icon)");
                case (1124):
                    return ("(the 2016 MSI LAN icon)");
                case (1125):
                    return ("(the 2016 MSI LAS icon)");
                case (1126):
                    return ("(the 2016 MSI LMS icon)");
                case (1127):
                    return ("(the 2016 MSI OPL icon)");
                case (1128):
                    return ("(the 2016 MSI GPL icon)");
                case (1129):
                    return ("(the 2016 MSI TR icon)");
                case (1130):
                    return ("(the 2016 MSI icon)");
                case (1132):
                    return ("(the 00 Reactivated icon)");
                case (1133):
                    return ("(the Superior Prototype icon)");
                case (1134):
                    return ("(the Mecha Zero icon)");
                case (1135):
                    return ("(the Prototype 00 icon)");
                case (1136):
                    return ("(the 2016 MSI Final NA icon)");
                case (1139):
                    return ("(the 2016 MSI Final LPL icon)");
                case (1140):
                    return ("(the 2016 MSI Final EU icon)");
                case (1142):
                    return ("(the 2016 MSI Final LCK icon)");
                case (1145):
                    return ("(the 2016 MSI Final LMS icon)");
                case (1148):
                    return ("(the 2016 MSI Final TR icon)");
                case (1149):
                    return ("(the Omen of the Damned icon)");
                case (1150):
                    return ("(the Omen of the Iron Inquisitor icon)");
                case (1151):
                    return ("(the Omen of the Cursed Revenant icon)");
                case (1152):
                    return ("(the Omen of the Black Scourge icon)");
                case (1153):
                    return ("(the Super Galaxy Fizz icon)");
                case (1154):
                    return ("(the Mega Shark icon)");
                case (1155):
                    return ("(the Super Galaxy Lamb icon)");
                case (1156):
                    return ("(the Super Galaxy Wolf icon)");
                case (1157):
                    return ("(the Super Galaxy Shyvana icon)");
                case (1158):
                    return ("(the Mega Dragon icon)");
                case (1159):
                    return ("(the El Tigre Braum icon)");
                case (1160):
                    return ("(the El Leon Gnar icon)");
                case (1161):
                    return ("(the El Macho Mundo icon)");
                case (1162):
                    return ("(the El Rayo Volibear icon)");
                case (1163):
                    return ("(the 2016 Summer LCS Apex Gaming icon)");
                case (1164):
                    return ("(the 2016 Summer LCS Schalke 04 icon)");
                case (1167):
                    return ("(the 2016 Summer LCS Ever icon)");
                case (1168):
                    return ("(the 2016 Summer LCS MVP icon)");
                case (1169):
                    return ("(the 2016 Summer LCS J Gaming icon)");
                case (1170):
                    return ("(the 2016 Summer LCS Boba Marines icon)");
                case (1171):
                    return ("(the 2016 Summer LCS Saigon Jokers icon)");
                case (1172):
                    return ("(the 2016 Summer LCS Super Hype Gaming icon)");
                case (1173):
                    return ("(the 2016 Summer LCS An Phat Ultimate icon)");
                case (1174):
                    return ("(the 2016 Summer LCS Hai Anh T Team icon)");
                case (1175):
                    return ("(the 2016 Summer LCS ToRa 269 icon)");
                case (1176):
                    return ("(the 2016 Summer LCS Zotac United icon)");
                case (1177):
                    return ("(the 2016 Summer LCS Cantho Cherry icon)");
                case (1178):
                    return ("(the 2016 Summer LCS Fate Team icon)");
                case (1179):
                    return ("(the 2016 Summer LCS SkyRed icon)");
                case (1180):
                    return ("(the 2016 Summer LCS HavoksGaming icon)");
                case (1181):
                    return ("(the 2016 Summer LCS ZAGA Talent Gaming icon)");
                case (1182):
                    return ("(the 2016 Summer LCS BrawL.Lan icon)");
                case (1183):
                    return ("(the 2016 Summer LCS Kaos Latin Gamers icon)");
                case (1184):
                    return ("(the 2016 Summer LCS Operation Kino e-Sports icon)");
                case (1185):
                    return ("(the 2016 Summer LCS Big Gods Esports icon)");
                case (1186):
                    return ("(the 2016 Summer LCS Besiktas e-Sports Club icon)");
                case (1187):
                    return ("(the 2016 Summer LCS ANX icon)");
                case (1188):
                    return ("(the 2016 Summer LCS Team Just.MSI icon)");
                case (1189):
                    return ("(the 2016 Summer LCS TORNADO ROX icon)");
                case (1190):
                    return ("(the 2016 Summer LCS Gambit Gaming icon)");
                case (1191):
                    return ("(the 2016 Summer LCS Abyss Esports icon)");
                case (1192):
                    return ("(the 2016 Summer LCS Tainted Minds icon)");
                case (1193):
                    return ("(the 2016 Summer LCS DetonatioN Gaming icon)");
                case (1194):
                    return ("(the 2016 Summer LCS Rampage, 7th Heaven icon)");
                case (1195):
                    return ("(the 2016 Summer LCS 7th Heaven icon)");
                case (1196):
                    return ("(the 2016 Summer LCS BlackEye icon)");
                case (1197):
                    return ("(the 2016 Summer LCS Unsold Stuff Gaming icon)");
                case (1198):
                    return ("(the 2016 Summer LCS Scarz icon)");
                case (1199):
                    return ("(the 2016 Summer LCS Royal Never Give Up icon)");
                case (1200):
                    return ("(the 2016 Summer LCS Oh My God icon)");
                case (1201):
                    return ("(the 2016 Summer LCS NewBee icon)");
                case (1202):
                    return ("(the 2016 Summer LCS I MAY icon)");
                case (1203):
                    return ("(the 2016 Summer LCS Invictus Gaming icon)");
                case (1204):
                    return ("(the 2016 Summer LCS Edward Gaming icon)");
                case (1205):
                    return ("(the 2016 Summer LCS Vici Gaming icon)");
                case (1206):
                    return ("(the 2016 Summer LCS Snake Esports icon)");
                case (1207):
                    return ("(the 2016 Summer LCS Game Talents icon)");
                case (1208):
                    return ("(the 2016 Summer LCS Team World Elite icon)");
                case (1209):
                    return ("(the 2016 Summer LCS LGD Gaming icon)");
                case (1210):
                    return ("(the 2016 Summer LCS Saint Gaming icon)");
                case (1211):
                    return ("(the Oblivion icon)");
                case (1212):
                    return ("(the Dark Star Varus icon)");
                case (1213):
                    return ("(the Dark Star Thresh icon)");
                case (1214):
                    return ("(the 2016 NA LCS Phoenix1 icon)");
                case (1215):
                    return ("(the 2016 NA LCS Team Envy icon)");
                case (1216):
                    return ("(the 2016 LMS Team Mist icon)");
                case (1219):
                    return ("(the Soy LoLero icon)");
                case (1225):
                    return ("(the recon PROJECT: Ashe icon)");
                case (1226):
                    return ("(the EM Caster icon icon)");
                case (1227):
                    return ("(the PROJECT: Ashe icon)");
                case (1228):
                    return ("(the PROJECT: Ekko icon)");
                case (1229):
                    return ("(the Decrypter icon icon)");
                case (1230):
                    return ("(the Hyper Edge icon icon)");
                case (1231):
                    return ("(the PROJECT: Katarina icon)");
                case (1232):
                    return ("(the Disruption icon icon)");
                case (1233):
                    return ("(the FIRST STRIKE PROJECT: Ashe icon)");
                case (1234):
                    return ("(the FIRST STRIKE PROJECT: Ekko icon)");
                case (1235):
                    return ("(the FIRST STRIKE PROJECT: Katarina icon)");
                case (1236):
                    return ("(the Cloud9 Worlds 2016 icon)");
                case (1237):
                    return ("(the Team Envy Worlds 2016 icon)");
                case (1238):
                    return ("(the Immortals Worlds 2016 icon)");
                case (1239):
                    return ("(the CLG Worlds 2016 icon)");
                case (1240):
                    return ("(the Team Liquid Worlds 2016 icon)");
                case (1241):
                    return ("(the TSM Worlds 2016 icon)");
                case (1242):
                    return ("(the G2 Esports Worlds 2016 icon)");
                case (1243):
                    return ("(the Splyce Worlds 2016 icon)");
                case (1244):
                    return ("(the Giants Worlds 2016 icon)");
                case (1245):
                    return ("(the H2K Worlds 2016 icon)");
                case (1246):
                    return ("(the Fnatic Worlds 2016 icon)");
                case (1247):
                    return ("(the Unicorns of Love Worlds 2016 icon)");
                case (1248):
                    return ("(the Edward Gaming Worlds 2016 icon)");
                case (1249):
                    return ("(the Royal Never Give Up Worlds 2016 icon)");
                case (1250):
                    return ("(the Team WE Worlds 2016 icon)");
                case (1251):
                    return ("(the IMay Worlds 2016 icon)");
                case (1252):
                    return ("(the Snake Worlds 2016 icon)");
                case (1253):
                    return ("(the Vici Gaming Worlds 2016 icon)");
                case (1258):
                    return ("(the ROX Tigers Worlds 2016 icon)");
                case (1259):
                    return ("(the SKT T1 Worlds 2016 icon)");
                case (1260):
                    return ("(the Samsung Galaxy Worlds 2016 icon)");
                case (1261):
                    return ("(the KT Rolster Worlds 2016 icon)");
                case (1262):
                    return ("(the Afreeca Freecs Worlds 2016 icon)");
                case (1264):
                    return ("(the Jin Air Green Wings Worlds 2016 icon)");
                case (1265):
                    return ("(the Albus Nox Luna Worlds 2016 icon)");
                case (1269):
                    return ("(the J Team Worlds 2016 icon)");
                case (1270):
                    return ("(the Flash Wolves Worlds 2016 icon)");
                case (1271):
                    return ("(the AHQ Worlds 2016 icon)");
                case (1272):
                    return ("(the Hong Kong Esports Worlds 2016 icon)");
                case (1278):
                    return ("(the Saigon Jokers Worlds 2016 icon)");
                case (1286):
                    return ("(the Kaos Latin Gamers Worlds 2016 icon)");
                case (1281):
                    return ("(the Dark Passage Worlds 2016 icon)");
                case (1287):
                    return ("(the INTZ Worlds 2016 icon)");
                case (1288):
                    return ("(the Rampage Worlds 2016 icon)");
                case (1291):
                    return ("(the Lyon Gaming Worlds 2016 icon)");
                case (1292):
                    return ("(the Havoks Gaming Worlds 2016 icon)");
                case (1294):
                    return ("(the The Chief eSports Club Worlds 2016 icon)");
                case (1295):
                    return ("(the Arcade Corki icon)");
                case (1296):
                    return ("(the Arcade Ahri icon)");
                case (1297):
                    return ("(the Arcade Ezreal icon)");
                case (1298):
                    return ("(the Power Up icon)");
                case (1300):
                    return ("(the Machi E-Sports Worlds 2016 icon)");
                case (1301):
                    return ("(the Party Hardy icon)");
                case (2072):
                    return ("(the Crabby Crab icon)");
                case (2073):
                    return ("(the Come at me Crab icon)");
                case (2074):
                    return ("(the Cool Crab icon)");
                case (2075):
                    return ("(the Lifesaver icon)");
                case (2076):
                    return ("(the Rubber Ducky icon)");
                case (2077):
                    return ("(the Shock and Shower icon)");
                default:
                    return ("(no description for this icon)");
            }
        }
        
        public static void print_n_space(int n)
        {
            for (int i = 0; i < n; i++)
                Console.Write(" ");
        }

        public static string get_n_space(int n)
        {
            string result = "";
            for (int i = 0; i < n; i++)
                result += " ";
            return result;
        }

        public static int max_length(List<string> element, int start_max, int nb_elt = -1)
        {
            if (nb_elt == -1)
            {
                nb_elt = element.Count;
            }
            for (int i = 0; i < element.Count && i < nb_elt; i++)
            {
                if (element[i].Length > start_max)
                {
                    start_max = element[i].Length;
                }
            }

            return (start_max);
        }

        public static int max_length_name(List<Summoner> element, int start_max)
        {
            for (int i = 0; i < element.Count; i++)
            {
                if (element[i] != null && element[i].name.Length > start_max)
                {
                    start_max = element[i].name.Length;
                }
            }

            return (start_max);
        }

        public static int max_length_division(List<Summoner> element, int start_max)
        {
            for (int i = 0; i < element.Count; i++)
            {
                if (element[i] != null && element[i].get_ranked_format().Length > start_max)
                {
                    start_max = element[i].get_ranked_format().Length;
                }
            }

            return (start_max);
        }

        public static int max_length_main(List<Summoner> element, int start_max)
        {
            for (int i = 0; i < element.Count; i++)
            {
                if (element[i] != null && element[i].main_champions_list[0].Length > start_max)
                {
                    start_max = element[i].main_champions_list[0].Length;
                }
            }

            return (start_max);
        }

        public static int max_length(List<double> element, int start_max, int nb_elt = -1)
        {
            string elt;
            if(nb_elt == -1)
            {
                nb_elt = element.Count;
            }
            for (int i = 0; i < element.Count && i < nb_elt; i++)
            {
                elt = (Convert.ToInt32(element[i])).ToString();
                if (elt.Length > start_max)
                {
                    start_max = elt.Length;
                }
            }

            return (start_max);
        }

        public static string get_map_from_id(double id_map)
        {
            switch (Convert.ToInt32(id_map))
            {
                case (1):
                    return ("Summoner's Rift (Original Summer Variant)");
                case (2):
                    return ("Summoner's Rift (Original Autumn Variant)");
                case (3):
                    return ("The Proving Grounds (Tutorial Map)");
                case (4):
                    return ("Twisted Treeline (Original Version)");
                case (8):
                    return ("The Crystal Scar");
                case (10):
                    return ("Twisted Treeline");
                case (11):
                    return ("Summoner's Rift");
                case (12):
                    return ("Howling Abyss");
                case (14):
                    return ("Butcher's Bridge");
                default:
                    return ("Unknown");
            }
        }

        public static string get_mode_from_type(double type)
        {
            switch (Convert.ToInt32(type))
            {
                case (0):
                    return ("Custom");
                case (2):
                    return ("Normal Blind 5v5");
                case (4):
                    return ("Ranked Solo 5v5");
                case (6):
                    return ("Ranked Premade 5v5");
                case (7):
                    return ("Coop vs IA 5v5");
                case (8):
                    return ("Normal Blind 3v3");
                case (9):
                    return ("Ranked Premade 3v3");
                case (14):
                    return ("Normal Draft 5v5");
                case (16):
                    return ("Normal Dominion Blind 5v5");
                case (17):
                    return ("Normal Dominion Draft 5v5");
                case (25):
                    return ("Coop vs IA Dominion Blind 5v5");
                case (31):
                    return ("Coop vs IA (Intro) 5v5");
                case (32):
                    return ("Coop vs IA (Beginner) 5v5");
                case (33):
                    return ("Coop vs IA (Intermediate) 5v5");
                case (41):
                    return ("Ranked Team 3v3");
                case (42):
                    return ("Ranked Team 5v5");
                case (52):
                    return ("Coop vs IA 3v3");
                case (61):
                    return ("Normal Team Builder 5v5");
                case (65):
                    return ("Normal Aram 5v5");
                case (70):
                    return ("One for All 5v5");
                case (72):
                    return ("Snowdown Showdown 1v1");
                case (73):
                    return ("Snowdown Showdown 2v2");
                case (75):
                    return ("Summoner's Rift 6v6 Hexakill");
                case (76):
                    return ("Normal Ultra Rapid Fire 5v5");
                case (78):
                    return ("One for All Mirror 5v5");
                case (83):
                    return ("Coop vs IA Ultra Rapid Fire 5v5");
                case (91):
                    return ("Doom Bots Rank 1 5v5");
                case (92):
                    return ("Doom Bots Rank 2 5v5");
                case (93):
                    return ("Doom Bots Rank 5 5v5");
                case (96):
                    return ("Ascension 5v5");
                case (98):
                    return ("Twisted Treeline 6v6 Hexakill");
                case (100):
                    return ("Butcher's Bridge Aram 5v5");
                case (300):
                    return ("King Poro 5v5");
                case (310):
                    return ("Nemesis 5v5");
                case (313):
                    return ("Black Market Brawlers 5v5");
                case (315):
                    return ("Nexus Siege 5v5");
                case (317):
                    return ("Definitely Not Dominion 5v5");
                case (318):
                    return ("All Random URF 5v5");
                case (400):
                    return ("Normal 5v5 Draft Pick");
                case (410):
                    return ("Ranked 5v5 Draft Pick");
                case (420):
                    return ("Ranked 5v5 Draft Pick");
                case (440):
                    return ("Ranked Flex 5v5 Draft Pick");
                default:
                    return ("Unknown");
            }
        }

        public static bool is_valid_server(ref string region)
        {
            if (region == "EUNE" || region == "eune" || region == "EUNe" || region == "eUNE" || region == "EuNE" || region == "EUnE" || region == "EUne" || region == "EuNe" || region == "EunE" || region == "eUNe" || region == "eUnE" || region == "euNE" || region == "Eune" || region == "eUne" || region == "euNe" || region == "eunE")
            {
                region = "eune";
            }
            else if (region == "EUW" || region == "euw" || region == "EUw" || region == "eUW" || region == "EuW" || region == "Euw" || region == "eUw" || region == "euW")
            {
                region = "euw";
            }
            else if (region == "NA" || region == "na" || region == "Na" || region == "nA")
            {
                region = "na";
            }
            else if (region == "BR" || region == "br" || region == "Br" || region == "bR")
            {
                region = "br";
            }
            else if (region == "LAN" || region == "lan" || region == "LAn" || region == "lAN" || region == "LaN" || region == "Lan" || region == "lAn" || region == "laN")
            {
                region = "lan";
            }
            else if (region == "KR" || region == "kr" || region == "Kr" || region == "kR")
            {
                region = "kr";
            }
            else if (region == "OCE" || region == "oce" || region == "Oce" || region == "OCe" || region == "oCe" || region == "oCE" || region == "ocE" || region == "OcE")
            {
                region = "oce";
            }
            else if (region == "TR" || region == "tr" || region == "Tr" || region == "tR")
            {
                region = "tr";
            }
            else if (region == "RU" || region == "ru" || region == "Ru" || region == "rU")
            {
                region = "ru";
            }
            else if (region == "LAS" || region == "las" || region == "LAs" || region == "lAS" || region == "LaS" || region == "Las" || region == "lAs" || region == "laS")
            {
                region = "las";
            }
            else if (region == "JP" || region == "jp" || region == "Jp" || region == "pJ")
            {
                region = "jp";
            }
            else
            {
                return (false);
            }
            return (true);
        }

        public static string extract_shorter_path(string full_path)
        {
            int i = full_path.Length - 1;

            if(full_path.Length > 0)
            {
                while(i >= 0 && full_path[i] != '/')
                {
                    i--;
                }
            }

            if(i < 0)
            {
                return (full_path);
            }
            return (full_path.Substring(i + 1, full_path.Length - i - 1));
        }

        public static string convert_server_to_server_name(string server)
        {
            switch(server)
            {
                case("euw"):
                    return ("EU West");
                case ("eune"):
                    return ("EU Nordic & East");
                case ("na"):
                    return ("North America");
                case ("br"):
                    return ("Brazil");
                case ("jp"):
                    return ("Japan");
                case ("lan"):
                    return ("Latin America North");
                case ("las"):
                    return ("Latin America South");
                case ("kr"):
                    return ("Korea");
                case ("oce"):
                    return ("Oceania");
                case ("tr"):
                    return ("Turkey");
                case("ru"):
                    return ("Russia");
                default:
                    return ("Unknown");
            }
        }

        public static ConsoleColor get_division_color(string division)
        {
            switch(division)
            {
                case ("BRONZE"):
                    return (ConsoleColor.DarkRed);
                case ("SILVER"):
                    return (ConsoleColor.Gray);
                case ("GOLD"):
                    return (ConsoleColor.DarkYellow);
                case ("PLATINUM"):
                    return (ConsoleColor.DarkGreen);
                case ("DIAMOND"):
                    return (ConsoleColor.DarkCyan);
                case ("MASTER"):
                    return (ConsoleColor.White);
                case ("CHALLENGER"):
                    return (ConsoleColor.Cyan);
                default:
                    return (ConsoleColor.DarkGray);
            }
        }

        static public void ShowApp(string app_path)
        {
            IntPtr h = Process.GetCurrentProcess().MainWindowHandle;
            ShowWindow(h, 5);
            SetForegroundWindow(h);
            SetFocus(h);
            System.Diagnostics.Debug.WriteLine(h);
        }

        static public void copyList(List<string> src, ref List<string> dst)
        {
            dst = new List<string>();
            for (int i = 0; i < src.Count; i++)
                dst.Add(src[i]);
        }

        public static List<string> cut_string_by_length(string s, int length)
        {
            int start = 0;
            int cur_length = 0;
            List<string> s_cut = new List<string>();

            if(length > 0)
            {
                while (start < s.Length)
                {
                    if (start + length <= s.Length)
                    {
                        cur_length = start + length;
                        while(cur_length > start && s[cur_length - 1] != ' ')
                        {
                            cur_length--;
                        }
                        if(cur_length == start)
                            s_cut.Add(s.Substring(start, length));
                        else
                            s_cut.Add(s.Substring(start, cur_length - start));
                    }
                    else
                    {
                        s_cut.Add(s.Substring(start, s.Length - start));
                    }
                    start += s_cut[s_cut.Count - 1].Length;
                }
            }
            else
            {
                s_cut.Add(s);
            }

            return (s_cut);
        }

        public static bool is_same_char(char c1, char c2)
        {
            if(c1 == c2)
            {
                return (true);
            }
            else if(c1 >= 65 && c1 <= 90)
            {
                return (c1 + 32 == c2);
            }
            else if(c1 >= 97 && c1 <= 122)
            {
                return (c1 - 32 == c2);
            }

            return (false);
        }

        public static bool is_same_word(string s1, string s2)
        {
            if (s1.Length == s2.Length)
            {
                if(s1 == s2)
                {
                    return (true);
                }
                for(int i = 0; i < s1.Length; i++)
                {
                    if(!is_same_char(s1[i], s2[i]))
                    {
                        return (false);
                    }
                }
            }
            else
            {
                return (false);
            }

            return (true);
        }

        public static bool s1_is_before_s2(string s1, string s2)
        {
            int i = 0;

            while(i < s1.Length && i < s2.Length && is_same_char(s1[i], s2[i]))
            {
                i++;
            }

            if(i < s1.Length && i < s2.Length)
            {
                return (c1_is_before_c2(s1[i], s2[i]));
            }
            return (i < s2.Length);
        }

        public static bool c1_is_before_c2(char c1, char c2)
        {
            if(c1 >= 65 && c1 <= 90)
            {
                if(c2 >= 97 && c2 <= 122)
                {
                    return (c1 + 32 < c2);
                }
            }
            else if(c1 >= 97 && c2 <= 122)
            {
                if(c2 >= 65 && c2 <= 90)
                {
                    return (c1 < c2 + 32);
                }
            }
            return (c1 < c2);
        }

        public static int indexOccurence(string text, string pattern, int startIndex = 0)
        {
            int index = startIndex;

            if(pattern.Length == 0 || text.Length == 0)
                return(-1);

            while (index <= text.Length - pattern.Length)
            {
                if (text[index] == pattern[0] && text.Substring(index, pattern.Length) == pattern)
                    return (index);
                index++;
            }
            
            return (-1);
        }

        public static void insert_in_list_sorted(ref List<string> l, string elt)
        {
            int i = 0;
            while(i < l.Count && s1_is_before_s2(l[i], elt))
            {
                i++;
            }
            l.Insert(i, elt);
        }

        public static void free_waiting_keys()
        {
            while(Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }

        public static string web_format_extractor(string string_web)
        {
            string new_string = "";
            int start = 0;
            int i = 0;

            while (i < string_web.Length)
            {
                if (string_web[i] == '<')
                {
                    new_string = new_string + string_web.Substring(start, i - start);
                }
                else if (string_web[i] == '>')
                {
                    start = i + 1;
                }
                i++;
            }
            if (start != i)
            {
                new_string = new_string + string_web.Substring(start, i - start);
            }
            return (new_string);
        }

        public static void createProfileFile(string file, string userName, string region, string id, string userLoL = "", string password = "")
        {
            List<string> content = new List<string>();
            content.Add(userName);
            content.Add(region);
            content.Add(id);
            content.Add(userLoL);
            content.Add(password);
            saveFile(file, content);
        }

        public static List<string> getFileContent(string file)
        {
            List<string> content = new List<string>();
            string current = "";

            if(File.Exists(file))
            {
                try
                {
                    StreamReader reader = new StreamReader(file);
                    current = reader.ReadLine();

                    while (current != null)
                    {
                        content.Add(current);
                        current = reader.ReadLine();
                    }

                    reader.Close();
                }
                catch (Exception)
                {
                }
            }

            return (content);
        }

        public static void saveFile(string file, List<string> content)
        {
            try
            {
                StreamWriter writer = new StreamWriter(file);
                for (int i = 0; i < content.Count; i++)
                    writer.WriteLine(content[i]);
                writer.Close();
            }
            catch(Exception)
            {
            }
        }

        public static string convertListToString(List<string> content)
        {
            string result = "";
            for (int i = 0; i < content.Count; i++)
                result += content[i];
            return (result);
        }

        public static void cursorBack(int n = 1)
        {
            if(n >= Console.WindowWidth)
            {
                Console.CursorTop = Console.CursorTop - (n / Console.WindowWidth);
                n = n % Console.WindowWidth;
            }

            for (int i = 0; i < n; i++)
            {
                if (Console.CursorLeft == 0)
                {
                    Console.CursorLeft = Console.WindowWidth - 1;
                    Console.CursorTop--;
                }
                else
                    Console.CursorLeft--;
            }
        }

        public static string Encrypt(string clearText, string EncryptionKey)
        {
            try
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(clearBytes, 0, clearBytes.Length);
                            cs.Close();
                        }
                        clearText = Convert.ToBase64String(ms.ToArray());
                    }
                }
                return clearText;
            }
            catch(Exception)
            {
            }
            return ("NO");
        }

        public static string Decrypt(string cipherText, string EncryptionKey)
        {
            try
            {
                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
                return cipherText;
            }
            catch(Exception)
            {
            }
            return ("NO");
        }
    }
}
