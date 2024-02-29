using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class AttackResource : Resource
    {
        public enum DamageType {Physical, Magic, Pure};

        [Export] DamageType damageType {get; set;}
        [Export] public int damage {get; set;}
    }
}  