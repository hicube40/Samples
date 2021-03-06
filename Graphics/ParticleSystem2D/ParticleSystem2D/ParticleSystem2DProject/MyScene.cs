#region Using Statements
using System;
using WaveEngine.Common;
using WaveEngine.Common.Graphics;
using WaveEngine.Common.Math;
using WaveEngine.Components.Cameras;
using WaveEngine.Components.Graphics2D;
using WaveEngine.Components.Graphics3D;
using WaveEngine.Components.UI;
using WaveEngine.Framework;
using WaveEngine.Framework.Graphics;
using WaveEngine.Framework.Resources;
using WaveEngine.Framework.Services;
using WaveEngine.Materials;
#endregion

namespace ParticleSystem2DProject
{
    public class MyScene : Scene
    {
        protected override void CreateScene()
        {
            FixedCamera2D camera2D = new FixedCamera2D("camera")
            {
                BackgroundColor = Color.Black,
            };
            EntityManager.Add(camera2D);            

            var textBlock = new TextBlock()
            {
                Text = "Touch the screen to control fireball",
                HorizontalAlignment = WaveEngine.Framework.UI.HorizontalAlignment.Left,
                VerticalAlignment = WaveEngine.Framework.UI.VerticalAlignment.Top,
                Margin = new WaveEngine.Framework.UI.Thickness(20),
                DrawOrder = 0.1f
            };

            var background = new Entity("back")
                .AddComponent(new Transform2D()
                {
                    DrawOrder = 1,
                    XScale = WaveServices.Platform.ScreenWidth / 256f,
                    YScale = WaveServices.Platform.ScreenHeight / 256f,
                })
                .AddComponent(new Sprite("Content/backgroundMeteors.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.Opaque));

            var mountains = new Entity("mountains")
                .AddComponent(new Transform2D()
                {
                    Y = WaveServices.Platform.ScreenHeight - 128,
                    XScale = WaveServices.Platform.ScreenWidth / 1024f
                })
                .AddComponent(new Sprite("Content/meteorMountains.wpk"))
                .AddComponent(new SpriteRenderer(DefaultLayers.GUI));

            var explosionParticles = new Entity("explosionParticles")
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateExplosion())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/particleFire.wpk", DefaultLayers.Additive)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));

            var dinos = new Entity("dinos")
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateDinosaurs())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/dinoParticle.wpk", DefaultLayers.Alpha)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));

            var meteorSmoke = new Entity("smoke")
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateSmokeParticle())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/meteorSmoke.wpk", DefaultLayers.Alpha)))
                .AddComponent(new ParticleSystemRenderer2D("smokeRenderer"));

            var meteorFire = new Entity("fire")
                .AddComponent(new Transform2D())
                .AddComponent(ParticleSystemFactory.CreateFireParticle())
                .AddComponent(new Material2D(new BasicMaterial2D("Content/particleFire.wpk", DefaultLayers.Additive)))
                .AddComponent(new ParticleSystemRenderer2D("particleRenderer"));

            var explosion = new Entity("explosion")
                .AddComponent(new Transform2D())
                .AddComponent(new ExplosionBehavior())
                .AddChild(explosionParticles)
                .AddChild(dinos);

            var meteor = new Entity("meteor")
                .AddComponent(new Transform2D())
                .AddComponent(new MeteorBehavior(explosion.FindComponent<ExplosionBehavior>()))
                .AddChild(meteorSmoke)
                .AddChild(meteorFire);

            this.EntityManager.Add(textBlock);
            this.EntityManager.Add(mountains);
            this.EntityManager.Add(meteor);
            this.EntityManager.Add(explosion);
            this.EntityManager.Add(background);
        }
    }
}
