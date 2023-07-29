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
                    new("soulrend", "Soul Rend", new(4, 4), DamageType.Necrotic, 5, critMult: 3, lifeSteal: .15f)
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
            { "mindreaper", new Weapon("mindreaper", "Mindreaper", new Attack[]
                {
                    new("stab", "Stab", new(12, 3), DamageType.Aberrant, 2, atkBonus: -2),
                    new("execute", "Execute", new(20, modifier: 15), DamageType.Psychic, 5, atkBonus: 3, critThreshold: 13),
                    new("mindshatter", "Mind Shatter", new(8, 4), DamageType.Aberrant, 6, atkBonus: -3, critThreshold: 17, critMult: 4)
                }, "A wicked dagger that seems to be phasing in and out of existence.", 2.5f, sellValue: 350, color: "plum")
            },
            { "landslide", new Weapon("landslide", "Landslide", new Attack[]
                {
                    new("slam", "Slam", new(12, 3, 4), DamageType.Bludgeoning, 3, critThreshold: 17, critMult: 3),
                    new("groundpound", "Groudpound", new(6, 4), DamageType.Thunder, 6, atkBonus: 12, critThreshold: 18, critMult: 3),
                    new("power", "Power Strike", new(12, 8, 10), DamageType.Bludgeoning, 10, atkBonus: 5, critThreshold: 16, critMult: 4)
                }, "A massive warhammer, carved out of a single block of stone.", 25f, sellValue: 500, color: "slategrey")
            },
            { "flametrident", new Weapon("flametrident", "Flame", new Attack[]
                {
                    new("jab", "Jab", new(12, modifier: 4), DamageType.Fire, 1, atkBonus: 2),
                    new("puncture", "Puncture", new(12, 4), DamageType.Fire, 4, atkBonus: 3, critThreshold: 17),
                    new("impale", "Impale", new(12, 4, 12), DamageType.Fire, 7, atkBonus: -2, critThreshold: 17, critMult: 4)
                }, "A long, dark red trident. The ends glow white-hot.", 12f, sellValue: 500, color: "firebrick")
            },
            { "frostbite", new Weapon("frostbite", "Frostbite", new Attack[]
                {
                    new("jab", "Jab", new(12, modifier: 4), DamageType.Cold, 1, atkBonus: 13),
                    new("puncture", "Puncture", new(12, 4), DamageType.Cold, 3, atkBonus: 7, critThreshold: 19),
                    new("impale", "Impale", new(12, 4, 12), DamageType.Cold, 7, atkBonus: -4, critThreshold: 16, critMult: 4.5f)
                }, "A freezing-cold spear.", 8f, sellValue: 500, color: "mediumturquoise")
            },
            { "bloodripper", new Weapon("bloodripper", "Bloodripper", new Attack[]
                {
                    new("slice", "Slice", new(12, 2, 4), DamageType.Slashing, 4, lifeSteal: .4f, critThreshold: 15, critMult: 1.5f),
                    new("gore", "Gore", new(12, 3, 5), DamageType.Slashing, 6, lifeSteal: .7f),
                    new("behead", "Behead", new(20, 2), DamageType.Slashing, 8, critThreshold: 17)
                }, "A huge, cruel-looking greatsword.", 17f, sellValue: 600, color: "red")
            },
            { "endbringer", new Weapon("endbringer", "Endbringer", new Attack[]
                {
                    new("slash", "Slash", new(12, 3, 6), DamageType.Aberrant, 3, lifeSteal: .2f, critThreshold: 16, critMult: 1.75f),
                    new("impale", "Impale", new(12, 5, 12), DamageType.Aberrant, 7, atkBonus: -3, lifeSteal: .7f, critThreshold: 17, critMult: 2.5f),
                    new("aura", "Unnatural Aura", new(20, 3, 10), DamageType.Aberrant, 8, atkBonus: 15, critThreshold: 17, lifeSteal: .3f)
                }, "A twisted, ethereal longsword. Something about it is unnerving.", 13f, sellValue: 1000, color: "midnightblue")
            },
            { "volcano", new Weapon("volcano", "Volcano", new Attack[]
                {
                    new("shoot", "Shoot", new(12, 4), DamageType.Fire, 3, critThreshold: 16, critMult: 2.5f, lifeSteal: .2f),
                    new("power", "Power Shot", new(12, 5, 12), DamageType.Fire, 7, atkBonus: -3, critThreshold: 14, critMult: 3.5f),
                    new("snipe", "Snipe", new(20, 4, 10), DamageType.Infernal, 8, atkBonus: -5, critThreshold: 17, critMult: 2.5f)
                }, "A hot red greatbow, made for punching though armor.", 12f, sellValue: 1000, color: "maroon")
            },
            { "shadowdeath", new Weapon("shadowdeath", "Shadowdeath", new Attack[]
                {
                    new("slash", "Slash", new(12, 3, 6), DamageType.Cold, 3, atkBonus: 2, lifeSteal: .3f, critThreshold: 16, critMult: 1.75f),
                    new("impale", "Impale", new(12, 5, 12), DamageType.Necrotic, 7, atkBonus: -2, lifeSteal: .6f, critThreshold: 17, critMult: 2.5f),
                    new("aura", "Aura of Darkness", new(20, 3, 10), DamageType.Necrotic, 8, atkBonus: 18, critThreshold: 16, lifeSteal: .3f)
                }, "A shadowy katana, slices through armor like water.", 12f, sellValue: 1000, color: "darkslategrey")
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
            { "platearmor", new Armor("platearmor", "Platearmor", 25, 350, 8, "A set of metal plates that covers the wearer.",
                resistances: new()
                {
                    { DamageType.Slashing, 3 },
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
                    { DamageType.Radiant, 5 },
                    { DamageType.Infernal, -3 }
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
            { "frostwall", new Armor("frostwall", "Frostwall", 15, 400, 7, "A set of full plate, but made out of never-melting ice.",
                color: "mediumturquoise",
                resistances: new()
                {
                    { DamageType.Slashing, 2 },
                    { DamageType.Fire, -3 },
                    { DamageType.Radiant, -2 },
                    { DamageType.Poison, 5 },
                    { DamageType.Cold, 5 }
                }
            )},
            { "livingstonearmor", new Armor("livingstonearmor", "Living Stone Armor", 35, 450, 9, "Armor that encases the wearer in stone.",
                color: "slategrey",
                resistances: new()
                {
                    { DamageType.Slashing, 5 },
                    { DamageType.Bludgeoning, 5 },
                    { DamageType.Piercing, 5 },
                    { DamageType.Fire, 3 },
                    { DamageType.Poison, 2 },
                    { DamageType.Cold, -3 }
                },
                strength: 2
            )},
            { "pyromancercloak", new Armor("pyromancercloak", "Pyromancer's Cloak", 5, 400, 4, "A flaming-red cloak that flickers in the light.",
                color: "firebrick",
                resistances: new()
                {
                    { DamageType.Slashing, 2 },
                    { DamageType.Bludgeoning, 2 },
                    { DamageType.Piercing, 2 },
                    { DamageType.Fire, 10 },
                    { DamageType.Radiant, 4 },
                    { DamageType.Cold, -3 }
                },
                agility: 2,
                intelligence: 2
            )},
            { "truepyromancercloak", new Armor("truepyromancercloak", "True Pyromancer's Cloak", 9, 800, 9, "A blazing cloak that banishes the dark. Smoke floats off the ends.",
                color: "maroon",
                resistances: new()
                {
                    { DamageType.Slashing, 2 },
                    { DamageType.Bludgeoning, 2 },
                    { DamageType.Piercing, 2 },
                    { DamageType.Fire, 25 },
                    { DamageType.Infernal, 10 },
                    { DamageType.Radiant, -5 },
                    { DamageType.Cold, -5 }
                },
                strength: 2,
                agility: 5,
                endurance: 1,
                intelligence: 5,
                wisdom: 3
            )},
            { "glacierarmor", new Armor("glacierarmor", "Glacier", 20, 400, 7, "A set of immovable armor.",
                color: "mediumturquoise",
                resistances: new()
                {
                    { DamageType.Slashing, 2 },
                    { DamageType.Bludgeoning, 1 },
                    { DamageType.Piercing, 1 },
                    { DamageType.Fire, -3 },
                    { DamageType.Cold, 15 },
                    { DamageType.Poison, 3 }
                }
            )},
            { "apprenticerobe", new Armor("apprenticerobe", "Apprentice's Robe", 5, 300, 2, "A plain brown robe, worn by apprentice scholars.",
                color: "wheat",
                intelligence: 2,
                wisdom: 5
            )},
            { "journeymanrobe", new Armor("journeymanrobe", "Journeyman's Robe", 6, 600, 3, "A worn grey robe, worn by travelling sages.",
                color: "wheat",
                intelligence: 4,
                wisdom: 10
            )},
            { "sagerobe", new Armor("sagerobe", "Sage's Robe", 8, 1000, 3, "A grey robe with some minor embellishments.",
                color: "wheat",
                intelligence: 6,
                wisdom: 15
            )},
            { "hellplate", new Armor("hellplate", "Hellplate", 25, 800, 11, "A set of molten plate armor.",
                color: "maroon",
                resistances: new()
                {
                    { DamageType.Slashing, 3 },
                    { DamageType.Bludgeoning, 1 },
                    { DamageType.Piercing, 1 },
                    { DamageType.Fire, 15 },
                    { DamageType.Infernal, 10 },
                    { DamageType.Radiant, -4 },
                    { DamageType.Cold, -4 }
                },
                strength: 4,
                constitution: 4,
                agility: 5,
                endurance: 3
            )},
            { "shadowplate", new Armor("shadowplate", "Shadowplate", 5, 800, 8, "A shadowy set of light armor.",
                color: "darkslategrey",
                resistances: new()
                {
                    { DamageType.Slashing, -1 },
                    { DamageType.Bludgeoning, 1 },
                    { DamageType.Piercing, 1 },
                    { DamageType.Fire, -5 },
                    { DamageType.Infernal, 3 },
                    { DamageType.Radiant, -5 },
                    { DamageType.Cold, 5 }
                },
                dexterity: 4,
                agility: 5,
                endurance: 3
            )},
            { "unnaturalshield", new Armor("unnaturalshield", "Unnatural Shield", 12, 1000, 12, "A set of tentacled, sinewy armor.",
                color: "purple",
                resistances: new()
                {
                    { DamageType.Slashing, 4 },
                    { DamageType.Bludgeoning, 2 },
                    { DamageType.Piercing, 2 },
                    { DamageType.Necrotic, 6 },
                    { DamageType.Infernal, 6 },
                    { DamageType.Radiant, 6 },
                    { DamageType.Cold, 5 },
                    { DamageType.Aberrant, 15 }
                },
                dexterity: 6,
                agility: 6,
                endurance: 3,
                intelligence: 10,
                wisdom: 10,
                charisma: 3
            )},
            { "radiance", new Armor("radiance", "Radiance", 20, 1000, 13, "A set of divinely blessed plate armor.",
                color: "lavenderblush",
                resistances: new()
                {
                    { DamageType.Slashing, 4 },
                    { DamageType.Bludgeoning, 2 },
                    { DamageType.Piercing, 2 },
                    { DamageType.Necrotic, -5 },
                    { DamageType.Infernal, 6 },
                    { DamageType.Radiant, 15 },
                    { DamageType.Fire, 10 },
                    { DamageType.Cold, 5 }
                },
                dexterity: 4,
                agility: 4,
                endurance: 7,
                wisdom: 5,
                charisma: 5
            )},

            //Consumables
            { "ale", new SimpleConsumable("ale", "Mug of Ale", 0.5f, (session) =>
                {
                    session?.Player?.Heal(3);
                }, "Drink", "A frothy mug of bitter ale. Heals 3 health.", sellValue: 10)
            },
            { "mushroom", new SimpleConsumable("mushroom", "Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(1);
                }, "Eat", "A small grey mushroom. Possibly edible.", sellValue: 1)
            },
            { "cookedmeat", new SimpleConsumable("cookedmeat", "Cooked Meat", 0.6f, (session) =>
                {
                    session?.Player?.Heal(10);
                }, "Eat", "A grilled hunk of meat. Heals 10 health.", sellValue: 30)
            },
            { "grilledmushroom", new SimpleConsumable("grilledmushroom", "Grilled Mushroom", 0.1f, (session) =>
                {
                    session?.Player?.Heal(10);
                }, "Eat", "An aromatic grilled mushroom. Probably not dangerous. Heals 10 health.", sellValue: 10)
            },
            { "lesserhealingpotion", new SimpleConsumable("lesserhealingpotion", "Lesser Healing Potion", 0.5f, (session) =>
                {
                    session?.Player?.Heal(25);
                }, "Drink", "A blood-red potion. A faint sparkling can be seen inside. Heals 25 health.", sellValue: 25, color: "red")
            },
            { "healingpotion", new SimpleConsumable("healingpotion", "Healing Potion", 1f, (session) =>
                {
                    session?.Player?.Heal(50);
                }, "Drink", "A blood-red potion, with a few white clouds swirling around in it. A notable sparkling can be seen inside. Heals 50 health.", sellValue: 50, 
                color: "red")
            },
            { "lesserstaminapotion", new SimpleConsumable("lesserstaminapotion", "Lesser Stamina Potion", 1f, (session) =>
                {
                    session?.Player?.RestoreStamina(20); //We can't use regular assignment here, so we call the method instead.
                }, "Drink", "A light-purple potion. Various chunks can be seen inside. Grants 20 stamina.", sellValue: 50)
            },
            { "returnscroll", new SimpleConsumable("returnscroll", "Scroll of Return", 2f, (session) =>
                {
                    Player? player = session?.Player;
                    Location.Get(player?.resetLocation ?? "dungeonentrance")?.Enter(player, player?.Location ?? null);
                }, "Use", "A long scroll, said to be able to teleport the reader back to a safe location.", sellValue: 75, color: "wheat")
            },
            { "returnamulet", new SimpleConsumable("returnamulet", "Amulet of Return", 2f, (session) =>
                {
                    Player? player = session?.Player;
                    Location.Get(player?.resetLocation ?? "dungeonentrance")?.Enter(player, player?.Location ?? null);
                }, "Use", "An amulet with a large amethyst, said to be able to teleport the wearer back to a safe location.", sellValue: 500, color: "purple", uses: 10)
            },
            { DungeonTeleportationScroll.GetId(0), new DungeonTeleportationScroll(0) },
            { DungeonTeleportationScroll.GetId(1), new DungeonTeleportationScroll(1) },
            { DungeonTeleportationScroll.GetId(2), new DungeonTeleportationScroll(2) },
            { DungeonTeleportationScroll.GetId(3), new DungeonTeleportationScroll(3) },
            { DungeonTeleportationScroll.GetId(4), new DungeonTeleportationScroll(4) },
            { DungeonTeleportationScroll.GetId(5), new DungeonTeleportationScroll(5) },
            { DungeonTeleportationScroll.GetId(6), new DungeonTeleportationScroll(6) },
            { DungeonTeleportationScroll.GetId(7), new DungeonTeleportationScroll(7) },
            { DungeonTeleportationScroll.GetId(8), new DungeonTeleportationScroll(8) },
            { DungeonTeleportationScroll.GetId(9), new DungeonTeleportationScroll(9) },

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
            { "ember", new SimpleItem("ember", "Ember", 1f, "A softly-glowing, still hot ember.", sellValue: 20, color: "firebrick") },
            { "frostshard", new SimpleItem("frostshard", "Frostshard", 1f, "A crystal of perma-frozen ice.", sellValue: 20, color: "mediumturquoise") },
            { "ice", new SimpleItem("ice", "Ice", 1f, "A small chunk of ice.", sellValue: 10, color: "mediumturquoise") },
            { "livingstone", new SimpleItem("livingstone", "Living Stone", 5f, "A rock carved out of an earthen creature.", sellValue: 60, color: "slategrey") },
            { "firesteel", new SimpleItem("firesteel", "Firesteel", .8f, "A flickering, red ingot of a warm metal. Hot to the touch.", sellValue: 40, color: "firebrick") },
            { "brimstone", new SimpleItem("brimstone", "Brimstone", 1.2f, "A dark reddish-black rock. Painful to the touch", sellValue: 60, color: "maroon") },
            { "hellsteel", new SimpleItem("hellsteel", "Hellsteel", 1.5f, "A molten bar of infernal steel. Hurts to touch, smell, or just generally be around", sellValue: 60, 
                color: "maroon") },
            { "otherworldlyshard", new SimpleItem("otherwordlyshard", "Otherworldly Shard", 0.1f, "A fragment of another universe. Glimpses of other realities can be seen " +
                "flittering in and out", sellValue: 120, color: "purple") },
            { "umbralvapor", new SimpleItem("umbralvapor", "Umbral Vapor", 0.1f, "A few wisps of barely-visible vapor", sellValue: 90, color: "darkslategrey") },
        });

        public static T? Get<T>(string id) where T : Item
        {
            try
            {
                ITEMS.TryGetValue(id, out Item? item);
                return item as T;
            } catch (Exception e)
            {
                Utils.Log($"Error getting item {id}");
                Utils.Log(e);
                return null;
            }
        }

        public static Item? Get(string id)
        {
            return Get<Item>(id);
        }
    }
}