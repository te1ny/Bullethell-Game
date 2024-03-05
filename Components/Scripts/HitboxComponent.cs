using Godot;
using Game.Resources;

namespace Game.Components
{
    public partial class HitboxComponent : Area2D
    {
        [Signal] public delegate void HitEventHandler(AttackResource attackResource, EffectsResource effectsResource);

        private HealthComponent healthComponent;
        private EffectsComponent effectsComponent;

        private bool disabled = false;
        public bool Disabled
        {
            get
            {
                return disabled;
            }
            set
            {
                disabled = value;
                if (disabled)
                {
                    Monitorable = false;
                }
                else
                {
                    Monitorable = true;
                }
            }
        }

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
