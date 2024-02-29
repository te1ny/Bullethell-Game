using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class ProjectileResource : Resource
    {
        public AttackResource attackResource;
        public EffectsResource effectsResource;
        public Vector2 Direction;
        public float Speed;
        public Vector2 StartPosition;

        public ProjectileResource(AttackResource attackResource, EffectsResource effectsResource, Vector2 Direction, float Speed, Vector2 StartPosition)
        {
            this.attackResource = attackResource;
            this.effectsResource = effectsResource;
            this.Direction = Direction;
            this.Speed = Speed;
            this.StartPosition = StartPosition;
        }
    }
}
