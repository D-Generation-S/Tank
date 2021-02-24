using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.DataStructure;
using Tank.Enums;
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
        private List<RenderContainer> containersToRender;

        private Effect currentEffect;

        private readonly Effect defaultEffect;

        private bool drawStart;

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Effect defaultEffect) : base()
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            validators.Add(new RenderableEntityValidator());

            usedContainers = new Stack<RenderContainer>();
            containersToRender = new List<RenderContainer>();
            this.defaultEffect = defaultEffect;
            drawStart = true;
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
            drawStart = true;
            foreach (uint entityId in watchedEntities)
            {
                if (entitiesToRemove.Contains(entityId))
                {
                    continue;
                }
                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                VisibleTextComponent textComponent = entityManager.GetComponent<VisibleTextComponent>(entityId);
                CollectTextures(placeableComponent, visibleComponent);
                CollectText(placeableComponent, textComponent);
            }


            containersToRender = containersToRender.OrderBy(container => container.ShaderEffect).ThenBy(container => container.Name).ToList();
            for (int i = 0; i < containersToRender.Count; i++)
            {
                RenderContainer currentContainer = containersToRender[i];
                BeginDraw(currentContainer.ShaderEffect);
                switch (currentContainer.RenderType)
                {
                    case RenderTypeEnum.Texture:
                        spriteBatch.Draw(
                          currentContainer.TextureToDraw,
                          currentContainer.Destination,
                          currentContainer.Source,
                          currentContainer.Color,
                          currentContainer.Rotation,
                          Vector2.Zero,
                          currentContainer.Effect,
                          currentContainer.LayerDepth
                        );
                        break;
                    case RenderTypeEnum.Text:
                        spriteBatch.DrawString(
                            currentContainer.Font,
                            currentContainer.Text,
                            currentContainer.Position,
                            currentContainer.Color,
                            currentContainer.Rotation,
                            Vector2.Zero,
                            currentContainer.Scale,
                            currentContainer.Effect,
                            currentContainer.LayerDepth
                        );
                        break;
                    default:
                        break;
                }
                usedContainers.Push(currentContainer);
            }
            containersToRender.Clear();

            spriteBatch.End();
            drawLocked = false;
        }

        /// <summary>
        /// Create a new draw cycle if a shader is applied
        /// </summary>
        /// <param name="effect"></param>
        private void BeginDraw(Effect effect)
        {
            effect = effect == null ? defaultEffect : effect;
            if (drawStart)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    effect,
                    null);
                graphicsDevice.Clear(Color.CornflowerBlue);
                drawStart = false;
                currentEffect = effect;
                return;
            }
            if (currentEffect != effect)
            {
                spriteBatch.End();
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    effect,
                    null
                    );
                currentEffect = effect;
            }
        }

        /// <summary>
        /// Render all the textures
        /// </summary>
        /// <param name="placeableComponent">The placeable component</param>
        /// <param name="visibleComponent">The visible component</param>
        private void CollectTextures(PlaceableComponent placeableComponent, VisibleComponent visibleComponent)
        {
            if (visibleComponent == null || placeableComponent == null || visibleComponent.Texture == null)
            {
                return;
            }
            RenderContainer renderContainer = GetRenderContainer();
            renderContainer.RenderType = RenderTypeEnum.Texture;
            renderContainer.TextureToDraw = visibleComponent.Texture;
            renderContainer.Destination = visibleComponent.Destination;
            renderContainer.Source = visibleComponent.Source;
            renderContainer.Color = visibleComponent.Color;
            renderContainer.Rotation = placeableComponent.Rotation;
            renderContainer.Effect = visibleComponent.Effect;
            renderContainer.LayerDepth = visibleComponent.LayerDepth;
            renderContainer.ShaderEffect = visibleComponent.ShaderEffect;
            renderContainer.Name = visibleComponent.Texture.Name;
            containersToRender.Add(renderContainer);
        }

        /// <summary>
        /// Render text data
        /// </summary>
        /// <param name="placeableComponent">The placeable component</param>
        /// <param name="textComponent">The text component</param>
        private void CollectText(PlaceableComponent placeableComponent, VisibleTextComponent textComponent)
        {
            if (textComponent == null || placeableComponent == null || textComponent.Font == null)
            {
                return;
            }
            RenderContainer renderContainer = GetRenderContainer();
            renderContainer.RenderType = RenderTypeEnum.Text;
            renderContainer.Text = textComponent.Text;
            renderContainer.Font = textComponent.Font;
            renderContainer.Position = placeableComponent.Position;
            renderContainer.Color = textComponent.Color;
            renderContainer.Rotation = placeableComponent.Rotation;
            renderContainer.Scale = textComponent.Scale;
            renderContainer.Effect = textComponent.Effect;
            renderContainer.LayerDepth = textComponent.LayerDepth;
            renderContainer.Name = string.Empty;
            renderContainer.ShaderEffect = textComponent.ShaderEffect;

            containersToRender.Add(renderContainer);
        }

        /// <summary>
        /// Get a render container
        /// </summary>
        /// <returns></returns>
        private RenderContainer GetRenderContainer()
        {
            return usedContainers.Count > 0 ? usedContainers.Pop() : new RenderContainer(); ;
        }
    }
}
