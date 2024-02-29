using Game.Objects.Projectiles;
using Godot;
using System;

namespace Game.Components
{   
    public partial class MovementComponent : Node2D
    {
        private const float speedLimit = 10000;

        private Vector2 direction = Vector2.Zero;
        public Vector2 Direction
        {
            get
            {
                return direction;
            }
            set
            {
                if(value.IsNormalized())
                {
                    direction = value;
                }
                else
                {
                    direction = value.Normalized();
                }
            }
        }

        public float BaseSpeed;

        public override void _Ready()
        {
            BaseSpeed = Speed;
        }

        private float speed = 100f;
        [Export] public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = Mathf.Clamp(value, 0f, speedLimit);
            }
        }

        public void Move(CharacterBody2D characterBody2D)
        {
            characterBody2D.Velocity = Speed * Direction;
            characterBody2D.MoveAndSlide();
        }

        public void Move(Projectile projectile)
        {
            projectile.GlobalPosition += Speed * Direction * Convert.ToSingle(GetPhysicsProcessDeltaTime());
        }
    }
}