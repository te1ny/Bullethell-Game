using Godot;
using GodotUtilities;

namespace Game.Objects.Projectiles
{
    [Scene]
    public partial class Arrow : Projectile
    {
        [Node] private AnimatedSprite2D effects;
        [Node] private PointLight2D pointLight2D;

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
            (projectileResource.effectsResource.Effects["Freezing"] || 
            projectileResource.effectsResource.Effects["Burning"] ||
            projectileResource.effectsResource.Effects["Poisoning"])
            {
                effects.Visible = true;
                pointLight2D.Visible = true;

                if 
                (projectileResource.effectsResource.Effects["Freezing"] && 
                projectileResource.effectsResource.Effects["Poisoning"])
                {
                    effects.Play("Double");
                    pointLight2D.Color = new Color("df0015"); // red
                }
                else if 
                (projectileResource.effectsResource.Effects["Burning"] && 
                projectileResource.effectsResource.Effects["Poisoning"])
                {
                    effects.Play("Double");
                    pointLight2D.Color = new Color("df0015"); // red
                }
                else if 
                (projectileResource.effectsResource.Effects["Freezing"])
                {
                    effects.Play("Freezing");
                    pointLight2D.Color = new Color("8bcaff"); // light blue
                }
                else if 
                (projectileResource.effectsResource.Effects["Burning"])
                {
                    effects.Play("Burning");
                    pointLight2D.Color = new Color("ff5020"); // orange
                }
                else if 
                (projectileResource.effectsResource.Effects["Poisoning"])
                {
                    effects.Play("Poisoning");
                    pointLight2D.Color = new Color("8bca39"); // green
                }
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
        }
    }
}