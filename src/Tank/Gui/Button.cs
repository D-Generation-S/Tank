using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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

        private SoundEffectInstance hoverEffect;
        private SoundEffectInstance clickEffect;

        public string Text;
        private readonly SpriteFont font;

        private int middlePartCount;
        private Rectangle collider;
        private int completeXSize;

        public bool Clicked { get; private set; }
        private bool waitForSound;

        public Button(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spritebatch, SpriteFont font)
            : base(position, width, textureToShow, spritebatch)
        {
            this.font = font;
            FillTextures(textureToShow);

            middlePartCount = (int)Math.Round((float)width / textureToShow.SingleImageSize.X);
            middlePartCount = middlePartCount == 0 ? 1 : middlePartCount;

            currentLeftSource = leftButtonSource;
            currentMiddleSource = middleButtonSource;
            currentRightSource = rightButtonSource;

            completeXSize = textureToShow.SingleImageSize.X * 2;
            completeXSize += textureToShow.SingleImageSize.X * middlePartCount;
            
            Text = string.Empty;
            UpdateCollider();
        }

        public void SetClickEffect(SoundEffect soundEffect)
        {
            clickEffect = soundEffect.CreateInstance();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            UpdateCollider();
        }

        private void UpdateCollider()
        {
            collider = new Rectangle((int)Position.X, (int)Position.Y, completeXSize, textureToShow.SingleImageSize.Y);
            Size = collider.Size.ToVector2();
        }

        private void FillTextures(SpriteSheet textureToShow)
        {
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
                new Rectangle((int)Position.X, (int)Position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y),
                currentLeftSource,
                Color.White
                );

            for (int i = 0; i < middlePartCount; i++)
            {
                int index = i + 1;
                spriteBatch.Draw(
                    textureToShow.CompleteImage,
                    new Rectangle((int)Position.X + textureToShow.SingleImageSize.X * index, (int)Position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y),
                    currentMiddleSource,
                    Color.White
                    );
            }

            int xPosition = (int)Position.X + textureToShow.SingleImageSize.X * 2;
            xPosition += textureToShow.SingleImageSize.X * (middlePartCount - 1);
            spriteBatch.Draw(
                textureToShow.CompleteImage,
                new Rectangle(xPosition, (int)Position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y),
                currentRightSource,
                Color.White
                );

            Vector2 textPosition = Position;
            textPosition.X += textureToShow.SingleImageSize.X;
            Vector2 textSize = GetTextLenght(Text, font);
            textPosition += Vector2.UnitY * textSize.Y;
            float middleSize = textureToShow.SingleImageSize.X * middlePartCount;
            textPosition.X += middleSize / 2;
            textPosition -= Vector2.UnitX * textSize.X / 2;
            spriteBatch.DrawString(font, Text, textPosition, Color.Black);
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

                if (mouseState.LeftButton == ButtonState.Pressed && !waitForSound)
                {
                    if (clickEffect != null && clickEffect.State != SoundState.Playing)
                    {
                        clickEffect.Play();
                        waitForSound = true;
                        return;
                    }
                    Clicked = true;
                }
            }

            if (waitForSound && clickEffect.State == SoundState.Stopped)
            {
                waitForSound = false;
                Clicked = true;
            }

            lastMouseState = mouseState;
        }
    }
}
