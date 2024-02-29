using Game.Weapons;
using Godot;

namespace Game.Managers
{
    public partial class PlayerManager : Manager
    {
        private RangeWeapon bow {get; set;}

        public override void _Ready()
        {
            base._Ready();
            bow = GetParent().GetNodeOrNull("Bow") as RangeWeapon;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Input.IsActionJustPressed("lcm"))
            {
                bow.EmitSignal(RangeWeapon.SignalName.Shoot);
            }

            // TEST MODULE DOWN

            if (Input.IsActionJustPressed("rcm"))
            {
                bow.weaponResource.Autofire = !bow.weaponResource.Autofire;
            }
            // TEST MODULE UP

            Vector2 CurrentDirection = Input.GetVector("a", "d", "w", "s").Normalized();

            movementComponent.Direction = CurrentDirection;
            movementComponent.Move(entity);
        }
    }
}