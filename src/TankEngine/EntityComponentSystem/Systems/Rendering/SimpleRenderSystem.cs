﻿using Microsoft.Xna.Framework;
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
    public class SimpleRenderSystem : AbstractSystem
    {
        /// <summary>
        /// The max number of layers
        /// </summary>
        private const float MAX_LAYER_COUNT = 1000f;

        /// <summary>
        /// The spritebatch used for drawing
        /// </summary>
        protected readonly SpriteBatch spriteBatch;

        /// <summary>
        /// The default effekt to use if non is set
        /// </summary>
        protected readonly Effect defaultEffekt;

        /// <summary>
        /// The viewport adapter to use
        /// </summary>
        protected readonly IViewportAdapter viewportAdapter;

        /// <summary>
        /// The Graphics device to use
        /// </summary>
        protected readonly GraphicsDevice graphicsDevice;

        /// <summary>
        /// The pool for render containers
        /// </summary>
        protected IObjectPool<TextureRenderContainer> renderContainerPool;

        /// <summary>
        /// The render target to draw the scene on
        /// </summary>
        private RenderTarget2D sceneRenderTarget;

        /// <summary>
        /// Create a new instance of this render system
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="defaultEffekt">The default effekt to use</param>
        /// <param name="viewportAdapter">The viewport adapter to use</param>
        public SimpleRenderSystem(SpriteBatch spriteBatch, Effect defaultEffekt, IViewportAdapter viewportAdapter, GraphicsDevice graphicsDevice)
        {
            this.spriteBatch = spriteBatch;
            this.defaultEffekt = defaultEffekt;
            this.viewportAdapter = viewportAdapter;
            this.graphicsDevice = graphicsDevice;
            renderContainerPool = new ConcurrentObjectPool<TextureRenderContainer>(() => new TextureRenderContainer(), 10);
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
            BeginDraw();
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
                defaultEffekt,
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
            graphicsDevice.SetRenderTarget(sceneRenderTarget);
            graphicsDevice.Clear(Color.CornflowerBlue);
            foreach (TextureRenderContainer container in GetTextureContainers())
            {
                Vector2 position = container.PositionComponent.Position + container.TextureComponent.DrawOffset;
                float layerDepth = MathHelper.Clamp(container.TextureComponent.DrawLayer / MAX_LAYER_COUNT, 0f, MAX_LAYER_COUNT);

                spriteBatch.Draw(
                    container.TextureComponent.Texture,
                    position,
                    container.TextureComponent.Source,
                    container.TextureComponent.Color,
                    container.PositionComponent.Rotation,
                    container.TextureComponent.RotationCenter,
                    container.TextureComponent.Scale,
                    container.TextureComponent.SpriteEffect,
                    layerDepth
                    );
                renderContainerPool.Return(container);
            }
        }

        /// <summary>
        /// Get the container to render
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerable<TextureRenderContainer> GetTextureContainers()
        {
            return watchedEntities.Where(entityId => !entitiesToRemove.Contains(entityId))
                                  .Where(entityId => entityManager.HasComponent<PositionComponent>(entityId) && entityManager.HasComponent<TextureComponent>(entityId))
                                  .Select(entityId => CreateTextureContainer(entityId))
                                  .Where(container => container != null)
                                  .OrderBy(container => container.TextureComponent.Texture.Name);
        }

        /// <summary>
        /// Create a new container based on the entityId
        /// </summary>
        /// <param name="entityId">The entity id to use</param>
        /// <returns>The texture render container</returns>
        protected virtual TextureRenderContainer CreateTextureContainer(uint entityId)
        {
            PositionComponent positionComponent = entityManager.GetComponent<PositionComponent>(entityId);
            TextureComponent textureComponent = entityManager.GetComponent<TextureComponent>(entityId);
            return CreateContainer(positionComponent, textureComponent);
        }

        /// <summary>
        /// Create a new container based on the given component
        /// </summary>
        /// <param name="positionComponent">The position component</param>
        /// <param name="textureComponent">The texture component</param>
        /// <returns>A useable texture render container</returns>
        protected virtual TextureRenderContainer CreateContainer(PositionComponent positionComponent, TextureComponent textureComponent)
        {
            if (positionComponent == null || textureComponent == null)
            {
                return null;
            }
            TextureRenderContainer returnContainer = renderContainerPool.Get();
            returnContainer.PositionComponent = positionComponent;
            returnContainer.TextureComponent = textureComponent;
            return returnContainer;
        }

        /// <summary>
        /// Begin the main draw loop
        /// </summary>
        protected virtual void BeginDraw()
        {
            spriteBatch.Begin(
                SpriteSortMode.FrontToBack,
                BlendState.NonPremultiplied,
                null,
                null,
                null,
                defaultEffekt,
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