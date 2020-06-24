using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using QwertysRandomContent.Config;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;

namespace QwertysRandomContent.NPCs.Fortress
{
    public class Hopper : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Enchanted Tile");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicFortress ? base.Texture + "_Classic" : base.Texture;

        public override void SetDefaults()
        {
            npc.width = 32;
            npc.height = 42;
            npc.aiStyle = -1;
            npc.damage = 28;
            npc.defense = 18;
            npc.lifeMax = 160;
            npc.value = 100;
            //npc.alpha = 100;
            //npc.behindTiles = true;
            npc.HitSound = SoundID.NPCHit7;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.knockBackResist = 0f;
            npc.noGravity = true;
            //npc.dontTakeDamage = true;
            //npc.scale = 1.2f;
            npc.buffImmune[20] = true;
            npc.buffImmune[24] = true;
            banner = npc.type;
            bannerItem = mod.ItemType("HopperBanner");

            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 40; i++)
                {
                    int dustType = mod.DustType("FortressDust"); ;
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                int dustType = mod.DustType("FortressDust"); ;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return preSetTimer <= 0;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return preSetTimer <= 0;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBrick"), Main.rand.Next(2, 5));
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>().TheFortress)
            {
                return 140f;
            }
            return 0f;
        }

        private int frame;
        private int timer;
        private float jumpSpeedY = -10.5f;
        private float jumpSpeedX = 4;
        private float aggroDistance = 400;
        private float aggroDistanceY = 200;
        private bool jump;
        private float gravity = .3f;
        private bool runOnce = true;
        private bool flipped = false;
        private int preSetTimer = 120;
        private bool spawnChildren = false;

        public override void AI()
        {
            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            if (runOnce)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        spawnChildren = true;

                        break;

                    case 1:
                        Point origin = npc.Center.ToTileCoordinates();
                        Point point;
                        for (int s = 0; s < 200; s++)
                        {
                            if (npc.Top.ToTileCoordinates().X - 10 < 0)
                            {
                                break;
                            }
                            if (!WorldUtils.Find(origin, Searches.Chain(new Searches.Up(2), new GenCondition[]
                            {
                                            new Conditions.IsSolid()
                            }), out point))
                            {
                                npc.position.Y--;
                                origin = npc.Center.ToTileCoordinates();
                            }
                            else
                            {
                                flipped = true;
                                break;
                            }
                        }
                        break;
                }
                if (!flipped)
                {
                    Point origin = npc.Center.ToTileCoordinates();
                    Point point;

                    while (!WorldUtils.Find(origin, Searches.Chain(new Searches.Down(4), new GenCondition[]
                    {
                                            new Conditions.IsSolid()
                    }), out point))
                    {
                        npc.position.Y++;
                        origin = npc.Center.ToTileCoordinates();
                    }
                }
                runOnce = false;
            }
            if (preSetTimer > 0)
            {
                preSetTimer--;
                npc.dontTakeDamage = true;
                npc.velocity = Vector2.Zero;
                float d = Main.rand.NextFloat() * (float)Math.PI * 2;
                Dust dusty = Dust.NewDustPerfect(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)) + QwertyMethods.PolarVector(30f, d + (float)Math.PI), mod.DustType("FortressDust"), QwertyMethods.PolarVector(3f, d), Scale: .5f);
                dusty.noGravity = true;
                if (preSetTimer == 0 && spawnChildren)
                {
                    int children = Main.rand.Next(3);
                    for (int i = 0; i < children; i++)
                    {
                        NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-40, 41), (int)npc.Center.Y, mod.NPCType("YoungTile"));
                    }
                }
            }
            else
            {
                if (frame == 0)
                {
                    npc.dontTakeDamage = true;
                }
                else
                {
                    npc.dontTakeDamage = false;
                }
                if (flipped)
                {
                    gravity = 0f;
                    npc.rotation = (float)Math.PI;
                    Player player = Main.player[npc.target];
                    npc.TargetClosest(true);
                    if (Collision.CheckAABBvLineCollision(player.position, player.Size, npc.Center, npc.Center + new Vector2(0, 1000)) && Collision.CanHit(npc.Center, 0, 0, player.Center, 0, 0))
                    {
                        flipped = false;
                        timer = 63;
                        jump = true;
                        npc.velocity.Y = 9;
                    }
                }
                else
                {
                    npc.rotation = 0f;
                    gravity = .3f;
                    float worldSizeModifier = (float)(Main.maxTilesX / 4200);
                    worldSizeModifier *= worldSizeModifier;
                    //small =1
                    //medium =2.25
                    //large =4
                    float num2 = (float)((double)(npc.position.Y / 16f - (60f + 10f * worldSizeModifier)) / (Main.worldSurface / 6.0));
                    if ((double)num2 < 0.25)
                    {
                        num2 = 0.25f;
                    }
                    if (num2 > 1f)
                    {
                        num2 = 1f;
                    }
                    gravity *= num2;
                    jumpSpeedY = gravity * -35;
                    //Main.NewText("gravity: " +gravity);
                    //Main.NewText("jump: " +jumpSpeedY);
                    Player player = Main.player[npc.target];
                    npc.TargetClosest(true);
                    //Main.NewText(Math.Abs(player.Center.X - npc.Center.X));
                    if (Math.Abs(player.Center.X - npc.Center.X) < aggroDistance && Math.Abs(player.Bottom.Y - npc.Bottom.Y) < aggroDistanceY)
                    {
                        jumpSpeedX = Math.Abs(player.Center.X - npc.Center.X) / 70 * (npc.confused ? -1 : 1);
                        timer++;
                        if (timer > 30)
                        {
                            frame = 3;
                            if (!jump)
                            {
                                if (player.Center.X > npc.Center.X)
                                {
                                    npc.velocity.X = jumpSpeedX;
                                    npc.velocity.Y = jumpSpeedY;
                                }
                                else
                                {
                                    npc.velocity.X = -jumpSpeedX;
                                    npc.velocity.Y = jumpSpeedY;
                                }
                                jump = true;
                            }
                        }
                        else if (timer > 20)
                        {
                            frame = 1;
                        }
                        else if (timer > 10)
                        {
                            frame = 2;
                        }
                        else
                        {
                            frame = 1;
                        }
                    }
                    else if (!jump)
                    {
                        frame = 0;
                        timer = 0;
                    }
                    if (npc.collideX)
                    {
                        npc.velocity.X *= -1;
                    }
                    if (timer > 62 && npc.collideY)
                    {
                        npc.velocity.X = 0;
                        npc.velocity.Y = 0;
                        jump = false;
                        timer = 0;
                    }
                    npc.velocity.Y += gravity;
                }
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frame * frameHeight;
        }
    }
}