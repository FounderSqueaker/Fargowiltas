﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Fargowiltas.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace Fargowiltas.Items.CaughtNPCs
{
    public class CaughtNPCItem : ModItem
    {
        public static Dictionary<int, int> CaughtNPCs = new Dictionary<int, int>(); // lol

        // todo: eventually turn back to int lol
        public Func<int> getId;
        public string quote;

        public CaughtNPCItem()
        {
            getId = () => NPCID.None;
            quote = "nil";
        }

        public CaughtNPCItem(Func<int> getId, string quote = "")
        {
            this.getId = getId;
            this.quote = quote;
        }

        public override string Texture => getId() < NPCID.Count
            ? $"Terraria/NPC_{getId()}"
            : NPCLoader.GetNPC(getId()).Texture;

        public override bool Autoload(ref string name) => true;

        public override bool CloneNewInstances => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault($"The {Regex.Replace(Name, "([A-Z])", " $1").Trim()}");

            try
            {
                if (string.IsNullOrEmpty(quote))
                    quote = $"'{NPCLoader.GetNPC(getId()).GetChat()}'";
            }
            catch
            {
                quote = "'I have little to say.'";
                // catch and ignore any thrown errors
            }

            if (!string.IsNullOrEmpty(quote))
                Tooltip.SetDefault(quote);

            // loaded in post-load yawn: Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, Main.npcFrameCount[getId()]));
        }

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 20;
            item.maxStack = 10;
            item.rare = ItemRarityID.Blue;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useAnimation = 15;
            item.useTime = 10;
            item.consumable = true;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item44;

            switch (getId())
            {
                case NPCID.Angler:
                    item.bait = 15;
                    break;

                case NPCID.None:
                    return;
            }

            item.makeNPC = (short)getId();
        }

        public override void PostUpdate()
        {
            if (getId() != NPCID.Guide || !item.lavaWet || NPC.AnyNPCs(NPCID.WallofFlesh))
                return;

            NPC.SpawnWOF(item.position);
            item.TurnToAir();
        }

        //public override ModItem NewInstance(Item itemClone) => new CaughtNPCItem(getId, quote);

        //public override ModItem Clone(Item item) => new CaughtNPCItem(getId, quote);

        public override ModItem Clone() => new CaughtNPCItem(getId, quote);

        public static void RegisterItems(Mod mod)
        {
            CaughtNPCs = new Dictionary<int, int>();

            void Add(string name, Func<int> id, string quote)
            {
                mod.AddItem(name, new CaughtNPCItem(id, quote));
                CaughtNPCs.Add(mod.ItemType(name), id());
            }
            void AddAutomatic(string name, Func<int> id) => Add($"Caught{name}", id, "");

            // manually register mutant and vanillas
            Add("Abominationn", ModContent.NPCType<Abominationn>, "'I sure wish I was a boss.'");
            Add("Angler", () => NPCID.Angler, "'You'd be a great helper minion!'");
            Add("ArmsDealer", () => NPCID.ArmsDealer, "'Keep your hands off my gun, buddy!'");
            Add("Clothier", () => NPCID.Clothier, "'Thanks again for freeing me from my curse.'");
            Add("Cyborg", () => NPCID.Cyborg, "'My expedition efficiency was critically reduced when a projectile impacted my locomotive actuator.'");
            Add("Demolitionist", () => NPCID.Demolitionist, "'It's a good day to die!'");
            Add("Deviantt", ModContent.NPCType<Deviantt>, "''Embrace suffering... and while you're at it, embrace another purchase!'");
            Add("Dryad", () => NPCID.Dryad, "'Be safe; Terraria needs you!'");
            Add("DyeTrader", () => NPCID.DyeTrader, "'My dear, what you're wearing is much too drab.'");
            Add("GoblinTinkerer", () => NPCID.GoblinTinkerer, "'Looking for a gadgets expert? I'm your goblin!'");
            // Golfer
            Add("Guide", () => NPCID.Guide, "'They say there is a person who will tell you how to survive in this land.'");
            Add("LumberJack", ModContent.NPCType<LumberJack>, "'I eat a bowl of woodchips for breakfast... without any milk.'");
            Add("Mechanic", () => NPCID.Mechanic, "'Always buy more wire than you need!'");
            Add("Merchant", () => NPCID.Merchant, "'Did you say gold? I'll take that off of ya.'");
            Add("Mutant", ModContent.NPCType<Mutant>, "'You're lucky I'm on your side.'");
            Add("Nurse", () => NPCID.Nurse, "'Show me where it hurts.'");
            Add("Painter", () => NPCID.Painter, "'I know the difference between turquoise and blue-green. But I won't tell you.'");
            Add("PartyGirl", () => NPCID.PartyGirl, "'We have to talk. It's... it's about parties.'");
            Add("Pirate", () => NPCID.Pirate, "'Stay off me booty, ya scallywag!'");
            Add("SantaClaus", () => NPCID.SantaClaus, "'What? You thought I wasn't real?'");
            Add("SkeletonMerchant", () => NPCID.SkeletonMerchant, "'You would not believe some of the things people throw at me... Wanna buy some of it?'");
            if (ModLoader.GetMod("FargowiltasSouls") != null)
                Add("Squirrel", ModContent.NPCType<Squirrel>, "*squeak*");
            Add("Steampunker", () => NPCID.Steampunker, "'Show me some gears!'");
            Add("Stylist", () => NPCID.Stylist, "'Did you even try to brush your hair today?'");
            Add("Tavernkeep", () => NPCID.DD2Bartender, "'What am I doing here...'");
            Add("TaxCollector", () => NPCID.TaxCollector, "'You again? Suppose you want more money!?'");
            Add("TravellingMerchant", () => NPCID.TravellingMerchant, "'I sell wares from places that might not even exist!'");
            Add("Truffle", () => NPCID.Truffle, "'Everyone in this town feels a bit off.'");
            Add("WitchDoctor", () => NPCID.WitchDoctor, "'Which doctor am I? The Witch Doctor am I.'");
            Add("Wizard", () => NPCID.Wizard, "'Want me to pull a coin from behind your ear? No? Ok.'");
            // Zoologist

        }
    }
}