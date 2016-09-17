using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Json;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace LoLapp
{
    public class Champion
    {
        public double id;
        public string name;
        public string title;
        public List<string> lore = new List<string>();
        public List<string> enemy_tips = new List<string>();
        public List<string> ally_tips = new List<string>();
        public List<string> skins = new List<string>();
        public List<string> spells = new List<string>();
        public List<string> spells_name = new List<string>();
        public List<string> spells_cd = new List<string>();
        public List<string> spells_cost = new List<string>();
        public List<string> spells_cost_type = new List<string>();
        public List<string> spells_range = new List<string>();
        public List<string> vars = new List<string>(); //coef, les resultats ne sont conservés que durant la création de l'objet
        public double attack;
        public double defense;
        public double magic;
        public double difficulty;
        public double base_HP;
        public double base_ressource;
        public double base_AD;
        public double base_AS;
        public double base_Armor;
        public double base_Mr;
        public double lvl_HP;
        public double lvl_ressource;
        public double lvl_AD;
        public double lvl_AS;
        public double lvl_Armor;
        public double lvl_Mr;
        public double atk_range;
        public double move_speed;

        public Champion(JsonObject result)
        {
            List<JsonObject> fieldtab = new List<JsonObject>();
            List<JsonObject> fieldtab2 = new List<JsonObject>();
            string s = "";
            string costburn = "";
            foreach (JsonObject field in result as JsonObjectCollection)
            {
                switch (field.Name)
                {
                    case ("id"):
                        this.id = (double)field.GetValue();
                        break;
                    case ("name"):
                        this.name = (string)field.GetValue();
                        break;
                    case ("title"):
                        this.title = (string)field.GetValue();
                        break;
                    case ("lore"):
                        this.lore.Add((string)field.GetValue());
                        lore_format();
                        break;
                    case ("allytips"):
                        ally_tips = new List<string>();
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            ally_tips.Add((string)fieldtab[i].GetValue());
                        }
                        break;
                    case ("enemytips"):
                        enemy_tips = new List<string>();
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            enemy_tips.Add((string)fieldtab[i].GetValue());
                        }
                        break;
                    case ("stats"):
                        foreach (JsonObject field2 in field as JsonObjectCollection)
                        {
                            switch (field2.Name)
                            {
                                case ("attackrange"):
                                    this.atk_range = (double)field2.GetValue();
                                    break;
                                case ("hp"):
                                    this.base_HP = (double)field2.GetValue();
                                    break;
                                case ("hpperlevel"):
                                    this.lvl_HP = (double)field2.GetValue();
                                    break;
                                case ("mp"):
                                    this.base_ressource = (double)field2.GetValue();
                                    break;
                                case ("mpperlevel"):
                                    this.lvl_ressource = (double)field2.GetValue();
                                    break;
                                case ("movespeed"):
                                    this.move_speed = (double)field2.GetValue();
                                    break;
                                case ("attackdamage"):
                                    this.base_AD = (double)field2.GetValue();
                                    break;
                                case ("attackdamageperlevel"):
                                    this.lvl_AD = (double)field2.GetValue();
                                    break;
                                case ("attackspeedoffset"):
                                    this.base_AS = Math.Round(0.625 / (1 + (double)field2.GetValue()), 3);
                                    break;
                                case ("attackspeedperlevel"):
                                    this.lvl_AS = (double)field2.GetValue();
                                    break;
                                case ("armor"):
                                    this.base_Armor = (double)field2.GetValue();
                                    break;
                                case ("armorperlevel"):
                                    this.lvl_Armor = (double)field2.GetValue();
                                    break;
                                case ("spellblock"):
                                    this.base_Mr = (double)field2.GetValue();
                                    break;
                                case ("spellblockperlevel"):
                                    this.lvl_Mr = (double)field2.GetValue();
                                    break;
                            }
                        }
                        break;
                    case ("passive"):
                        foreach (JsonObject field2 in field as JsonObjectCollection)
                        {
                            switch (field2.Name)
                            {
                                case ("description"):
                                    this.spells.Insert(0, Data_library.web_format_extractor((string)field2.GetValue()));
                                    break;
                                case ("name"):
                                    this.spells_name.Insert(0, Data_library.web_format_extractor((string)field2.GetValue()));
                                    break;
                            }
                        }
                        break;
                    case ("spells"):
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            this.spells_cost.Add("Passive");
                            costburn = "";
                            foreach (JsonObject field2 in fieldtab[i] as JsonObjectCollection)
                            {
                                switch (field2.Name)
                                {
                                    case ("description"):
                                        this.spells.Add(Data_library.web_format_extractor((string)field2.GetValue()));
                                        break;
                                    case ("name"):
                                        this.spells_name.Add(Data_library.web_format_extractor((string)field2.GetValue()));
                                        break;
                                    case ("effectBurn"):
                                        this.vars = new List<string>();
                                        fieldtab2 = (List<JsonObject>)field2.GetValue();
                                        for (int j = 0; j < fieldtab2.Count; j++)
                                        {
                                            this.vars.Add((string)fieldtab2[j].GetValue());
                                        }
                                        break;
                                    case ("cooldown"):
                                        fieldtab2 = (List<JsonObject>)field2.GetValue();
                                        s = "";
                                        for (int j = 0; j < fieldtab2.Count; j++)
                                        {
                                            if(j == 0)
                                            {
                                                s = s + fieldtab2[j];
                                            }
                                            else
                                            {
                                                s = s + "/" + fieldtab2[j];
                                            }
                                        }
                                        same_values(ref s);
                                        this.spells_cd.Add(s);
                                        break;
                                    case ("costType"):
                                        this.spells_cost_type.Add((string)field2.GetValue());
                                        break;
                                    case ("costBurn"):
                                        costburn = (string)field2.GetValue();
                                        break;
                                    case ("resource"):
                                        this.spells_cost[i] = (string)field2.GetValue();
                                        break;
                                    case ("range"):
                                        s = "";
                                        try
                                        {
                                            fieldtab2 = (List<JsonObject>)field2.GetValue();
                                            for (int j = 0; j < fieldtab2.Count; j++)
                                            {
                                                if (j == 0)
                                                {
                                                    s = s + fieldtab2[j];
                                                }
                                                else
                                                {
                                                    s = s + "/" + fieldtab2[j];
                                                }
                                            }
                                            same_values(ref s);
                                        }
                                        catch(Exception)
                                        {

                                        }
                                        this.spells_range.Add(s);
                                        break;
                                }
                            }
                            this.spells_cost[i] = cost_format(this.spells_cost[i], this.vars, costburn);
                        }
                        break;
                    case ("info"):
                        foreach (JsonObject field2 in field as JsonObjectCollection)
                        {
                            switch (field2.Name)
                            {
                                case ("attack"):
                                    this.attack = (double)field2.GetValue();
                                    break;
                                case ("defense"):
                                    this.defense = (double)field2.GetValue();
                                    break;
                                case ("magic"):
                                    this.magic = (double)field2.GetValue();
                                    break;
                                case ("difficulty"):
                                    this.difficulty = (double)field2.GetValue();
                                    break;
                            }
                        }
                        break;
                    case ("skins"):
                        skins = new List<string>();
                        fieldtab = (List<JsonObject>)field.GetValue();
                        for (int i = 0; i < fieldtab.Count; i++)
                        {
                            foreach (JsonObject field2 in (fieldtab[i]) as JsonObjectCollection)
                            {
                                switch(field2.Name)
                                {
                                    case("name"):
                                        if((string)field2.GetValue() != "default")
                                        {
                                            skins.Add((string)field2.GetValue());
                                        }
                                        break;
                                }
                            }
                        }
                        break;
                }
            }
        }

        public void display_champion_info()
        {
            Console.ResetColor();
            Data_library.print_n_space((Console.WindowWidth / 2) - (name.Length / 2));
            Console.WriteLine(name);
            Data_library.print_n_space((Console.WindowWidth / 2) - (title.Length / 2));
            Console.Write(title + "\n\n");

            int max_length_col = Data_library.max_length(new List<string>() { "HP: " + base_HP + "+" + lvl_HP + "*LvL", "Ressource: " + base_ressource + "+" + lvl_ressource + "*LvL", "Attack Range: " + atk_range, "Move Speed: " + move_speed }, 0);
            List<string> text_cut = new List<string>();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("   Attack:     ");
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Data_library.print_n_space((int)attack);
            Console.ResetColor();
            Data_library.print_n_space(11 - (int)attack);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("HP: " + base_HP + "+" + lvl_HP + "*LvL");
            Data_library.print_n_space(max_length_col - ("HP: " + base_HP + "+" + lvl_HP + "*LvL").Length + 1);
            Console.Write("AD: " + base_AD + "+" + lvl_AD + "*LvL");
            Console.Write("\n   Defense:    ");
            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Data_library.print_n_space((int)defense);
            Console.ResetColor();
            Data_library.print_n_space(11 - (int)defense);
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.Write("Ressource: " + base_ressource + "+" + lvl_ressource + "*LvL");
            Data_library.print_n_space(max_length_col - ("Ressource: " + base_ressource + "+" + lvl_ressource + "*LvL").Length + 1);
            Console.Write("Attack Speed: " + base_AS + "+" + lvl_AS + "%*LvL");
            Console.Write("\n   Magic:      ");
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Data_library.print_n_space((int)magic);
            Console.ResetColor();
            Data_library.print_n_space(11 - (int)magic);
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("Attack Range: " + atk_range);
            Data_library.print_n_space(max_length_col - ("Attack Range: " + atk_range).Length + 1);
            Console.Write("Armor: " + base_Armor + "+" + lvl_Armor + "*LvL");
            Console.Write("\n   Difficulty: ");
            Console.BackgroundColor = ConsoleColor.DarkMagenta;
            Data_library.print_n_space((int)difficulty);
            Console.ResetColor();
            Data_library.print_n_space(11 - (int)difficulty);
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Move Speed: " + move_speed);
            Data_library.print_n_space(max_length_col - ("Move Speed: " + move_speed).Length + 1);
            Console.Write("Magic Resistance: " + base_Mr + "+" + lvl_Mr + "*LvL");
            Console.Write("\n\n");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Data_library.print_n_space((Console.WindowWidth / 2) - 2);
            Console.WriteLine("Skins\n");
            for (int i = 0; i < skins.Count; i++)
            {
                if(i > 0)
                {
                    Console.Write(" -");
                }
                else
                {
                    Console.Write("  ");
                }
                if (Console.CursorLeft + skins[i].Length + 3 < Console.WindowWidth)
                {
                    Console.Write(" " + skins[i]);
                }
                else
                {
                    Console.Write("\n   " + skins[i]);
                }
            }
            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.Write("\n\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 4);
            Console.WriteLine("Abilities\n");
            for (int i = 0; i < spells.Count; i++)
            {
                text_cut = Data_library.cut_string_by_length(spells[i], Console.WindowWidth - 5);
                if (Console.BufferHeight < Console.CursorTop + text_cut.Count + 4)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + text_cut.Count + 3);
                }
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("   " + spells_name[i]);
                if(i > 0)
                {
                    Console.Write("   ");
                    Console.ForegroundColor = get_color_for_ressource(spells_cost_type[i - 1], ConsoleColor.DarkGray);
                    Console.Write(spells_cost[i - 1]);
                    Console.ResetColor();
                    Console.Write(" - ");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.Write("Cooldown: " + spells_cd[i-1] + "s");
                    Console.ResetColor();
                    Console.Write(" - ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    if(spells_range[i-1] != "" && spells_range[i-1] != "0" && spells_range[i-1] != "1")
                    {
                        
                        Console.WriteLine("Range: " + spells_range[i - 1]);
                    }
                    else
                    {
                        Console.WriteLine("Range: Self");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
                for (int j = 0; j < text_cut.Count; j++)
                {
                    Console.WriteLine("   " + text_cut[j]);
                }
                Console.WriteLine();
            }
            if (Console.BufferHeight < Console.CursorTop + 4)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 3);
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 4);
            Console.WriteLine("Ally Tips\n");
            for (int i = 0; i < ally_tips.Count; i++)
            {
                ally_tips[i] = Data_library.web_format_extractor(ally_tips[i]);
                text_cut = Data_library.cut_string_by_length(ally_tips[i], Console.WindowWidth - 7);
                if(Console.BufferHeight < Console.CursorTop + text_cut.Count + 1)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + text_cut.Count);
                }
                Console.Write("   - ");
                for(int j = 0; j < text_cut.Count; j++)
                {
                    if(j > 0)
                    {
                        Console.Write("     ");
                    }
                    Console.WriteLine(text_cut[j]);
                }
            }
            if (Console.BufferHeight < Console.CursorTop + 4)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 3);
            }
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 5);
            Console.WriteLine("Ennemy Tips\n");
            for (int i = 0; i < enemy_tips.Count; i++)
            {
                enemy_tips[i] = Data_library.web_format_extractor(enemy_tips[i]);
                text_cut = Data_library.cut_string_by_length(enemy_tips[i], Console.WindowWidth - 7);
                if (Console.BufferHeight < Console.CursorTop + text_cut.Count + 1)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + text_cut.Count);
                }
                Console.Write("   - ");
                for (int j = 0; j < text_cut.Count; j++)
                {
                    if (j > 0)
                    {
                        Console.Write("     ");
                    }
                    Console.WriteLine(text_cut[j]);
                }
            }
            if (Console.BufferHeight < Console.CursorTop + 3)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 2);
            }
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\n");
            Data_library.print_n_space((Console.WindowWidth / 2) - 2);
            Console.WriteLine("Lore");
            for (int i = 0; i < lore.Count; i++)
            {
                if (Console.BufferHeight < Console.CursorTop + 2)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 1);
                }
                Console.Write("\n");
                if(i == lore.Count - 1)
                {
                    text_cut = Data_library.cut_string_by_length(lore[i], Console.WindowWidth - 5);
                    if (Console.BufferHeight < Console.CursorTop + text_cut.Count + 2)
                    {
                        Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + text_cut.Count + 1);
                    }
                    for (int j = 0; j < text_cut.Count; j++)
                    {
                        Data_library.print_n_space((Console.WindowWidth / 2) - (text_cut[j].Length / 2));
                        Console.WriteLine(text_cut[j]);
                    }
                    Console.Write("\n");
                }
                else
                {
                    text_cut = Data_library.cut_string_by_length(lore[i], Console.WindowWidth - 5);
                    if (Console.BufferHeight < Console.CursorTop + text_cut.Count + 1)
                    {
                        Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + text_cut.Count);
                    }
                    for (int j = 0; j < text_cut.Count; j++)
                    {
                        Console.WriteLine("   " + text_cut[j]);
                    }
                }
            }

            if (Console.BufferHeight < Console.CursorTop + 3)
            {
                Console.SetBufferSize(Console.WindowWidth, Console.BufferHeight + 2);
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Data_library.print_n_space((Console.WindowWidth / 2) - 14);
            Console.Write("Press 'Enter' to continue...");
            Console.SetCursorPosition(0, 0);
            Data_library.free_waiting_keys();
            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {

            }
            Console.Clear();
            Console.SetWindowSize(84, 30);
            Console.SetBufferSize(84, 30);
        }

        private void lore_format()
        {
            List<string> new_lore_format = new List<string>();
            int start = 0;
            int i = 0;

            while(i < lore[0].Length)
            {
                if (lore[0][i] == '<')
                {
                    new_lore_format.Add(lore[0].Substring(start, i - start));
                    start += new_lore_format[new_lore_format.Count - 1].Length + 4;
                    i += 4;
                }
                else
                {
                    i++;
                }
            }
            if(start < i)
            {
                new_lore_format.Add(lore[0].Substring(start, i - start));
                if (new_lore_format[new_lore_format.Count - 1] == "-- " + name)
                {
                    new_lore_format.RemoveAt(new_lore_format.Count - 1);
                }
            }

            if(new_lore_format[new_lore_format.Count - 1] == "")
            {
                new_lore_format.RemoveAt(new_lore_format.Count - 1);
            }
            lore = new_lore_format;
        }

        private string cost_format(string ressource, List<string> e, string cost)
        {
            string result = "";
            int index = 0;
            int cur = 0;

            while(index < ressource.Length)
            {
                if(ressource[index] == '{')
                {
                    cur = index;
                    index = index + 2;
                    while(index < ressource.Length && ressource[index] != '{')
                    {
                        index++;
                    }
                    if(ressource[cur + 3] == 'c')
                    {
                        result = result + cost + ressource.Substring(cur + 10, index - cur - 10);
                    }
                    else
                    {
                        int coef = cur + 4;
                        while (coef < ressource.Length && Char.IsNumber(ressource[coef]))
                        {
                            coef++;
                        }
                        coef = Convert.ToInt32(ressource.Substring(cur + 4, coef - cur - 4));
                        if (coef < e.Count)
                        {
                            result = result + e[coef] + ressource.Substring(cur + 7 + coef.ToString().Length, index - cur - 7 - coef.ToString().Length);
                        }
                    }
                }
                else
                {
                    result = result + ressource[index];
                    index++;
                }
            }

            return (result);
        }

        private bool same_values(ref string values)
        {
            if(values == "")
            {
                return (false);
            }
            string[] tabValues = values.Split('/');
            int i = 0;
            while(i+ 1 < tabValues.Length)
            {
                if(tabValues[i] != tabValues[i+1])
                {
                    return (false);
                }
                i++;
            }
            values = tabValues[0];
            return (true);
        }

        private ConsoleColor get_color_for_ressource(string ressource, ConsoleColor defaultColor)
        {
            switch (ressource)
            {
                case ("Mana"):
                    return (ConsoleColor.DarkCyan);
                case ("10pMaxHealth,Mana"):
                    return (ConsoleColor.DarkCyan);
                case ("Manaplus@Effect3Amount@ManaPerSecond"):
                    return (ConsoleColor.DarkCyan);
                case ("Mana,@Effect2Amount@Focus"):
                    return (ConsoleColor.DarkCyan);
                case ("Health"):
                    return (ConsoleColor.DarkGreen);
                case ("pofcurrentHealth"):
                    return (ConsoleColor.DarkGreen);
                case ("pofCurrentHealth"):
                    return (ConsoleColor.DarkGreen);
                case ("@Effect1Amount*2@HealthPerSec"):
                    this.spells_cost[1] = "10/15/20/25/30 Health per Second";
                    return (ConsoleColor.DarkGreen);
                case ("FuryaSecond"):
                    return (ConsoleColor.DarkRed);
                case ("Energy"):
                    return (ConsoleColor.DarkYellow);
                case ("EssenceofShadow"):
                    return (ConsoleColor.DarkYellow);
                case ("Heat"):
                    return (ConsoleColor.DarkRed);
                case ("NoCostor50Fury"):
                    return (ConsoleColor.DarkRed);
                case ("Builds1Ferocity"):
                    return (ConsoleColor.DarkRed);
                case ("Builds5Ferocity"):
                    return (ConsoleColor.DarkRed);
                default:
                    return (defaultColor);
            }
        }
    }
}
