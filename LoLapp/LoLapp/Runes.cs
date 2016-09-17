using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoLapp
{
    class Runes
    {
        public List<double> runesID = new List<double>();
        public List<double> runesQuantity = new List<double>();

        public Runes()
        {
            runesID = new List<double>();
            runesQuantity = new List<double>();
        }

        public void addRune(double runeID, double quantity)
        {
            runesID.Add(runeID);
            runesQuantity.Add(quantity);
        }

        public string getRuneSource(double id)
        {
            switch (Convert.ToInt32(id))
            {
                case (5235):
                    return ("ability power");
                case (5234):
                    return ("% cooldowns per level");
                case (5233):
                    return ("% cooldowns");
                case (5370):
                    return ("Energy regen/5 sec per level");
                case (5230):
                    return ("health regen / 5 sec. per level");
                case (8016):
                    return ("movement speed");
                case (5374):
                    return ("Energy");
                case (8015):
                    return ("health");
                case (5373):
                    return ("Energy regen/5 sec");
                case (5372):
                    return ("Energy/level");
                case (8017):
                    return ("ability power");
                case (5371):
                    return ("Energy");
                case (8012):
                    return ("cooldowns");
                case (8011):
                    return ("ability power");
                case (8014):
                    return ("magic penetration");
                case (8013):
                    return ("armor penetration");
                case (5368):
                    return ("experience gained");
                case (5369):
                    return ("Energy regen/5 sec");
                case (8008):
                    return ("critical damage");
                case (8009):
                    return ("health");
                case (5229):
                    return ("health regen / 5 sec.");
                case (5227):
                    return ("magic resist");
                case (5228):
                    return ("magic resist per level");
                case (5225):
                    return ("armor");
                case (5226):
                    return ("armor per level");
                case (8021):
                    return ("health");
                case (8020):
                    return ("armor penetration");
                case (5221):
                    return ("armor penetration");
                case (5224):
                    return ("health per level");
                case (5223):
                    return ("health");
                case (5361):
                    return ("mana regen / 5 sec.");
                case (5360):
                    return ("mana per level");
                case (5363):
                    return ("magic penetration");
                case (5362):
                    return ("mana regen / 5 sec. per level");
                case (5365):
                    return ("movement speed");
                case (5367):
                    return ("gold / 10 sec.");
                case (8022):
                    return ("% movement speed");
                case (5366):
                    return ("% time dead");
                case (5357):
                    return ("ability power");
                case (5358):
                    return ("ability power per level");
                case (5359):
                    return ("mana");
                case (8019):
                    return ("magic penetration");
                case (5219):
                    return ("% critical chance");
                case (5214):
                    return ("attack damage per level");
                case (5215):
                    return ("% attack speed");
                case (5217):
                    return ("% critical damage");
                case (5253):
                    return ("armor penetration");
                case (5251):
                    return ("% critical chance");
                case (5257):
                    return ("armor");
                case (5256):
                    return ("health per level");
                case (5255):
                    return ("health");
                case (5356):
                    return ("% cooldowns per level");
                case (5355):
                    return ("% cooldowns");
                case (8035):
                    return ("% movement speed");
                case (5352):
                    return ("health regen / 5 sec. per level");
                case (5351):
                    return ("health regen / 5 sec.");
                case (5350):
                    return ("magic resist per level");
                case (5348):
                    return ("armor per level");
                case (5349):
                    return ("magic resist");
                case (5346):
                    return ("health per level");
                case (5347):
                    return ("armor");
                case (5249):
                    return ("% critical damage");
                case (5247):
                    return ("% attack speed");
                case (5240):
                    return ("mana regen / 5 sec. per level");
                case (5241):
                    return ("magic penetration");
                case (5243):
                    return ("% movement speed");
                case (5246):
                    return ("attack damage per level");
                case (5245):
                    return ("attack damage");
                case (5343):
                    return ("armor penetration");
                case (5345):
                    return ("health");
                case (5341):
                    return ("% critical chance");
                case (5339):
                    return ("% critical damage");
                case (5335):
                    return ("attack damage");
                case (5336):
                    return ("attack damage per level");
                case (5337):
                    return ("% attack speed");
                case (5236):
                    return ("ability power per level");
                case (5237):
                    return ("mana");
                case (5238):
                    return ("mana per level");
                case (5239):
                    return ("mana regen / 5 sec.");
                case (5129):
                    return ("% critical chance");
                case (5330):
                    return ("mana per level");
                case (5127):
                    return ("% critical damage");
                case (5331):
                    return ("mana regen / 5 sec.");
                case (5332):
                    return ("mana regen / 5 sec. per level");
                case (5400):
                    return ("Armor Penetration / +0.34 Magic Penetration");
                case (5327):
                    return ("ability power");
                case (5131):
                    return ("armor penetration");
                case (5325):
                    return ("% cooldowns");
                case (5135):
                    return ("armor");
                case (5134):
                    return ("health per level");
                case (5329):
                    return ("mana");
                case (5133):
                    return ("health");
                case (5328):
                    return ("ability power per level");
                case (5115):
                    return ("mana");
                case (5116):
                    return ("mana per level");
                case (5117):
                    return ("mana regen / 5 sec.");
                case (5118):
                    return ("mana regen / 5 sec. per level");
                case (5410):
                    return ("% Life Steal");
                case (5320):
                    return ("magic resist per level");
                case (5119):
                    return ("magic penetration");
                case (5411):
                    return ("% Life Steal");
                case (5321):
                    return ("health regen / 5 sec.");
                case (5322):
                    return ("health regen / 5 sec. per level");
                case (5409):
                    return ("% Spellvamp.");
                case (5404):
                    return ("% increased health.");
                case (5403):
                    return ("gold / 10 sec.");
                case (5402):
                    return ("Armor Penetration / +0.62 Magic Penetration");
                case (5316):
                    return ("health per level");
                case (5121):
                    return ("% movement speed");
                case (5401):
                    return ("AMP / +0.48 MP");
                case (5315):
                    return ("health");
                case (5408):
                    return ("% Spellvamp.");
                case (5318):
                    return ("armor per level");
                case (5123):
                    return ("attack damage");
                case (5407):
                    return ("% Spellvamp.");
                case (5317):
                    return ("armor");
                case (5406):
                    return ("% increased health.");
                case (5125):
                    return ("% attack speed");
                case (5405):
                    return ("% increased health.");
                case (5319):
                    return ("magic resist");
                case (5124):
                    return ("attack damage per level");
                case (5311):
                    return ("% critical chance");
                case (5108):
                    return ("health regen / 5 sec. per level");
                case (5106):
                    return ("magic resist per level");
                case (5107):
                    return ("health regen / 5 sec.");
                case (5104):
                    return ("armor per level");
                case (5105):
                    return ("magic resist");
                case (5007):
                    return ("% critical chance");
                case (5005):
                    return ("% critical damage");
                case (5213):
                    return ("attack damage");
                case (5210):
                    return ("mana regen / 5 sec. per level");
                case (5009):
                    return ("armor penetration");
                case (5206):
                    return ("ability power per level");
                case (5011):
                    return ("health");
                case (5205):
                    return ("ability power");
                case (5203):
                    return ("% cooldowns");
                case (5015):
                    return ("magic resist");
                case (5209):
                    return ("mana regen / 5 sec.");
                case (5208):
                    return ("mana per level");
                case (5013):
                    return ("armor");
                case (5207):
                    return ("mana");
                case (5012):
                    return ("health per level");
                case (5309):
                    return ("% critical damage");
                case (5114):
                    return ("ability power per level");
                case (5113):
                    return ("ability power");
                case (5307):
                    return ("% attack speed");
                case (5112):
                    return ("% cooldowns per level");
                case (5306):
                    return ("attack damage per level");
                case (5111):
                    return ("% cooldowns");
                case (5305):
                    return ("attack damage");
                case (5303):
                    return ("magic penetration");
                case (5302):
                    return ("mana regen / 5 sec. per level");
                case (5300):
                    return ("mana per level");
                case (5301):
                    return ("mana regen / 5 sec.");
                case (5200):
                    return ("health regen / 5 sec. per level");
                case (5002):
                    return ("attack damage per level");
                case (5001):
                    return ("attack damage");
                case (5003):
                    return ("% attack speed");
                case (5101):
                    return ("health");
                case (5103):
                    return ("armor");
                case (5102):
                    return ("health per level");
                case (5168):
                    return ("magic resist per level");
                case (5169):
                    return ("health regen / 5 sec.");
                case (5167):
                    return ("magic resist");
                case (5164):
                    return ("health per level");
                case (5165):
                    return ("armor");
                case (5163):
                    return ("health");
                case (5021):
                    return ("% cooldowns");
                case (5025):
                    return ("mana");
                case (5026):
                    return ("mana per level");
                case (5023):
                    return ("ability power");
                case (5024):
                    return ("ability power per level");
                case (5016):
                    return ("magic resist per level");
                case (5159):
                    return ("% critical chance");
                case (5177):
                    return ("mana");
                case (5178):
                    return ("mana per level");
                case (5179):
                    return ("mana regen / 5 sec.");
                case (5173):
                    return ("% cooldowns");
                case (5174):
                    return ("% cooldowns per level");
                case (5175):
                    return ("ability power");
                case (5176):
                    return ("ability power per level");
                case (5031):
                    return ("attack damage");
                case (5032):
                    return ("attack damage per level");
                case (5033):
                    return ("% attack speed");
                case (5035):
                    return ("% critical damage");
                case (5037):
                    return ("% critical chance");
                case (10001):
                    return ("% critical damage");
                case (5027):
                    return ("mana regen / 5 sec.");
                case (10002):
                    return ("% movement speed");
                case (5029):
                    return ("magic penetration");
                case (5418):
                    return ("Armor Penetration / +1.4 Magic Penetration");
                case (5143):
                    return ("% cooldowns");
                case (5416):
                    return ("Armor Penetration / +0.78 Magic Penetration");
                case (5417):
                    return ("Armor Penetration / +1.09 Magic Penetration");
                case (5146):
                    return ("ability power per level");
                case (5414):
                    return ("% Health.");
                case (5147):
                    return ("mana");
                case (5415):
                    return ("% Health.");
                case (5412):
                    return ("% Life Steal.");
                case (5145):
                    return ("ability power");
                case (5413):
                    return ("% Health.");
                case (5047):
                    return ("health regen / 5 sec.");
                case (5045):
                    return ("magic resist");
                case (5046):
                    return ("magic resist per level");
                case (5043):
                    return ("armor");
                case (5041):
                    return ("health");
                case (5042):
                    return ("health per level");
                case (5138):
                    return ("magic resist per level");
                case (5137):
                    return ("magic resist");
                case (5151):
                    return ("magic penetration");
                case (5153):
                    return ("attack damage");
                case (5051):
                    return ("% cooldowns");
                case (5154):
                    return ("attack damage per level");
                case (5155):
                    return ("% attack speed");
                case (5157):
                    return ("% critical damage");
                case (5056):
                    return ("mana per level");
                case (5057):
                    return ("mana regen / 5 sec.");
                case (5058):
                    return ("mana regen / 5 sec. per level");
                case (5059):
                    return ("magic penetration");
                case (5052):
                    return ("% cooldowns per level");
                case (5053):
                    return ("ability power");
                case (5054):
                    return ("ability power per level");
                case (5055):
                    return ("mana");
                case (5149):
                    return ("mana regen / 5 sec.");
                case (5148):
                    return ("mana per level");
                case (5065):
                    return ("% critical damage");
                case (5259):
                    return ("magic resist");
                case (5063):
                    return ("% attack speed");
                case (5067):
                    return ("% critical chance");
                case (5062):
                    return ("attack damage per level");
                case (5061):
                    return ("attack damage");
                case (5260):
                    return ("magic resist per level");
                case (5267):
                    return ("ability power");
                case (5268):
                    return ("ability power per level");
                case (5265):
                    return ("% cooldowns");
                case (5075):
                    return ("magic resist");
                case (5074):
                    return ("armor per level");
                case (5269):
                    return ("mana");
                case (5077):
                    return ("health regen / 5 sec.");
                case (5076):
                    return ("magic resist per level");
                case (5078):
                    return ("health regen / 5 sec. per level");
                case (5071):
                    return ("health");
                case (5073):
                    return ("armor");
                case (5072):
                    return ("health per level");
                case (5270):
                    return ("mana per level");
                case (5271):
                    return ("mana regen / 5 sec.");
                case (5273):
                    return ("magic penetration");
                case (5275):
                    return ("attack damage");
                case (5276):
                    return ("attack damage per level");
                case (5277):
                    return ("% attack speed");
                case (5279):
                    return ("% critical damage");
                case (5088):
                    return ("mana regen / 5 sec. per level");
                case (5183):
                    return ("attack damage");
                case (5087):
                    return ("mana regen / 5 sec.");
                case (5086):
                    return ("mana per level");
                case (5181):
                    return ("magic penetration");
                case (5085):
                    return ("mana");
                case (5180):
                    return ("mana regen / 5 sec. per level");
                case (5084):
                    return ("ability power per level");
                case (5187):
                    return ("% critical damage");
                case (5083):
                    return ("ability power");
                case (5185):
                    return ("% attack speed");
                case (5081):
                    return ("% cooldowns");
                case (5184):
                    return ("attack damage per level");
                case (5189):
                    return ("% critical chance");
                case (5281):
                    return ("% critical chance");
                case (5289):
                    return ("magic resist");
                case (5287):
                    return ("armor");
                case (5285):
                    return ("health");
                case (5286):
                    return ("health per level");
                case (5097):
                    return ("% critical chance");
                case (5099):
                    return ("armor penetration");
                case (5194):
                    return ("health per level");
                case (5193):
                    return ("health");
                case (5093):
                    return ("% attack speed");
                case (5196):
                    return ("armor per level");
                case (5092):
                    return ("attack damage per level");
                case (5195):
                    return ("armor");
                case (5095):
                    return ("% critical damage");
                case (5198):
                    return ("magic resist per level");
                case (5197):
                    return ("magic resist");
                case (5199):
                    return ("health regen / 5 sec.");
                case (5091):
                    return ("attack damage");
                case (5290):
                    return ("magic resist per level");
                case (5291):
                    return ("health regen / 5 sec.");
                case (8001):
                    return ("% critical damage");
                case (8002):
                    return ("% critical chance");
                case (8003):
                    return ("% cooldowns");
                case (8005):
                    return ("ability power per level");
                case (8006):
                    return ("health per level");
                case (8007):
                    return ("% attack speed");
                case (5298):
                    return ("ability power per level");
                case (5299):
                    return ("mana");
                case (5295):
                    return ("% cooldowns");
                case (5296):
                    return ("% cooldowns per level");
                case (5297):
                    return ("ability power");

            }

            return ("Unknown Rune");
        }

        public ConsoleColor getRuneUtility(string source)
        {
            switch(source)
            {
                case ("ability power"):
                    return (ConsoleColor.DarkRed);
                case ("% cooldowns per level"):
                    return (ConsoleColor.DarkCyan);
                case ("% cooldowns"):
                    return (ConsoleColor.DarkCyan);
                case ("Energy regen/5 sec per level"):
                    return (ConsoleColor.DarkYellow);
                case ("health regen / 5 sec. per level"):
                    return (ConsoleColor.DarkGreen);
                case ("movement speed"):
                    return (ConsoleColor.DarkGray);
                case ("Energy"):
                    return (ConsoleColor.DarkYellow);
                case ("health"):
                    return (ConsoleColor.DarkGreen);
                case ("Energy regen/5 sec"):
                    return (ConsoleColor.DarkYellow);
                case ("Energy/level"):
                    return (ConsoleColor.DarkYellow);
                case ("cooldowns"):
                    return (ConsoleColor.DarkCyan);
                case ("magic penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("armor penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("experience gained"):
                    return (ConsoleColor.DarkGray);
                case ("critical damage"):
                    return (ConsoleColor.DarkRed);
                case ("health regen / 5 sec."):
                    return (ConsoleColor.DarkGreen);
                case ("magic resist"):
                    return (ConsoleColor.DarkYellow);
                case ("magic resist per level"):
                    return (ConsoleColor.DarkYellow);
                case ("armor"):
                    return (ConsoleColor.DarkYellow);
                case ("armor per level"):
                    return (ConsoleColor.DarkYellow);
                case ("health per level"):
                    return (ConsoleColor.DarkGreen);
                case ("mana regen / 5 sec."):
                    return (ConsoleColor.DarkBlue);
                case ("mana per level"):
                    return (ConsoleColor.DarkBlue);
                case ("mana regen / 5 sec. per level"):
                    return (ConsoleColor.DarkBlue);
                case ("gold / 10 sec."):
                    return (ConsoleColor.DarkYellow);
                case ("% movement speed"):
                    return (ConsoleColor.DarkGray);
                case ("% time dead"):
                    return (ConsoleColor.DarkGray);
                case ("ability power per level"):
                    return (ConsoleColor.DarkRed);
                case ("mana"):
                    return (ConsoleColor.DarkBlue);
                case ("% critical chance"):
                    return (ConsoleColor.DarkRed);
                case ("attack damage per level"):
                    return (ConsoleColor.DarkRed);
                case ("% attack speed"):
                    return (ConsoleColor.DarkCyan);
                case ("% critical damage"):
                    return (ConsoleColor.DarkRed);
                case ("attack damage"):
                    return (ConsoleColor.DarkRed);
                case ("Armor Penetration / +0.34 Magic Penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("% Life Steal"):
                    return (ConsoleColor.DarkRed);
                case ("% Spellvamp."):
                    return (ConsoleColor.DarkRed);
                case ("% increased health."):
                    return (ConsoleColor.DarkGreen);
                case ("Armor Penetration / +0.62 Magic Penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("AMP / +0.48 MP"):
                    return (ConsoleColor.DarkMagenta);
                case ("Armor Penetration / +1.4 Magic Penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("Armor Penetration / +0.78 Magic Penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("Armor Penetration / +1.09 Magic Penetration"):
                    return (ConsoleColor.DarkMagenta);
                case ("% Health."):
                    return (ConsoleColor.DarkGreen);
                case ("% Life Steal."):
                    return (ConsoleColor.DarkRed);

            }
            return(Console.ForegroundColor);
        }

        public double getRuneEffect(double id)
        {
            switch(Convert.ToInt32(id))
            {
                case (5235):
                    return (3.85);
                case (5234):
                    return (0.21);
                case (5233):
                    return (1.95);
                case (5370):
                    return (0.064);
                case (5230):
                    return (0.22);
                case (8016):
                    return (1.39);
                case (5374):
                    return (5.4);
                case (8015):
                    return (24);
                case (5373):
                    return (1.575);
                case (5372):
                    return (0.161);
                case (8017):
                    return (4.56);
                case (5371):
                    return (2.2);
                case (8012):
                    return (0.75);
                case (8011):
                    return (0.66);
                case (8014):
                    return (1.85);
                case (8013):
                    return (2.37);
                case (5368):
                    return (2);
                case (5369):
                    return (0.63);
                case (8008):
                    return (2);
                case (8009):
                    return (3.56);
                case (5229):
                    return (2.1);
                case (5227):
                    return (3.11);
                case (5228):
                    return (0.29);
                case (5225):
                    return (3.32);
                case (5226):
                    return (0.29);
                case (8021):
                    return (26);
                case (8020):
                    return (2.56);
                case (5221):
                    return (1.99);
                case (5224):
                    return (2.1);
                case (5223):
                    return (20);
                case (5361):
                    return (1.25);
                case (5360):
                    return (4.17);
                case (5363):
                    return (2.01);
                case (5362):
                    return (0.24);
                case (5365):
                    return (1.5);
                case (5367):
                    return (1);
                case (8022):
                    return (1.5);
                case (5366):
                    return (5);
                case (5357):
                    return (4.95);
                case (5358):
                    return (0.43);
                case (5359):
                    return (37.5);
                case (8019):
                    return (2.01);
                case (5219):
                    return (1.44);
                case (5214):
                    return (0.19);
                case (5215):
                    return (3.51);
                case (5217):
                    return (3.47);
                case (5253):
                    return (1.28);
                case (5251):
                    return (0.93);
                case (5257):
                    return (0.91);
                case (5256):
                    return (0.54);
                case (5255):
                    return (3.47);
                case (5356):
                    return (0.28);
                case (5355):
                    return (2.5);
                case (8035):
                    return (1.5);
                case (5352):
                    return (0.28);
                case (5351):
                    return (2.7);
                case (5350):
                    return (0.37);
                case (5348):
                    return (0.38);
                case (5349):
                    return (4);
                case (5346):
                    return (2.7);
                case (5347):
                    return (4.26);
                case (5249):
                    return (2.23);
                case (5247):
                    return (1.7);
                case (5240):
                    return (0.19);
                case (5241):
                    return (1.56);
                case (5243):
                    return (1.17);
                case (5246):
                    return (0.13);
                case (5245):
                    return (0.95);
                case (5343):
                    return (2.56);
                case (5345):
                    return (26);
                case (5341):
                    return (1.86);
                case (5339):
                    return (4.46);
                case (5335):
                    return (2.25);
                case (5336):
                    return (0.25);
                case (5337):
                    return (4.5);
                case (5236):
                    return (0.34);
                case (5237):
                    return (29.17);
                case (5238):
                    return (3.24);
                case (5239):
                    return (0.97);
                case (5129):
                    return (0.72);
                case (5330):
                    return (1.17);
                case (5127):
                    return (1.74);
                case (5331):
                    return (0.41);
                case (5332):
                    return (0.065);
                case (5400):
                    return (0.5);
                case (5327):
                    return (0.59);
                case (5131):
                    return (1);
                case (5325):
                    return (0.36);
                case (5135):
                    return (0.71);
                case (5134):
                    return (0.42);
                case (5329):
                    return (6.89);
                case (5133):
                    return (2.7);
                case (5328):
                    return (0.1);
                case (5115):
                    return (20.83);
                case (5116):
                    return (2.31);
                case (5117):
                    return (0.69);
                case (5118):
                    return (0.14);
                case (5410):
                    return (0.84);
                case (5320):
                    return (0.1);
                case (5119):
                    return (1.11);
                case (5411):
                    return (1.17);
                case (5321):
                    return (0.56);
                case (5322):
                    return (0.11);
                case (5409):
                    return (2);
                case (5404):
                    return (0.84);
                case (5403):
                    return (0.25);
                case (5402):
                    return (0.9);
                case (5316):
                    return (1.33);
                case (5121):
                    return (0.83);
                case (5401):
                    return (0.7);
                case (5315):
                    return (8);
                case (5408):
                    return (1.56);
                case (5318):
                    return (0.16);
                case (5123):
                    return (0.74);
                case (5407):
                    return (1.12);
                case (5317):
                    return (1);
                case (5406):
                    return (1.5);
                case (5125):
                    return (1.32);
                case (5405):
                    return (1.17);
                case (5319):
                    return (0.74);
                case (5124):
                    return (0.1);
                case (5311):
                    return (0.42);
                case (5108):
                    return (0.16);
                case (5106):
                    return (0.21);
                case (5107):
                    return (1.5);
                case (5104):
                    return (0.21);
                case (5105):
                    return (2.22);
                case (5007):
                    return (0.52);
                case (5005):
                    return (1.24);
                case (5213):
                    return (1.75);
                case (5210):
                    return (0.05);
                case (5009):
                    return (0.72);
                case (5206):
                    return (0.08);
                case (5011):
                    return (1.93);
                case (5205):
                    return (0.46);
                case (5203):
                    return (0.29);
                case (5015):
                    return (0.43);
                case (5209):
                    return (0.32);
                case (5208):
                    return (0.91);
                case (5013):
                    return (0.51);
                case (5207):
                    return (5.36);
                case (5012):
                    return (0.3);
                case (5309):
                    return (0.78);
                case (5114):
                    return (0.24);
                case (5113):
                    return (2.75);
                case (5307):
                    return (0.76);
                case (5112):
                    return (0.15);
                case (5306):
                    return (0.06);
                case (5111):
                    return (1.4);
                case (5305):
                    return (0.43);
                case (5303):
                    return (0.63);
                case (5302):
                    return (0.06);
                case (5300):
                    return (1.42);
                case (5301):
                    return (0.33);
                case (5200):
                    return (0.09);
                case (5002):
                    return (0.08);
                case (5001):
                    return (0.53);
                case (5003):
                    return (0.94);
                case (5101):
                    return (14.5);
                case (5103):
                    return (2.37);
                case (5102):
                    return (1.5);
                case (5168):
                    return (0.13);
                case (5169):
                    return (0.21);
                case (5167):
                    return (1.04);
                case (5164):
                    return (0.42);
                case (5165):
                    return (0.55);
                case (5163):
                    return (2.08);
                case (5021):
                    return (0.11);
                case (5025):
                    return (3.28);
                case (5026):
                    return (0.65);
                case (5023):
                    return (0.33);
                case (5024):
                    return (0.06);
                case (5016):
                    return (0.04);
                case (5159):
                    return (0.22);
                case (5177):
                    return (8.75);
                case (5178):
                    return (1.1);
                case (5179):
                    return (0.26);
                case (5173):
                    return (0.67);
                case (5174):
                    return (0.07);
                case (5175):
                    return (0.92);
                case (5176):
                    return (0.13);
                case (5031):
                    return (0.16);
                case (5032):
                    return (0.02);
                case (5033):
                    return (0.35);
                case (5035):
                    return (0.31);
                case (5037):
                    return (0.15);
                case (10001):
                    return (2.23);
                case (5027):
                    return (0.15);
                case (10002):
                    return (1.5);
                case (5029):
                    return (0.49);
                case (5418):
                    return (1.79);
                case (5143):
                    return (0.16);
                case (5416):
                    return (0.99);
                case (5417):
                    return (1.39);
                case (5146):
                    return (0.08);
                case (5414):
                    return (0.39);
                case (5147):
                    return (4.59);
                case (5415):
                    return (0.5);
                case (5412):
                    return (1.5);
                case (5145):
                    return (0.46);
                case (5413):
                    return (0.28);
                case (5047):
                    return (0.15);
                case (5045):
                    return (0.74);
                case (5046):
                    return (0.09);
                case (5043):
                    return (0.39);
                case (5041):
                    return (1.49);
                case (5042):
                    return (0.3);
                case (5138):
                    return (0.06);
                case (5137):
                    return (0.6);
                case (5151):
                    return (0.68);
                case (5153):
                    return (0.22);
                case (5051):
                    return (0.47);
                case (5154):
                    return (0.03);
                case (5155):
                    return (0.5);
                case (5157):
                    return (0.43);
                case (5056):
                    return (0.79);
                case (5057):
                    return (0.19);
                case (5058):
                    return (0.04);
                case (5059):
                    return (0.35);
                case (5052):
                    return (0.05);
                case (5053):
                    return (0.66);
                case (5054):
                    return (0.1);
                case (5055):
                    return (6.25);
                case (5149):
                    return (0.2);
                case (5148):
                    return (0.91);
                case (5065):
                    return (0.43);
                case (5259):
                    return (0.77);
                case (5063):
                    return (0.42);
                case (5067):
                    return (0.23);
                case (5062):
                    return (0.03);
                case (5061):
                    return (0.24);
                case (5260):
                    return (0.07);
                case (5267):
                    return (0.59);
                case (5268):
                    return (0.1);
                case (5265):
                    return (0.2);
                case (5075):
                    return (0.41);
                case (5074):
                    return (0.09);
                case (5269):
                    return (5.91);
                case (5077):
                    return (0.31);
                case (5076):
                    return (0.05);
                case (5078):
                    return (0.06);
                case (5071):
                    return (4.48);
                case (5073):
                    return (0.56);
                case (5072):
                    return (0.75);
                case (5270):
                    return (1.17);
                case (5271):
                    return (0.26);
                case (5273):
                    return (0.87);
                case (5275):
                    return (0.28);
                case (5276):
                    return (0.04);
                case (5277):
                    return (0.64);
                case (5279):
                    return (0.56);
                case (5088):
                    return (0.036);
                case (5183):
                    return (0.33);
                case (5087):
                    return (0.23);
                case (5086):
                    return (0.65);
                case (5181):
                    return (0.49);
                case (5085):
                    return (3.83);
                case (5180):
                    return (0.05);
                case (5084):
                    return (0.06);
                case (5187):
                    return (0.61);
                case (5083):
                    return (0.33);
                case (5185):
                    return (0.59);
                case (5081):
                    return (0.2);
                case (5184):
                    return (0.05);
                case (5189):
                    return (0.32);
                case (5281):
                    return (0.28);
                case (5289):
                    return (1.34);
                case (5287):
                    return (0.7);
                case (5285):
                    return (2.67);
                case (5286):
                    return (0.54);
                case (5097):
                    return (1.03);
                case (5099):
                    return (1.42);
                case (5194):
                    return (1.04);
                case (5193):
                    return (6.24);
                case (5093):
                    return (2.52);
                case (5196):
                    return (0.13);
                case (5092):
                    return (0.14);
                case (5195):
                    return (0.78);
                case (5095):
                    return (2.48);
                case (5198):
                    return (0.08);
                case (5197):
                    return (0.58);
                case (5199):
                    return (0.43);
                case (5091):
                    return (1.25);
                case (5290):
                    return (0.16);
                case (5291):
                    return (0.27);
                case (8001):
                    return (2);
                case (8002):
                    return (0.62);
                case (8003):
                    return (0.75);
                case (8005):
                    return (0.12);
                case (8006):
                    return (0.72);
                case (8007):
                    return (1.13);
                case (5298):
                    return (0.17);
                case (5299):
                    return (11.25);
                case (5295):
                    return (0.83);
                case (5296):
                    return (0.09);
                case (5297):
                    return (1.19);

            }

            return (0);
        }

        public int getRuneCategory(double id)
        {
            /* 0 = Unknown
             * 1 = Mark
             * 2 = Glyph
             * 3 = Seal
             * 4 = Quintessence
             */
            switch (Convert.ToInt32(id))
            {
                case (5235):
                    return (4);
                case (5234):
                    return (4);
                case (5233):
                    return (4);
                case (5370):
                    return (3);
                case (5230):
                    return (4);
                case (8016):
                    return (4);
                case (5374):
                    return (4);
                case (8015):
                    return (4);
                case (5373):
                    return (4);
                case (5372):
                    return (2);
                case (8017):
                    return (4);
                case (5371):
                    return (2);
                case (8012):
                    return (2);
                case (8011):
                    return (2);
                case (8014):
                    return (4);
                case (8013):
                    return (4);
                case (5368):
                    return (4);
                case (5369):
                    return (3);
                case (8008):
                    return (1);
                case (8009):
                    return (2);
                case (5229):
                    return (4);
                case (5227):
                    return (4);
                case (5228):
                    return (4);
                case (5225):
                    return (4);
                case (5226):
                    return (4);
                case (8021):
                    return (4);
                case (8020):
                    return (4);
                case (5221):
                    return (4);
                case (5224):
                    return (4);
                case (5223):
                    return (4);
                case (5361):
                    return (4);
                case (5360):
                    return (4);
                case (5363):
                    return (4);
                case (5362):
                    return (4);
                case (5365):
                    return (4);
                case (5367):
                    return (4);
                case (8022):
                    return (4);
                case (5366):
                    return (4);
                case (5357):
                    return (4);
                case (5358):
                    return (4);
                case (5359):
                    return (4);
                case (8019):
                    return (4);
                case (5219):
                    return (4);
                case (5214):
                    return (4);
                case (5215):
                    return (4);
                case (5217):
                    return (4);
                case (5253):
                    return (1);
                case (5251):
                    return (1);
                case (5257):
                    return (1);
                case (5256):
                    return (1);
                case (5255):
                    return (1);
                case (5356):
                    return (4);
                case (5355):
                    return (4);
                case (8035):
                    return (4);
                case (5352):
                    return (4);
                case (5351):
                    return (4);
                case (5350):
                    return (4);
                case (5348):
                    return (4);
                case (5349):
                    return (4);
                case (5346):
                    return (4);
                case (5347):
                    return (4);
                case (5249):
                    return (1);
                case (5247):
                    return (1);
                case (5240):
                    return (4);
                case (5241):
                    return (4);
                case (5243):
                    return (4);
                case (5246):
                    return (1);
                case (5245):
                    return (1);
                case (5343):
                    return (4);
                case (5345):
                    return (4);
                case (5341):
                    return (4);
                case (5339):
                    return (4);
                case (5335):
                    return (4);
                case (5336):
                    return (4);
                case (5337):
                    return (4);
                case (5236):
                    return (4);
                case (5237):
                    return (4);
                case (5238):
                    return (4);
                case (5239):
                    return (4);
                case (5129):
                    return (1);
                case (5330):
                    return (3);
                case (5127):
                    return (1);
                case (5331):
                    return (3);
                case (5332):
                    return (3);
                case (5400):
                    return (1);
                case (5327):
                    return (3);
                case (5131):
                    return (1);
                case (5325):
                    return (3);
                case (5135):
                    return (1);
                case (5134):
                    return (1);
                case (5329):
                    return (3);
                case (5133):
                    return (1);
                case (5328):
                    return (3);
                case (5115):
                    return (4);
                case (5116):
                    return (4);
                case (5117):
                    return (4);
                case (5118):
                    return (4);
                case (5410):
                    return (4);
                case (5320):
                    return (3);
                case (5119):
                    return (4);
                case (5411):
                    return (4);
                case (5321):
                    return (3);
                case (5322):
                    return (3);
                case (5409):
                    return (4);
                case (5404):
                    return (4);
                case (5403):
                    return (3);
                case (5402):
                    return (1);
                case (5316):
                    return (3);
                case (5121):
                    return (4);
                case (5401):
                    return (1);
                case (5315):
                    return (3);
                case (5408):
                    return (4);
                case (5318):
                    return (3);
                case (5123):
                    return (1);
                case (5407):
                    return (4);
                case (5317):
                    return (3);
                case (5406):
                    return (4);
                case (5125):
                    return (1);
                case (5405):
                    return (4);
                case (5319):
                    return (3);
                case (5124):
                    return (1);
                case (5311):
                    return (3);
                case (5108):
                    return (4);
                case (5106):
                    return (4);
                case (5107):
                    return (4);
                case (5104):
                    return (4);
                case (5105):
                    return (4);
                case (5007):
                    return (1);
                case (5005):
                    return (1);
                case (5213):
                    return (4);
                case (5210):
                    return (3);
                case (5009):
                    return (1);
                case (5206):
                    return (3);
                case (5011):
                    return (1);
                case (5205):
                    return (3);
                case (5203):
                    return (3);
                case (5015):
                    return (1);
                case (5209):
                    return (3);
                case (5208):
                    return (3);
                case (5013):
                    return (1);
                case (5207):
                    return (3);
                case (5012):
                    return (1);
                case (5309):
                    return (3);
                case (5114):
                    return (4);
                case (5113):
                    return (4);
                case (5307):
                    return (3);
                case (5112):
                    return (4);
                case (5306):
                    return (3);
                case (5111):
                    return (4);
                case (5305):
                    return (3);
                case (5303):
                    return (2);
                case (5302):
                    return (2);
                case (5300):
                    return (2);
                case (5301):
                    return (2);
                case (5200):
                    return (3);
                case (5002):
                    return (1);
                case (5001):
                    return (1);
                case (5003):
                    return (1);
                case (5101):
                    return (4);
                case (5103):
                    return (4);
                case (5102):
                    return (4);
                case (5168):
                    return (2);
                case (5169):
                    return (2);
                case (5167):
                    return (2);
                case (5164):
                    return (2);
                case (5165):
                    return (2);
                case (5163):
                    return (2);
                case (5021):
                    return (1);
                case (5025):
                    return (1);
                case (5026):
                    return (1);
                case (5023):
                    return (1);
                case (5024):
                    return (1);
                case (5016):
                    return (1);
                case (5159):
                    return (2);
                case (5177):
                    return (2);
                case (5178):
                    return (2);
                case (5179):
                    return (2);
                case (5173):
                    return (2);
                case (5174):
                    return (2);
                case (5175):
                    return (2);
                case (5176):
                    return (2);
                case (5031):
                    return (2);
                case (5032):
                    return (2);
                case (5033):
                    return (2);
                case (5035):
                    return (2);
                case (5037):
                    return (2);
                case (10001):
                    return (1);
                case (5027):
                    return (1);
                case (10002):
                    return (4);
                case (5029):
                    return (1);
                case (5418):
                    return (4);
                case (5143):
                    return (1);
                case (5416):
                    return (4);
                case (5417):
                    return (4);
                case (5146):
                    return (1);
                case (5414):
                    return (3);
                case (5147):
                    return (1);
                case (5415):
                    return (3);
                case (5412):
                    return (4);
                case (5145):
                    return (1);
                case (5413):
                    return (3);
                case (5047):
                    return (2);
                case (5045):
                    return (2);
                case (5046):
                    return (2);
                case (5043):
                    return (2);
                case (5041):
                    return (2);
                case (5042):
                    return (2);
                case (5138):
                    return (1);
                case (5137):
                    return (1);
                case (5151):
                    return (1);
                case (5153):
                    return (2);
                case (5051):
                    return (2);
                case (5154):
                    return (2);
                case (5155):
                    return (2);
                case (5157):
                    return (2);
                case (5056):
                    return (2);
                case (5057):
                    return (2);
                case (5058):
                    return (2);
                case (5059):
                    return (2);
                case (5052):
                    return (2);
                case (5053):
                    return (2);
                case (5054):
                    return (2);
                case (5055):
                    return (2);
                case (5149):
                    return (1);
                case (5148):
                    return (1);
                case (5065):
                    return (3);
                case (5259):
                    return (1);
                case (5063):
                    return (3);
                case (5067):
                    return (3);
                case (5062):
                    return (3);
                case (5061):
                    return (3);
                case (5260):
                    return (1);
                case (5267):
                    return (1);
                case (5268):
                    return (1);
                case (5265):
                    return (1);
                case (5075):
                    return (3);
                case (5074):
                    return (3);
                case (5269):
                    return (1);
                case (5077):
                    return (3);
                case (5076):
                    return (3);
                case (5078):
                    return (3);
                case (5071):
                    return (3);
                case (5073):
                    return (3);
                case (5072):
                    return (3);
                case (5270):
                    return (1);
                case (5271):
                    return (1);
                case (5273):
                    return (1);
                case (5275):
                    return (2);
                case (5276):
                    return (2);
                case (5277):
                    return (2);
                case (5279):
                    return (2);
                case (5088):
                    return (3);
                case (5183):
                    return (3);
                case (5087):
                    return (3);
                case (5086):
                    return (3);
                case (5181):
                    return (2);
                case (5085):
                    return (3);
                case (5180):
                    return (2);
                case (5084):
                    return (3);
                case (5187):
                    return (3);
                case (5083):
                    return (3);
                case (5185):
                    return (3);
                case (5081):
                    return (3);
                case (5184):
                    return (3);
                case (5189):
                    return (3);
                case (5281):
                    return (2);
                case (5289):
                    return (2);
                case (5287):
                    return (2);
                case (5285):
                    return (2);
                case (5286):
                    return (2);
                case (5097):
                    return (4);
                case (5099):
                    return (4);
                case (5194):
                    return (3);
                case (5193):
                    return (3);
                case (5093):
                    return (4);
                case (5196):
                    return (3);
                case (5092):
                    return (4);
                case (5195):
                    return (3);
                case (5095):
                    return (4);
                case (5198):
                    return (3);
                case (5197):
                    return (3);
                case (5199):
                    return (3);
                case (5091):
                    return (4);
                case (5290):
                    return (2);
                case (5291):
                    return (2);
                case (8001):
                    return (1);
                case (8002):
                    return (1);
                case (8003):
                    return (2);
                case (8005):
                    return (2);
                case (8006):
                    return (3);
                case (8007):
                    return (1);
                case (5298):
                    return (2);
                case (5299):
                    return (2);
                case (5295):
                    return (2);
                case (5296):
                    return (2);
                case (5297):
                    return (2);

            }
            return (0);
        } //bug blue-yellow
    }
}
