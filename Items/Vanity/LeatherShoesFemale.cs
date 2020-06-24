using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Vanity
{
    [AutoloadEquip(EquipType.Shoes)]
    public class LeatherShoesFemale : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Women's Shoes");
            Tooltip.SetDefault("");
        }

        public override void SetDefaults()
        {
            item.value = 1000;
            item.rare = 1;

            item.width = 32;
            item.height = 32;
            item.accessory = true;
            item.vanity = true;
        }
    }
}