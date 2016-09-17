using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Rate_Limit
    {
        public List<Key> allKeys;

        public Rate_Limit()
        {
            allKeys = new List<Key>();
        }

        public void addKeys(string keyValue, string appdata_dir = "")
        {
            Random rnd = new Random();

            if(!keyIsUsed(keyValue))
            {
                allKeys.Add(new Key(keyValue));
                if (appdata_dir != "")
                {
                    List<string> content = new List<string>();
                    content = Data_library.getFileContent(appdata_dir + "keys");
                    if(content.Count > 1)
                        content.RemoveAt(0);
                    content.Add(encryptKey(keyValue, rnd));
                    content.Insert(0, hashFile(content));
                    Data_library.saveFile(appdata_dir + "keys", content);
                }
            }
        }

        public bool keyIsUsed(string key)
        {
            for (int i = 0; i < allKeys.Count; i++)
            {
                if (allKeys[i].KeyValue == key)
                    return (true);
            }
            return (false);
        }

        public static string hashFile(List<string> content)
        {
            return (Data_library.convertListToString(content).GetHashCode().ToString("X"));
        }

        public string encryptKey(string key, Random rnd)
        {
            string keyEncrypted = "";
            char randomChar = ' ';
            string encryptionKey = "lolapp";
            int step = 0;
            int length = 0;
            
            while(step < 6)
            {
                length++;
                if(length < 13 * (step + 1))
                    randomChar = (char)rnd.Next(32, 127);
                else
                {
                    if (step == 0 || step == 2)
                        randomChar = 'l';
                    else if (step == 1)
                        randomChar = 'o';
                    else if (step == 3)
                        randomChar = 'a';
                    else if (step == 4 || step == 5)
                        randomChar = 'p';
                }

                keyEncrypted += randomChar;
                if ((randomChar == 'l' || randomChar == 'L') && (step == 0 || step == 2))
                    step++;
                else if ((randomChar == 'o' || randomChar == 'O') && step == 1)
                    step++;
                else if ((randomChar == 'a' || randomChar == 'A') && step == 3)
                    step++;
                else if ((randomChar == 'p' || randomChar == 'P') && (step == 4 || step == 5))
                    step++;
            }

            for (int i = 0; i < key.Length; i++)
            {
                if(i == 8 || i == 13 || i == 18 || i == 23)
                {
                    keyEncrypted += (char)(97 + Convert.ToInt32(i + ""));
                }
                else
                {
                    randomChar = (char)(key[i] ^ encryptionKey[i % encryptionKey.Length]);
                    if(randomChar < 33 || randomChar == 27 || randomChar > 255)
                        keyEncrypted = keyEncrypted + " " + key[i];
                    else
                        keyEncrypted += (char)(key[i] ^ encryptionKey[i % encryptionKey.Length]);
                }
            }

            step = rnd.Next(24, 42); ;
            for (int i = 0; i < step; i++)
            {
                keyEncrypted += (char)rnd.Next(32, 127);
            }

            return (keyEncrypted);
        }

        public string decryptKey(string encryptedKey)
        {
            string key = "";
            string encryptionKey = "lolapp";
            int step = 0;
            int bonusIndex = 0;

            for (int i = 0; i < encryptedKey.Length && step < 6; i++)
            {
                if ((encryptedKey[i] == 'l' || encryptedKey[i] == 'L') && (step == 0 || step == 2))
                    step++;
                else if ((encryptedKey[i] == 'o' || encryptedKey[i] == 'O') && step == 1)
                    step++;
                else if ((encryptedKey[i] == 'a' || encryptedKey[i] == 'A') && step == 3)
                    step++;
                else if ((encryptedKey[i] == 'p' || encryptedKey[i] == 'P') && step == 4)
                    step++;
                else if ((encryptedKey[i] == 'p' || encryptedKey[i] == 'P') && step == 5)
                    step = i + 1;
            }

            for (int i = 0; i < 36; i++) //36 = RIOT API KEY LENGTH
            {
                if(i == 8 || i == 13 || i == 18 || i == 23)
                {
                    key += '-';
                }
                else
                {
                    
                    if(encryptedKey[i + step + bonusIndex] == ' ')
                    {
                        bonusIndex++;
                        key += encryptedKey[i + step + bonusIndex];
                    }
                    else
                    {
                        key += (char)((encryptedKey[i + step + bonusIndex]) ^ encryptionKey[i % encryptionKey.Length]);
                    }
                }
            }

            return (key);
        }

        public bool can_execute_request(ref string key_to_use, int nb_request = 1)
        {
            for (int i = 0; i < allKeys.Count; i++)
            {
                if (allKeys[i].getRequestAvailableSecond() >= nb_request && allKeys[i].getRequestAvailableMinutes() >= nb_request)
                {
                    key_to_use = allKeys[i].KeyValue;
                    return (true);
                }
            }
            return (false);
        }

        public void update()
        {
            for (int i = 0; i < allKeys.Count; i++)
            {
                while(allKeys[i].request_limit_second_key.Count > 0 && allKeys[i].request_limit_second_key[0].AddSeconds(10) < DateTime.Now)
                {
                    allKeys[i].request_limit_second_key.Remove(allKeys[i].request_limit_second_key[0]);
                }
                while (allKeys[i].request_limit_10minutes_key.Count > 0 && allKeys[i].request_limit_10minutes_key[0].AddMinutes(10) < DateTime.Now)
                {
                    allKeys[i].request_limit_10minutes_key.Remove(allKeys[i].request_limit_10minutes_key[0]);
                }
            }
        }

        public void count_new_request(string key_used, int nb_request = 1)
        {
            for (int i = 0; i < nb_request; i++)
            {
                for (int j = 0; j < allKeys.Count && nb_request > 0; j++)
                {
                    if(key_used == allKeys[j].KeyValue)
                    {
                        allKeys[j].request_limit_10minutes_key.Add(DateTime.Now);
                        allKeys[j].request_limit_second_key.Add(DateTime.Now);
                        nb_request--;
                    }
                }
            }
        }
    }
}
