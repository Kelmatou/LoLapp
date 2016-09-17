using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Summoner
    {
        public double _id;
        public string name;
        public double profileIcon;
        public double lvl;
        public string divisionName;
        public string divisionRank;
        public string divisionTier;
        public double leaguePoints;
        public double current_KDA_kill;
        public double current_KDA_death;
        public double current_KDA_assist;
        public double current_win_rate;
        public List<string> main_champions_list = new List<string>();
        public List<double> main_game_list = new List<double>();
        public List<double> main_mastery_list = new List<double>();
        public List<double> main_win_rates_list = new List<double>();
        public List<double> main_KDA_kill_list = new List<double>();
        public List<double> main_KDA_death_list = new List<double>();
        public List<double> main_KDA_assist_list = new List<double>();

        public Summoner(JsonObject result)
        {
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                foreach(JsonObject field2 in field as JsonObjectCollection)
                {
                    switch(field2.Name)
                    {
                        case("id"):
                            this._id = (double)field2.GetValue();
                            break;
                        case ("name"):
                            this.name = (string)field2.GetValue();
                            break;
                        case ("profileIconId"):
                            this.profileIcon = (double)field2.GetValue();
                            break;
                        case ("summonerLevel"):
                            this.lvl = (double)field2.GetValue();
                            break;
                    }
                }
            }
            divisionName = "";
            divisionRank = "";
            divisionTier = "Unranked";
            leaguePoints = 0;
            main_champions_list.Add("Unknown");
            main_game_list.Add(0);
            main_mastery_list.Add(0);
            main_KDA_kill_list.Add(0);
            main_KDA_death_list.Add(0);
            main_KDA_assist_list.Add(0);
            current_KDA_kill = 0;
            current_KDA_death = 0;
            current_KDA_assist = 0;
            main_win_rates_list.Add(0);
            current_win_rate = 0;
        }

        public Summoner(JsonObject result, bool rooster_mode, JsonObject result2 = null)
        {
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                try
                {
                    foreach (JsonObject field2 in field as JsonObjectCollection)
                    {
                        switch (field2.Name)
                        {
                            case ("id"):
                                this._id = (double)field2.GetValue();
                                break;
                            case ("name"):
                                this.name = (string)field2.GetValue();
                                break;
                            case ("profileIconId"):
                                this.profileIcon = (double)field2.GetValue();
                                break;
                            case ("summonerLevel"):
                                this.lvl = (double)field2.GetValue();
                                break;
                        }
                    }
                }
                catch(Exception)
                {
                    switch (field.Name)
                    {
                        case ("id"):
                            this._id = (double)field.GetValue();
                            break;
                        case ("name"):
                            this.name = (string)field.GetValue();
                            break;
                        case ("profileIconId"):
                            this.profileIcon = (double)field.GetValue();
                            break;
                        case ("summonerLevel"):
                            this.lvl = (double)field.GetValue();
                            break;
                    }
                }
                
            }
            if(result2 != null)
            {
                List<JsonObject> fieldtab1 = new List<JsonObject>();
                fieldtab1 = (List<JsonObject>)result2.GetValue();
                for (int j = 0; j < fieldtab1.Count && main_mastery_list.Count < 5; j++)
                {
                    foreach (JsonObject field in (fieldtab1[j]) as JsonObjectCollection)
                    {
                        switch (field.Name)
                        {
                            case ("championId"):
                                main_champions_list.Add(Data_library.get_champion_from_id((double)field.GetValue()));
                                break;
                            case ("championLevel"):
                                main_mastery_list.Add((double)field.GetValue());
                                break;
                        }
                    }
                    main_game_list.Add(0);
                    main_KDA_kill_list.Add(0);
                    main_KDA_death_list.Add(0);
                    main_KDA_assist_list.Add(0);
                    main_win_rates_list.Add(0);
                }
            }
            else
            {
                main_champions_list.Add("Unknown");
                main_mastery_list.Add(0);
                main_game_list.Add(0);
                main_KDA_kill_list.Add(0);
                main_KDA_death_list.Add(0);
                main_KDA_assist_list.Add(0);
                main_win_rates_list.Add(0);
            }
            divisionName = "";
            divisionRank = "";
            divisionTier = "Unranked";
            leaguePoints = 0;
            current_KDA_kill = 0;
            current_KDA_death = 0;
            current_KDA_assist = 0;
            current_win_rate = 0;
        }

        public Summoner(JsonObject result1, JsonObject result2, JsonObject result3)
        {
            foreach (JsonObject field in result1 as JsonObjectCollection)
            {
                foreach (JsonObject field2 in field as JsonObjectCollection)
                {
                    switch (field2.Name)
                    {
                        case ("id"):
                            this._id = (double)field2.GetValue();
                            break;
                        case ("name"):
                            this.name = (string)field2.GetValue();
                            break;
                        case ("profileIconId"):
                            this.profileIcon = (double)field2.GetValue();
                            break;
                        case ("summonerLevel"):
                            this.lvl = (double)field2.GetValue();
                            break;
                    }
                }
            }

            List<JsonObject> fieldtab1 = new List<JsonObject>();
            this.divisionName = "";
            this.divisionRank = "";
            this.divisionTier = "Unranked";
            this.leaguePoints = 0;
            bool enable = false;
            string name2 = "";
            string tier = "";
            foreach (JsonObject field in result2 as JsonObjectCollection)
            {
                fieldtab1 = (List<JsonObject>)field.GetValue();
                for (int i = 0; i < fieldtab1.Count; i++)
                {
                    foreach (JsonObject field2 in (fieldtab1[i]) as JsonObjectCollection)
                    {
                        switch (field2.Name)
                        {
                            case ("queue"):
                                enable = (string)field2.GetValue() == "RANKED_SOLO_5x5";
                                if(enable)
                                {
                                    this.divisionTier = tier;
                                    this.divisionName = name2;
                                }
                                break;
                            case ("name"):
                                name2 = (string)field2.GetValue();
                                break;
                            case ("entries"):
                                List<JsonObject> fieldtab2 = (List<JsonObject>)field2.GetValue();
                                for (int j = 0; j < fieldtab2.Count; j++)
                                {
                                    foreach(JsonObject field3 in (fieldtab2[j]) as JsonObjectCollection)
                                    {
                                        switch(field3.Name)
                                        {
                                            case ("division"):
                                                if (enable)
                                                    this.divisionRank = (string)field3.GetValue();
                                                break;
                                            case ("leaguePoints"):
                                                if (enable)
                                                    this.leaguePoints = (double)field3.GetValue();
                                                break;
                                        }
                                    }
                                }
                                break;
                            case("tier"):
                                tier = (string)field2.GetValue();
                                break;
                        }
                    }
                }
            }
            if (result2 != null)
            {
                fieldtab1 = new List<JsonObject>();
                fieldtab1 = (List<JsonObject>)result3.GetValue();
                for (int j = 0; j < fieldtab1.Count && main_mastery_list.Count < 5; j++)
                {
                    foreach (JsonObject field in (fieldtab1[j]) as JsonObjectCollection)
                    {
                        switch (field.Name)
                        {
                            case ("championId"):
                                main_champions_list.Add(Data_library.get_champion_from_id((double)field.GetValue()));
                                break;
                            case ("championLevel"):
                                main_mastery_list.Add((double)field.GetValue());
                                break;
                        }
                    }
                    main_game_list.Add(0);
                    main_KDA_kill_list.Add(0);
                    main_KDA_death_list.Add(0);
                    main_KDA_assist_list.Add(0);
                    main_win_rates_list.Add(0);
                }
            }
            else
            {
                main_champions_list.Add("Unknown");
                main_mastery_list.Add(0);
                main_game_list.Add(0);
                main_KDA_kill_list.Add(0);
                main_KDA_death_list.Add(0);
                main_KDA_assist_list.Add(0);
                main_win_rates_list.Add(0);
            }
            current_KDA_kill = 0;
            current_KDA_death = 0;
            current_KDA_assist = 0;
            current_win_rate = 0;
        }

        public Summoner(JsonObject result1, JsonObject result2, JsonObject result3, JsonObject result4, double current_champ_ID = -1)
        {
            foreach (JsonObject field in result1 as JsonObjectCollection)
            {
                foreach (JsonObject field2 in field as JsonObjectCollection)
                {
                    switch (field2.Name)
                    {
                        case ("id"):
                            this._id = (double)field2.GetValue();
                            break;
                        case ("name"):
                            this.name = (string)field2.GetValue();
                            break;
                        case ("profileIconId"):
                            this.profileIcon = (double)field2.GetValue();
                            break;
                        case ("summonerLevel"):
                            this.lvl = (double)field2.GetValue();
                            break;
                    }
                }
            }

            List<JsonObject> fieldtab1 = new List<JsonObject>();
            this.divisionName = "";
            this.divisionRank = "";
            this.divisionTier = "Unranked";
            this.leaguePoints = 0;
            bool enable = false;
            string name2 = "";
            string tier = "";
            foreach (JsonObject field in result2 as JsonObjectCollection)
            {
                fieldtab1 = (List<JsonObject>)field.GetValue();
                for (int i = 0; i < fieldtab1.Count; i++)
                {
                    foreach (JsonObject field2 in (fieldtab1[i]) as JsonObjectCollection)
                    {
                        switch (field2.Name)
                        {
                            case ("queue"):
                                enable = (string)field2.GetValue() == "RANKED_SOLO_5x5";
                                if (enable)
                                {
                                    this.divisionTier = tier;
                                    this.divisionName = name2;
                                }
                                break;
                            case ("name"):
                                name2 = (string)field2.GetValue();
                                break;
                            case ("entries"):
                                List<JsonObject> fieldtab2 = (List<JsonObject>)field2.GetValue();
                                for (int j = 0; j < fieldtab2.Count; j++)
                                {
                                    foreach (JsonObject field3 in (fieldtab2[j]) as JsonObjectCollection)
                                    {
                                        switch (field3.Name)
                                        {
                                            case ("division"):
                                                if (enable)
                                                    this.divisionRank = (string)field3.GetValue();
                                                break;
                                            case ("leaguePoints"):
                                                if (enable)
                                                    this.leaguePoints = (double)field3.GetValue();
                                                break;
                                        }
                                    }
                                }
                                break;
                            case ("tier"):
                                tier = (string)field2.GetValue();
                                break;
                        }
                    }
                }
            }

            main_champions_list = new List<string>();
            main_game_list = new List<double>();
            main_KDA_kill_list = new List<double>();
            main_KDA_death_list = new List<double>();
            main_KDA_assist_list = new List<double>();
            main_win_rates_list = new List<double>();
            current_KDA_kill = 0;
            current_KDA_death = 0;
            current_KDA_assist = 0;
            bool is_current_champ = false;
            double current_played = 0;
            foreach (JsonObject field in result3 as JsonObjectCollection)
            {
                switch (field.Name)
                {
                    case ("champions"):
                        fieldtab1 = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab1.Count; i++)
                        {
                            enable = false;
                            foreach (JsonObject field2 in (fieldtab1[i]) as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("id"):
                                        if ((double)field2.GetValue() != 0)
                                        {
                                            main_champions_list.Add(Data_library.get_champion_from_id((double)field2.GetValue()));
                                            enable = true;
                                        }
                                        is_current_champ = (double)field2.GetValue() == current_champ_ID;
                                        break;
                                    case ("stats"):
                                        foreach (JsonObject field3 in field2 as JsonObjectCollection)
                                        {
                                            switch (field3.Name)
                                            {
                                                case ("totalSessionsPlayed"):
                                                    if(enable)
                                                    {
                                                        main_game_list.Add((double)field3.GetValue());
                                                    }
                                                    if (is_current_champ)
                                                    {
                                                        current_played = (double)field3.GetValue();
                                                    }
                                                    break;
                                                case ("totalSessionsWon"):
                                                    if(enable)
                                                    {
                                                        if (main_game_list[main_game_list.Count - 1] != 0)
                                                        {
                                                            main_win_rates_list.Add(Math.Round(((double)field3.GetValue() * 100 / main_game_list[main_game_list.Count - 1]), 0));
                                                        }
                                                        else
                                                        {
                                                            main_win_rates_list.Add(0);
                                                        }
                                                    }
                                                    if (is_current_champ && current_played != 0)
                                                    {
                                                        current_win_rate = Math.Round(((double)field3.GetValue() / current_played) * 100, 0);
                                                    }
                                                    break;
                                                case ("totalChampionKills"):
                                                    if(enable)
                                                    {
                                                        if (main_game_list[main_game_list.Count - 1] != 0)
                                                        {
                                                            main_KDA_kill_list.Add((double)field3.GetValue() / main_game_list[main_game_list.Count - 1]);
                                                        }
                                                        else
                                                        {
                                                            main_KDA_kill_list.Add(0);
                                                        }
                                                    }
                                                    if (is_current_champ)
                                                    {
                                                        current_KDA_kill = (double)field3.GetValue() / current_played;
                                                    }
                                                    break;
                                                case ("totalDeathsPerSession"):
                                                    if(enable)
                                                    {
                                                        if (main_game_list[main_game_list.Count - 1] != 0)
                                                        {
                                                            main_KDA_death_list.Add((double)field3.GetValue() / main_game_list[main_game_list.Count - 1]);
                                                        }
                                                        else
                                                        {
                                                            main_KDA_death_list.Add(0);
                                                        }
                                                    }
                                                    if (is_current_champ)
                                                    {
                                                        current_KDA_death = (double)field3.GetValue() / current_played;
                                                    }
                                                    break;
                                                case ("totalAssists"):
                                                    if(enable)
                                                    {
                                                        if (main_game_list[main_game_list.Count - 1] != 0)
                                                        {
                                                            main_KDA_assist_list.Add((double)field3.GetValue() / main_game_list[main_game_list.Count - 1]);
                                                        }
                                                        else
                                                        {
                                                            main_KDA_assist_list.Add(0);
                                                        }
                                                    }
                                                    if (is_current_champ)
                                                    {
                                                        current_KDA_assist = (double)field3.GetValue() / current_played;
                                                    }
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
            }

            sort_by_play_time(ref main_champions_list, ref main_game_list, ref main_KDA_kill_list, ref main_KDA_death_list, ref main_KDA_assist_list, ref main_win_rates_list);
            main_mastery_list = new List<double>();
            for (int i = 0; i < 5 && i < main_KDA_assist_list.Count; i++)
            {
                main_KDA_kill_list[i] = Math.Round(main_KDA_kill_list[i], 1);
                main_KDA_death_list[i] = Math.Round(main_KDA_death_list[i], 1);
                main_KDA_assist_list[i] = Math.Round(main_KDA_assist_list[i], 1);
                fieldtab1 = new List<JsonObject>();
                fieldtab1 = (List<JsonObject>)result4.GetValue();
                string curChamp = "";
                for (int j = 0; j < fieldtab1.Count; j++)
                {
                    foreach (JsonObject field in (fieldtab1[j]) as JsonObjectCollection)
                    {
                        switch (field.Name)
                        {
                            case ("championId"):
                                curChamp = Data_library.get_champion_from_id((double)field.GetValue());
                                break;
                            case ("championLevel"):
                                if(main_champions_list[i] == curChamp)
                                {
                                    main_mastery_list.Add((double)field.GetValue());
                                }
                                break;
                        }
                    }
                }
            }

            adjustPlayTimeWithMastery(ref main_champions_list, ref main_game_list, ref main_KDA_kill_list, ref main_KDA_death_list, ref main_KDA_assist_list, ref main_win_rates_list, ref main_mastery_list);
            current_KDA_kill = Math.Round(current_KDA_kill, 0);
            current_KDA_death = Math.Round(current_KDA_death, 0);
            current_KDA_assist = Math.Round(current_KDA_assist, 0);
        }

        public string get_current_KDA_format()
        {
            return (current_KDA_kill + "/" + current_KDA_death + "/" + current_KDA_assist);
        }

        public string get_main_KDA_format(int number_in_list = 0)
        {
            if(number_in_list < main_KDA_assist_list.Count)
            {
                return (Math.Round(main_KDA_kill_list[number_in_list], 0) + "/" + Math.Round(main_KDA_death_list[number_in_list], 0) + "/" + Math.Round(main_KDA_assist_list[number_in_list], 0));
            }
            return ("0/0/0");
        } //ENCORE ARRONDIS /!\

        public string get_ranked_format()
        {
            if(divisionTier == "Unranked")
            {
                return ("[UNRANKED]");
            }
            else
            {
                return ("[" + divisionTier + " " + divisionRank + "]");
            }
        }

        public string getSummonerMainRole()
        {
            int[] roles = new int[6] { 0, 0, 0, 0, 0, 0 };
            int mainRole = 0;
            int secondMainRole = 0;
            string result = "";
            if (main_champions_list.Count == 0)
                return ("New player");
            for (int i = 0; i < 5 && i < main_champions_list.Count && main_champions_list[i] != "Unknown"; i++)
                roles[Data_library.get_champion_role(main_champions_list[i])[0]] += 1 + (int)main_game_list[i] / 10;
            for (int i = 1; i < 5; i++)
            {
                if (roles[i] > roles[mainRole])
                {
                    mainRole = i;
                    secondMainRole = i;
                }  
                if (roles[i] == roles[mainRole])
                    secondMainRole = i;
            }
            switch (mainRole)
            {
                case (0):
                    result += "Top";
                    break;
                case (1):
                    result += "Jungler";
                    break;
                case (2):
                    result += "Mid";
                    break;
                case (3):
                    result += "Adc";
                    break;
                case (4):
                    result += "Support";
                    break;
            }
            if(mainRole != secondMainRole)
            {
                switch (secondMainRole)
                {
                    case (0):
                        result += " - Top";
                        break;
                    case (1):
                        result += " - Jungler";
                        break;
                    case (2):
                        result += " - Mid";
                        break;
                    case (3):
                        result += " - Adc";
                        break;
                    case (4):
                        result += " - Support";
                        break;
                }
            }

            return (result);
        }

        public void print_all_summoner_info()
        {
            int nbr_spaces_champ_KDA = 13;
            int nbr_spaces_KDA_Win = 3;
            int nbr_spaces_Win_game = 8;
            int nbr_spaces_game_mastery = 11;
            List<string> KDA_format = new List<string>();
            List<string> win_rates = new List<string>();
            for (int i = 0; i < 5 && i < main_KDA_assist_list.Count; i++)
            {
                KDA_format.Add(main_KDA_kill_list[i] + "/" + main_KDA_death_list[i] + "/" + main_KDA_assist_list[i]);
                win_rates.Add(main_win_rates_list[i].ToString() + "%");
            }
            nbr_spaces_champ_KDA = Data_library.max_length(main_champions_list, 13, 5);
            nbr_spaces_KDA_Win = Data_library.max_length(KDA_format, 3, 5);
            nbr_spaces_Win_game = Data_library.max_length(win_rates, 8, 5);
            nbr_spaces_game_mastery = Data_library.max_length(main_game_list, 11, 5);
            Console.ForegroundColor = Data_library.get_division_color(divisionTier);
            Console.Write("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - (name.Length / 2));
            Console.WriteLine(name);
            Data_library.print_n_space((Console.WindowWidth / 2) - (getSummonerMainRole().Length / 2));
            Console.WriteLine(getSummonerMainRole() + "\n");
            if (divisionName != "")
            {
                Data_library.print_n_space((Console.WindowWidth / 2) - (get_ranked_format().Length / 2) - (divisionName.Length / 2) - (leaguePoints.ToString().Length / 2) - 4);
                Console.WriteLine(get_ranked_format() + " - " + divisionName + " - " + leaguePoints + "LP\n\n");
            }
            else
            {
                Data_library.print_n_space((Console.WindowWidth / 2) - (get_ranked_format().Length / 2) - 5);
                Console.WriteLine("LvL " + lvl + " - " + get_ranked_format() + "\n\n");
            }
            Data_library.print_n_space((Console.WindowWidth / 2) - 25);
            Console.Write("Main Champion");
            Data_library.print_n_space(nbr_spaces_champ_KDA - 12);
            Console.Write("KDA");
            Data_library.print_n_space(nbr_spaces_KDA_Win - 2);
            Console.Write("Win Rate");
            Data_library.print_n_space(nbr_spaces_Win_game - 7);
            Console.Write("Game Played");
            Data_library.print_n_space(nbr_spaces_game_mastery - 10);
            Console.WriteLine("Mastery");
            for (int i = 0; i < 5 && i < main_champions_list.Count && main_champions_list[i] != "Unknown"; i++ )
            {
                Data_library.print_n_space((Console.WindowWidth / 2) - 25);
                Console.Write(main_champions_list[i]);
                Data_library.print_n_space(nbr_spaces_champ_KDA - main_champions_list[i].Length + 1);
                Console.Write(KDA_format[i]);
                Data_library.print_n_space(nbr_spaces_KDA_Win - KDA_format[i].Length + 1);
                Console.Write(win_rates[i]);
                Data_library.print_n_space(nbr_spaces_Win_game - win_rates[i].ToString().Length + 1);
                Console.Write(main_game_list[i]);
                Data_library.print_n_space(nbr_spaces_game_mastery - main_game_list[i].ToString().Length + 1);
                Console.WriteLine(main_mastery_list[i]);
            }

            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - (Data_library.get_profileicon_details(profileIcon).Length / 2) - (_id.ToString().Length / 2) - 10);
            Console.WriteLine("Profile icon " + Data_library.get_profileicon_details(profileIcon) + " - ID (" + _id + ")\n\n\n\n");
            Console.ResetColor();
            Data_library.print_n_space((Console.WindowWidth / 2) - 14);
            Console.Write("Press 'Enter' to continue...");
            Data_library.free_waiting_keys();
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {

            }
            Console.Clear();
        }

        public void sort_by_play_time(ref List<string> champions, ref List<double> play_time, ref List<double> kill, ref List<double> death, ref List<double> assist, ref List<double> win) // A OPTI
        {
            int move = 1;
            double swap_double;
            string swap_string;
            while(move > 0)
            {
                move = 0;
                for(int i = 0; i < play_time.Count - 1; i++)
                {
                    if(play_time[i] < play_time[i+1])
                    {
                        swap_string = champions[i];
                        champions[i] = champions[i + 1];
                        champions[i + 1] = swap_string;

                        swap_double = play_time[i];
                        play_time[i] = play_time[i + 1];
                        play_time[i + 1] = swap_double;

                        swap_double = kill[i];
                        kill[i] = kill[i + 1];
                        kill[i + 1] = swap_double;

                        swap_double = death[i];
                        death[i] = death[i + 1];
                        death[i + 1] = swap_double;

                        swap_double = assist[i];
                        assist[i] = assist[i + 1];
                        assist[i + 1] = swap_double;

                        swap_double = win[i];
                        win[i] = win[i + 1];
                        win[i + 1] = swap_double;

                        move++;
                    }
                }
            }
        }

        public void adjustPlayTimeWithMastery(ref List<string> champions, ref List<double> play_time, ref List<double> kill, ref List<double> death, ref List<double> assist, ref List<double> win, ref List<double> mastery) // A OPTI
        {
            int move = 1;
            double swap_double;
            string swap_string;
            while (move > 0)
            {
                move = 0;
                for (int i = 0; i < mastery.Count - 1; i++)
                {
                    if ((play_time[i] == play_time[i + 1] && mastery[i] < mastery[i + 1]) || (play_time[i] == play_time[i + 1] && mastery[i] == mastery[i + 1] && win[i] < win[i + 1]))
                    {
                        swap_string = champions[i];
                        champions[i] = champions[i + 1];
                        champions[i + 1] = swap_string;

                        swap_double = play_time[i];
                        play_time[i] = play_time[i + 1];
                        play_time[i + 1] = swap_double;

                        swap_double = kill[i];
                        kill[i] = kill[i + 1];
                        kill[i + 1] = swap_double;

                        swap_double = death[i];
                        death[i] = death[i + 1];
                        death[i + 1] = swap_double;

                        swap_double = assist[i];
                        assist[i] = assist[i + 1];
                        assist[i + 1] = swap_double;

                        swap_double = win[i];
                        win[i] = win[i + 1];
                        win[i + 1] = swap_double;

                        swap_double = mastery[i];
                        mastery[i] = mastery[i + 1];
                        mastery[i + 1] = swap_double;

                        move++;
                    }
                }
            }
        }
    }
}
