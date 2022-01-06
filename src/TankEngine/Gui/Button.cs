using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using TankEngine.Commands;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Gui
{
    /// <summary>
    /// A simple button
    /// </summary>
    public class Button : TextArea
    {
        /// <summary>
        /// The last mouse state
        /// </summary>
        private MouseState lastMouseState;

        /// <summary>
        /// The normal left button source
        /// </summary>
        private Rectangle leftIdle;

        /// <summary>
        /// The normal middle button source
        /// </summary>
        private Rectangle centerIdle;

        /// <summary>
        /// The normal right button soruce
        /// </summary>
        private Rectangle rightIdle;

        /// <summary>
        /// The hover left button soruce
        /// </summary>
        private Rectangle leftActive;

        /// <summary>
        /// The hover middle button source
        /// </summary>
        private Rectangle centerActive;

        /// <summary>
        /// The hover right button source
        /// </summary>
        private Rectangle rightActive;

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
        public Button(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spritebatch)
            : base(position, width, spritesheetTexture, spritebatch)
        {
            lastMouseState = Mouse.GetState();
        }

        /// <inheritdoc/>
        protected override void Setup()
        {
            base.Setup();
            leftIdle = leftPartToDraw;
            centerIdle = centerPartToDraw;
            rightIdle = rightPartToDraw;

            leftActive = GetArea(LEFT_TAG, "active").Area;
            centerActive = GetArea(CENTER_TAG, "active").Area;
            rightActive = GetArea(RIGHT_TAG, "active").Area;
        }

        /// <summary>
        /// Set the command to use on click
        /// </summary>
        /// <param name="command">The command to use</param>
        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        /// <summary>
        /// Set the command to use on click
        /// </summary>
        /// <param name="actionToPerform">The action to perform</param>
        public void SetCommand(Action actionToPerform)
        {
            command = new ActionCommand(actionToPerform);
        }

        /// <inheritdoc/>
        protected override SpritesheetArea GetCenterArea()
        {
            return GetArea(CENTER_TAG, "idle");
        }

        /// <inheritdoc/>
        protected override SpritesheetArea GetLeftArea()
        {
            return GetArea(LEFT_TAG, "idle");
        }

        /// <inheritdoc/>
        protected override SpritesheetArea GetRightArea()
        {
            return GetArea(RIGHT_TAG, "idle");
        }

        private SpritesheetArea GetArea(string orientation, string state)
        {
            return Areas.FirstOrDefault(area => area.ContainsPropertyValue(orientation, false) && area.ContainsPropertyValue(state, false));
        }

        /// <inheritdoc/>
        protected override void SetupAreas()
        {
            Areas = spritesheetTexture.Areas.Where(area => area.Properties.Any(SearchByPropertyValue("button"))).ToList();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();

            leftPartToDraw = leftIdle;
            centerPartToDraw = centerIdle;
            rightPartToDraw = rightIdle;

            Clicked = false;
            if (collider.Contains(GetMousePosition()))
            {
                leftPartToDraw = leftActive;
                centerPartToDraw = centerActive;
                rightPartToDraw = rightActive;
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
