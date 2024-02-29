using Godot;
using Game.Resources;

namespace Game.Components
{
    public partial class HitboxComponent : Area2D
    {
        [Signal] public delegate void HitEventHandler(AttackResource attackResource, EffectsResource effectsResource);

        private HealthComponent healthComponent;
        private EffectsComponent effectsComponent;

        public override void _Ready()
        {
            Hit += OnHit;
            healthComponent = GetParent().GetNodeOrNull("HealthComponent") as HealthComponent;
            effectsComponent = GetParent().GetNodeOrNull("EffectsComponent") as EffectsComponent;
            Monitoring = false;
        }

        private void OnHit(AttackResource attackResource, EffectsResource effectsResource)
        {
            healthComponent.Damage(attackResource.damage);
            effectsComponent.EmitSignal(EffectsComponent.SignalName.ProcessEffects, effectsResource);
        }
    }   
}
