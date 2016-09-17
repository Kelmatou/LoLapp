using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLapp
{
    class Mastery
    {
        public List<double> masteriesID = new List<double>();
        public List<double> masteriesLvl = new List<double>();

        public Mastery()
        {
            masteriesID = new List<double>();
            masteriesLvl = new List<double>();
        }

        public void addMastery(double id, double lvl = 1)
        {
            masteriesID.Add(id);
            masteriesLvl.Add(lvl);
        }

        public string getMasteriesPlacement()
        {
            int ferocity = 0;
            int cunning = 0;
            int resolve = 0;

            for (int i = 0; i < masteriesID.Count; i++)
            {
                if (getMasterieColumn(masteriesID[i]) == 1)
                    ferocity += (int)masteriesLvl[i];
                else if (getMasterieColumn(masteriesID[i]) == 2)
                    cunning += (int)masteriesLvl[i];
                else if (getMasterieColumn(masteriesID[i]) == 3)
                    resolve += (int)masteriesLvl[i];
            }

            return (ferocity + "/" + cunning + "/" + resolve);
        }

        public string getKeyStoneName()
        {
            for (int i = 0; i < masteriesID.Count; i++)
            {
                switch (Convert.ToInt32(masteriesID[i]))
                {
                    case (6261):
                        return ("Grasp of the Undying");
                    case (6162):
                        return ("Fervor of Battle");
                    case (6161):
                        return ("Warlord's Bloodlust");
                    case (6164):
                        return ("Deathfire Touch");
                    case (6262):
                        return ("Strength of the Ages");
                    case (6263):
                        return ("Bond of Stone");
                    case (6362):
                        return ("Thunderlord's Decree");
                    case (6361):
                        return ("Stormraider's Surge");
                    case (6363):
                        return ("Windspeaker's Blessing");
                }
            }
            return ("No Keystone");
        }

        public string getKeyStoneDescription()
        {
            for (int i = 0; i < masteriesID.Count; i++)
            {
                switch(Convert.ToInt32(masteriesID[i]))
                {
                case (6261):
                    return ("Every 4 seconds in combat, your next attack against an enemy champion steals life equal to 2.5% of your max Health (halved for ranged champions, deals magic damage)");
                case (6162):
                    return ("You generate stacks of Fervor by hitting enemy champions with attacks and abilities. Your basic attacks deal 1-14 bonus physical damage (based on your level) to enemy champions for each of your stacks of Fervor (max 8 stacks).Damaging enemy champions with an attack generates 1 stack of Fervor (2 for melee champions) and damaging enemy champions with an ability generates 2 stacks of Fervor (2 second cooldown). Stacks of Fervor last 6 seconds.");
                case (6161):
                    return ("Gain increasingly more Life Steal based on your missing health against champions (up to 20%). Against minions gain 50% benefit (25% for ranged champions).");
                case (6164):
                    return ("Your damaging abilities cause enemy champions to take additional magic damage equal to 8 + 60% of your Bonus Attack Damage and 25% of your Ability Power, over 4 seconds. Deathfire Touch has reduced effectiveness when applied by area of effect and damage over time abilities.     - Area of Effect: 2 second duration.      - Damage over time: 1 second duration.");
                case (6262):
                    return ("Siege minions and large monsters that you or nearby allied champions kill grant 20 and 10 permanent Health, respectively (300 max). After reaching the max bonus, these kills instead restore 6% of your Maximum Health.");
                case (6263):
                    return ("+4% Damage Reduction. 6% of the damage from enemy champions taken by the nearest allied champion is dealt to you instead. Damage is not redirected if you are below 5% of your maximum health.");
                case (6362):
                    return ("Your 3rd attack or damaging spell against the same enemy champion calls down a lightning strike, dealing magic damage in the area. Damage: 10 per level, plus 30% of your Bonus Attack Damage, and 10% of your Ability Power (25-15 second cooldown, based on level).");
                case (6361):
                    return ("Dealing 30% of a champion's max Health within 2.5 seconds grants you 40% Movement Speed and 75% Slow Resistance for 3 seconds (10 second cooldown).");
                case (6363):
                    return ("Your heals and shields are 10% stronger. Additionally, your shields and heals on other allies increase their armor by 5-22 (based on level) and their magic resistance by half that amount for 3 seconds.");
                }
            }
            return ("No Keystone");
        }

        public ConsoleColor getKeyStoneColor()
        {
            switch(getKeyStoneName())
            {
                case ("Grasp of the Undying"):
                    return (Console.ForegroundColor = ConsoleColor.DarkGreen);
                case ("Fervor of Battle"):
                    return (Console.ForegroundColor = ConsoleColor.DarkRed);
                case ("Warlord's Bloodlust"):
                    return (Console.ForegroundColor = ConsoleColor.DarkRed);
                case ("Deathfire Touch"):
                    return (Console.ForegroundColor = ConsoleColor.DarkRed);
                case ("Strength of the Ages"):
                    return (Console.ForegroundColor = ConsoleColor.DarkGreen);
                case ("Bond of Stone"):
                    return (Console.ForegroundColor = ConsoleColor.DarkGreen);
                case ("Thunderlord's Decree"):
                    return (Console.ForegroundColor = ConsoleColor.DarkCyan);
                case ("Stormraider's Surge"):
                    return (Console.ForegroundColor = ConsoleColor.DarkCyan);
                case ("Windspeaker's Blessing"):
                    return (Console.ForegroundColor = ConsoleColor.DarkCyan);
            }
            return (Console.ForegroundColor);
        }

        public int getMasterieColumn(double id)
        {
            switch (Convert.ToInt32(id))
            {
                case (6121):
                    return (1);
                case (6122):
                    return (1);
                case (6261):
                    return (3);
                case (6123):
                    return (1);
                case (6141):
                    return (1);
                case (6142):
                    return (1);
                case (6162):
                    return (1);
                case (6161):
                    return (1);
                case (6164):
                    return (1);
                case (6323):
                    return (2);
                case (6342):
                    return (2);
                case (6241):
                    return (3);
                case (6242):
                    return (3);
                case (6321):
                    return (2);
                case (6322):
                    return (2);
                case (6262):
                    return (3);
                case (6263):
                    return (3);
                case (6343):
                    return (2);
                case (6352):
                    return (2);
                case (6351):
                    return (2);
                case (6212):
                    return (3);
                case (6211):
                    return (3);
                case (6111):
                    return (1);
                case (6134):
                    return (1);
                case (6114):
                    return (1);
                case (6151):
                    return (1);
                case (6131):
                    return (1);
                case (6231):
                    return (3);
                case (6154):
                    return (1);
                case (6311):
                    return (2);
                case (6232):
                    return (3);
                case (6312):
                    return (2);
                case (6331):
                    return (2);
                case (6332):
                    return (2);
                case (6251):
                    return (3);
                case (6252):
                    return (3);
                case (6362):
                    return (2);
                case (6221):
                    return (3);
                case (6361):
                    return (2);
                case (6363):
                    return (2);
                case (6223):
                    return (3);
                default:
                    return (0);
            }
        }

        public ConsoleColor getMasteryColumnColor(double id)
        {
            switch (Convert.ToInt32(id))
            {
                case (6121):
                    return (ConsoleColor.DarkRed);
                case (6122):
                    return (ConsoleColor.DarkRed);
                case (6261):
                    return (ConsoleColor.DarkGreen);
                case (6123):
                    return (ConsoleColor.DarkRed);
                case (6141):
                    return (ConsoleColor.DarkRed);
                case (6142):
                    return (ConsoleColor.DarkRed);
                case (6162):
                    return (ConsoleColor.DarkRed);
                case (6161):
                    return (ConsoleColor.DarkRed);
                case (6164):
                    return (ConsoleColor.DarkRed);
                case (6323):
                    return (ConsoleColor.DarkCyan);
                case (6342):
                    return (ConsoleColor.DarkCyan);
                case (6241):
                    return (ConsoleColor.DarkGreen);
                case (6242):
                    return (ConsoleColor.DarkGreen);
                case (6321):
                    return (ConsoleColor.DarkCyan);
                case (6322):
                    return (ConsoleColor.DarkCyan);
                case (6262):
                    return (ConsoleColor.DarkGreen);
                case (6263):
                    return (ConsoleColor.DarkGreen);
                case (6343):
                    return (ConsoleColor.DarkCyan);
                case (6352):
                    return (ConsoleColor.DarkCyan);
                case (6351):
                    return (ConsoleColor.DarkCyan);
                case (6212):
                    return (ConsoleColor.DarkGreen);
                case (6211):
                    return (ConsoleColor.DarkGreen);
                case (6111):
                    return (ConsoleColor.DarkRed);
                case (6134):
                    return (ConsoleColor.DarkRed);
                case (6114):
                    return (ConsoleColor.DarkRed);
                case (6151):
                    return (ConsoleColor.DarkRed);
                case (6131):
                    return (ConsoleColor.DarkRed);
                case (6231):
                    return (ConsoleColor.DarkGreen);
                case (6154):
                    return (ConsoleColor.DarkRed);
                case (6311):
                    return (ConsoleColor.DarkCyan);
                case (6232):
                    return (ConsoleColor.DarkGreen);
                case (6312):
                    return (ConsoleColor.DarkCyan);
                case (6331):
                    return (ConsoleColor.DarkCyan);
                case (6332):
                    return (ConsoleColor.DarkCyan);
                case (6251):
                    return (ConsoleColor.DarkGreen);
                case (6252):
                    return (ConsoleColor.DarkGreen);
                case (6362):
                    return (ConsoleColor.DarkCyan);
                case (6221):
                    return (ConsoleColor.DarkGreen);
                case (6361):
                    return (ConsoleColor.DarkCyan);
                case (6363):
                    return (ConsoleColor.DarkCyan);
                case (6223):
                    return (ConsoleColor.DarkGreen);
                default:
                    return (Console.ForegroundColor);
            }
        }

        public string getMasteryName(double idMastery)
        {
            switch (Convert.ToInt32(idMastery))
            {
                case (6121):
                    return ("Double Edged Sword");
                case (6122):
                    return ("Feast");
                case (6261):
                    return ("Grasp of the Undying");
                case (6123):
                    return ("Expose Weakness");
                case (6141):
                    return ("Bounty Hunter");
                case (6142):
                    return ("Oppressor");
                case (6162):
                    return ("Fervor of Battle");
                case (6161):
                    return ("Warlord's Bloodlust");
                case (6164):
                    return ("Deathfire Touch");
                case (6323):
                    return ("Assassin");
                case (6342):
                    return ("Bandit");
                case (6241):
                    return ("Insight");
                case (6242):
                    return ("Perseverance");
                case (6321):
                    return ("Runic Affinity");
                case (6322):
                    return ("Secret Stash");
                case (6262):
                    return ("Strength of the Ages");
                case (6263):
                    return ("Bond of Stone");
                case (6343):
                    return ("Dangerous Game");
                case (6352):
                    return ("Intelligence");
                case (6351):
                    return ("Precision");
                case (6212):
                    return ("Unyielding");
                case (6211):
                    return ("Recovery");
                case (6111):
                    return ("Fury");
                case (6134):
                    return ("Natural Talent");
                case (6114):
                    return ("Sorcery");
                case (6151):
                    return ("Battering Blows");
                case (6131):
                    return ("Vampirism");
                case (6231):
                    return ("Runic Armor");
                case (6154):
                    return ("Piercing Thoughts");
                case (6311):
                    return ("Wanderer");
                case (6232):
                    return ("Veteran's Scars");
                case (6312):
                    return ("Savagery");
                case (6331):
                    return ("Merciless");
                case (6332):
                    return ("Meditation");
                case (6251):
                    return ("Swiftness");
                case (6252):
                    return ("Legendary Guardian");
                case (6362):
                    return ("Thunderlord's Decree");
                case (6221):
                    return ("Explorer");
                case (6361):
                    return ("Stormraider's Surge");
                case (6363):
                    return ("Windspeaker's Blessing");
                case (6223):
                    return ("Tough Skin");
                default:
                    return ("Unknown");
            }
        }

        public string getMasteryDescription(double idMastery, double lvl = 1)
        {
            switch (Convert.ToInt32(idMastery))
            {
                case (6121):
                    return ("Melee: Deal 3% additional damage, take 1.5% additional damage. Ranged: Deal and take 2% additional damage");
                case (6122):
                    return ("Killing a unit restores 20 Health (30 second cooldown)");
                case (6261):
                    return ("Every 4 seconds in combat, your next attack against an enemy champion steals life equal to 2.5% of your max Health (halved for ranged champions, deals magic damage)");
                case (6123):
                    return ("Damaging enemy champions causes them to take 3% more damage from your allies");
                case (6141):
                    return ("Deal 1% increased damage for each unique enemy champion you have killed");
                case (6142):
                    return ("Deal 2.5% increased damage to targets with impaired movement (slow, stun, root, taunt, etc.)");
                case (6162):
                    return ("You generate stacks of Fervor by hitting enemy champions with attacks and abilities. Your basic attacks deal 1-14 bonus physical damage (based on your level) to enemy champions for each of your stacks of Fervor (max 8 stacks).Damaging enemy champions with an attack generates 1 stack of Fervor (2 for melee champions) and damaging enemy champions with an ability generates 2 stacks of Fervor (2 second cooldown). Stacks of Fervor last 6 seconds.");
                case (6161):
                    return ("Gain increasingly more Life Steal based on your missing health against champions (up to 20%). Against minions gain 50% benefit (25% for ranged champions).");
                case (6164):
                    return ("Your damaging abilities cause enemy champions to take additional magic damage equal to 8 + 60% of your Bonus Attack Damage and 25% of your Ability Power, over 4 seconds. Deathfire Touch has reduced effectiveness when applied by area of effect and damage over time abilities.     - Area of Effect: 2 second duration.      - Damage over time: 1 second duration.");
                case (6323):
                    return ("Deal 2% increased damage to champions when no allied champions are nearby");
                case (6342):
                    return ("Gain 1 gold for each nearby minion killed by an ally. Gain 3 gold (10 if melee) when hitting an enemy champion with a basic attack (5 second cooldown)");
                case (6241):
                    return ("Reduces the cooldown of Summoner Spells by 15%");
                case (6242):
                    return ("+50% Base Health Regen, increased to +200% when below 25% Health");
                case (6321):
                    return ("Buffs from neutral monsters last 15% longer");
                case (6322):
                    return ("Your Potions and Elixirs last 10% longer.Your Health Potions are replaced with Biscuits that restore 15 Health and Mana instantly on use");
                case (6262):
                    return ("Siege minions and large monsters that you or nearby allied champions kill grant 20 and 10 permanent Health, respectively (300 max). After reaching the max bonus, these kills instead restore 6% of your Maximum Health.");
                case (6263):
                    return ("+4% Damage Reduction. 6% of the damage from enemy champions taken by the nearest allied champion is dealt to you instead. Damage is not redirected if you are below 5% of your maximum health.");
                case (6343):
                    return ("Champion kills and assists restore 5% of your missing Health and Mana");
                case (6352):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Your Cooldown Reduction cap is increased to 41% and you gain 1% Cooldown Reduction");
                        case (2):
                            return ("Your Cooldown Reduction cap is increased to 42% and you gain 2% Cooldown Reduction");
                        case (3):
                            return ("Your Cooldown Reduction cap is increased to 43% and you gain 3% Cooldown Reduction");
                        case (4):
                            return ("Your Cooldown Reduction cap is increased to 44% and you gain 4% Cooldown Reduction");
                        case (5):
                            return ("Your Cooldown Reduction cap is increased to 45% and you gain 5% Cooldown Reduction");
                    }
                    break;
                case (6351):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Gain 0.6 + 0.06 per level Magic Penetration and Armor Penetration");
                        case (2):
                            return ("Gain 1.2 + 0.12 per level Magic Penetration and Armor Penetration");
                        case (3):
                            return ("Gain 1.8 + 0.18 per level Magic Penetration and Armor Penetration");
                        case (4):
                            return ("Gain 2.4 + 0.24 per level Magic Penetration and Armor Penetration");
                        case (5):
                            return ("Gain 3 + 0.3 per level Magic Penetration and Armor Penetration");
                    }
                    break;
                case (6212):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+1% Bonus Armor and Magic Resist");
                        case (2):
                            return ("+2% Bonus Armor and Magic Resist");
                        case (3):
                            return ("+3% Bonus Armor and Magic Resist");
                        case (4):
                            return ("+4% Bonus Armor and Magic Resist");
                        case (5):
                            return ("+5% Bonus Armor and Magic Resist");
                    }
                    break;
                case (6211):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.4 Health per 5 seconds");
                        case (2):
                            return ("+0.8 Health per 5 seconds");
                        case (3):
                            return ("+1.2 Health per 5 seconds");
                        case (4):
                            return ("+1.6 Health per 5 seconds");
                        case (5):
                            return ("+2 Health per 5 seconds");
                    }
                    break;
                case (6111):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.8% Attack Speed");
                        case (2):
                            return ("+1.6% Attack Speed");
                        case (3):
                            return ("+2.4% Attack Speed");
                        case (4):
                            return ("+3.2% Attack Speed");
                        case (5):
                            return ("+4% Attack Speed");
                    }
                    break;
                case (6134):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+2 Attack Damage and 3 Ability Power at level 18 (0.11 Attack Damage and 0.16 Ability Power per level)");
                        case (2):
                            return ("+4 Attack Damage and 6 Ability Power at level 18 (0.22 Attack Damage and 0.32 Ability Power per level)");
                        case (3):
                            return ("+6 Attack Damage and 9 Ability Power at level 18 (0.33 Attack Damage and 0.5 Ability Power per level)");
                        case (4):
                            return ("+8 Attack Damage and 12 Ability Power at level 18 (0.44 Attack Damage and 0.66 Ability Power per level)");
                        case (5):
                            return ("+10 Attack Damage and 15 Ability Power at level 18 (0.55 Attack Damage and 0.83 Ability Power per level)");
                    }
                    break;
                case (6114):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.4% increased Ability damage");
                        case (2):
                            return ("+0.8% increased Ability damage");
                        case (3):
                            return ("+1.2% increased Ability damage");
                        case (4):
                            return ("+1.6% increased Ability damage");
                        case (5):
                            return ("+2% increased Ability damage");
                    }
                    break;
                case (6151):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+1.4% Armor Penetration");
                        case (2):
                            return ("+2.8% Armor Penetration");
                        case (3):
                            return ("+4.2% Armor Penetration");
                        case (4):
                            return ("+5.6% Armor Penetration");
                        case (5):
                            return ("+7% Armor Penetration");
                    }
                    break;
                case (6131):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.4% Lifesteal and Spell Vamp");
                        case (2):
                            return ("+0.8% Lifesteal and Spell Vamp");
                        case (3):
                            return ("+1.2% Lifesteal and Spell Vamp");
                        case (4):
                            return ("+1.6% Lifesteal and Spell Vamp");
                        case (5):
                            return ("+2% Lifesteal and Spell Vamp");
                    }
                    break;

                case (6231):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Shields, healing, regeneration, and lifesteal on you are 1.6% stronger");
                        case (2):
                            return ("Shields, healing, regeneration, and lifesteal on you are 3.2% stronger");
                        case (3):
                            return ("Shields, healing, regeneration, and lifesteal on you are 4.8% stronger");
                        case (4):
                            return ("Shields, healing, regeneration, and lifesteal on you are 6.4% stronger");
                        case (5):
                            return ("Shields, healing, regeneration, and lifesteal on you are 8% stronger");
                    }
                    break;
                case (6154):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+1.4% Magic Penetration");
                        case (2):
                            return ("+2.8% Magic Penetration");
                        case (3):
                            return ("+4.2% Magic Penetration");
                        case (4):
                            return ("+5.6% Magic Penetration");
                        case (5):
                            return ("+7% Magic Penetration");
                    }
                    break;
                case (6311):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.6% Movement Speed out of combat");
                        case (2):
                            return ("+1.2% Movement Speed out of combat");
                        case (3):
                            return ("+1.8% Movement Speed out of combat");
                        case (4):
                            return ("+2.4% Movement Speed out of combat");
                        case (5):
                            return ("+3% Movement Speed out of combat");
                    }
                    break;
                case (6232):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+9 Health");
                        case (2):
                            return ("+18 Health");
                        case (3):
                            return ("+27 Health");
                        case (4):
                            return ("+36 Health");
                        case (5):
                            return ("+45 Health");
                    }
                    break;
                case (6312):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Single target attacks and spells deal 1 bonus damage to minions and monsters");
                        case (2):
                            return ("Single target attacks and spells deal 2 bonus damage to minions and monsters");
                        case (3):
                            return ("Single target attacks and spells deal 3 bonus damage to minions and monsters");
                        case (4):
                            return ("Single target attacks and spells deal 4 bonus damage to minions and monsters");
                        case (5):
                            return ("Single target attacks and spells deal 5 bonus damage to minions and monsters");
                    }
                    break;
                case (6331):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Deal 1% increased damage to champions below 40% Health");
                        case (2):
                            return ("Deal 2% increased damage to champions below 40% Health");
                        case (3):
                            return ("Deal 3% increased damage to champions below 40% Health");
                        case (4):
                            return ("Deal 4% increased damage to champions below 40% Health");
                        case (5):
                            return ("Deal 5% increased damage to champions below 40% Health");
                    }
                    break;
                case (6332):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("Regenerate 0.3% of your missing Mana every 5 seconds");
                        case (2):
                            return ("Regenerate 0.6% of your missing Mana every 5 seconds");
                        case (3):
                            return ("Regenerate 0.9% of your missing Mana every 5 seconds");
                        case (4):
                            return ("Regenerate 1.2% of your missing Mana every 5 seconds");
                        case (5):
                            return ("Regenerate 1.5% of your missing Mana every 5 seconds");
                    }
                    break;
                case (6251):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+3% Tenacity and Slow Resist");
                        case (2):
                            return ("+6% Tenacity and Slow Resist");
                        case (3):
                            return ("+9% Tenacity and Slow Resist");
                        case (4):
                            return ("+12% Tenacity and Slow Resist");
                        case (5):
                            return ("+15% Tenacity and Slow Resist");
                    }
                    break;
                case (6252):
                    switch (Convert.ToInt32(lvl))
                    {
                        case (1):
                            return ("+0.6 Armor and Magic Resist for each nearby enemy champion");
                        case (2):
                            return ("+1.2 Armor and Magic Resist for each nearby enemy champion");
                        case (3):
                            return ("+1.8 Armor and Magic Resist for each nearby enemy champion");
                        case (4):
                            return ("+2.4 Armor and Magic Resist for each nearby enemy champion");
                        case (5):
                            return ("+3 Armor and Magic Resist for each nearby enemy champion");
                    }
                    break;
                case (6362):
                    return ("Your 3rd attack or damaging spell against the same enemy champion calls down a lightning strike, dealing magic damage in the area. Damage: 10 per level, plus 30% of your Bonus Attack Damage, and 10% of your Ability Power (25-15 second cooldown, based on level).");
                case (6221):
                    return ("+15 Movement Speed in Brush and River");
                case (6361):
                    return ("Dealing 30% of a champion's max Health within 2.5 seconds grants you 40% Movement Speed and 75% Slow Resistance for 3 seconds (10 second cooldown).");
                case (6363):
                    return ("Your heals and shields are 10% stronger. Additionally, your shields and heals on other allies increase their armor by 5-22 (based on level) and their magic resistance by half that amount for 3 seconds.");
                case (6223):
                    return ("You take 2 less damage from champion and neutral monster basic attacks");
            }
            return ("Unknown");
        }
    }
}
