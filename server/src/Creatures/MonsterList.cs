namespace Creatures
{
    public static class MonsterList
    {

        public static readonly Table<Func<Creature>> MONSTERS = new(

            //Zombie
            new(1, () => new SimpleMonster("zombie", "Zombie", 5)),

            //Skeleton
            new(1, () => new SimpleMonster("skeleton", "Skeleton", 5)),

            //Giant Rat
            new(1, () => new SimpleMonster("giantrat", "Giant Rat", 4)),

            //Slime
            new(1, () => new SimpleMonster("slime", "Slime", 7))

        );

    }
}
