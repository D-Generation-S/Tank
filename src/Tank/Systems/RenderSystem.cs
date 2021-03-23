using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tank.Adapter;
using Tank.Builders;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.DataStructure;
using Tank.Enums;
using Tank.Events;
using Tank.Events.EntityBased;
using Tank.Events.StateEvents;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Utils;
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
        private readonly SpriteFont fontToUse;

        /// <summary>
        /// Builder to use for showing that a screenshot was taken
        /// </summary>
        private readonly IGameObjectBuilder textBuilder;

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
        /// The screenshot render target
        /// </summary>
        private RenderTarget2D screenshotRenderTarget;

        /// <summary>
        /// All the post processing effects
        /// </summary>
        private readonly List<Effect> postProcessing;

        /// <summary>
        /// The graphic device to use
        /// </summary>
        private GraphicsDevice graphicsDevice => TankGame.PublicGraphicsDevice;

        /// <summary>
        /// The viewport adapter
        /// </summary>
        private IViewportAdapter viewportAdapter => TankGame.PublicViewportAdapter;

        /// <summary>
        /// The virtual viewport
        /// </summary>
        private Viewport viewport => viewportAdapter.VirtualViewport;

        /// <summary>
        /// Is this the start of the draw call
        /// </summary>
        private bool drawStart;

        /// <summary>
        /// The base path for screenshots
        /// </summary>
        private string screenshotBasePath;

        /// <summary>
        /// The path to save a screenshot it
        /// </summary>
        private string screenshotPath;

        /// <summary>
        /// Should we take a screenshot in the current draw
        /// </summary>
        private bool takeScreenshot => screenshotPath != string.Empty && screenshotPath != null;

        /// <summary>
        /// Texture to use for debugging
        /// </summary>
        private Texture2D debugTexture;

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(SpriteBatch spriteBatch, SpriteFont fontToUse, Effect defaultEffect)
            : this(spriteBatch, fontToUse, defaultEffect, new List<Effect>() { defaultEffect })
        {
        }

        /// <summary>
        /// Create a new instance for the renderer
        /// </summary>
        /// <param name="spriteBatch"></param>
        public RenderSystem(
            SpriteBatch spriteBatch,
            SpriteFont fontToUse,
            Effect defaultEffect,
            List<Effect> postProcessing
            ) : base()
        {
            this.spriteBatch = spriteBatch;
            this.fontToUse = fontToUse;
            textBuilder = new FadeOutTextBuilder(fontToUse);
            validators.Add(new RenderableEntityValidator());

            usedContainers = new Stack<RenderContainer>();
            containersToRender = new List<RenderContainer>();
            this.defaultEffect = defaultEffect;
            this.postProcessing = postProcessing;
            gameRenderTarget = new RenderTarget2D(graphicsDevice, viewportAdapter.Viewport.Width, viewportAdapter.Viewport.Height);
            postProcessingRenderTarget = new RenderTarget2D(graphicsDevice, viewportAdapter.Viewport.Width, viewportAdapter.Viewport.Height);
            screenshotRenderTarget = new RenderTarget2D(graphicsDevice, viewportAdapter.Viewport.Width, viewportAdapter.Viewport.Height);

            drawStart = true;
            DefaultFolderUtils folderUtils = new DefaultFolderUtils();
            screenshotBasePath = folderUtils.GetGameFolder();
            screenshotBasePath = Path.Combine(screenshotBasePath, "Screenshots");

            Color[] debugTextureColor = new Color[1];
            debugTextureColor[0] = Color.White;
            debugTexture = new Texture2D(graphicsDevice, 1, 1);
            debugTexture.SetData(debugTextureColor);

        }

        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent(this, typeof(TakeScreenshotEvent));

            textBuilder.Init(entityManager);
        }

        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is TakeScreenshotEvent)
            {
                if (!Directory.Exists(screenshotBasePath))
                {
                    Directory.CreateDirectory(screenshotBasePath);
                }

                CopyRenderTarget(postProcessing.Count == 0 ? gameRenderTarget : postProcessingRenderTarget, screenshotRenderTarget);
                Task.Run(() => TakeScreenshot(screenshotRenderTarget, screenshotPath));
                screenshotPath = string.Empty;

                screenshotPath = Path.Combine(screenshotBasePath, DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_GameScreenshot.png");
                AddEntityEvent newEntityEvent = eventManager.CreateEvent<AddEntityEvent>();
                string text = "Took screenshot: " + screenshotPath;
                List<IComponent> components = textBuilder.BuildGameComponents(text);
                PlaceableComponent placeableComponent = (PlaceableComponent)components.Find(component => component.Type == typeof(PlaceableComponent));

                List<uint> messageEntities = entityManager.GetEntitiesWithComponent<MessageTag>();
                Vector2 textSize = fontToUse.MeasureString(text);

                Vector2 realPosition = Vector2.UnitY * (screenshotRenderTarget.Height);
                realPosition += Vector2.UnitX * textSize.X;
                realPosition -= textSize;
                realPosition -= Vector2.UnitY * (textSize.Y * messageEntities.Count);
                placeableComponent.Position = realPosition;

                newEntityEvent.Components = components;
                FireEvent(newEntityEvent);


            }
        }

        private async Task<bool> TakeScreenshot(RenderTarget2D renderTarget, string path)
        {
            using (StreamWriter writer = new StreamWriter(screenshotPath))
            {
                renderTarget.SaveAsPng(writer.BaseStream, renderTarget.Width, renderTarget.Height);
            }
            return true;
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

            containersToRender = containersToRender.OrderBy(container => container.RenderType).ThenBy(container => container.ShaderEffect).ThenBy(container => container.Name).ToList();
            graphicsDevice.SetRenderTarget(gameRenderTarget);
            for (int i = 0; i < containersToRender.Count; i++)
            {
                RenderEntities(containersToRender[i]);
                usedContainers.Push(containersToRender[i]);
            }
            containersToRender.Clear();
            if (drawStart)
            {
                graphicsDevice.SetRenderTarget(null);
                return;
            }
            spriteBatch.End();

            for (int i = 0; i < postProcessing.Count; i++)
            {
                graphicsDevice.SetRenderTarget(postProcessingRenderTarget);
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    null,
                    null,
                    null,
                    postProcessing[i],
                    viewportAdapter.GetScaleMatrix()
                    );

                spriteBatch.Draw(gameRenderTarget, new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);
                spriteBatch.End();
                CopyRenderTarget(postProcessingRenderTarget, gameRenderTarget);
            }
            
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
                postProcessing.Count == 0 ? gameRenderTarget : postProcessingRenderTarget,
                new Rectangle(0, 0, viewport.Width, viewport.Height),
                Color.White
                );

            spriteBatch.End();


            drawLocked = false;
        }

        /// <summary>
        /// Copy the render target
        /// </summary>
        /// <param name="source">The source of the render target</param>
        /// <param name="renderTarget">The target to render to</param>
        private void CopyRenderTarget(RenderTarget2D source, RenderTarget2D renderTarget)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            spriteBatch.Begin();
            spriteBatch.Draw(
                source,
                new Rectangle(0, 0, viewport.Width, viewport.Height),
                Color.White
                );
            spriteBatch.End();
        }

        /// <summary>
        /// Render a given entity
        /// </summary>
        /// <param name="currentContainer">The current container to render</param>
        private void RenderEntities(RenderContainer currentContainer)
        {
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
            spriteBatch.Draw(debugTexture, new Rectangle((int)currentContainer.Position.X - 8, (int)currentContainer.Position.Y - 8, 16,16), null, Color.Violet);
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
                    BlendState.NonPremultiplied,
                    null,
                    null,
                    null,
                    effect,
                    viewportAdapter.GetScaleMatrix());
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
                    BlendState.NonPremultiplied,
                    null,
                    null,
                    null,
                    effect,
                    viewportAdapter.GetScaleMatrix()
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

            Rectangle destination = visibleComponent.Destination;
            destination.X = (int)placeableComponent.Position.X;
            destination.Y = (int)placeableComponent.Position.Y;
            if (visibleComponent.DrawMiddle)
            {
                int witdth = visibleComponent.SingleTextureSize != Rectangle.Empty ? visibleComponent.SingleTextureSize.Width : visibleComponent.Texture.Width;
                int height = visibleComponent.SingleTextureSize != Rectangle.Empty ? visibleComponent.SingleTextureSize.Height : visibleComponent.Texture.Height;
                destination.X -= witdth / 2;
                destination.Y -= height / 2;
            }
            visibleComponent.Destination = destination;

            RenderContainer renderContainer = GetRenderContainer();
            renderContainer.RenderType = RenderTypeEnum.Texture;
            renderContainer.TextureToDraw = visibleComponent.Texture;
            renderContainer.Destination =  visibleComponent.Destination;
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
