using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using TankEngine.DataStructures.Geometrics;
using TankEngine.EntityComponentSystem;
using TankEngine.Factories;

namespace Tank.Builders
{
    /// <summary>
    /// Standard simple bullet builder
    /// </summary>
    class BaseBulletBuilder : BaseBuilder
    {
        /// <summary>
        /// The animation frames
        /// </summary>
        private readonly List<Rectangle> animationFrames;

        /// <summary>
        /// The collider for the bullets
        /// </summary>
        private readonly Rectangle collider;

        /// <summary>
        /// The source of the bullet
        /// </summary>
        private readonly Rectangle source;

        /// <summary>
        /// The texture for the bullets
        /// </summary>
        private readonly Texture2D texture;

        /// <summary>
        /// The explosion factory to use
        /// </summary>
        private readonly IFactory<List<IComponent>> explosionFactory;

        /// <summary>
        /// The area to make some damage
        /// </summary>
        private readonly Circle damageArea;

        /// <summary>
        /// The name for the animation
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The rotation center to use
        /// </summary>
        private readonly Vector2 rotationCenter;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="animationFrames">The aniamations frames</param>
        /// <param name="texture">The texture to use</param>
        /// <param name="explosionFactory">The explosion factory to use</param>
        public BaseBulletBuilder(
            List<Rectangle> animationFrames,
            Texture2D texture,
            IFactory<List<IComponent>> explosionFactory
            )
        {
            collider = new Rectangle(0, 0, 16, 16);
            source = new Rectangle(0, 0, 32, 32);
            this.animationFrames = animationFrames;
            this.texture = texture;
            this.explosionFactory = explosionFactory;
            damageArea = new Circle(0, 0, 16);
            name = "Idle";
            rotationCenter = new Vector2(source.Width / 2, source.Height / 2);
        }

        /// <inheritdoc/>
        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> returnComponents = new List<IComponent>();
            if (entityManager == null)
            {
                return returnComponents;
            }
            PlaceableComponent position = entityManager.CreateComponent<PlaceableComponent>();
            position.Position = Vector2.Zero;

            MoveableComponent moveable = entityManager.CreateComponent<MoveableComponent>();
            moveable.Velocity = Vector2.Zero;
            moveable.ApplyPhysic = true;
            moveable.PhysicRotate = true;
            moveable.Mass = 7;


            VisibleComponent visuals = entityManager.CreateComponent<VisibleComponent>();
            visuals.Texture = texture;
            visuals.Source = source;
            visuals.Destination = collider;
            visuals.SingleTextureSize = collider;
            //visuals.DrawMiddle = true;
            visuals.RotationCenter = rotationCenter;
            MapColliderTag colliderTag = entityManager.CreateComponent<MapColliderTag>();

            AnimationComponent animation = entityManager.CreateComponent<AnimationComponent>();
            animation.FrameSeconds = 0.02f;
            animation.SpriteSources = animationFrames;
            animation.Name = name;
            animation.Active = true;
            animation.Loop = true;
            animation.PingPong = true;
            ColliderComponent colliderComponent = entityManager.CreateComponent<ColliderComponent>();
            colliderComponent.Collider = collider;
            colliderComponent.FireCollideEvent = true;
            colliderComponent.MapCollision = true;

            DamageComponent damage = entityManager.CreateComponent<DamageComponent>();
            damage.DamageTerrain = true;
            damage.CenterDamageValue = 50;
            damage.DamageArea = damageArea;
            damage.EffectFactory = explosionFactory;

            returnComponents.Add(position);
            returnComponents.Add(moveable);
            returnComponents.Add(visuals);
            returnComponents.Add(animation);
            returnComponents.Add(colliderTag);
            returnComponents.Add(colliderComponent);
            returnComponents.Add(damage);
            return returnComponents;
        }
    }
}
