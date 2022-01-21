using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using TankEngine.Adapter;
using TankEngine.DataStructures.Pools;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;
using TankEngine.EntityComponentSystem.Validator;
using TankEngine.EntityComponentSystem.Validator.Base;
using TankEngine.EntityComponentSystem.Validator.LogicValidators;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    /// <summary>
    /// Simple render system which will render from 0,0 of the world to the end of the viewport
    /// </summary>
    public class SimpleRenderSystem : AbstractSystem
    {
        /// <summary>
        /// The spritebatch used for drawing
        /// </summary>
        protected readonly SpriteBatch spriteBatch;

        /// <summary>
        /// The default effekt to use if non is set
        /// </summary>
        protected readonly Effect defaultEffect;

        /// <summary>
        /// The viewport adapter to use
        /// </summary>
        protected readonly IViewportAdapter viewportAdapter;

        /// <summary>
        /// The Graphics device to use
        /// </summary>
        protected readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// The pool for render texture containers
        /// </summary>
        protected IObjectPool<RenderObjectContainer> renderObjectContainerPool;

        /// <summary>
        /// The render target to draw the scene on
        /// </summary>
        protected RenderTarget2D sceneRenderTarget;

        /// <summary>
        /// Create a new instance of this render system
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="defaultEffekt">The default effekt to use</param>
        /// <param name="viewportAdapter">The viewport adapter to use</param>
        /// <param name="graphicsDevice">The graphics device used to generate render targets</param>
        public SimpleRenderSystem(SpriteBatch spriteBatch, Effect defaultEffekt, IViewportAdapter viewportAdapter, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.defaultEffect = defaultEffekt;
            this.viewportAdapter = viewportAdapter;
            this.graphicsDevice = graphicsDevice;
            renderObjectContainerPool = new ConcurrentObjectPool<RenderObjectContainer>(() => new RenderObjectContainer(), 20);
            CreateSceneRenderTarget();

            IValidatable orValidator = new OrValidator(new TextureRenderingValidator(), new TextRenderingValidator());
            validators.Add(orValidator);
        }

        /// <summary>
        /// Create the scene render target
        /// </summary>
        private void CreateSceneRenderTarget()
        {
            if (viewportAdapter == null || graphicsDevice == null)
            {
                return;
            }
            sceneRenderTarget = new RenderTarget2D(graphicsDevice, viewportAdapter.Viewport.Width, viewportAdapter.Viewport.Height);
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            drawLocked = true;
            BeginDraw(defaultEffect);
            DrawGameObjects();
            EndDraw();
            DrawCompletedScene(sceneRenderTarget);
            drawLocked = false;
        }

        /// <summary>
        /// Draw the completed scene
        /// </summary>
        /// <param name="scene"></param>
        protected virtual void DrawCompletedScene(RenderTarget2D scene)
        {
            graphicsDevice.SetRenderTarget(null);
            viewportAdapter.Reset();
            graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                null,
                null,
                null,
                defaultEffect,
                viewportAdapter.GetScaleMatrix()
            );

            spriteBatch.Draw(
                scene,
                new Rectangle(0, 0, viewportAdapter.VirtualWidth, viewportAdapter.VirtualHeight),
                Color.White
                );

            spriteBatch.End();
        }

        /// <summary>
        /// Draw all the game objects
        /// </summary>
        protected virtual void DrawGameObjects()
        {
            Effect currentEffect = defaultEffect;
            graphicsDevice.SetRenderTarget(sceneRenderTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);
            foreach (RenderObjectContainer container in GetRenderContainers())
            {
                currentEffect = BeginDrawGameObject(container.ShaderEffect, currentEffect);
                if (container.ContainerType == RenderContainerTypeEnum.Texture)
                {
                    DrawTextureContainer(container);
                    continue;
                }
                DrawTextContainer(container);

                renderObjectContainerPool.Return(container);
            }
        }

        /// <summary>
        /// Begin new draw call if required for rendering of game objects
        /// </summary>
        /// <param name="containerEffect">The effect of the current container</param>
        /// <param name="currentEffect">The last used effect by the last render call</param>
        /// <returns></returns>
        protected virtual Effect BeginDrawGameObject(Effect containerEffect, Effect currentEffect)
        {
            containerEffect = containerEffect ?? defaultEffect;
            if (containerEffect != currentEffect)
            {
                EndDraw();
                BeginDraw(containerEffect);
            }
            return containerEffect;
        }

        /// <summary>
        /// Draw a texture container
        /// </summary>
        /// <param name="container">The texture container to draw</param>
        private void DrawTextureContainer(RenderObjectContainer container)
        {
            Vector2 position = GetBaseDrawPosition(container.PositionComponent) + container.TextureComponent.DrawOffset;
            int width = (int)(container.TextureComponent.Source.Width * container.TextureComponent.Scale);
            int height = (int)(container.TextureComponent.Source.Height * container.TextureComponent.Scale);
            Rectangle targetDrawArea = new Rectangle((int)position.X, (int)position.Y, width, height);
            if (!IsInRenderArea(targetDrawArea))
            {
                return;
            }
            spriteBatch.Draw(
                container.TextureComponent.Texture,
                position,
                container.TextureComponent.Source,
                container.TextureComponent.Color,
                container.PositionComponent.Rotation,
                container.TextureComponent.RotationCenter,
                container.TextureComponent.Scale,
                container.TextureComponent.SpriteEffect,
                0
                );
        }

        /// <summary>
        /// Draw a single text container
        /// </summary>
        /// <param name="container">The text container to draw</param>
        private void DrawTextContainer(RenderObjectContainer container)
        {
            Vector2 position = GetBaseDrawPosition(container.PositionComponent) + container.TextComponent.DrawOffset;
            Vector2 fontSize = container.TextComponent.Font.MeasureString(container.TextComponent.Text) * container.TextComponent.Scale;
            Rectangle targetDrawArea = new Rectangle((int)position.X, (int)position.Y, (int)fontSize.X, (int)fontSize.Y);
            if (!IsInRenderArea(targetDrawArea))
            {
                return;
            }
            spriteBatch.DrawString(
                container.TextComponent.Font,
                container.TextComponent.Text,
                position,
                container.TextComponent.Color,
                container.PositionComponent.Rotation,
                container.TextComponent.RotationCenter,
                container.TextComponent.Scale,
                container.TextComponent.SpriteEffect,
                0
                );
        }

        /// <summary>
        /// Is the current thing to draw in the render area
        /// </summary>
        /// <param name="targetDrawArea">The are the texture or text is getting drawn to</param>
        /// <returns>True if inside of the render area</returns>
        protected virtual bool IsInRenderArea(Rectangle targetDrawArea)
        {
            return viewportAdapter.Viewport.Bounds.Intersects(targetDrawArea);
        }

        /// <summary>
        /// Get the base draw position for the given texture
        /// </summary>
        /// <param name="positionComponent">The position component of the entity</param>
        /// <returns>The vector where to draw the dataset</returns>
        protected virtual Vector2 GetBaseDrawPosition(PositionComponent positionComponent)
        {
            return positionComponent.Position;
        }

        /// <summary>
        /// Get the texture container to render
        /// </summary>
        /// <returns>A IEnumerable with all the texture containers</returns>
        protected virtual IEnumerable<RenderObjectContainer> GetRenderContainers()
        {
            return watchedEntities.Where(entityId => !entitiesToRemove.Contains(entityId))
                                  .Where(entityId => entityManager.HasComponent<PositionComponent>(entityId) && (entityManager.HasComponent<TextureComponent>(entityId) || entityManager.HasComponent<TextComponent>(entityId)))
                                  .Select(entityId => CreateRenderContainer(entityId))
                                  .Where(container => container != null && container.ContainerType != RenderContainerTypeEnum.Unknown)
                                  .OrderBy(container => container.DrawLayer)
                                  .ThenBy(container => container.Name)
                                  .ThenBy(container => container.EffectName);
        }

        /// <summary>
        /// Create render container from given entity id
        /// </summary>
        /// <param name="entityId">The entity id to create the container from</param>
        /// <returns>A render container ready to use, can be null</returns>
        protected virtual RenderObjectContainer CreateRenderContainer(uint entityId)
        {
            PositionComponent positionComponent = entityManager.GetComponent<PositionComponent>(entityId);
            TextureComponent textureComponent = entityManager.GetComponent<TextureComponent>(entityId);
            if (textureComponent != null)
            {
                return CreateTextureContainer(positionComponent, textureComponent);
            }
            return CreateTextContainer(positionComponent, entityManager.GetComponent<TextComponent>(entityId));
        }

        /// <summary>
        /// Create a new text container based on the given component
        /// </summary>
        /// <param name="positionComponent">The position component</param>
        /// <param name="textComponent">The text component</param>
        /// <returns>A useable texture render container</returns>
        private RenderObjectContainer CreateTextContainer(PositionComponent positionComponent, TextComponent textComponent)
        {
            if (positionComponent == null || textComponent == null)
            {
                return null;
            }
            RenderObjectContainer returnContainer = renderObjectContainerPool.Get();
            returnContainer.PositionComponent = positionComponent;
            returnContainer.TextComponent = textComponent;
            returnContainer.ContainerType = RenderContainerTypeEnum.Text;
            return returnContainer;
        }

        /// <summary>
        /// Create a new texture container based on the entityId
        /// </summary>
        /// <param name="entityId">The entity id to use</param>
        /// <returns>The texture render container</returns>
        protected virtual RenderObjectContainer CreateTextureContainer(uint entityId)
        {
            PositionComponent positionComponent = entityManager.GetComponent<PositionComponent>(entityId);
            TextureComponent textureComponent = entityManager.GetComponent<TextureComponent>(entityId);
            return CreateTextureContainer(positionComponent, textureComponent);
        }

        /// <summary>
        /// Create a new texture container based on the given component
        /// </summary>
        /// <param name="positionComponent">The position component</param>
        /// <param name="textureComponent">The texture component</param>
        /// <returns>A useable texture render container</returns>
        protected virtual RenderObjectContainer CreateTextureContainer(PositionComponent positionComponent, TextureComponent textureComponent)
        {
            if (positionComponent == null || textureComponent == null)
            {
                return null;
            }
            RenderObjectContainer returnContainer = renderObjectContainerPool.Get();
            returnContainer.PositionComponent = positionComponent;
            returnContainer.TextureComponent = textureComponent;
            returnContainer.ContainerType = RenderContainerTypeEnum.Texture;
            return returnContainer;
        }

        /// <summary>
        /// Begin the main draw loop
        /// </summary>
        protected virtual void BeginDraw(Effect effect)
        {
            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.NonPremultiplied,
                null,
                null,
                null,
                effect,
                viewportAdapter.GetScaleMatrix());
        }

        /// <summary>
        /// End main draw loop
        /// </summary>
        protected virtual void EndDraw()
        {
            spriteBatch.End();
        }
    }
}
