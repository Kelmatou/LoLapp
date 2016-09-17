using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Team //this class contains all the teamS info from a summoner
    {
        private List<string> teamName = new List<string>();
        private List<string> teamTag = new List<string>();
        private List<string> teamID = new List<string>();
        private List<double> teamCreationDate = new List<double>();
        private List<double> teamLastGame = new List<double>();
        private List<string> DivisionName3x3 = new List<string>();
        private List<string> DivisionRank3x3 = new List<string>();
        private List<string> DivisionTier3x3 = new List<string>();
        private List<string> DivisionName5x5 = new List<string>();
        private List<string> DivisionRank5x5 = new List<string>();
        private List<string> DivisionTier5x5 = new List<string>();
        private List<List<Summoner>> all_summoners = new List<List<Summoner>>();
        private List<List<double>> all_summoners_ID = new List<List<double>>();
        private List<List<double>> all_summoners_Join = new List<List<double>>();
        private string region;
        private string summonerName;

        public Team(JsonObject result, double summonerID, string region, string summonerName, ref LolRequester requester)
        {
            List<JsonObject> fieldtab1 = new List<JsonObject>();
            List<JsonObject> fieldtab2 = new List<JsonObject>();
            int current_team_adding = 0;

            this.region = region;
            this.summonerName = summonerName;

            foreach (JsonObject field in result as JsonObjectCollection)
            {
                fieldtab1 = (List<JsonObject>)field.GetValue();
                for (int i = 0; i < fieldtab1.Count; i++)
                {
                    all_summoners_ID.Add(new List<double>());
                    all_summoners_Join.Add(new List<double>());
                    teamLastGame.Add(-1);
                    foreach (JsonObject field2 in (fieldtab1[i]) as JsonObjectCollection)
                    {
                        switch (field2.Name)
                        {
                            case("tag"):
                                this.teamTag.Add((string)field2.GetValue());
                                break;
                            case ("name"):
                                this.teamName.Add((string)field2.GetValue());
                                break;
                            case ("lastGameDate"):
                                this.teamLastGame[this.teamLastGame.Count - 1] = (double)field2.GetValue();
                                break;
                            case ("createDate"):
                                this.teamCreationDate.Add((double)field2.GetValue());
                                break;
                            case ("fullId"):
                                this.teamID.Add((string)field2.GetValue());
                                break;
                            case ("roster"):
                                foreach(JsonObject field3 in field2 as JsonObjectCollection)
                                {
                                    switch(field3.Name)
                                    {
                                        case("memberList"):
                                            fieldtab2 = (List<JsonObject>)field3.GetValue();
                                            for(int j = 0; j < fieldtab2.Count; j++)
                                            {
                                                foreach (JsonObject field4 in fieldtab2[j] as JsonObjectCollection)
                                                {
                                                    switch (field4.Name)
                                                    {
                                                        case("joinDate"):
                                                            all_summoners_Join[current_team_adding].Add((double)field4.GetValue());
                                                            break;
                                                        case ("playerId"):
                                                            all_summoners_ID[current_team_adding].Add((double)field4.GetValue());
                                                            break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                }
                                break;
                        }
                    }
                    current_team_adding++;
                }  
            }

            int mode = -1;
            string temp_name = "";
            string temp_tier = "";
            bool mode5x5 = true;
            bool target_team = false;
            JsonObject requestDivision = null;
            for(int i = 0; i < teamID.Count; i++)
            {
                DivisionName3x3.Add("");
                DivisionRank3x3.Add("");
                DivisionTier3x3.Add("Unranked");
                DivisionName5x5.Add("");
                DivisionRank5x5.Add("");
                DivisionTier5x5.Add("Unranked");

                if (requester.Request(string.Format("https://" + region + ".api.pvp.net/api/lol/" + region + "/v2.5/league/by-team/" + teamID[i] + "?api_key="), true, ref requestDivision, summonerName, region, ref mode))
                {
                    foreach (JsonObject field in requestDivision as JsonObjectCollection)
                    {
                        fieldtab1 = (List<JsonObject>)field.GetValue();
                        for (int j = 0; j < fieldtab1.Count; j++ )
                        {
                            foreach (JsonObject field2 in fieldtab1[j] as JsonObjectCollection)
                            {
                                switch(field2.Name)
                                {
                                    case("name"):
                                        temp_name = (string)field2.GetValue();
                                        break;
                                    case ("queue"):
                                        mode5x5 = (string)field2.GetValue() == "RANKED_TEAM_5x5";
                                        if(mode5x5)
                                        {
                                            this.DivisionName5x5[this.DivisionName5x5.Count - 1] = temp_name;
                                            this.DivisionTier5x5[this.DivisionTier5x5.Count - 1] = temp_tier;
                                        }
                                        else
                                        {
                                            this.DivisionName3x3[this.DivisionName3x3.Count - 1] = temp_name;
                                            this.DivisionTier3x3[this.DivisionTier3x3.Count - 1] = temp_tier;
                                        }
                                        break;
                                    case ("tier"):
                                        temp_tier = (string)field2.GetValue();
                                        break;
                                    case ("entries"):
                                        fieldtab2 = (List<JsonObject>)field2.GetValue();
                                        for (int k = 0; k < fieldtab2.Count; k++)
                                        {
                                            foreach (JsonObject field3 in fieldtab2[k] as JsonObjectCollection)
                                            {
                                                switch (field3.Name)
                                                {
                                                    case ("division"):
                                                        if(target_team)
                                                        {
                                                            if (mode5x5)
                                                            {
                                                                this.DivisionRank5x5[this.DivisionRank5x5.Count - 1] = (string)field3.GetValue();
                                                            }
                                                            else
                                                            {
                                                                this.DivisionRank3x3[this.DivisionRank3x3.Count - 1] = (string)field3.GetValue();
                                                            }
                                                        }
                                                        break;
                                                    case ("playerOrTeamId"):
                                                        target_team = (string)field3.GetValue() == teamID[i];
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void print_team(int teamNumber, ref LolRequester requester)
        {
            int k = 0;
            int nbr_space_Summoner_Division = 1;
            int nbr_space_Division_Main = 1;
            int nbr_space_Main_KDA = 1;
            int nbr_space_intro = 0;
            List<string> Summoner_name = new List<string>();
            List<string> Summoner_Division = new List<string>();
            List<string> Summoner_Main = new List<string>();
            List<string> Summoner_KDA = new List<string>();
            //INFO TEAM
            nbr_space_intro = 55 + teamName[teamNumber].Length + teamTag[teamNumber].Length;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\n> Creation Date - " + print_date(teamCreationDate[teamNumber]) + " > " + teamName[teamNumber] + " [" + teamTag[teamNumber] + "] < " + (teamLastGame[teamNumber] == -1 ? "Never Played" : print_date(teamLastGame[teamNumber])) + " - Last Game");
            Console.Write("> Ranked 5x5 - [" + format_full_division(DivisionTier5x5[teamNumber], DivisionRank5x5[teamNumber]) + "]");
            Data_library.print_n_space(nbr_space_intro - 32 - format_full_division(DivisionTier5x5[teamNumber], DivisionRank5x5[teamNumber]).Length - format_full_division(DivisionTier3x3[teamNumber], DivisionRank3x3[teamNumber]).Length);
            Console.WriteLine("[" + format_full_division(DivisionTier3x3[teamNumber], DivisionRank3x3[teamNumber]) + "] - Ranked 3x3");
            if (DivisionName3x3[teamNumber] != "")
            {
                if (DivisionName5x5[teamNumber] != "")
                {
                    Console.Write("> Ranked 5x5 - " + DivisionName5x5[teamNumber]);
                    Data_library.print_n_space(nbr_space_intro - 28 - DivisionName5x5[teamNumber].Length - DivisionName3x3[teamNumber].Length);
                    Console.WriteLine(DivisionName3x3[teamNumber] + " - Ranked 3x3");
                }
                else
                {
                    Console.Write("> Ranked 5x5 - Unknown");
                    Data_library.print_n_space(nbr_space_intro - 35 - DivisionName3x3[teamNumber].Length);
                    Console.WriteLine(DivisionName3x3[teamNumber] + " - Ranked 3x3");
                }
            }
            else if (DivisionName5x5[teamNumber] != "")
            {
                Console.Write("> Ranked 5x5 - " + DivisionName5x5[teamNumber]);
                Data_library.print_n_space(nbr_space_intro - 35 - DivisionName5x5[teamNumber].Length);
                Console.WriteLine("Unknown - Ranked 3x3");
            }
            Console.WriteLine();
            //SI ON A PAS LES INFOS DES MEMBRES D'UNE TEAM
            int line = Console.CursorTop;
            if(all_summoners[teamNumber][0] == null)
            {
                Console.Write("> LoLapp: waiting for members stats...");
                string all_ID_to_string = "";
                for(int j = 0; j < all_summoners_ID[teamNumber].Count; j++)
                {
                    if (all_ID_to_string == "")
                    {
                        all_ID_to_string = all_summoners_ID[teamNumber][j].ToString();
                    }
                    else
                    {
                        all_ID_to_string = all_ID_to_string + "," + all_summoners_ID[teamNumber][j].ToString();
                    }
                }
                Roster members_info = null;
                members_info = requester.GetMultiPlayer(ref requester, all_ID_to_string, region, false);
                if(members_info != null)
                {
                    data_sort(members_info, ref Summoner_name, ref Summoner_Division, ref Summoner_Main, ref Summoner_KDA, teamNumber);
                    for (int i = 0; i < members_info.members.Count; i++)
                    {
                        all_summoners[teamNumber][i] = members_info.members[i];
                    }
                }
            }
            
            //AFFICHAGE DES INFO JOUEURS
            if (teamNumber < teamName.Count)
            {
                nbr_space_Summoner_Division = Data_library.max_length_name(all_summoners[teamNumber], 8);
                nbr_space_Division_Main = Data_library.max_length_division(all_summoners[teamNumber], 8);
                nbr_space_Main_KDA = Data_library.max_length_main(all_summoners[teamNumber], 8);
                Console.SetCursorPosition(0, line);
                Console.Write("                                                                                   ");
                Console.SetCursorPosition(0, line);
                Console.Write("> Date");
                Data_library.print_n_space(5);
                Console.Write("Summoner");
                Data_library.print_n_space(nbr_space_Summoner_Division - 7);
                Console.Write("Division");
                Data_library.print_n_space(nbr_space_Division_Main - 7);
                Console.Write("Favorite");
                Data_library.print_n_space(nbr_space_Main_KDA - 7);
                Console.WriteLine("KDA");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                for (int l = 0; l < all_summoners[teamNumber].Count && all_summoners[teamNumber][l] != null; l++)
                {
                    Console.Write("> " + print_date(all_summoners_Join[teamNumber][k]) + " " + all_summoners[teamNumber][l].name);
                    Data_library.print_n_space(nbr_space_Summoner_Division - all_summoners[teamNumber][l].name.Length + 1);
                    Console.Write(all_summoners[teamNumber][l].get_ranked_format());
                    Data_library.print_n_space(nbr_space_Division_Main - all_summoners[teamNumber][l].get_ranked_format().Length + 1);
                    Console.Write(all_summoners[teamNumber][l].main_champions_list[0]);
                    Data_library.print_n_space(nbr_space_Main_KDA - all_summoners[teamNumber][l].main_champions_list[0].Length + 1);
                    Console.WriteLine(all_summoners[teamNumber][l].get_main_KDA_format());
                    k++;
                    if (Console.ForegroundColor == ConsoleColor.DarkBlue)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                    }
                }
                Console.ResetColor();
            }
        }

        public void team_selection_menu(ref LolRequester requester)
        {
            int current_line = 0;
            int previous_line = 0;
            bool validation = false;
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            init_all_summoners(teamName.Count);

            while(current_line < teamName.Count)
            {
                clean_menu(current_line);
                while(!validation)
                {
                    key_pressed = Console.ReadKey(true);
                    apply_team_selection_action(key_pressed, ref current_line, ref previous_line, ref validation, ref requester);
                }
                validation = false;
            }
        }

        public void print_team_selection(int current_line)
        {
            if(teamName.Count > 0)
            {
                for(int i = 0; i < teamName.Count; i++)
                {
                    Console.SetCursorPosition(0, 4 + i);
                    Data_library.print_n_space((Console.WindowWidth / 2) - ((teamName[i].Length + teamTag[i].Length) / 2) - 1);
                    Console.Write(teamName[i] + " [" + teamTag[i] + "]");
                }
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.Write("Main Menu");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(0, 4 + current_line);
                if(current_line < teamName.Count)
                {
                    Data_library.print_n_space((Console.WindowWidth / 2) - ((teamName[current_line].Length + teamTag[current_line].Length) / 2) - 3);
                    Console.Write("> " + teamName[current_line] + " [" + teamTag[current_line] + "] <");
                }
                else
                {
                    Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                    Console.Write("> Main Menu <");
                }
            }
            else
            {
                Console.SetCursorPosition(0, Console.CursorTop + 1);
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.Write("> Main Menu <");
            }
        }

        public void update_team_selection(int current_line, int previous_line)
        {
            Console.SetCursorPosition(0, 4 + current_line);
            Data_library.print_n_space(Console.WindowWidth - 1);
            Console.SetCursorPosition(0, 4 + previous_line);
            Data_library.print_n_space(Console.WindowWidth - 1);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if(current_line == teamName.Count)
            {
                Console.SetCursorPosition(0, 4 + current_line);
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.Write("> Main Menu <");
            }
            else
            {
                Console.SetCursorPosition(0, 4 + current_line);
                Data_library.print_n_space((Console.WindowWidth / 2) - 3 - ((teamName[current_line].Length + teamTag[current_line].Length) / 2));
                Console.Write("> " + teamName[current_line] + " [" + teamTag[current_line] + "] <");
            }
            Console.ResetColor();
            if(previous_line == teamName.Count)
            {
                Console.SetCursorPosition(0, 4 + previous_line);
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.Write("Main Menu");
            }
            else
            {
                Console.SetCursorPosition(0, 4 + previous_line);
                Data_library.print_n_space((Console.WindowWidth / 2) - 1 - ((teamName[previous_line].Length + teamTag[previous_line].Length) / 2));
                Console.Write(teamName[previous_line] + " [" + teamTag[previous_line] + "]");
            }
        }

        public void clean_menu(int current_line)
        {
            Console.Clear();
            Console.SetCursorPosition((Console.WindowWidth / 2) - 6, 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Summoner Team\n");
            Console.ResetColor();
            print_team_selection(current_line);
        }

        public bool apply_team_selection_action(ConsoleKeyInfo key_pressed, ref int current_line, ref int previous_line, ref bool validation, ref LolRequester requester)
        {
            switch (key_pressed.Key)
            {
                case (ConsoleKey.UpArrow):
                    previous_line = current_line;
                    if (current_line == 0)
                    {
                        current_line = teamName.Count;
                    }
                    else
                    {
                        current_line--;
                    }
                    break;
                case (ConsoleKey.DownArrow):
                    previous_line = current_line;
                    if (current_line == teamName.Count)
                    {
                        current_line = 0;
                    }
                    else
                    {
                        current_line++;
                    }
                    break;
                case (ConsoleKey.Escape):
                    previous_line = current_line;
                    if (current_line == teamName.Count)
                    {
                        validation = true;
                    }
                    else
                    {
                        current_line = teamName.Count;
                    }
                    break;
                case (ConsoleKey.Enter):
                    previous_line = current_line;
                    validation = current_line == teamName.Count;
                    break;
                case (ConsoleKey.D0):
                    previous_line = current_line;
                    current_line = 0;
                    break;
                case (ConsoleKey.NumPad0):
                    previous_line = current_line;
                    current_line = 0;
                    break;
                case (ConsoleKey.D1):
                    if (teamName.Count >= 1)
                    {
                        previous_line = current_line;
                        current_line = 1;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad1):
                    if (teamName.Count >= 1)
                    {
                        previous_line = current_line;
                        current_line = 1;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D2):
                    if (teamName.Count >= 2)
                    {
                        previous_line = current_line;
                        current_line = 2;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad2):
                    if (teamName.Count >= 2)
                    {
                        previous_line = current_line;
                        current_line = 2;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D3):
                    if (teamName.Count >= 3)
                    {
                        previous_line = current_line;
                        current_line = 3;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad3):
                    if (teamName.Count >= 3)
                    {
                        previous_line = current_line;
                        current_line = 3;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D4):
                    if (teamName.Count >= 4)
                    {
                        previous_line = current_line;
                        current_line = 4;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad4):
                    if (teamName.Count >= 4)
                    {
                        previous_line = current_line;
                        current_line = 4;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D5):
                    if (teamName.Count >= 5)
                    {
                        previous_line = current_line;
                        current_line = 5;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad5):
                    if (teamName.Count >= 5)
                    {
                        previous_line = current_line;
                        current_line = 5;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D6):
                    if (teamName.Count >= 6)
                    {
                        previous_line = current_line;
                        current_line = 6;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad6):
                    if (teamName.Count >= 6)
                    {
                        previous_line = current_line;
                        current_line = 6;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D7):
                    if (teamName.Count >= 7)
                    {
                        previous_line = current_line;
                        current_line = 7;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad7):
                    if (teamName.Count >= 7)
                    {
                        previous_line = current_line;
                        current_line = 7;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D8):
                    if (teamName.Count >= 8)
                    {
                        previous_line = current_line;
                        current_line = 8;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad8):
                    if (teamName.Count >= 8)
                    {
                        previous_line = current_line;
                        current_line = 8;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.D9):
                    if (teamName.Count >= 9)
                    {
                        previous_line = current_line;
                        current_line = 9;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                case (ConsoleKey.NumPad9):
                    if (teamName.Count >= 9)
                    {
                        previous_line = current_line;
                        current_line = 9;
                    }
                    else
                    {
                        return (false);
                    }
                    break;
                default:
                    return (false);
            }
            if(current_line != previous_line)
            {
                clean_menu(current_line);
            }
            //afficher la team en question!
            if (current_line < teamName.Count && ((key_pressed.Key == ConsoleKey.Enter && all_summoners[current_line][0] == null ) || (all_summoners[current_line][0] != null && current_line != previous_line)))
            {
                Console.SetCursorPosition(0, 4 + teamName.Count + 2);
                print_team(current_line, ref requester);
            }
            return (true);
        }

        private string format_full_division(string Tier, string Rank)
        {
            if(Tier == "Unranked")
            {
                return ("UNRANKED");
            }
            else
            {
                return (Tier + " " + Rank);
            }
        }

        private string print_date(double time_epoch)
        {
            DateTime converted_time = new DateTime(1970, 1, 1);
            int day = converted_time.AddMilliseconds(time_epoch).Day + 100; //LE + 100 SERT A FAIRE APPARAITRE LE 0 SI LE NUMERO ETAIT 1 -> 01
            int mounth = converted_time.AddMilliseconds(time_epoch).Month + 100; //MEME RAISON
            int year = converted_time.AddMilliseconds(time_epoch).Year % 100 + 100; //MEME RAISON
            return(day.ToString().Substring(1, 2) + "/" + mounth.ToString().Substring(1, 2) + "/" + year.ToString().Substring(1, 2));
        }

        private void data_sort(Roster members_info, ref List<string> Summoner_name, ref List<string> Summoner_Division, ref List<string> Summoner_Main, ref List<string> Summoner_KDA, int teamNumber)
        {
            int j;
            for(int i = 0; i < all_summoners_ID[teamNumber].Count; i++)
            {
                j = 0;
                while(all_summoners_ID[teamNumber][i] != members_info.members[j]._id)
                {
                    j++;
                }
                Summoner_name.Add(members_info.members[j].name);
                Summoner_Division.Add("[" + format_full_division(members_info.members[j].divisionTier, members_info.members[j].divisionRank) + "]");
                Summoner_Main.Add(members_info.members[j].main_champions_list[0]);
                Summoner_KDA.Add(members_info.members[j].get_main_KDA_format());
            }
        }

        private void init_all_summoners(int nbr_team)
        {
            for(int i = 0; i < nbr_team; i++)
            {
               all_summoners.Add(new List<Summoner>());
               for(int j = 0; j < 10; j++)
               {
                   all_summoners[i].Add(null);
               }
            }
        }
    }
}
