using Godot.Collections;
using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class EffectsResource : Resource
    {
        public Dictionary<string, bool> Effects = new Dictionary<string, bool>
        {
            {
                "Freezing", false
            },
            {
                "Burning", false
            },
            {
                "Poisoning", false
            }
        };
    }
}  