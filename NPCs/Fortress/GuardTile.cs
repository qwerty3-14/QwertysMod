using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.NPCs.Fortress
{
    public class GuardTile : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guard Tile");
            Main.npcFrameCount[npc.type] = 8;
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicFortress ? base.Texture + "_Classic" : base.Texture;

        public override void SetDefaults()
        {
            npc.width = 64;
            npc.height = 64;
            npc.aiStyle = -1;
            npc.damage = 120;
            npc.defense = 30;
            npc.lifeMax = 480;
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
            bannerItem = mod.ItemType("GuardTileBanner");
            npc.buffImmune[BuffID.Confused] = false;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.damage = 180;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life <= 0)
            {
                for (int i = 0; i < 90; i++)
                {
                    int dustType = mod.DustType("FortressDust");
                    int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                    Dust dust = Main.dust[dustIndex];
                    dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                    dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                    dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                }
            }
            for (int i = 0; i < 9; i++)
            {
                int dustType = mod.DustType("FortressDust"); ;
                int dustIndex = Dust.NewDust(npc.position, npc.width, npc.height, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
                dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
            }
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FortressBrick"), Main.rand.Next(7, 20));
            if (Main.rand.Next(100) < (Main.expertMode ? 15 : 10))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TileStaff"), 1);
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.player.GetModPlayer<FortressBiome>().TheFortress && Main.hardMode)
            {
                return 40f;
            }
            return 0f;
        }

        private int direction = 0;
        private float speed = 10f;
        private float acceleration = 10f / 60f;
        private float maxAwareDistance = 1000f;
        private int timer;
        private int rushCooldown = 60;
        private int frame;
        private int frameTimer;

        public override void AI()
        {
            npc.GetGlobalNPC<FortressNPCGeneral>().fortressNPC = true;
            Player player = Main.player[npc.target];
            npc.TargetClosest(true);
            timer++;
            switch (direction)
            {
                case 0:
                    npc.velocity = new Vector2(0, 0);
                    float point = 0f;
                    if (timer > rushCooldown)
                    {
                        npc.dontTakeDamage = true;
                        frame = 6;
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, npc.Center, new Vector2(npc.Center.X + maxAwareDistance, npc.Center.Y), npc.height, ref point) && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            direction = 1;
                            timer = 0;
                        }

                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, npc.Center, new Vector2(npc.Center.X - maxAwareDistance, npc.Center.Y), npc.height, ref point) && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            direction = 2;
                            timer = 0;
                        }
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, npc.Center, new Vector2(npc.Center.X, npc.Center.Y - maxAwareDistance), npc.width, ref point) && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            direction = 3;
                            timer = 0;
                        }
                        if (Collision.CheckAABBvLineCollision(player.position, player.Size, npc.Center, new Vector2(npc.Center.X, npc.Center.Y + maxAwareDistance), npc.width, ref point) && Collision.CanHit(npc.position, npc.width, npc.height, player.position, player.width, player.height))
                        {
                            direction = 4;
                            timer = 0;
                        }
                    }
                    break;

                case 1:
                    frame = 4;
                    npc.dontTakeDamage = false;
                    npc.velocity += new Vector2(acceleration * (npc.confused ? -1 : 1), 0);
                    if (npc.velocity.Length() > speed)
                    {
                        npc.velocity = new Vector2(speed * (npc.confused ? -1 : 1), 0);
                    }
                    if (timer > rushCooldown && (npc.collideX || npc.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 2:
                    frame = 2;
                    npc.dontTakeDamage = false;
                    npc.velocity += new Vector2(-acceleration * (npc.confused ? -1 : 1), 0);
                    if (npc.velocity.Length() > speed)
                    {
                        npc.velocity = new Vector2(-speed * (npc.confused ? -1 : 1), 0);
                    }
                    if (timer > rushCooldown && (npc.collideX || npc.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 3:
                    frame = 1;
                    npc.dontTakeDamage = false;
                    npc.velocity += new Vector2(0, -acceleration * (npc.confused ? -1 : 1));
                    if (npc.velocity.Length() > speed)
                    {
                        npc.velocity = new Vector2(0, -speed * (npc.confused ? -1 : 1));
                    }
                    if (timer > rushCooldown && (npc.collideX || npc.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;

                case 4:
                    frame = 3;
                    npc.dontTakeDamage = false;
                    npc.velocity += new Vector2(0, acceleration * (npc.confused ? -1 : 1));
                    if (npc.velocity.Length() > speed)
                    {
                        npc.velocity = new Vector2(0, speed * (npc.confused ? -1 : 1));
                    }
                    if (timer > rushCooldown && (npc.collideX || npc.collideY))
                    {
                        direction = 0;
                        timer = 0;
                    }
                    break;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = frameHeight * frame;
        }
    }
}