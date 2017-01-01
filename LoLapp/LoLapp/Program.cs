using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Net.Json;
using Newtonsoft.Json;
using Microsoft.VisualBasic.Devices;
using System.Reflection;
using agsXMPP;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Program
    {
        const string version = "VERSION: 1701012302 [PATCH 6.24]";
        const string patch_compatibility = "6.24";

        static void Main(string[] args)
        {
            int exitCode = 1;
            /*Random rnd = new Random();
            signPatchFile("D:\\Info_project\\C#_project\\Apps\\LoLapp\\LoLapp\\Patch\\patch_6_124", rnd);*/
            while (exitCode != 0)
            {
                try
                {
                    LoLapp(args);
                    exitCode = 0;
                }
                catch (Exception)
                {
                    Console.WriteLine("> LoLapp crashed :'(");
                    if (args.Length == 0)
                        Console.WriteLine("> Restarting...");
                    else
                        exitCode = 0;
                    Thread.Sleep(1500);
                }
            }
        }

        static void LoLapp(string[] args = null)
        {
            Console.Title = "LoLapp";
            Console.SetWindowSize(84, 30);
            Console.SetBufferSize(84, 30);
            Console.CursorVisible = false;
            bool chatOnly = (args.Length > 0 && args[0] == "ChatOnly");
            bool summonerOnly = (args.Length > 0 && args[0] == "SummonerOnly");
            string appdata_dir = Script.init_datapath();
            LolRequester requester = new LolRequester(appdata_dir);
            if (requester.request_limit.allKeys.Count == 0)
            {
                addKeysScreen(ref requester, appdata_dir, false);
                Console.Clear();
            }
            if(!chatOnly && !summonerOnly)
            {
                print_intro(false, "", "", patch_compatibility, "Unknown", 0);
                print_main_menu();
            }

            #region variables
            Computer my_computer = new Computer();
            int current_line = 0;
            int previous_line = 0;
            bool validation = false;
            bool working = true;
            int game_mode = 0;
            bool script_mode = false;
            bool out_of_game = true;
            bool lolclient_connected = client_launched();
            string userName = Script.auto_connect_userName(appdata_dir + "connected_profile");
            string userRegion = Script.auto_connect_userRegion(appdata_dir + "connected_profile");
            List<string> all_users = get_all_users(appdata_dir, userName);
            List<string> allLoLUsers = get_all_lolusers(appdata_dir, userName);
            string League_of_Legends_Version = "Unknown";
            string League_of_Legends_Status = "Unknown";
            if (!chatOnly && my_computer.Network.IsAvailable)
            {
                League_of_Legends_Version = get_League_of_Legends_Version(ref requester);
                League_of_Legends_Status = get_League_of_Legends_Status(ref requester, userRegion);
            }
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            #endregion

            while (working)
            {
                Console.Title = "LoLapp";
                Console.Clear();
                if(!chatOnly && !summonerOnly)
                {
                    print_intro(lolclient_connected, userName, userRegion, League_of_Legends_Version, League_of_Legends_Status, game_mode);
                    print_main_menu(current_line);
                    while (!validation && (!game_launched() || !out_of_game) && !File.Exists(appdata_dir + "script") && !script_mode)
                    {
                        if (Console.KeyAvailable || game_mode > 0)
                        {
                            key_pressed = Console.ReadKey(true);
                            if (apply_main_menu_action(key_pressed, ref current_line, ref previous_line, ref validation, lolclient_connected, userName, userRegion, League_of_Legends_Version, League_of_Legends_Status, ref game_mode))
                            {
                                update_intro(ref lolclient_connected, userName, userRegion, League_of_Legends_Status, game_mode);
                                update_main_menu(current_line, previous_line);
                            }
                            out_of_game = (out_of_game || !game_launched());
                            if (out_of_game && game_mode == 1)
                            {
                                game_mode = 0;
                                update_intro(ref lolclient_connected, userName, userRegion, League_of_Legends_Status, game_mode);
                            }
                        }
                        else
                        {
                            Thread.Sleep(30);
                            out_of_game = (out_of_game || !game_launched());
                            if (out_of_game)
                                game_mode = 0;
                            else
                                game_mode = 1;
                            update_intro(ref lolclient_connected, userName, userRegion, League_of_Legends_Status, game_mode);
                        }
                    }
                    validation = false;
                }

                if (chatOnly)
                    current_line = 2;
                else if (summonerOnly)
                {
                    current_line = 3;
                    if (File.Exists(appdata_dir + "script"))
                        script_mode = true;
                }
                else if (game_launched() && out_of_game)
                {
                    try
                    {
                        StreamWriter script_creator = new StreamWriter(appdata_dir + "script");
                        script_creator.WriteLine("5");
                        script_creator.WriteLine(userName);
                        script_creator.WriteLine(userRegion);
                        script_creator.Close();
                        out_of_game = false;
                        script_mode = true;
                        current_line = -1;
                    }
                    catch (Exception)
                    {
                    }
                }
                else if (File.Exists(appdata_dir + "script"))
                {
                    script_mode = true;
                    current_line = Script.convert_string_choice_to_int(Script.read_script_instruction(appdata_dir + "script"));
                    if (current_line > 9 || current_line < 0)
                    {
                        current_line = -1;
                        script_mode = false;
                    }
                    Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
                }

                try //launch app
                {
                    if (current_line == -1)
                        current_line = 0;
                    else if (current_line == 0)
                    {
                        if (!client_launched())
                            launch_League_of_Legends(appdata_dir);
                        else
                        {
                            Console.SetCursorPosition(0, 25);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Data_library.print_n_space((Console.WindowWidth / 2) - 19);
                            Console.WriteLine("League of Legends is already running...");
                            Thread.Sleep(750);
                            Console.SetCursorPosition(0, 24);
                            Console.ResetColor();
                            Data_library.print_n_space((Console.WindowWidth / 2) + 39);
                        }
                    }
                    else if (current_line == 1)
                    {
                        if (!client_launched())
                            launch_League_of_Legends_PBE(appdata_dir);
                        else
                        {
                            Console.SetCursorPosition(0, 25);
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Data_library.print_n_space((Console.WindowWidth / 2) - 19);
                            Console.WriteLine("League of Legends is already running...");
                            Thread.Sleep(750);
                            Console.SetCursorPosition(0, 24);
                            Console.ResetColor();
                            Data_library.print_n_space((Console.WindowWidth / 2) + 39);
                        }
                    }
                    else if(current_line == 2)
                    {
                        Console.Clear();
                        launch_Chat(ref requester, all_users, allLoLUsers, userRegion, appdata_dir, ref args);
                        working = !chatOnly;
                    }
                    else if (current_line == 3)
                    {
                        Console.Clear();
                        launch_Search_By_Name(ref script_mode, appdata_dir, all_users, ref requester);
                        working = !summonerOnly;
                    }
                    else if (current_line == 4)
                    {
                        Console.Clear();
                        launch_Search_Champion_Info(ref script_mode, appdata_dir + "script", ref requester);
                    }
                    else if (current_line == 5)
                    {
                        Console.Clear();
                        launch_Current_Game(ref script_mode, appdata_dir, all_users, ref requester);
                    }
                    else if (current_line == 6)
                    {
                        Console.Clear();
                        launch_History_By_Name(ref script_mode, appdata_dir, all_users, ref requester);
                    }
                    else if (current_line == 7)
                    {
                        Console.Clear();
                        launch_Patch_Update(lolclient_connected, userName, userRegion, requester, League_of_Legends_Version, League_of_Legends_Status, appdata_dir);
                    }
                    else if (current_line == 8)
                    {
                        Console.Clear();
                        settings(ref userName, ref userRegion, appdata_dir, ref lolclient_connected, League_of_Legends_Version, ref League_of_Legends_Status, ref game_mode, ref all_users, ref requester, ref allLoLUsers);
                    }
                    else if (current_line == 9)
                    {
                        working = false;
                    }
                }
                catch (Exception)
                {
                    Console.ResetColor();
                    Console.WriteLine("\n> LoLapp: an unhandle error occured!\n");
                    Console.WriteLine("\n> LoLapp: Restarting...\n");
                    Thread.Sleep(2000);
                }
            }
        }

        static void launch_League_of_Legends(string appdata_dir)
        {
            string location = get_lol_location(appdata_dir);
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();

            Console.ResetColor();
            if (location != "" && File.Exists(location))
            {
                try
                {
                    launch_app(location);
                }
                catch (Exception)
                {
                    Console.WriteLine("> LoLapp: No permission to launch League of Legends");
                    Thread.Sleep(2000);
                }
            }
            else if (File.Exists("C:\\Riot Games\\League of Legends\\LeagueClient.exe"))
            {
                try
                {
                    launch_app("C:\\Riot Games\\League of Legends\\LeagueClient.exe");
                }
                catch (Exception)
                {
                    Console.WriteLine("> LoLapp: No permission to launch League of Legends");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.Clear();
                Console.Write("\n\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 16);
                Console.Write("League of Legends not found.\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 22);
                Console.Write("Would you like to tell us where it is? (Y/N)\n\n");
                do
                {
                    key_pressed = Console.ReadKey(true);
                }
                while (key_pressed.Key != ConsoleKey.Y && key_pressed.Key != ConsoleKey.N);
                if (key_pressed.Key == ConsoleKey.Y)
                {
                    Console.Write("> LoLapp: Enter path of League of Legends: ");
                    location = Console.ReadLine();
                    if (location.Length > 5 && (location.Substring(location.Length - 4, 4) == ".exe" || location.Substring(location.Length - 4, 4) == ".lnk" || location.Substring(location.Length - 5, 5) == ".exe\"" || location.Substring(location.Length - 5, 5) == ".lnk\""))
                    {
                        save_lol_location(appdata_dir, location);
                        try
                        {
                            launch_app(location);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("> LoLapp: No permission to launch League of Legends");
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        Console.WriteLine("> LoLapp: Invalid file");
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        static void launch_League_of_Legends_PBE(string appdata_dir)
        {
            string location = get_pbe_location(appdata_dir);
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();

            Console.ResetColor();
            if (location != "" && File.Exists(location))
            {
                try
                {
                    launch_app(location);
                }
                catch (Exception)
                {
                    Console.WriteLine("> LoLapp: No permission to launch League of Legends PBE");
                    Thread.Sleep(2000);
                }
            }
            else if (File.Exists("C:\\Riot Games\\League of Legends\\PBE\\PBE\\lol.launcher.exe"))
            {
                try
                {
                    launch_app("C:\\Riot Games\\League of Legends\\PBE\\PBE\\lol.launcher.exe");
                }
                catch (Exception)
                {
                    Console.WriteLine("> LoLapp: No permission to launch League of Legends PBE");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.Clear();
                Console.Write("\n\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 16);
                Console.Write("League of Legends PBE not found.\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 22);
                Console.Write("Would you like to tell us where it is? (Y/N)\n\n");
                do
                {
                    key_pressed = Console.ReadKey(true);
                }
                while (key_pressed.Key != ConsoleKey.Y && key_pressed.Key != ConsoleKey.N);
                if (key_pressed.Key == ConsoleKey.Y)
                {
                    Console.Write("> LoLapp: Enter path of League of Legends PBE: ");
                    location = Console.ReadLine();
                    if (location.Length > 5 && (location.Substring(location.Length - 4, 4) == ".exe" || location.Substring(location.Length - 4, 4) == ".lnk" || location.Substring(location.Length - 5, 5) == ".exe\"" || location.Substring(location.Length - 5, 5) == ".lnk\""))
                    {
                        save_pbe_location(appdata_dir, location);
                        try
                        {
                            launch_app(location);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("> LoLapp: No permission to launch League of Legends PBE");
                            Thread.Sleep(2000);
                        }
                    }
                    else
                    {
                        Console.WriteLine("> LoLapp: Invalid file");
                        Thread.Sleep(2000);
                    }
                }
            }
        }

        static void launch_Chat(ref LolRequester requester, List<string> allUsers, List<string> allLoLusers, string userRegion, string appdata_dir, ref string[] args)
        {
            XmppClientConnection xmpp = new XmppClientConnection();
            try
            {
                Chat.ChatSession(ref xmpp, ref requester, allUsers, ref allLoLusers, userRegion, appdata_dir, args);
            }
            catch(Exception)
            {
                xmpp.Close();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("> LoLapp: Connection error");
                Console.WriteLine("> LoLapp: You've been disconnected from Chat");
                Thread.Sleep(2000);
            }
            args = new string[0];
        }

        static void launch_Search_By_Name(ref bool script_mode, string appdata_dir, List<string> all_users, ref LolRequester requester)
        {
            string summonerName = "";
            string region = "";

            Console.Title = "LoLapp - Summoner Info";
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 6);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Summoner Info\n");
            Console.ResetColor();

            if (script_mode)
            {
                summonerName = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
                region = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n> Summoner name: ");
            if (summonerName != "")
                Console.WriteLine(summonerName);
            else
                summonerName = summoner_selection(all_users);
            Console.Write("> Region: ");
            if (region != "")
                Console.WriteLine(Data_library.convert_server_to_server_name(region));
            else if (summonerName != "")
                region = server_selection(get_server_in_profile_files(appdata_dir + "Profiles/" + summonerName));
            else
                Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (region != "")
            {
                if (Data_library.is_valid_server(ref region))
                {
                    Summoner summoner = requester.GetSummoner(2, summonerName, region, true);
                    Console.SetCursorPosition(0, Console.CursorTop - 2);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Console.SetCursorPosition(0, Console.CursorTop);
                    if (summoner != null)
                    {
                        summoner.print_all_summoner_info();
                    }
                    else
                    {
                        Console.WriteLine("\n\n");
                        Data_library.print_n_space((Console.WindowWidth / 2) - 7 - (summonerName.Length / 2) - (Data_library.convert_server_to_server_name(region).Length / 2));
                        Console.WriteLine(summonerName + " not found on " + Data_library.convert_server_to_server_name(region) + "\n\n");
                        Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                        Console.Write("Press 'Enter' to continue...");
                        while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                        {}
                        Console.Clear();
                    }
                }
                else
                {
                    Console.WriteLine("> LoLapp: " + region + ": Unknown region");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("> LoLapp: Process canceled");
                Thread.Sleep(200);
            }
        }

        static void launch_Search_Champion_Info(ref bool script_mode, string script_path, ref LolRequester requester)
        {
            string ChampionName = "";
            int cursor_begin;

            Console.Title = "LoLapp - Champion Info";
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 6);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Champion Info\n");
            Console.ResetColor();

            if (script_mode)
            {
                ChampionName = Script.read_script_instruction(script_path);
                Script.delete_script_instruction(script_path, ref script_mode);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            cursor_begin = Console.CursorTop + 1;
            Console.Write("\n> Champion name: ");
            if (ChampionName != "")
                Console.WriteLine(ChampionName);
            else
                ChampionName = summoner_selection(Data_library.get_champion_list(), false);
            if (ChampionName != "")
            {
                Console.SetCursorPosition(0, cursor_begin);
                Data_library.print_n_space(ChampionName.Length + 17);
                Console.SetCursorPosition(0, cursor_begin);
                Champion champion = requester.GetChampionInfo(ChampionName);
                if (champion != null)
                    champion.display_champion_info();
            }
            else
            {
                Console.WriteLine("> LoLapp: Process canceled");
                Thread.Sleep(200);
            }
        }

        static void launch_Current_Game(ref bool script_mode, string appdata_dir, List<string> all_users, ref LolRequester requester)
        {
            string summonerName = "";
            string region = "";

            Console.Title = "LoLapp - Summoner Game";
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 6);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Summoner Game\n");
            Console.ResetColor();

            if (script_mode)
            {
                summonerName = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
                region = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n> Summoner name: ");
            if (summonerName != "")
            {
                Console.WriteLine(summonerName);
            }
            else
            {
                summonerName = summoner_selection(all_users);
            }
            Console.Write("> Region: ");
            if (region != "")
            {
                Console.WriteLine(Data_library.convert_server_to_server_name(region));
            }
            else if (summonerName != "")
            {
                region = server_selection(get_server_in_profile_files(appdata_dir + "Profiles/" + summonerName));
            }
            else
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            if (region != "")
            {
                if (Data_library.is_valid_server(ref region))
                {
                    Game summonerGame = requester.GetGame(summonerName, region, ref requester, appdata_dir);
                }
                else
                {
                    Console.WriteLine("> LoLapp: " + region + ": Unknown region");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("> LoLapp: Process canceled");
                Thread.Sleep(200);
            }
        }

        static void launch_History_By_Name(ref bool script_mode, string appdata_dir, List<string> all_users, ref LolRequester requester)
        {
            string summonerName = "";
            string region = "";

            Console.Title = "LoLapp - Summoner History";
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 8);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Summoner History\n");
            Console.ResetColor();

            if (script_mode)
            {
                summonerName = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
                region = Script.read_script_instruction(appdata_dir + "script");
                Script.delete_script_instruction(appdata_dir + "script", ref script_mode);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n> Summoner name: ");
            if (summonerName != "")
            {
                Console.WriteLine(summonerName);
            }
            else
            {
                summonerName = summoner_selection(all_users);
            }
            Console.Write("> Region: ");
            if (region != "")
            {
                Console.WriteLine(Data_library.convert_server_to_server_name(region));
            }
            else if (summonerName != "")
            {
                region = server_selection(get_server_in_profile_files(appdata_dir + "Profiles/" + summonerName));
            }
            else
            {
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            if (region != "")
            {
                if (Data_library.is_valid_server(ref region))
                {
                    History summonerhistory = requester.GetHistory(summonerName, region, ref requester);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Console.WriteLine("\n");
                    Data_library.print_n_space(Console.WindowWidth - 1);
                    Console.SetCursorPosition(0, Console.CursorTop - 3);
                    if (summonerhistory != null)
                    {
                        summonerhistory.print_history(summonerName);
                    }
                    Console.WriteLine("\n\n");
                    Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                    Console.Write("Press 'Enter' to continue...");
                    Data_library.free_waiting_keys();
                    while (Console.ReadKey(true).Key != ConsoleKey.Enter)
                    {
                    }
                    Console.Clear();
                    Console.SetWindowSize(84, 30);
                    Console.SetBufferSize(84, 30);
                }
                else
                {
                    Console.WriteLine("> LoLapp: " + region + ": Unknown region");
                    Thread.Sleep(2000);
                }
            }
            else
            {
                Console.WriteLine("> LoLapp: Process canceled");
                Thread.Sleep(200);
            }
        }

        static void launch_Patch_Update(bool lolclient_connected, string userName, string userRegion, LolRequester requester, string League_of_Legends_version, string League_of_Legends_status, string appdata_dir)
        {
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            string search = "";
            int nbrSpace = 0;
            bool webPatch = false;
            List<string> contentToDisplay = new List<string>();
            int season = 0, update = 0;
            if (League_of_Legends_version != "Unknown")
            {
                season = Convert.ToInt32(League_of_Legends_version[0] + "");
                update = Convert.ToInt32(League_of_Legends_version.Substring(2, League_of_Legends_version.Length - 2));
            }
            else
            {
                season = Convert.ToInt32(patch_compatibility[0] + "");
                update = Convert.ToInt32(patch_compatibility.Substring(2, patch_compatibility.Length - 2));
            }
            string patchFile = "";
            Console.Title = "LoLapp - Patch note";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 5);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Patch note\n");
            Console.ResetColor();

            do
            {
                Console.Clear();
                Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, 0, false, false);
                Console.ResetColor();
                contentToDisplay = new List<string>();
                patchFile = "";
                webPatch = false;

                if(search == "")
                {
                    while (season > 0 && patchFile == "")
                    {
                        while (update >= 0 && patchFile == "")
                        {
                            if (File.Exists(appdata_dir + "Patch/patch_" + season + "_" + update))
                                patchFile = appdata_dir + "Patch/patch_" + season + "_" + update;
                            else
                                update--;
                        }
                        if (update < 0)
                        {
                            season--;
                            update = 30;
                        }
                    }
                }
                else
                {
                    int index = 0;
                    while(index < search.Length && search[index] != '.')
                        index++;
                    season = Convert.ToInt32(search.Substring(0, index));
                    if (index < search.Length - 1)
                        update = Convert.ToInt32(search.Substring(index + 1, search.Length - index - 1));
                    else
                        update = 0;

                    if (File.Exists(appdata_dir + "Patch/patch_" + season + "_" + update))
                        patchFile = appdata_dir + "Patch/patch_" + season + "_" + update;
                    else if(season > 3 && update > 0 && (season < Convert.ToInt32(patch_compatibility.Split('.')[0]) || (season == Convert.ToInt32(patch_compatibility.Split('.')[0]) && update <= Convert.ToInt32(patch_compatibility.Split('.')[1]))))
                    {
                        ProcessStartInfo pStart = new ProcessStartInfo();
                        Process p = Process.Start("http://na.leagueoflegends.com/en/news/game-updates/patch/patch-" + season + update + "-notes");
                        webPatch = true;
                    }
                }
                
                if(!webPatch)
                {
                    contentToDisplay = Data_library.getFileContent(patchFile);

                    if (contentToDisplay.Count > 0)
                    {
                        string hashCode = contentToDisplay[0];
                        contentToDisplay.RemoveAt(0);
                        if (Rate_Limit.hashFile(contentToDisplay) == hashCode)
                        {
                            Console.SetCursorPosition(0, 6);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                            Console.WriteLine("Patch " + season + "." + update + "\n");
                            for (int i = 1; i < contentToDisplay.Count; i++)
                            {
                                if (contentToDisplay[i].Length > 0 && contentToDisplay[i][0] == '<')
                                {
                                    int j = 1;
                                    while (j < contentToDisplay[i].Length && contentToDisplay[i][j] != '>')
                                    {
                                        if (contentToDisplay[i][j] == '-') //red
                                            Console.ForegroundColor = ConsoleColor.DarkRed;
                                        else if (contentToDisplay[i][j] == '+') //green
                                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                                        else if (contentToDisplay[i][j] == 'G') //gray
                                            Console.ForegroundColor = ConsoleColor.DarkGray;
                                        else if (contentToDisplay[i][j] == 'Y') //yellow
                                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                                        else if (contentToDisplay[i][j] == 'B') //blue
                                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                                        else if (contentToDisplay[i][j] == 'N') //null
                                            nbrSpace = 0;
                                        else if (contentToDisplay[i][j] == 'T') //title
                                            nbrSpace = 8;
                                        else if (contentToDisplay[i][j] == 'S') //subtitle
                                            nbrSpace = 10;
                                        else if (contentToDisplay[i][j] == 'E') //equilibrage
                                            nbrSpace = 16;
                                        else if (contentToDisplay[i][j] == 'C') //context
                                            nbrSpace = 12;
                                        j++;
                                    }
                                    contentToDisplay[i] = contentToDisplay[i].Substring(j + 1, contentToDisplay[i].Length - j - 1);
                                }

                                if (contentToDisplay[i].Length >= Console.WindowWidth - 6 - nbrSpace)
                                {
                                    List<string> currentContent = Data_library.cut_string_by_length(contentToDisplay[i], Console.WindowWidth - 6 - nbrSpace);
                                    if (Console.CursorTop + currentContent.Count + 2 >= Console.WindowHeight)
                                        Console.SetBufferSize(Console.WindowWidth, Console.CursorTop + currentContent.Count + 2);
                                    for (int j = 0; j < currentContent.Count; j++)
                                    {
                                        Data_library.print_n_space(nbrSpace + 3);
                                        Console.WriteLine(currentContent[j]);
                                    }
                                }
                                else
                                {
                                    if (Console.CursorTop + 2 >= Console.WindowHeight)
                                        Console.SetBufferSize(Console.WindowWidth, Console.CursorTop + 2);
                                    Data_library.print_n_space(nbrSpace + 3);
                                    Console.WriteLine(contentToDisplay[i]);
                                }
                            }
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.SetBufferSize(Console.WindowWidth, 30);
                            Console.SetWindowSize(Console.WindowWidth, 30);
                            Console.SetCursorPosition(0, 14);
                            Data_library.print_n_space((Console.WindowWidth / 2) - 16);
                            Console.WriteLine("Patch note " + season + "." + update + " was corrupted...");
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.SetBufferSize(Console.WindowWidth, 30);
                        Console.SetWindowSize(Console.WindowWidth, 30);
                        if (search == "")
                        {
                            Console.SetCursorPosition(0, 14);
                            Data_library.print_n_space((Console.WindowWidth / 2) - 13);
                            Console.WriteLine("No Patch note available...");
                        }
                        else
                        {
                            Console.SetCursorPosition(0, 14);
                            Data_library.print_n_space((Console.WindowWidth / 2) - 13);
                            Console.WriteLine("Patch note " + season + "." + update + " unavailable...");
                        }
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.SetBufferSize(Console.WindowWidth, 30);
                    Console.SetWindowSize(Console.WindowWidth, 30);
                    Console.SetCursorPosition(0, 14);
                    Data_library.print_n_space((Console.WindowWidth / 2) - 21);
                    Console.WriteLine("Patch note " + season + "." + update + " is not available in LoLapp");
                }

                if (Console.CursorTop + 2 >= 28)
                    Console.SetBufferSize(Console.WindowWidth, Console.CursorTop + 2);
                else if(version.Length + 3 < Console.WindowWidth)
                {
                    Console.ResetColor();
                    Console.SetBufferSize(Console.WindowWidth, 30);
                    Console.SetWindowSize(Console.WindowWidth, 30);
                    Console.SetCursorPosition(2, 28);
                    Console.Write(version);
                    if (version.Length + 18 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(Console.WindowWidth - 15, 28);
                        Console.WriteLine("Kelmatou Apps©");
                        if (patch_compatibility != League_of_Legends_version && League_of_Legends_version != "Unknown" && (version.Length + League_of_Legends_version.Length + 46) < Console.WindowWidth)
                        {
                            Console.SetCursorPosition(version.Length + 2, 28);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("   Check for Update [PATCH " + League_of_Legends_version + "]");
                            Console.ResetColor();
                        }
                    }
                }
                Console.SetCursorPosition(0, 0);
                Data_library.free_waiting_keys();
                Console.ResetColor();
                search = "";
                do
                {
                    key_pressed = Console.ReadKey(true);
                    if (Console.WindowHeight < 6)
                    {
                        Console.SetBufferSize(Console.WindowWidth, 30);
                        Console.SetWindowSize(Console.WindowWidth, 30);
                    }
                    if (key_pressed.KeyChar >= 48 && key_pressed.KeyChar <= 57)
                    {
                        if (search.Length < 5)
                        {
                            search += key_pressed.KeyChar;
                            if (Console.WindowWidth - 22 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - 22, 5);
                            else
                                Console.SetCursorPosition(0, 5);
                            Console.Write("Research: Patch " + search);
                        }
                    }
                    else if(key_pressed.KeyChar == '.')
                    {
                        if (search.Length > 0 && search.Length < 5 && !search.Contains('.'))
                        {
                            search += key_pressed.KeyChar;
                            if (Console.WindowWidth - 22 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - 22, 5);
                            else
                                Console.SetCursorPosition(0, 5);
                            Console.Write("Research: Patch " + search);
                        }
                    }
                    else if(key_pressed.Key == ConsoleKey.Backspace)
                    {
                        if(search.Length == 1)
                        {
                            search = "";
                            if (Console.WindowWidth - 22 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - 22, 5);
                            else
                                Console.SetCursorPosition(0, 5);
                            Console.Write("                     ");
                        }
                        else if(search.Length > 0)
                        {
                            search = search.Substring(0, search.Length - 1);
                            if (Console.WindowWidth - 22 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - 22, 5);
                            else
                                Console.SetCursorPosition(0, 5);
                            Console.Write("Research: Patch " + search + " ");
                        }
                    }
                    else if (key_pressed.Key == ConsoleKey.Escape || key_pressed.Key == ConsoleKey.Delete)
                    {
                        search = "";
                        if (Console.WindowWidth - 22 >= 0)
                            Console.SetCursorPosition(Console.WindowWidth - 22, 5);
                        else
                            Console.SetCursorPosition(0, 5);
                        Console.Write("                     ");
                    }
                }
                while (key_pressed.Key != ConsoleKey.Enter);
            } while (search != "");
            Console.Clear();
            Console.SetWindowSize(84, 30);
            Console.SetBufferSize(84, 30);
        }

        static void settings(ref string userName, ref string userRegion, string appdata_dir, ref bool lolclient_connected, string League_of_Legends_version, ref string League_of_Legends_status, ref int game_mode, ref List<string> all_users, ref LolRequester requester, ref List<string> allLoLusers)
        {
            int current_line = 0;
            int previous_line = 0;
            bool validation = false;
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();

            while (current_line != 7)
            {
                Console.Title = "LoLapp - Settings";
                Console.WriteLine("\n");
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Settings\n");
                Console.ResetColor();

                print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                print_option_menu(current_line);

                while (!validation)
                {
                    key_pressed = Console.ReadKey(true);
                    if (apply_option_menu_action(key_pressed, ref current_line, ref previous_line, ref validation, lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, ref game_mode, appdata_dir))
                    {
                        update_intro(ref lolclient_connected, userName, userRegion, League_of_Legends_status, game_mode);
                        update_option_menu(current_line, previous_line);
                    }
                }
                validation = false;

                Console.Clear();
                print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);

                if (current_line == 0)
                    settings_add_user(ref userName, ref userRegion, appdata_dir, ref all_users, ref requester);
                else if (current_line == 1)
                    settings_remove_user(ref userName, ref userRegion, appdata_dir, ref all_users);
                else if (current_line == 2)
                    settings_connect_user(ref userName, ref userRegion, appdata_dir, ref all_users, ref requester, ref League_of_Legends_status);
                else if (current_line == 3)
                    settings_print_all_users(appdata_dir, userName);
                else if (current_line == 4)
                    settings_forget_LoLuser(appdata_dir, ref allLoLusers, userName);
                else if (current_line == 5)
                    addKeysScreen(ref requester, appdata_dir, true, lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                else if (current_line == 6)
                    help();
                Console.Clear();
            }
            Console.ResetColor();
        }

        static void settings_add_user(ref string userName, ref string userRegion, string appdata_dir, ref List<string> all_users, ref LolRequester requester)
        {
            string userName_temp;
            string userRegion_temp;

            Console.Title = "LoLapp - Settings - Add";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 1);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Add\n");
            Console.ResetColor();

            Console.SetCursorPosition(34, 17);
            Console.Write("User: ");
            Console.CursorVisible = true;
            userName_temp = Console.ReadLine();
            Console.CursorVisible = false;
            Console.SetCursorPosition(34, 18);
            Console.Write("Region: ");
            userRegion_temp = server_selection(userRegion, Console.ForegroundColor, true);

            if (userRegion_temp == "")
            {
                Console.SetCursorPosition(27, Console.CursorTop);
                Console.WriteLine("Adding new profile canceled");
                Thread.Sleep(200);
            }
            else
            {
                Console.SetCursorPosition(24, Console.CursorTop);
                if (get_server_in_profile_files(appdata_dir + "Profiles/" + userName_temp) == userRegion_temp)
                {
                    Console.Write(userName_temp + " on " + Data_library.convert_server_to_server_name(userRegion_temp) + " already exists!");
                    Thread.Sleep(2000);
                }
                else
                {
                    save_new_profile(appdata_dir, userName_temp, userRegion_temp, ref all_users, ref requester);
                    Thread.Sleep(200);
                }
            }
        }

        static void settings_remove_user(ref string userName, ref string userRegion, string appdata_dir, ref List<string> all_users)
        {
            string userName_temp;

            Console.Title = "LoLapp - Settings - Remove";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 3);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Remove\n");
            Console.ResetColor();

            Console.SetCursorPosition(34, 17);
            if(all_users.Count > 0)
            {
                Console.Write("User: ");
                userName_temp = summoner_selection(all_users, false, Console.ForegroundColor, true);

                if (userName_temp == "")
                {
                    Console.SetCursorPosition(34, Console.CursorTop);
                    Console.WriteLine("Remove canceled");
                    Thread.Sleep(200);
                }
                else
                {
                    Console.SetCursorPosition(27, Console.CursorTop);
                    remove_profile(appdata_dir, userName_temp, ref userName, ref userRegion, ref all_users);
                    Thread.Sleep(200);
                }
            }
            else
            {
                Console.Write("No User to remove.");
                Thread.Sleep(1000);
            }
        }

        static void settings_connect_user(ref string userName, ref string userRegion, string appdata_dir, ref List<string> all_users, ref LolRequester requester, ref string League_of_Legends_status)
        {
            string userName_temp;
            string userRegion_temp = "";
            int all_users_count = all_users.Count;

            Console.Title = "LoLapp - Settings - Connect";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 3);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Connect\n");
            Console.ResetColor();

            Console.SetCursorPosition(34, 17);
            Console.Write("User: ");
            userName_temp = summoner_selection(all_users, true, Console.ForegroundColor, true);
            if (userName_temp != "")
            {
                userRegion_temp = get_server_in_profile_files(appdata_dir + "Profiles/" + userName_temp);
                if (userRegion_temp == "")
                {
                    Console.SetCursorPosition(34, Console.CursorTop);
                    Console.Write("Region: ");
                    userRegion_temp = server_selection(userRegion, Console.ForegroundColor, true);
                }
                else
                {
                    Console.SetCursorPosition(25, Console.CursorTop);
                    save_connected_profile(appdata_dir, userName_temp, userRegion_temp, ref userName, ref userRegion, ref all_users, ref requester, ref League_of_Legends_status, false);
                    all_users_count = -1;
                }
            }

            if (all_users_count >= 0)
            {
                if (userRegion_temp == "" || userName_temp == "")
                {
                    Console.SetCursorPosition(34, Console.CursorTop);
                    Console.WriteLine("Connection canceled");
                    Thread.Sleep(200);
                }
                else
                {
                    Console.SetCursorPosition(25, Console.CursorTop);
                    if (!save_connected_profile(appdata_dir, userName_temp, userRegion_temp, ref userName, ref userRegion, ref all_users, ref requester, ref League_of_Legends_status) && all_users_count < all_users.Count)
                    {
                        all_users.Remove(all_users[all_users.Count - 1]);
                    }
                    Thread.Sleep(200);
                }
            }
        }

        static void settings_print_all_users(string appdata_dir, string userName)
        {
            Console.Title = "LoLapp - Settings - Users";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Users\n");
            Console.ResetColor();

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(34, 7 + i);
                Data_library.print_n_space(Console.WindowWidth - 35);
            }

            List<string> all_users = get_all_users(appdata_dir, userName);

            for (int i = 0; i < all_users.Count; i++)
            {
                if (i % 2 == 0)
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                else
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                if (i < 20)
                    Console.SetCursorPosition(2, 4 + i);
                else if (i < 40)
                    Console.SetCursorPosition(32, -16 + i);
                else if (i < 60)
                    Console.SetCursorPosition(57, -36 + i);
                if (i < 60)
                    Console.Write((i + 1) + " - " + all_users[i]);
            }
            Console.SetCursorPosition(29, 25);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press Enter to continue...");
            Console.ResetColor();
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
            }
        } //jusqu'à 60 joueurs pour l'instant

        static void settings_forget_LoLuser(string appdata_dir, ref List<string> allLoLuser, string userName)
        {
            Console.Title = "LoLapp - Settings - Forget";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Forget\n");
            Console.ResetColor();

            string LoLuser = "";
            Console.SetCursorPosition(34, 17);
            if (allLoLuser.Count > 0)
            {
                Console.Write("LoL User: ");
                LoLuser = summoner_selection(allLoLuser, false, Console.ForegroundColor, true);

                if (LoLuser == "")
                {
                    Console.SetCursorPosition(34, Console.CursorTop);
                    Console.WriteLine("Remove canceled");
                    Thread.Sleep(200);
                }
                else
                {
                    Console.SetCursorPosition(27, Console.CursorTop);
                    forget_LoLuser(appdata_dir, LoLuser, ref allLoLuser, userName);
                    Thread.Sleep(500);
                }
            }
            else
            {
                Console.Write("No LoL User to remove.");
                Thread.Sleep(1000);
            }
        }

        static void addKeysScreen(ref LolRequester requester, string appdata_dir, bool exitPossible, bool lolclient_connected = false, string userName = "", string userRegion = "", string League_of_Legends_version = patch_compatibility, string League_of_Legends_status = "Unknown", int gaming_mode = 0)
        {
            int attempts = 0;
            string newKey = "";
            Console.Clear();
            Console.ResetColor();
            print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, gaming_mode);
            Console.ResetColor();
            Console.SetCursorPosition(25, 17);
            Console.Write("LoLapp uses a Riot API key to run.");
            Console.SetCursorPosition(16, 18);
            Console.Write("You need to sign up at https://developer.riotgames.com/");
            Console.SetCursorPosition(20, 19);
            Console.Write("Then go to your dashboard and pick up the key");
            Console.SetCursorPosition(23, 20);
            Console.Write("(XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX)");
            Console.SetCursorPosition(25, 21);
            Console.Write("Then enter it (copy/paste) there.");

            while (attempts >= 0)
            {
                attempts++;
                if (attempts > 3)
                {
                    Console.SetCursorPosition(18, 23);
                    Console.Write("You need to wait " + (attempts - 3) + " minutes before attempting again");
                    Thread.Sleep((attempts - 3) * 60000);
                }
                Console.SetCursorPosition(18, 23);
                Console.Write("                                                            ");
                Console.SetCursorPosition(25, 23);
                Console.Write("New key: ");
                Console.CursorVisible = true;
                newKey = Console.ReadLine();
                Console.CursorVisible = false;
                Console.SetCursorPosition(25, Console.CursorTop);
                if (Key.isKeyFormat(newKey) && !requester.request_limit.keyIsUsed(newKey) && requester.keyVerification(newKey))
                {
                    requester.request_limit.addKeys(newKey, appdata_dir);
                    attempts = -1;
                    Console.Write("New key added!");
                }
                else if (requester.request_limit.keyIsUsed(newKey))
                {
                    Console.Write("Key is already used");
                }
                else
                {
                    Console.Write("Invalid key!");
                }
                Thread.Sleep(1500);
                Console.SetCursorPosition(25, Console.CursorTop);
                Console.Write("                   ");
                if (exitPossible)
                    attempts = -1;
            }
        }

        static void help()
        {
            Console.Title = "LoLapp - Settings - Help";
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 1);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Help\n");
            Console.ResetColor();
            Console.SetCursorPosition(0, 5);
            Data_library.print_n_space((Console.WindowWidth / 2) - 9);
            Console.Write("                   ");

            for (int i = 0; i < 5; i++)
            {
                Console.SetCursorPosition(34, 7 + i);
                Data_library.print_n_space(Console.WindowWidth - 35);
            }

            Console.SetCursorPosition(2, 4);
            Console.Write("> In main menu:");
            Console.SetCursorPosition(5, 5);
            Console.Write("- Use '↑' and '↓' to select an application.");
            Console.SetCursorPosition(5, 6);
            Console.Write("- Press 'Enter' to validate your selection.");
            Console.SetCursorPosition(5, 7);
            Console.Write("- Press 'L' to launch League of Legends.");
            Console.SetCursorPosition(5, 8);
            Console.Write("- Press 'G' to enable (or disable) Gaming Mode (Eco mode).");
            Console.SetCursorPosition(2, 10);
            Console.Write("> In application:");
            Console.SetCursorPosition(5, 11);
            Console.Write("- Use '←' and '→' to select User name or Server.");
            Console.SetCursorPosition(5, 12);
            Console.Write("- You can also search by writing the name on your keyboard.");
            Console.SetCursorPosition(5, 13);
            Console.Write("- Press 'Enter' to validate your selection each time.");
            Console.SetCursorPosition(7, 14);
            Console.Write("- If you choose 'Enter Username', write your name after 'Enter'.");
            Console.SetCursorPosition(2, 16);
            Console.Write("> In settings:");
            Console.SetCursorPosition(5, 17);
            Console.Write("- Use '↑' and '↓' to select an action.");
            Console.SetCursorPosition(5, 18);
            Console.Write("- Press 'Enter' to validate your selection.");
            Console.SetCursorPosition(5, 19);
            Console.Write("- Connect to LoLapp will launch your game details when the game starts.");
            Console.SetCursorPosition(2, 21);
            Console.Write("> LoLapp:");
            Console.SetCursorPosition(5, 22);
            Console.Write("- If you see any data (except icons) is 'Unknown', check for update.");
            Console.SetCursorPosition(5, 23);
            Console.Write("- If an application returns an error message everytime, check for update.");
            Console.SetCursorPosition(5, 24);
            Console.Write("- Thanks for using LoLapp!");
            Console.SetCursorPosition(29, 26);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Press Enter to continue...");
            Console.ResetColor();

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
            }
        }

        static string get_League_of_Legends_Version(ref LolRequester requester)
        {
            string version = "Unknown";

            version = requester.get_League_of_Legends_Patch();

            return (version);
        }

        static string get_League_of_Legends_Status(ref LolRequester requester, string userRegion)
        {
            string status = "Unknown";

            status = requester.get_League_of_Legends_Status(userRegion);

            return (status);
        }

        static List<string> get_all_users(string appdata_dir, string connected_user)
        {
            List<string> all_users = new List<string>();

            try
            {
                if (Directory.Exists(appdata_dir + "/Profiles/"))
                {
                    List<string> all_profiles_file = Directory.EnumerateFiles(appdata_dir + "/Profiles/").ToList();

                    for (int i = 0; i < all_profiles_file.Count; i++)
                    {
                        if (connected_user == Data_library.extract_shorter_path(all_profiles_file[i]))
                        {
                            all_users.Insert(0, Data_library.extract_shorter_path(all_profiles_file[i]));
                        }
                        else
                        {
                            all_users.Add(Data_library.extract_shorter_path(all_profiles_file[i]));
                        }
                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(appdata_dir + "/Profiles");
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {

            }


            return (all_users);
        }

        static List<string> get_all_lolusers(string appdata_dir, string connected_user)
        {
            List<string> all_lolusers = new List<string>();

            try
            {
                if (Directory.Exists(appdata_dir + "/Profiles/"))
                {
                    List<string> all_profiles_file = Directory.EnumerateFiles(appdata_dir + "/Profiles/").ToList();
                    List<string> content = new List<string>();

                    for (int i = 0; i < all_profiles_file.Count; i++)
                    {
                        content = Data_library.getFileContent(all_profiles_file[i]);
                        if (connected_user == Data_library.extract_shorter_path(all_profiles_file[i]))
                        {
                            if (content.Count > 4 && content[3] != "" && Data_library.Decrypt(content[3], connected_user) != "NO" && Data_library.Decrypt(content[3], connected_user) != "" && Data_library.Decrypt(content[4], content[0]) != "NO")
                                all_lolusers.Insert(0, Data_library.Decrypt(content[3], connected_user));
                        }
                        else
                        {
                            if (content.Count > 4 && content[3] != "" && Data_library.Decrypt(content[3], Data_library.extract_shorter_path(all_profiles_file[i])) != "NO" && Data_library.Decrypt(content[3], content[0]) != "" && Data_library.Decrypt(content[4], content[0]) != "NO")
                                all_lolusers.Add(Data_library.Decrypt(content[3], Data_library.extract_shorter_path(all_profiles_file[i])));
                        }
                    }
                }
                else
                {
                    try
                    {
                        Directory.CreateDirectory(appdata_dir + "/Profiles");
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            catch (Exception)
            {
            }


            return (all_lolusers);
        }

        static void print_intro(bool lolclient_connected, string userName, string userRegion, string League_of_Legends_version, string League_of_Legends_status, int gaming_mode, bool printLogo = true, bool printBottom = true)
        {
            if (lolclient_connected)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if ((userName == "" && 61 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 44 < Console.WindowWidth))
                {
                    Console.SetCursorPosition(Console.WindowWidth - 24, 1);
                    Console.WriteLine("Connected on LoL Client");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if ((userName == "" && 66 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 49 < Console.WindowWidth))
                {
                    Console.SetCursorPosition(Console.WindowWidth - 29, 1);
                    Console.WriteLine("Disconnected from LoL Client");
                }
            }
            if (userName == "")
            {
                if (37 < Console.WindowWidth)
                {
                    Console.SetCursorPosition(2, 1);
                    Console.WriteLine("Unknown user -> settings to log in");
                }
            }
            else
            {
                if (userName.Length + 17 < Console.WindowWidth)
                {
                    Console.SetCursorPosition(2, 1);
                    Console.Write("Welcome back " + userName + "!");
                    if (userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 20 < Console.WindowWidth)
                    {
                        if (League_of_Legends_status == "Online")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                        }
                        else if (League_of_Legends_status == "Offline")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                        }
                        else if (League_of_Legends_status == "Alert")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        else if (League_of_Legends_status == "Deploying")
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                        }
                        bool middle = false;
                        if (lolclient_connected)
                            middle = (userName == "" && 61 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 44 < Console.WindowWidth);
                        else
                            middle = (userName == "" && 66 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 49 < Console.WindowWidth);
                        if (middle)
                        {
                            if(userName == "")
                                for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 35; i++)
                                    Console.Write(" ");
                            else
                                for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 30; i++)
                                    Console.Write(" ");
                        }
                        else if (Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4 >= 0)
                            Console.SetCursorPosition(Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4, 1);
                        Console.Write(" " + Data_library.convert_server_to_server_name(userRegion) + ": " + League_of_Legends_status + " ");
                    }
                }
            }
            Console.ResetColor();
            if (printBottom)
            {
                if (version.Length + 3 < Console.WindowWidth)
                {
                    Console.ResetColor();
                    Console.SetCursorPosition(2, 28);
                    Console.Write(version);
                    if (version.Length + 18 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(Console.WindowWidth - 15, 28);
                        Console.WriteLine("Kelmatou Apps©");
                        if (patch_compatibility != League_of_Legends_version && League_of_Legends_version != "Unknown" && (version.Length + League_of_Legends_version.Length + 46) < Console.WindowWidth)
                        {
                            Console.SetCursorPosition(version.Length + 2, 28);
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write("   Check for Update [PATCH " + League_of_Legends_version + "]");
                            Console.ResetColor();
                        }
                    }
                }
            }
            if (printLogo)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.SetCursorPosition(0, 7);
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("\\              /");
                Console.SetCursorPosition(0, 8);
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine(" \\            /");
                Console.SetCursorPosition(0, 9);
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("-=>  LOLapp  <=-");
                Console.SetCursorPosition(0, 10);
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine(" /            \\");
                Console.SetCursorPosition(0, 11);
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("/              \\");
            }
            if (gaming_mode > 0)
            {
                Console.SetCursorPosition(0, 5);
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                if (gaming_mode == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                Console.Write("<!> Gaming Mode <!>");
            }
            else
            {
                Console.SetCursorPosition(0, 5);
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                Console.Write("                   ");
            }
        }

        static void update_intro(ref bool lolclient_connected, string userName, string userRegion, string League_of_Legends_status, int gaming_mode)
        {
            if (lolclient_connected && !client_launched()) //changement d'état du client
            {
                lolclient_connected = false;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("                                                                                    ");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                if ((userName == "" && 66 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 49 < Console.WindowWidth))
                {
                    Console.SetCursorPosition(Console.WindowWidth - 29, 1);
                    Console.WriteLine("Disconnected from LoL Client");
                }
                if (userName == "")
                {
                    if (37 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(2, 1);
                        Console.WriteLine("Unknown user -> settings to log in");
                    }
                }
                else
                {
                    if (userName.Length + 17 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(2, 1);
                        Console.Write("Welcome back " + userName + "!");
                        if (userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 20 < Console.WindowWidth)
                        {
                            if (League_of_Legends_status == "Online")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            }
                            else if (League_of_Legends_status == "Offline")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else if (League_of_Legends_status == "Alert")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            else if (League_of_Legends_status == "Deploying")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            bool middle = false;
                            if (lolclient_connected)
                                middle = (userName == "" && 61 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 44 < Console.WindowWidth);
                            else
                                middle = (userName == "" && 66 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 49 < Console.WindowWidth);
                            if (middle)
                            {
                                if (userName == "")
                                    for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 35; i++)
                                        Console.Write(" ");
                                else
                                    for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 30; i++)
                                        Console.Write(" ");
                            }
                            else if (Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4, 1);
                            Console.Write(" " + Data_library.convert_server_to_server_name(userRegion) + ": " + League_of_Legends_status + " ");
                        }
                    }
                }
            }
            if (!lolclient_connected && client_launched())
            {
                lolclient_connected = true;
                Console.SetCursorPosition(0, 1);
                Console.WriteLine("                                                                                    ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                if ((userName == "" && 61 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 44 < Console.WindowWidth))
                {
                    Console.SetCursorPosition(Console.WindowWidth - 24, 1);
                    Console.WriteLine("Connected on LoL Client");
                }
                if (userName == "")
                {
                    if (37 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(2, 1);
                        Console.WriteLine("Unknown user -> settings to log in");
                    }
                }
                else
                {
                    if (userName.Length + 17 < Console.WindowWidth)
                    {
                        Console.SetCursorPosition(2, 1);
                        Console.Write("Welcome back " + userName + "!");
                        if (userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 20 < Console.WindowWidth)
                        {
                            if (League_of_Legends_status == "Online")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                            }
                            else if (League_of_Legends_status == "Offline")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                            }
                            else if (League_of_Legends_status == "Alert")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            else if (League_of_Legends_status == "Deploying")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                            }
                            bool middle = false;
                            if (lolclient_connected)
                                middle = (userName == "" && 61 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 44 < Console.WindowWidth);
                            else
                                middle = (userName == "" && 66 < Console.WindowWidth) || (userName != "" && userName.Length + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 49 < Console.WindowWidth);
                            if (middle)
                            {
                                if (userName == "")
                                    for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 35; i++)
                                        Console.Write(" ");
                                else
                                    for (int i = 0; i < (Console.WindowWidth / 2) - userName.Length - (Data_library.convert_server_to_server_name(userRegion).Length / 2) - (League_of_Legends_status.Length / 2) - 17 && Console.CursorLeft + Data_library.convert_server_to_server_name(userRegion).Length + League_of_Legends_status.Length + 3 < Console.WindowWidth - 30; i++)
                                        Console.Write(" ");
                            }
                            else if (Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4 >= 0)
                                Console.SetCursorPosition(Console.WindowWidth - Data_library.convert_server_to_server_name(userRegion).Length - League_of_Legends_status.Length - 4, 1);
                            Console.Write(" " + Data_library.convert_server_to_server_name(userRegion) + ": " + League_of_Legends_status + " ");
                        }
                    }
                }
            }
            if (gaming_mode > 0)
            {
                Console.SetCursorPosition(0, 5);
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                if (gaming_mode == 1)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                }
                Console.Write("<!> Gaming Mode <!>");
            }
            else
            {
                Console.SetCursorPosition(0, 5);
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                Console.Write("                   ");
            }
        }

        static bool game_launched()
        {
            Process[] game_process = Process.GetProcessesByName("League of Legends");
            return (game_process.Length != 0);
        }

        static bool client_launched()
        {
            Process[] game_process = Process.GetProcessesByName("LeagueClientUx");
            if (game_process.Length != 0)
            {
                return (true);
            }
            game_process = Process.GetProcessesByName("LeagueClient");
            return (game_process.Length != 0);
        }

        static void print_main_menu(int current_line = 0)
        {
            if (current_line == 0)
            {
                Console.SetCursorPosition(0, 14);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                Console.WriteLine("> Launch League of Legends <");
            }
            else
            {
                Console.SetCursorPosition(0, 14);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 12);
                Console.WriteLine("Launch League of Legends");
            }
            if (current_line == 1)
            {
                Console.SetCursorPosition(0, 15);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 16);
                Console.WriteLine("> Launch League of Legends PBE <");
            }
            else
            {
                Console.SetCursorPosition(0, 15);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                Console.WriteLine("Launch League of Legends PBE");
            }
            if (current_line == 2)
            {
                Console.SetCursorPosition(0, 16);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("> Launch Chat <");
            }
            else
            {
                Console.SetCursorPosition(0, 16);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                Console.WriteLine("Launch Chat");
            }
            if (current_line == 3)
            {
                Console.SetCursorPosition(0, 17);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("> Summoner info <");
            }
            else
            {
                Console.SetCursorPosition(0, 17);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.WriteLine("Summoner info");
            }
            if (current_line == 4)
            {
                Console.SetCursorPosition(0, 18);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("> Champion info <");
            }
            else
            {
                Console.SetCursorPosition(0, 18);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.WriteLine("Champion info");
            }

            if (current_line == 5)
            {
                Console.SetCursorPosition(0, 19);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                Console.WriteLine("> Current game info <");
            }
            else
            {
                Console.SetCursorPosition(0, 19);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("Current game info");
            }
            if (current_line == 6)
            {
                Console.SetCursorPosition(0, 20);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 12);
                Console.WriteLine("> Ranked match history <");
            }
            else
            {
                Console.SetCursorPosition(0, 20);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                Console.WriteLine("Ranked match history");
            }
            if (current_line == 7)
            {
                Console.SetCursorPosition(0, 21);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("> Patch note <");
            }
            else
            {
                Console.SetCursorPosition(0, 21);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                Console.WriteLine("Patch note");
            }
            if (current_line == 8)
            {
                Console.SetCursorPosition(0, 22);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.WriteLine("> Settings <");
            }
            else
            {
                Console.SetCursorPosition(0, 22);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.WriteLine("Settings");
            }
            if (current_line == 9)
            {
                Console.SetCursorPosition(0, 23);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.WriteLine("> Exit <");
            }
            else
            {
                Console.SetCursorPosition(0, 23);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 2);
                Console.WriteLine("Exit");
            }
        }

        static void update_main_menu(int current_line, int previous_line)
        {
            switch (previous_line)
            {
                case (0):
                    Console.SetCursorPosition(0, 14);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 12);
                    Console.WriteLine("Launch League of Legends     ");
                    break;
                case (1):
                    Console.SetCursorPosition(0, 15);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                    Console.WriteLine("Launch League of Legends PBE     ");
                    break;
                case (2):
                    Console.SetCursorPosition(0, 16);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                    Console.WriteLine("Launch Chat     ");
                    break;
                case (3):
                    Console.SetCursorPosition(0, 17);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                    Console.WriteLine("Summoner info     ");
                    break;
                case (4):
                    Console.SetCursorPosition(0, 18);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                    Console.WriteLine("Champion info     ");
                    break;
                case (5):
                    Console.SetCursorPosition(0, 19);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                    Console.WriteLine("Current game info     ");
                    break;
                case (6):
                    Console.SetCursorPosition(0, 20);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                    Console.WriteLine("Ranked match history     ");
                    break;
                case (7):
                    Console.SetCursorPosition(0, 21);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                    Console.WriteLine("Patch note     ");
                    break;
                case (8):
                    Console.SetCursorPosition(0, 22);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                    Console.WriteLine("Settings     ");
                    break;
                case (9):
                    Console.SetCursorPosition(0, 23);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 2);
                    Console.WriteLine("Exit     ");
                    break;
            }

            switch (current_line)
            {
                case (0):
                    Console.SetCursorPosition(0, 14);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 14);
                    Console.WriteLine("> Launch League of Legends <");
                    break;
                case (1):
                    Console.SetCursorPosition(0, 15);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 16);
                    Console.WriteLine("> Launch League of Legends PBE <");
                    break;
                case (2):
                    Console.SetCursorPosition(0, 16);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("> Launch Chat <");
                    break;
                case (3):
                    Console.SetCursorPosition(0, 17);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                    Console.WriteLine("> Summoner info <");
                    break;
                case (4):
                    Console.SetCursorPosition(0, 18);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                    Console.WriteLine("> Champion info <");
                    break;
                case (5):
                    Console.SetCursorPosition(0, 19);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                    Console.WriteLine("> Current game info <");
                    break;
                case (6):
                    Console.SetCursorPosition(0, 20);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 12);
                    Console.WriteLine("> Ranked match history <");
                    break;
                case (7):
                    Console.SetCursorPosition(0, 21);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("> Patch note <");
                    break;
                case (8):
                    Console.SetCursorPosition(0, 22);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                    Console.WriteLine("> Settings <");
                    break;
                case (9):
                    Console.SetCursorPosition(0, 23);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                    Console.WriteLine("> Exit <");
                    break;
            }
        }

        static bool apply_main_menu_action(ConsoleKeyInfo key_pressed, ref int current_line, ref int previous_line, ref bool validation, bool lolclient_connected, string userName, string userRegion, string League_of_Legends_version, string League_of_Legends_status, ref int game_mode)
        {
            switch (key_pressed.Key)
            {
                case (ConsoleKey.UpArrow):
                    previous_line = current_line;
                    if (current_line == 0)
                        current_line = 9;
                    else
                        current_line--;
                    break;
                case (ConsoleKey.DownArrow):
                    previous_line = current_line;
                    if (current_line == 9)
                        current_line = 0;
                    else
                        current_line++;
                    break;
                case (ConsoleKey.Escape):
                    previous_line = current_line;
                    if (current_line == 9)
                        validation = true;
                    else
                        current_line = 9;
                    break;
                case (ConsoleKey.Enter):
                    validation = true;
                    break;
                case (ConsoleKey.L):
                    previous_line = current_line;
                    current_line = 0;
                    validation = true;
                    break;
                case (ConsoleKey.G):
                    if (game_mode != 2)
                        game_mode = 2;
                    else
                        game_mode = 0;
                    break;
                case (ConsoleKey.F1):
                    Console.Clear();
                    print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                    help();
                    Console.Clear();
                    Console.Title = "LoLapp";
                    print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                    print_main_menu(current_line);
                    break;
                case (ConsoleKey.F4):
                    if (key_pressed.Modifiers == ConsoleModifiers.Alt)
                    {
                        previous_line = current_line;
                        current_line = 9;
                        validation = true;
                    }
                    break;
                case (ConsoleKey.D0):
                    previous_line = current_line;
                    current_line = 0;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad0):
                    previous_line = current_line;
                    current_line = 0;
                    return (current_line != previous_line);
                case (ConsoleKey.D1):
                    previous_line = current_line;
                    current_line = 1;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad1):
                    previous_line = current_line;
                    current_line = 1;
                    return (current_line != previous_line);
                case (ConsoleKey.D2):
                    previous_line = current_line;
                    current_line = 2;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad2):
                    previous_line = current_line;
                    current_line = 2;
                    return (current_line != previous_line);
                case (ConsoleKey.D3):
                    previous_line = current_line;
                    current_line = 3;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad3):
                    previous_line = current_line;
                    current_line = 3;
                    return (current_line != previous_line);
                case (ConsoleKey.D4):
                    previous_line = current_line;
                    current_line = 4;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad4):
                    previous_line = current_line;
                    current_line = 4;
                    return (current_line != previous_line); ;
                case (ConsoleKey.D5):
                    previous_line = current_line;
                    current_line = 5;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad5):
                    previous_line = current_line;
                    current_line = 5;
                    return (current_line != previous_line);
                case (ConsoleKey.D6):
                    previous_line = current_line;
                    current_line = 6;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad6):
                    previous_line = current_line;
                    current_line = 6;
                    return (current_line != previous_line);
                case (ConsoleKey.D7):
                    previous_line = current_line;
                    current_line = 7;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad7):
                    previous_line = current_line;
                    current_line = 7;
                    return (current_line != previous_line);
                case (ConsoleKey.D8):
                    previous_line = current_line;
                    current_line = 8;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad8):
                    previous_line = current_line;
                    current_line = 8;
                    return (current_line != previous_line);
                case (ConsoleKey.D9):
                    previous_line = current_line;
                    current_line = 9;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad9):
                    previous_line = current_line;
                    current_line = 9;
                    return (current_line != previous_line);
                default:
                    return (false);
            }
            return (true);
        }

        static void print_option_menu(int current_line = 0)
        {
            if (current_line == 0)
            {
                Console.SetCursorPosition(0, 15);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                Console.WriteLine("> Add New Profile <");
            }
            else
            {
                Console.SetCursorPosition(0, 15);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("Add New Profile   ");
            }
            if (current_line == 1)
            {
                Console.SetCursorPosition(0, 16);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                Console.WriteLine("> Remove Profile <");
            }
            else
            {
                Console.SetCursorPosition(0, 16);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("Remove Profile   ");
            }
            if (current_line == 2)
            {
                Console.SetCursorPosition(0, 17);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("> Change User <");
            }
            else
            {
                Console.SetCursorPosition(0, 17);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                Console.WriteLine("Change User   ");
            }
            if (current_line == 3)
            {
                Console.SetCursorPosition(0, 18);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                Console.WriteLine("> Display Profiles <");
            }
            else
            {
                Console.SetCursorPosition(0, 18);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                Console.WriteLine("Display Profiles   ");
            }
            if (current_line == 4)
            {
                Console.SetCursorPosition(0, 19);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                Console.WriteLine("> Forget LoL User <");
            }
            else
            {
                Console.SetCursorPosition(0, 19);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("Forget LoL User   ");
            }
            if (current_line == 5)
            {
                Console.SetCursorPosition(0, 20);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                Console.WriteLine("> Add new key <");
            }
            else
            {
                Console.SetCursorPosition(0, 20);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                Console.WriteLine("Add new key   ");
            }
            if (current_line == 6)
            {
                Console.SetCursorPosition(0, 21);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.WriteLine("> Help <");
            }
            else
            {
                Console.SetCursorPosition(0, 21);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 2);
                Console.WriteLine("Help   ");
            }
            if (current_line == 7)
            {
                Console.SetCursorPosition(0, 22);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                Console.WriteLine("> Main Menu <");
            }
            else
            {
                Console.SetCursorPosition(0, 22);
                Console.ResetColor();
                Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                Console.WriteLine("Main Menu   ");
            }
        }

        static void update_option_menu(int current_line, int previous_line)
        {
            switch (previous_line)
            {
                case (0):
                    Console.SetCursorPosition(0, 15);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("Add New Profile   ");
                    break;
                case (1):
                    Console.SetCursorPosition(0, 16);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("Remove Profile   ");
                    break;
                case (2):
                    Console.SetCursorPosition(0, 17);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                    Console.WriteLine("Change User   ");
                    break;
                case (3):
                    Console.SetCursorPosition(0, 18);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 8);
                    Console.WriteLine("Display Profiles   ");
                    break;
                case (4):
                    Console.SetCursorPosition(0, 19);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("Forget LoL User   ");
                    break;
                case (5):
                    Console.SetCursorPosition(0, 20);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 5);
                    Console.WriteLine("Add new key   ");
                    break;
                case (6):
                    Console.SetCursorPosition(0, 21);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 2);
                    Console.WriteLine("Help   ");
                    break;
                case (7):
                    Console.SetCursorPosition(0, 22);
                    Console.ResetColor();
                    Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                    Console.WriteLine("Main Menu   ");
                    break;
            }

            switch (current_line)
            {
                case (0):
                    Console.SetCursorPosition(0, 15);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                    Console.WriteLine("> Add New Profile <");
                    break;
                case (1):
                    Console.SetCursorPosition(0, 16);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                    Console.WriteLine("> Remove Profile <");
                    break;
                case (2):
                    Console.SetCursorPosition(0, 17);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("> Change User <");
                    break;
                case (3):
                    Console.SetCursorPosition(0, 18);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 10);
                    Console.WriteLine("> Display Profiles <");
                    break;
                case (4):
                    Console.SetCursorPosition(0, 19);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 9);
                    Console.WriteLine("> Forget LoL User <");
                    break;
                case (5):
                    Console.SetCursorPosition(0, 20);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 7);
                    Console.WriteLine("> Add new key <");
                    break;
                case (6):
                    Console.SetCursorPosition(0, 21);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                    Console.WriteLine("> Help <");
                    break;
                case (7):
                    Console.SetCursorPosition(0, 22);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Data_library.print_n_space((Console.WindowWidth / 2) - 6);
                    Console.WriteLine("> Main Menu <");
                    break;
            }
        }

        static bool apply_option_menu_action(ConsoleKeyInfo key_pressed, ref int current_line, ref int previous_line, ref bool validation, bool lolclient_connected, string userName, string userRegion, string League_of_Legends_version, string League_of_Legends_status, ref int game_mode, string appdata_dir)
        {
            switch (key_pressed.Key)
            {
                case (ConsoleKey.UpArrow):
                    previous_line = current_line;
                    if (current_line == 0)
                        current_line = 7;
                    else
                        current_line--;
                    break;
                case (ConsoleKey.DownArrow):
                    previous_line = current_line;
                    if (current_line == 7)
                        current_line = 0;
                    else
                        current_line++;
                    break;
                case (ConsoleKey.Escape):
                    previous_line = current_line;
                    if (current_line == 7)
                        validation = true;
                    else
                        current_line = 7;
                    break;
                case (ConsoleKey.Enter):
                    validation = true;
                    break;
                case (ConsoleKey.G):
                    if (game_mode != 2)
                        game_mode = 2;
                    else
                        game_mode = 0;
                    break;
                case (ConsoleKey.F1):
                    Console.Clear();
                    print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                    help();
                    Console.Clear();
                    Console.Title = "LoLapp - Settings";
                    Console.WriteLine("\n");
                    Data_library.print_n_space((Console.WindowWidth / 2) - 4);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Settings\n");
                    Console.ResetColor();
                    print_intro(lolclient_connected, userName, userRegion, League_of_Legends_version, League_of_Legends_status, game_mode);
                    print_option_menu(current_line);
                    break;
                case (ConsoleKey.F4):
                    if (key_pressed.Modifiers == ConsoleModifiers.Alt)
                    {
                        current_line = 7;
                        validation = true;
                        StreamWriter script = new StreamWriter(appdata_dir + "script");
                        script.WriteLine(9);
                        script.Close();
                    }
                    break;
                case (ConsoleKey.D0):
                    previous_line = current_line;
                    current_line = 0;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad0):
                    previous_line = current_line;
                    current_line = 0;
                    return (current_line != previous_line);
                case (ConsoleKey.D1):
                    previous_line = current_line;
                    current_line = 1;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad1):
                    previous_line = current_line;
                    current_line = 1;
                    return (current_line != previous_line);
                case (ConsoleKey.D2):
                    previous_line = current_line;
                    current_line = 2;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad2):
                    previous_line = current_line;
                    current_line = 2;
                    return (current_line != previous_line);
                case (ConsoleKey.D3):
                    previous_line = current_line;
                    current_line = 3;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad3):
                    previous_line = current_line;
                    current_line = 3;
                    return (current_line != previous_line);
                case (ConsoleKey.D4):
                    previous_line = current_line;
                    current_line = 4;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad4):
                    previous_line = current_line;
                    current_line = 4;
                    return (current_line != previous_line);
                case (ConsoleKey.D5):
                    previous_line = current_line;
                    current_line = 5;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad5):
                    previous_line = current_line;
                    current_line = 5;
                    return (current_line != previous_line);
                case (ConsoleKey.D6):
                    previous_line = current_line;
                    current_line = 6;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad6):
                    previous_line = current_line;
                    current_line = 6;
                    return (current_line != previous_line);
                case (ConsoleKey.D7):
                    previous_line = current_line;
                    current_line = 7;
                    return (current_line != previous_line);
                case (ConsoleKey.NumPad7):
                    previous_line = current_line;
                    current_line = 7;
                    return (current_line != previous_line);
                default:
                    return (false);
            }
            return (true);
        }

        static string get_lol_location(string appdata_dir)
        {
            string location = "";
            if (File.Exists(appdata_dir + "/lol_location"))
            {
                try
                {
                    StreamReader reader = new StreamReader(appdata_dir + "/lol_location");

                    location = reader.ReadLine();

                    reader.Close();
                }
                catch (Exception)
                {

                }

                if (location == null)
                    location = "";
            }

            return (location);
        }

        static string get_pbe_location(string appdata_dir)
        {
            string location = "";
            if (File.Exists(appdata_dir + "/pbe_location"))
            {
                try
                {
                    StreamReader reader = new StreamReader(appdata_dir + "/pbe_location");

                    location = reader.ReadLine();

                    reader.Close();
                }
                catch (Exception)
                {

                }

                if (location == null)
                    location = "";
            }

            return (location);
        }

        static void save_lol_location(string appdata_dir, string location)
        {
            try
            {
                StreamWriter writer = new StreamWriter(appdata_dir + "/lol_location");

                writer.WriteLine(location);

                writer.Close();
            }
            catch (Exception)
            {

            }
        }

        static void save_pbe_location(string appdata_dir, string location)
        {
            try
            {
                StreamWriter writer = new StreamWriter(appdata_dir + "/pbe_location");

                writer.WriteLine(location);

                writer.Close();
            }
            catch (Exception)
            {

            }
        }

        static void launch_app(string location)
        {
            try
            {
                ProcessStartInfo pStart = new ProcessStartInfo();
                pStart.FileName = location;
                pStart.RedirectStandardOutput = false;
                pStart.RedirectStandardError = false;
                pStart.RedirectStandardInput = false;
                pStart.UseShellExecute = true;
                pStart.CreateNoWindow = true;
                Process p = Process.Start(pStart);
            }
            catch (Exception)
            {

            }
        }

        static bool save_connected_profile(string appdata_dir, string userName_temp, string userRegion_temp, ref string userName, ref string userRegion, ref List<string> all_users, ref LolRequester requester, ref string League_of_Legends_status, bool verification_needed = true)
        {
            Summoner user = null;
            Computer my_computer = new Computer();

            if (verification_needed || my_computer.Network.IsAvailable)
            {
                user = requester.GetSummoner(2, userName_temp, userRegion_temp, true);
            }

            if (user != null || !verification_needed)
            {
                all_users.Remove(userName_temp);
                all_users.Remove(userName);
                Data_library.insert_in_list_sorted(ref all_users, userName);
                all_users.Insert(0, userName_temp);
                userName = userName_temp;
                userRegion = userRegion_temp;
                Data_library.createProfileFile(appdata_dir + "connected_profile", userName, userRegion, user._id.ToString());
                Data_library.createProfileFile(appdata_dir + "Profiles/" + userName, userName, userRegion, user._id.ToString());
                League_of_Legends_status = get_League_of_Legends_Status(ref requester, userRegion);
                Console.WriteLine("Connected as " + userName + " on " + Data_library.convert_server_to_server_name(userRegion));
            }
            else
            {
                Console.WriteLine("Unable to verify " + userName_temp + " on " + Data_library.convert_server_to_server_name(userRegion_temp));
            }

            return (user != null);
        }

        static void save_new_profile(string appdata_dir, string userName_temp, string userRegion_temp, ref List<string> all_users, ref LolRequester requester)
        {
            Summoner user = requester.GetSummoner(2, userName_temp, userRegion_temp, true);

            if (user != null)
            {
                Data_library.createProfileFile(appdata_dir + "Profiles/" + userName_temp, userName_temp, userRegion_temp, user._id.ToString());
                all_users.Add(userName_temp);
                Console.WriteLine("New profile added: " + userName_temp + " on " + Data_library.convert_server_to_server_name(userRegion_temp));
            }
            else
            {
                Console.WriteLine("Unable to verify " + userName_temp + " on " + Data_library.convert_server_to_server_name(userRegion_temp));
            }
        }

        static void remove_profile(string appdata_dir, string user_to_remove, ref string userName, ref string userRegion, ref List<string> all_users)
        {
            if (File.Exists(appdata_dir + "Profiles/" + user_to_remove))
            {
                try
                {
                    File.Delete(appdata_dir + "Profiles/" + user_to_remove);
                    all_users.Remove(user_to_remove);
                    if (user_to_remove == userName)
                    {
                        userName = "";
                        userRegion = "";
                        StreamWriter writer = new StreamWriter(appdata_dir + "connected_profile");
                        writer.Close();
                    }

                    Console.Write(user_to_remove + " removed successfully!");
                }
                catch (Exception)
                {
                    Console.Write(user_to_remove + " fail to remove!");
                }
            }
            else
            {
                Console.Write(user_to_remove + " is not an user!");
            }
        }

        static void forget_LoLuser(string appdata_dir, string LoLuser, ref List<string> allLoLuser, string userName)
        {
            List<string> all_users = get_all_users(appdata_dir, userName);
            List<string> content = new List<string>();
            bool found = false;
            for (int i = 0; i < all_users.Count; i++)
            {
                if(File.Exists(appdata_dir + "Profiles/" + all_users[i]))
                {
                    content = Data_library.getFileContent(appdata_dir + "Profiles/" + all_users[i]);
                    if (content.Count == 5 && Data_library.Decrypt(content[3], content[0]) == LoLuser)
                    {
                        content.RemoveAt(4);
                        content.RemoveAt(3);
                        Data_library.saveFile(appdata_dir + "Profiles/" + all_users[i], content);
                        allLoLuser.Remove(LoLuser);
                        Console.WriteLine("> " + all_users[i] + " credential preferences forgot");
                        found = true;
                    }
                }
            }
            if (!found)
                Console.WriteLine("> " + LoLuser + " not found");
        }

        static string get_server_in_profile_files(string profile_file)
        {
            string server = "";

            if (File.Exists(profile_file))
            {
                try
                {
                    StreamReader reader = new StreamReader(profile_file);
                    reader.ReadLine();
                    server = reader.ReadLine();
                    reader.Close();
                }
                catch (Exception)
                {

                }
            }

            return (server);
        }

        public static string summoner_selection(List<string> users, bool add_possibility = true, ConsoleColor text_color = ConsoleColor.DarkGreen, bool afficher_version = false)
        {
            string search = "";
            bool searchEnabled = false;

            if (add_possibility)
            {
                users.Add("Enter summoner");
            }
            int current_summoner = 0;
            int begin_pos = Console.CursorLeft;
            int begin_top = Console.CursorTop;
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();

            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(2, Console.BufferHeight - 2);
                if (search != "")
                {
                    if (search.Length == 1)
                        Console.Write("                                ");
                    Console.ForegroundColor = text_color;
                    Console.SetCursorPosition(begin_pos, begin_top);
                    Data_library.print_n_space(Data_library.max_length(users, 0));
                    Console.SetCursorPosition(begin_pos, begin_top);
                    if ((search.Length > users[current_summoner].Length || !Data_library.is_same_word(search, users[current_summoner].Substring(0, search.Length))) && searchEnabled)
                    {
                        if(add_possibility)
                        {
                            if (search.Length <= 24)
                                Console.Write(search + " ");
                            else
                                Console.Write(search.Substring(search.Length - 24, 24) + " ");
                        }
                        else
                            Console.Write(users[current_summoner]);
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    }
                    else
                    {
                        Console.Write(users[current_summoner]);
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                    }
                    Console.SetCursorPosition(2, Console.BufferHeight - 2);
                    if (search.Length <= 24)
                        Console.Write("Research: " + search + " ");
                    else
                        Console.Write("Research: " + search.Substring(search.Length - 24, 24) + " ");
                }
                else if (afficher_version)
                {
                    Console.ResetColor();
                    Console.Write(version);
                    Console.ForegroundColor = text_color;
                    Console.SetCursorPosition(begin_pos, begin_top);
                    Data_library.print_n_space(Data_library.max_length(users, 0));
                    Console.SetCursorPosition(begin_pos, begin_top);
                    Console.Write(users[current_summoner]);
                }
                else
                {
                    Console.Write("           ");
                    Console.ForegroundColor = text_color;
                    Console.SetCursorPosition(begin_pos, begin_top);
                    Data_library.print_n_space(Data_library.max_length(users, 0));
                    Console.SetCursorPosition(begin_pos, begin_top);
                    Console.Write(users[current_summoner]);
                }
                key_pressed = Console.ReadKey(true);
            } while (!apply_action_server_or_user_selection(key_pressed, ref current_summoner, users.Count - 1, ref search, add_possibility, ref searchEnabled, begin_pos, begin_top, users));

            Console.SetCursorPosition(2, Console.BufferHeight - 2);
            Console.Write("                                  ");
            if (afficher_version)
            {
                Console.SetCursorPosition(2, Console.BufferHeight - 2);
                Console.Write(version);
            }
            Console.SetCursorPosition(0, begin_top + 1);
            if (current_summoner >= 0)
            {
                if (add_possibility && current_summoner == users.Count - 1 && search == "")
                {
                    Console.SetCursorPosition(begin_pos, Console.CursorTop - 1);
                    Data_library.print_n_space(14);
                    Console.SetCursorPosition(begin_pos, Console.CursorTop);
                    Console.CursorVisible = true;
                    users[current_summoner] = Console.ReadLine();
                    Console.CursorVisible = false;
                }
                else if(add_possibility && current_summoner == users.Count - 1 && search != "")
                {
                    users[current_summoner] = search;
                }
                else if (add_possibility && current_summoner != users.Count - 1)
                {
                    users.Remove(users[users.Count - 1]);
                }
                Console.ForegroundColor = text_color;
                return (users[current_summoner]);
            }
            else
            {
                if (add_possibility)
                {
                    users.Remove(users[users.Count - 1]);
                }
                Console.ForegroundColor = text_color;
                return ("");
            }
        }

        public static string server_selection(string userRegion, ConsoleColor text_color = ConsoleColor.DarkGreen, bool afficher_version = false)
        {
            string[] all_servers = connectedServerFirst(new string[11] { "euw", "eune", "na", "br", "jp", "lan", "las", "kr", "oce", "tr", "ru" }, userRegion);
            string[] all_servers_name = connectedServerFirst(new string[11] { "EU West", "EU Nordic & East", "North America", "Brazil", "Japan", "Latin America North", "Latin America South", "Korea", "Oceania", "Turkey", "Russia" }, Data_library.convert_server_to_server_name(userRegion));
            string search = "";
            int current_server = 0;
            bool argBool = false;
            int begin_pos = Console.CursorLeft;
            int begin_top = Console.CursorTop;
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();

            do
            {
                Console.ResetColor();
                Console.SetCursorPosition(2, Console.BufferHeight - 2);
                if (search != "")
                {
                    if (search.Length == 1)
                    {
                        Console.Write("                                ");
                        Console.SetCursorPosition(2, Console.BufferHeight - 2);
                    }
                    if (search.Length > all_servers_name[current_server].Length || !Data_library.is_same_word(search, all_servers_name[current_server].Substring(0, search.Length)))
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    if (search.Length <= 24)
                        Console.Write("Research: " + search + " ");
                    else
                        Console.Write("Research: " + search.Substring(search.Length - 24, 24) + " ");
                }
                else if (afficher_version)
                {
                    Console.ResetColor();
                    Console.Write(version);
                }
                else
                    Console.Write("           ");
                Console.ForegroundColor = text_color;
                Console.SetCursorPosition(begin_pos, begin_top);
                Data_library.print_n_space(19);
                Console.SetCursorPosition(begin_pos, begin_top);
                Console.Write(all_servers_name[current_server]);
                key_pressed = Console.ReadKey(true);
            } while (!apply_action_server_or_user_selection(key_pressed, ref current_server, 10, ref search, false, ref argBool, 0, 0, null, all_servers_name));

            Console.SetCursorPosition(2, Console.BufferHeight - 2);
            Console.Write("                                  ");
            if (afficher_version)
            {
                Console.ResetColor();
                Console.SetCursorPosition(2, Console.BufferHeight - 2);
                Console.Write(version);
            }
            Console.SetCursorPosition(0, begin_top + 1);
            if (current_server >= 0)
                return (all_servers[current_server]);
            else
                return ("");
        }

        static string[] connectedServerFirst(string[] allServers, string connectedServer)
        {
            int i = 0;
            string swap = "";
            while (i < allServers.Length && allServers[i] != connectedServer)
                i++;

            if (i < allServers.Length)
            {
                swap = allServers[0];
                allServers[0] = allServers[i];
                allServers[i] = swap;
            }

            return (allServers);
        }

        static bool apply_action_server_or_user_selection(ConsoleKeyInfo key_pressed, ref int current_pos, int max_pos, ref string research, bool add_possibility, ref bool searchEnabled, int selectionPosLeft, int selectionPosTop, List<string> all_users = null, string[] all_servers = null)
        {
            int search = -1;
            if ((key_pressed.KeyChar >= 32 && key_pressed.KeyChar <= 126) || (key_pressed.KeyChar >= 128 && key_pressed.KeyChar <= 255))
            {
                research = research + key_pressed.KeyChar;
                searchEnabled = true;
            }

            switch (key_pressed.Key)
            {
                case (ConsoleKey.LeftArrow):
                    Console.SetCursorPosition(selectionPosLeft, selectionPosTop);
                        Console.Write("                        ");
                    searchEnabled = false;
                    if (current_pos == 0)
                        current_pos = max_pos;
                    else
                        current_pos--;
                    break;
                case (ConsoleKey.RightArrow):
                    Console.SetCursorPosition(selectionPosLeft, selectionPosTop);
                        Console.Write("                        ");
                    searchEnabled = false;
                    if (current_pos == max_pos)
                        current_pos = 0;
                    else
                        current_pos++;
                    break;
                case (ConsoleKey.Enter):
                    return (true);
                case (ConsoleKey.Escape):
                    current_pos = -1;
                    return (true);
                case (ConsoleKey.Backspace):
                    searchEnabled = true;
                    if (research.Length > 0)
                    {
                        research = research.Substring(0, research.Length - 1);
                        if (all_users != null)
                            search = get_search_index(research, all_users);
                        else if (all_servers != null)
                            search = get_search_index(research, all_servers);

                        if (search != -1)
                            current_pos = search;
                        else if (add_possibility)
                        {
                            if (all_users != null)
                                current_pos = all_users.Count - 1;
                            else if (all_servers != null)
                                current_pos = all_servers.Length - 1;
                        }
                    } 
                    break;
                case (ConsoleKey.Delete):
                    research = "";
                    Console.SetCursorPosition(2, Console.BufferHeight - 2);
                    Console.Write("                                  ");
                    if(all_users != null)
                    {
                        Console.SetCursorPosition(selectionPosLeft, selectionPosTop);
                        Console.Write("                        ");
                    }
                    break;
                default:
                    if (all_users != null)
                        search = get_search_index(research, all_users);
                    else if (all_servers != null)
                        search = get_search_index(research, all_servers);

                    if (search != -1)
                        current_pos = search;
                    else if (add_possibility)
                    {
                        if (all_users != null)
                            current_pos = all_users.Count - 1;
                        else if (all_servers != null)
                            current_pos = all_servers.Length - 1;
                    }
                    break;
            }

            return (false);
        }

        static int get_search_index(string start, List<string> all_users)
        {
            int i = 0;

            while (i < all_users.Count)
            {
                if (all_users[i].Length > 0 && all_users[i].Length >= start.Length)
                {
                    if (Data_library.is_same_word(all_users[i].Substring(0, start.Length), start))
                    {
                        return (i);
                    }
                }
                i++;
            }

            return (-1);
        }

        static int get_search_index(string start, string[] all_users)
        {
            int i = 0;

            while (i < all_users.Length)
            {
                if (all_users[i].Length > 0 && all_users[i].Length >= start.Length)
                {
                    if (Data_library.is_same_word(all_users[i].Substring(0, start.Length), start))
                    {
                        return (i);
                    }
                }
                i++;
            }

            return (-1);
        }

        static void signPatchFile(string file, Random rnd)
        {
            List<string> content = new List<string>();
            content = Data_library.getFileContent(file);
            content.Insert(0, Rate_Limit.hashFile(content));
            Data_library.saveFile(file, content);
        }
    }
}
