using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using Mario_s_Lib;
using System;
using Color = System.Drawing.Color;

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
        private static bool getCheckBoxItem(Menu m, string item)
        {
            return m[item].Cast<CheckBox>().CurrentValue;
        }
        private static int getSliderItem(Menu m, string item)
        {
            return m[item].Cast<Slider>().CurrentValue;
        }
        public static bool HUDEnable
        {
            get { return getCheckBoxItem(GeneralMenu, "enableHUD"); }
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
        public static void CreateMenu()
        {
            HUDMenu = MainMenu.AddMenu("Big Fat HUD", "bigfathud");
            GeneralMenu = HUDMenu.AddSubMenu("General Setting", generalsetting);
            GeneralMenu.Add("enableHUD", new CheckBox("Enable"));
            GeneralMenu.Add("scale", new Slider("HUD Scale % (F5 to Reload)", 100, 80));
            GeneralMenu.Add("HUD.x", new Slider("X", 0, 0, 2000));
            GeneralMenu.Add("HUD.y", new Slider("Y", 0, 0, 2000));
            ColorMenu = HUDMenu.AddSubMenu("Color Setting", colorsetting);
            HighHPBarSlide = new ColorSlide(ColorMenu, "highColor", Color.Green, "High-HP Bar Color:");
            ColorMenu.Add("HPref", new Slider("HP divides at:", 20, 1, 40));
            LowHPBarSlide = new ColorSlide(ColorMenu, "lowColor", Color.DarkRed, "Low-HP Bar Color:");
            ManBarSlide = new ColorSlide(ColorMenu, "manaColor", Color.Orange, "Mana Bar Color:");
            Others = HUDMenu.AddSubMenu("Other Setting", othersetting);
            Others.AddLabel("Comming Soon TM");
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
    }
}