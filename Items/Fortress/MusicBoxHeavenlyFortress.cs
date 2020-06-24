using QwertysRandomContent.Config;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Fortress
{
    public class MusicBoxHeavenlyFortress : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Music Box (Heavenly Fortress)");
        }

        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicFortress ? base.Texture + "_Classic" : base.Texture;

        public override void SetDefaults()
        {
            item.useStyle = 1;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
            item.autoReuse = true;
            item.consumable = true;
            item.createTile = mod.TileType("MusicBoxHeavenlyFortress");
            item.width = 24;
            item.height = 24;
            item.rare = 4;
            item.value = 100000;
            item.accessory = true;
        }
    }
}