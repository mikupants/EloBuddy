using System.Drawing;
using BigFatHUD.Properties;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Events;
using System;
using SharpDX;
using Color = SharpDX.Color;
using EloBuddy.SDK.Enumerations;

namespace BigFatHUD
{
    
    public class HUD
    {
        
        private float width { get; set; }
        private readonly Render.Line HPline;
        private readonly Render.Line Manaline;
        private readonly AIHeroClient Hero;
        private readonly int Number;
        private float lastdeath { get; set; }
        private float predictedHP { get; set; }
        private bool lastdeathcontrol { get; set; }
        private int lastchange1 { get; set; }
        private readonly Render.Sprite Ronimage;
        private readonly Render.Sprite S1;
        private readonly Render.Sprite S2;

        public HUD(AIHeroClient hero, int number = 0)
        {
            Hero = hero;
            Number = number;
            lastdeathcontrol = true;
            lastdeath = 0f;
            predictedHP = 0f;

            var xadd = number * 110f * Menus.HUDScale;

            var scal = Menus.HUDScale;

            var bmp = Resources.ResourceManager.GetObject(hero.ChampionName + "_Square_0") as Bitmap;
            var image = new Render.Sprite(bmp, new Vector2(xadd, 0));

            var bmp2 = Resources.main as Bitmap;
            var image2 = new Render.Sprite(bmp2, new Vector2(xadd, 0));

            var RON = Resources.Ron as Bitmap;
            Ronimage = new Render.Sprite(RON, new Vector2(xadd, 0));

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
                    v2.X += predictedHP * (0.82f - 0.07f) * scal;
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
                    v2.X += hero.ManaPercent * (0.82f - 0.07f) * scal;
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

            S1 = new Render.Sprite(S1bitmap, new Vector2(xadd, 0));
            S2 = new Render.Sprite(S2bitmap, new Vector2(xadd, 0));

            var black1 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));
            var black2 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));
            var black3 = new Render.Sprite(Resources.black60, new Vector2(xadd, 0));

            black1.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            black1.VisibleCondition = sender => Menus.HUDEnable &&
                        Sspell1.CooldownExpires - Game.Time > 0 &&
                        !Sspell1.IsReady;
            black1.PositionUpdate = delegate
            {
                Vector2 v2 = new Vector2(Menus.HUDX + xadd, Menus.HUDY);
                v2.X += scal * 60;
                v2.Y += scal * 6;
                return v2;
            };
            black1.Add(1);

            black2.Scale = new Vector2(Menus.HUDScale, Menus.HUDScale);
            black2.VisibleCondition = sender => Menus.HUDEnable &&
                        Sspell2.CooldownExpires - Game.Time > 0 &&
                        !Sspell2.IsReady;
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
                bool fix1 = (hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires - Game.Time > 0 && hero.Spellbook.GetSpell(SpellSlot.R).IsLearned)||
                !hero.Spellbook.GetSpell(SpellSlot.R).IsLearned;
                return Menus.HUDEnable && fix1;
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
                    v2.X += Menus.CDX;
                    v2.Y += Menus.CDY;
                    return v2;
                },
                TextUpdate = () => Menus.Format3(hero.Spellbook.GetSpell(SpellSlot.R).CooldownExpires - Game.Time),
                OutLined = true,
                Centered = true
            };
            var isdead = new Render.Text((int)xadd, 0, "Dead", (int)(22 * Menus.HUDScale), Color.Red)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable && hero.IsDead,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + 44.5f * Menus.HUDScale, Menus.HUDY + 69f * Menus.HUDScale);
                    return v2;
                },
                TextUpdate = delegate
                {
                    return lastdeath == 0f ? "?": Menus.Format3(lastdeath + Menus.GetDeathTime(hero, Game.Time - 35f) - Game.Time);
                },
                OutLined = true,
                Centered = true
            };
            isdead.Add(5);
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
            var SummSpe_1 = new Render.Text((int)xadd, 0, "", (int)(24 * Menus.HUDScale), Color.DarkRed)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable &&
                        Sspell1.CooldownExpires - Game.Time > 0 &&
                        !Sspell1.IsReady,
                PositionUpdate = delegate
                {
                    Vector2 v2 = new Vector2(Menus.HUDX + xadd + (71 * scal), Menus.HUDY + (17 * scal));
                    return v2;
                },
                //ms = Program.Message.Replace("#enemyname", slangName(msg.Name)).Replace("#spell", msg.Spell).Replace("#time", msg.Time);
                TextUpdate = () => Menus.Format(Sspell1.CooldownExpires - Game.Time),
                OutLined = true,
                Centered = true
            };
            var SummSpe_2 = new Render.Text((int)xadd, 0, "", (int)(24 * Menus.HUDScale), Color.DarkRed)
            {
                VisibleCondition =
                    sender =>
                        Menus.HUDEnable &&
                        Sspell2.CooldownExpires - Game.Time > 0 &&
                        !Sspell2.IsReady,
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
            Game.OnWndProc += Game_OnWndProc;
            Teleport.OnTeleport += Teleport_OnTeleport;
        }

        private void Teleport_OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            if (Hero == null || !Hero.IsValid || Hero.IsAlly)
            {
                return;
            }

            if (args.Type != TeleportType.Recall || !(sender is AIHeroClient))
            {
                return;
            }

            if (sender.NetworkId == Hero.NetworkId && args.Status == TeleportStatus.Finish)
            {
                Core.DelayAction(() =>
                {
                    if (!Hero.IsVisible)
                        predictedHP += (100f - predictedHP) / 4f;
                },
                500);
                Core.DelayAction(() =>
                {
                    if (!Hero.IsVisible)
                        predictedHP += (100f - predictedHP) / 3f;
                },
                500);
                Core.DelayAction(() =>
                {
                    if (!Hero.IsVisible)
                        predictedHP += (100f - predictedHP) / 2f;
                },
                500);
                Core.DelayAction(() =>
                {
                    if (!Hero.IsVisible)
                        predictedHP = 100f;
                },
                500);
            }
        }

        private void Game_OnUpdate(EventArgs args)
        {
            if (Hero.IsVisible) predictedHP = Hero.HealthPercent;
            else if (predictedHP < 100)
            {
                if (Environment.TickCount - lastchange1 > 5000)
                {
                    predictedHP += 5f * Hero.HPRegenRate / Hero.MaxHealth;
                    lastchange1 = Environment.TickCount;
                }
            }
            else
            {
                predictedHP = 100f;
            }
            if (Hero.HealthPercent <= 18f) HPline.Color = Menus.LowHPBarColor;
            else HPline.Color = Menus.HighHPBarColor;
            Manaline.Color = Menus.ManaBarColor;
            if (Hero.IsDead && lastdeathcontrol)
            {
                lastdeath = Game.Time;
                lastdeathcontrol = false;
            }
            if (!Hero.IsDead && !lastdeathcontrol)
            {
                lastdeath = Game.Time;
                lastdeathcontrol = true;
            }
        }
        // double click
        #region click
        private bool isOn(SpellSlot spell)
        {
            return !isOff(spell) && Hero.Spellbook.GetSpell(spell).IsLearned;
        }
        private bool isOff(SpellSlot spell)
        {
            return !Hero.Spellbook.GetSpell(spell).IsReady &&
                        Hero.Spellbook.GetSpell(spell).CooldownExpires - Game.Time > 0;
        }
        private void Game_OnWndProc(WndEventArgs args)
        {
            if (!Menus.HUDEnable || !Menus.TeamTimer || args.Msg != (uint)WindowsMessages.WM_LBUTTONDBLCLK)
                return;
            if (isinsideR(Game.CursorPos2D) && isOff(SpellSlot.R) && Hero.Spellbook.GetSpell(SpellSlot.R).IsLearned)
            {
                Chat.Print((int)Hero.DeathDuration + "");
                Chat.Say(sayRexpiretime());
            }
            if (isinsideR(Game.CursorPos2D) && isOn(SpellSlot.R))
            {
                Chat.Print((int)Hero.DeathDuration + "");
                Chat.Say(getshortname() + " has R");
            }
            if (isinsideR(Game.CursorPos2D) && !Hero.Spellbook.GetSpell(SpellSlot.R).IsLearned)
            {
                Chat.Print((int)Hero.DeathDuration + "");
                Program.Print(Hero.ChampionName + " didn't learn R");
            }
            if (isinsideS1(Game.CursorPos2D) && isOff(SpellSlot.Summoner1))
            {
                //Chat.Print("isinS1");
                //Chat.Print(Menus.Format2(Hero.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires));
                //Chat.Print(TimeSpan.FromMilliseconds(Program.sw.ElapsedMilliseconds + 3000));
                Chat.Say(sayS1expiretime());
            }
            if (isinsideS1(Game.CursorPos2D) && isOn(SpellSlot.Summoner1))
            {
                //Chat.Print("isinS1");
                //Chat.Print(Menus.Format2(Hero.Spellbook.GetSpell(SpellSlot.Summoner1).CooldownExpires));
                //Chat.Print(TimeSpan.FromMilliseconds(Program.sw.ElapsedMilliseconds + 3000));
                Chat.Say(getshortname() + " has " + getshortname2(Hero.Spellbook.GetSpell(SpellSlot.Summoner1).Name));
            }
            if (isinsideS2(Game.CursorPos2D) && isOff(SpellSlot.Summoner2))
            {
                //Chat.Print("isinS1");
                Chat.Say(sayS2expiretime());
            }
            if (isinsideS2(Game.CursorPos2D) && isOn(SpellSlot.Summoner2))
            {
                //Chat.Print("isinS2");
                Chat.Say(getshortname() + " has " + getshortname2(Hero.Spellbook.GetSpell(SpellSlot.Summoner2).Name));
            }
        }
        private string sayRexpiretime()
        {
            var R = Hero.Spellbook.GetSpell(SpellSlot.R);
            return finalsay("R", R.CooldownExpires - 35f);
        }
        private string sayS1expiretime()
        {
            var S1 = Hero.Spellbook.GetSpell(SpellSlot.Summoner1);
            return finalsay(getshortname2(S1.Name), S1.CooldownExpires - 35f);
        }
        private string sayS2expiretime()
        {
            var S2 = Hero.Spellbook.GetSpell(SpellSlot.Summoner2);
            return finalsay(getshortname2(S2.Name), S2.CooldownExpires - 35f);
        }
        private bool isinsideR(Vector2 pos)
        {
            Vector2 v2 = Ronimage.Position;
            v2.X += Menus.HUDScale * Ronimage.Width / 2.0f;
            v2.Y += Menus.HUDScale * Ronimage.Height / 2.0f;
            if (pos.Distance(v2) - 1f <= Ronimage.Width / 2.0f)
            {
                return true;
            }
            return false;
        }
        private bool isinsideS1(Vector2 pos)
        {
            Vector2 v2 = S1.Position;
            if (pos.X >= v2.X && pos.X <= v2.X + Menus.HUDScale * S1.Width
                && pos.Y >= v2.Y && pos.Y <= v2.Y + Menus.HUDScale * S1.Height)
            {
                return true;
            }
            return false;
        }
        private bool isinsideS2(Vector2 pos)
        {
            Vector2 v2 = S2.Position;
            if (pos.X >= v2.X && pos.X <= v2.X + Menus.HUDScale * S2.Width
                && pos.Y >= v2.Y && pos.Y <= v2.Y + Menus.HUDScale * S2.Height)
            {
                return true;
            }
            return false;
        }
        private string finalsay(string spell, float time)
        {
            return getshortname() + " " + spell + " " + Menus.Format2(time);
        }
        private string getshortname()
        {
            if (Menus.UsePosition && !Menus.DontUsePositionFor(Hero))
            {
                switch (Menus.Position(Hero))
                {
                    case 1:
                        return "TOP";
                    case 2:
                        return "JUG";
                    case 3:
                        return "MID";
                    case 4:
                        return "SUP";
                    case 5:
                        return "ADC";
                    default:
                        break;
                }
            }
            switch (Hero.ChampionName)
            {
                case "Alistar": return "ali";
                case "Blitzcrank": return "blitz";
                case "Caitlyn": return "cait";
                case "Cassiopeia": return "cass";
                case "ChoGath": return "cho";
                case "Chogath": return "cho";
                case "Dr.Mundo": return "mundo";
                case "Evelynn": return "eve";
                case "Ezreal": return "ez";
                case "Fiddlesticks": return "fiddles";
                case "Heimerdinger": return "heimer";
                case "Jarvan IV": return "j4";
                case "Katarina": return "kat";
                case "KogMaw": return "kog";
                case "Kogmaw": return "kog";
                case "LeBlanc": return "lb";
                case "LeeSin": return "lee";
                case "Lissandra": return "liss";
                case "Malphite": return "malph";
                case "Malzahar": return "malz";
                case "MasterYi": return "yi";
                case "MissFortune": return "mf";
                case "MonkeyKing": return "monkey";
                case "Mordekaiser": return "mord";
                case "Morgana": return "morg";
                case "Nautilus": return "naut";
                case "Nidalee": return "nid";
                case "Nocturne": return "noct";
                case "Orianna": return "ori";
                case "Rek'Sai": return "reksai";
                case "Sejuani": return "sej";
                case "TahmKench": return "tahm";
                case "Tristana": return "trist";
                case "Tryndamere": return "trynd";
                case "TwistedFate": return "tf";
                case "Vel'Koz": return "velkoz";
                case "Vladimir": return "vlad";
                case "Volibear": return "voli";
                case "Xin Zhao": return "zhao";
                default: return Hero.ChampionName.ToLower();
            }
        }
        private string getshortname2(String spell)
        {
            if (spell.ToLower().Contains("smite")) return "smite";
            switch (spell.ToLower())
            {
                case "summonerflash": return "flash";
                case "summonerdot": return "ignite";
                case "summonerexhaust": return "exhaust";
                case "summonerhaste": return "ghost";
                case "summonerheal": return "heal";
                case "summonerteleport": return "TP";
                case "summonerboost": return "cleanse";
                case "summonerbarrier": return "barrier";
                default:
                    Chat.Print(spell.ToLower());
                    return "summoner spell";//what?????????
            }
        }
        #endregion click
    }
}
