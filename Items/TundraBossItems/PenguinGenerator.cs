using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.TundraBossItems
{
    public class PenguinGenerator : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Penguin Generator");
            Tooltip.SetDefault("Attacks have a 10% chance to release penguins/nHide visual if the noise bothers you");
        }

        public override void SetDefaults()
        {
            item.value = 100000;
            item.rare = 1;
            item.expert = true;

            item.width = 28;
            item.height = 32;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PenguinEffect>().effect = true;
            player.GetModPlayer<PenguinEffect>().noSound = hideVisual;
        }
    }

    public class PenguinLimit : GlobalProjectile
    {
        public override bool InstancePerEntity => true;
        public bool realeasedPenguin = false;
    }

    public class PenguinEffect : ModPlayer
    {
        public bool effect;
        public bool noSound;

        public override void ResetEffects()
        {
            effect = false;
            noSound = false;
        }

        public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.Next(10) == 0 && effect && !target.immortal)
            {
                if (!noSound)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PenguinCall").WithVolume(1f).WithPitchVariance(0), player.Center);
                }

                Projectile penguin = Main.projectile[Projectile.NewProjectile(player.Center, new Vector2(6, 0), mod.ProjectileType("SlidingPenguin"), item.damage, item.knockBack, player.whoAmI)];
                penguin.melee = item.melee;
                penguin.ranged = item.ranged;
                penguin.thrown = item.thrown;
                penguin.minion = item.summon;
                penguin.magic = item.magic;
                penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(penguin);
                }
                penguin = Main.projectile[Projectile.NewProjectile(player.Center, new Vector2(-6, 0), mod.ProjectileType("SlidingPenguin"), item.damage, item.knockBack, player.whoAmI)];
                penguin.melee = item.melee;
                penguin.ranged = item.ranged;
                penguin.thrown = item.thrown;
                penguin.minion = item.summon;
                penguin.magic = item.magic;
                penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(penguin);
                }
            }
        }

        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.rand.Next(10) == 0 && effect && !target.immortal && !proj.GetGlobalProjectile<PenguinLimit>().realeasedPenguin)
            {
                if (!noSound)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/SoundEffects/PenguinCall").WithVolume(1f).WithPitchVariance(0), player.Center);
                }
                Projectile penguin = Main.projectile[Projectile.NewProjectile(player.Center, new Vector2(6, 0), mod.ProjectileType("SlidingPenguin"), damage, proj.knockBack, player.whoAmI)];
                penguin.melee = proj.melee;
                penguin.ranged = proj.ranged;
                penguin.thrown = proj.thrown;
                penguin.minion = proj.minion;
                penguin.magic = proj.magic;
                penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(penguin);
                }
                penguin = Main.projectile[Projectile.NewProjectile(player.Center, new Vector2(-6, 0), mod.ProjectileType("SlidingPenguin"), damage, proj.knockBack, player.whoAmI)];
                penguin.melee = proj.melee;
                penguin.ranged = proj.ranged;
                penguin.thrown = proj.thrown;
                penguin.minion = proj.minion;
                penguin.magic = proj.magic;
                penguin.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                proj.GetGlobalProjectile<PenguinLimit>().realeasedPenguin = true;
                if (Main.netMode == 1)
                {
                    QwertysRandomContent.UpdateProjectileClass(penguin);
                }
            }
        }
    }
}