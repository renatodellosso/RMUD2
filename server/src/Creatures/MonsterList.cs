using Items;
using ItemTypes;

namespace Creatures
{
    public static class MonsterList
    {

        public static readonly Table<Func<Creature>> MONSTERS = new(

            //Zombie
            new(1, () => new SimpleMonster("zombie", "Zombie", 5, new("bite", "Bite", AbilityScore.Strength, new(6)))),

            //Skeleton
            new(1, () => new SimpleMonster("skeleton", "Skeleton", 5, ItemList.Get<Weapon>("spear"))),

            //Giant Rat
            new(1, () => new SimpleMonster("giantrat", "Giant Rat", 4, new("bite", "Bite", AbilityScore.Strength, new(4)))),

            //Slime
            new(1, () => new SimpleMonster("slime", "Slime", 7, new("ooze", "Ooze", AbilityScore.Constitution, new(4))))

        );

    }
}
