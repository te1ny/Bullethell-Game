using Godot;

namespace Game.Trackers
{
    public partial class StatsTracker : RefCounted
    {
        /// <summary>
        /// Called when the value of money, experience or level is changed 
        /// so that the GUI will request the new information to display correctly.
        /// </summary>
        [Signal] public delegate void ValuesChangedEventHandler();

        /// <summary>
        /// Called each time the level is changed to bring up the new skill selection window.
        /// </summary>
        /// <param name="_previousLevel"></param>
        /// <param name="_currentLevel"></param>
        [Signal] public delegate void LevelChangedEventHandler(int _previousLevel, int _currentLevel);

        private int _currentMoney = 0;
        private int _currentExperience = 0;
        private int _currentLevel = 1;

        private int _CurrentMoney
        {
            get
            {
                return _currentMoney;
            }
            set
            {
                _currentMoney = value;
                EmitSignal(SignalName.ValuesChanged);
            }
        }
        private int _CurrentExperience
        {
            get
            {
                return _currentExperience;
            }
            set
            {
                _currentExperience = value;
                int bufferLevel = _CurrentLevel;
                for (int i = bufferLevel; ;i++)
                {
                    if (_currentExperience > bufferLevel * 50)
                    {
                        _currentExperience -= bufferLevel * 50;
                        bufferLevel += 1;
                    }
                    else
                    {
                        break;
                    }
                }

                if (bufferLevel > _CurrentLevel)
                {
                    _CurrentLevel = bufferLevel;
                }

                EmitSignal(SignalName.ValuesChanged);
            }
        }
        private int _CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                EmitSignal(SignalName.LevelChanged, _currentLevel, value);
                _currentLevel = value;
                EmitSignal(SignalName.ValuesChanged);
            }
        }  

        public int GetMoney()
        {
            return _CurrentMoney;
        }

        public int GetExperience()
        {
            return _CurrentExperience;
        }

        public int GetLevel()
        {
            return _CurrentLevel;
        }

        /// <summary>
        /// Adds money to the current money.
        /// </summary>
        /// <param name="what">
        /// Number of money to be added.
        /// </param>
        public void AddMoney(int what)
        {
            _CurrentMoney += what;
        }

        /// <summary>
        /// Adds XP to the current experience.
        /// </summary>
        /// <param name="what">
        /// Number of XP to be added.
        /// </param>
        public void AddExperience(int what)
        {
            _CurrentExperience += what;
        }

        /// <summary>
        /// Adds level to the current level.
        /// </summary>
        /// <param name="what">
        /// Number of levels to be added.
        /// </param>
        public void AddLevel(int what)
        {
            _CurrentLevel += what;
        }
    }
}