using System;
using Godot;
using GodotUtilities;
using Game.Managers;

namespace Game.GUI
{
	[Scene]
	public partial class GUI : Control
	{
		private GameManager gameManager {get; set;}

		[Node] private Label moneyLabel;
		[Node] private Label experienceLabel;
		[Node] private Label levelLabel;
		[Node] private Label timeLabel;
		[Node] private Label debugEnemiesCountLabel;
		[Node] private Label debugEffectsLevel;
		private GridContainer debugContainer;

		private Button debugAddLevelFreezing;
		private Button debugAddLevelBurning;
		private Button debugAddLevelPoisoning;

		private Button debugSubtractLevelFreezing;
		private Button debugSubtractLevelBurning;
		private Button debugSubtractLevelPoisoning;

		private float currentTime = 0;

		private float minutesF = 0;
		private float secondsF = 0;

		string minutesS => Convert.ToString(minutesF).Length == 2 ? Convert.ToString(minutesF) : "0" + Convert.ToString(minutesF);
		string secondsS => Convert.ToString(secondsF).Length == 2 ? Convert.ToString(secondsF) : "0" + Convert.ToString(secondsF);

        public override void _Notification(int what)
        {
			if (what == NotificationSceneInstantiated)
			{
				this.WireNodes();
			}
        }

        public override void _Ready()
        {
			gameManager = GetNodeOrNull("/root/GameManager") as GameManager;
			gameManager.ChangeValues += OnChangeValues;
			debugContainer = GetNodeOrNull("DebugContainer") as GridContainer;

			debugAddLevelFreezing = debugContainer.GetNodeOrNull("DebugAddLevelFreezing") as Button;
			debugAddLevelBurning = debugContainer.GetNodeOrNull("DebugAddLevelBurning") as Button;
			debugAddLevelPoisoning = debugContainer.GetNodeOrNull("DebugAddLevelPoisoning") as Button;
			
			debugSubtractLevelFreezing = debugContainer.GetNodeOrNull("DebugSubtractLevelFreezing") as Button;
			debugSubtractLevelBurning = debugContainer.GetNodeOrNull("DebugSubtractLevelBurning") as Button;
			debugSubtractLevelPoisoning = debugContainer.GetNodeOrNull("DebugSubtractLevelPoisoning") as Button;

			debugAddLevelFreezing.Pressed += OnDebugAddLevelFreezing;
			debugAddLevelBurning.Pressed += OnDebugAddLevelBurning;
			debugAddLevelPoisoning.Pressed += OnDebugAddLevelPoisoning;
			
			debugSubtractLevelFreezing.Pressed += OnDebugSubtractLevelFreezing;
			debugSubtractLevelBurning.Pressed += OnDebugSubtractLevelBurning;
			debugSubtractLevelPoisoning.Pressed += OnDebugSubtractLevelPoisoning;
		}

        public override void _PhysicsProcess(double delta)
        {
			currentTime += 1 * Convert.ToSingle(delta);
			debugEnemiesCountLabel.Text = "ENEMYS COUNT: " + Convert.ToString(gameManager.EnemysCount);
			ChangeTime();
        }

        private void OnChangeValues()
		{
			moneyLabel.Text = "MONEY: " + Convert.ToString(gameManager.CurrentMoney);
			experienceLabel.Text = "XP: " + Convert.ToString(gameManager.CurrentExperience);
			levelLabel.Text = "LEVEL: " + Convert.ToString(gameManager.CurrentLevel);
		}

		private void ChangeTime()
		{
			minutesF = MathF.Truncate(currentTime / 60f);
			secondsF = MathF.Truncate(currentTime % 60f);

            timeLabel.Text = minutesS + ":" + secondsS;
		}

		private void OnDebugAddLevelFreezing()
		{
			string effect = "Freezing";
			if ((int)gameManager.SkillParameters[effect]["Level"] < 5)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] + 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
			}
			UpdateEffectsLabel();
		}
		private void OnDebugAddLevelBurning()
		{
			string effect = "Burning";
			if ((int)gameManager.SkillParameters[effect]["Level"] < 5)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] + 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
				gameManager.SkillParameters[effect]["Damage"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseDamage"];
			}
			UpdateEffectsLabel();
		}
		private void OnDebugAddLevelPoisoning()
		{
			string effect = "Poisoning";
			if ((int)gameManager.SkillParameters[effect]["Level"] < 5)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] + 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
				gameManager.SkillParameters[effect]["Damage"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseDamage"];
				gameManager.SkillParameters[effect]["Slowing"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseSlowing"];
			}
			UpdateEffectsLabel();
		}


		private void OnDebugSubtractLevelFreezing()
		{
			string effect = "Freezing";
			if ((int)gameManager.SkillParameters[effect]["Level"] > 0)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] - 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
			}
			UpdateEffectsLabel();
		}
		private void OnDebugSubtractLevelBurning()
		{
			string effect = "Burning";
			if ((int)gameManager.SkillParameters[effect]["Level"] > 0)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] - 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
				gameManager.SkillParameters[effect]["Damage"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseDamage"];
			}
			UpdateEffectsLabel();
		}
		private void OnDebugSubtractLevelPoisoning()
		{
			string effect = "Poisoning";
			if ((int)gameManager.SkillParameters[effect]["Level"] > 0)
			{
				gameManager.SkillParameters[effect]["Level"] = (int)gameManager.SkillParameters[effect]["Level"] - 1;
				gameManager.SkillParameters[effect]["Chance"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseChance"];
				gameManager.SkillParameters[effect]["Ticks"] = (int)gameManager.SkillParameters[effect]["Level"] * (int)gameManager.SkillParameters[effect]["BaseTicks"];
				gameManager.SkillParameters[effect]["Damage"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseDamage"];
				gameManager.SkillParameters[effect]["Slowing"] = (int)gameManager.SkillParameters[effect]["Level"] * (float)gameManager.SkillParameters[effect]["BaseSlowing"];
			}
			UpdateEffectsLabel();
		}

		private void UpdateEffectsLabel()
		{
			debugEffectsLevel.Text = 
			"Freezing: " + Convert.ToString(gameManager.SkillParameters["Freezing"]["Level"]) + '\n' +
			"Burning: " + Convert.ToString(gameManager.SkillParameters["Burning"]["Level"]) + '\n' +
			"Poisoning: " + Convert.ToString(gameManager.SkillParameters["Poisoning"]["Level"]);
		}
    }
}

