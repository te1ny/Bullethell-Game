using Godot;
using GodotUtilities.Logic;

namespace Game.Objects.Entitys
{
	public partial class Player : Entity
	{
		public DelegateStateMachine stateMachine = new DelegateStateMachine();

		public override void _Ready()
		{
			stateMachine.AddStates(StateIdle);
			stateMachine.AddStates(StateRun);
			stateMachine.SetInitialState(StateIdle);
		}

		public override void _PhysicsProcess(double delta)
		{
			LookToMouse();
			stateMachine.Update();
		}

		private void StateIdle()
		{
			if (Velocity != Vector2.Zero)
			{
				stateMachine.ChangeState(StateRun);
				return;
			}
			animatedSprite2D.Play("Idle");
		}

		private void StateRun()
		{
			if (Velocity == Vector2.Zero)
			{
				stateMachine.ChangeState(StateIdle);
				return;
			}
			animatedSprite2D.Play("Run");
		}
	}
}

