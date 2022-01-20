using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using Tank.Components.DataLookup;
using Tank.Components.GameObject;
using Tank.Components.Input;
using Tank.Components.Tags;
using Tank.Events.StateEvents;
using Tank.Interfaces.Builders;
using Tank.Register;
using Tank.Systems.Data;
using Tank.Validator.Input;
using TankEngine.EntityComponentSystem;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Systems;

namespace Tank.Systems
{
    class KeyboardInputSystem : AbstractSystem
    {
        private readonly Register<IGameObjectBuilder> projectileRegister;
        KeyboardState previousState;
        bool newState;

        public KeyboardInputSystem(Register<IGameObjectBuilder> projectileRegister) : base()
        {
            newState = false;
            validators.Add(new KeyboardControllableObjectValidator());
            this.projectileRegister = projectileRegister;
        }

        public override void Restore()
        {
            base.Restore();
            newState = true;
            previousState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (uint entityId in watchedEntities)
            {
                if (!entityManager.HasComponent<ActiveGameObjectTag>(entityId))
                {
                    continue;
                }



                KeyboardControllerComponent keyboardController = entityManager.GetComponent<KeyboardControllerComponent>(entityId);
                ControllableGameObject controllableGameObject = entityManager.GetComponent<ControllableGameObject>(entityId);
                GameObjectData gameObjectData = entityManager.GetComponent<GameObjectData>(entityId);
                if (keyboardController == null || controllableGameObject == null)
                {
                    return;
                }
                KeyboardState keyboardState = Keyboard.GetState();
                if (keyboardState.IsKeyUp(keyboardController.Menu))
                {
                    newState = false;
                }
                Debug.WriteLine("Barrel pos " + controllableGameObject.BarrelRotationDegree);
                Debug.WriteLine("Strenght pos " + controllableGameObject.Strength);
                if (!newState && keyboardState.IsKeyDown(keyboardController.Menu))
                {
                    FireEvent(eventManager.CreateEvent<OpenMenuEvent>());
                }

                if (previousState.IsKeyUp(keyboardController.Screenshot) && keyboardState.IsKeyDown(keyboardController.Screenshot))
                {
                    FireEvent(eventManager.CreateEvent<TakeScreenshotEvent>());
                }

                if (keyboardState.IsKeyDown(keyboardController.BarrelLeft))
                {
                    Debug.WriteLine(entityId + " Barrel left");
                    controllableGameObject.BarrelRotationDegree -= ControlStaticValues.BARREL_ROTATION_DEGREE;
                    if (controllableGameObject.BarrelRotationDegree < ControlStaticValues.MAX_BARREL_LEFT)
                    {
                        controllableGameObject.BarrelRotationDegree = ControlStaticValues.MAX_BARREL_LEFT;
                    }
                }
                if (keyboardState.IsKeyDown(keyboardController.BarrelRight))
                {
                    Debug.WriteLine(entityId + " Barrel right");
                    controllableGameObject.BarrelRotationDegree += ControlStaticValues.BARREL_ROTATION_DEGREE;
                    if (controllableGameObject.BarrelRotationDegree > ControlStaticValues.MAX_BARREL_RIGHT)
                    {
                        controllableGameObject.BarrelRotationDegree = ControlStaticValues.MAX_BARREL_RIGHT;
                    }
                }

                if (keyboardState.IsKeyDown(keyboardController.IncreaseStrengh))
                {
                    controllableGameObject.Strength += ControlStaticValues.STRENGTH_CHANGE;
                    if (controllableGameObject.Strength > ControlStaticValues.MAX_STRENGHT)
                    {
                        controllableGameObject.Strength = ControlStaticValues.MAX_STRENGHT;
                    }
                    UpdateGameObjectData(gameObjectData, "Strength", controllableGameObject.Strength);
                }

                if (keyboardState.IsKeyDown(keyboardController.DecreaseStrengh))
                {
                    controllableGameObject.Strength -= ControlStaticValues.STRENGTH_CHANGE;
                    if (controllableGameObject.Strength < ControlStaticValues.MIN_STRENGHT)
                    {
                        controllableGameObject.Strength = ControlStaticValues.MIN_STRENGHT;
                    }
                    UpdateGameObjectData(gameObjectData, "Strength", controllableGameObject.Strength);
                }

                if (previousState.IsKeyUp(keyboardController.NextProjectile) && keyboardState.IsKeyDown(keyboardController.NextProjectile))
                {
                    Debug.WriteLine(entityId + " Next projectile");
                    int nextRegister = controllableGameObject.SelectedProjectile;
                    nextRegister++;
                    if (projectileRegister.Contains(nextRegister))
                    {
                        controllableGameObject.SelectedProjectile = nextRegister;
                    }
                    else
                    {
                        controllableGameObject.SelectedProjectile = 0;
                    }

                }

                if (previousState.IsKeyUp(keyboardController.PreviousProjectile) && keyboardState.IsKeyDown(keyboardController.PreviousProjectile))
                {
                    Debug.WriteLine(entityId + " previous projectile");
                    int nextRegister = controllableGameObject.SelectedProjectile;
                    nextRegister--;
                    if (projectileRegister.Contains(nextRegister))
                    {
                        controllableGameObject.SelectedProjectile = nextRegister;
                    }
                    else
                    {
                        controllableGameObject.SelectedProjectile = projectileRegister.Count - 1;
                    }

                }

                if (previousState.IsKeyUp(keyboardController.Fire) && keyboardState.IsKeyDown(keyboardController.Fire) && entityManager.HasComponent<CanPerformActionTag>(entityId))
                {
                    entityManager.RemoveComponents(entityId, typeof(CanPerformActionTag));

                    PositionComponent placeable = entityManager.GetComponent<PositionComponent>(entityId);
                    ProjectileDataComponent additionalData = entityManager.GetComponents<ProjectileDataComponent>().Find(data => data.Position == controllableGameObject.SelectedProjectile);
                    TextureComponent visibility = entityManager.GetComponent<TextureComponent>(entityId);


                    List<IComponent> components = new List<IComponent>();
                    PositionComponent placeableComponent = entityManager.CreateComponent<PositionComponent>();
                    placeableComponent.Rotation = controllableGameObject.BarrelRotationRadians;
                    placeableComponent.Position = placeable.Position;

                    ProjectileSpawnComponent projectileSpawnComponent = entityManager.CreateComponent<ProjectileSpawnComponent>();
                    float offset = visibility.Texture.Width + visibility.Texture.Height;
                    projectileSpawnComponent.Offset = offset / 2;
                    projectileSpawnComponent.ProjectileToSpawn = controllableGameObject.SelectedProjectile;
                    projectileSpawnComponent.Strenght = controllableGameObject.Strength;
                    projectileSpawnComponent.Amount = additionalData.Amount;
                    projectileSpawnComponent.TicksUntilSpawn = additionalData.TicksUntilSpawn;
                    projectileSpawnComponent.UseParentEntity = true;
                    projectileSpawnComponent.ParentEntity = entityId;

                    components.Add(placeableComponent);
                    components.Add(projectileSpawnComponent);
                    components.Add(entityManager.CreateComponent<RoundBlockingTag>());

                    AddEntityEvent addEntityEvent = eventManager.CreateEvent<AddEntityEvent>();
                    addEntityEvent.Components = components;
                    FireEvent(addEntityEvent);

                    Debug.WriteLine(entityId + " Fire");
                }

                previousState = keyboardState;
            }
        }

        /// <summary>
        /// Udpate the game object data
        /// </summary>
        /// <param name="gameObjectData">The game object to update</param>
        /// <param name="fieldName">The field name to update</param>
        /// <param name="newValue">The new value to use</param>
        private void UpdateGameObjectData(GameObjectData gameObjectData, string fieldName, float newValue)
        {
            if (gameObjectData.Properties.ContainsKey(fieldName))
            {
                gameObjectData.Properties[fieldName] = newValue;
                gameObjectData.DataChanged = true;
            }
        }
    }
}
