
namespace Game.Objects.Entitys
{
	public partial class Golem : Entity
	{
		public override void _Ready()
		{
			animatedSprite2D.Play("Stalk");
		}

		public override void _PhysicsProcess(double delta)
		{
			LookToDirection();
		}
	}
}