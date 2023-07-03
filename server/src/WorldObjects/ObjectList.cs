using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldObjects
{
    public static class ObjectList
    {
        static readonly Table<Func<string, WorldObject>> OBJECTS = new(
            new(1f, (location) => 
                new Harvestable("mushroompatch", "Mushroom Patch", location, "A small patch of grey mushrooms. Might be edible, might not.", "Pick", "mushroom", 1, 3)),
            new(1f, (location) => new Harvestable("bonepile", "Bone Pile", location, "A pile of assorted bones.", "Take", "bone", 2, 4)),
            new(1f, (location) => new Harvestable("slimetrail", "Slime Trail", location, "A thin trail of slime.", "Scrape Off", "slime", 1, 1)),
            new(1f, (location) => new Harvestable("ironvein", "Iron Vein", location, "A strip of raw iron runs through the wall.", "Mine", "ironore",
                getAmtRange: (player) => Utils.HasItem(player, "pickaxe") ? new int[]{2, 3} : new int[]{1, 1})),
            new(1f, (location) => new Harvestable("coalvein", "Coal Vein", location, "A strip of raw coal runs through the wall.", "Mine", "coal",
                getAmtRange: (player) => Utils.HasItem(player, "pickaxe") ? new int[] { 2, 3 } : new int[] { 1, 1 })),
            new(1f, (location) =>
                new Harvestable("lostpurse", "Lost Purse", location, "A small purse, containing a handful of coins.", "Take", "coin", 2, 5))
        );

        public static Func<string, WorldObject> Get()
        {
            return OBJECTS.Get();
        }
    }
}
