using Godot;
using Godot.Collections;
using Game.GUIS;

namespace Game.Managers
{
    public partial class GameManager : Node
    {
        [Signal] public delegate void ChangeValuesEventHandler();
        [Signal] public delegate void ChangeModeEventHandler(int NewMode);
        [Signal] public delegate void ChangeSkillParametersEventHandler(int skillID, bool LevelUp);

        public enum Mode {Menu, Play};
        private Mode currentMode;
        public Mode CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                currentMode = value;
                this.EmitSignal(SignalName.ChangeMode, (int)CurrentMode);
            }
        }
        
        private Array<Texture2D> cursors;

        private GUI BaseGUI;

        public override void _Ready()
        {
            ProcessMode = ProcessModeEnum.Always;

            BaseGUI = GetNode("/root/Main/GUILayer/GUI") as GUI;

            for (int i = 0; i < SkillParameters.Count; i++)
            {
                SkillParameters[i]["Texture"] = GD.Load<Texture2D>(PathToSkillTextures + (string)SkillParameters[i]["TextureName"]);
            }

            cursors = new Array<Texture2D> 
            {
                ResourceLoader.Load("res://Resources/Textures/Сursors/arrow 32x32.png" ) as Texture2D,
                ResourceLoader.Load("res://Resources/Textures/Сursors/crosshair.png") as Texture2D
            };

            ChangeMode += OnChangeMode;
            CurrentMode = Mode.Play;
        }

        private void OnChangeMode(int NewMode)
        {
            Input.SetCustomMouseCursor(cursors[NewMode], Input.CursorShape.Arrow, new Vector2(16, 16));
        }

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
                EmitSignal(SignalName.ChangeValues);
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
                EmitSignal(SignalName.ChangeValues);
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
                EmitSignal(SignalName.ChangeValues);
            }
        }

        public int EnemysCount = 0;

        // Skills

        public string PathToSkillTextures = "res://Resources/Textures/Skills/";

        public Dictionary<int, Dictionary<string, Variant>> SkillParameters = new Dictionary<int, Dictionary<string, Variant>>
        {
            {0, new Dictionary<string, Variant>
                {
                    {"Name", "Freezing"},
                    {"TextureName", "Freezing.png"},
                    
                    {"Level", 0},

                    {"Chance", 0},
                    {"BaseChance", 20},

                    {"Damage", 0f},
                    {"BaseDamage", 0f},

                    {"Ticks", 0},
                    {"BaseTicks", 1},
                    {"TickDuration", 2f},

                    {"Slowing", 0},
                    {"BaseSlowing", 0f}
                }
            },
            {1, new Dictionary<string, Variant>
                {
                    {"Name", "Burning"},
                    {"TextureName", "Burning.png"},

                    {"Level", 0},

                    {"Chance", 0},
                    {"BaseChance", 20},

                    {"Damage", 0f},
                    {"BaseDamage", 1f},

                    {"Ticks", 0},
                    {"BaseTicks", 2},
                    {"TickDuration", 1f},

                    {"Slowing", 0},
                    {"BaseSlowing", 0f}
                }
            },
            {2, new Dictionary<string, Variant>
                {
                    {"Name", "Poisoning"},
                    {"TextureName", "Poisoning.png"},

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

        public override void _Input(InputEvent @event)
        {
            if (@event is InputEventKey eventKey)
            {
                if(eventKey.Pressed && eventKey.Keycode == Key.Escape)
                {
                    if (CurrentMode == Mode.Menu)
                    {
                        CurrentMode = Mode.Play;
                    }
                    else if (CurrentMode == Mode.Play)
                    {
                        CurrentMode = Mode.Menu;
                    }
                }
            }
        }
    
        public void AddSkillLevel(int skillID)
        {
			if ((int)SkillParameters[skillID]["Level"] < 5)
			{
				SkillParameters[skillID]["Level"] = (int)SkillParameters[skillID]["Level"] + 1;
				SkillParameters[skillID]["Chance"] = (int)SkillParameters[skillID]["Level"] * (int)SkillParameters[skillID]["BaseChance"];
				SkillParameters[skillID]["Ticks"] = (int)SkillParameters[skillID]["Level"] * (int)SkillParameters[skillID]["BaseTicks"];
				SkillParameters[skillID]["Damage"] = (int)SkillParameters[skillID]["Level"] * (float)SkillParameters[skillID]["BaseDamage"];
				SkillParameters[skillID]["Slowing"] = (int)SkillParameters[skillID]["Level"] * (float)SkillParameters[skillID]["BaseSlowing"];
            }
            EmitSignal(SignalName.ChangeSkillParameters, skillID, true);
        }

        public void SubtractSkillLevel(int skillID)
        {
            if ((int)SkillParameters[skillID]["Level"] > 0)
			{
				SkillParameters[skillID]["Level"] = (int)SkillParameters[skillID]["Level"] - 1;
				SkillParameters[skillID]["Chance"] = (int)SkillParameters[skillID]["Level"] * (int)SkillParameters[skillID]["BaseChance"];
				SkillParameters[skillID]["Ticks"] = (int)SkillParameters[skillID]["Level"] * (int)SkillParameters[skillID]["BaseTicks"];
				SkillParameters[skillID]["Damage"] = (int)SkillParameters[skillID]["Level"] * (float)SkillParameters[skillID]["BaseDamage"];
				SkillParameters[skillID]["Slowing"] = (int)SkillParameters[skillID]["Level"] * (float)SkillParameters[skillID]["BaseSlowing"];
			}
            EmitSignal(SignalName.ChangeSkillParameters, skillID, false);
        }
    }
}