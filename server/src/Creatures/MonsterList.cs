using Items;
using ItemTypes;

namespace Creatures
{
    public static class MonsterList
    {

        public delegate Creature MonsterEntry(bool actual = true);

        public static readonly Table<MonsterEntry> MONSTERS = new(

            //Zombie
            new(1, (actual) => new SimpleMonster("zombie", "Zombie", 5, new("", "",
                new Attack[]
                {
                    new("bite", "Bite", 6, DamageType.Piercing),
                    new("punch", "Punch", 4, DamageType.Bludgeoning, atkBonus: 3),
                    new("bodyslam", "Body Slam", 4, DamageType.Bludgeoning, critThreshold: 18)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("rottenflesh"))
                ),
                xp: 20,
                scaleTableWeight: (floor) =>
                {
                    return 1f - floor.temperature - floor.Depth * 0.4f;
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Skeleton
            new(1, (actual) => new SimpleMonster("skeleton", "Skeleton", 5, ItemList.Get<Weapon>("spear"),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("bone"))
                ), minDrops: 1, maxDrops: 3,
                xp: 25,
                scaleTableWeight: (floor) =>
                {
                    return 1f - floor.Depth * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Piercing, 3 },
                    { DamageType.Bludgeoning, -2 },
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Giant Rat
            new(1, (actual) => new SimpleMonster("giantrat", "Giant Rat", 4, new("bite", "Bite", 4, DamageType.Piercing),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat"))
                ),
                xp: 15,
                scaleTableWeight: (floor) =>
                {
                    return 1.2f - floor.Depth * 0.4f - Math.Abs(floor.temperature);
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Slime
            new(1, (actual) => new SimpleMonster("slime", "Slime", 7, new("ooze", "Ooze", 4, DamageType.Poison, abilityScore: AbilityScore.Constitution),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("slime"))
                ), minDrops: 1, maxDrops: 2,
                xp: 25,
                scaleTableWeight: (floor) =>
                {
                    return 1f - floor.Depth * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Piercing, 4 },
                    { DamageType.Slashing, 2 },
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Troll
            new(1, (actual) => new SimpleMonster("troll", "Troll", 15, new("punch", "Punch", 8, DamageType.Bludgeoning),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("taintedflesh"))
                ), minDrops: 1, maxDrops: 2,
                xp: 75,
                onTick: (data) =>
                {
                    if (Utils.tickCount % 2 == 0)
                        data.self.Heal(1);
                },
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 2) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Fire, -5 }
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Blood Slime
            new(1, (actual) => new SimpleMonster("bloodslime", "Blood Slime", 20,
                new(new Attack[] { new("ooze", "Vampiric Ooze", 6, DamageType.Poison, dmgAbilityScore: AbilityScore.Constitution, lifeSteal: .35f) }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("slime")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("vampiricdust"))
                ), minDrops: 1, maxDrops: 2,
                xp: 50,
                scaleTableWeight: (floor) =>
                {
                    return 1f - Math.Abs(floor.Depth - 2) * 0.4f + floor.arcane - floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Piercing, 5 },
                    { DamageType.Slashing, 3 },
                },
                constitution: 1,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Twig Blight
            new(1, (actual) => new SimpleMonster("twigblight", "Twig Blight", 4, new("stab", "Stab", 6, DamageType.Piercing),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("log"))
                ), minDrops: 1, maxDrops: 2,
                xp: 20,
                scaleTableWeight: (floor) =>
                {
                    return 1f - floor.Depth * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Fire, -2 }
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Oak Blight
            new(1, (actual) => new SimpleMonster("oakblight", "Oak Blight", 8, new("stab", "Stab", 6, DamageType.Piercing),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("log"))
                ), minDrops: 2, maxDrops: 3,
                xp: 40,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - floor.Depth * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Fire, -1 }
                },
                strength: 1,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Fungus Zombie
            new(1, (actual) => new SimpleMonster("funguszombie", "Fungus Zombie", 12, new("punch", "Punch", 8, DamageType.Poison),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("taintedflesh")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("spore"))
                ), minDrops: 2, maxDrops: 3,
                xp: 60,
                scaleTableWeight: (floor) =>
                {
                    return 1.2f - Math.Abs(floor.Depth - 2) * 0.3f - Math.Abs(floor.temperature);
                },
                resistances: new()
                {
                    { DamageType.Fire, -1 }
                },
                strength: 3,
                dexterity: -1,
                constitution: 1,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Fungus Troll
            new(100, (actual) => new SimpleMonster("fungustroll", "Fungus Troll", 25,
                new(new Attack[]
                {
                    new("slam", "Slam", 12, DamageType.Bludgeoning, critThreshold: 19),
                    new("fungusbreath", "Fungus Breath", 12, DamageType.Poison, atkBonus: 5)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("taintedflesh")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("spore"))
                ), minDrops: 2, maxDrops: 3,
                xp: 100,
                onTick: (data) =>
                {
                    if (Utils.tickCount % 2 == 0)
                        data.self.Heal(2);
                },
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 3) * 0.4f - Math.Abs(floor.temperature);
                },
                resistances: new()
                {
                    { DamageType.Fire, -3 }
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Ogre
            new(1, (actual) => new SimpleMonster("ogre", "Ogre", 25,
                new(new Attack[]
                {
                    new("slam", "Slam", 12, DamageType.Bludgeoning, critThreshold: 18),
                    new("groundpound", "Ground Pound", 8, DamageType.Thunder, critThreshold: 16)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.5f, () => new("boneclub"))
                ), minDrops: 1, maxDrops: 2,
                xp: 125,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 3) * 0.4f;
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Shadowcat
            new(1, (actual) => new SimpleMonster("shadowcat", "Shadowcat", 20,
                new(new Attack[]
                {
                    new("pounce", "Pounce", 12, DamageType.Bludgeoning, critThreshold: 19, critMult: 3),
                    new("slash", "Slash", 10, DamageType.Necrotic, critThreshold: 19)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("shadowessence"))
                ), minDrops: 2, maxDrops: 3,
                xp: 100,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 3) * 0.4f;
                },
                agility: 4,
                dexterity: 2,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Zombie Slime
            new(1, (actual) => new SimpleMonster("zombieslime", "Zombie Slime", 20,
                new(new Attack[]
                {
                    new("ooze", "Ooze", 12, DamageType.Poison, critThreshold: 19, dmgAbilityScore: AbilityScore.Constitution, lifeSteal: .25f),
                    new("necroticgoop", "Necrotic Goop", 12, DamageType.Necrotic, critThreshold: 18, dmgAbilityScore: AbilityScore.Constitution,lifeSteal: .4f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("slime")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("shadowessence"))
                ), minDrops: 2, maxDrops: 3,
                xp: 80,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 3) * 0.4f + floor.arcane - floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Poison, 1 },
                    { DamageType.Necrotic, 4 },
                    { DamageType.Radiant, -3 }
                },
                constitution: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Heavenly Defender
            new(1, (actual) => new SimpleMonster("heavenlydefender", "Heavenly Defender", 50,
                new(new Attack[]
                {
                    new("smite", "Smite", 12, DamageType.Radiant, atkBonus: 5, critThreshold: 18, dmgAbilityScore: AbilityScore.Wisdom),
                    new("longsword", "Longsword", 12, DamageType.Slashing, critThreshold: 17),
                    new("divineintervention", "Divine Intervention", 12, DamageType.Radiant, atkBonus: 6, critThreshold: 17, critMult: 3)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("holyblood")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("longsword"))
                ), minDrops: 1, maxDrops: 2,
                xp: 130,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 4) * 0.4f + floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, -3 },
                    { DamageType.Infernal, -3 },
                    { DamageType.Fire, 2 },
                    { DamageType.Radiant, 12 },
                    { DamageType.Psychic, 1 },
                    { DamageType.Slashing, 2 },
                    { DamageType.Bludgeoning, 2 },
                    { DamageType.Piercing, 2}
                },
                strength: 4,
                dexterity: 2,
                wisdom: 4,
                agility: 3,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Lesser Angel
            new(1, (actual) => new SimpleMonster("lesserangel", "Lesser Angel", 30,
                new(new Attack[]
                {
                    new("smite", "Smite", 10, DamageType.Radiant, atkBonus: 2, critThreshold: 19, dmgAbilityScore: AbilityScore.Wisdom),
                    new("mace", "Mace", 8, DamageType.Slashing, critThreshold: 18, critMult: 3),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("holyblood")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("mace"))
                ), minDrops: 1, maxDrops: 2,
                xp: 80,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 4) * 0.3f + floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, -5 },
                    { DamageType.Infernal, -5 },
                    { DamageType.Radiant, 6 },
                    { DamageType.Fire, 1 }
                },
                strength: 3,
                dexterity: 2,
                wisdom: 3,
                agility: 2,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Mutant Troll
            new(1, (actual) => new SimpleMonster("mutanttroll", "Mutant Troll", 70,
                new(new Attack[] {
                    new("punch", "Punch", 12, DamageType.Bludgeoning, atkBonus: 7, critThreshold: 18),
                    new("bite", "Bite", 10, DamageType.Poison, atkBonus: -2, critThreshold: 15),
                    new("bodyslam", "Bodyslam", new(6, 3), DamageType.Bludgeoning, critThreshold: 18, critMult: 3)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("taintedflesh")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("aberrantcluster"))
                ), minDrops: 2, maxDrops: 3,
                xp: 200,
                onTick: (data) =>
                {
                    if (Utils.tickCount % 2 == 0)
                        data.self.Heal(3);
                },
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 5) * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Poison, 4 },
                    { DamageType.Necrotic, 2 },
                    { DamageType.Radiant, -2 },
                    { DamageType.Fire, -1 }
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Living Tentacle
            new(1, (actual) => new SimpleMonster("livingtentacle", "Living Tentacle", 35,
                new(new Attack[] {
                    new("slap", "Slap", 10, DamageType.Bludgeoning),
                    new("choke", "choke", 8, DamageType.Bludgeoning, critThreshold: 18, critMult: 5)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("aberrantcluster"))
                ), minDrops: 2, maxDrops: 3,
                xp: 80,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 4) * 0.3f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Poison, 6 },
                    { DamageType.Necrotic, 3 },
                    { DamageType.Radiant, -3 }
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //SPECIAL: Mysterious Trader - Not a monster
            new(0.25f, (actual) => new Trader("mysterioustrader", "Mysterious Trader", "I bring you wonders from the beyond", RecipeLists.GenMysteriousTraderInventory(),
                canSell: false, nameColor: "blue")),

            //Fire Elemental
            new(1, (actual) => new SimpleMonster("firelemental", "Fire Elemental", 65,
                new(new Attack[] {
                    new("sear", "Sear", 20, DamageType.Fire, critThreshold: 17, lifeSteal: .4f),
                    new("ignite", "Ignite", new(12, 2), DamageType.Fire, atkBonus: 3, critThreshold: 18, critMult: 3, lifeSteal: .6f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("coal")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("ember"))
                ), minDrops: 2, maxDrops: 3,
                xp: 220,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 6) * 0.4f + floor.temperature;
                },
                resistances: new()
                {
                    { DamageType.Poison, 20 },
                    { DamageType.Fire, 20 },
                    { DamageType.Cold, -5 }
                },
                dexterity: 5,
                agility: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Emberbug
            new(1, (actual) => new SimpleMonster("emberbug", "Ember Bug", 60,
                new(new Attack[] {
                    new("bite", "Bite", new(8, 2, 5), DamageType.Fire, critThreshold: 17, critMult: 4, lifeSteal: .4f),
                    new("sear", "Sear", new(8, 2), DamageType.Fire, atkBonus: 8, critThreshold: 18, critMult: 3, lifeSteal: .3f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("coal")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("ember")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat"))
                ), minDrops: 1, maxDrops: 2,
                xp: 200,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 6) * 0.4f + floor.temperature;
                },
                resistances: new()
                {
                    { DamageType.Fire, 20 },
                    { DamageType.Cold, -5 }
                },
                dexterity: 5,
                agility: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Frost Elemental
            new(1, (actual) => new SimpleMonster("frostelemental", "Frost Elemental", 80,
                new(new Attack[] {
                    new("slam", "Slam", new(8, 2, 5), DamageType.Cold, critThreshold: 17, critMult: 4),
                    new("freezingbreath", "Freezing Breath", new(6, 4), DamageType.Cold, atkBonus: 8, critThreshold: 18, critMult: 3, lifeSteal: .3f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("frostshard")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("ice"))
                ), minDrops: 2, maxDrops: 3,
                xp: 250,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 6) * 0.4f - floor.temperature;
                },
                resistances: new()
                {
                    { DamageType.Fire, -5 },
                    { DamageType.Cold, 20 }
                },
                onTick: (data) =>
                {
                    if (data.tickCount % 5 == 0)
                    {
                        Location? location = data.self.Location;
                        if (location != null)
                        {
                            foreach (Creature creature in location.creatures)
                                if (creature != data.self && creature is Player)
                                    creature.TakeDamage(10, DamageType.Cold, data.self, true);
                        }
                    }
                },
                dexterity: 5,
                agility: 3,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Frost Golem
            new(1, (actual) => new SimpleMonster("frostelemental", "Frost Elemental", 150,
                new(new Attack[] {
                    new("slam", "Slam", new(8, 3, 7), DamageType.Cold, critThreshold: 16, critMult: 4),
                    new("freezingbreath", "Freezing Breath", new(6, 6), DamageType.Cold, atkBonus: 12, critThreshold: 17, critMult: 3.5f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1.5f, () => new("frostshard")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1.5f, () => new("ice")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("frostwall"))
                ), minDrops: 3, maxDrops: 4,
                xp: 350,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 7) * 0.4f - floor.temperature;
                },
                resistances: new()
                {
                    { DamageType.Fire, -5 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Cold, 30 },
                    { DamageType.Bludgeoning, 5 },
                    { DamageType.Slashing, 5 },
                    { DamageType.Piercing, 5 },
                    { DamageType.Poison, 30 }
                },
                onTick: (data) =>
                {
                    if (data.tickCount % 4 == 0)
                    {
                        Location? location = data.self.Location;
                        if (location != null)
                        {
                            foreach (Creature creature in location.creatures)
                                if (creature != data.self && creature is Player)
                                    creature.TakeDamage(10, DamageType.Cold, data.self, true);
                        }
                    }
                },
                dexterity: 3,
                agility: 1,
                strength: 3,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Tentacled Horror
            new(1, (actual) => new SimpleMonster("tentacledhorror", "Tentacled Horror", 120,
                new(new Attack[] {
                    new("strangle", "Strangle", new(10, 4, 10), DamageType.Bludgeoning, atkBonus: -5, critMult: 2.5f),
                    new("aberrant", "Aberrant Aura", new(3, 6), DamageType.Aberrant, atkBonus: 12, critThreshold: 16, critMult: 4f),
                    new("shatter", "Mind Shatter", new(20, 2), DamageType.Aberrant, critThreshold: 13, critMult: 2.5f),
                    new("presence", "Unnerving Presence", new(10, 2), DamageType.Psychic, critThreshold: 8, critMult: 1.5f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1.5f, () => new("aberrantcluster")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1.5f, () => new("taintedflesh")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("mindreaper"))
                ), minDrops: 3, maxDrops: 4,
                xp: 350,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 7) * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -10 },
                    { DamageType.Psychic, 30 },
                    { DamageType.Aberrant, 30 },
                    { DamageType.Poison, 30 }
                },
                dexterity: 5,
                agility: 3,
                strength: 2,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Stone Golem
            new(1, (actual) => new SimpleMonster("stonegolem", "Stone Golem", 200,
                new(new Attack[] {
                    new("slam", "Slam", new(12, 3, 5), DamageType.Bludgeoning, critThreshold: 18, critMult: 3),
                    new("earthquake", "Earthquake", new(6, 6), DamageType.Bludgeoning, atkBonus: 12, critThreshold: 17, critMult: 3.5f),
                    new("stomp", "Stomp", new(8, 6), DamageType.Bludgeoning, atkBonus: -5, critMult: 5)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(2f, () => new("livingstone")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("landslide"))
                ), minDrops: 2, maxDrops: 3,
                xp: 350,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 7) * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Radiant, 10 },
                    { DamageType.Necrotic, 10 },
                    { DamageType.Psychic, 30 },
                    { DamageType.Aberrant, 5 },
                    { DamageType.Poison, 30 },
                    { DamageType.Cold, 30 },
                    { DamageType.Fire, 30 }
                },
                strength: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Lost Paladin
            new(1, (actual) => new SimpleMonster("lostpaladin", "Lost Paladin", 125,
                new(new Attack[] {
                    new("slash", "Slash", new(12, 3, 5), DamageType.Bludgeoning, atkBonus: 5, critThreshold: 18, critMult: 3),
                    new("smite", "Smite", new(10, 4, 10), DamageType.Radiant, atkBonus: 12, critThreshold: 17, critMult: 3.5f, lifeSteal: .4f),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(3f, () => new("holyblood")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("longsword")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("breastplate"))
                ), minDrops: 1, maxDrops: 2,
                xp: 250,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 7) * 0.4f + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Radiant, 10 },
                    { DamageType.Necrotic, -10 },
                    { DamageType.Infernal, -10 },
                    { DamageType.Cold, 5 }
                },
                strength: 5,
                dexterity: 5,
                agility: 4,
                defense: 7,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Undead Adventurer
            new(1, (actual) => new SimpleMonster("undeadadventurer", "Undead Adventurer", 145,
                new Weapon[] {
                    (Weapon)ItemList.Get("mindbreaker"),
                    (Weapon)ItemList.Get("hereticsword"),
                    (Weapon)ItemList.Get("hereticsword"),
                    (Weapon)ItemList.Get("landslide"),
                    (Weapon)ItemList.Get("druidaxe")
                }[Utils.RandInt(5)],
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(3f, () => new("coin", Utils.RandInt(50, 100))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("longsword")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("breastplate"))
                ), minDrops: 1, maxDrops: 2,
                xp: 275,
                scaleTableWeight: (floor) =>
                {
                    return 0.6f + (floor.Depth - 6) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -5 },
                    { DamageType.Necrotic, 5 },
                    { DamageType.Cold, 5 }
                },
                strength: Utils.RandInt(10),
                dexterity: Utils.RandInt(10),
                agility: Utils.RandInt(10),
                constitution: Utils.RandInt(10),
                defense: Utils.RandInt(10),
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Flame Demon
            new(1, (actual) => new SimpleMonster("flamedemon", "Flame Demon", 175,
                new(new Attack[]
                {
                    new("torch", "Torch", new(12, 3, 8), DamageType.Fire, critThreshold: 16, lifeSteal: .2f),
                    new("infernal", "Infernal Touch", new(20, 2), DamageType.Infernal, atkBonus: -5, critThreshold: 18, critMult: 3.5f),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(3f, () => new("ember", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("brimstone"))
                ), minDrops: 1, maxDrops: 2,
                xp: 325,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 8) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -10 },
                    { DamageType.Infernal, 5 },
                    { DamageType.Fire, 10 }
                },
                strength: 2,
                dexterity: 3,
                agility: 6,
                defense: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Hell Ogre
            new(1, (actual) => new SimpleMonster("hellogre", "Hell Ogre", 250,
                new(new Attack[]
                {
                    new("torch", "Torch", new(12, 3, 8), DamageType.Fire, critThreshold: 16),
                    new("slam", "Slam", new(20, 2, 10), DamageType.Bludgeoning, critThreshold: 19, critMult: 3.75f),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(3f, () => new("ember", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("brimstone")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.5f, () => new("boneclub"))
                ), minDrops: 2, maxDrops: 3,
                xp: 425,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 9) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -10 },
                    { DamageType.Infernal, 5 },
                    { DamageType.Fire, 10 }
                },
                strength: 5,
                dexterity: 4,
                agility: -2,
                defense: 7,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Brimstone Cat
            new(1, (actual) => new SimpleMonster("brimstonecat", "Brimstone Cat", 220,
                new(new Attack[]
                {
                    new("slash", "Slash", new(8, 4, 8), DamageType.Slashing, atkBonus: 12, critThreshold: 16),
                    new("pounce", "Pounce", new(20, 3), DamageType.Bludgeoning, critThreshold: 17, critMult: 3.5f),
                    new("bite", "Flame Bite", new(4, 12), DamageType.Infernal, critThreshold: 18, critMult: 3.5f),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("ember", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.75f, () => new("brimstone")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("shadowessence"))
                ), minDrops: 2, maxDrops: 3,
                xp: 415,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 8) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -10 },
                    { DamageType.Infernal, 5 },
                    { DamageType.Fire, 10 },
                    { DamageType.Cold, 3 }
                },
                strength: 5,
                dexterity: 7,
                agility: 6,
                defense: 5,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Corrupted Templar
            new(1, (actual) => new SimpleMonster("corruptedtemplar", "Corrupted Templar", 300,
                (Weapon)ItemList.Get("endbringer"),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("aberrantcluster", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.75f, () => new("platearmor")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.75f, () => new("otherworldlyshard"))
                ), minDrops: 2, maxDrops: 3,
                xp: 475,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 9) * 0.4f;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -5 },
                    { DamageType.Infernal, 5 },
                    { DamageType.Fire, 7 },
                    { DamageType.Cold, 7 },
                    { DamageType.Necrotic, 7 },
                    { DamageType.Aberrant, 15 }
                },
                strength: 7,
                dexterity: 7,
                agility: 7,
                endurance: 7,
                defense: 9,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Brimstone Golem
             new(1, (actual) => new SimpleMonster("brimstonegolem", "Brimstone Golem", 300,
                new(new Attack[] {
                    new("slam", "Slam", new(12, 4, 5), DamageType.Bludgeoning, critThreshold: 18, critMult: 3),
                    new("hellquake", "Hellquake", new(8, 6), DamageType.Fire, atkBonus: 12, critThreshold: 17, critMult: 3.5f, lifeSteal: .35f),
                    new("stomp", "Stomp", new(8, 8), DamageType.Bludgeoning, atkBonus: -5, critMult: 5)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(2f, () => new("livingstone")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(2f, () => new("brimstone")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1f, () => new("landslide"))
                ), minDrops: 2, maxDrops: 3,
                xp: 500,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 9) * 0.4f + floor.arcane - floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Radiant, -10 },
                    { DamageType.Necrotic, 10 },
                    { DamageType.Psychic, 30 },
                    { DamageType.Aberrant, 5 },
                    { DamageType.Poison, 30 },
                    { DamageType.Cold, -5 },
                    { DamageType.Fire, 30 },
                    { DamageType.Infernal, 30 }
                },
                strength: 8,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Greater Angel
             new(1, (actual) => new SimpleMonster("greaterangel", "Greater Angel", 275,
                new(new Attack[]
                {
                    new("smite", "Smite", new(10, 5), DamageType.Radiant, atkBonus: 7, critThreshold: 17, lifeSteal: .7f, dmgAbilityScore: AbilityScore.Wisdom),
                    new("mace", "Holy Mace", new(8, 7), DamageType.Slashing, critThreshold: 18, critMult: 4),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("holyblood", Utils.RandInt(2, 4))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("mace")),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(0.5f, () => new("clericarmor"))
                ), minDrops: 1, maxDrops: 3,
                xp: 500,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 9) * 0.3f + floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, -5 },
                    { DamageType.Infernal, -10 },
                    { DamageType.Radiant, 30 },
                    { DamageType.Fire, 5 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Poison, 5 },
                    { DamageType.Cold, 5 }
                },
                strength: 10,
                dexterity: 8,
                agility: 8,
                wisdom: 7,
                defense: 8,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Silent Shadow
             new(1, (actual) => new SimpleMonster("silentshadow", "Silent Shadow", 225,
                new(new Attack[]
                {
                    new("strangle", "Strangle", new(6, 10), DamageType.Bludgeoning, atkBonus: 7, critThreshold: 17, lifeSteal: .7f, dmgAbilityScore: AbilityScore.Wisdom),
                    new("assassinate", "Assassinate", new(12, 7), DamageType.Slashing, critThreshold: 14, critMult: 3),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("shadowessence", Utils.RandInt(2, 4))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("darksteel", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("umbralvapor", 1))
                ), minDrops: 1, maxDrops: 3,
                xp: 430,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 8) * 0.3f - floor.holy + floor.arcane;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, 30 },
                    { DamageType.Radiant, 30 },
                    { DamageType.Fire, -5 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Poison, 30 },
                    { DamageType.Cold, 30 }
                },
                strength: 5,
                dexterity: 12,
                agility: 12,
                defense: 3,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Ice Demon
             new(1, (actual) => new SimpleMonster("icedemon", "Ice Demon", 265,
                new(new Attack[]
                {
                    new("chill", "Chill", new(12, 3, 8), DamageType.Cold, critThreshold: 16, lifeSteal: .4f),
                    new("infernal", "Infernal Touch", new(20, 2), DamageType.Infernal, atkBonus: -5, critThreshold: 18, critMult: 3.5f, lifeSteal: .25f),
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("ice", Utils.RandInt(2, 5))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("frostshard", Utils.RandInt(1, 3))),
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("brimstone", 1))
                ), minDrops: 1, maxDrops: 3,
                xp: 450,
                scaleTableWeight: (floor) =>
                {
                    return 0.8f - Math.Abs(floor.Depth - 9) * 0.3f - floor.holy;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, 10 },
                    { DamageType.Radiant, -10 },
                    { DamageType.Fire, -10 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Poison, 30 },
                    { DamageType.Cold, 30 },
                    { DamageType.Infernal, 30 }
                },
                strength: 8,
                dexterity: 6,
                agility: 6,
                defense: 9,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Mammoth Giant
             new(1, (actual) => new SimpleMonster("mammothgiant", Utils.Style("Mammoth Giant", "cyan"), 500,
                new(new Attack[]
                {
                    new("crush", "Crush", new(12, 5, 12), DamageType.Bludgeoning, critThreshold: 16, critMult: 3),
                    new("gore", "Gore", new(20, 3), DamageType.Piercing, atkBonus: -5, critThreshold: 16, critMult: 4.5f, lifeSteal: .25f),
                    new("frostbreath", "Frostbreath", new(20, 5), DamageType.Cold, atkBonus: 10)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("mammothsoul"))
                ), minDrops: 1, maxDrops: 1,
                xp: 1000,
                scaleTableWeight: (floor) =>
                {
                    return floor.Depth == 10 ? 1 : 0;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, 15 },
                    { DamageType.Fire, -5 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Poison, 5 },
                    { DamageType.Cold, 30 },
                },
                onTick: (data) =>
                {
                    if (data.tickCount % 1 == 0)
                    {
                        Location? location = data.self.Location;
                        if (location != null)
                        {
                            foreach (Creature creature in location.creatures)
                                if (creature != data.self && creature is Player)
                                    creature.TakeDamage(10, DamageType.Cold, data.self, true);
                        }
                    }
                },
                strength: 12,
                dexterity: 10,
                agility: 10,
                defense: 13,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

             //Hell Commander
             new(1, (actual) => new SimpleMonster("hellcommander", Utils.Style("Hell Commander", "darkred"), 400,
                new(new Attack[]
                {
                    new("slash", "Slash", new(8, 8, 15), DamageType.Slashing, atkBonus: 8, critThreshold: 18, critMult: 4),
                    new("stomp", "Stomp", new(20, 4), DamageType.Bludgeoning, atkBonus: -5, critThreshold: 16, critMult: 2.5f),
                    new("aura", "Aura of Cinders", new(20, 4), DamageType.Infernal, atkBonus: 20, critThreshold: 8, critMult: 1.5f, lifeSteal: .4f)
                }),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("demonsoul"))
                ), minDrops: 1, maxDrops: 1,
                xp: 1000,
                scaleTableWeight: (floor) =>
                {
                    return floor.Depth == 10 ? 1 : 0;
                },
                resistances: new()
                {
                    { DamageType.Necrotic, 15 },
                    { DamageType.Fire, -5 },
                    { DamageType.Psychic, 5 },
                    { DamageType.Poison, 5 },
                    { DamageType.Cold, 30 },
                },
                onTick: (data) =>
                {
                    if (data.tickCount % 1 == 0)
                    {
                        Location? location = data.self.Location;
                        if (location != null)
                        {
                            foreach (Creature creature in location.creatures)
                                if (creature != data.self && creature is Player)
                                    creature.TakeDamage(10, DamageType.Cold, data.self, true);
                        }
                    }
                },
                strength: 15,
                dexterity: 7,
                agility: 15,
                defense: 9,
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            ))
        );

    }
}