using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress.CaeliteWeapons
{
    public class CaeliteMagicWeapon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Restless Sun");
            Tooltip.SetDefault("Blessed by higher beings this weapon excels in large open areas!");
        }

        public override void SetDefaults()
        {
            item.damage = 18;
            item.magic = true;
            item.knockBack = 1;
            item.value = 50000;
            item.rare = 3;
            item.width = 24;
            item.height = 28;
            item.useStyle = 5;
            item.shootSpeed = 12f;
            item.useTime = 34;
            item.useAnimation = 34;
            item.mana = 11;
            item.shoot = mod.ProjectileType("CaeliteMagicProjectile");
            item.noUseGraphic = false;
            item.noMelee = true;
            item.UseSound = SoundID.Item21;
            item.autoReuse = true;
        }

        private float direction;

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            int rng = Main.rand.Next(100);
            if (rng == 0)
            {
                int numberOfProjectiles = 10;
                float spread = (float)Math.PI / 2;
                float speed = new Vector2(speedX, speedY).Length();
                for (int p = 0; p < numberOfProjectiles; p++)
                {
                    direction = (new Vector2(speedX, speedY).ToRotation() - (spread / 2)) + (spread * ((float)p / (float)numberOfProjectiles));
                    Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
                }
            }
            else if (rng < 10)
            {
                float speed = new Vector2(speedX, speedY).Length();
                direction = (new Vector2(speedX, speedY).ToRotation() - (float)Math.PI / 6);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
                direction = (new Vector2(speedX, speedY).ToRotation() + (float)Math.PI / 6);
                Projectile.NewProjectile(position, QwertyMethods.PolarVector(speed, direction), type, damage, knockBack, player.whoAmI);
            }
            else
            {
                return true;
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CaeliteBar"), 12);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public class CaeliteMagicProjectile : ModProjectile
        {
            public override void SetStaticDefaults()
            {
                DisplayName.SetDefault("RestlessSun");
            }

            public override void SetDefaults()
            {
                projectile.aiStyle = 0;
                //aiType = ProjectileID.Bullet;
                projectile.width = 44;
                projectile.height = 44;
                projectile.friendly = true;
                projectile.penetrate = 3;
                projectile.magic = true;
                projectile.timeLeft = 180;
                projectile.usesLocalNPCImmunity = true;
                projectile.localNPCHitCooldown = 30;
                projectile.tileCollide = true;
                projectile.light = 1f;
            }

            private float outOfPhaseHeight;

            public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
            {
                if (Main.rand.Next(10) == 0)
                {
                    target.AddBuff(mod.BuffType("PowerDown"), 120);
                }
                projectile.localNPCImmunity[target.whoAmI] = -1;
                //target.immune[projectile.owner] = 0;
            }

            private NPC target;
            private NPC possibleTarget;
            private bool foundTarget;
            private float maxDistance = 10000f;
            private float distance;
            private int timer;
            private float speed = 24;
            private bool runOnce = true;
            private float direction;

            public override void AI()
            {
                if (runOnce)
                {
                    direction = projectile.velocity.ToRotation();

                    runOnce = false;
                }
                Player player = Main.player[projectile.owner];
                for (int k = 0; k < 200; k++)
                {
                    possibleTarget = Main.npc[k];
                    if (!Collision.CheckAABBvAABBCollision(projectile.position, projectile.Size, possibleTarget.position, possibleTarget.Size))
                    {
                        projectile.localNPCImmunity[k] = 0;
                    }
                }
                if (QwertyMethods.ClosestNPC(ref target, maxDistance, projectile.Center))
                {
                    direction = QwertyMethods.SlowRotation(direction, (target.Center - projectile.Center).ToRotation(), 10f);
                }
                projectile.velocity = new Vector2((float)Math.Cos(direction) * speed, (float)Math.Sin(direction) * speed);
                foundTarget = false;
                maxDistance = 10000f;
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"));
                projectile.rotation += (float)Math.PI / 7.5f;
            }

            public override void Kill(int timeLeft)
            {
                for (int i = 0; i < 6; i++)
                {
                    Dust dust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("CaeliteDust"))];
                }
            }
        }
    }
}