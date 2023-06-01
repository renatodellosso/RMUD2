using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Config
{
    
    public const uint PORT = 1001;

    public const uint HANDLER_THREADS = 3;
    /// <summary>
    /// How long to sleep each handler thread after processing or checking for a request
    /// </summary>
    public const int HANDLER_SLEEP_INTERVAL = 50;

    public const string ENV_PATH = "../../../.env";

    public const int PBKDF2_ITERATIONS = 600000;

    public const int MAX_SIGN_IN_TRIES = 5;
    public static readonly TimeSpan LOCK_OUT_DURATION = new(0, 5, 0); //Hours, minutes, then seconds. Not sure why I can't use const here, but I can't, so I use static readonly

    public const int CODE_LENGTH = 6;

    public const string START_LOCATION = "intro";

    public const int TICK_INTERVAL = 1000;

    public static class DungeonGeneration
    {
        public const int MIN_FLOORS = 3, MAX_FLOORS = 5;
        public const int MIN_FLOOR_SIZE = 5, MAX_FLOOR_SIZE = 15;

        //MIN_FILL is the minimum % of the floor that has rooms
        public const float EXIT_CHANCE = .4f, MIN_FILL = .3f, STAIR_CHANCE = .2f;

        public const int SLEEP_INTERVAL = 1000;

        public const float MONSTER_CHANCE = .75f;
        public const int MIN_MONSTERS = 1, MAX_MONSTERS = 3;
    }

}
