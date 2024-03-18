using Game.Managers;
using Game.Trackers;
using Godot;
using GodotUtilities;

namespace Game
{
    [Scene]
    public partial class Main : Node
    {
        private GameManager gameManager;

        [Node] private Node2D Place;

        [Node] private CanvasLayer GUILayer;
        private Control GUI;
        private Control PauseGUI;
        private Control SelectionGUI;

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
            gameManager.modeTracker.ModeChanged += _ChangeMode;

            GUI = GUILayer.GetNode("GUI") as Control;
            PauseGUI = GUILayer.GetNode("PauseGUI") as Control;
            SelectionGUI = GUILayer.GetNode("SelectionGUI") as Control;
        }

        private void _ChangeMode()
        {
            switch (gameManager.modeTracker.GetMode())
            {
                case ModeTracker.Mode.None:
                    break;

                case ModeTracker.Mode.Menu:
                    break;

                case ModeTracker.Mode.Play:
                    GUI.Show();
                    PauseGUI.Hide();
                    SelectionGUI.Hide();
                    
                    GetTree().Paused = false;
                    break;

                case ModeTracker.Mode.Pause:
                    PauseGUI.Show();
                    GUI.Hide();
                    SelectionGUI.Hide();

                    GetTree().Paused = true;
                    break;

                case ModeTracker.Mode.Selection:
                    SelectionGUI.Show();
                    GUI.Hide();
                    PauseGUI.Hide();

                    GetTree().Paused = true;
                    break;
            };
        }
    }
}