using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Components
{
    /// <summary>
    /// This class represents a animation on this object
    /// </summary>
    class AnimationComponent : BaseComponent
    {
        /// <summary>
        /// Name of the animation
        /// </summary>
        private string name;

        /// <summary>
        /// Public accessor to the animation name
        /// </summary>
        public string Name
        {
            get => name;
            set => name = value;
        }

        /// <summary>
        /// Is this animation activ at the moment
        /// </summary>
        private bool active;


        /// <summary>
        /// Public accessor if the animation is active right now
        /// </summary>
        public bool Active
        {
            get => active;
            set => active = value;
        }

        /// <summary>
        /// Animation is played in forward direction right now
        /// </summary>
        private bool forwardDirection;

        /// <summary>
        /// Public access to the animation direction
        /// </summary>
        public bool ForwardDirection
        {
            get => forwardDirection;
            set => forwardDirection = value;
        }

        /// <summary>
        /// Is the animation a ping pong animation
        /// </summary>
        private bool pingPong;

        /// <summary>
        /// Public accessor if the animation is a ping pong animation
        /// </summary>
        public bool PingPong
        {
            get => pingPong;
            set => pingPong = value;
        }

        /// <summary>
        /// Is the animation played in a loop
        /// </summary>
        private bool loop;

        /// <summary>
        /// Public accessor if the animation is played in a loop
        /// </summary>
        public bool Loop
        {
            get => loop;
            set => loop = value;
        }

        /// <summary>
        /// Current index of the frame
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// Public accessor to the current frame index
        /// </summary>
        public int CurrentIndex
        {
            get => currentIndex;
            set => currentIndex = value;
        }

        /// <summary>
        /// The seconds used a threshold to move the index
        /// </summary>
        private float timeThreshold;

        /// <summary>
        /// Public access to the threshold for moving the index
        /// </summary>
        public float TimeThreshold
        {
            get => timeThreshold;
            set => timeThreshold = value;
        }

        /// <summary>
        /// The update frequency for the index in seconds
        /// </summary>
        private readonly float frameSeconds;

        /// <summary>
        /// Readonly access to the update frequency for the index in seconds
        /// </summary>
        public float FrameSeconds => frameSeconds;

        /// <summary>
        /// All the rectangles in the sprite making up this animation
        /// </summary>
        private readonly List<Rectangle> spriteSources;

        /// <summary>
        /// Readonly access to the sprite positions making up this animation
        /// </summary>
        public List<Rectangle> SpriteSources => spriteSources;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="frameSeconds">The second threshold used to update the index</param>
        /// <param name="spriteSources">The frames in the sprite</param>
        public AnimationComponent(float frameSeconds, List<Rectangle> spriteSources)
            : this(0, frameSeconds, spriteSources)
        {

        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="startIndex">The index to start the animation at</param>
        /// <param name="frameSeconds">The second threshold used to update the index</param>
        /// <param name="spriteSources">The frames in the sprite</param>
        public AnimationComponent(int startIndex, float frameSeconds, List<Rectangle> spriteSources)
        {
            currentIndex = startIndex;
            this.frameSeconds = frameSeconds;
            this.spriteSources = spriteSources;
            ForwardDirection = true;
            allowMultiple = true;
        }
    }
}
