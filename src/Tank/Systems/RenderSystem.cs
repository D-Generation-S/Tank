using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
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

        /// <summary>
        /// The graphic device to use
        /// </summary>
        private readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// All the already used containers to prevent gc
        /// </summary>
        private readonly Stack<RenderContainer> usedContainers;

        /// <summary>
        /// The containers which should be drawn in this call
        /// </summary>
        private readonly List<RenderContainer> containersToRender;

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice) : base()
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
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
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, null);
            graphicsDevice.Clear(Color.CornflowerBlue);
            foreach (uint entityId in watchedEntities)
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    continue;
                }
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                VisibleTextComponent textComponent = entityManager.GetComponent<VisibleTextComponent>(entityId);
                RenderTextures(placeableComponent, visibleComponent);
                RenderText(placeableComponent, textComponent);
            }

            spriteBatch.End();
            drawLocked = false;
        }

        /// <summary>
        /// Render all the textures
        /// </summary>
        /// <param name="placeableComponent">The placeable component</param>
        /// <param name="visibleComponent">The visible component</param>
        private void RenderTextures(PlaceableComponent placeableComponent, VisibleComponent visibleComponent)
        {
            if (visibleComponent == null || placeableComponent == null || visibleComponent.Texture == null)
            {
                return;
            }
            RenderContainer renderContainer = usedContainers.Count > 0 ? usedContainers.Pop() : new RenderContainer();
            renderContainer.TextureToDraw = visibleComponent.Texture;
            renderContainer.Destination = visibleComponent.Destination;
            renderContainer.Source = visibleComponent.Source;
            renderContainer.Color = visibleComponent.Color;
            renderContainer.Rotation = placeableComponent.Rotation;
            renderContainer.Effect = visibleComponent.Effect;
            renderContainer.LayerDepth = visibleComponent.LayerDepth;
            containersToRender.Add(renderContainer);
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
                  renderContainer.Effect,
                  renderContainer.LayerDepth
              );
                usedContainers.Push(currentContainer);
            }
            containersToRender.Clear();
        }

        /// <summary>
        /// Render text data
        /// </summary>
        /// <param name="placeableComponent">The placeable component</param>
        /// <param name="textComponent">The text component</param>
        private void RenderText(PlaceableComponent placeableComponent, VisibleTextComponent textComponent)
        {
            if (textComponent == null || placeableComponent == null || textComponent.Font == null)
            {
                return;
            }
            spriteBatch.DrawString(
                textComponent.Font,
                textComponent.Text,
                placeableComponent.Position,
                textComponent.Color,
                placeableComponent.Rotation,
                Vector2.Zero,
                textComponent.Scale,
                textComponent.Effect,
                textComponent.LayerDepth
            );
        }
    }
}
