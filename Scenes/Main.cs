using Game.Managers;
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
            gameManager.ChangeMode += _ChangeMode;

            GUI = GUILayer.GetNode("GUI") as Control;
            PauseGUI = GUILayer.GetNode("PauseGUI") as Control;
        }

        private void _ChangeMode(int what)
        {
            if (what == (int) GameManager.Mode.Menu)
            {   
                GUI.Hide();
                PauseGUI.Show();
                GetTree().Paused = true;
            }
            else if (what == (int) GameManager.Mode.Play)
            {
                GUI.Show();
                PauseGUI.Hide();
                Place.SetPhysicsProcess(true);
                GetTree().Paused = false;
            }
        }
    }
}