using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Tank.Commands;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class Button : VisibleUiElement
    {
        private MouseState lastMouseState;

        private Rectangle leftButtonSource;
        private Rectangle middleButtonSource;
        private Rectangle rightButtonSource;

        private Rectangle leftActiveButtonSource;
        private Rectangle middleActiveButtonSource;
        private Rectangle rightActiveButtonSource;

        private Rectangle currentLeftSource;
        private Rectangle currentMiddleSource;
        private Rectangle currentRightSource;

        private Position imageSize; 

        private int middlePartCount;
        private int completeXSize;

        public bool Clicked { get; private set; }
        private bool waitForSound;

        private ICommand command;

        public Button(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spritebatch)
            : base(position, width, textureToShow, spritebatch)
        {
        }

        protected override void Setup()
        {
            middlePartCount = (int)Math.Round((float)width / textureToShow.SingleImageSize.X);
            middlePartCount = middlePartCount == 0 ? 1 : middlePartCount;

            currentLeftSource = leftButtonSource;
            currentMiddleSource = middleButtonSource;
            currentRightSource = rightButtonSource;

            completeXSize = imageSize.X * 2;
            completeXSize += imageSize.X * middlePartCount;
        }

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        protected override void UpdateCollider()
        {
            collider = new Rectangle((int)Position.X, (int)Position.Y, completeXSize, imageSize.Y);
            Size = collider.Size.ToVector2();
        }

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

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(
                textureToShow.CompleteImage,
                new Rectangle((int)Position.X, (int)Position.Y, imageSize.X, imageSize.Y),
                currentLeftSource,
                Color.White
                );

            for (int i = 0; i < middlePartCount; i++)
            {
                int index = i + 1;
                spriteBatch.Draw(
                    textureToShow.CompleteImage,
                    new Rectangle((int)Position.X + imageSize.X * index, (int)Position.Y, imageSize.X, imageSize.Y),
                    currentMiddleSource,
                    Color.White
                    );
            }

            int xPosition = (int)Position.X + imageSize.X * 2;
            xPosition += imageSize.X * (middlePartCount - 1);
            spriteBatch.Draw(
                textureToShow.CompleteImage,
                new Rectangle(xPosition, (int)Position.Y, imageSize.X, imageSize.Y),
                currentRightSource,
                Color.White
                );

            if (font == null)
            {
                return;
            }
            Vector2 textPosition = Position;
            textPosition.X += imageSize.X;
            Vector2 textSize = GetTextLenght(text, font);
            textPosition += Vector2.UnitY * textSize.Y;
            float middleSize = imageSize.X * middlePartCount;
            textPosition.X += middleSize / 2;
            textPosition -= Vector2.UnitX * textSize.X / 2;
            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }

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
                    && hoverSound.State != SoundState.Playing)
                {
                    hoverSound.Play();
                }

                if (mouseState.LeftButton == ButtonState.Pressed && !waitForSound)
                {
                    if (clickSound != null && clickSound.State != SoundState.Playing)
                    {
                        clickSound.Play();
                        waitForSound = true;
                        return;
                    }
                    Clicked = true;
                }
            }

            if (waitForSound && clickSound.State == SoundState.Stopped)
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
