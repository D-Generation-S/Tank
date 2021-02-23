﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.DataStructure;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will render any entity which is visible
    /// </summary>
    class RenderSystem : AbstractSystem
    {
        /// <summary>
        /// The spritebatch instance to use
        /// </summary>
        private readonly SpriteBatch spriteBatch;

        private readonly Stack<RenderContainer> usedContainers;

        private readonly List<RenderContainer> containersToRender;

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(SpriteBatch spriteBatch) : base()
        {
            this.spriteBatch = spriteBatch;
            validators.Add(new RenderableEntityValidator());

            usedContainers = new Stack<RenderContainer>();
            containersToRender = new List<RenderContainer>();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            updateLocked = true;
            foreach (uint entityId in watchedEntities)
            {
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                if (placeableComponent == null || visibleComponent == null)
                {
                    continue;
                }
                Rectangle destination = visibleComponent.Destination;
                destination.X = (int)placeableComponent.Position.X;
                destination.Y = (int)placeableComponent.Position.Y;
                visibleComponent.Destination = destination;
            }
            updateLocked = false;
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            drawLocked = true;
            foreach (uint entityId in watchedEntities)
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    continue;
                }
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                if (visibleComponent == null || placeableComponent == null || visibleComponent.Texture == null)
                {
                    continue;
                }
                RenderContainer renderContainer = usedContainers.Count > 0 ? usedContainers.Pop() : new RenderContainer();
                renderContainer.TextureToDraw = visibleComponent.Texture;
                renderContainer.Destination = visibleComponent.Destination;
                renderContainer.Source = visibleComponent.Source;
                renderContainer.Color = visibleComponent.Color;
                renderContainer.Rotation = placeableComponent.Rotation;
                containersToRender.Add(renderContainer);
            }
            containersToRender.Sort((containerA, containerB) => containerA.TextureToDraw.Name.CompareTo(containerB.TextureToDraw.Name));

            for (int i = 0; i < containersToRender.Count; i++)
            {
                RenderContainer currentContainer = containersToRender[i];
                 spriteBatch.Draw(
                   currentContainer.TextureToDraw,
                   currentContainer.Destination,
                   currentContainer.Source,
                   currentContainer.Color,
                   currentContainer.Rotation,
                   Vector2.Zero,
                   SpriteEffects.None,
                   1f
               );
                usedContainers.Push(currentContainer);
            }
            containersToRender.Clear();
            drawLocked = false;
        }
    }
}
