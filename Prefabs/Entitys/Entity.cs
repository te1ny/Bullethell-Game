using Game.Components;
using Godot;
using GodotUtilities;

namespace Game.Objects.Entitys
{
    [Scene]
    public partial class Entity : CharacterBody2D
    {
        [Node] public MovementComponent movementComponent;
        [Node] public AnimatedSprite2D animatedSprite2D;

        public override void _Notification(int what)
        {
            if (what == NotificationSceneInstantiated)
            {
                this.WireNodes();
            }
        }

        public void LookToDirection()
        {
            if (movementComponent.Direction.X > 0 && animatedSprite2D.FlipH == true)
            {
                animatedSprite2D.FlipH = false;
            }
            if (movementComponent.Direction.X < 0 && animatedSprite2D.FlipH == false)
            {
                animatedSprite2D.FlipH = true;
            }
        }

        public void LookToMouse()
        {
            if ((GetGlobalMousePosition() - GlobalPosition).Normalized().X > 0 && animatedSprite2D.FlipH == true)
            {
                animatedSprite2D.FlipH = false;
            }
            if ((GetGlobalMousePosition() - GlobalPosition).Normalized().X < 0 && animatedSprite2D.FlipH == false)
            {
                animatedSprite2D.FlipH = true;
            }
        }
    }
}