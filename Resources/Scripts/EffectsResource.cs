using Godot.Collections;
using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class EffectsResource : Resource
    {
        public Dictionary<int, bool> Effects = new Dictionary<int, bool>
        {
            {
                0, false
            },
            {
                1, false
            },
            {
                2, false
            }
        };
    }
}  