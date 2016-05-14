using EloBuddy;
using System.Collections.Generic;
using System.Linq;

namespace BigFatHUD
{
    //nothing yet
    internal class API
    {
        public static List<HeroandDamage> _heroesinfo = new List<HeroandDamage>();
        public void InitialAPI()
        {
            _heroesinfo.Clear();
            foreach (AIHeroClient hero in
                ObjectManager.Get<AIHeroClient>().Where(hero => hero.Team != ObjectManager.Player.Team))
            {
                _heroesinfo.Add(new HeroandDamage(hero, 0f));
            }
        }
        public void DrawDamge(AIHeroClient target, float damage)
        {
            foreach(var hero in _heroesinfo)
            {
                if (target == hero.Hero) hero.Damage = damage;
            }
        }
        public class HeroandDamage
        {
            public AIHeroClient Hero { get; set; }
            public float Damage { get; set; }
            public HeroandDamage(AIHeroClient hero, float damage)
            {
                Hero = hero;
                Damage = damage;
            }
        }
    }
}