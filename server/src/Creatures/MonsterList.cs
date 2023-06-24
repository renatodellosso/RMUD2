using Items;
using ItemTypes;

namespace Creatures
{
    public static class MonsterList
    {

        public static readonly Table<Func<Creature>> MONSTERS = new(

            //Zombie
            new(1, () => new SimpleMonster("zombie", "Zombie", 5, new("bite", "Bite", AbilityScore.Strength, new(6)), 
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("rottenflesh"))
                ),
                xp: 20
            )),

            //Skeleton
            new(1, () => new SimpleMonster("skeleton", "Skeleton", 5, ItemList.Get<Weapon>("spear"),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("bone"))
                ), minDrops: 1, maxDrops: 3,
                xp: 25
            )),

            //Giant Rat
            new(1, () => new SimpleMonster("giantrat", "Giant Rat", 4, new("bite", "Bite", AbilityScore.Strength, new(4)),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("meat"))
                ),
                xp: 15
            )),

            //Slime
            new(1, () => new SimpleMonster("slime", "Slime", 7, new("ooze", "Ooze", AbilityScore.Constitution, new(4)),
                drops: new(
                    new KeyValuePair<float, Func<ItemHolder<Item>>>(1, () => new("slime"))
                ), minDrops: 1, maxDrops: 2,
                xp: 25
            ))

        );

    }
}
