using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.Rhuthinium
{
    public class RhuthiniumSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Sword");
            Tooltip.SetDefault("Killing enemies builds up a charge. Right click to realease this charge.");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicRhuthinium ? base.Texture + "_Old" : base.Texture;

        public override void SetDefaults()
        {
            item.damage = 30;
            item.melee = true;

            item.useTime = 20;
            item.useAnimation = 20;
            item.useStyle = 1;
            item.knockBack = 3;
            item.value = 25000;
            item.rare = 3;
            item.UseSound = SoundID.Item1;

            item.width = 64;
            item.height = 64;
            item.crit = 5;
            item.autoReuse = true;
            //item.scale = 5;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("RhuthiniumBar"), 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();

            if (target.life <= 0 && !target.SpawnedFromStatue)
            {
                modPlayer.RhuthiniumCharge++;
                CombatText.NewText(target.getRect(), new Color(38, 126, 126), modPlayer.RhuthiniumCharge, true, false);
            }
        }
    }

    public class RhuthiniumCharge : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rhuthinium Charge");
        }

        public override void SetDefaults()
        {
            projectile.aiStyle = 1;
            aiType = ProjectileID.Bullet;
            projectile.width = 14;
            projectile.height = 14;
            projectile.friendly = true;
            projectile.penetrate = 1;
            projectile.melee = true;
            projectile.knockBack = 10f;
        }

        public override void AI()
        {
            if (Main.rand.Next(2) == 0)
            {
                Dust d = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, mod.DustType("RhuthiniumDust"))];
                d.frame.Y = 0;
                d.noGravity = true;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 32; i++)
            {
                Dust d = Dust.NewDustPerfect(projectile.Center, mod.DustType("RhuthiniumDust"));
                d.frame.Y = 0;
                d.velocity *= 2;
                d.noGravity = true;
            }
        }
    }
}