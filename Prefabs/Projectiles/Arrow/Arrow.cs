using Godot;
using GodotUtilities;

namespace Game.Objects.Projectiles
{
	[Scene]
	public partial class Arrow : Projectile
	{
		[Node] private Node2D Particles;

		public override void _Notification(int what)
		{
			base._Notification(what);
			if (what == NotificationSceneInstantiated)
			{
				this.WireNodes();
			}
		}

		public override void _Ready()
		{
			base._Ready();
			if 
			(projectileResource.effectsResource.Effects[0] || 
			projectileResource.effectsResource.Effects[1] ||
			projectileResource.effectsResource.Effects[2])
			{
				if 
				(projectileResource.effectsResource.Effects[0] && 
				projectileResource.effectsResource.Effects[2])
				{
					Particles.GetNode<GpuParticles2D>("FreezingPoisoningParticle").Emitting = true;
					Particles.GetNode<GpuParticles2D>("FreezingPoisoningParticle").Visible = true;
				}
				else if 
				(projectileResource.effectsResource.Effects[1] && 
				projectileResource.effectsResource.Effects[2])
				{
					Particles.GetNode<GpuParticles2D>("BurningPoisoningParticle").Emitting = true;
					Particles.GetNode<GpuParticles2D>("BurningPoisoningParticle").Visible = true;
				}
				else if 
				(projectileResource.effectsResource.Effects[0])
				{
					Particles.GetNode<GpuParticles2D>("FreezingParticle").Emitting = true;
					Particles.GetNode<GpuParticles2D>("FreezingParticle").Visible = true;
				}
				else if 
				(projectileResource.effectsResource.Effects[1])
				{
					Particles.GetNode<GpuParticles2D>("BurningParticle").Emitting = true;
					Particles.GetNode<GpuParticles2D>("BurningParticle").Visible = true;
				}
				else if 
				(projectileResource.effectsResource.Effects[2])
				{
					Particles.GetNode<GpuParticles2D>("PoisoningParticle").Emitting = true;
					Particles.GetNode<GpuParticles2D>("PoisoningParticle").Visible = true;
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			base._PhysicsProcess(delta);
		}
	}
}
