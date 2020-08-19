using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Shroomite
{
    public class GunChakram : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Chakram");
            Tooltip.SetDefault("Shoots bullets when it bounces off walls or enemies!" + "\nMore melee speed will make it fire more bullets");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGunChakram ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 49;
            item.melee = true;
            item.noMelee = true;

            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.knockBack = 2;
            item.value = 50000;
            item.rare = 8;
            item.UseSound = SoundID.Item1;
            item.noUseGraphic = true;
            item.width = 72;
            item.height = 72;

            item.autoReuse = true;
            item.shoot = mod.ProjectileType("GunChakramP");
            item.shootSpeed = 12;
        }

        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ShroomiteBar, 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < 1000; ++i)
            {
                if (Main.projectile[i].active && Main.projectile[i].owner == Main.myPlayer && Main.projectile[i].type == item.shoot)
                {
                    return false;
                }
            }
            return true;
        }
    }

    public class GunChakramP : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;
            //aiType = ProjectileID.ThornChakram;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.width = 36;
            projectile.height = 36;
            projectile.melee = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gun Chakram");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicGunChakram ? base.Texture + "_Old" : base.Texture;

        public bool runOnce = true;
        public int rotationDirection = 1;
        public int timer;
        public float direction;
        public float speed;
        public bool runOnceTwo = true;
        public float playerDirection;
        public bool hasAproachedPlayer;

        public override void AI()
        {
            Player player = Main.player[projectile.owner];
            if (runOnce)
            {
                speed = projectile.velocity.Length();
                if (projectile.velocity.X > 0)
                {
                    rotationDirection = -1;
                }
                runOnce = false;
            }
            projectile.rotation += (rotationDirection * 8 * (float)Math.PI) / 60;
            timer++;
            if (timer > 30)
            {
                projectile.tileCollide = false;
                if (runOnceTwo)
                {
                    direction = (projectile.velocity).ToRotation();
                    runOnceTwo = false;
                }

                playerDirection = (player.Center - projectile.Center).ToRotation();
                direction = playerDirection;

                float distance = (float)Math.Sqrt((player.Center.X - projectile.Center.X) * (player.Center.X - projectile.Center.X) + (player.Center.Y - projectile.Center.Y) * (player.Center.Y - projectile.Center.Y));
                if (distance < 10 * (speed / 15))
                {
                    projectile.Kill();
                }
                projectile.velocity.X = speed * (float)Math.Cos(direction);
                projectile.velocity.Y = speed * (float)Math.Sin(direction);
            }
        }

        public int bullet = 14;
        public bool canShoot = true;
        public float speedB = 14f;
        public float BulVel = 12;
        public float dir = 0;

        private void Shoot()
        {
            Main.PlaySound(SoundID.Item38, projectile.Center);
            timer += 10;
            Player player = Main.player[projectile.owner];
            int weaponDamage = projectile.damage;
            float weaponKnockback = projectile.knockBack;
            int bulletCount = 8 + (int)((((1 / player.meleeSpeed) - 1) * 100) / 10);

            if (projectile.UseAmmo(AmmoID.Bullet, ref bullet, ref speedB, ref weaponDamage, ref weaponKnockback, false))
            {
                List<Projectile> bullets = QwertyMethods.ProjectileSpread(projectile.Center, bulletCount, BulVel, bullet, weaponDamage, weaponKnockback, projectile.owner);
                foreach (Projectile bul in bullets)
                {
                    bul.melee = true;
                    bul.ranged = false;
                    if (Main.netMode == 1)
                    {
                        QwertysRandomContent.UpdateProjectileClass(bul);
                    }
                }
            }
        }

        public override bool OnTileCollide(Vector2 velocityChange)
        {
            Shoot();
            if (projectile.velocity.X != velocityChange.X)
            {
                projectile.velocity.X = -velocityChange.X;
            }
            if (projectile.velocity.Y != velocityChange.Y)
            {
                projectile.velocity.Y = -velocityChange.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            Shoot();
            projectile.velocity.X = -projectile.velocity.X;
            projectile.velocity.Y = -projectile.velocity.Y;
        }
    }
}