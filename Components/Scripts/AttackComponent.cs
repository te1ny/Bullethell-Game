using Godot;
using Godot.Collections;
using Game.Resources;

namespace Game.Components
{
    public partial class AttackComponent : Area2D
    {
        [Signal] public delegate void AttackEventHandler(AttackResource attackResource, EffectsResource effectsResource);

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
                    Monitoring = false;
                }
                else
                {
                    Monitoring = true;
                }
            }
        }

        public override void _Ready()
        {
            Attack += OnAttack;
            Monitorable = false;
        }

        private void OnAttack(AttackResource attackResource, EffectsResource effectsResource)
        {
            Array<Area2D> Enemys = GetOverlappingAreas();
            if (Enemys.Count != 0)
            {
                for (int i = 0; i < Enemys.Count; i++)
                {
                    Enemys[i].EmitSignal(HitboxComponent.SignalName.Hit, attackResource, effectsResource);
                }
            }
        }
    }
}