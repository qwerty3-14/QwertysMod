﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace QwertysRandomContent.Items.Weapons.DukeFishron
{
    public class DukeDrop : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if(!Main.expertMode && npc.type == NPCID.DukeFishron)
            {
                string itemName = "";
                switch(Main.rand.Next(4))
                {
                    case 0:
                        itemName = "Cyclone";
                        break;
                    case 1:
                        itemName = "Whirlpool";
                        break;
                    case 2:
                        itemName = "BubbleBrewerBaton";
                        break;
                    case 3:
                        itemName = "SSHurricaneItem";
                        break;
                }
                Item.NewItem(npc.getRect(), mod.ItemType(itemName));
            }
        }
    }
    public class DukeBag : GlobalItem
    {
        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag" && arg == ItemID.FishronBossBag )
            {
                string itemName = "";
                switch (Main.rand.Next(4))
                {
                    case 0:
                        itemName = "Cyclone";
                        break;
                    case 1:
                        itemName = "Whirlpool";
                        break;
                    case 2:
                        itemName = "BubbleBrewerBaton";
                        break;
                    case 3:
                        itemName = "SSHurricaneItem";
                        break;
                }
                player.QuickSpawnItem(mod.ItemType(itemName));
            }
        }
    }
}
