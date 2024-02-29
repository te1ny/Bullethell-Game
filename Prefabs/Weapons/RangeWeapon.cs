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

                if (generator.RandiRange(1, 100) <= (int)gameManager.SkillParameters["Freezing"]["Chance"] && (int)gameManager.SkillParameters["Freezing"]["Level"] > 0)
                {
                    effectsResource.Effects["Freezing"] = true;
                }
                if (generator.RandiRange(1, 100) <= (int)gameManager.SkillParameters["Burning"]["Chance"] && (int)gameManager.SkillParameters["Burning"]["Level"] > 0)
                {
                    effectsResource.Effects["Burning"] = true;
                }
                if (generator.RandiRange(1, 100) <= (int)gameManager.SkillParameters["Poisoning"]["Chance"] && (int)gameManager.SkillParameters["Poisoning"]["Level"] > 0)
                {
                    effectsResource.Effects["Poisoning"] = true;
                }

                if (effectsResource.Effects["Freezing"] && effectsResource.Effects["Burning"])
                {
                    effectsResource.Effects["Freezing"] = false;
                    effectsResource.Effects["Burning"] = false;
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

