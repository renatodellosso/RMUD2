using OpenAI.ObjectModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

static class Config
{
    
    public const uint PORT = 2001;
    public static readonly string[] PREFIXES = new string[] { 
        "http://*:" + PORT + "/",
        //"http://localhost:" + PORT + "/", //I think we can only have 1 wildcard prefix, so this is redundant
        //"http://0.0.0.0:" + PORT + "/", //Doesn't work for some reason
    };

    public const uint HANDLER_THREADS = 3;
    /// <summary>
    /// How long to sleep each handler thread after processing or checking for a request
    /// </summary>
    public const int HANDLER_SLEEP_INTERVAL = 50;

    public const string ENV_PATH = "../../../.env";

    public const int MAX_SIGN_IN_TRIES = 5;
    public static readonly TimeSpan LOCK_OUT_DURATION = new(0, 5, 0); //Hours, minutes, then seconds. I can only use const for primitives, so I use static readonly

    public const int CODE_LENGTH = 6;

    public const int TICK_INTERVAL = 1000;
    public const int BOT_STATUS_UPDATE_FREQUENCY = 10;

    public const long MAX_RAM = 1024 * 1024 * 1024; //1GB

    public const int MAX_LOG_LENGTH = 30;

    public static class DungeonGeneration
    {
        public const int MIN_FLOORS = 5, MAX_FLOORS = 7;
        public const int MIN_FLOOR_SIZE = 10, MAX_FLOOR_SIZE = 17;

        //MIN_FILL is the minimum % of the floor that has rooms
        public const float EXIT_CHANCE = .3f, MIN_FILL = .35f, STAIR_CHANCE = .6f;

        public const int SLEEP_INTERVAL = 1000;

        public const float MONSTER_CHANCE = .75f;
        public const int MIN_MONSTERS = 1, MAX_MONSTERS = 3;

        public const float OBJECT_CHANCE = .5f;
        public const int MIN_OBJECTS = 1, MAX_OBJECTS = 2;
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

        public const int BASE_CARRY_WEIGHT = 60;
        public const int CARRY_WEIGHT_PER_STR = 10;

        public const int BASE_PLAYER_HP = 35;
        public const int HP_PER_CON = 5;

        public const int BASE_STAMINA = 5;
        public const int STAMINA_PER_END = 1;
        public const int BASE_STAMINA_REGEN = 1;
        public const float STAMINA_REGEN_PER_EVERY_OTHER_AGI = .05f;
        public const float ENCUMBRANCE_STAMINA_REGEN_REDUCTION_PER_LB = .05f;

        public const float XP_PER_WIS = .02f;

        public const float BASE_SELL_CUT = .7f;
        public const float SELL_CUT_PER_CHA = .05f;

        public const string START_LOCATION = "intro";
        public const string RESPAWN_LOCATION = "townsquare";

        public const int MAX_ENEMIES_IN_ROOM = 3; //Only applies when a monster is moving through a room
    }

    public static class SessionRemoval
    {
        public const int CHECK_INTERVAL = 60000; //1 minute
        public const int MAX_AGE = 120000; //2 minutes
    }

}
