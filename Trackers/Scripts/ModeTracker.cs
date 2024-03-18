using Godot;
using Godot.Collections;

namespace Game.Trackers
{
    public partial class ModeTracker : RefCounted
    {
        [Signal] public delegate void ModeChangedEventHandler();
        
        public enum Mode {None, Menu, Play, Pause, Selection};

        private Array<Texture2D> cursors;

        private Mode _currentMode = Mode.Menu;
        private Mode _CurrentMode
        {
            get
            {
                return _currentMode;
            }
            set
            {
                _currentMode = value;
                if (_currentMode == Mode.Play)
                {
                    Input.SetCustomMouseCursor(cursors[0], Input.CursorShape.Arrow, new Vector2(16, 16));
                }
                else
                {
                    Input.SetCustomMouseCursor(cursors[1], Input.CursorShape.Arrow, new Vector2(16, 16));
                }
                EmitSignal(SignalName.ModeChanged);
            }
        }

        public ModeTracker()
        {
            cursors = new Array<Texture2D> 
            {
                ResourceLoader.Load("res://Resources/Textures/Сursors/arrow 32x32.png" ) as Texture2D,
                ResourceLoader.Load("res://Resources/Textures/Сursors/crosshair.png") as Texture2D,
            };
        }

        public Mode GetMode()
        {
            return _CurrentMode;
        }

        public void SetMode(Mode mode)
        {
            _CurrentMode = mode;
        }
    }
}