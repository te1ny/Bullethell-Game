using System;
using Game.Managers;
using Godot;

namespace Game.GUIS
{
	public partial class SkillSlot : Panel
	{
		private GameManager gameManager;

		private TextureRect skillBox;
		private TextureRect skillTexture;
		private Label levelLabel;
		private AnimationPlayer animationPlayer;

		private int skillID;

		public override void _Ready()
		{
			gameManager = GetNodeOrNull("/root/GameManager") as GameManager;
			gameManager.skillsTracker.SkillsDataChanged += _ChangeSkillParameters;

			skillBox = GetNode("SkillBox") as TextureRect;
			skillTexture = skillBox.GetNode("SkillTexture") as TextureRect;
			levelLabel = GetNode("LevelLabel") as Label;
			animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		}

		private void _ChangeSkillParameters(int _skillID, bool _LevelUp)
		{
			if (this.skillID == _skillID)
			{
				levelLabel.Text = Convert.ToString((int)gameManager.skillsTracker.GetData(_skillID, Trackers.SkillsTracker.Properties.Level));
			}
		}

		public void SetSkill(int skillID)
		{
			this.skillID = skillID;
			SetTexture(this.skillID);
		}

		public int GetSkill()
		{
			return this.skillID;
		}

		public void SetTexture(int _skillID)
		{
			skillTexture.Texture = (Texture2D)gameManager.skillsTracker.GetData(_skillID, Trackers.SkillsTracker.Properties.Texture);
		}

		public void StartAnimation()
		{
			animationPlayer.Play("start");
		}

		public async void DeleteSkillSlot()
		{
			gameManager.skillsTracker.SkillsDataChanged -= _ChangeSkillParameters;
			animationPlayer.PlayBackwards("start");
			await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
			QueueFree();
		}
	}
}