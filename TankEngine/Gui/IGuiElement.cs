using Microsoft.Xna.Framework;
using TankEngine.Wrapper;

namespace TankEngine.Gui
{
    /// <summary>
    /// Interface to describe gui elements
    /// </summary>
    public interface IGuiElement : GameStates.IUpdateable, GameStates.IDrawable
    {
        /// <summary>
        /// Set the mouse wrapper so we can use virtual viewports
        /// </summary>
        /// <param name="mouseWrapper"></param>
        void SetMouseWrapper(MouseWrapper mouseWrapper);

        /// <summary>
        /// The name of the element
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The size of the element
        /// </summary>
        Vector2 Size { get; }

        /// <summary>
        /// The positiotn of the element
        /// </summary>
        Vector2 Position { get; }

        /// <summary>
        /// Set the element position
        /// </summary>
        /// <param name="position">The new position to use</param>
        void SetPosition(Vector2 position);
    }
}
