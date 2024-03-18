using Game.Managers;
using Godot;
using GodotUtilities;

namespace Game.GUIS
{
	[Scene]
	public partial class SkillSelector : Button
	{
		private GameManager gameManager;

		[Node] private TextureRect skillTexture;
		[Node] public AnimationPlayer animationPlayer;
		private int skillID;

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
			Pressed += _OnButtonPressed;
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
			skillTexture.Texture = (Texture2D)gameManager.skillsTracker.GetData(skillID, Trackers.SkillsTracker.Properties.Texture);
		}

		private void _OnButtonPressed()
		{
			gameManager.skillsTracker.AddSkillLevel(skillID);
			GetParent().GetParent().GetParent().EmitSignal(SelectionGUI.SignalName.AcceptedSelect); 
		}

		public void StartSelector()
		{
			animationPlayer.Play("Start");
		}

		public void DeleteSelector()
		{
			animationPlayer.Play("End");
		}
	}
}