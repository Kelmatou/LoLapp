﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Key
    {
        public string KeyValue;
        public List<DateTime> request_limit_second_key;
        public List<DateTime> request_limit_10minutes_key;

        public Key(string key)
        {
            KeyValue = key;
            request_limit_10minutes_key = new List<DateTime>();
            request_limit_second_key = new List<DateTime>();
        }

        public int getRequestAvailableSecond()
        {
            return (8 - request_limit_second_key.Count());
        }

        public int getRequestAvailableMinutes()
        {
            return (500 - request_limit_10minutes_key.Count());
        }

        public static bool isKeyFormat(string key)
        {
            if (key.Length != 36)
                return (false);

            for (int i = 0; i < 36; i++)
            {
                if (i == 8 || i == 13 || i == 18 || i == 23)
                {
                    if (key[i] != '-')
                        return (false);
                }
                else if (!Char.IsNumber(key[i]) && (key[i] < 97 || key[i] > 102))
                    return (false);
            }

            return (true);
        }
    }
}
