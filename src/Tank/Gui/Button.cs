using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Commands;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    /// <summary>
    /// A simple button
    /// </summary>
    class Button : TextArea
    {
        /// <summary>
        /// The last mouse state
        /// </summary>
        private MouseState lastMouseState;

        /// <summary>
        /// The normal left button source
        /// </summary>
        private Rectangle leftButtonSource;

        /// <summary>
        /// The normal middle button source
        /// </summary>
        private Rectangle middleButtonSource;

        /// <summary>
        /// The normal right button soruce
        /// </summary>
        private Rectangle rightButtonSource;

        /// <summary>
        /// The hover left button soruce
        /// </summary>
        private Rectangle leftActiveButtonSource;

        /// <summary>
        /// The hover middle button source
        /// </summary>
        private Rectangle middleActiveButtonSource;

        /// <summary>
        /// The hover right button source
        /// </summary>
        private Rectangle rightActiveButtonSource;

        /// <summary>
        /// Is the button clicked
        /// </summary>
        public bool Clicked { get; private set; }

        /// <summary>
        /// wait for the sound to complete
        /// </summary>
        private bool waitForSound;

        /// <summary>
        /// The command to use
        /// </summary>
        private ICommand command;

        /// <inheritdoc/>
        public Button(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spritebatch)
            : base(position, width, textureToShow, spritebatch)
        {
            lastMouseState = Mouse.GetState();
        }

        /// <inheritdoc/>
        protected override void Setup()
        {
            base.Setup();

            currentLeftSource = leftButtonSource;
            currentMiddleSource = middleButtonSource;
            currentRightSource = rightButtonSource;
        }

        /// <summary>
        /// Set the command to use on click
        /// </summary>
        /// <param name="command">The command to use</param>
        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        /// <inheritdoc/>
        protected override void SetupTextures()
        {
            imageSize = textureToShow.GetPatternImageSize("buttonLeft");
            leftButtonSource = textureToShow.GetAreaFromPatternName("ButtonLeft");
            middleButtonSource = textureToShow.GetAreaFromPatternName("ButtonMiddle");
            rightButtonSource = textureToShow.GetAreaFromPatternName("ButtonRight");

            leftActiveButtonSource = textureToShow.GetAreaFromPatternName("ButtonActiveLeft");
            middleActiveButtonSource = textureToShow.GetAreaFromPatternName("ButtonActiveMiddle");
            rightActiveButtonSource = textureToShow.GetAreaFromPatternName("ButtonActiveRight");
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            currentLeftSource = leftButtonSource;
            currentMiddleSource = middleButtonSource;
            currentRightSource = rightButtonSource;

            Clicked = false;
            if (collider.Contains(GetMousePosition()))
            {
                currentLeftSource = leftActiveButtonSource;
                currentMiddleSource = middleActiveButtonSource;
                currentRightSource = rightActiveButtonSource;
                if (hoverSound != null 
                    && !collider.Contains(mouseWrapper.GetPosition(lastMouseState.Position)) 
                    && hoverSoundInstance.State != SoundState.Playing)
                {
                    hoverSound.Play(effectVolume, 0, 0);
                }

                if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released && !waitForSound)
                {
                    if (clickSound != null && clickSoundInstance.State != SoundState.Playing)
                    {
                        clickSound.Play(effectVolume, 0, 0);
                        waitForSound = true;
                        return;
                    }
                    Clicked = true;
                }
            }

            if (waitForSound && clickSoundInstance.State == SoundState.Stopped)
            {
                waitForSound = false;
                if (command != null && command.CanExecute())
                {
                    command.Execute();
                }
                Clicked = true;
            }

            lastMouseState = mouseState;
        }
    }
}
