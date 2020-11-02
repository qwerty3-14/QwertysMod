﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.RuneGhostItems
{
    public class LeechScroll : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Leech Scroll");
            Tooltip.SetDefault("Ranged attacks may summon leech runes that can heal you");
        }

        public override void SetDefaults()
        {
            item.value = 500000;
            item.rare = 9;
            item.ranged = true;
            item.damage = 50;

            item.width = 54;
            item.height = 56;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<ScrollEffects>().leech = true;
        }
    }

    internal class LeechRuneFreindly : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.aiStyle = -1;

            projectile.width = 20;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;

            projectile.tileCollide = true;
            projectile.timeLeft = 180;
            projectile.ranged = true;
        }

        public int runeTimer;
        public float startDistance = 200f;
        public float direction;
        public float runeSpeed = 10;
        public bool runOnce = true;
        public float aim;

        public override void AI()
        {
            projectile.rotation += MathHelper.ToRadians(3);
        }

        public override void Kill(int timeLeft)
        {
            for (int d = 0; d <= 100; d++)
            {
                Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("LeechRuneDeath"));
            }
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (!target.immortal && !target.SpawnedFromStatue)
            {
                Player player = Main.player[projectile.owner];
                if(Main.rand.Next(2) == 0)
                {
                    player.statLife++;
                    player.HealEffect(1, true);
                }
                
            }
        }
    }
}