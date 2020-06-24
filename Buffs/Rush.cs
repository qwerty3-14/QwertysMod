using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Buffs
{
    public class Rush : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Rush");
            Description.SetDefault("+2 dash power");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            var modPlayer = player.GetModPlayer<QwertyPlayer>();

            modPlayer.customDashBonusSpeed += 2;
        }
    }
}