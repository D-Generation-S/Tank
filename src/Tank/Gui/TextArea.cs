using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{ 
    /// <summary>
    /// A simple text area
    /// </summary>
    class TextArea : VisibleUiElement
    {
        /// <summary>
        /// The number of middle parts
        /// </summary>
        protected int middlePartCount;

        /// <summary>
        /// The complete width
        /// </summary>
        protected int completeXSize;

        /// <summary>
        /// The size of a single image
        /// </summary>
        protected Position imageSize;

        /// <summary>
        /// The current left part to draw
        /// </summary>
        protected Rectangle currentLeftSource;

        /// <summary>
        /// The current middle part to draw
        /// </summary>
        protected Rectangle currentMiddleSource;

        /// <summary>
        /// The current right part to draw
        /// </summary>
        protected Rectangle currentRightSource;

        /// <inheritdoc/>
        public TextArea(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width, textureToShow, spriteBatch)
        {
        }

        /// <inheritdoc/>
        protected override void Setup()
        {
            middlePartCount = (int)Math.Round((float)width / textureToShow.SingleImageSize.X);
            middlePartCount = middlePartCount == 0 ? 1 : middlePartCount;

            completeXSize = imageSize.X * 2;
            completeXSize += imageSize.X * middlePartCount;
        }

        /// <inheritdoc/>
        protected override void SetupTextures()
        {
            imageSize = textureToShow.GetPatternImageSize("buttonLeft");
            currentLeftSource = textureToShow.GetAreaFromPatternName("ButtonLeft");
            currentMiddleSource = textureToShow.GetAreaFromPatternName("ButtonMiddle");
            currentRightSource = textureToShow.GetAreaFromPatternName("ButtonRight");
        }

        /// <inheritdoc/>
        protected override void UpdateCollider()
        {
            collider = new Rectangle((int)Position.X, (int)Position.Y, completeXSize, imageSize.Y);
            Size = collider.Size.ToVector2();
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            DrawBackground();
            DrawText();
        }

        protected virtual void DrawBackground()
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
        }

        protected virtual void DrawText()
        {
            if (font == null)
            {
                return;
            }
            Vector2 textPosition = GetHorizontalTextMiddle(text);
            textPosition = CenterTextVertical(textPosition, text);
            spriteBatch.DrawString(font, text, textPosition, Color.Black);
        }

        protected virtual Vector2 GetHorizontalTextMiddle(string textToUse)
        {
            Vector2 startPosition = Position;

            Vector2 textSize = GetTextLenght(textToUse);
            float middleSize = imageSize.X * middlePartCount;

            startPosition += Vector2.UnitX * imageSize.X;
            startPosition += Vector2.UnitX * (middleSize / 2);
            startPosition -= Vector2.UnitX * (textSize.X / 2);

            return startPosition;
        }

        protected virtual Vector2 CenterTextVertical(Vector2 startPosition, string text)
        {
            Vector2 textSize = GetTextLenght(text);
            return startPosition + Vector2.UnitY * textSize.Y;
        }
    }
}
