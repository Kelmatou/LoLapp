using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol.client;
using agsXMPP.Collections;
using agsXMPP.protocol.iq.roster;
using System.Threading;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LoLapp
{
    public static class Chat
    {
        //command to call LoLapp in ChatOnly mode: "ChatOnly" [<LoLuser> <region> <password>] [TARGET]
        private static List<string> allFriendsName = new List<string>();
        private static List<string> allFriendsJID = new List<string>();
        private static List<string> allFriendsStatus = new List<string>();
        private static List<List<string>> allMessages = new List<List<string>>();
        private static List<List<string>> allMessagesTime = new List<List<string>>();
        private static List<string> alertList = new List<string>();
        private static bool _wait;
        private static bool printTime;
        private static bool commandCheck;
        private static bool messageReceived;
        private static int textFirstLine;
        private static bool init;

        public static void ChatSession(ref XmppClientConnection xmpp, ref LolRequester requester, List<string> allUsers, ref List<string> allLoLusers, string userRegion, string appdata_dir, string[] args = null)
        {
            #region variables
            string status = "<body><statusMsg>{0}</statusMsg></body>";
            string statusSentence = "";
            string userLoL = "";
            string userName = "";
            string JID_Sender = "";
            string Password = "";
            string server = "";
            string selfID = "";
            string receiverName = "Unknown";
            string JID_Receiver = "";
            int profileAutoconnectedCode = 0; //0 = N/A, 1 = saved, 2 = user doesn't want to save
            allFriendsName = new List<string>();
            allFriendsJID = new List<string>();
            allFriendsStatus = new List<string>();
            messageReceived = false;
            allMessages = new List<List<string>>();
            allMessagesTime = new List<List<string>>();
            printTime = false;
            init = true;
            commandCheck = true;
            alertList = new List<string>();
            #endregion

            #region windowSettings
            Console.Title = "LoLapp - Chat";
            Console.WriteLine("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 2);
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Chat\n");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            #endregion

            #region login
            if(args == null || args.Length <= 1)
            {
                Console.Write("> User name: ");
                List<string> copy = new List<string>();
                Data_library.copyList(allLoLusers, ref copy);
                userLoL = Program.summoner_selection(copy);
                if (userLoL != "")
                {
                    if (autoConnection(appdata_dir, allUsers, userLoL, ref userName, ref server, ref Password))
                    {
                        profileAutoconnectedCode = 1;
                        Console.WriteLine("> Server: " + Data_library.convert_server_to_server_name(server));
                        if (server != "")
                        {
                            Console.Write("> Password: ");
                            for (int i = 0; i < Password.Length; i++)
                                Console.Write("*");
                            Console.WriteLine();
                        }
                    }
                    else
                    {
                        if (Password == "NO")
                            profileAutoconnectedCode = 3;
                        Console.Write("> Server: ");
                        server = Program.server_selection(userRegion);
                        if (server != "")
                        {
                            Console.Write("> Password: ");
                            Password = getPassword();
                        }
                    }
                    Console.WriteLine();
                }
            }
            else if(args.Length > 3)
            {
                if (Data_library.is_valid_server(ref args[2]))
                {
                    userLoL = args[1];
                    server = args[2];
                    Password = args[3];
                    Console.WriteLine("> User name: " + userLoL);
                    Console.WriteLine("> Server: " + Data_library.convert_server_to_server_name(server));
                    Console.Write("> Password: ");
                    for (int i = 0; i < Password.Length; i++)
                        Console.Write("*");
                    Console.WriteLine();
                }
                else
                    Console.WriteLine("> Invalid server (" + args[2] + ")");
            }
            #endregion

            if (userLoL == "" || Password == "")
            {
                Console.WriteLine("> Process canceled");
                Thread.Sleep(200);
            }
            else if (server == "")
            {
                Console.WriteLine("> You server is unknown or doesn't exist.");
                Thread.Sleep(2000);
            }
            else
            {
                /*
             * Creating the Jid and the XmppClientConnection objects
             */
                #region establishingConnection
                JID_Sender = userLoL + "@pvp.net/xiff";
                Jid jidSender = new Jid(JID_Sender);
                xmpp.Server = "pvp.net";
                xmpp.ConnectServer = getChatServer(server);
                xmpp.Port = 5223;
                xmpp.Username = userLoL;
                xmpp.Password = "AIR_" + Password;
                xmpp.UseSSL = true;
                /*
                 * Open the connection
                 * and register the OnLogin event handler
                 */
                xmpp.Open(jidSender.User, xmpp.Password);
                xmpp.OnLogin += new ObjectHandler(xmpp_OnLogin);

                Console.Write("> Wait for Login ");
                int i = 0;
                _wait = true;
                do
                {
                    Console.Write(".");
                    i++;
                    if (i == 20)
                        _wait = false;
                    Thread.Sleep(500);
                } while (_wait);
                Console.WriteLine();
                #endregion

                if (!xmpp.Authenticated)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("> Authentication failed...");
                    Console.WriteLine("> Reasons may be:");
                    Console.WriteLine(">  - You entered your Summoner name, not your user name");
                    Console.WriteLine(">  - You selected a wrong server");
                    Console.WriteLine(">  - You entered a wrong password");
                    Console.WriteLine(">  - You are not connected to a network");
                    Console.WriteLine(">  - LoLapp is not allowed to use the network");
                    Console.WriteLine("> Press any key to continue");
                    Console.ReadKey(true);
                }
                else
                {
                    //sending presence to riot server (Online)
                    #region presence
                    statusSentence = "Using LoLapp Chat";
                    updateStatus(xmpp, PresenceType.available, ShowType.chat, status, statusSentence);
                    #endregion

                    #region initChat
                    //get all connected friends
                    xmpp.OnPresence += new PresenceHandler(xmpp_OnPresence);

                    selfID = xmpp.MyJID.User.Substring(3);
                    if(userName == "")
                    {
                        Summoner newProfile = requester.GetSummoner(selfID, server, false);
                        if (newProfile != null)
                            userName = newProfile.name;
                        else
                            userName = "Unknown";
                    }
                    Console.WriteLine("\n");
                    Data_library.print_n_space((Console.WindowWidth / 2) - 2);
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine("Chat\n");
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("> Now connected as " + userName + " on " + Data_library.convert_server_to_server_name(server) + "\n");
                    if(profileAutoconnectedCode == 0 && userName != "Unknown" && userName != "")
                    {
                        Console.WriteLine("> Would you like to remember credentials? (Y/N)");
                        char answer = Console.ReadKey(true).KeyChar;
                        if(answer == 'Y' || answer == 'y')
                        {
                            allLoLusers.Add(userLoL);
                            savePassword(appdata_dir, userName, server, selfID, userLoL, Password);
                            Console.WriteLine("> Credentials saved for " + userName + " on " + Data_library.convert_server_to_server_name(server));
                        }
                        else
                        {
                            savePassword(appdata_dir, userName, server, selfID, userLoL, "NO", "NO");
                            Console.WriteLine("> Credentials not saved");
                        }
                        Console.WriteLine();
                    }
                    
                    //wait until we received the list of available contacts (process started with PresenceHandler above)    
                    Thread.Sleep(1000);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    for (int j = 0; j < allFriendsJID.Count; j++)
                    {
                        addSummonerName(extractIDfromJID(allFriendsJID[j]), allUsers, server, appdata_dir, ref requester, userName);
                        allMessages.Add(new List<string>());
                        allMessagesTime.Add(new List<string>());
                        /*
                      * Catching incoming messages in
                      * the MessageCallBack
                      */
                        xmpp.MesagageGrabber.Add(new Jid(allFriendsJID[j]),
                                             new BareJidComparer(),
                                             new MessageCB(MessageCallBack),
                                             null);
                    }
                    if (allFriendsName.Count == 0)
                        Console.WriteLine("> No friend online :(");
                    else
                        displayContact();
                    Console.WriteLine();
                    #endregion

                    #region chat
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("> Start Chat (\"/exit\" to end the session)");
                    Console.WriteLine("> To see all commands available, \"/help\"\n");
                    //sending messages
                    string outMessage = "";
                    textFirstLine = Console.CursorTop;
                    do
                    {
                        if (init)
                        {
                            outMessage = "/receiver";
                            if (args.Length >= 5)
                                outMessage += " " + args[4];
                            init = !init;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            outMessage = writeText(ref xmpp, userName, allUsers, server, appdata_dir, ref requester, JID_Receiver, receiverName);
                            Console.WriteLine();
                        }

                        if (outMessage != "/exit" && isCommand(outMessage) == 0)
                        {
                            if(JID_Receiver == "")
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.WriteLine("> LoLapp: No receiver specified");
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                            }
                            else
                            {
                                if(commandCheck && outMessage.Length > 0 && outMessage[0] == '/')
                                {
                                    Data_library.free_waiting_keys();
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.WriteLine("> Command check: Press Enter to send message (else message will be erased)");
                                    if (Console.ReadKey(true).Key != ConsoleKey.Enter)
                                    {
                                        outMessage = "";
                                        Console.WriteLine("> Command check: Message not sent");
                                    }
                                    else
                                        Console.WriteLine("> Command check: Message sent");
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                }
                                if(!commandCheck || (commandCheck && outMessage != ""))
                                {
                                    List<string> messageCut = Data_library.cut_string_by_length(outMessage, 200);
                                    for (int j = 0; j < messageCut.Count; j++)
                                    {
                                        xmpp.Send(new Message(new Jid(JID_Receiver),
                                                  MessageType.chat,
                                                  messageCut[j]));
                                    }
                                }
                            }
                        }
                        else if (isCommand(outMessage) != 0)
                            commandExecution(outMessage, isCommand(outMessage), ref statusSentence, xmpp, status, ref JID_Receiver, ref receiverName, userName);
                    } while (outMessage != "/exit" && receiverName != "");
                    #endregion
                    Console.ForegroundColor = ConsoleColor.White;
                    statusSentence = "";
                    updateStatus(xmpp, PresenceType.available, ShowType.NONE, status, statusSentence);
                    xmpp.Close();
                    Console.WriteLine("> Chat connection ended");
                    Thread.Sleep(500);
                    Console.Clear();
                    Console.SetWindowSize(84, 30);
                    Console.SetBufferSize(84, 30);
                }
            }
            Data_library.free_waiting_keys();
        }

        // Is called, if the precence of a roster contact changed        
        private static void xmpp_OnPresence(object sender, Presence pres)
        {
            XmppClientConnection xmpp = (XmppClientConnection)sender;
            if (xmpp.MyJID.Bare != pres.From.Bare && !allFriendsJID.Contains(pres.From.Bare)) //add player
            {
                allFriendsJID.Add(pres.From.Bare);
                allFriendsStatus.Add(getStatusFormat(pres.Show.ToString(), pres.Type.ToString(), pres.Status));
            }
            else if (!init && xmpp.MyJID.Bare != pres.From.Bare) //update status
            {
                int index = allFriendsJID.IndexOf(pres.From.Bare);
                if ((Console.Title.Length > 16 && allFriendsName[index] == Console.Title.Substring(16) && allFriendsStatus[index] != getStatusFormat(pres.Show.ToString(), pres.Type.ToString(), pres.Status)) || alertList.Contains(allFriendsName[index])) //current contact
                {
                    if (Console.CursorTop > Console.BufferHeight - 6)
                        Console.BufferHeight += 20;
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.SetCursorPosition(0, textFirstLine);
                    Data_library.print_n_space(200);
                    Console.SetCursorPosition(0, textFirstLine);
                    Console.WriteLine("> " + (printTime ? "[" + getTime() + "] " : "") + allFriendsName[index] + " is now " + getStatusFormat(pres.Show.ToString(), pres.Type.ToString(), pres.Status));
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    messageReceived = true;
                    if(alertList.Contains(allFriendsName[index]))
                        Data_library.ShowApp(Assembly.GetExecutingAssembly().Location);
                }
                allFriendsStatus[index] = getStatusFormat(pres.Show.ToString(), pres.Type.ToString(), pres.Status);
            }
        }

        // Is raised when login and authentication is finished 
        private static void xmpp_OnLogin(object sender)
        {
            _wait = false;
            Console.WriteLine("> Logged In");
        }

        //Handles incoming messages
        private static void MessageCallBack(object sender,
                                    agsXMPP.protocol.client.Message msg,
                                    object data)
        {
            if (msg.Body != null)
            {
                string summoner = getSummonerFromJID(msg.From.Bare);
                if (Console.Title.Length > 16 && summoner == Console.Title.Substring(16)) //current contact
                {
                    if (Console.CursorTop > Console.BufferHeight - 3)
                        Console.BufferHeight += 20;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.SetCursorPosition(0, textFirstLine);
                    Data_library.print_n_space(200);
                    Console.SetCursorPosition(0, textFirstLine);
                    Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + summoner + ": ");
                    if (msg.Body.Length <= Console.WindowWidth - Console.CursorLeft - 2)
                        Console.WriteLine(msg.Body);
                    else
                    {
                        int start = Console.WindowWidth - Console.CursorLeft - 2;
                        while(start >= 0 && msg.Body[start] != ' ')
                            start--;
                        List<string> message = Data_library.cut_string_by_length(msg.Body.Substring(start + 1), Console.WindowWidth - 4);
                        Console.WriteLine(msg.Body.Substring(0, start));
                        for (int i = 0; i < message.Count; i++)
                            Console.WriteLine("  " + message[i]);
                    }
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    messageReceived = true;
                    if(alertList.Contains(summoner))
                        Data_library.ShowApp(Assembly.GetExecutingAssembly().Location);
                }
                else
                {
                    if(allFriendsJID.Contains(msg.From.Bare)) //other contact
                    {
                        int index = allFriendsJID.IndexOf(msg.From.Bare);
                        if(allMessages[index].Count == 0)
                        {
                            if (Console.CursorTop > Console.BufferHeight - 3)
                                Console.BufferHeight += 20;
                            ConsoleColor old = Console.ForegroundColor;
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.SetCursorPosition(0, textFirstLine);
                            Data_library.print_n_space(200);
                            Console.SetCursorPosition(0, textFirstLine);
                            Console.WriteLine("> " + (printTime ? "[" + getTime() + "] " : "") + getSummonerFromJID(msg.From.Bare) + ": new message");
                            Console.ForegroundColor = old;
                            messageReceived = true;
                        }
                        allMessages[index].Add(msg.Body);
                        allMessagesTime[index].Add(getTime());
                        if(alertList.Contains(allFriendsName[index]))
                            Data_library.ShowApp(Assembly.GetExecutingAssembly().Location);
                    }
                }
            }
        }

        private static void updateStatus(XmppClientConnection xmpp, PresenceType presence, ShowType show, string statusFormat, string newStatus, string serverStatus = "online")
        {
            Presence p = new Presence(ShowType.chat, serverStatus);
            p.Status = string.Format(statusFormat, newStatus);
            p.Show = show;
            p.Type = presence;
            xmpp.Send(p);
        }

        private static string writeText(ref XmppClientConnection xmpp, string userName, List<string> allusers, string region, string appdata_dir, ref LolRequester requester, string JID_Receiver, string receiver_name)
        {
            string text = "";
            int lastMinute = DateTime.Now.Minute;
            int wordLength = 0;
            List<int> lastLinePosition = new List<int>();
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            Console.CursorVisible = true;

            textFirstLine = Console.CursorTop;
            Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": ");
            do
            {
                do
                {
                    if (Console.CursorTop == Console.BufferHeight - 2)
                        Console.BufferHeight += 20;
                    if(allFriendsName.Count < allFriendsJID.Count)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        for (int i = allFriendsName.Count; i < allFriendsJID.Count; i++)
                        {
                            addSummonerName(extractIDfromJID(allFriendsJID[i]), allusers, region, appdata_dir, ref requester, userName);
                            allMessages.Add(new List<string>());
                            allMessagesTime.Add(new List<string>());
                            /*
                      * Catching incoming messages in
                      * the MessageCallBack
                      */
                            xmpp.MesagageGrabber.Add(new Jid(allFriendsJID[i]),
                                                 new BareJidComparer(),
                                                 new MessageCB(MessageCallBack),
                                                 null);
                            Console.SetCursorPosition(0, textFirstLine + i - allFriendsName.Count + 1);
                            Console.WriteLine("> " + (printTime ? "[" + getTime() + "] " : "") + allFriendsName[i] + " is now " + allFriendsStatus[i]);
                        }
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        messageReceived = true;
                    }
                    if (lastMinute != DateTime.Now.Minute && printTime)
                    {
                        messageReceived = true;
                        lastMinute = DateTime.Now.Minute;
                        Console.SetCursorPosition(0, textFirstLine);
                    }
                    if (messageReceived)
                    {
                        textFirstLine = Console.CursorTop;
                        messageReceived = false;
                        Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": ");
                        if (text.Length <= Console.WindowWidth - Console.CursorLeft - 2)//PB MESSAGES LONG SANS ESPACE
                            Console.Write(text);
                        else
                        {
                            int start = Console.WindowWidth - Console.CursorLeft - 2;
                            while (start >= 0 && text[start] != ' ')
                                start--;
                            List<string> message = Data_library.cut_string_by_length(text.Substring(start + 1), Console.WindowWidth - 4);
                            Console.WriteLine(text.Substring(0, start));
                            for (int i = 0; i < message.Count - 1; i++)
                                Console.WriteLine("  " + message[i]);
                            Console.Write("  " + message[message.Count - 1]);
                        }
                    }
                    Thread.Sleep(10);
                } while (!Console.KeyAvailable);
                key_pressed = Console.ReadKey(true);
                keyboardEntry(ref text, key_pressed, userName, region, appdata_dir, ref wordLength, ref lastLinePosition, JID_Receiver, receiver_name);
            } while ((key_pressed.Key != ConsoleKey.Enter || text == "") && key_pressed.Key != ConsoleKey.Escape && (key_pressed.Key != ConsoleKey.F4 || key_pressed.Modifiers != ConsoleModifiers.Alt) && key_pressed.Key != ConsoleKey.F1 && key_pressed.Key != ConsoleKey.F2);

            Console.CursorVisible = false;
            return (text);
        }

        private static void keyboardEntry(ref string text, ConsoleKeyInfo key_pressed, string userName, string region, string appdata_dir, ref int wordLength, ref List<int> lastLinePosition, string JID_Receiver, string receiver_name)
        {
            if (key_pressed.Key == ConsoleKey.Backspace && text.Length > 0)
            {
                if(wordLength > 0)
                    wordLength--;
                text = text.Substring(0, text.Length - 1);
                eraseChar(lastLinePosition, text);
            }
            else if (key_pressed.Key == ConsoleKey.Delete)
            {
                wordLength = 0;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": ");
            }
            else if (key_pressed.Key == ConsoleKey.Escape)
            {
                wordLength = 5;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "/exit";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
            }
            else if (key_pressed.Key == ConsoleKey.F1)
            {
                wordLength = 5;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "/help";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
            }
            else if (key_pressed.Key == ConsoleKey.F2)
            {
                wordLength = 8;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "/contact";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
            }
            else if (key_pressed.Key == ConsoleKey.Tab)
            {
                wordLength = 5;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "/msg ";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
            }
            else if(key_pressed.Key == ConsoleKey.F4 && key_pressed.Modifiers == ConsoleModifiers.Alt)
            {
                wordLength = 5;
                int nbrLine = Console.CursorTop - textFirstLine + 1;
                Console.SetCursorPosition(0, textFirstLine);
                Data_library.print_n_space(nbrLine * Console.WindowWidth);
                Console.SetCursorPosition(0, textFirstLine);
                text = "/exit";
                Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
                Data_library.saveFile(appdata_dir + "script", new List<string>() {"9"});
            }
            else if (key_pressed.Key == ConsoleKey.L && key_pressed.Modifiers == ConsoleModifiers.Control)
            {
                Program.launch_League_of_Legends(appdata_dir, false);
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else if (key_pressed.Key == ConsoleKey.N && key_pressed.Modifiers == ConsoleModifiers.Control)
            {
                ProcessStartInfo pStart = new ProcessStartInfo();
                pStart.FileName = Assembly.GetExecutingAssembly().Location;
                pStart.RedirectStandardOutput = false;
                pStart.RedirectStandardError = false;
                pStart.RedirectStandardInput = false;
                pStart.UseShellExecute = true;
                pStart.CreateNoWindow = true;
                Process p = Process.Start(pStart);
            }
            else if (key_pressed.Key == ConsoleKey.P && key_pressed.Modifiers == ConsoleModifiers.Control)
            {
                if (JID_Receiver == "")
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("> LoLapp: No receiver specified");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("> " + (printTime ? "[" + getTime() + "] " : "") + userName + ": " + text);
                }
                else
                {
                    Data_library.openProfile(receiver_name, region, appdata_dir);
                }
            }
            else if (key_pressed.Key == ConsoleKey.K && key_pressed.Modifiers == ConsoleModifiers.Control)
            {
                clearChat();
            }
            else if ((key_pressed.KeyChar >= 32 && key_pressed.KeyChar <= 126) || (key_pressed.KeyChar >= 128 && key_pressed.KeyChar <= 255))
            {
                if (key_pressed.KeyChar == ' ')
                    wordLength = 0;
                else
                    wordLength++;
                printNewChar(key_pressed.KeyChar, text, wordLength, ref lastLinePosition);
                text += key_pressed.KeyChar;
            }
        }

        private static void printNewChar(char c, string text, int wordLength, ref List<int> lastLinePosition)
        {
            if(Console.CursorLeft == Console.WindowWidth - 3)
            {
                if(wordLength > 0 && wordLength < Console.WindowWidth - 5)
                {
                    Data_library.cursorBack(wordLength - 1);
                    if (lastLinePosition.Count < Console.CursorTop - textFirstLine + 1)
                        lastLinePosition.Add(Console.CursorLeft - 1);
                    else
                        lastLinePosition[Console.CursorTop - textFirstLine] = Console.CursorLeft - 1;
                    Data_library.print_n_space(wordLength - 1);
                    Console.SetCursorPosition(2, Console.CursorTop + 1);
                    Console.Write(text.Substring(text.Length - wordLength + 1));
                }
                else
                {
                    if (lastLinePosition.Count < Console.CursorTop - textFirstLine + 1)
                        lastLinePosition.Add(Console.CursorLeft - 1);
                    else
                        lastLinePosition[Console.CursorTop - textFirstLine] = Console.CursorLeft - 1;
                    Console.SetCursorPosition(2, Console.CursorTop + 1);
                }
            }
                
            Console.Write(c);
        }

        private static void eraseChar(List<int> lastLinePosition, string text)
        {
            if (Console.CursorLeft == 2)
            {
                Console.CursorTop--;
                Console.CursorLeft = lastLinePosition[Console.CursorTop - textFirstLine];
                Console.Write(" ");
                Console.CursorLeft--;
            }
            else
            {
                Data_library.cursorBack();
                Console.Write(" ");
                Data_library.cursorBack();
                if(text.Length == 0 && Console.CursorLeft == 2)
                {
                    Console.CursorTop--;
                    Console.CursorLeft = lastLinePosition[0] + 1;
                }
            }
        }

        private static string getPassword()
        {
            string password = "";
            ConsoleKeyInfo key_pressed = new ConsoleKeyInfo();
            int beginTop = Console.CursorTop;
            int beginLeft = Console.CursorLeft;
            Console.CursorVisible = true;

            do
            {
                key_pressed = Console.ReadKey(true);
                if ((key_pressed.KeyChar >= 32 && key_pressed.KeyChar <= 126) || (key_pressed.KeyChar >= 128 && key_pressed.KeyChar <= 255))
                {
                    password += key_pressed.KeyChar;
                    Console.Write("*");
                }
                else if (key_pressed.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Data_library.cursorBack();
                    Console.Write(" ");
                    Data_library.cursorBack();
                }
                else if(key_pressed.Key == ConsoleKey.Delete)
                {
                    Console.SetCursorPosition(beginLeft, beginTop);
                    Data_library.print_n_space(password.Length);
                    Console.SetCursorPosition(beginLeft, beginTop);
                    password = "";
                }
                else if(key_pressed.Key == ConsoleKey.Escape)
                    password = "";
            } while (key_pressed.Key != ConsoleKey.Enter && key_pressed.Key != ConsoleKey.Escape);

            Console.CursorVisible = false;
            return (password);
        }

        private static string getChatServer(string region)
        {
            switch(region)
            {
                case ("euw"):
                    return "chat.euw1.lol.riotgames.com";
                case ("eune"):
                    return "chat.eun1.lol.riotgames.com";
                case ("na"):
                    return "chat.na2.lol.riotgames.com";
                case ("br"):
                    return "chat.br.lol.riotgames.com";
                case ("jp"):
                    return "chat.jp1.lol.riotgames.com";
                case("kr"):
                    return "chat.kr.lol.riotgames.com";
                case ("lan"):
                    return "chat.la1.lol.riotgames.com";
                case ("las"):
                    return "chat.la2.lol.riotgames.com";
                case ("oce"):
                    return "chat.oc1.lol.riotgames.com";
                case ("tr"):
                    return "chat.tr.lol.riotgames.com";
                case ("ru"):
                    return "chat.ru.lol.riotgames.com";
                default:
                    return "";
            }
        }

        private static void savePassword(string appdata_dir, string userName, string region, string id, string userLoL, string password, string userLoLNO = "")
        {
            userLoL = Data_library.Encrypt(userLoL, userName);
            if (userLoLNO == "")
                password = Data_library.Encrypt(password, userName);
            else
                password = Data_library.Encrypt("NO", userLoL);
            
            Data_library.createProfileFile(appdata_dir + "Profiles/" + userName, userName, region, id, userLoL, password);
        }

        private static bool autoConnection(string appdata_dir, List<string> allUsers, string userLoL, ref string userName, ref string region, ref string password)
        {
            for (int i = 0; i < allUsers.Count; i++)
            {
                if (File.Exists(appdata_dir + "Profiles/" + allUsers[i]))
                {
                    List<string> content = Data_library.getFileContent(appdata_dir + "Profiles/" + allUsers[i]);
                    if (content.Count == 5 && Data_library.Decrypt(content[3], allUsers[i]) == userLoL && content[4] != "")
                    {
                        userName = allUsers[i];
                        password = Data_library.Decrypt(content[4], content[0]);
                        region = content[1];
                        return (password != "NO");
                    }
                    else if (content.Count == 5)
                        password = Data_library.Decrypt(content[4], userLoL);
                }
            }
            return (false);
        }

        private static void addSummonerName(string id, List<string> allUsers, string region, string appdata_dir, ref LolRequester requester, string userName)
        {
            List<string> content = new List<string>();
            string name = "Unknown(" + id + ")";

            //Look at contact file
            if(File.Exists(appdata_dir + "Chat/" + userName))
            {
                content = Data_library.getFileContent(appdata_dir + "Chat/" + userName);
                for (int i = 0; i < content.Count && name == "Unknown(" + id + ")"; i++)
                {
                    string[] contactInfo = content[i].Split(':');
                    if(contactInfo.Length == 3)
                    {
                        if(contactInfo[1] == id)
                        {
                            name = contactInfo[0];
                            if(contactNeedUpdate(contactInfo[2]))
                                updateContact(userName, id, region, ref name, ref requester, appdata_dir, i);
                        }
                    }
                }
            }
            else
            {
                try
                {
                    if (!Directory.Exists(appdata_dir + "Chat"))
                        Directory.CreateDirectory(appdata_dir + "Chat");
                }
                catch(Exception)
                {
                }
            }

            //Look at LoLapp contact
            for (int i = 0; i < allUsers.Count && name == "Unknown(" + id + ")"; i++)
            {
                content = Data_library.getFileContent(appdata_dir + "Profiles/" + allUsers[i]);
                if (content.Count > 2 && content[2] == id)
                {
                    name = allUsers[i];
                    updateContact(userName, id, region, ref name, ref requester, appdata_dir);
                }  
            }

            //Request LoL serves
            if(name == "Unknown(" + id + ")")
            {
                Summoner player = requester.GetSummoner(id, region, false);
                if (player != null)
                {
                    name = player.name;
                    updateContact(userName, id, region, ref name, ref requester, appdata_dir);
                }
            }
            allFriendsName.Add(name);
        }

        private static bool contactNeedUpdate(string lastUpdate)
        {
            try
            {
                string[] jjmmaaaa = lastUpdate.Split('-');
                DateTime update = new DateTime(Convert.ToInt32(jjmmaaaa[2]), Convert.ToInt32(jjmmaaaa[1]), Convert.ToInt32(jjmmaaaa[0]));
                return (update.AddDays(15) <= DateTime.Now);
            }
            catch(Exception)
            {
            }
            return (false);
        }

        private static void updateContact(string userName, string id, string region, ref string name, ref LolRequester requester, string appdata_dir, int indexToReplace = -1)
        {
            Summoner player = requester.GetSummoner(id, region, false);
            if(player != null)
            {
                name = player.name;
                addContactToFile(userName, name, id, appdata_dir, indexToReplace);
            }
        }

        private static void addContactToFile(string userName, string name, string id, string appdata_dir, int indexToReplace = -1)
        {
            if (!Directory.Exists(appdata_dir + "Chat"))
                Directory.CreateDirectory(appdata_dir + "Chat");
            List<string> content = Data_library.getFileContent(appdata_dir + "Chat/" + userName);
            if (indexToReplace > -1 && indexToReplace < content.Count)
                content[indexToReplace] = name + ":" + id + ":" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year;
            else
                content.Add(name + ":" + id + ":" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year);
            Data_library.saveFile(appdata_dir + "Chat/" + userName, content);
        }

        private static string extractIDfromJID(string jid)
        {
            int i = 3;
            while (i < jid.Length && Char.IsNumber(jid[i]))
                i++;
            if (i < jid.Length)
                return (jid.Substring(3, i - 3));
            else
                return ("");
        }

        private static string getSummonerFromJID(string jid)
        {
            for (int i = 0; i < allFriendsJID.Count; i++)
			{
			    if(allFriendsJID[i] == jid)
                    return(allFriendsName[i]);
			}
            return("Unknown");
        }

        private static string getJIDFromSummoner(string summoner)
        {
            for (int i = 0; i < allFriendsName.Count; i++)
            {
                if (allFriendsName[i] == summoner)
                    return (allFriendsJID[i]);
            }
            return ("Unknown");
        }

        private static string getStatusFormat(string status, string type, string info)
        {
            switch(status)
            {
                case ("NONE"):
                    if (type == "unavailable")
                        return ("Disconnected");
                    else
                        return ("on Mobile");
                case ("away"):
                    return ("Away");
                case("chat"):
                    return ("Online");
                case("dnd"):
                    int indexChamp = Data_library.indexOccurence(info, "<skinname>") + 10;
                    int indexQueueType = indexChamp <= 9 ? -1 : Data_library.indexOccurence(info, "<gameQueueType>", indexChamp) + 15;
                    int indexQueueStatus = Data_library.indexOccurence(info, "<gameStatus>") + 12;
                    if (indexQueueStatus > 11)
                    {
                        string gameStatus = getGameStatusFormat(info.Substring(indexQueueStatus, Data_library.indexOccurence(info, "<", indexQueueStatus) - indexQueueStatus));
                        if (indexQueueType > 14)
                            gameStatus += " " + getGameTypeFormat(info.Substring(indexQueueType, Data_library.indexOccurence(info, "<", indexQueueType) - indexQueueType)) + " game";
                        if (indexChamp > 9 && info.Substring(indexChamp, Data_library.indexOccurence(info, "<", indexChamp) - indexChamp) != "Random")
                            gameStatus += " with " + getChampionName(info.Substring(indexChamp, Data_library.indexOccurence(info, "<", indexChamp) - indexChamp));
                        return (gameStatus);
                    }
                    return ("Busy");
                default:
                    return (status);
            }
        }

        private static string getChampionName(string champion)
        {
            string name = "" + champion[0];
            for (int i = 1; i < champion.Length; i++)
            {
                if (champion[i] >= 'A' && champion[i] <= 'Z')
                    name = name + " ";
                name = name + champion[i];
            }
            return name;
        }

        private static string getGameStatusFormat(string gameStatus)
        {
            switch(gameStatus)
            {
                case ("inGame"):
                    return ("Playing");
                case ("inQueue"):
                    return ("in Queue");
                case ("championSelect"):
                    return ("in Champion Select");
                case ("spectating"):
                    return ("Spectating");
                default:
                    return (gameStatus);
            }
        }

        private static string getGameTypeFormat(string gameType)
        {
            switch(gameType)
            {
                case ("RANKED_SOLO_5x5"):
                    return ("Ranked 5v5");
                case ("RANKED_TEAM_3x3"):
                    return ("Ranked 3v3");
                case ("RANKED_TEAM_5x5"):
                    return ("Ranked Team 5v5");
                case ("NORMAL"):
                    return ("Normal 5v5");
                case ("CAP_5x5"):
                    return ("Normal 5v5");
                case ("NORMAL_3x3"):
                    return ("Normal 3v3");
                case ("ARAM_UNRANKED_5x5"):
                    return ("Aram");
                case ("HEXAKILL"):
                    return ("Hexakill");
                case ("SR_6x6"):
                    return ("Hexakill");
                case ("ODIN_UNRANKED"):
                    return ("Dominion");
                case ("ONEFORALL_5x5"):
                    return ("One for All");
                case ("FIRSTBLOOD_1x1"):
                    return ("Snowdown 1v1");
                case ("FIRSTBLOOD_2x2"):
                    return ("Snowdown 2v2");
                case ("URF"):
                    return ("Ultra Rapid Fire");
                case ("URF_BOT"):
                    return ("Ultra Rapid Fire AI");
                case ("NIGHTMARE_BOT"):
                    return ("Doom Bots");
                case ("ASCENSION"):
                    return ("Ascension");
                case ("KING_PORO"):
                    return ("King Poro");
                case ("COUNTER_PICK"):
                    return ("Nemesis");
                case ("BILGEWATER"):
                    return ("Bilgewater");
                case ("BOT"):
                    return ("COOP vs AI 5v5");
                case ("BOT_3x3"):
                    return ("COOP vs AI 3v3");
                case ("RANKED_FLEX_SR"):
                    return ("Ranked Flex 5v5");
                case ("NONE"):
                    return ("Custom");
                default:
                    return (gameType);
            }
        }

        private static string getTime()
        {
            return ((DateTime.Now.Hour < 10 ? "0" : "") + DateTime.Now.Hour + ":" + (DateTime.Now.Minute < 10 ? "0" : "") + DateTime.Now.Minute);
        }

        private static int isCommand(string text)
        {
            if(text.Split(' ').Length > 0)
            {
                switch(text.Split(' ')[0])
                {
                    case ("/help"):
                        return (1);
                    case ("/status"):
                        return(2);
                    case ("/invisible"):
                        return (3);
                    case ("/receiver"):
                        return (4);
                    case ("/r"):
                        return (4);
                    case ("/available"):
                        return (5);
                    case ("/away"):
                        return (6);
                    case ("/time"):
                        return (7);
                    case ("/message"):
                        return (8);
                    case ("/msg"):
                        return (8);
                    case ("/contact"):
                        return (9);
                    case ("/notification"):
                        return (10);
                    case ("/notif"):
                        return (10);
                    case ("/check"):
                        return (11);
                    case ("/alert"):
                        return (12);
                    case ("/clear"):
                        return (13);
                    default:
                        return (0);
                }
            }
            return (0);
        }

        private static void commandExecution(string command, int code, ref string statusSentence, XmppClientConnection xmpp, string statusFormat, ref string JID_Receiver, ref string receiverName, string userName)
        {
            string[] commandArg = command.Split(' ');
            Console.ForegroundColor = ConsoleColor.DarkGray;
            switch(code)
            {
                case (1):
                    help();
                    break;
                case (2):
                    updateStatusCmd(commandArg, ref statusSentence, xmpp, statusFormat);
                    break;
                case (3):
                    goInvisible(xmpp, statusFormat, statusSentence);
                    break;
                case (4):
                    changeReceiver(commandArg, ref JID_Receiver, ref receiverName, userName);
                    break;
                case (5):
                    goVisible(xmpp, statusFormat, statusSentence);
                    break;
                case (6):
                    goAway(xmpp, statusFormat, statusSentence);
                    break;
                case (7):
                    printTime = !printTime;
                    break;
                case (8):
                    sendMessage(commandArg, xmpp);
                    break;
                case (9):
                    displayContact();
                    break;
                case (10):
                    displayNotif();
                    break;
                case (11):
                    commandCheck = !commandCheck;
                    if (Console.CursorTop > Console.BufferHeight - 3)
                        Console.BufferHeight += 20;
                    Console.WriteLine("> LoLapp: Command check " + (commandCheck ? "enabled" : "disabled"));
                    break;
                case (12):
                    alertManager(commandArg);
                    break;
                case (13):
                    clearChat();
                    break;
                default:
                    if (Console.CursorTop > Console.BufferHeight - 3)
                        Console.BufferHeight += 20;
                    Console.WriteLine("> LoLapp: Unknown command");
                    break;
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
        }

        //Command function

        private static void help()
        {
            if (Console.CursorTop > Console.BufferHeight - 15)
                Console.BufferHeight += 20;
            Console.WriteLine("> /alert <summoner name>    --- (enable/disable) alerts for a summoner");
            Console.WriteLine("> /available                --- set you as available");//green in LoLclient
            Console.WriteLine("> /away                     --- set you as away"); //red in LoLclient
            Console.WriteLine("> /check                    --- (enable/disable) command check");
            Console.WriteLine("> /clear                    --- clear the screen");
            Console.WriteLine("> /contact                  --- show all contact status");
            Console.WriteLine("> /exit                     --- exit Chat");
            Console.WriteLine("> /invisible                --- set your visibility as disconnected");//like disconnected in LoLclient
            Console.WriteLine("> /message <summoner> <msg> --- send a message fastly to someone (/msg)");            Console.WriteLine("> /notification             --- show all message waiting (/notif)");
            Console.WriteLine("> /receiver [summoner name] --- change your receiver (/r)");
            Console.WriteLine("> /status <new status>      --- modify you status");
            Console.WriteLine("> /time                     --- (print/stop printing) time before message");
        }

        private static void updateStatusCmd(string[] command, ref string statusSentence, XmppClientConnection xmpp, string statusFormat)
        {
            if (Console.CursorTop > Console.BufferHeight - 3)
                Console.BufferHeight += 20;
            if (command.Length > 1)
            {
                statusSentence = command[1];
                for (int i = 2; i < command.Length; i++)
                    statusSentence += " " + command[i];
                if(statusSentence.Length <= 25)
                {
                    updateStatus(xmpp, PresenceType.available, ShowType.chat, statusFormat, statusSentence);
                    Console.WriteLine("> LoLapp: Status changed");
                }
                else if (command.Length > 1)
                    Console.WriteLine("> LoLapp: Status must be less than 25 characters");
            }
            else
                Console.WriteLine("> LoLapp: Status missing");
        }

        private static void goInvisible(XmppClientConnection xmpp, string statusFormat, string statusSentence)
        {
            updateStatus(xmpp, PresenceType.unavailable, ShowType.chat, statusFormat, statusSentence);
            if (Console.CursorTop > Console.BufferHeight - 3)
                Console.BufferHeight += 20;
            Console.WriteLine("> LoLapp: connected as Invisible");
        }

        private static void goVisible(XmppClientConnection xmpp, string statusFormat, string statusSentence)
        {
            updateStatus(xmpp, PresenceType.available, ShowType.chat, statusFormat, statusSentence);
            if (Console.CursorTop > Console.BufferHeight - 3)
                Console.BufferHeight += 20;
            Console.WriteLine("> LoLapp: connected as Online");
        }

        private static void goAway(XmppClientConnection xmpp, string statusFormat, string statusSentence)
        {
            updateStatus(xmpp, PresenceType.available, ShowType.away, statusFormat, statusSentence);
            if (Console.CursorTop > Console.BufferHeight - 3)
                Console.BufferHeight += 20;
            Console.WriteLine("> LoLapp: connected as Away");
        }

        private static void changeReceiver(string[] command, ref string JID_Receiver, ref string receiverName, string userName)
        {
            List<string> allFriendsCopy = new List<string>();
            if (Console.CursorTop > Console.BufferHeight - 5)
                Console.BufferHeight += 20;
            if (command.Length > 1)
            {
                string receiverUnited = command[1];
                for (int i = 2; i < command.Length; i++)
                    receiverUnited += " " + command[i];
                if (userName != receiverUnited)
                {
                    if (allFriendsName.Contains(receiverUnited))
                    {
                        Console.Title = "LoLapp - Chat - " + receiverUnited;
                        receiverName = receiverUnited;
                        JID_Receiver = getJIDFromSummoner(receiverName) + "/xiff";
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("> LoLapp: contact changed as " + receiverName);
                        Console.WriteLine("> " + receiverName + " is " + allFriendsStatus[allFriendsName.IndexOf(receiverName)]);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        displayWaitingMessages(receiverName);
                    }
                    else
                    {
                        Console.WriteLine("> LoLapp: " + receiverUnited + " is not connected");
                    }
                }
                else
                    Console.WriteLine("> LoLapp: Receiver must be different from sender");
            }
            else if(allFriendsName.Count > 0)
            {
                int line = Console.CursorTop;
                string receiverTemp = "";
                do
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.Write("> Enter contact's name: ");
                    Data_library.copyList(allFriendsName, ref allFriendsCopy);
                    receiverTemp = Program.summoner_selection(allFriendsCopy, true, Console.ForegroundColor);
                    if (userName == receiverTemp)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(0, line);
                        Console.WriteLine("> LoLapp: Receiver must be different from sender");
                        Console.SetCursorPosition(0, line + 1);
                    }
                    else if (!allFriendsName.Contains(receiverTemp) && receiverTemp != "")
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.SetCursorPosition(0, line);
                        Console.WriteLine("> LoLapp: contact must be connected             ");
                        Console.SetCursorPosition(0, line + 1);
                    }
                } while ((userName == receiverTemp || !allFriendsName.Contains(receiverTemp)) && receiverTemp != "");
                if (receiverTemp != "")
                {
                    receiverName = receiverTemp;
                    Console.Title = "LoLapp - Chat - " + receiverName;
                    JID_Receiver = getJIDFromSummoner(receiverName) + "/xiff";
                    Console.SetCursorPosition(0, line);
                    Data_library.print_n_space(Console.WindowWidth);
                    Data_library.print_n_space(Console.WindowWidth);
                    Console.SetCursorPosition(0, line);
                    Console.WriteLine("> LoLapp: contact changed as " + receiverName);
                    Console.WriteLine("> " + receiverName + " is " + allFriendsStatus[allFriendsName.IndexOf(receiverName)]);
                    displayWaitingMessages(receiverName);
                }
            }
            else
                Console.WriteLine("> LoLapp: No contact online");
        }

        private static void displayWaitingMessages(string receiverName)
        {
            if (allFriendsName.Count > 0 && allFriendsName.Contains(receiverName))
            {
                int index = allFriendsName.IndexOf(receiverName);
                if (allMessages[index].Count > 0)
                {
                    if (Console.CursorTop > Console.BufferHeight - 2 - allMessages[index].Count)
                        Console.BufferHeight += 20;
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int i = 0; i < allMessages[index].Count; i++)
                    {
                        Console.Write("> " + (printTime ? "[" + allMessagesTime[index][i] + "] " : "") + receiverName + ": ");
                        if (allMessages[index][i].Length <= Console.WindowWidth - Console.CursorLeft - 2)
                            Console.WriteLine(allMessages[index][i]);
                        else
                        {
                            int start = Console.WindowWidth - Console.CursorLeft - 2;
                            while (start >= 0 && allMessages[index][i][start] != ' ')
                                start--;
                            List<string> message = Data_library.cut_string_by_length(allMessages[index][i].Substring(start + 1), Console.WindowWidth - 4);
                            Console.WriteLine(allMessages[index][i].Substring(0, start));
                            for (int j = 0; j < message.Count; j++)
                                Console.WriteLine("  " + message[j]);
                        }
                    }
                        
                    allMessages[index] = new List<string>();
                    allMessagesTime[index] = new List<string>();
                }
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else
                Console.WriteLine("> LoLapp: No contact online");
        }

        private static void sendMessage(string[] command, XmppClientConnection xmpp)
        {
            if (Console.CursorTop > Console.BufferHeight - 4)
                Console.BufferHeight += 20;
            if (command.Length <= 1)
                Console.WriteLine("> LoLapp: Receiver missing");
            else if (command.Length == 2)
                Console.WriteLine("> LoLapp: Message missing");
            else
            {
                string receiver = command[1];
                int i = 2;
                if(receiver.Length > 1 && receiver[0] == '"')
                {
                    receiver = receiver.Substring(1);
                    if (receiver[receiver.Length - 1] == '"')
                        receiver = receiver.Substring(0, receiver.Length - 1);
                    else
                    {
                        while (i < command.Length - 1 && command[i][command[i].Length - 1] != '"')
                        {
                            receiver += command[i];
                            i++;
                        }
                        if (i < command.Length - 1)
                        {
                            receiver += " " + command[i].Substring(0, command[i].Length - 1);
                            i++;
                        }
                    }
                }
                else
                {
                    while (!allFriendsName.Contains(receiver) && i < command.Length - 1)
                    {
                        receiver += " " + command[i];
                        i++;
                    }
                }
                if (allFriendsName.Contains(receiver))
                {
                    string message = "";
                    for (; i < command.Length; i++)
                        message += (message == "" ? "" : " ") + command[i];
                    List<string> messageCut = Data_library.cut_string_by_length(message, 200);
                    for (int j = 0; j < messageCut.Count; j++)
                    {
                        xmpp.Send(new Message(new Jid(allFriendsJID[allFriendsName.IndexOf(receiver)] + "/xiff"),
                                                  MessageType.chat,
                                                  messageCut[j]));
                    }
                }
                else
                {
                    Console.WriteLine("> LoLapp: " + receiver + " is not connected");
                    Console.WriteLine("> LoLapp: Try writing contact name like this: \"Summoner name\"");
                }
            }
        }

        private static void displayContact()
        {
            if (Console.CursorTop > Console.BufferHeight - 3 - allFriendsName.Count)
                Console.BufferHeight += 20;
            if (allFriendsName.Count > 0)
            {
                int nbr_space_name_status = Data_library.max_length(allFriendsName, 8);
                Console.Write("> Summoner");
                Data_library.print_n_space(nbr_space_name_status - 7);
                Console.WriteLine("Status");
                for (int i = 0; i < allFriendsName.Count; i++)
                {
                    Console.Write("> " + allFriendsName[i]);
                    Data_library.print_n_space(nbr_space_name_status - allFriendsName[i].Length + 1);
                    Console.WriteLine(allFriendsStatus[i]);
                }
            }
            else
                Console.WriteLine("> No contact online");
        }

        private static void displayNotif()
        {
            bool notifFound = false;
            for (int i = 0; i < allFriendsName.Count; i++)
            {
                if(allMessages[i].Count > 0)
                {
                    if (Console.CursorTop > Console.BufferHeight - allMessages[i].Count - 4)
                        Console.BufferHeight += 20;
                    notifFound = true;
                    Console.WriteLine("> " + allFriendsName[i] + ": ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    for (int j = 0; j < allMessages[i].Count && j < 3; j++)
                    {
                        Console.Write("> " + allFriendsName[i] + ": ");
                        if (allMessages[i][j].Length <= Console.WindowWidth - Console.CursorLeft - 2)
                            Console.WriteLine(allMessages[i][j]);
                        else
                        {
                            int start = Console.WindowWidth - Console.CursorLeft - 2;
                            while (start >= 0 && allMessages[i][j][start] != ' ')
                                start--;
                            List<string> message = Data_library.cut_string_by_length(allMessages[i][j].Substring(start + 1), Console.WindowWidth - 4);
                            Console.WriteLine(allMessages[i][j].Substring(0, start));
                            for (int k = 0; k < message.Count; k++)
                                Console.WriteLine("  " + message[k]);
                        }
                    }  
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    if (allMessages[i].Count > 3)
                        Console.WriteLine("> And " + (allMessages[i].Count - 3) + " other messages");
                }
            }
            if (Console.CursorTop > Console.BufferHeight - 3)
                Console.BufferHeight += 20;
            if (!notifFound)
                Console.WriteLine("> No unread message found");
            else
                Console.WriteLine("> End of notifications");
        }

        private static void alertManager(string [] command)
        {
            if (command.Length <= 1)
                Console.WriteLine("> LoLapp: Summoner missing");
            else
            {
                string summoner = command[1];
                for (int i = 2; i < command.Length; i++)
                    summoner += " " + command[i];
                if (alertList.Contains(summoner))
                {
                    alertList.Remove(summoner);
                    Console.WriteLine("> LoLapp: Alerts for " + summoner + " disabled");
                } 
                else
                {
                    alertList.Add(summoner);
                    Console.WriteLine("> LoLapp: Alerts for " + summoner + " enabled");
                }   
            }
        }

        private static void clearChat()
        {
            int curLeft = Console.CursorLeft;
            int curTop = Console.CursorTop;
            while (Console.CursorTop > Console.BufferHeight - Console.WindowHeight - 1)
                Console.BufferHeight++;
            for (int i = 0; i < Console.WindowHeight; i++)
                Console.WriteLine();
            Console.SetCursorPosition(curLeft, curTop);
        }
    }
}
