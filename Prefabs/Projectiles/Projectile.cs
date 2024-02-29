using System;
using Game.Components;
using Game.Resources;
using Godot;
using GodotUtilities;

namespace Game.Objects.Projectiles
{   
    [Scene]
    public partial class Projectile : Node2D
    {
        public ProjectileResource projectileResource {get; set;}

        [Node] private AttackComponent attackComponent {get; set;}
        [Node] private MovementComponent movementComponent {get; set;}  

        public override void _Notification(int what)
        {
            if (what == NotificationSceneInstantiated)
            {
                this.WireNodes();
            }
        }

        public override void _Ready()
        {
            LookAt(projectileResource.Direction);
            GlobalPosition = projectileResource.StartPosition;
            movementComponent.Speed = projectileResource.Speed;    
            movementComponent.Direction = projectileResource.Direction;
        }

        private double deleteTimer = 0;

        public override void _PhysicsProcess(double delta)
        {
            deleteTimer += delta;
            if (attackComponent.HasOverlappingAreas())
            {
                attackComponent.EmitSignal(AttackComponent.SignalName.Attack, projectileResource.attackResource, projectileResource.effectsResource);
                this.QueueFree();
            }
            if (deleteTimer >= 5)
            {
                this.QueueFree();
            }
            movementComponent.Move(this);
        }
    }
}