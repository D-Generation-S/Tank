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

        /// <summary>
        /// Current effect to apply
        /// </summary>
        private Effect currentEffect;

        /// <summary>
        /// The default sprite effect
        /// </summary>
        private readonly Effect defaultEffect;

        /// <summary>
        /// Rendertarget to use
        /// </summary>
        private RenderTarget2D gameRenderTarget;

        /// <summary>
        /// Rendertarget to use
        /// </summary>
        private RenderTarget2D postProcessingRenderTarget;

        /// <summary>
        /// The color to copy over for new draw cycle
        /// </summary>
        private readonly Color[] targetContent;

        /// <summary>
        /// All the post processing effects
        /// </summary>
        private readonly List<Effect> postProcessing;

        /// <summary>
        /// Is this the start of the draw call
        /// </summary>
        private bool drawStart;

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Effect defaultEffect)
            : this(spriteBatch, graphicsDevice, defaultEffect, new List<Effect>() { defaultEffect })
        {
        }

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(
            SpriteBatch spriteBatch,
            GraphicsDevice graphicsDevice,
            Effect defaultEffect,
            List<Effect> postProcessing
            ) : base()
        {
            this.spriteBatch = spriteBatch;
            this.graphicsDevice = graphicsDevice;
            validators.Add(new RenderableEntityValidator());

            usedContainers = new Stack<RenderContainer>();
            containersToRender = new List<RenderContainer>();
            this.defaultEffect = defaultEffect;
            this.postProcessing = postProcessing;
            gameRenderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            postProcessingRenderTarget = new RenderTarget2D(graphicsDevice, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height);
            targetContent = new Color[postProcessingRenderTarget.Width * postProcessingRenderTarget.Height];
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
                if (placeableComponent == null)
                {
                    continue;
                }
                CollectTextures(placeableComponent, visibleComponent);
                CollectText(placeableComponent, textComponent);
            }

            containersToRender = containersToRender.OrderBy(container => container.ShaderEffect).ThenBy(container => container.Name).ToList();
            graphicsDevice.SetRenderTarget(gameRenderTarget);
            for (int i = 0; i < containersToRender.Count; i++)
            {
                RenderEntities(i);
            }
            containersToRender.Clear();
            if (drawStart)
            {
                graphicsDevice.SetRenderTarget(null);
                return;
            }
            spriteBatch.End();

            graphicsDevice.SetRenderTarget(postProcessingRenderTarget);
            for (int i = 0; i < postProcessing.Count; i++)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    postProcessing[i],
                    null
                    );

                spriteBatch.Draw(gameRenderTarget, Vector2.Zero, Color.White);
                spriteBatch.End();
                postProcessingRenderTarget.GetData<Color>(targetContent);
                gameRenderTarget.SetData<Color>(targetContent);
            }
            graphicsDevice.SetRenderTarget(null);
            graphicsDevice.Clear(Color.Violet);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                defaultEffect,
                null
            );

            spriteBatch.Draw(
                postProcessing.Count == 0 ? gameRenderTarget : postProcessingRenderTarget,
                Vector2.Zero,
                Color.White
                );

            spriteBatch.End();
            drawLocked = false;
        }

        private void RenderEntities(int i)
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
                    SpriteSortMode.Immediate,
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
                    SpriteSortMode.Immediate,
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
            if (visibleComponent == null || visibleComponent.Texture == null)
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
            if (textComponent == null || textComponent.Font == null)
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
