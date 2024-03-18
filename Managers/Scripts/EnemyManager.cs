using Game.Resources;
using Godot;
using Game.Components;

namespace Game.Managers
{
    public partial class EnemyManager : Manager
    {
        [Export] private AttackResource attackResource;
        [Export] private CostResource costResource;
        private EffectsResource effectsResource = new EffectsResource();
        [Export] private float AttackDelay;

        private Node2D enemyNode;
        private Node2D place;
        private CharacterBody2D player;
        private AttackComponent attackComponent;
        private EffectsComponent effectsComponent;

        private Timer attackTimer;

        public override void _Ready()
        {
            base._Ready();
            
            enemyNode = entity.GetParent() as Node2D;
            place = enemyNode.GetParent() as Node2D;
            player = place.GetNodeOrNull("Player") as CharacterBody2D;

            healthComponent.Died += OnDied;

            attackComponent = entity.GetNodeOrNull("AttackComponent") as AttackComponent;
            effectsComponent = entity.GetNodeOrNull("EffectsComponent") as EffectsComponent;

            attackTimer = new Timer();
            attackTimer.WaitTime = AttackDelay;
            attackTimer.Autostart = false;
            attackTimer.OneShot = true;
            AddChild(attackTimer);
        }

        private Vector2 directionToPlayer => player.GlobalPosition - entity.GlobalPosition;

        public override void _PhysicsProcess(double delta)
        {
            movementComponent.Direction = directionToPlayer;
            movementComponent.Move(entity);
            
            if (directionToPlayer.X * directionToPlayer.X + directionToPlayer.Y * directionToPlayer.X > 400)
            {
                attackComponent.Monitoring = false;
            }
            else
            {
                attackComponent.Monitoring = true;
                if (attackTimer.IsStopped() && attackComponent.HasOverlappingAreas() && !attackComponent.Disabled)
                {
                    attackComponent.EmitSignal(AttackComponent.SignalName.Attack, attackResource, effectsResource);
                    attackTimer.Start();
                }
            }

        }

        private async void OnDied()
        {
            SetPhysicsProcess(false);

            movementComponent.Disabled = true;
            attackComponent.Disabled = true;
            hitboxComponent.Disabled = true;
            collisionShape2D.Disabled = true;

            effectsComponent.Visible = false;
            shadowComponent.Visible = false;

            animatedSprite2D.Modulate = new Color(1,1,1,0);

            gameManager.statsTracker.AddMoney(costResource.Money);
            gameManager.statsTracker.AddExperience(costResource.Experience);

            await ToSignal(damageIndicator.animationPlayer, AnimationPlayer.SignalName.AnimationFinished);
            entity.QueueFree();
        }
    }
}