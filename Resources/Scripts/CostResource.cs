using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class CostResource : Resource
    {
        [Export] public int Money {get; set;}
        [Export] public int Experience {get; set;}
    }
}  