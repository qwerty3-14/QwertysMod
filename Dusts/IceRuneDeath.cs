using Terraria;
using Terraria.ModLoader;

namespace QwertysRandomContent.Dusts
{
    public class IceRuneDeath : ModDust
    {
        public override void OnSpawn(Dust dust)
        {
            dust.noGravity = true;
            dust.noLight = false;
            dust.scale = 1f;
        }
    }
}