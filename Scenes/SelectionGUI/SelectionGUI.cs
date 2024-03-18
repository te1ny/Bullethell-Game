using Godot;
using GodotUtilities;
using Game.Managers;
using Game.Trackers;
using Godot.Collections;

namespace Game.GUIS
{
	[Scene]
	public partial class SelectionGUI : Control
	{
		[Signal] public delegate void AcceptedSelectEventHandler();

		private GameManager gameManager;
		[Node("AspectRatioContainer/SkillBar")] private VFlowContainer _skillSelectorBar;
		
		private PackedScene skillSelectorReference;

		private RandomNumberGenerator random;

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
			gameManager.statsTracker.LevelChanged += _LevelChanged;
			skillSelectorReference = GD.Load<PackedScene>("res://Scenes/SelectionGUI/SkillSelector/SkillSelector.tscn");
			AcceptedSelect += _AcceptedSelect;
			random = new RandomNumberGenerator();
		}

		private void _LevelChanged(int _previousLevel, int _currentLevel)
		{
			gameManager.modeTracker.SetMode(ModeTracker.Mode.Selection);
			CheckSkillLevels();
		}

		private void CheckSkillLevels()
		{
			Array<int> bufferSkillsID = new Array<int> {};
			for(int i = 0; i < gameManager.skillsTracker.GetCountSkills(); i++)
			{
				int randomSkillId;
				for (;;)
				{
					randomSkillId = random.RandiRange(0, gameManager.skillsTracker.GetCountSkills() - 1);
					if (!bufferSkillsID.Contains(randomSkillId))
					{
						bufferSkillsID.Add(randomSkillId);
						break;
					}
				}
				
				if (_skillSelectorBar.GetChildCount() == 3)
				{
					break;
				}
				else if ((int)gameManager.skillsTracker.GetData(randomSkillId, SkillsTracker.Properties.Level) < 5)
				{
					AddSkillSelector(randomSkillId);
				}
			}
		}

		private void AddSkillSelector(int skillID)
		{
			SkillSelector skill = skillSelectorReference.Instantiate() as SkillSelector;
			_skillSelectorBar.AddChild(skill);
			skill.SetSkill(skillID);
			skill.StartSelector();
		}

		private async void _AcceptedSelect()
		{
			for(int i = 0; i < _skillSelectorBar.GetChildCount(); i++)
			{
				SkillSelector skill = _skillSelectorBar.GetChild(i) as SkillSelector;
				skill.DeleteSelector();
				if (i + 1 >= _skillSelectorBar.GetChildCount())
				{
					await ToSignal(skill, SkillSelector.SignalName.TreeExited);
				}
			}
			Visible = false;
			gameManager.modeTracker.SetMode(ModeTracker.Mode.Play);
		}
	}
}

