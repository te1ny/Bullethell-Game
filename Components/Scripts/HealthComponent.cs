using Godot;

namespace Game.Components
{
    [Tool]
    public partial class HealthComponent : Node2D
    {
        [Signal] public delegate void InitializeHealthEventHandler();
        [Signal] public delegate void HealthChangedEventHandler(HealthUpdate healthUpdate);
        [Signal] public delegate void ImmortalEventHandler();
        [Signal] public delegate void DiedEventHandler();

        private const int limitHealth = short.MaxValue;

        private bool immortality = false;
        private float maxHealth = 5f;
        private float currentHealth = 5f;

        public bool Alive => CurrentHealth != 0 ? true : false;
        public float PercentageHealth => (CurrentHealth / MaxHealth) * 100f;

        [Export] public bool Immortality 
        {
            get
            {
                return immortality;
            }
            set
            {
                immortality = value;
            }
        }
        [Export] public float MaxHealth
        {
            get
            {
                return maxHealth;
            }
        
            set
            {
                maxHealth = Mathf.Clamp(value, 0, limitHealth);
            }
        }
        public float CurrentHealth 
        {
            get
            {
                return currentHealth;
            }
        
            set
            {
                HealthUpdate healthUpdate = new HealthUpdate();

                healthUpdate.MaxHealth = MaxHealth;
                healthUpdate.PreviousHealth = CurrentHealth;

                currentHealth = Mathf.Clamp(value, 0f, MaxHealth);
                
                healthUpdate.CurrentHealth = CurrentHealth;
                EmitSignal(HealthComponent.SignalName.HealthChanged, healthUpdate); 
                if (!Alive) { EmitSignal(HealthComponent.SignalName.Died); }
            }
        }

        public override void _Ready()
        {
            InitializeHealth += OnInitializeHealth;
            EmitSignal(HealthComponent.SignalName.InitializeHealth);
        }
    
        private void OnInitializeHealth()
        {
            CurrentHealth = MaxHealth;
        }

        public void SetMaxHealth(float Value)
        {
            MaxHealth = Value;
        }

        public void Damage(float Value)
        {
            if (Alive && !Immortality)
            {
                CurrentHealth -= Value;

            }
            if (Immortality)
            {
                EmitSignal(HealthComponent.SignalName.Immortal);
            }
        }

        public void Heal(float Value)
        {
            if (Alive)
            {
                CurrentHealth += Value;
            }
        }

        public void SetImmortality(bool Value)
        {
            Immortality = Value;
        }
    }

    public partial class HealthUpdate : RefCounted
    {
        public float MaxHealth;
        public float PreviousHealth;
        public float CurrentHealth;

        public bool Alive => CurrentHealth != 0 ? true : false;

        public bool IsHeal => CurrentHealth >= PreviousHealth ? true : false;
        public bool IsDamage => CurrentHealth < PreviousHealth ? true : false;

        public float PercentageHealth => (CurrentHealth / MaxHealth) * 100f;
    }
}
