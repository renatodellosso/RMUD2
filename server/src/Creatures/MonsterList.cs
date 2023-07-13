using Items;
using ItemTypes;

namespace Creatures
{
    public static class MonsterList
    {

        public delegate Creature MonsterEntry(bool actual = true);

        public static readonly Table<MonsterEntry> MONSTERS = new(

            //Zombie
            new(1, (actual) => new SimpleMonster("zombie", "Zombie", 5, new("bite", "Bite", AbilityScore.Strength, new(6)), 
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("rottenflesh"))
                ),
                xp: 20,
                scaleTableWeight: (floor) =>
                {
                    return 1.2f - floor.temperature - floor.Depth * 0.4f;
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
                    return 1.2f - floor.Depth * 0.4f;
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Giant Rat
            new(1, (actual) => new SimpleMonster("giantrat", "Giant Rat", 4, new("bite", "Bite", AbilityScore.Strength, new(4)),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat"))
                ),
                xp: 15,
                scaleTableWeight: (floor) =>
                {
                    return 1.5f - floor.Depth * 0.4f - Math.Abs(floor.temperature);
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Slime
            new(1, (actual) => new SimpleMonster("slime", "Slime", 7, new("ooze", "Ooze", AbilityScore.Constitution, new(4)),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("slime"))
                ), minDrops: 1, maxDrops: 2,
                xp: 25,
                scaleTableWeight: (floor) =>
                {
                    return 1.2f - floor.Depth * 0.4f + floor.arcane;
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            )),

            //Troll
            new(1, (actual) => new SimpleMonster("troll", "Troll", 15, new("punch", "Punch", AbilityScore.Strength, new(8)),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("taintedflesh"))
                ), minDrops: 1, maxDrops: 2,
                xp: 60, onTick: (data) =>
                {
                    if(Utils.tickCount % 2 == 0) 
                        data.self.Heal(1);
                },
                scaleTableWeight: (floor) =>
                {
                    return 0.8f + (floor.Depth - 2) * 0.4f;
                },
                actual: actual //Breaks if we don't have this. It's used in dungeon generation
            ))

        );

    }
}
