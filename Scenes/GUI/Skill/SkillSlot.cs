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
			gameManager.ChangeSkillParameters += _ChangeSkillParameters;

			skillBox = GetNode("SkillBox") as TextureRect;
			skillTexture = skillBox.GetNode("SkillTexture") as TextureRect;
			levelLabel = GetNode("LevelLabel") as Label;
			animationPlayer = GetNode("AnimationPlayer") as AnimationPlayer;
		}

		private void _ChangeSkillParameters(int skillID, bool LevelUp)
		{
			if (this.skillID == skillID)
			{
				levelLabel.Text = Convert.ToString((int)gameManager.SkillParameters[this.skillID]["Level"]);
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

		public void SetTexture(int skillID)
		{
			skillTexture.Texture = (Texture2D)gameManager.SkillParameters[skillID]["Texture"];
		}

		public void StartAnimation()
		{
			animationPlayer.Play("start");
		}

		public async void DeleteSkillSlot()
		{
			gameManager.ChangeSkillParameters -= _ChangeSkillParameters;
			animationPlayer.PlayBackwards("start");
			await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
			QueueFree();
		}
	}
}