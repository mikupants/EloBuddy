using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using BigFatHUD.Properties;
using System.Diagnostics;

namespace BigFatHUD
{
    internal class Program
    {

        public static IList<HUD> _heroHUD = new List<HUD>();
        public static IList<HUD2> _heroHUD2 = new List<HUD2>();
        public static AIHeroClient EnemyTOP;
        public static AIHeroClient EnemyJUG;
        public static AIHeroClient EnemyMID;
        public static AIHeroClient EnemySUP;
        public static AIHeroClient EnemyADC;
        private static IList<AIHeroClient> _heroes = new List<AIHeroClient>();

        public static Stopwatch sw;
        

        private static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            Menus.CreateMenu();
            sw = new Stopwatch();
            sw.Start();
            Resources.ResourceManager.IgnoreCase = true;
            if (_heroHUD.Count != 0) _heroHUD.Clear();
            if (_heroHUD2.Count != 0) _heroHUD2.Clear();
            if (_heroes.Count != 0) _heroes.Clear();
            foreach (AIHeroClient hero in
                ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team))
            {
                switch(Menus.Position(hero))
                {
                    case 1:
                        EnemyTOP = hero;
                        break;
                    case 2:
                        EnemyJUG = hero;
                        break;
                    case 3:
                        EnemyMID = hero;
                        break;
                    case 4:
                        EnemySUP = hero;
                        break;
                    case 5:
                        EnemyADC = hero;
                        break;
                    default:
                        break;
                }
            }
            if (PositionSet())
            {
                _heroes.Add(EnemyTOP);
                _heroes.Add(EnemyJUG);
                _heroes.Add(EnemyMID);
                _heroes.Add(EnemySUP);
                _heroes.Add(EnemyADC);
                foreach (AIHeroClient hero in _heroes)
                {
                    if (hero != null && !Menus.HUD2Enable)
                    {
                        _heroHUD.Add(new HUD(hero, _heroHUD.Count));
                    }
                    if (hero != null && Menus.HUD2Enable)
                    {
                        _heroHUD2.Add(new HUD2(hero, _heroHUD2.Count));
                    }
                }
            }
            else
            {
                if (ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team).Count() < 5)
                {
                    Print("Exist only " + ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team).Count() + " enemies");
                    Print("Use default order");
                }
                else
                {
                    Print("If you prefer to position then championname, set them in setting!");
                    if (NotAllUnset())
                    {
                        Print("Not all champions set, or have conflicts");
                        Print("Use default order");
                    }
                    if (!NotAllUnset())
                        Print("No champion set, use default order");
                }
                foreach (AIHeroClient hero in ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team))
                {
                    if (hero != null && !Menus.HUD2Enable)
                    {
                        _heroHUD.Add(new HUD(hero, _heroHUD.Count));
                    }
                    if (hero != null && Menus.HUD2Enable)
                    {
                        _heroHUD2.Add(new HUD2(hero, _heroHUD2.Count));
                    }
                }
            }
            Print("Loaded!");
        }
        
        public static bool PositionSet()
        {
            return EnemyTOP != null && EnemyJUG != null && EnemyMID != null && EnemySUP != null && EnemyADC != null;
        }
        public static bool NotAllUnset()
        {
            return PositionSet() && !(EnemyTOP == null && EnemyJUG == null && EnemyMID == null && EnemySUP == null && EnemyADC == null);
        }
        public static void Print(string msg)
        {
            Chat.Print("<font color='#ff3232'>Big Fat </font><font color='#d4d4d4'>HUD:</font> <font color='#FFFFFF'>" + msg + "</font>");
        }
    }
}