using System.Drawing;
using BigFatHUD.Properties;
using EloBuddy;
using System;
using SharpDX;
using Color = SharpDX.Color;


namespace BigFatHUD
{
    public class HUD
    {
        private float width { get; set; }
        private readonly Render.Line HPline;
        private readonly Render.Line Manaline;
        private readonly AIHeroClient Hero;
        public HUD(AIHeroClient hero, int number = 0)
        {
            Hero = hero;
            var xadd = number * 110f * Menus.HUDScale;

            var scal = Menus.HUDScale;

            var bmp = Resources.ResourceManager.GetObject(hero.ChampionName + "_Square_0") as Bitmap;
            var image = new Render.Sprite(bmp, new Vector2(xadd, 0));

            var bmp2 = Resources.main as Bitmap;
            var image2 = new Render.Sprite(bmp2, new Vector2(xadd, 0));

            var RON = Resources.Ron as Bitmap;
            var Ronimage = new Render.Sprite(RON, new Vector2(xadd, 0));

            var ROFF = Resources.Roff as Bitmap;
            var Roffimage = new Render.Sprite(ROFF, new Vector2(xadd, 0));

            HPline = new Render.Line(new Vector2(xadd, 0), new Vector2(xadd, 0), (int)(8f * Menus.HUDScale), Menus.HighHPBarColor)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable,
                StartPositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (7 * scal), Menus.HUDY + (64.5f * scal));
                    return v2;
                },
                EndPositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (7 * scal), Menus.HUDY + (64.5f * scal));
                    v2.X += hero.HealthPercent * (0.81f - 0.07f);
                    return v2;
                },
            };
            Manaline = new Render.Line(new Vector2(xadd, 0), new Vector2(xadd, 0), (int)(8f * Menus.HUDScale), Menus.ManaBarColor)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable,
                StartPositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (7 * scal), Menus.HUDY + (73.5f * scal));
                    return v2;
                },
                EndPositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (7 * scal), Menus.HUDY + (73.5f * scal));
                    v2.X += hero.ManaPercent * (0.81f - 0.07f);
                    return v2;
                }
            };
            var overlay00 = new Render.Sprite(Resources.ffx, new Vector2(xadd, 0))
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (7 * scal), Menus.HUDY + (61f * scal));
                    return v2;
                },
                Scale = new Vector2(Menus.HUDScale, Menus.HUDScale)
            };
            HPline.Add(5);
            Manaline.Add(5);
            overlay00.Add(6);

            var Sspell1 = hero.Spellbook.GetSpell(SpellSlot.Summoner1);
            var Sspell2 = hero.Spellbook.GetSpell(SpellSlot.Summoner2);

            var S1bitmap = Resources.ResourceManager.GetObject(Sspell1.Name.ToLower()) as Bitmap;
            var S2bitmap = Resources.ResourceManager.GetObject(Sspell2.Name.ToLower()) as Bitmap;

            var S1 = new Render.Sprite(S1bitmap, new Vector2(xadd, 0));
            var S2 = new Render.Sprite(S2bitmap, new Vector2(xadd, 0));

            var black1 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));
            var black2 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));
            var black3 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));

            black1.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            black1.VisibleCondition = sender => Menus.HUDEnable && !Sspell1.IsReady;
            black1.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += scal * 60;
                v2.Y += scal * 6;
                return v2;
            };
            black1.Add(1);

            black2.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            black2.VisibleCondition = sender => Menus.HUDEnable && !Sspell2.IsReady;
            black2.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += scal * 60;
                v2.Y += scal * 32;
                return v2;
            };
            black2.Add(1);

            black3.Scale = new Vector2(Menus.HUDScale * 46f / 23f, Menus.HUDScale * 45f / 23f);
            black3.VisibleCondition = sender => Menus.HUDEnable && hero.IsDead;
            black3.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += 7f * scal;
                v2.Y += 8f * scal;
                return v2;
            };
            black3.Add(1);

            S1.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            S1.VisibleCondition = sender => Menus.HUDEnable;
            S1.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += scal * 60;
                v2.Y += scal * 6;
                return v2;
            };

            S2.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            S2.VisibleCondition = sender => Menus.HUDEnable;
            S2.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += scal * 60;
                v2.Y += scal * 32;
                return v2;
            };

            image2.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            image2.VisibleCondition = sender => Menus.HUDEnable;
            image2.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                return v2;
            };

            image.Scale = new Vector2(Menus.HUDScale * 46f / 120f, Menus.HUDScale * 45f / 120f);
            image.VisibleCondition = sender => Menus.HUDEnable;
            image.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += 7f * scal;
                v2.Y += 8f * scal;
                return v2;
            };

            Ronimage.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            Ronimage.VisibleCondition = delegate
            {
                return Menus.HUDEnable;
            };
            Ronimage.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X -= scal * Ronimage.Width / 2.2f;
                v2.Y -= scal * Ronimage.Height / 2.4f;
                return v2;
            };

            Roffimage.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            Roffimage.VisibleCondition = delegate
            {
                return Menus.HUDEnable &&
                !hero.Spellbook.GetSpell(SpellSlot.R).IsReady &&
                        hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires - Game.Time > 0;
            };
            Roffimage.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X -= scal * Ronimage.Width / 2.2f;
                v2.Y -= scal * Ronimage.Height / 2.4f;
                return v2;
            };

            var Text = new Render.Text((int)xadd, 0, "", (int)(20 * Menus.HUDScale), Color.White)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable && !hero.Spellbook.GetSpell(SpellSlot.R).IsReady &&
                        hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires - Game.Time > 0,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                    return v2;
                },
                TextUpdate = () => Menus.Format(hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires - Game.Time),
                OutLined = true,
                Centered = true
            };
            var Level = new Render.Text((int)xadd, 0, "", (int)(20 * Menus.HUDScale), Color.White)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (44 * scal), Menus.HUDY + (46 * scal));
                    return v2;
                },
                TextUpdate = () => Menus.Format(hero.Level),
                OutLined = true,
                Centered = true
            };
            var SummSpe_1 = new Render.Text((int)xadd, 0, "", (int)(24 * Menus.HUDScale), Color.White)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable &&
                        Sspell1.CooldownExpires - Game.Time > 0 &&
                        Sspell1.IsReady,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (71 * scal), Menus.HUDY + (17 * scal));
                    return v2;
                },
                TextUpdate = () => Menus.Format(Sspell1.CooldownExpires - Game.Time),
                OutLined = true,
                Centered = true
            };
            var SummSpe_2 = new Render.Text((int)xadd, 0, "", (int)(24 * Menus.HUDScale), Color.White)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable &&
                        Sspell2.CooldownExpires - Game.Time > 0 &&
                        Sspell2.IsReady,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (71 * scal), Menus.HUDY + (44 * scal));
                    return v2;
                },
                TextUpdate = () => Menus.Format(Sspell2.CooldownExpires - Game.Time),
                OutLined = true,
                Centered = true
            };


            image.Add(0);
            image2.Add(1);
            Ronimage.Add(2);
            Roffimage.Add(3);
            Text.Add(4);
            Level.Add(2);
            S1.Add(0);
            S2.Add(0);
            SummSpe_1.Add(2);
            SummSpe_2.Add(2);

            Game.OnUpdate += Game_OnUpdate;
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (Hero.HealthPercent <= 18f) HPline.Color = Menus.LowHPBarColor;
            else HPline.Color = Menus.HighHPBarColor;
            Manaline.Color = Menus.ManaBarColor;
        }
    }
}
