using Game.Trackers;
using Godot;

namespace Game.Managers
{
    public partial class GameManager : Node
    {
        public ModeTracker modeTracker {get; set;}
        public StatsTracker statsTracker {get; set;}
        public SkillsTracker skillsTracker {get; set;}

        public override void _Ready()
        {
            ProcessMode = ProcessModeEnum.Always;
            modeTracker = new ModeTracker();
            OpenSession();
        }

        public void OpenSession()
        {
            statsTracker = new StatsTracker();
            skillsTracker = new SkillsTracker();
            modeTracker.SetMode(ModeTracker.Mode.Play);
        }

        public void CloseSession()
        {
            statsTracker = null;
            skillsTracker = null;
            modeTracker.SetMode(ModeTracker.Mode.Menu);
        }

        // TEST (IN PROGRESS)
        public override void _Input(InputEvent @event)
        {
            if (Input.IsActionJustPressed("esc"))
			{
				if (modeTracker.GetMode() == Trackers.ModeTracker.Mode.Pause)
				{
					modeTracker.SetMode(Trackers.ModeTracker.Mode.Play);
				}
				else if (modeTracker.GetMode() == Trackers.ModeTracker.Mode.Play)
				{
					modeTracker.SetMode(Trackers.ModeTracker.Mode.Pause);
				}
			}
        }
    }
}