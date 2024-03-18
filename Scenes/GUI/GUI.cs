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
			gameManager.skillsTracker.SkillsDataChanged += _ChangeSkillParameters;

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

		private void _ChangeSkillParameters(int _skillID, bool _levelUp)
		{
			if ((int)gameManager.skillsTracker.GetData(_skillID, Trackers.SkillsTracker.Properties.Level) == 0 && !_levelUp)
			{
				DeleteSkillSlot(_skillID);
				return;
			}

			if ((int)gameManager.skillsTracker.GetData(_skillID, Trackers.SkillsTracker.Properties.Level) == 1 && _levelUp)
			{
				AddSkillSlot(_skillID);
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

