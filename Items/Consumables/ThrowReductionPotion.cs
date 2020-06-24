using QwertysRandomContent.Config;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Consumables
{
    public class ThrowReductionPotion : ModItem
    {
        public override string Texture => ModContent.GetInstance<SpriteSettings>().ClassicPotions ? base.Texture + "_Old" : base.Texture;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Throw Reduction Potion");
            Tooltip.SetDefault("40% chance not to consume thrown items");
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 32;
            item.maxStack = 30;
            item.rare = 3;
            item.useAnimation = 17;
            item.useTime = 45;
            item.useStyle = 2;
            item.UseSound = SoundID.Item3;
            item.consumable = true;
            //item.potion = true;
            item.useTurn = true;
            item.buffType = mod.BuffType("ThrowReduction");
            item.buffTime = 60 * 60 * 7;
        }

        public override bool CanUseItem(Player player)
        {
            return true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BottledWater);
            recipe.AddIngredient(ItemID.Waterleaf);
            recipe.AddIngredient(ItemID.Blinkroot);
            recipe.AddIngredient(mod.ItemType("CaeliteCore"));
            recipe.AddTile(TileID.Bottles);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}