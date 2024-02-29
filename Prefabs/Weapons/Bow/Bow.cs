using Godot;
using GodotUtilities;

namespace Game.Weapons
{
    [Scene]
    public partial class Bow : RangeWeapon
    {   
        [Node] public Sprite2D Sprite2D;
        public AudioStreamPlayer2D audioStreamPlayer;
        public override void _Notification(int what)
        {
            base._Notification(what);
            if (what == NotificationSceneInstantiated)
            {
                this.WireNodes();
            }
        } 
        public override void _Ready()
        {
            base._Ready();

            audioStreamPlayer = new AudioStreamPlayer2D();
            AddChild(audioStreamPlayer);

            audioStreamPlayer.Stream = GD.Load<AudioStreamWav>("res://Resources/Sounds/536066__eminyildirim__bow-impact.wav");
            audioStreamPlayer.MaxPolyphony = 20;
        }
    }
}