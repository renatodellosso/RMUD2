using ItemTypes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Items
{
    public static class ItemList
    {
        //We use ConcurrentDictionary instead of Dictionary because it is thread-safe
        //https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2?view=net-7.0
        static readonly ConcurrentDictionary<string, Item> ITEMS = new(new Dictionary<string, Item>()
        {
            { "coin", new SimpleItem("coin", "Gold Coin", 0.01f, "A shiny gold coin.", sellValue : 1, color: "yellow") },

            //Weapons
            { "spear", new Weapon("spear", "Spear", new Attack[]
                {
                    new("stab", "Stab", 8, DamageType.Piercing, 2),
                    new("power", "Power Thrust", 10, DamageType.Piercing, 3)
                }, "A stick with a pointy end.", 5, sellValue: 20)
            },
            { "axe", new Weapon("axe", "Axe", new Attack[]
                {
                    new("chop", "Chop", 8, DamageType.Slashing, 2),
                    new("swing", "Swing", new(4, 3), DamageType.Slashing, 4)
                }, "Good for chopping, both logs and necks.", 8, sellValue: 25)
            },
            { "pickaxe", new Weapon("pickaxe", "Pickaxe", 4, DamageType.Piercing, "A useful mining tool that could serve as a weapon in a pinch.",
                8, sellValue: 20) },
            { "longsword", new Weapon("longsword", "Longsword", new Attack[]
                {
                    new("swing", "Swing", new(4, 2), DamageType.Slashing, 2),
                    new("impale", "Impale", 12, DamageType.Piercing, 4, atkBonus: -1, critThreshold: 18, critMult: 3)
                }, "A long, thin sword.", 8, sellValue: 50)
            },
            { "mace", new Weapon("mace", "Mace", new Attack[]
                {
                    new("swing", "Swing", new(2, 4), DamageType.Bludgeoning, 2, critThreshold: 18, critMult: 3),
                    new("power", "Power Strike", new(4, 3), DamageType.Bludgeoning, 4, atkBonus: -1, critThreshold: 16, critMult: 3)
                }, "A heavy ball on the end of a rod, great for breaking skulls.", 10, sellValue: 55)
            },
            { "boneclub", new Weapon("boneclub", "Bone Club", new Attack[]
                {
                    new("swing", "Swing", new(4, 2), DamageType.Bludgeoning, 3),
                    new("groundpound", "Ground Pound", new(6, 2), DamageType.Thunder, 6, atkBonus: 2)
                }, "A club made out of a massive bone.", 12, sellValue: 65)
            },
            { "unholyknife", new Weapon("unholyknife", "Unholy Knife", new Attack[]
                {
                    new("stab", "Stab", new(8, modifier: 2), DamageType.Cold, 2, lifeSteal: .1f),
                    new("soulrend", "Soul Rend", new(4, 4), DamageType.Necrotic, 5, critMult: 3, lifeSteal: .1f)
                }, "A blasphemous blade.", 4, sellValue: 65, color: "red")
            },
            { "hereticsword", new Weapon("hereticsword", "Heretic", new Attack[]
                {
                    new("slash", "Slash", 10, DamageType.Slashing, 2, lifeSteal: .15f),
                    new("stab", "Stab", new(10, modifier: 2), DamageType.Piercing, 3, lifeSteal: .15f, critMult: 4),
                    new("soulrend", "Soul Rend", new(4, 4), DamageType.Necrotic, 5, critMult: 3, lifeSteal: .25f)
                }, "A longsword, made for revealing dark truths.", 8, sellValue: 125, color: "khaki")
            },
            { "druidaxe", new Weapon("druidaxe", "Druid's Axe", new Attack[]
                {
                    new("slash", "Slash", new(3, 4, 1), DamageType.Slashing, 2, critThreshold: 18),
                    new("downward", "Downward Blow", new(10, modifier: 2), DamageType.Slashing, 4, critThreshold: 18, critMult: 4),
                    new("fungal", "Fungal Release", new(6, 3), DamageType.Poison, 5, atkBonus: 5)
                }, "A large axe, wrapped in fungal growths.", 8, sellValue: 125, color: "darkseagreen")
            },
            { "mindbreaker", new Weapon("mindbreaker", "Mindbreaker", new Attack[]
                {
                    new("stab", "Stab", new(12, 2), DamageType.Psychic, 2, atkBonus: -2),
                    new("execute", "Execute", new(12, modifier: 10), DamageType.Slashing, 5, atkBonus: 3, critThreshold: 15, critMult: 1.5f),
                    new("mindshatter", "Mind Shatter", new(6, 4), DamageType.Aberrant, 5, atkBonus: -2, critThreshold: 18, critMult: 3)
                }, "A slim knife that seems eerily blurry.", 2, sellValue: 175, color: "plum")
            },

            //Armors
            { "peasantclothes", new Armor("peasantclothes", "Peasant Clothes", 5, 10, 0, "A ragged set of clothes.")},
            { "breastplate", new Armor("breastplate", "Breastplate", 15, 100, 3, "A protective metal breastplate.",
                resistances: new()
                {
                    { DamageType.Slashing, 1 },
                    { DamageType.Piercing, 1 }
                }    
            )},
            { "chainmail", new Armor("chainmail", "Chainmail", 20, 125, 5, "A chain shirt.",
                resistances: new()
                {
                    { DamageType.Bludgeoning, 2 },
                }
            )},
            { "darkcloak", new Armor("darkcloak", "Dark Cloak", 10, 125, 3, "A shadowy cloak.", color: "darkslategrey",
                resistances: new()
                {
                    { DamageType.Necrotic, 3 },
                    { DamageType.Piercing, 1 },
                    { DamageType.Psychic, 2 },
                }
            )},
            { "darksteelbreastplate", new Armor("darksteelbreastplate", "Darksteel Breastplate", 5, 155, 3, "A shadowy, dark breastplate.", color: "darkslategrey",
                resistances: new()
                {
                    { DamageType.Piercing, 1 },
                    { DamageType.Slashing, 1 },
                    { DamageType.Necrotic, 5 },
                    { DamageType.Psychic, 1 },
                    { DamageType.Cold, 2 },
                    { DamageType.Radiant, -2 }
                }
            )},
            { "clericarmor", new Armor("clericarmor", "Cleric's Armor", 15, 200, 5, "A lightly-painted breastplate, with a large holy symbol on the front.",
                color: "mistyrose",
                resistances: new()
                {
                    { DamageType.Piercing, 1 },
                    { DamageType.Slashing, 1 },
                    { DamageType.Necrotic, 5 },
                    { DamageType.Psychic, -1 },
                    { DamageType.Fire, 2 },
                    { DamageType.Radiant, 5 }
                }
            )},
            { "fungalarmor", new Armor("fungalarmor", "Fungal Embrace", 12, 200, 5, "A set of armor, made out of wood overtaken with fungus.",
                color: "darkseagreen",
                resistances: new()
                {
                    { DamageType.Piercing, 1 },
                    { DamageType.Fire, -2 },
                    { DamageType.Radiant, -2 },
                    { DamageType.Poison, 5 },
                    { DamageType.Necrotic, 3 }
                }
            )},

            //Consumables
            { "ale", new SimpleConsumable("ale", "Mug of Ale", 0.5f, (session) =>
                {
                    session?.Player?.Heal(2);
                }, "Drink", "A frothy mug of bitter ale.", sellValue : 2)
            },
            { "mushroom", new SimpleConsumable("mushroom", "Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(1);
                }, "Eat", "A small grey mushroom. Possibly edible.", sellValue : 1)
            },
            { "cookedmeat", new SimpleConsumable("cookedmeat", "Cooked Meat", 0.6f, (session) =>
                {
                    session?.Player?.Heal(3);
                }, "Eat", "A grilled hunk of meat.", sellValue : 8)
            },
            { "grilledmushroom", new SimpleConsumable("grilledmushroom", "Grilled Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(2);
                }, "Eat", "An aromatic grilled mushroom. Probably not dangerous.", sellValue : 6)
            },
            { "lesserhealingpotion", new SimpleConsumable("lesserhealingpotion", "Lesser Healing Potion", 0.5f, (session) =>
                {
                    session?.Player?.Heal(10);
                }, "Drink", "A blood-red potion. A faint sparkling can be seen inside.", sellValue : 25, color: "red")
            },
            { "healingpotion", new SimpleConsumable("healingpotion", "Healing Potion", 1f, (session) =>
                {
                    session?.Player?.Heal(25);
                }, "Drink", "A blood-red potion, with a few white clouds swirling around in it. A notable sparkling can be seen inside.", sellValue : 50, color: "red")
            },
            { "lesserstaminapotion", new SimpleConsumable("lesserstaminapotion", "Lesser Stamina Potion", 1f, (session) =>
                {
                    session?.Player?.RestoreStamina(10); //We can't use regular assignment here, so we call the method instead.
                }, "Drink", "A light-purple potion. Various chunks can be seen inside", sellValue : 50)
            },

            //Misc Items
            { "rottenflesh", new SimpleItem("rottenflesh", "Rotten Flesh", 0.5f, "Rotten, decaying flesh, crawling with maggots. You probably shouldn't touch it.", sellValue: 1) },
            { "bone", new SimpleItem("bone", "Bone", 1, "A durable, white (actually more grey) bone.", sellValue: 2) },
            { "slime", new SimpleItem("slime", "Slime", 0.25f, "A glob of greenish-grey goo. Icky.", sellValue: 1) },
            { "meat", new SimpleItem("meat", "Meat", 1, "Mostly edible carcass. Where it's from, no one knows (it's best not to care).", sellValue : 3) },
            { "ironore", new SimpleItem("ironore", "Iron Ore", 1, "A chunk of raw iron.", sellValue : 3) },
            { "coal", new SimpleItem("coal", "Coal", 1, "A chunk of coal, good for smelting.", sellValue : 4) },
            { "taintedflesh", new SimpleItem("taintedflesh", "Tainted Flesh", 1, "A hunk of corrupted meat.", sellValue : 7, color: "plum") },
            { "ironbar", new SimpleItem("ironbar", "Iron Bar", 1, "A rectangular bar of iron, ready for use.", sellValue : 10) },
            { "log", new SimpleItem("log", "Log", 2, "A cylindrical piece of wood.", sellValue : 4) },
            { "vampiricdust", new SimpleItem("vampiricdust", "Vampiric Dust", 0.15f, "A sprinkling of shimmery red dust.", sellValue: 8, color: "red") },
            { "bottle", new SimpleItem("bottle", "Empty Bottle", 0.2f, "An empty glass bottle.", sellValue: 4) },
            { "spore", new SimpleItem("spore", "Spore", 0.1f, "An loose fungal spore.", sellValue: 7, color: "darkseagreen") },
            { "shadowessence", new SimpleItem("shadowessence", "Shadow Essence", 0.5f, "A swirling shadow.", sellValue: 10, color: "darkslategrey") },
            { "cloth", new SimpleItem("cloth", "Cloth", .4f, "A sheet of thin material, useful for clothing.", sellValue: 5) },
            { "darksteel", new SimpleItem("darksteel", "Darksteel", 1f, "A shadowy metal bar, used for creating unholy weapons and armor.", sellValue: 20, color: "darkslategrey") },
            { "holyblood", new SimpleItem("holyblood", "Holy Blood", 0.5f, "A few drops of blood from a divine creature.", sellValue: 25, color: "crimson") },
            { "aberrantcluster", new SimpleItem("aberrantcluster", "Aberrant Cluster", 2f, "A chunk of unusual flesh. Seems to still be living.", sellValue: 30, color: "plum") },
        });

        public static T? Get<T>(string id) where T : Item
        {
            ITEMS.TryGetValue(id, out Item? item);
            return item as T;
        }

        public static Item? Get(string id)
        {
            return Get<Item>(id);
        }
    }
}