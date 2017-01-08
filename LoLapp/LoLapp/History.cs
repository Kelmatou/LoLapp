using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LoLapp
{
    public class History
    {
        List<double> game_ID = new List<double>();
        List<double> game_duration = new List<double>();
        List<double> game_date = new List<double>();
        List<double> game_kill = new List<double>();
        List<double> game_death = new List<double>();
        List<double> game_assistance = new List<double>();
        List<double> game_minion = new List<double>();
        List<double> game_monster = new List<double>();
        List<bool> game_won = new List<bool>();
        List<double> game_lvl = new List<double>();
        List<double> game_gold = new List<double>();
        List<string> game_champion = new List<string>();
        List<string> game_role = new List<string>();

        public History(JsonObject result, string summoner, string region, string root, string summonerHistory_details, string versionHistory_details, ref LolRequester requester)
        {
            List<JsonObject> fieldtab = new List<JsonObject>();
            foreach(JsonObject field in result as JsonObjectCollection)
            {
                switch(field.Name)
                {
                    case("matches"):
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count && i < 10; i++)
                        {
                            foreach (JsonObject field2 in (fieldtab[i]) as JsonObjectCollection)
                            {
                                 switch (field2.Name)
                                 {
                                     case ("champion"):
                                         game_champion.Add(Data_library.get_champion_from_id((double)(field2.GetValue())));
                                         break;
                                     case ("matchId"):
                                         game_ID.Add((double)(field2.GetValue()));
                                         break;
                                     case ("lane"):
                                         game_role.Add((formatRole((string)(field2.GetValue()), game_champion[game_champion.Count - 1])));
                                         break;
                                 }
                            }
                        }
                        break;
                }
            }

            int mode = -1;
            for (int i = 0; i < game_ID.Count; i++)
            {
                result = null;
                if (requester.Request(string.Format(root + summonerHistory_details, region, versionHistory_details, game_ID[i]), true, ref result, summoner, region, ref mode, false))
                    History_details(result, game_champion[i]);
            }
        }

        public void History_details(JsonObject result, string champion_name)
        {
            List<JsonObject> fieldtab = new List<JsonObject>();
            bool enable;
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                switch (field.Name)
                {
                    case ("matchCreation"):
                        game_date.Add((double)(field.GetValue()));
                        break;
                    case ("matchDuration"):
                        game_duration.Add((double)(field.GetValue()));
                        break;
                    case ("participants"):
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            enable = false;
                            foreach (JsonObject field2 in (fieldtab[i]) as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("championId"):
                                        enable = Data_library.get_champion_from_id((double)field2.GetValue()) == champion_name;
                                        break;
                                    case ("stats"):
                                        if(enable)
                                        {
                                            foreach (JsonObject field3 in field2 as JsonObjectCollection)
                                            {
                                                switch (field3.Name)
                                                {
                                                    case ("winner"):
                                                        game_won.Add((bool)(field3.GetValue()));
                                                        break;
                                                    case ("deaths"):
                                                        game_death.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("assists"):
                                                        game_assistance.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("kills"):
                                                        game_kill.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("minionsKilled"):
                                                        game_minion.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("neutralMinionsKilled"):
                                                        game_monster.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("champLevel"):
                                                        game_lvl.Add((double)(field3.GetValue()));
                                                        break;
                                                    case ("goldEarned"):
                                                        game_gold.Add((double)(field3.GetValue()));
                                                        break;
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
        }

        private string formatRole(string role, string champion)
        {
            switch(role)
            {
                case ("MID"):
                    return ("Mid");
                case ("MIDDLE"):
                    return ("Mid");
                case ("TOP"):
                    return ("Top");
                case ("BOTTOM"):
                    return (Data_library.get_champion_role(champion)[0] == 3 ? "Adc" : "Support");
                case ("BOT"):
                    return (Data_library.get_champion_role(champion)[0] == 3 ? "Adc" : "Support");
                case ("JUNGLE"):
                    return ("Jungle");
                default:
                    return (role);
            }
        }

        public void print_history(string summonerName)
        {
            int nbr_space_champion_lvl = Data_library.max_length(game_champion,8);
            int nbr_space_gold_date = Data_library.max_length(game_gold, 4);
            int nbr_space_date_duration = 8;
            int nbr_space_role_kill = Data_library.max_length(game_role, 4);
            Console.WriteLine();
            Data_library.print_n_space((Console.WindowWidth / 2) - (summonerName.Length / 2) - 10);
            Console.WriteLine(summonerName + " ranked match history\n\n\n");
            if (Console.BufferWidth <= 53 + nbr_space_champion_lvl + nbr_space_role_kill + nbr_space_gold_date + nbr_space_date_duration)
            {
                Console.SetBufferSize(54 + nbr_space_champion_lvl + nbr_space_role_kill + nbr_space_gold_date + nbr_space_date_duration, Console.BufferHeight);
                Console.SetWindowSize(53 + nbr_space_champion_lvl + nbr_space_role_kill + nbr_space_gold_date + nbr_space_date_duration, Console.WindowHeight);
            }
            Console.Write("  Champion");
            Data_library.print_n_space(nbr_space_champion_lvl - 7);
            Console.Write("Level Role");
            Data_library.print_n_space(nbr_space_role_kill - 3);
            Console.Write("Kill Death Assist Minion Monster Gold");
            Data_library.print_n_space(nbr_space_gold_date - 3);
            Console.Write("Date");
            Data_library.print_n_space(nbr_space_date_duration - 3);
            Console.WriteLine("Duration");
            for(int i = 0; i < game_won.Count; i++)
            {
                if(game_won[i])
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                else
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("  " + game_champion[i]);
                Data_library.print_n_space(nbr_space_champion_lvl - game_champion[i].Length + 1);
                Console.Write(game_lvl[i]);
                Data_library.print_n_space(6 - Convert.ToInt32(game_lvl[i]).ToString().Length);
                Console.Write(game_role[i]);
                Data_library.print_n_space(nbr_space_role_kill - game_role[i].Length + 1);
                Console.Write(game_kill[i]);
                Data_library.print_n_space(5 - Convert.ToInt32(game_kill[i]).ToString().Length);
                Console.Write(game_death[i]);
                Data_library.print_n_space(6 - Convert.ToInt32(game_death[i]).ToString().Length);
                Console.Write(game_assistance[i]);
                Data_library.print_n_space(7 - Convert.ToInt32(game_assistance[i]).ToString().Length);
                Console.Write(game_minion[i]);
                Data_library.print_n_space(7 - Convert.ToInt32(game_minion[i]).ToString().Length);
                Console.Write(game_monster[i]);
                Data_library.print_n_space(8 - Convert.ToInt32(game_monster[i]).ToString().Length);
                Console.Write(game_gold[i]);
                Data_library.print_n_space(nbr_space_gold_date - Convert.ToInt32(game_gold[i]).ToString().Length + 1);
                print_date(game_date[i]);
                Data_library.print_n_space(nbr_space_date_duration - 7);
                print_time(Convert.ToInt32(game_duration[i]));
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
        }

        private void print_time(int sec)
        {
            int min = sec / 60;
            int hour = min / 60;

            if (sec <= 0)
                Console.Write("0s");
            else
            {
                if (hour > 0)
                    Console.Write(hour + "h");
                if (min % 60 > 0)
                    Console.Write((min % 60 < 10 ? "0" : "") + min % 60 + "m");
                if (sec % 60 > 0)
                    Console.Write((sec % 60 < 10 ? "0" : "") + sec % 60 + "s");
            }
            Console.WriteLine();
        }

        private void print_date(double time_epoch)
        {
            DateTime converted_time = new DateTime(1970, 1, 1);
            int day = converted_time.AddMilliseconds(time_epoch).Day + 100; //LE + 100 SERT A FAIRE APPARAITRE LE 0 SI LE NUMERO ETAIT 1 -> 01
            int mounth = converted_time.AddMilliseconds(time_epoch).Month + 100; //MEME RAISON
            int year = converted_time.AddMilliseconds(time_epoch).Year % 100 + 100; //MEME RAISON
            Console.Write(day.ToString().Substring(1, 2) + "/" + mounth.ToString().Substring(1, 2) + "/" + year.ToString().Substring(1, 2));
        }
    }
}
