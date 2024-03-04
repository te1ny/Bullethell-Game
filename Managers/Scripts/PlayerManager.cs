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
            if (Input.IsActionJustPressed("i"))
            {
                gameManager.AddSkillLevel(0);
            }
            if (Input.IsActionJustPressed("o"))
            {
                gameManager.AddSkillLevel(1);
            }
            if (Input.IsActionJustPressed("p"))
            {
                gameManager.AddSkillLevel(2);
            }

            if (Input.IsActionJustPressed("j"))
            {
                gameManager.SubtractSkillLevel(0);
            }
            if (Input.IsActionJustPressed("k"))
            {
                gameManager.SubtractSkillLevel(1);
            }
            if (Input.IsActionJustPressed("l"))
            {
                gameManager.SubtractSkillLevel(2);
            }
            // TEST MODULE UP

            Vector2 CurrentDirection = Input.GetVector("a", "d", "w", "s").Normalized();

            movementComponent.Direction = CurrentDirection;
            movementComponent.Move(entity);
        }
    }
}