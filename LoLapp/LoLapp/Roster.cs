using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Roster
    {
        public List<Summoner> members =  new List<Summoner>();

        public Roster(JsonObject result, JsonObject result2, string region, ref LolRequester requester, List<double> current_champ_ID = null, List<double> players_ID = null)
        {
            JsonObject result3;
            int player_index = 0;
            int i;
            bool enable;
            string tier = "";
            string name2 = "";
            double mostplayed = 0;
            double champId_temp = 0;
            List<JsonObject> fieldtab1 = new List<JsonObject>();
            int mode = -1;
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                members.Add(new Summoner(field, true));
                JsonObject mastery = null;
                if (requester.Request("https://" + region + ".api.pvp.net/championmastery/location/" + requester.ConvertToGameServer(region) + "/player/" + members[members.Count - 1]._id + "/champions?api_key=", true, ref mastery, members[members.Count - 1].name, region, ref mode, false))
                {
                    members[members.Count - 1] = new Summoner(field, true, mastery);
                }
            }
            if(result2 != null)
            {
                foreach (JsonObject field in result2 as JsonObjectCollection)
                {
                    i = 0;
                    enable = false;
                    fieldtab1 = (List<JsonObject>)field.GetValue();
                    while (i < members.Count && field.Name != members[i]._id.ToString())
                    {
                        i++;
                    }
                    if (players_ID != null)
                    {
                        player_index = get_index_player(players_ID, members[i]._id);
                    }
                    if (i < members.Count)
                    {
                        for (int b = 0; b < fieldtab1.Count; b++)
                        {
                            foreach (JsonObject field2 in fieldtab1[b] as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("queue"):
                                        enable = (string)field2.GetValue() == "RANKED_SOLO_5x5";
                                        if (enable)
                                        {
                                            members[i].divisionName = name2;
                                            members[i].divisionTier = tier;
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
                                                        {
                                                            members[i].divisionRank = (string)field3.GetValue();
                                                        }
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
                        bool is_current_champ = false;
                        double current_played = 0;
                        mostplayed = 0;
                        champId_temp = 0;
                        result3 = null;
                        if (requester.Request(string.Format("https://" + region + ".api.pvp.net/api/lol/" + region + "/v1.3/stats/by-summoner/" + members[i]._id + "/ranked?season=SEASON2017&api_key="), true, ref result3, members[i].name, region, ref mode, false, members[i]._id))
                        {
                            foreach (JsonObject field2 in result3 as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("champions"):
                                        fieldtab1 = (List<JsonObject>)field2.GetValue();
                                        for (int j = 0; j < fieldtab1.Count; j++)
                                        {
                                            enable = false;
                                            foreach (JsonObject field3 in (fieldtab1[j]) as JsonObjectCollection)
                                            {
                                                switch (field3.Name)
                                                {
                                                    case ("id"):
                                                        champId_temp = (double)field3.GetValue();
                                                        if (players_ID != null)
                                                        {
                                                            is_current_champ = (double)field3.GetValue() == current_champ_ID[player_index];
                                                        }
                                                        break;
                                                    case ("stats"):
                                                        foreach (JsonObject field4 in field3 as JsonObjectCollection)
                                                        {
                                                            switch (field4.Name)
                                                            {
                                                                case ("totalSessionsPlayed"):
                                                                    if ((double)field4.GetValue() > mostplayed && champId_temp != 0)
                                                                    {
                                                                        mostplayed = (double)field4.GetValue();
                                                                        members[i].main_champions_list[0] = Data_library.get_champion_from_id(champId_temp);
                                                                        enable = true;
                                                                    }
                                                                    if (is_current_champ)
                                                                    {
                                                                        current_played = (double)field4.GetValue();
                                                                    }
                                                                    break;
                                                                case ("totalSessionsWon"):
                                                                    if (enable && mostplayed != 0)
                                                                    {
                                                                        members[i].main_win_rates_list[0] = Math.Round((double)field4.GetValue() * 100 / mostplayed, 0);
                                                                    }
                                                                    if (is_current_champ && current_played != 0)
                                                                    {
                                                                        members[i].current_win_rate = Math.Round((double)field4.GetValue() * 100 / current_played, 0);
                                                                    }
                                                                    break;
                                                                case ("totalChampionKills"):
                                                                    if (enable)
                                                                    {
                                                                        members[i].main_KDA_kill_list[0] = (double)field4.GetValue() / mostplayed;
                                                                    }
                                                                    if (is_current_champ)
                                                                    {
                                                                        members[i].current_KDA_kill = (double)field4.GetValue() / current_played;
                                                                    }
                                                                    break;
                                                                case ("totalDeathsPerSession"):
                                                                    if (enable)
                                                                    {
                                                                        members[i].main_KDA_death_list[0] = (double)field4.GetValue() / mostplayed;
                                                                    }
                                                                    if (is_current_champ)
                                                                    {
                                                                        members[i].current_KDA_death = (double)field4.GetValue() / current_played;
                                                                    }
                                                                    break;
                                                                case ("totalAssists"):
                                                                    if (enable)
                                                                    {
                                                                        members[i].main_KDA_assist_list[0] = (double)field4.GetValue() / mostplayed;
                                                                    }
                                                                    if (is_current_champ)
                                                                    {
                                                                        members[i].current_KDA_assist = (double)field4.GetValue() / current_played;
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

                            members[i].main_KDA_kill_list[0] = Math.Round(members[i].main_KDA_kill_list[0], 1);
                            members[i].main_KDA_death_list[0] = Math.Round(members[i].main_KDA_death_list[0], 1);
                            members[i].main_KDA_assist_list[0] = Math.Round(members[i].main_KDA_assist_list[0], 1);

                            members[i].current_KDA_kill = Math.Round(members[i].current_KDA_kill, 0);
                            members[i].current_KDA_death = Math.Round(members[i].current_KDA_death, 0);
                            members[i].current_KDA_assist = Math.Round(members[i].current_KDA_assist, 0);
                        }
                    }
                }
            }
        }

        private int get_index_player(List<double> players_ID, double ID)
        {
            int i = 0;

            while(i < players_ID.Count && players_ID[i] != ID)
            {
                i++;
            }

            return (i);
        }
    }
}
