using OpenAI.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Config
{
    
    public const uint PORT = 2001;

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

    public const int TICK_INTERVAL = 1000;

    public static class DungeonGeneration
    {
        public const int MIN_FLOORS = 3, MAX_FLOORS = 5;
        public const int MIN_FLOOR_SIZE = 5, MAX_FLOOR_SIZE = 15;

        //MIN_FILL is the minimum % of the floor that has rooms
        public const float EXIT_CHANCE = .4f, MIN_FILL = .3f, STAIR_CHANCE = .4f;

        public const int SLEEP_INTERVAL = 1000;

        public const float MONSTER_CHANCE = .75f;
        public const int MIN_MONSTERS = 1, MAX_MONSTERS = 3;

        public const float OBJECT_CHANCE = .5f;
        public const int MIN_OBJECTS = 1, MAX_OBJECTS = 1;
    }

    public static class AI
    {
        //Not const, but don't change
        public static string DEFAULT_MODEL = Models.ChatGpt3_5Turbo;

        public static int MAX_RETURN_TOKENS = 20;
        public static double COST_PER_TOKEN = .0015 / 1000; //Cost per token in dollars
    }

    public static class Gameplay
    {
        public const float FLAVOR_MSG_CHANCE = .025f; //Per second

        public const int REST_COST = 5;

        public const int BASE_CARRY_WEIGHT = 40;
        public const int CARRY_WEIGHT_PER_STR = 5;

        public const int BASE_PLAYER_HP = 35;
        public const int HP_PER_CON = 5;

        public const float BASE_SELL_CUT = .7f;
        public const float SELL_CUT_PER_CHA = .05f;

        public const string START_LOCATION = "intro";
        public const string RESPAWN_LOCATION = "townsquare";
    }

}
