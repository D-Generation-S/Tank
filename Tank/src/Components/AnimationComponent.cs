using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    class AnimationComponent : BaseComponent
    {
        private string name;
        public string Name
        {
            get => name;
            set => name = value;
        }

        private bool active;
        public bool Active
        {
            get => active;
            set => active = value;
        }

        private bool forwardDirection;
        public bool ForwardDirection
        {
            get => forwardDirection;
            set => forwardDirection = value;
        }

        private bool pingPong;
        public bool PingPong
        {
            get => pingPong;
            set => pingPong = value;
        }

        private bool loop;
        public bool Loop
        {
            get => loop;
            set => loop = value;
        }

        private int currentIndex;
        public int CurrentIndex
        {
            get => currentIndex;
            set => currentIndex = value;
        }


        private float timeThreshold;
        public float TimeThreshold
        {
            get => timeThreshold;
            set => timeThreshold = value;
        }

        private readonly float frameSeconds;
        public float FrameSeconds => frameSeconds;

        private readonly List<Rectangle> spriteSources;
        public List<Rectangle> SpriteSources => spriteSources;

        public AnimationComponent(float frameSeconds, List<Rectangle> spriteSources)
            : this(0, frameSeconds, spriteSources)
        {

        }

        public AnimationComponent(int startIndex, float frameSeconds, List<Rectangle> spriteSources)
        {
            currentIndex = startIndex;
            this.frameSeconds = frameSeconds;
            this.spriteSources = spriteSources;
            this.ForwardDirection = true;
            
        }
    }
}
