using Game.Components;
using Game.Indicators;
using Godot;

namespace Game.Managers
{
    public partial class Manager : Node2D
    {
        public GameManager gameManager;

        public CharacterBody2D entity;

        public HealthComponent healthComponent;
        public MovementComponent movementComponent;
        public DamageIndicator damageIndicator;
        public HitboxComponent hitboxComponent;
        public CollisionShape2D collisionShape2D;
        public AnimatedSprite2D animatedSprite2D;
        public Node2D shadowComponent;

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