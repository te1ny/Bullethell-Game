using System;
using Game.Managers;
using Game.Resources;
using Godot;

namespace Game.Components
{
	public partial class EffectsComponent : Node2D
	{
		[Signal] public delegate void ProcessEffectsEventHandler(EffectsResource effectsResource);

        [Export] private bool IsPlayer = false;

        private GameManager gameManager;
        private EnemyManager enemyManager;
        private HealthComponent healthComponent;
        private MovementComponent movementComponent;
        private AnimatedSprite2D animatedSprite2D;

        private Sprite2D FreezingStaticEffect;
        private AnimatedSprite2D AnimatedEffect;

        private int freezingTicks = 0;
        private int burningTicks = 0;
        private int poisoningTicks = 0;

        private float freezingTimer = 0;
        private float burningTimer = 0;
        private float poisoningTimer = 0;

        public override void _Ready()
        {
            SetPhysicsProcess(false);
            if (!IsPlayer)
            {
                enemyManager = GetParent().GetNodeOrNull("EnemyManager") as EnemyManager;
            }
            this.ProcessEffects += OnProcessEffects;

            gameManager = GetNodeOrNull("/root/GameManager") as GameManager;

            healthComponent = GetParent().GetNodeOrNull("HealthComponent") as HealthComponent;
            movementComponent = GetParent().GetNodeOrNull("MovementComponent") as MovementComponent;
            animatedSprite2D = GetParent().GetNodeOrNull("AnimatedSprite2D") as AnimatedSprite2D;

            FreezingStaticEffect = GetNodeOrNull("StaticEffect") as Sprite2D;
            AnimatedEffect = GetNodeOrNull("AnimatedEffects") as AnimatedSprite2D;

            FreezingStaticEffect.Visible = false;
            AnimatedEffect.Visible = false;
        }

        public override void _PhysicsProcess(double delta)
        {
            if (freezingTicks == 0f && burningTicks == 0f && poisoningTicks == 0f
            && freezingTimer == 0f && burningTimer == 0f && poisoningTimer == 0f)
            {
                SetPhysicsProcess(false);
            }

            CheckToEnable();
            CheckToDisable();
            ChangeTicks();

            freezingTimer = Convert.ToSingle(Godot.Mathf.Clamp(freezingTimer - delta, 0f, 10f));
            burningTimer = Convert.ToSingle(Godot.Mathf.Clamp(burningTimer - delta, 0f, 10f));
            poisoningTimer = Convert.ToSingle(Godot.Mathf.Clamp(poisoningTimer - delta, 0f, 10f));
        }

        private void OnProcessEffects(EffectsResource effectsResource)
		{
            if (!IsPlayer)
            {
                if (effectsResource.Effects["Freezing"])
                {
                    freezingTicks = Godot.Mathf.Max((int)gameManager.SkillParameters["Freezing"]["Ticks"], freezingTicks);
                    enemyManager.SetPhysicsProcess(false);
                    animatedSprite2D.Stop();
                    movementComponent.Speed = 0f;
                }
                if (effectsResource.Effects["Burning"])
                {
                    burningTicks = Godot.Mathf.Max((int)gameManager.SkillParameters["Burning"]["Ticks"], burningTicks);
                }
                if (effectsResource.Effects["Poisoning"])
                {
                    poisoningTicks = Godot.Mathf.Max((int)gameManager.SkillParameters["Poisoning"]["Ticks"], poisoningTicks);
                    movementComponent.Speed *= 1f - ((int)gameManager.SkillParameters["Poisoning"]["Slowing"] / 100f);
                }

                if (freezingTicks > 0 && burningTicks > 0)
                {
                    freezingTicks = 0;
                    burningTicks = 0;
                }

                freezingTimer = (float)gameManager.SkillParameters["Freezing"]["TickDuration"];
                burningTimer = (float)gameManager.SkillParameters["Burning"]["TickDuration"];
                poisoningTimer = (float)gameManager.SkillParameters["Poisoning"]["TickDuration"];

                SetPhysicsProcess(true);
            }
		}

        private void CheckToEnable()
        {
            if (freezingTicks > 0f && poisoningTicks > 0f)
            {
                FreezingStaticEffect.Visible = true;
                AnimatedEffect.Visible = true;
                AnimatedEffect.Play("Poisoning");
            }
            else if (burningTicks > 0f && poisoningTicks > 0f)
            {
                AnimatedEffect.Visible = true;
                AnimatedEffect.Play("Double");
            }
            else if (freezingTicks > 0f)
            {
                FreezingStaticEffect.Visible = true;
            }
            else if (burningTicks > 0f)
            {
                AnimatedEffect.Visible = true;
                AnimatedEffect.Play("Burning");
            }
            else if (poisoningTicks > 0f)
            {
                AnimatedEffect.Visible = true;
                AnimatedEffect.Play("Poisoning");
            }
        }

        private void CheckToDisable()
        {
            if (freezingTicks == 0f && poisoningTicks == 0f && burningTicks == 0f)
            {
                FreezingStaticEffect.Visible = false;
                AnimatedEffect.Visible = false;
                AnimatedEffect.Stop();
                enemyManager.SetPhysicsProcess(true);
                animatedSprite2D.Play("Stalk");
            }
            else if (freezingTicks == 0f)
            {
                FreezingStaticEffect.Visible = false;
                enemyManager.SetPhysicsProcess(true);
                animatedSprite2D.Play("Stalk");
            }
            else if (burningTicks == 0f && poisoningTicks == 0)
            {
                AnimatedEffect.Visible = false;
                AnimatedEffect.Stop();
            }
            if (poisoningTicks == 0)
            {
                movementComponent.Speed = movementComponent.BaseSpeed;
            }
        }

        private void ChangeTicks()
        {
            if (freezingTimer == 0f && freezingTicks > 0)
            {
                freezingTicks -= 1;
                freezingTimer = (float)gameManager.SkillParameters["Freezing"]["TickDuration"];
            }
            if (burningTimer == 0f && burningTicks > 0)
            {
                burningTicks -= 1;
                burningTimer = (float)gameManager.SkillParameters["Burning"]["TickDuration"];
                healthComponent.Damage((float)gameManager.SkillParameters["Burning"]["Damage"]);
            }
            if (poisoningTimer == 0f && poisoningTicks > 0)
            {
                poisoningTicks -= 1;
                poisoningTimer = (float)gameManager.SkillParameters["Poisoning"]["TickDuration"];
                GD.Print("Poison Ticks - ", poisoningTicks);
                healthComponent.Damage((float)gameManager.SkillParameters["Poisoning"]["Damage"]);
            }
        }
    }
}

