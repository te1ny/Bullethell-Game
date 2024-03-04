using System;
using Game.Components;
using Godot;
using GodotUtilities;

namespace Game.Indicators
{
	[Scene]
	public partial class DamageIndicator : Node2D
	{
		[Node] private Label label {get; set;}
		[Node] public AnimationPlayer animationPlayer {get; set;}

		private CharacterBody2D entity {get; set;}
		private HealthComponent healthComponent {get; set;}
		private AnimatedSprite2D animatedSprite2D {get; set;}

		private Color visibleModulate {get; set;}

		private ShaderMaterial shader_material {get; set;}

		public override void _Notification(int what)
		{
			if (what == NotificationSceneInstantiated)
			{
				this.WireNodes();
			}
		}

		public override void _Ready()
		{
			label.Text = "0,0";
			visibleModulate = label.SelfModulate;
			label.SelfModulate = new Color(1, 1, 1, 0);

			entity = GetParent() as CharacterBody2D;

			healthComponent = entity.GetNodeOrNull("HealthComponent") as HealthComponent;
			healthComponent.HealthChanged += OnHealthChanged;
			healthComponent.Immortal += OnImmortal;

			animatedSprite2D = entity.GetNodeOrNull("AnimatedSprite2D") as AnimatedSprite2D;

			shader_material = new ShaderMaterial();
			shader_material.Shader = GD.Load<VisualShader>("res://Indicators/DamageIndicator/HitShader.tres");
			
			animatedSprite2D.Material = shader_material;
		}

		private void OnHealthChanged(HealthUpdate healthUpdate)
		{
			float value = healthUpdate.CurrentHealth - healthUpdate.PreviousHealth;
			if (animationPlayer.IsPlaying() && label.Text != "IMMORTAL")
			{
				label.Text = Convert.ToString(Convert.ToSingle(label.Text) + value);
				animationPlayer.Stop();
			}
			else
			{
				label.Text = Convert.ToString(value);
			}
			animationPlayer.Play("hit");
		}

		private void OnImmortal()
		{
			animationPlayer.Stop();
			animationPlayer.Play("immortal");
		}

		public void EnableShader()
		{
			shader_material.SetShaderParameter("Enabled", true);
		}
		
		public void DisableShader()
		{
			shader_material.SetShaderParameter("Enabled", false);
		}
	}
}

