using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Json;
using Newtonsoft.Json;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Game
    {
        private double game_id;
        private double map_id;
        private double game_duration;
        private double game_type;
        private double nbr_blue_players;
        private double nbr_blue_ban;
        private string region;
        private List<string> players = new List<string>();
        private List<string> players_champion = new List<string>();
        private List<double> players_championID = new List<double>();
        private List<double> players_ID = new List<double>();
        private List<double> banned_champion = new List<double>();
        private List<double> SummonerSpell1 = new List<double>();
        private List<double> SummonerSpell2 = new List<double>();
        private List<Runes> SummonerRunes = new List<Runes>();
        private List<Mastery> SummonerMasteries = new List<Mastery>();
        private List<string> SummonerMasteryScore = new List<string>();
        private int[] player_role = new int[10];

        public Game(JsonObject result, string region, ref LolRequester requester, string appdata_dir)
        {
            nbr_blue_players = 0;
            List<JsonObject> fieldtab = new List<JsonObject>();
            List<JsonObject> fieldtab2 = new List<JsonObject>();

            this.region = region;
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                switch (field.Name)
                {
                    case ("bannedChampions"):
                        fieldtab = (List<JsonObject>)field.GetValue();

                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            foreach (JsonObject field2 in (fieldtab[i]) as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("championId"):
                                        this.banned_champion.Add((double)field2.GetValue());
                                        break;
                                    case ("teamId"):
                                        if((double)field2.GetValue() == 100)
                                            nbr_blue_ban++;
                                        break;
                                }
                            }
                        }

                        break;
                    case ("gameId"):
                        this.game_id = (double)field.GetValue();
                        break;
                    case ("gameLength"):
                        this.game_duration = (double)field.GetValue();
                        break;
                    case ("gameQueueConfigId"):
                        this.game_type = (double)field.GetValue();
                        break;
                    case ("mapId"):
                        this.map_id = (double)field.GetValue();
                        break;
                    case ("participants"):
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            foreach (JsonObject field2 in (fieldtab[i]) as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("championId"):
                                        this.players_champion.Add(Data_library.get_champion_from_id((double)field2.GetValue()));
                                        this.players_championID.Add((double)field2.GetValue());
                                        break;
                                    case ("summonerName"):
                                        players.Add((string)field2.GetValue());
                                        break;
                                    case ("summonerId"):
                                        players_ID.Add((double)field2.GetValue());
                                        break;
                                    case ("spell1Id"):
                                        SummonerSpell1.Add((double)field2.GetValue());
                                        break;
                                    case ("spell2Id"):
                                        SummonerSpell2.Add((double)field2.GetValue());
                                        break;
                                    case ("teamId"):
                                        if((double)field2.GetValue() == 100)
                                            nbr_blue_players++;
                                        break;
                                    case ("masteries"):
                                        fieldtab2 = (List<JsonObject>)field2.GetValue();
                                        SummonerMasteries.Add(new Mastery());
                                        double rank = 1;
                                        double masteryID = 0;
                                        for (int j = 0; j < fieldtab2.Count; j++)
                                        {
                                            foreach (JsonObject field3 in (fieldtab2[j]) as JsonObjectCollection)
                                            {
                                                switch (field3.Name)
                                                {
                                                    case ("rank"):
                                                        rank = (double)field3.GetValue();
                                                        break;
                                                    case ("masteryId"):
                                                        masteryID = (double)field3.GetValue();
                                                        break;
                                                }
                                            }
                                            SummonerMasteries[SummonerMasteries.Count - 1].addMastery(masteryID, rank);
                                        }
                                        SummonerMasteryScore.Add(SummonerMasteries[SummonerMasteries.Count - 1].getMasteriesPlacement());
                                        break;
                                    case ("runes"):
                                        fieldtab2 = (List<JsonObject>)field2.GetValue();
                                        SummonerRunes.Add(new Runes());
                                        double count = 1;
                                        double runeID = 0;
                                        for (int j = 0; j < fieldtab2.Count; j++)
                                        {
                                            foreach (JsonObject field3 in (fieldtab2[j]) as JsonObjectCollection)
                                            {
                                                switch (field3.Name)
                                                {
                                                    case ("count"):
                                                        count = (double)field3.GetValue();
                                                        break;
                                                    case ("runeId"):
                                                        runeID = (double)field3.GetValue();
                                                        break;
                                                }
                                            }
                                            SummonerRunes[SummonerRunes.Count - 1].addRune(runeID, count);
                                        }
                                        break;
                                }
                            }
                        }
                  
                        break;
                }
            }
            
            print_game(ref requester, appdata_dir);
        }

        public void print_game(ref LolRequester requester, string appdata_dir)
        {
            Console.SetBufferSize(84, 30);
            Console.SetWindowSize(84, 30);
            print_time(Convert.ToInt32(game_duration));
            string mode = Data_library.get_mode_from_type(game_type);
            string map = Data_library.get_map_from_id(map_id);
            int summonerSelected = 0;
            int masterySelected = 0; //0 means keystone, else is an other mastery
            int presSumSelected = -1;
            int allSumPresentationLine = 0;
            int runesLine = 0;
            int masteriesLine = 0;
            int endPageLine = 0; //line of presstocontinue
            bool masteryUpdate = false;
            List<string> allInfoSummoner = new List<string>();
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            int nbr_space = (Console.WindowWidth - Console.CursorLeft - mode.Length - map.Length - 9) / 4;
            Data_library.print_n_space(((Console.WindowWidth / 2) - Console.CursorLeft - (mode.Length / 2) - 1) / 2);
            Console.Write("-");
            Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - (mode.Length / 2) - 1);
            Console.Write(mode);
            Data_library.print_n_space((Console.WindowWidth - Console.CursorLeft - map.Length - 2)/2 - 1);
            Console.Write("-");
            Data_library.print_n_space(Console.WindowWidth - Console.CursorLeft - map.Length - 2);
            Console.WriteLine(map + "\n");
            Console.Write("  Blue Team");
            Data_library.print_n_space(Console.WindowWidth - Console.CursorLeft - 10);
            Console.WriteLine("Red Team");
            if (map_id == 11 && players.Count == 10)
            {
                sort_by_role();
                print_game_line_role();
            }
            else
            {
                print_game_line();
            }
            if(banned_champion.Count > 0)
            {
                Console.Write(" ");
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("Banned Champion");
                print_ban_line();
            }
            Console.Write("\n> LoLapp: waiting for players stats...");
            allSumPresentationLine = Console.CursorTop + 1;
            present_summoners(ref requester, ref allInfoSummoner);
            presSumSelected = summonerSelected;
            runesLine = Console.CursorTop; //buffer should go down by 1 so it's in fact the first sum
            addSummonerDetails(summonerSelected, ref masterySelected, ref masteriesLine);
            Data_library.ShowApp(Assembly.GetExecutingAssembly().Location);
            printPressToContinue(ref endPageLine);
            do
            {
                key_pressed = Console.ReadKey(true);
                applySumSelection(key_pressed, ref summonerSelected, ref presSumSelected, ref masterySelected, ref masteryUpdate, appdata_dir);
                updateSumInfo(allInfoSummoner, summonerSelected, presSumSelected, allSumPresentationLine);
                updateSummonerDetails(summonerSelected, presSumSelected, runesLine, masteriesLine, ref endPageLine, ref masterySelected, masteryUpdate);
            } while (key_pressed.Key != ConsoleKey.Enter);
            Console.Clear();
            Console.SetWindowSize(84, 30);
            Console.SetBufferSize(84, 30);
        }

        private void print_game_line()
        {
            double blue_players_printed = 0;
            double red_players_printed = 0;

            while((blue_players_printed + red_players_printed) < players.Count)
            {
                Console.Write("   ");
                if(blue_players_printed < nbr_blue_players)
                {
                    Console.Write(players[(int)blue_players_printed] + " (" + players_champion[(int)blue_players_printed] + ")");
                    blue_players_printed++;
                }
                if (red_players_printed < (players.Count - nbr_blue_players))
                {
                    Data_library.print_n_space(Console.WindowWidth - Console.CursorLeft - players[(int)(nbr_blue_players + red_players_printed)].Length - players_champion[(int)(nbr_blue_players + red_players_printed)].Length - 6);
                    Console.WriteLine("(" + players_champion[(int)(nbr_blue_players + red_players_printed)] + ") " + players[(int)(nbr_blue_players + red_players_printed)]);
                    red_players_printed++;
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        private void print_game_line_role()
        {
            int index;
            for (int i = 0; i < 5; i++)
            {
                index = get_index_player_role(i, 0);
                Console.Write("   " + players[index] + " (" + players_champion[index] + ")");
                switch(i)
                {
                    case (0):
                        Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - 2);
                        Console.Write("Top");
                        break;
                    case (1):
                        Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - 3);
                        Console.Write("Jungle");
                        break;
                    case (2):
                        Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - 2);
                        Console.Write("Mid");
                        break;
                    case (3):
                        Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - 2);
                        Console.Write("Adc");
                        break;
                    case (4):
                        Data_library.print_n_space((Console.WindowWidth / 2) - Console.CursorLeft - 3);
                        Console.Write("Support");
                        break;
                }
                index = get_index_player_role(i, 1);
                Data_library.print_n_space(Console.WindowWidth - Console.CursorLeft - players[index].Length - players_champion[index].Length - 6);
                Console.WriteLine("(" + players_champion[index] + ") " + players[index]);
            }
        }

        private void print_ban_line()
        {
            double blue_ban_printed = 0;
            double red_ban_printed = 0;

            while ((blue_ban_printed + red_ban_printed) < banned_champion.Count)
            {
                Console.Write("   ");
                if (blue_ban_printed < nbr_blue_ban)
                {
                    Console.Write(Data_library.get_champion_from_id(Convert.ToInt32(banned_champion[(int)(blue_ban_printed + red_ban_printed)])));
                    blue_ban_printed++;
                }
                if (red_ban_printed < (banned_champion.Count - nbr_blue_ban))
                {
                    Data_library.print_n_space(Console.WindowWidth - Console.CursorLeft - Data_library.get_champion_from_id(Convert.ToInt32(banned_champion[(int)(blue_ban_printed + red_ban_printed)])).Length - 3);
                    Console.WriteLine(Data_library.get_champion_from_id(Convert.ToInt32(banned_champion[(int)(blue_ban_printed + red_ban_printed)])));
                    red_ban_printed++;
                }
                else
                {
                    Console.WriteLine();
                }
            }
        }

        private void print_time(int sec)
        {
            int min = sec / 60;
            int hour = min / 60;

            Console.Write("\n  ");
            if(sec <= 0)
                Console.Write("0s");
            else
            {
                if (hour > 0)
                    Console.Write(hour + "h");
                if (min % 60 > 0)
                    Console.Write(min % 60 + "m");
                if (sec % 60 > 0)
                    Console.Write(sec % 60 + "s");
            }
        }

        private void present_summoners(ref LolRequester requester, ref List<string> allInfoSum, int sumSelected = 0)
        {
            Roster players_info = null;
            Roster players_info_complementation = null;
            List<string> players_full_division = new List<string>();
            List<string> players_main_champion = new List<string>();
            List<string> players_current_KDA = new List<string>();
            List<string> players_main_KDA = new List<string>();
            List<double> players_lvl = new List<double>();
            List<string> players_main_winrate = new List<string>();
            List<string> players_current_winrate = new List<string>();
            int nbr_char_summoner_lvl;
            int nbr_char_lvl_division;
            int nbr_char_division_champion;
            int nbr_char_champion_current_KDA;
            int nbr_char_current_KDA_current_winrate;
            int nbr_char_current_winrate_mostplayed;
            int nbr_char_mostplayed_main_KDA;
            int nbr_char_main_KDA_main_winrate;
            int nbr_char_main_winrates_masteries;
            int a;
            string all_ID_to_string = "";
            string all_ID_to_string_complementation = "";

            for (a = 0; a < players.Count; a++) //creation des strings d'ID (2 string max)
            {
                if (all_ID_to_string == "")
                    all_ID_to_string = players_ID[a].ToString();
                else if (a < 10)
                    all_ID_to_string = all_ID_to_string + "," + players_ID[a].ToString();
                else if (all_ID_to_string_complementation == "")
                    all_ID_to_string_complementation = players_ID[a].ToString();
                else
                    all_ID_to_string_complementation = all_ID_to_string_complementation + "," + players_ID[a].ToString();
            }

            players_info = requester.GetMultiPlayer(ref requester, all_ID_to_string, region, false, players_championID, players_ID);
            if (players.Count > 10)
                players_info_complementation = requester.GetMultiPlayer(ref requester, all_ID_to_string_complementation, region, false, players_championID, players_ID);
            if (players_info != null && (players_info_complementation != null && players.Count > 10 || players_info_complementation == null && players.Count <= 10)) //si +10 joueurs, on fusionne les listes
            {
                if (players_info_complementation != null)
                {
                    for (a = 0; a < players_info_complementation.members.Count; a++)
                        players_info.members.Add(players_info_complementation.members[a]);
                }
            }
            for (int j = 0; j < players_ID.Count; j++) //prepare les listes d'affichage
            {
                a = 0;
                while (a < players_info.members.Count && players_ID[j] != players_info.members[a]._id)
                    a++;
                if(a < players_info.members.Count)
                {
                    if (players_info.members[a].divisionTier == "Unranked")
                        players_full_division.Add("[UNRANKED]");
                    else
                        players_full_division.Add("[" + players_info.members[a].divisionTier + " " + players_info.members[a].divisionRank + "]");

                    players_current_KDA.Add(players_info.members[a].get_current_KDA_format());
                    players_main_KDA.Add(players_info.members[a].get_main_KDA_format());
                    if(players_info.members[a].main_champions_list.Count > 0)
                    {
                        players_main_champion.Add(players_info.members[a].main_champions_list[0]);
                        players_main_winrate.Add(players_info.members[a].main_win_rates_list[0].ToString() + "%");
                    } 
                    else
                    {
                        players_main_champion.Add("Unknown");
                        players_main_winrate.Add("0%");
                    }
                    players_lvl.Add(players_info.members[a].lvl);
                    players_current_winrate.Add(players_info.members[a].current_win_rate.ToString() + "%");
                }
            }

            nbr_char_summoner_lvl = Data_library.max_length(players, 8); //8 etant la longueur de Summoner (titre)
            nbr_char_lvl_division = Data_library.max_length(players_lvl, 3); //3 etant la longueur de lvl (titre)
            nbr_char_division_champion = Data_library.max_length(players_full_division, 8); //8 etant la longueur de division (titre)
            nbr_char_champion_current_KDA = Data_library.max_length(players_champion, 8); //8 etant la longueur de champion (titre)
            nbr_char_current_KDA_current_winrate = Data_library.max_length(players_current_KDA, 3); //3 etant la longueur de KDA (titre)
            nbr_char_current_winrate_mostplayed = Data_library.max_length(players_current_winrate, 3); //3 etant la longueur de Win (titre)
            nbr_char_mostplayed_main_KDA = Data_library.max_length(players_main_champion, 8); //8 etant la longueur de Favorite (titre)
            nbr_char_main_KDA_main_winrate = Data_library.max_length(players_main_KDA, 3); //3 etant la longueur de KDA (titre)
            nbr_char_main_winrates_masteries = Data_library.max_length(players_main_winrate, 3); //3 etant la longueur de Win (titre)
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write("                                                                                   ");
            Console.SetCursorPosition(0, Console.CursorTop);
            if(Console.CursorTop + players.Count + 2 >= Console.BufferHeight)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.CursorTop + players.Count + 2);
                Console.SetWindowSize(Console.WindowWidth, Console.CursorTop + players.Count + 2);
            }
            if (14 + nbr_char_summoner_lvl + nbr_char_lvl_division + nbr_char_division_champion + nbr_char_champion_current_KDA + nbr_char_current_KDA_current_winrate + nbr_char_current_winrate_mostplayed + nbr_char_mostplayed_main_KDA + nbr_char_main_KDA_main_winrate + nbr_char_main_winrates_masteries + Data_library.max_length(SummonerMasteryScore, 7) >= Console.BufferWidth)
            {
                Console.SetBufferSize(15 + nbr_char_summoner_lvl + nbr_char_lvl_division + nbr_char_division_champion + nbr_char_champion_current_KDA + nbr_char_current_KDA_current_winrate + nbr_char_current_winrate_mostplayed + nbr_char_mostplayed_main_KDA + nbr_char_main_KDA_main_winrate + nbr_char_main_winrates_masteries + Data_library.max_length(SummonerMasteryScore, 7), Console.WindowHeight);
                Console.SetWindowSize(15 + nbr_char_summoner_lvl + nbr_char_lvl_division + nbr_char_division_champion + nbr_char_champion_current_KDA + nbr_char_current_KDA_current_winrate + nbr_char_current_winrate_mostplayed + nbr_char_mostplayed_main_KDA + nbr_char_main_KDA_main_winrate + nbr_char_main_winrates_masteries + Data_library.max_length(SummonerMasteryScore, 7), Console.WindowHeight);
            }

            allInfoSum.Add("  Summoner" 
                + Data_library.get_n_space(nbr_char_summoner_lvl - 7) 
                + "LvL" + Data_library.get_n_space(nbr_char_lvl_division - 2) 
                + "Division" + Data_library.get_n_space(nbr_char_division_champion - 7) 
                + "Champion" + Data_library.get_n_space(nbr_char_champion_current_KDA - 7) 
                + "KDA" + Data_library.get_n_space(nbr_char_current_KDA_current_winrate - 2) 
                + "Win" + Data_library.get_n_space(nbr_char_current_winrate_mostplayed - 2) 
                + "Favorite" + Data_library.get_n_space(nbr_char_mostplayed_main_KDA - 7) 
                + "KDA" + Data_library.get_n_space(nbr_char_main_KDA_main_winrate - 2) 
                + "Win" + Data_library.get_n_space(nbr_char_main_winrates_masteries - 2) 
                + "Mastery");

            for (int i = 0; i < players.Count; i++)
            {
                allInfoSum.Add(players[i]
                + Data_library.get_n_space(nbr_char_summoner_lvl - players[i].Length + 1)
                + players_lvl[i] + Data_library.get_n_space(nbr_char_lvl_division - players_lvl[i].ToString().Length + 1)
                + players_full_division[i] + Data_library.get_n_space(nbr_char_division_champion - players_full_division[i].Length + 1)
                + players_champion[i] + Data_library.get_n_space(nbr_char_champion_current_KDA - players_champion[i].Length + 1)
                + players_current_KDA[i] + Data_library.get_n_space(nbr_char_current_KDA_current_winrate - players_current_KDA[i].Length + 1)
                + players_current_winrate[i] + Data_library.get_n_space(nbr_char_current_winrate_mostplayed - players_current_winrate[i].ToString().Length + 1)
                + players_main_champion[i] + Data_library.get_n_space(nbr_char_mostplayed_main_KDA - players_main_champion[i].Length + 1)
                + players_main_KDA[i] + Data_library.get_n_space(nbr_char_main_KDA_main_winrate - players_main_KDA[i].Length + 1)
                + players_main_winrate[i] + Data_library.get_n_space(nbr_char_main_winrates_masteries - players_main_winrate[i].Length + 1)
                + SummonerMasteryScore[i]);
            }

            displaySumInfo(allInfoSum, sumSelected);
        }

        private void displaySumInfo(List<string> infos, int sumSelected)
        {
            if(infos.Count > 0)
                Console.WriteLine(infos[0]);
            for (int i = 1; i < infos.Count; i++)
            {
                if (i - 1 < nbr_blue_players)
                {
                    if (i - 1 == sumSelected)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                }
                else
                {
                    if (i - 1 == sumSelected)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                }

                if (i - 1 == sumSelected)
                    Console.Write("> ");
                else
                    Console.Write("  ");
                Console.WriteLine(infos[i]);
            }
        }

        private void updateSumInfo(List<string> infos, int sumSelected, int preSumSelected, int firstLine)
        {
            if(preSumSelected != sumSelected)
            {
                Console.SetCursorPosition(0, firstLine + preSumSelected);
                if (preSumSelected < nbr_blue_players)
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                else
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("  " + infos[preSumSelected + 1]);
                Console.SetCursorPosition(0, firstLine + sumSelected);
                if (sumSelected < nbr_blue_players)
                    Console.ForegroundColor = ConsoleColor.Blue;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("> " + infos[sumSelected + 1]);
            }
        }

        private void addSummonerDetails(int index, ref int masterySelected, ref int masteriesLine, bool masteriesOnly = false)
        {
            if (Console.CursorTop + 1 >= Console.BufferHeight)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 1);
                Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 1);
            }
            Console.WriteLine();
            List<string> runesBonus = new List<string>();
            List<double> runesEffect = new List<double>();
            int j = 0;

            if (!masteriesOnly)
            {
                for (int i = 0; i < SummonerRunes[index].runesID.Count; i++)
                {
                    j = 0;
                    while (j < runesBonus.Count && runesBonus[j] != SummonerRunes[index].getRuneSource(SummonerRunes[index].runesID[i]))
                        j++;
                    if (j == runesBonus.Count)
                    {
                        runesBonus.Add(SummonerRunes[index].getRuneSource(SummonerRunes[index].runesID[i]));
                        runesEffect.Add(SummonerRunes[index].getRuneEffect(SummonerRunes[index].runesID[i]) * SummonerRunes[index].runesQuantity[i]);
                    }
                    else
                        runesEffect[j] += SummonerRunes[index].getRuneEffect(SummonerRunes[index].runesID[i]) * SummonerRunes[index].runesQuantity[i];
                }
                if (Console.CursorTop + 1 >= Console.BufferHeight)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 1);
                    Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 1);
                }
                Console.WriteLine("  > Runes:");
                for (int i = 0; i < runesBonus.Count; i++)
                {
                    if (Console.CursorTop + 1 >= Console.BufferHeight)
                    {
                        Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 1);
                        Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 1);
                    }

                    Console.ForegroundColor = SummonerRunes[index].getRuneUtility(runesBonus[i]);

                    if (runesBonus[i][0] == '%')
                    {
                        if (runesBonus[i][runesBonus[i].Length - 1] == 'l')
                            Console.WriteLine("    > " + Math.Round(runesEffect[i], 1) + runesBonus[i] + " (" + Math.Round(runesEffect[i] * 18, 1) + " at level 18)");
                        else
                            Console.WriteLine("    > " + Math.Round(runesEffect[i], 1) + runesBonus[i]);
                    }
                    else
                    {
                        if (runesBonus[i][runesBonus[i].Length - 1] == 'l')
                            Console.WriteLine("    > " + Math.Round(runesEffect[i], 1) + " " + runesBonus[i] + " (" + Math.Round(runesEffect[i] * 18, 1) + " at level 18)");
                        else
                            Console.WriteLine("    > " + Math.Round(runesEffect[i], 1) + " " + runesBonus[i]);
                    }
                }
            }

            List<string> description = new List<string>();
            masteriesLine = Console.CursorTop;
            if(masterySelected == 0)
            {
                description = Data_library.cut_string_by_length(SummonerMasteries[index].getKeyStoneDescription(), Console.WindowWidth - 6);
                if (Console.CursorTop + 3 + description.Count >= Console.BufferHeight)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 3 + description.Count);
                    Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 3 + description.Count);
                }
                Console.ForegroundColor = SummonerMasteries[index].getKeyStoneColor();
                Console.WriteLine("\n  > Keystone Mastery: " + SummonerMasteries[index].getKeyStoneName() + "\n");
            }
            else
            {
                double curMastID = 0;
                curMastID = SummonerMasteries[index].masteriesID[masterySelected - 1];
                description = Data_library.cut_string_by_length(SummonerMasteries[index].getMasteryDescription(curMastID, SummonerMasteries[index].masteriesLvl[masterySelected - 1]), Console.WindowWidth - 6);
                if (Console.CursorTop + 3 + description.Count >= Console.BufferHeight)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 3 + description.Count);
                    Console.SetWindowSize(Console.WindowWidth, Console.WindowHeight + 3 + description.Count);
                }
                Console.ForegroundColor = SummonerMasteries[index].getMasteryColumnColor(curMastID);
                Console.WriteLine("\n  > Mastery : " + SummonerMasteries[index].getMasteryName(curMastID) + "\n");
            }

            for (int i = 0; i < description.Count; i++)
                Console.WriteLine("    " + description[i]);
        }

        private void updateSummonerDetails(int sumSelected, int preSumSelected, int runesLine, int masteriesLine, ref int lastLine, ref int masterySelected, bool masteryUpdate)
        {
            if(sumSelected != preSumSelected || masteryUpdate)
            {
                int i = 0;
                int firstLine = Console.CursorTop;
                if (sumSelected != preSumSelected)
                    firstLine = runesLine;
                else
                    firstLine = masteriesLine;
                Console.SetCursorPosition(0, firstLine);
                for (i = 0; i < lastLine - firstLine + 1; i++)
                    Data_library.print_n_space(Console.WindowWidth - 1);
                Console.SetCursorPosition(0, firstLine);
                addSummonerDetails(sumSelected, ref masterySelected, ref i, sumSelected == preSumSelected);
                printPressToContinue(ref lastLine);
            }
        }

        private void applySumSelection(ConsoleKeyInfo key_pressed, ref int sumSelected, ref int preSumSelected, ref int masterySelected, ref bool masteryUpdate, string appdata_dir)
        {
            masteryUpdate = false;
            preSumSelected = sumSelected;
            switch(key_pressed.Key)
            {
                case (ConsoleKey.UpArrow):
                    if (sumSelected == 0)
                        sumSelected = players.Count - 1;
                    else
                        sumSelected--;
                    masterySelected = 0;
                    break;
                case (ConsoleKey.DownArrow):
                    sumSelected = (sumSelected + 1) % players.Count;
                    masterySelected = 0;
                    break;
                case(ConsoleKey.LeftArrow):
                    if (masterySelected > 0)
                        masterySelected--;
                    else
                        masterySelected = SummonerMasteries[sumSelected].masteriesID.Count;
                    masteryUpdate = true;
                    break;
                case (ConsoleKey.RightArrow):
                    masterySelected = (masterySelected + 1) % (SummonerMasteries[sumSelected].masteriesID.Count + 1);
                    masteryUpdate = true;
                    break;
                case (ConsoleKey.D0):
                    if (players.Count > 0)
                        sumSelected = 0;
                    break;
                case (ConsoleKey.NumPad0):
                    if (players.Count > 0)
                        sumSelected = 0;
                    break;
                case (ConsoleKey.D1):
                    if (players.Count > 1)
                        sumSelected = 1;
                    break;
                case (ConsoleKey.NumPad1):
                    if (players.Count > 1)
                        sumSelected = 1;
                    break;
                case (ConsoleKey.D2):
                    if (players.Count > 2)
                        sumSelected = 2;
                    break;
                case (ConsoleKey.NumPad2):
                    if (players.Count > 2)
                        sumSelected = 2;
                    break;
                case (ConsoleKey.D3):
                    if (players.Count > 3)
                        sumSelected = 3;
                    break;
                case (ConsoleKey.NumPad3):
                    if (players.Count > 3)
                        sumSelected = 3;
                    break;
                case (ConsoleKey.D4):
                    if (players.Count > 4)
                        sumSelected = 4;
                    break;
                case (ConsoleKey.NumPad4):
                    if (players.Count > 4)
                        sumSelected = 4;
                    break;
                case (ConsoleKey.D5):
                    if (players.Count > 5)
                        sumSelected = 5;
                    break;
                case (ConsoleKey.NumPad5):
                    if (players.Count > 5)
                        sumSelected = 5;
                    break;
                case (ConsoleKey.D6):
                    if (players.Count > 6)
                        sumSelected = 6;
                    break;
                case (ConsoleKey.NumPad6):
                    if (players.Count > 6)
                        sumSelected = 6;
                    break;
                case (ConsoleKey.D7):
                    if (players.Count > 7)
                        sumSelected = 7;
                    break;
                case (ConsoleKey.NumPad7):
                    if (players.Count > 7)
                        sumSelected = 7;
                    break;
                case (ConsoleKey.D8):
                    if (players.Count > 8)
                        sumSelected = 8;
                    break;
                case (ConsoleKey.NumPad8):
                    if (players.Count > 8)
                        sumSelected = 8;
                    break;
                case (ConsoleKey.D9):
                    if (players.Count > 9)
                        sumSelected = 9;
                    break;
                case (ConsoleKey.NumPad9):
                    if (players.Count > 9)
                        sumSelected = 9;
                    break;
                case (ConsoleKey.P):
                    openProfile(players[sumSelected], region, appdata_dir);
                    break;
            }
            if (sumSelected != preSumSelected)
                masterySelected = 0;
        }

        private void printPressToContinue(ref int endPageLine)
        {
            if (Console.CursorTop + 4 >= Console.BufferHeight)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.CursorTop + 4);
                Console.SetWindowSize(Console.WindowWidth, Console.CursorTop + 4);
            }
            Console.SetCursorPosition(0, Console.BufferHeight - 1);
            Data_library.print_n_space((Console.WindowWidth / 2) - 14);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press 'Enter' to continue...");
            if(endPageLine < Console.CursorTop)
                endPageLine = Console.CursorTop;
            Data_library.free_waiting_keys();
        }

        private void sort_by_role()
        {
            /* SUMMONER SPELLS:
             * 3 = exaust
             * 4 = flash
             * 7 = heal
             * 11 = smite
             * 12 = teleportation
             * 14 = ignite
             * 21 = shield
            */
            player_role = new int[10];
            bool[] matched = new bool[10];
            for(int i = 0; i < 10; i++)
            {
                matched[i] = false;
            }
            int current_score;
            int selected_score;
            int selected = -1;
            
            //calcule du role de JUNGLER
            for (int t = 0; t < 2; t++)
            {
                selected_score = -1;
                for (int i = 0; i < 5; i++)
                {
                    current_score = 0;
                    if (SummonerSpell1[t * 5 + i] == 11 || SummonerSpell2[t * 5 + i] == 11)
                    {
                        current_score += 2;
                    }
                    if (Data_library.get_champion_role(players_champion[t * 5 + i])[0] == 1)
                    {
                        current_score++;
                    }
                    if (current_score >= selected_score && !matched[t * 5 + i])
                    {
                        if(current_score == selected_score)
                        {
                            if(final_decision(players_champion[selected], players_champion[t * 5 + i], 1) == 2)
                            {
                                selected = t * 5 + i;
                                selected_score = current_score;
                            }
                        }
                        else
                        {
                            selected = t * 5 + i;
                            selected_score = current_score;
                        }
                    }
                }
                player_role[selected] = 1;
                matched[selected] = true;
            }

            //calcule du role de ADC
            for (int t = 0; t < 2; t++)
            {
                selected_score = -1;
                for (int i = 0; i < 5; i++)
                {
                    current_score = 0;
                    if (SummonerSpell1[t * 5 + i] == 7 || SummonerSpell2[t * 5 + i] == 7)
                    {
                        current_score += 2;
                    }
                    if (Data_library.get_champion_role(players_champion[t * 5 + i])[0] == 3)
                    {
                        current_score++;
                    }
                    if (current_score >= selected_score && !matched[t * 5 + i])
                    {
                        if (current_score == selected_score)
                        {
                            if (final_decision(players_champion[selected], players_champion[t * 5 + i], 3) == 2)
                            {
                                selected = t * 5 + i;
                                selected_score = current_score;
                            }
                        }
                        else
                        {
                            selected = t * 5 + i;
                            selected_score = current_score;
                        }
                    }
                }
                player_role[selected] = 3;
                matched[selected] = true;
            }

            //calcule du role de TOP
            for (int t = 0; t < 2; t++)
            {
                selected_score = -1;
                for (int i = 0; i < 5; i++)
                {
                    current_score = 0;
                    if (SummonerSpell1[t * 5 + i] == 12 || SummonerSpell2[t * 5 + i] == 12)
                    {
                        current_score += 2;
                    }
                    if (Data_library.get_champion_role(players_champion[t * 5 + i])[0] == 0)
                    {
                        current_score++;
                    }
                    if (current_score >= selected_score && !matched[t * 5 + i])
                    {
                        if (current_score == selected_score)
                        {
                            if (final_decision(players_champion[selected], players_champion[t * 5 + i], 0) == 2)
                            {
                                selected = t * 5 + i;
                                selected_score = current_score;
                            }
                        }
                        else
                        {
                            selected = t * 5 + i;
                            selected_score = current_score;
                        }
                    }
                }
                player_role[selected] = 0;
                matched[selected] = true;
            }

            //calcule du role de MID
            for (int t = 0; t < 2; t++)
            {
                selected_score = -1;
                for (int i = 0; i < 5; i++)
                {
                    current_score = 0;
                    if (SummonerSpell1[t * 5 + i] == 14 || SummonerSpell2[t * 5 + i] == 14 || SummonerSpell1[t * 5 + i] == 21 || SummonerSpell2[t * 5 + i] == 21)
                    {
                        current_score++;
                    }
                    if (Data_library.get_champion_role(players_champion[t * 5 + i])[0] == 2)
                    {
                        current_score += 2;
                    }
                    if (current_score >= selected_score && !matched[t * 5 + i])
                    {
                        if (current_score == selected_score)
                        {
                            if (final_decision(players_champion[selected], players_champion[t * 5 + i], 2) == 2)
                            {
                                selected = t * 5 + i;
                                selected_score = current_score;
                            }
                        }
                        else
                        {
                            selected = t * 5 + i;
                            selected_score = current_score;
                        }
                    }
                }
                player_role[selected] = 2;
                matched[selected] = true;
            }

            //calcule du role de SUPPORT
            for (int t = 0; t < 2; t++)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!matched[t * 5 + i])
                    {
                        player_role[t * 5 + i] = 4;
                    }
                }
            }
        }

        private int final_decision(string champ1, string champ2, int role_wanted)
        {
            int[] role1 = Data_library.get_champion_role(champ1);
            int[] role2 = Data_library.get_champion_role(champ2);
            int[] required = new int[5] {5, 5, 5, 5, 5};
            int index = 0, i = 0;

            switch(role_wanted)
            {
                case (0):
                    required = new int[5] { 0, 1, 2, 4, 3 };
                    break;
                case (1):
                    required = new int[5] { 1, 0, 4, 3, 2 };
                    break;
                case (2):
                    required = new int[5] { 2, 4, 3, 1, 0 };
                    break;
                case (3):
                    required = new int[5] { 3, 2, 4, 0, 1 };
                    break;
                case (4):
                    required = new int[5] { 4, 2, 0, 1, 3 };
                    break;
                    
            }

            while(index < 5)
            {
                i = 0;
                while (i < 5 && role1[i] != required[index] && role2[i] != required[index])
                {
                    i++;
                }
                if (i == 5 || (role1[i] == required[index] && role2[i] == required[index]))
                {
                    index++;
                }
                else if (role1[i] == required[index])
                {
                    i = 1;
                    index = 5;
                }
                else
                {
                    i = 2;
                    index = 5;
                }
            }
            
            return(i);
        }

        private int get_index_player_role(int role, int team)
        {
            int i = 0;
            for(i = 0; i < 5 && player_role[team * 5 + i] != role; i++)
            {

            }
            return (team * 5 + i);
        }

        private void openProfile(string summonerName, string region, string appdata_dir)
        {
            try
            {
                StreamWriter scriptCreator = new StreamWriter(appdata_dir + "script");
                scriptCreator.WriteLine(summonerName);
                scriptCreator.WriteLine(region);
                scriptCreator.Close();

                ProcessStartInfo pStart = new ProcessStartInfo();
                pStart.FileName = Assembly.GetExecutingAssembly().Location;
                pStart.Arguments = "SummonerOnly";
                pStart.RedirectStandardOutput = false;
                pStart.RedirectStandardError = false;
                pStart.RedirectStandardInput = false;
                pStart.UseShellExecute = true;
                pStart.CreateNoWindow = true;
                Process p = Process.Start(pStart);
            }
            catch(Exception)
            {
            }
        }
    }
}
