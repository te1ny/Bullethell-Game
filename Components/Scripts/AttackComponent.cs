using Godot;
using Godot.Collections;
using Game.Resources;

namespace Game.Components
{
    public partial class AttackComponent : Area2D
    {
        [Signal] public delegate void AttackEventHandler(AttackResource attackResource, EffectsResource effectsResource);

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