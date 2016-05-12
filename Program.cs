using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using BigFatHUD.Properties;

namespace BigFatHUD
{
    internal class Program
    {

        private static IList<HUD> _heroHUD = new List<HUD>();

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menus.CreateMenu();
            Resources.ResourceManager.IgnoreCase = true;
            if (_heroHUD.Count != 0) _heroHUD.Clear();
            foreach (AIHeroClient hero in
                ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team))
            {
                if (hero != null)
                {
                    _heroHUD.Add(new HUD(hero, _heroHUD.Count));
                }
            }
            Print("Loaded!");
        }

        private static void Print(string msg)
        {
            Chat.Print("<font color='#ff3232'>Big Fat </font><font color='#d4d4d4'>HUD:</font> <font color='#FFFFFF'>" + msg + "</font>");
        }
    }
}