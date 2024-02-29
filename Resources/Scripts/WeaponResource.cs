using Godot;

namespace Game.Resources
{
    [GlobalClass]
    public partial class WeaponResource : Resource
    {
        public enum AttackType {Melee, Range};

        private const float limitLaunchSpeedMIN = 200f;
        private const float limitLaunchSpeedMAX = 1500f;

        private int ammoCount = 5;
        private float reloadTime = 1f;
        private float attackDelay = 0.5f;
        private float launchSpeed = 400f;
        
        [ExportGroup("User Parameters")]
        [Export] public bool ManualControl = false;
        
        [ExportGroup("Weapon Parameters")]
        [Export] public bool Autofire = false;
        [Export] public AttackType attackType = AttackType.Range;
        [Export] public int AmmoCount
        {
            get
            {
                return ammoCount;
            }
            set
            {
                ammoCount = value;
            }
        }
        [Export] public float ReloadTime
        {
            get
            {
                return reloadTime;
            }
            set
            {
                reloadTime = value;
            }
        }
        [Export] public float AttackDelay
        {
            get
            {
                return attackDelay;
            }
            
            set
            {
                attackDelay = value;
            }
        }
        [Export] public float LaunchSpeed
        {
            get
            {
                return launchSpeed;
            }
            
            set
            {
                launchSpeed = Mathf.Clamp(value, limitLaunchSpeedMIN, limitLaunchSpeedMAX);
            }
        }
    }
}  