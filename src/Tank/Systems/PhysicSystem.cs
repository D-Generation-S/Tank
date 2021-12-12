using Microsoft.Xna.Framework;
using System;
using Tank.Components;
using Tank.Events;
using Tank.Events.EntityBased;
using Tank.Events.PhysicBased;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Utils;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will performe physic actions on the entites
    /// </summary>
    class PhysicSystem : AbstractSystem
    {
        /// <summary>
        /// A fixed number used as to find out how many physic updates are needed
        /// </summary>
        private readonly float fixedDeltaTime;

        /// <summary>
        /// The milliseconds which are spend for an update
        /// </summary>
        private readonly float fixedDeltaTimeSeconds;

        /// <summary>
        /// The time left over from the last physic calculation
        /// </summary>
        private float leftOverDeltaTime;

        /// <summary>
        /// The time from the last call
        /// </summary>
        private float previousTime;

        /// <summary>
        /// The screen rectangle, entites leaving the area will be removed
        /// </summary>
        private Rectangle screenBound;

        /// <summary>
        /// The extended screenBound
        /// </summary>
        private Rectangle extendedScreenBounds;

        /// <summary>
        /// The gravity applied by the physic system
        /// </summary>
        private Vector2 gravityForce;

        /// <summary>
        /// The wind applied by the physic system
        /// </summary>
        private Vector2 windForce;

        /// <summary>
        /// The physic system got restored
        /// </summary>
        private bool gotRestored;

        /// <summary>
        /// Create a new instance of the physic system with a given screen bound
        /// </summary>
        /// <param name="screenBound"></param>
        public PhysicSystem(Rectangle screenBound) : this(screenBound, 9.8f)
        {

        }

        /// <summary>
        /// Create a new instance of the physic system with a given screen bound
        /// </summary>
        /// <param name="screenBound"></param>
        public PhysicSystem(Rectangle screenBound, float gravity) : this(screenBound, gravity, 0)
        {
        }

        /// <summary>
        /// Create a new instance of the physic system with a given screen bound
        /// </summary>
        /// <param name="screenBound"></param>
        public PhysicSystem(Rectangle screenBound, float gravity, float wind) : base()
        {
            fixedDeltaTime = 16;
            fixedDeltaTimeSeconds = fixedDeltaTime / 1000f;
            gravityForce = new Vector2(0, gravity);
            windForce = new Vector2(wind, 0);
            this.screenBound = screenBound;
            extendedScreenBounds = screenBound;
            extendedScreenBounds.X -= screenBound.Width / 4;
            extendedScreenBounds.Y -= screenBound.Height / 4;
            extendedScreenBounds.Width += screenBound.Width / 2;
            extendedScreenBounds.Height += screenBound.Height / 2;

            validators.Add(new PhysicEntityValidator());
            validators.Add(new MapValidator());
            gotRestored = true;
        }

        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent(this, typeof(ApplyForceEvent));
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float deltaTime = gameTime.TotalGameTime.Milliseconds - previousTime;
            previousTime = gameTime.TotalGameTime.Milliseconds;
            MapComponent map = GetMapComponent();
            if (map == null)
            {
                return;
            }

            int timeSteps = (int)((deltaTime + leftOverDeltaTime) / fixedDeltaTime);

            leftOverDeltaTime = deltaTime - (timeSteps * fixedDeltaTime);
            if (gotRestored)
            {
                gotRestored = false;
                leftOverDeltaTime = 0;
                timeSteps = 1;
            }

            updateLocked = true;

            foreach (uint entityId in watchedEntities)
            {
                if (entityManager.HasComponent(entityId, typeof(MapComponent)) || entitiesToRemove.Contains(entityId))
                {
                    continue;
                }
                CalculatePhysic(map, timeSteps, entityId);
            }
            updateLocked = false;
        }

        /// <inheritdoc/>
        public override void Restore()
        {
            gotRestored = true;
        }

        /// <summary>
        /// Calculate the physic for the entity
        /// </summary>
        /// <param name="map"></param>
        /// <param name="timeSteps"></param>
        /// <param name="entityId"></param>
        private void CalculatePhysic(MapComponent map, int timeSteps, uint entityId)
        {
            Vector2 oldPosition = Vector2.Zero;
            Vector2 bottomCenter = Vector2.Zero;
            for (int iteration = 0; iteration < timeSteps; iteration++)
            {
                PlaceableComponent placeComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                MoveableComponent moveComponent = entityManager.GetComponent<MoveableComponent>(entityId);
                ColliderComponent colliderComponent = entityManager.GetComponent<ColliderComponent>(entityId);

                if (placeComponent == null || moveComponent == null)
                {
                    break;
                }

                if (!moveComponent.ApplyPhysic)
                {
                    placeComponent.Position += moveComponent.Velocity;
                    if (!extendedScreenBounds.Contains(placeComponent.Position))
                    {
                        RemoveEntityEvent removeEntityEvent = CreateEvent<RemoveEntityEvent>();
                        removeEntityEvent.EntityId = entityId;
                        FireEvent(removeEntityEvent);
                        return;
                    }
                    continue;
                }

                if (colliderComponent == null)
                {
                    return;
                }

                ApplyForce(moveComponent, gravityForce, false);
                ApplyForce(moveComponent, windForce);

                oldPosition = placeComponent.Position;
                bottomCenter = new Vector2(colliderComponent.Collider.Center.X, colliderComponent.Collider.Bottom - 2);
                bottomCenter += oldPosition;
                Raycast raycast = new Raycast(bottomCenter, moveComponent.Velocity, moveComponent.Velocity.Length());
                Point[] rayPath = raycast.GetPoints();
                for (int y = 0; y < rayPath.Length; y++)
                {
                    Point currentPosition = rayPath[y];
                    if (IsPixelSolid(map, currentPosition))
                    {
                        Vector2 xForce = Vector2.UnitX;
                        xForce *= moveComponent.Velocity.X;
                        Raycast horizontalRaycast = new Raycast(bottomCenter, xForce, xForce.Length());
                        Point[] horizontalRay = horizontalRaycast.GetPoints();
                        for (int x = 0; x < horizontalRay.Length; x++)
                        {
                            Point currentXPosition = rayPath[x];
                            if (IsPixelSolid(map, currentXPosition))
                            {
                                xForce *= -1;
                                ApplyForce(moveComponent, windForce * -1);
                                ApplyForce(moveComponent, xForce, false);
                                break;
                            }
                        }

                        if (colliderComponent.FireCollideEvent)
                        {
                            if (colliderComponent.FireBelow)
                            {
                                Raycast bottomCast = new Raycast(bottomCenter, Vector2.UnitY, 10);
                                Point[] cast = bottomCast.GetPoints();
                                for (int i = 0; i < cast.Length; i++)
                                {
                                    if (!map.ImageData.IsInArray(cast[i]))
                                    {
                                        continue;
                                    }
                                    if (map.NotSolidColors.Contains(map.ImageData.GetValue(cast[i])))
                                    {
                                        MapCollisionEvent mapCollisionEvent = CreateEvent<MapCollisionEvent>();
                                        mapCollisionEvent.EntityId = entityId;
                                        mapCollisionEvent.Position = cast[i].ToVector2();
                                        FireEvent(mapCollisionEvent);
                                        break;
                                    }
                                }
                                return;
                            }

                            Vector2 frontPosition = Vector2.UnitX;
                            frontPosition *= colliderComponent.Collider.Right;
                            if (moveComponent.Velocity.X < 0)
                            {
                                frontPosition *= -1;
                            }
                            frontPosition += bottomCenter;
                            MapCollisionEvent mapPositionCollisionEvent = CreateEvent<MapCollisionEvent>();
                            mapPositionCollisionEvent.EntityId = entityId;
                            mapPositionCollisionEvent.Position = frontPosition;
                            FireEvent(mapPositionCollisionEvent);
                            return;
                        }

                        Vector2 normalForce = GetNormalForce(moveComponent, bottomCenter, currentPosition.ToVector2());
                        ApplyForce(moveComponent, normalForce, false);
                        ApplyForce(moveComponent, gravityForce * -1, false);
                        break;
                    }
                }
                //moveComponent.Acceleration = moveComponent.Acceleration;
                moveComponent.Velocity += moveComponent.Acceleration;
                placeComponent.Position += moveComponent.Velocity;

                if (moveComponent.PhysicRotate)
                {
                    Vector2 rotationTarget = placeComponent.Position - oldPosition;
                    placeComponent.Rotation = (float)Math.Atan2(
                        rotationTarget.Y,
                        rotationTarget.X
                    );
                }

                if (!map.ImageData.IsInArray(placeComponent.Position))
                {
                    RemoveEntityEvent removeEntityEvent = CreateEvent<RemoveEntityEvent>();
                    removeEntityEvent.EntityId = entityId;
                    FireEvent(removeEntityEvent);
                }
                moveComponent.Acceleration *= 0;
            }
        }

        private bool IsPixelSolid(MapComponent map, Point currentPosition)
        {
            return map.ImageData.IsInArray(currentPosition) && !map.NotSolidColors.Contains(map.ImageData.GetValue(currentPosition));
        }

        /// <inheritdoc/>
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is ApplyForceEvent forceEvent)
            {
                if (watchedEntities.Contains(forceEvent.EntityId))
                {
                    MoveableComponent moveComponent = entityManager.GetComponent<MoveableComponent>(forceEvent.EntityId);
                    ApplyForce(moveComponent, forceEvent.Force);
                }
            }
        }

        /// <summary>
        /// Get the normal force
        /// </summary>
        /// <param name="moveComponent">The current movement</param>
        /// <param name="lowestPixel">The lowest pixel of the entity</param>
        /// <param name="currentPosition">The current position of the entity</param>
        /// <returns></returns>
        private Vector2 GetNormalForce(MoveableComponent moveComponent, Vector2 lowestPixel, Vector2 currentPosition)
        {
            Vector2 normalForce = Vector2.UnitY;
            normalForce *= moveComponent.Velocity.Y;

            normalForce += lowestPixel - currentPosition;
            normalForce *= -1;

            return normalForce;
        }

        /// <summary>
        /// Apply a force to a moveable component
        /// </summary>
        /// <param name="moveableComponent">The component to apply the force to</param>
        /// <param name="force">The force to apply</param>
        private void ApplyForce(MoveableComponent moveableComponent, Vector2 force)
        {
            ApplyForce(moveableComponent, force, true);
        }

        /// <summary>
        /// Apply a force to a moveable component
        /// </summary>
        /// <param name="moveableComponent">The component to apply the force to</param>
        /// <param name="force">The force to apply</param>
        private void ApplyForce(MoveableComponent moveableComponent, Vector2 force, bool applyMass)
        {
            if (moveableComponent == null)
            {
                return;
            }
            Vector2 forceCopy = applyMass ? force : force * moveableComponent.Mass;
            moveableComponent.Acceleration += forceCopy / moveableComponent.Mass;
        }

        /// <summary>
        /// This method will return you the map component from the watched entity
        /// </summary>
        /// <returns>The instance of the map component if present or null</returns>
        private MapComponent GetMapComponent()
        {
            foreach (uint entityId in watchedEntities)
            {
                if (entityManager.HasComponent(entityId, typeof(MapComponent)))
                {
                    return entityManager.GetComponent<MapComponent>(entityId);
                }
            }
            return null;
        }
    }
}
