using Godot;
using Godot.Collections;
using System;

namespace Game.Trackers
{
    public partial class SkillsTracker : RefCounted
    {
        // TEST SIGNAL
        [Signal] public delegate void SkillsDataChangedEventHandler(int _skillID, bool _levelUp);

        public enum Properties
        {
            Name,
            //
            Texture,
            TextureName,
            //
            Level,
            //
            Chance, 
            BaseChance,
            //
            Damage, 
            BaseDamage,
            //
            Ticks, 
            BaseTicks, 
            TickDuration,
            //
            Slowing, 
            BaseSlowing
        };

        private Dictionary<string, Dictionary<string, Variant>> _skillParameters;

        private string PathToSkillTextures = "res://Resources/Textures/Skills/";

        public SkillsTracker()
        {
            FileAccess file = FileAccess.Open("res://Data/SkillsData.json", FileAccess.ModeFlags.Read); // Open File
            string json_string = file.GetAsText(); // Convert JSON to string

            Json JSON = new Json();
            Error error = JSON.Parse(json_string);
            if (error == Error.Ok) 
            {
                _skillParameters = JSON.Data.AsGodotDictionary<string, Dictionary<string, Variant>>();
            }

            for (int i = 0; i < _skillParameters.Count; i++)
            {
                Texture2D texture = GD.Load<Texture2D>(PathToSkillTextures + GetData(i, Properties.TextureName));
                SetData(i, Properties.Texture, texture);
            }
        }

        public void AddSkillLevel(int _skillID)
        {
			if ((int)GetData(_skillID, Properties.Level) < 5)
			{
				SetData(_skillID, Properties.Level, (int)GetData(_skillID, Properties.Level) + 1);
				SetData(_skillID, Properties.Chance, (int)GetData(_skillID, Properties.Level) * (int)GetData(_skillID, Properties.BaseChance));
				SetData(_skillID, Properties.Ticks, (int)GetData(_skillID, Properties.Level) * (int)GetData(_skillID, Properties.BaseTicks));
				SetData(_skillID, Properties.Damage, (int)GetData(_skillID, Properties.Level) * (float)GetData(_skillID,Properties.BaseDamage));
				SetData(_skillID, Properties.Slowing, (int)GetData(_skillID, Properties.Level) * (float)GetData(_skillID,Properties.BaseSlowing));
            }
            EmitSignal(SignalName.SkillsDataChanged, _skillID, true);
        }

        public void SubtractSkillLevel(int _skillID)
        {
            if ((int)GetData(_skillID, Properties.Level) > 0)
			{
				SetData(_skillID, Properties.Level, (int)GetData(_skillID, Properties.Level) - 1);
				SetData(_skillID, Properties.Chance, (int)GetData(_skillID, Properties.Level) * (int)GetData(_skillID, Properties.BaseChance));
				SetData(_skillID, Properties.Ticks, (int)GetData(_skillID, Properties.Level) * (int)GetData(_skillID, Properties.BaseTicks));
				SetData(_skillID, Properties.Damage, (int)GetData(_skillID, Properties.Level) * (float)GetData(_skillID,Properties.BaseDamage));
				SetData(_skillID, Properties.Slowing, (int)GetData(_skillID, Properties.Level) * (float)GetData(_skillID,Properties.BaseSlowing));
			}
            EmitSignal(SignalName.SkillsDataChanged, _skillID, false);
        }

        public Variant GetData(int key, Properties what)
        {
            return _skillParameters[Convert.ToString(key)][what.ToString()];
        }

        public Variant GetData(string key, Properties what)
        {
            return _skillParameters[key][what.ToString()];
        }

        public void SetData(int key, Properties what, Variant value)
        {
            _skillParameters[Convert.ToString(key)][what.ToString()] = value;
        }

        public void SetData(string key, Properties what, Variant value)
        {
            _skillParameters[key][what.ToString()] = value;
        } 
    
        public int GetCountSkills()
        {
            return _skillParameters.Count;
        }

        public int GetCountProperties()
        {
            return Enum.GetNames(typeof(Properties)).Length;
        }
    }
}