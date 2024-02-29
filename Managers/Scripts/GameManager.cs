using Game.Resources;
using Godot;
using Godot.Collections;

namespace Game.Managers
{
    public partial class GameManager : Node
    {
        [Signal] public delegate void ChangeValuesEventHandler();

        public EffectsResource effectsResource = new EffectsResource();
        public int EnemysCount = 0;

        // Stats
        private const int maxSelfLevel = 15;

        private int currentMoney = 0;
        private int currentExperience = 0;
        private int currentLevel = 0;

        public int CurrentMoney
        {
            get
            {
                return currentMoney;
            }
            set
            {
                currentMoney = value;
                EmitSignal(GameManager.SignalName.ChangeValues);
            }
        }
        public int CurrentExperience
        {
            get
            {
                return currentExperience;
            }
            set
            {
                currentExperience = value;
                if (currentExperience > CurrentLevel * 100)
                {
                    currentExperience = currentExperience - CurrentLevel * 100;
                    CurrentLevel += 1;
                }
                EmitSignal(GameManager.SignalName.ChangeValues);
            }
        }
        public int CurrentLevel
        {
            get
            {
                return currentLevel;
            }
            set
            {
                currentLevel = value;
                EmitSignal(GameManager.SignalName.ChangeValues);
            }
        }

        // Skills
        //private const int maxAbilityLevel = 5;

        public Dictionary<string, Dictionary<string, Variant>> SkillParameters = new Dictionary<string, Dictionary<string, Variant>>
        {
            {"Freezing", new Dictionary<string, Variant>
                {
                    {"Level", 0},

                    {"Chance", 0},
                    {"BaseChance", 20},

                    {"Ticks", 0},
                    {"BaseTicks", 1},
                    {"TickDuration", 2f},
                    
                }
            },
            {"Burning", new Dictionary<string, Variant>
                {
                    {"Level", 0},

                    {"Chance", 0},
                    {"BaseChance", 20},

                    {"Damage", 0f},
                    {"BaseDamage", 1f},

                    {"Ticks", 0},
                    {"BaseTicks", 2},
                    {"TickDuration", 1f},
                }
            },
            {"Poisoning", new Dictionary<string, Variant>
                {
                    {"Level", 0},

                    {"Chance", 0},
                    {"BaseChance", 20},

                    {"Damage", 0f},
                    {"BaseDamage", 0.5f},

                    {"Ticks", 0},
                    {"BaseTicks", 2},
                    {"TickDuration", 0.5f},

                    {"Slowing", 0},
                    {"BaseSlowing", 7.5f}
                }
            }
            
        };

        public bool autofireSelected = false;
    }
}