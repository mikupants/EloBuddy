using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using EloBuddy;
using EloBuddy.SDK.Events;
using System;
using Color = System.Drawing.Color;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using BigFatHUD.Properties;
using System.Diagnostics;

namespace BigFatHUD
{
    internal class Menus
    {
        private const string othersetting = "othersettingid";
        private const string colorsetting = "colorsettingid";
        private const string generalsetting = "generalsettingid";
        private static Menu HUDMenu;
        private static Menu GeneralMenu;
        private static Menu ColorMenu;
        private static Menu Others;
        private static ColorSlide HighHPBarSlide;
        private static ColorSlide LowHPBarSlide;
        private static ColorSlide ManBarSlide;
        private static ColorSlide DamageBarSlide;
        private static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }
        private static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }
        private static int getBoxItem(Menu m, string item)
        {
            return m[item].Cast<ComboBox>().CurrentValue;
        }
        public static bool HUDEnable
        {
            get { return ShopHider? getCheckBoxItem(GeneralMenu, "enableHUD") && !Shop.IsOpen : getCheckBoxItem(GeneralMenu, "enableHUD"); }
        }
        public static bool HUD2Enable
        {
            get { return getCheckBoxItem(GeneralMenu, "enableHUD2"); }
        }
        public static float HUDScale
        {
            get { return getSliderItem(GeneralMenu, "scale") / 100f; }
        }

        public static int HUDX
        {
            get { return getSliderItem(GeneralMenu, "HUD.x"); }
        }

        public static int HUDY
        {
            get { return getSliderItem(GeneralMenu, "HUD.y"); }
        }
        public static int CDX
        {
            get { return getSliderItem(GeneralMenu, "HUD.CDx"); }
        }

        public static int CDY
        {
            get { return getSliderItem(GeneralMenu, "HUD.CDy"); }
        }
        public static int HPDivide
        {
            get { return getSliderItem(GeneralMenu, "HPref"); }
        }
        public static SharpDX.Color HighHPBarColor
        {
            get { return HighHPBarSlide.GetSharpColor(); }
        }
        public static SharpDX.Color LowHPBarColor
        {
            get { return LowHPBarSlide.GetSharpColor(); }
        }
        public static SharpDX.Color ManaBarColor
        {
            get { return ManBarSlide.GetSharpColor(); }
        }
        public static SharpDX.Color DamgeBarColor
        {
            get { return DamageBarSlide.GetSharpColor(); }
        }
        public static bool TeamTimer
        {
            get { return getCheckBoxItem(Others, "enableTeamtimer"); }
        }
        public static bool UsePosition //for all
        {
            get { return getCheckBoxItem(Others, "usepostion"); }
        }
        public static bool ShopHider
        {
            get { return getCheckBoxItem(GeneralMenu, "shophider"); }
        }
        public static bool EnableDamageAPI
        {
            get { return getCheckBoxItem(Others, "enableDamgeAPI"); }
        }
        public static int Position(AIHeroClient hero)
        {
            return getBoxItem(Others, "posof" + hero.NetworkId + hero.ChampionName);
        }
        public static bool DontUsePositionFor(AIHeroClient hero)
        {
            return getCheckBoxItem(Others, "dontusepostionfor" + +hero.NetworkId + hero.ChampionName);
        }
        public static int DeathRate
        {
            get { return getSliderItem(Others, "DeathRate"); }
        }
        public static void CreateMenu()
        {
            HUDMenu = MainMenu.AddMenu("Big Fat HUD", "bigfathud");
            GeneralMenu = HUDMenu.AddSubMenu("General Setting", generalsetting);
            GeneralMenu.Add("enableHUD", new CheckBox("Enable"));
            GeneralMenu.Add("shophider", new CheckBox("Hide if Shop is open"));
            GeneralMenu.Add("enableHUD2", new CheckBox("Use Vertical Version(F5 to Reload)", false));
            GeneralMenu.Add("scale", new Slider("HUD Scale % (F5 to Reload)", 100, 80));
            GeneralMenu.Add("HUD.x", new Slider("X", 0, 0, 2000));
            GeneralMenu.Add("HUD.y", new Slider("Y", 0, 0, 2000));
            GeneralMenu.Add("HUD.CDx", new Slider("CD positon offset X", 0, -10, 10));
            GeneralMenu.Add("HUD.CDy", new Slider("CD positon offset Y", 0, -10, 10));
            ColorMenu = HUDMenu.AddSubMenu("Color Setting", colorsetting);
            HighHPBarSlide = new ColorSlide(ColorMenu, "highColor", Color.Green, "High-HP Bar Color:");
            ColorMenu.Add("HPref", new Slider("HP divides at:", 20, 1, 40));
            LowHPBarSlide = new ColorSlide(ColorMenu, "lowColor", Color.DarkRed, "Low-HP Bar Color:");
            ManBarSlide = new ColorSlide(ColorMenu, "manaColor", Color.AliceBlue, "Mana Bar Color:");
            Others = HUDMenu.AddSubMenu("Other Setting", othersetting);
            Others.Add("enableTeamtimer", new CheckBox("Enable Team Timer"));
            Others.AddLabel("Double left click to inform allies about enemy CDs!");
            Others.AddLabel("Example:");
            Others.AddLabel(" - if enemy has spell A, will send: #enemy has #A");
            Others.AddLabel(" - if spell A is in CD, will send: #enemy #A #time");
            Others.AddLabel("#enemy is short name of enemy; #A is R/Flash/Ignite/...; #time is the time it will cool down");
            Others.AddSeparator();
            Others.Add("usepostion", new CheckBox("Team Timer: Use Positon as #enemy for all", false));
            Others.AddLabel(" - if false:    team timer will use champion name(shorted)");
            Others.AddLabel(" - if true:     team timer will use the position name you set");
            Others.AddLabel("If positon not set: team timer will use champion name(shorted)");
            Others.AddSeparator();
            foreach (AIHeroClient hero in ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team))
            {
                Others.Add("posof" + hero.NetworkId + hero.ChampionName, new ComboBox(hero.ChampionName + " Position:", 0, "not set", "TOP", "JUG", "MID", "SUP", "ADC"));
                Others.Add("dontusepostionfor" + +hero.NetworkId + hero.ChampionName, new CheckBox("Team Timer: Don't Use Positon for "+ hero.ChampionName, false));
                Others.AddSeparator(10);
            }
            Others.AddSeparator();
            Others.Add("enableDamgeAPI", new CheckBox("Enable Damge Indicator(API)"));
            DamageBarSlide = new ColorSlide(Others, "damageColor", Color.Orange, "Damge Bar Color(F5 to reload):");
            Others.AddSeparator();
            Others.AddLabel("Death Time Increase Rate:");
            Others.AddLabel(" - The parts per 10000 increased to respawn time every minute after 10:00 but before 60:00");
            Others.Add("DeathRate", new Slider("DTIR(Per 10000)", 88, 75, 110));
            Others.AddLabel("Dont touch if the death respawn time is correct");
        }
        public static string Format(float f)
        {
            TimeSpan t = TimeSpan.FromSeconds(f);
            if (t.Minutes < 1)
            {
                return t.Seconds + "";
            }
            if (t.Seconds >= 10)
            {
                return t.Minutes + ":" + t.Seconds;
            }
            return t.Minutes + ":0" + t.Seconds;
        }
        public static string Format3(float f)
        {
            TimeSpan t = TimeSpan.FromSeconds(f);
            if (t.Minutes == 1 && t.Seconds <40 )
            {
                int time = t.Seconds + 60;
                return time + "";
            }
            else
            {
                if (t.Minutes < 1)
                {
                    return t.Seconds + "";
                }
                if (t.Seconds >= 10)
                {
                    return t.Minutes + ":" + t.Seconds;
                }
            }
            return t.Minutes + ":0" + t.Seconds;
        }
        public static string Format2(float f)
        {
            TimeSpan t = TimeSpan.FromSeconds(f);
            if (t.Hours > 0)
            {
                return t.Hours * 60 + t.Minutes + ":" + t.Seconds;
            }
            if (t.Minutes < 1)
            {
                return  "00:" + t.Seconds;// it is unpossible actually
            }
            if (t.Seconds >= 10)
            {
                return t.Minutes + ":" + t.Seconds;
            }
            return t.Minutes + ":0" + t.Seconds;
        }
        public static float GetDeathTime(AIHeroClient deadman, float gametime)
        {
            float rate = DeathRate / 10000f;
            float BRW = (deadman.Level * 2.5f + 7.5f);
            if (gametime > 10 * 60 && gametime < 60 * 60) BRW += BRW * ((int)((gametime - 10 * 60)/60)) * rate;
            if (gametime > 60 * 60) BRW += BRW * rate/2f;
            return BRW;
        }
    }
}