using Godot;
using Game.Resources;
using Game.Objects.Projectiles;
using Game.Managers;

namespace Game.Weapons
{
    public partial class RangeWeapon : Node2D
    {
        [Signal] public delegate void ShootEventHandler();

        public GameManager gameManager; 

        // Resources
        [ExportGroup("Projectile Settings")]
        [Export] public PackedScene projectile;

        [ExportGroup("Attack Settings")]
        [Export] public AttackResource attackResource;

        [ExportGroup("Weapon Settings")]
        [Export] public WeaponResource weaponResource;

        public Node PlaceNode;
        public Node ProjectilesNode;

        public Timer attackDelayTimer = new Timer();
        public Timer reloadTimer = new Timer();

        private int totalAmmo;
        public int TotalAmmo
        {
            get
            {
                return totalAmmo;
            }
            set
            {
                totalAmmo = value;
                totalAmmo = Mathf.Clamp(totalAmmo, 0, weaponResource.AmmoCount);

                if (totalAmmo == 0)
                {
                    reloadTimer.Start();
                }
            }
        }

        public RandomNumberGenerator generator = new RandomNumberGenerator();

        public override void _Ready()
        {
            Shoot += OnShoot;

            gameManager = GetNodeOrNull("/root/GameManager") as GameManager;

            TotalAmmo = weaponResource.AmmoCount;

            attackDelayTimer.WaitTime = weaponResource.AttackDelay;
            attackDelayTimer.OneShot = true;
            attackDelayTimer.Autostart = false;
            AddChild(attackDelayTimer);

            reloadTimer.WaitTime = weaponResource.ReloadTime;
            reloadTimer.OneShot = true;
            reloadTimer.Autostart = false;
            reloadTimer.Timeout += OnReloadTimerTimeout;
            AddChild(reloadTimer);

            if (weaponResource.ManualControl)
            {
                PlaceNode = GetParent().GetParent();
                ProjectilesNode = PlaceNode.GetNodeOrNull("Projectiles");
            }
            else
            {
                PlaceNode = GetParent().GetParent().GetParent();
                ProjectilesNode = PlaceNode.GetNodeOrNull("Projectiles");
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (weaponResource.ManualControl)
            {
                LookAt(GetGlobalMousePosition());
            }
            if (weaponResource.Autofire)
            {
                this.EmitSignal(RangeWeapon.SignalName.Shoot);
            }
        }

        private void OnReloadTimerTimeout()
        {
            TotalAmmo = weaponResource.AmmoCount;
        }

        private void OnShoot()
        {
            if (attackDelayTimer.IsStopped() && reloadTimer.IsStopped())
            {
                EffectsResource effectsResource = new EffectsResource();

                if (generator.RandiRange(1, 100) <= (int)gameManager.skillsTracker.GetData(0, Trackers.SkillsTracker.Properties.Chance) && (int)gameManager.skillsTracker.GetData(0, Trackers.SkillsTracker.Properties.Level) > 0)
                {
                    effectsResource.Effects[0] = true;
                }
                if (generator.RandiRange(1, 100) <= (int)gameManager.skillsTracker.GetData(1, Trackers.SkillsTracker.Properties.Chance) && (int)gameManager.skillsTracker.GetData(1, Trackers.SkillsTracker.Properties.Level) > 0)
                {
                    effectsResource.Effects[1] = true;
                }
                if (generator.RandiRange(1, 100) <= (int)gameManager.skillsTracker.GetData(2, Trackers.SkillsTracker.Properties.Chance) && (int)gameManager.skillsTracker.GetData(2, Trackers.SkillsTracker.Properties.Level) > 0)
                {
                    effectsResource.Effects[2] = true;
                }

                if (effectsResource.Effects[0] && effectsResource.Effects[1])
                {
                    effectsResource.Effects[0] = false;
                    effectsResource.Effects[1] = false;
                }

                Vector2 ShootDirection = (GetGlobalMousePosition() - GlobalPosition).Normalized();
                ProjectileResource projectileResource = new ProjectileResource(attackResource, effectsResource, ShootDirection, weaponResource.LaunchSpeed, GlobalPosition);

                Projectile newProjectile = projectile.Instantiate() as Projectile;
                newProjectile.projectileResource = projectileResource;
                
                ProjectilesNode.AddChild(newProjectile);

                TotalAmmo -= 1;
                if (reloadTimer.IsStopped())
                {
                    attackDelayTimer.Start();
                }
            }
        }
    }
}

