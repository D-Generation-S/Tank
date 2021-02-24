using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Tank.Components.Rendering
{
    /// <summary>
    /// This class represents a animation on this object
    /// </summary>
    class AnimationComponent : BaseComponent
    {
        /// <summary>
        /// Public accessor to the animation name
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Public accessor if the animation is active right now
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Public access to the animation direction
        /// </summary>
        public bool ForwardDirection { get; set; }

        /// <summary>
        /// Public accessor if the animation is a ping pong animation
        /// </summary>
        public bool PingPong { get; set; }

        /// <summary>
        /// Public accessor if the animation is played in a loop
        /// </summary>
        public bool Loop { get; set; }

        /// <summary>
        /// Public accessor to the current frame index
        /// </summary>
        public int CurrentIndex { get; set; }

        /// <summary>
        /// Public access to the threshold for moving the index
        /// </summary>
        public float TimeThreshold { get; set; }

        /// <summary>
        /// Readonly access to the update frequency for the index in seconds
        /// </summary>
        public float FrameSeconds { get; set; }

        /// <summary>
        /// Readonly access to the sprite positions making up this animation
        /// </summary>
        public List<Rectangle> SpriteSources { get; set; }

        public override void Init()
        {
            Name = string.Empty;
            Active = false;
            ForwardDirection = true;
            PingPong = false;
            Loop = false;
            CurrentIndex = 0;
            TimeThreshold = 0;
            FrameSeconds = 1f;
            SpriteSources = null;
        }
    }
}
