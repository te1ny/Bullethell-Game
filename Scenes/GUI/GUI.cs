using System;
using Godot;
using GodotUtilities;
using Game.Managers;

namespace Game.GUIS
{
	[Scene]
	public partial class GUI : Control
	{
		private GameManager gameManager;

		// Nodes
		[Node] private Label timeLabel;
		[Node] private AspectRatioContainer aspectRatioContainer;
		private VFlowContainer skillBar;

		private PackedScene skillReference;

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
			gameManager.ChangeSkillParameters += _ChangeSkillParameters;

			skillReference = GD.Load<PackedScene>("res://Scenes/GUI/Skill/SkillSlot.tscn");
			skillBar = aspectRatioContainer.GetNode("SkillBar") as VFlowContainer;
		}

		public override void _PhysicsProcess(double delta)
		{
			currentTime += Convert.ToSingle(delta);
			ChangeTime();
		}

		private void ChangeTime()
		{
			minutesF = MathF.Truncate(currentTime / 60f);
			secondsF = MathF.Truncate(currentTime % 60f);

			timeLabel.Text = minutesS + ":" + secondsS;
		}

		private void _ChangeSkillParameters(int skillID, bool LevelUp)
		{
			if ((int)gameManager.SkillParameters[skillID]["Level"] == 0 && !LevelUp)
			{
				DeleteSkillSlot(skillID);
				return;
			}

			if ((int)gameManager.SkillParameters[skillID]["Level"] == 1 && LevelUp)
			{
				AddSkillSlot(skillID);
				return;
			}
		}

		public void AddSkillSlot(int skillID)
		{
			SkillSlot skill = skillReference.Instantiate() as SkillSlot;
			skillBar.AddChild(skill);

			skill.SetSkill(skillID);
			skill.StartAnimation();
		}

		public void DeleteSkillSlot(int skillID)
		{
			for (int i = 0; i < skillBar.GetChildren().Count; i++)
			{
				SkillSlot skill = skillBar.GetChild(i) as SkillSlot;
				if (skill.GetSkill() == skillID)
				{
					skill.DeleteSkillSlot();
					break;
				}
			}
		}
	}
}

