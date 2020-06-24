﻿using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Accesories
{
    internal class MinionFang : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Minion Fang");
            Tooltip.SetDefault("Makes minion attacks trigger melee effects" + "\nFor example weapon imbunes or beetle scalemail set bonus" + "\nTHIS DOES NOT MEAN THEY GET BOOSTED BY MELEE DAMAGE!");
        }

        public override void SetDefaults()
        {
            item.value = 30000;
            item.rare = 3;

            item.width = 30;
            item.height = 36;

            item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();
            modPlayer.minionFang = true;
        }
    }

    internal class FangEffect : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            QwertyPlayer modPlayer = player.GetModPlayer<QwertyPlayer>();
            if (modPlayer.minionFang && projectile.minion)
            {
                projectile.melee = true;
            }
            else if (projectile.minion)
            {
                projectile.melee = false;
            }
        }
    }
}