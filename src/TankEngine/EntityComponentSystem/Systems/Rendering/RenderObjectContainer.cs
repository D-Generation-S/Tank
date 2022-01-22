using Microsoft.Xna.Framework.Graphics;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Components.World;

namespace TankEngine.EntityComponentSystem.Systems.Rendering
{
    /// <summary>
    /// Container for texture rendering
    /// </summary>
    public class RenderObjectContainer
    {
        /// <summary>
        /// Type of the render container
        /// </summary>
        public RenderContainerTypeEnum ContainerType { get; set; }

        /// <summary>
        /// The position component used for getting the position
        /// </summary>
        public PositionComponent PositionComponent { get; set; }

        /// <summary>
        /// The texture component used for drawing
        /// </summary>
        public TextureComponent TextureComponent { get; set; }

        /// <summary>
        /// The text component used for drawing
        /// </summary>
        public TextComponent TextComponent { get; set; }

        /// <summary>
        /// The draw layer of the container
        /// </summary>
        public int DrawLayer
        {
            get
            {
                if (ContainerType == RenderContainerTypeEnum.Texture && TextureComponent != null)
                {
                    return TextureComponent.DrawLayer;
                }
                if (ContainerType == RenderContainerTypeEnum.Text && TextComponent != null)
                {
                    return TextComponent.DrawLayer;
                }
                return int.MinValue;
            }
        }

        /// <summary>
        /// The name to search for, this is as exmaple the name of the texture
        /// </summary>
        public string Name
        {
            get
            {
                if (ContainerType == RenderContainerTypeEnum.Texture && TextureComponent != null)
                {
                    return TextureComponent.Texture.Name;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// The name of the effect to use with the entity
        /// </summary>
        public string EffectName => ShaderEffect == null ? string.Empty : ShaderEffect.Name;

        /// <summary>
        /// The shader effect to use for drawing
        /// </summary>
        public Effect ShaderEffect
        {
            get
            {
                if (ContainerType == RenderContainerTypeEnum.Texture && TextureComponent != null && TextureComponent.ShaderEffect != null)
                {
                    return TextureComponent.ShaderEffect;
                }
                if (ContainerType == RenderContainerTypeEnum.Text && TextComponent != null && TextComponent.ShaderEffect != null)
                {
                    return TextComponent.ShaderEffect;
                }
                return null;
            }
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public RenderObjectContainer()
        {
            Reset();
        }

        /// <summary>
        /// Reset the containet
        /// </summary>
        public void Reset()
        {
            PositionComponent = null;
            TextureComponent = null;
            TextComponent = null;
            ContainerType = RenderContainerTypeEnum.Unknown;
        }
    }
}
