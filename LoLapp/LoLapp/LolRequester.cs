using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LoLapp
{
    public class LolRequester
    {
        private string root; //server
        private const string versionSummonerPath = "v1.4"; //summoner
        private const string versionSummonerDivision = "v2.5"; //league
        private const string versionChampionStat = "v1.3"; //stat
        private const string versionHistory = "v2.2"; //matchlist
        private const string versionHistory_details = "v2.2"; //match
        private const string versionStaticData = "v1.2"; //static data

        private string summonerPath = "/api/lol/{0}/{1}/summoner/by-name/{2}?api_key=";// url pour les infos de base (dont l'ID) //{0} = region; {1} = version api; {2} = summonerName
        private string summonerbyID = "/api/lol/{0}/{1}/summoner/{2}?api_key=";// url pour les infos de base (dont l'ID) //{0} = region; {1} = version api; {2} = summonerID
        private string summonerDivision = "/api/lol/{0}/{1}/league/by-summoner/{2}/entry?api_key=";// url pour les infos de base (dont l'ID) //{0} = region; {1} = version api; {2} = ID summoner
        private string championStat = "/api/lol/{0}/{1}/stats/by-summoner/{2}/ranked?season=SEASON2016&api_key=";// url pour les infos de base (dont l'ID) //{0} = region; {1} = version api; {2} = ID summoner
        private string gamePath = "/observer-mode/rest/consumer/getSpectatorGameInfo/{1}/{2}?api_key="; //url pour les infos de la game //{1} = server; {2} = ID summoner
        private string summonerHistory = "/api/lol/{0}/{1}/matchlist/by-summoner/{2}?api_key=";// url pour les infos de base de l'historique (dont l'ID) //{0} = region; {1} = version api; {2} = ID summoner
        private string summonerHistory_details = "/api/lol/{0}/{1}/match/{2}?api_key=";// url pour les infos detaillee de l'historique //{0} = region; {1} = version api; {2} = ID game
        private string championInfo = "/api/lol/static-data/{0}/{1}/champion/{2}?champData=allytips,enemytips,info,lore,passive,skins,spells,stats&api_key=";// url pour les infos d'un champion //{0} = region; {1} = version api; {2} = ID champion
        private string championMastery = "/championmastery/location/{0}/player/{1}/champions?api_key="; //url pour obtenir le niveau de maitrise des champions d'un summoner //{0} = region; {1} = ID summoner
        private string League_of_Legends_version = "https://global.api.pvp.net/api/lol/static-data/euw/v1.2/versions?api_key=";
        private string League_of_Legends_status = "http://status.leagueoflegends.com/shards/";

        public Rate_Limit request_limit;

        public LolRequester(string appdata_dir = "")
        {
            this.request_limit = new Rate_Limit();
            if(appdata_dir != "")
                loadKeys(appdata_dir);
        }

        private bool loadKeys(string appdata_dir)
        {
            List<string> content = new List<string>();
            content = Data_library.getFileContent(appdata_dir + "keys");
            if(content.Count == 0)
                return(false);
            string hashCode = content[0];
            content.RemoveAt(0);
            if(hashCode == Rate_Limit.hashFile(content))
            {
                for (int i = 0; i < content.Count; i++)
                    request_limit.addKeys(request_limit.decryptKey(content[i]));
            }
            else
            {
                File.Delete(appdata_dir + "keys");
                Data_library.saveFile(appdata_dir + "keys", new List<string>());
                return (false);
            }
            return (true);
        }

        public bool Request(string url, bool add_key, ref JsonObject demand, string summonerName, string region, ref int mode, bool show_error = true, double summonerID = 0)
        {
            string key_to_use = "";
            Uri path;
            while(!request_limit.can_execute_request(ref key_to_use))
            {
                Thread.Sleep(1000);
                request_limit.update();
            }
            if(add_key)
                path = new Uri(string.Format(url + key_to_use));
            else
                path = new Uri(string.Format(url));
            
            request_limit.count_new_request(key_to_use);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                JsonTextParser parser = new JsonTextParser();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                demand = parser.Parse(reader.ReadToEnd());

                reader.Close();
                response.Close();
                return (true);
            }
            catch(Exception e)
            {
                switch (get_error(e.Message))
                {
                    case (400):
                        if (show_error)
                            Console.WriteLine("> LoLapp: Server error: Bad Request");
                        break;
                    case (401):
                        if (show_error)
                            Console.WriteLine("> LoLapp: Server error: Acces denied");
                        break;
                    case (403):
                        if (show_error)
                            Console.WriteLine("> LoLapp: Server error: Forbidden");
                        break;
                    case (404):
                        if (path == new Uri(string.Format(root + summonerPath, region, versionSummonerPath, summonerName)) || path == new Uri(string.Format(root + summonerbyID, region, versionSummonerPath, summonerID))) //SI INFO DE BASE NON TROUVEE
                        {
                            if (show_error)
                                Console.WriteLine("> LoLapp: " + summonerName + " not found on server " + Data_library.convert_server_to_server_name(region));
                        }
                        else if (path == new Uri(string.Format(root + gamePath, region, (ConvertToGameServer(region)), summonerID))) //SI GAME NON TROUVEE
                        {
                            if (show_error)
                                Console.WriteLine("> LoLapp: " + summonerName + " is not currently in a game");
                        }
                        else if (path == new Uri(string.Format(root + championStat, region, versionChampionStat, summonerID))) //SI STATS RANKED NON TROUVEE
                        {
                            if (show_error)
                                Console.WriteLine("> LoLapp: " + summonerName + ": stats not found...");
                        }
                        else if (path == new Uri(string.Format(root + summonerDivision, region, versionSummonerDivision, summonerID))) //SI NIVEAU RANKED NON TROUVEE
                        {
                            if (!show_error)
                                mode = -1;
                            //beaucoup de joueurs sont unranked, on ne le montre pas lors de la recherche
                        }
                        else if (path == new Uri(string.Format(root + summonerHistory, region, versionHistory, summonerID))) //SI HISTORIQUE NON TROUVE
                        {
                            if (show_error)
                                Console.WriteLine("> LoLapp: " + summonerName + ": history not found...");
                        }
                        else if (path == new Uri(string.Format(root + summonerHistory_details, region, versionHistory_details, summonerID))) //SI HISTORIQUE DETAILLE NON TROUVE
                        {
                            if (show_error)
                                Console.WriteLine("> LoLapp: " + summonerName + ": history details not found...");
                        }
                        break;
                    case (429):
                        if (show_error)
                        {
                            Console.SetCursorPosition(0, Console.CursorTop - 1 >= 0 ? Console.CursorTop - 1 : 0);
                            Console.WriteLine("> LoLapp: Server error: Rate limit exceeded");
                        }
                        while (!request_limit.can_execute_request(ref key_to_use, 5))
                        {
                            Thread.Sleep(2000);
                            request_limit.update();
                        }
                        demand = null;
                        return(Request(url, add_key, ref demand, summonerName, region, ref mode, show_error, summonerID));
                    case (500):
                        if (show_error)
                            Console.WriteLine("> LoLapp: Server error: Internal server error");
                        break;
                    case (503):
                        if (show_error)
                            Console.WriteLine("> LoLapp: Server error: Service unavailable");
                        break;
                    default:
                        if(show_error)
                            Console.WriteLine("> LoLapp: Connection error");
                        break;
                }
                return (false);
            }
        }

        public bool keyVerification(string newKey)
        {
            string url = League_of_Legends_version + newKey;
            Uri path = new Uri(string.Format(url));
            JsonObject demand;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                JsonTextParser parser = new JsonTextParser();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                demand = parser.Parse(reader.ReadToEnd());
                JsonTextReader test2 = new JsonTextReader(new StringReader(demand.ToString()));

                reader.Close();
                response.Close();
                return (true);
            }
            catch (Exception e)
            {
                if(get_error(e.Message) >= 500)
                {
                    Console.Write("> Server is busy. Try later");
                }
                return (false);
            }
        }

        private bool Request(Uri url, ref string demand)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                demand = reader.ReadToEnd();

                reader.Close();
                response.Close();
                return (true);
            }
            catch (Exception)
            {
                Console.WriteLine("> LoLapp: Connection error");
                return (false);
            }
        }

        public string getHTMLPage(string url)
        {
            string result = "";
            Uri urlFormat = new Uri(url);
            Request(urlFormat, ref result);
            return (result);
        }

        public string get_League_of_Legends_Patch()
        {
            JsonObject demand = null;
            int mode = -1;
            string version = "Unknown";

            if (Request(League_of_Legends_version, true, ref demand, "", "", ref mode))
            {
                List<JsonObject> fieldtab1 = new List<JsonObject>();
                fieldtab1 = (List<JsonObject>)demand.GetValue();
                version = (string)fieldtab1[0].GetValue();
                mode = 0;
                while(mode < version.Length && version[mode] != '.')
                    mode++;
                mode++;
                while (mode < version.Length && version[mode] != '.')
                    mode++;
                if(mode < version.Length)
                    version = version.Substring(0, mode);
            }

            return (version);
        }

        public string get_League_of_Legends_Status(string server)
        {
            JsonObject demand = null;
            int mode = -1;
            string status = "Unknown";

            if (Request(League_of_Legends_status + server, false, ref demand, "", "", ref mode))
            {
                foreach (JsonObject field in demand as JsonObjectCollection)
                {
                    switch (field.Name)
                    {
                        case ("services"):
                            List<JsonObject> fieldtab1 = new List<JsonObject>();
                            fieldtab1 = (List<JsonObject>)field.GetValue();
                            for (int i = 0; i < fieldtab1.Count; i++)
                            {
                                foreach (JsonObject field2 in (fieldtab1[i]) as JsonObjectCollection)
                                {
                                    switch(field2.Name)
                                    {
                                        case("name"):
                                            if ((string)field2.GetValue() == "Game")
                                                mode = 1;
                                            else
                                                mode = 0;
                                            break;
                                        case("status"):
                                            if(mode == 1)
                                                status = (string)field2.GetValue();
                                            break;
                                    }
                                }
                            }
                        break;
                    }
                }
                switch(status)
                {
                    case ("online"):
                        status = "Online";
                        break;
                    case ("offline"):
                        status = "Offline";
                        break;
                    case ("altert"):
                        status = "Alert";
                        break;
                    case ("deploying"):
                        status = "Deploying";
                        break;
                }
            }

            return (status);
        }

        public Champion GetChampionInfo(string ChampionName)
        {
            double champion_id = Data_library.get_id_from_champion(ChampionName);
            if(champion_id != 0)
            {
                root = "https://global.api.pvp.net";
                JsonObject demand = null;
                int mode = -1;
                if (Request(string.Format(root + championInfo, "euw", versionStaticData, champion_id), true, ref demand, ChampionName, "euw", ref mode, true, champion_id))
                {
                    return (new Champion(demand));
                }
                else
                {
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Data_library.print_n_space(Console.WindowWidth - 1);
                Console.SetCursorPosition(0, Console.CursorTop);
                Data_library.print_n_space((Console.WindowWidth / 2) - 9 - (ChampionName.Length / 2));
                Console.WriteLine(ChampionName + ": Unknown champion\n\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                Console.Write("Press 'Enter' to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {

                }
                Console.Clear();
            }

            return (null);
        }

        public Game GetGame(string summonerName, string region, ref LolRequester requester, string appdata_dir)
        {
            Summoner player = GetSummoner(0, summonerName, region, false);
            root = "https://" + region + ".api.pvp.net";
            if (player != null)
            {
                JsonObject demand = null;
                int mode = -1;

                if (Request(string.Format(root + gamePath, region, (ConvertToGameServer(region)), player._id), true, ref demand, summonerName, region, ref mode, false, player._id))
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    return (new Game(demand, region, ref requester, appdata_dir));
                }
                else
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Data_library.print_n_space((Console.WindowWidth / 2) - 12 - (summonerName.Length / 2));
                    Console.WriteLine(summonerName + " is not currently in game\n\n");
                    Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                    Console.Write("Press 'Enter' to continue...");
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                    {

                    }
                    Console.Clear();
                }
            }
            else
            {
                Console.WriteLine("\n\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 7 - (summonerName.Length / 2) - (Data_library.convert_server_to_server_name(region).Length / 2));
                Console.WriteLine(summonerName + " not found on " + Data_library.convert_server_to_server_name(region) + "\n\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                Console.Write("Press 'Enter' to continue...");
                while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                {

                }
                Console.Clear();
            }

            return (null);
        }

        public History GetHistory(string summonerName, string region, ref LolRequester requester)
        {
            Summoner player = GetSummoner(0, summonerName, region);
            root = "https://" + region + ".api.pvp.net";
            if (player != null)
            {
                JsonObject demand = null;
                int mode = -1;

                if (Request(string.Format(root + summonerHistory, region, versionHistory, player._id), true, ref demand, summonerName, region, ref mode, true, player._id))
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("\n> Waiting for " + summonerName + " history...");
                    Console.SetCursorPosition(0, Console.CursorTop - 3);
                    return (new History(demand, summonerName, region, root, summonerHistory_details, versionHistory_details, ref requester));
                }
            }

            return (null);
        }

        public Summoner GetSummoner(int mode, string summonerName, string region, bool show_error = true, double search_current_KDA = -1)
        {
            /* mode = 0: retourne un summoner avec juste l'ID, le nom...
               mode = 1: ajoute les infos de ranked
               mode = 2: ajoute les stats de ses champions
             */
            root = "https://" + region + ".api.pvp.net";
            JsonObject demand1 = null;

            if (Request(string.Format(root + summonerPath, region, versionSummonerPath, summonerName), true, ref demand1, summonerName, region, ref mode, show_error))
            {
                Summoner player = new Summoner(demand1);
                if (mode == 0)
                {
                    return (player);
                }
                else
                {
                    JsonObject demand2 = null;
                    JsonObject demand4 = null;
                    Request(string.Format(root + championMastery, ConvertToGameServer(region), player._id), true, ref demand4, summonerName, region, ref mode, show_error, player._id);
                    if (Request(string.Format(root + summonerDivision, region, versionSummonerDivision, player._id), true, ref demand2, summonerName, region, ref mode, show_error, player._id))
                    {
                        if (mode == 1)
                        {
                            player = new Summoner(demand1, demand2, demand4);
                            return (player);
                        }
                        else
                        {
                            JsonObject demand3 = null;
                            
                            if (Request(string.Format(root + championStat, region, versionChampionStat, player._id), true, ref demand3, summonerName, region, ref mode, show_error, player._id))
                            {
                                return (new Summoner(demand1, demand2, demand3, demand4, search_current_KDA));
                            }
                            else
                            {
                                if (show_error || mode == -1)
                                {
                                    return (player);
                                }
                            }
                        }
                    }
                    else if (show_error || mode == -1)
                    {
                        player = new Summoner(demand1, false, demand4);
                        return (player);
                    }
                }
            }
            return (null);
        }

        public Summoner GetSummoner(string id, string region, bool show_error = true)
        {
            root = "https://" + region + ".api.pvp.net";
            JsonObject demand1 = null;
            int mode = -1;

            if (Request(string.Format(root + summonerbyID, region, versionSummonerPath, id), true, ref demand1, "", region, ref mode, show_error, Convert.ToDouble(id)))
            {
                return (new Summoner(demand1));
            }
            return (null);
        }

        public Roster GetMultiPlayer(ref LolRequester requester, string summonerName, string region, bool show_error = true, List<double> current_champion_ID = null, List<double> players_ID = null) //L'utilisation de cette fonction suppose que tous les joueurs existent!
        {
            int mode = -1;
            root = "https://" + region + ".api.pvp.net";
            JsonObject demand1 = null;
            JsonObject demand2 = null;

            if (Request(string.Format(root + summonerbyID, region, versionSummonerPath, summonerName), true, ref demand1, summonerName, region, ref mode, show_error))
            {
                Request(string.Format(root + summonerDivision, region, versionSummonerDivision, summonerName), true, ref demand2, summonerName, region, ref mode, show_error);
                return (new Roster(demand1, demand2, region, ref requester, current_champion_ID, players_ID));
            }
            return (null);
        }

        public string ConvertToGameServer(string region)
        {
            if (region == "EUNE" || region == "eune" || region == "EUNe" || region == "eUNE" || region == "EuNE" || region == "EUnE" || region == "EUne" || region == "EuNe" || region == "EunE" || region == "eUNe" || region == "eUnE" || region == "euNE" || region == "Eune" || region == "eUne" || region == "euNe" || region == "eunE")
            {
                return ("EUN1");
            }
            else if (region == "EUW" || region == "euw" || region == "EUw" || region == "eUW" || region == "EuW" || region == "Euw" || region == "eUw" || region == "euW")
            {
                return ("EUW1");
            }
            else if (region == "NA" || region == "na" || region == "Na" || region == "nA")
            {
                return ("NA1");
            }
            else if (region == "BR" || region == "br" || region == "Br" || region == "bR")
            {
                return ("BR1");
            }
            else if (region == "LAN" || region == "lan" || region == "LAn" || region == "lAN" || region == "LaN" || region == "Lan" || region == "lAn" || region == "laN")
            {
                return ("LA1");
            }
            else if (region == "KR" || region == "kr" || region == "Kr" || region == "kR")
            {
                return ("KR");
            }
            else if (region == "OCE" || region == "oce" || region == "Oce" || region == "OCe" || region == "oCe" || region == "oCE" || region == "ocE" || region == "OcE")
            {
                return ("OC1");
            }
            else if (region == "TR" || region == "tr" || region == "Tr" || region == "tR")
            {
                return ("TR1");
            }
            else if (region == "RU" || region == "ru" || region == "Ru" || region == "rU")
            {
                return ("RU");
            }
            else if (region == "LAS" || region == "las" || region == "LAs" || region == "lAS" || region == "LaS" || region == "Las" || region == "lAs" || region == "laS")
            {
                return ("LA2");
            }
            else if (region == "JP" || region == "jp" || region == "Jp" || region == "jP")
            {
                return ("JP1");
            }
            else
            {
                return ("");
            }
        }

        private int get_error(string error)
        {
            int i = 0;
            int code = 0;

            while(i < error.Length && error[i] != '(')
            {
                i++;
            }
            i++;

            while(i < error.Length && Char.IsNumber(error[i]))
            {
                code = code * 10 + Convert.ToInt32("" + error[i]);
                i++;
            }

            return (code);
        }
    }
}
