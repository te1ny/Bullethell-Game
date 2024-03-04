using Game.Managers;
using Godot;

namespace Game.GUIS
{
	public partial class PauseGUI : Control
	{
		private GameManager gameManager;

		private HBoxContainer buttonContainer;
		
		private Button playButton;
		private Button settingsButton;
		private Button exitButton;

        public override void _Ready()
        {
			gameManager = GetNodeOrNull("/root/GameManager") as GameManager;

			buttonContainer = GetNode("ButtonContainer") as HBoxContainer;

			playButton = buttonContainer.GetNode("PlayButton") as Button;
			settingsButton = buttonContainer.GetNode("SettingsButton") as Button;
			exitButton = buttonContainer.GetNode("ExitButton") as Button;

            playButton.Pressed += _PlayPressed;
			settingsButton.Pressed += _SettingsPressed;
			exitButton.Pressed += _ExitPressed;
        }

		private void _PlayPressed()
		{
			gameManager.CurrentMode = GameManager.Mode.Play;
		}
		private void _SettingsPressed()
		{
			
		}
		private void _ExitPressed()
		{
			GetTree().Quit();
		}
    }
}