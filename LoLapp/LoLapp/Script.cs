using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LoLapp
{
    public static class Script
    {
        public static string read_script_instruction(string path)
        {
            string next_instruction = "";
            if (File.Exists(path))
            {
                StreamReader script_reader = new StreamReader(path);
                next_instruction = script_reader.ReadLine();
                script_reader.Close();
            }
            return (next_instruction);
        }

        public static void delete_script_instruction(string path, ref bool script_mode)
        {
            if (File.Exists(path))
            {
                List<string> lines = new List<string>();
                StreamReader reader = new StreamReader(path);
                string current = reader.ReadLine();
                current = reader.ReadLine();
                while (current != null)
                {
                    lines.Add(current);
                    current = reader.ReadLine();
                }
                reader.Close();
                if (lines.Count > 0)
                {
                    StreamWriter writer = new StreamWriter(path);
                    for (int i = 0; i < lines.Count; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                    writer.Close();
                }
                else
                {
                    File.Delete(path);
                    script_mode = false;
                }
            }
        }

        public static string init_datapath()
        {
            char racine = 'A';
            int i = 0;

            while (i < 26 && !Directory.Exists(racine + ":/Users/" + Environment.UserName + "/AppData/Roaming/Kelmatou Apps/LoLapp/"))
            {
                i++;
                racine = (char)(racine + 1);
            }

            if (racine == i)
            {
                return ("");
            }
            else
            {
                return (racine + ":/Users/" + Environment.UserName + "/AppData/Roaming/Kelmatou Apps/LoLapp/");
            }
        }

        public static string auto_connect_userName(string profile_path)
        {
            string userName = "";
            if (File.Exists(profile_path))
            {
                StreamReader reader = new StreamReader(profile_path);
                userName = reader.ReadLine();
                reader.Close();
                if (userName == null)
                {
                    return ("");
                }
            }

            return (userName);
        }

        public static string auto_connect_userRegion(string profile_path)
        {
            string userRegion = "";
            if (File.Exists(profile_path))
            {
                StreamReader reader = new StreamReader(profile_path);
                userRegion = reader.ReadLine();
                userRegion = reader.ReadLine();
                reader.Close();
                if (userRegion == null)
                {
                    return ("");
                }
            }

            return (userRegion);
        }

        public static int convert_string_choice_to_int(string choice)
        {
            switch(choice)
            {
                case ("0"):
                    return (0);
                case ("1"):
                    return (1);
                case ("2"):
                    return (2);
                case ("3"):
                    return (3);
                case ("4"):
                    return (4);
                case ("5"):
                    return (5);
                case ("6"):
                    return (6);
                case ("7"):
                    return (7);
                case ("8"):
                    return (8);
                case ("9"):
                    return (9);
                case ("10"):
                    return (10);
                default:
                    return (-1);
            }
        }
    }
}
