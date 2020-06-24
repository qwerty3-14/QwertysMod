using Microsoft.Xna.Framework;
using QwertysRandomContent.Config;
using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Tiles
{
    public class DnasBrick : ModTile
    {
        public override bool Autoload(ref string name, ref string texture)
        {
            if (ModContent.GetInstance<SpriteSettings>().ClassicFortress)
            {
                texture += "_Classic";
            }
            return base.Autoload(ref name, ref texture);
        }

        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            //Main.tileSpelunker[Type] = true;
            Main.tileBrick[Type] = true;
            Main.tileBlendAll[Type] = true;

            dustType = mod.DustType("FortressDust");
            soundType = 21;
            soundStyle = 2;
            minPick = 50;
            AddMapEntry(new Color(162, 184, 185));
            mineResist = 1;
            drop = mod.ItemType("DnasBrick");
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.0f;
            g = 0.0f;
            b = 0.0f;
        }

        public override bool CanExplode(int i, int j)
        {
            return true;
        }
    }
}