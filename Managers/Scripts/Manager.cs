using Game.Components;
using Game.Indicators;
using Godot;

namespace Game.Managers
{
    public partial class Manager : Node2D
    {
        public GameManager gameManager {get; set;}

        public CharacterBody2D entity {get; set;}

        public HealthComponent healthComponent {get; set;}
        public MovementComponent movementComponent {get; set;}
        public DamageIndicator damageIndicator {get; set;}
        public HitboxComponent hitboxComponent {get; set;}
        public CollisionShape2D collisionShape2D {get; set;}
        public AnimatedSprite2D animatedSprite2D {get; set;}
        public Node2D shadowComponent {get; set;}

        public override void _Ready()
        {
            gameManager = GetNodeOrNull("/root/GameManager") as GameManager;

            entity = GetParent() as CharacterBody2D;

            healthComponent = entity.GetNodeOrNull("HealthComponent") as HealthComponent;

            movementComponent = entity.GetNodeOrNull("MovementComponent") as MovementComponent;

            damageIndicator = entity.GetNodeOrNull("DamageIndicator") as DamageIndicator;

            hitboxComponent = entity.GetNodeOrNull("HitboxComponent") as HitboxComponent;

            collisionShape2D = entity.GetNodeOrNull("CollisionShape2D") as CollisionShape2D;

            animatedSprite2D = entity.GetNodeOrNull("AnimatedSprite2D") as AnimatedSprite2D;

            shadowComponent = entity.GetNodeOrNull("ShadowComponent") as Node2D;
        }
    }
}