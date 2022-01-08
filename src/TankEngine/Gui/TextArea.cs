using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using TankEngine.DataStructures.Spritesheet;

namespace TankEngine.Gui
{
    /// <summary>
    /// A simple text area
    /// </summary>
    public class TextArea : VisibleUiElement
    {
        /// <summary>
        /// The default search filter to use for getting the ui visibles
        /// </summary>
        private const string DEFAULT_FILTER = "textarea";

        /// <summary>
        /// The number of middle parts
        /// </summary>
        protected int middlePartCount;

        /// <summary>
        /// The complete width
        /// </summary>
        protected int completeXSize;

        /// <summary>
        /// The areas for the gui element
        /// </summary>
        protected List<SpritesheetArea> Areas;

        /// <summary>
        /// The current left part to draw
        /// </summary>
        protected Rectangle leftPartToDraw;

        /// <summary>
        /// The current middle part to draw
        /// </summary>
        protected Rectangle centerPartToDraw;

        /// <summary>
        /// The current right part to draw
        /// </summary>
        protected Rectangle rightPartToDraw;

        /// <summary>
        /// Create a new text area element
        /// </summary>
        /// <param name="position">The position of the element</param>
        /// <param name="width">The width of the element</param>
        /// <param name="spritesheetTexture">Ther spritesheet texture to use</param>
        /// <param name="spriteBatch">The spritebatch used for drawing the texture</param>
        public TextArea(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch)
            : this(position, width, spritesheetTexture, spriteBatch, DEFAULT_FILTER)
        {
        }

        /// <summary>
        /// Create a new text area element
        /// </summary>
        /// <param name="position">The position of the element</param>
        /// <param name="width">The width of the element</param>
        /// <param name="spritesheetTexture">Ther spritesheet texture to use</param>
        /// <param name="spriteBatch">The spritebatch used for drawing the texture</param>
        /// <param name="baseFilter">The filter to use for getting the areas for the element on the spritesheet</param>
        public TextArea(Vector2 position, int width, SpritesheetTexture spritesheetTexture, SpriteBatch spriteBatch, string baseFilter)
            : base(position, width, spritesheetTexture, spriteBatch, baseFilter)
        {
        }

        /// <inheritdoc/>
        protected override void Setup()
        {
            //@Note: This is properly not always correct since there could be multiple areas with the same tag!
            SpritesheetArea centerArea = GetCenterArea();
            SpritesheetArea leftArea = GetLeftArea();
            SpritesheetArea rightArea = GetRightArea();

            if (centerArea == null || leftArea == null || rightArea == null)
            {
                return;
            }

            centerPartToDraw = centerArea.Area;
            leftPartToDraw = leftArea.Area;
            rightPartToDraw = rightArea.Area;

            middlePartCount = (int)Math.Round((float)width / centerPartToDraw.Width);
            middlePartCount = middlePartCount == 0 ? 1 : middlePartCount;
            completeXSize = leftPartToDraw.Width + rightPartToDraw.Width;
            completeXSize += centerPartToDraw.Width * middlePartCount;
        }

        /// <summary>
        /// Get the left area for inital setup
        /// </summary>
        /// <returns>The left area to use</returns>
        protected virtual SpritesheetArea GetCenterArea()
        {
            return Areas.FirstOrDefault(area => area.ContainsPropertyValue(CENTER_TAG, false));
        }

        /// <summary>
        /// Get the left area for inital setup
        /// </summary>
        /// <returns>The left area to use</returns>
        protected virtual SpritesheetArea GetLeftArea()
        {
            return Areas.FirstOrDefault(area => area.ContainsPropertyValue(LEFT_TAG, false));
        }

        /// <summary>
        /// Get the right area for inital setup
        /// </summary>
        /// <returns>The right area to use</returns>
        protected virtual SpritesheetArea GetRightArea()
        {
            return Areas.FirstOrDefault(area => area.ContainsPropertyValue(RIGHT_TAG, false));
        }

        /// <inheritdoc/>
        protected override void SetupAreas()
        {
            Areas = spritesheetTexture.Areas.Where(area => area.Properties.Any(SearchByPropertyValue("textarea"))).ToList();
        }

        /// <inheritdoc/>
        protected override void UpdateCollider()
        {
            collider = new Rectangle((int)Position.X, (int)Position.Y, completeXSize, centerPartToDraw.Height);
            Size = collider.Size.ToVector2();
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            DrawBackground();
            DrawText();
        }

        /// <inheritdoc/>
        protected virtual void DrawBackground()
        {

            spriteBatch.Draw(
                spritesheetTexture.Texture,
                new Rectangle((int)Position.X, (int)Position.Y, leftPartToDraw.Width, leftPartToDraw.Height),
                leftPartToDraw,
                Color.White
                );
            for (int i = 0; i < middlePartCount; i++)
            {
                int xCenterPosition = (int)(Position.X + leftPartToDraw.Width);
                xCenterPosition += centerPartToDraw.Width * i;
                spriteBatch.Draw(
                    spritesheetTexture.Texture,
                    new Rectangle(xCenterPosition, (int)Position.Y, centerPartToDraw.Width, centerPartToDraw.Height),
                    centerPartToDraw,
                    Color.White
                    );
            }

            int xPosition = (int)(Position.X + leftPartToDraw.Width);
            xPosition += centerPartToDraw.Width * (middlePartCount);
            spriteBatch.Draw(
                spritesheetTexture.Texture,
                new Rectangle(xPosition, (int)Position.Y, rightPartToDraw.Width, rightPartToDraw.Height),
                rightPartToDraw,
                Color.White
                );
        }

        /// <summary>
        /// Draw the text on the background
        /// </summary>
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

        /// <summary>
        /// Get the horizontal position of the text
        /// </summary>
        /// <param name="textToUse">The text to find the middle for</param>
        /// <returns>The correct position</returns>
        protected virtual Vector2 GetHorizontalTextMiddle(string textToUse)
        {
            Vector2 startPosition = Position;

            Vector2 textSize = GetTextLenght(textToUse);
            float middleSize = centerPartToDraw.Width * middlePartCount;

            startPosition += Vector2.UnitX * leftPartToDraw.Width;
            startPosition += Vector2.UnitX * (middleSize / 2);
            startPosition -= Vector2.UnitX * (textSize.X / 2);

            return startPosition;
        }

        /// <summary>
        /// Center the text vertical
        /// </summary>
        /// <param name="startPosition">The start position</param>
        /// <param name="text">The text to center</param>
        /// <returns>The correct y position</returns>
        protected virtual Vector2 CenterTextVertical(Vector2 startPosition, string text)
        {
            Vector2 textSize = GetTextLenght(text);
            return startPosition + Vector2.UnitY * (textSize.Y / 2);
        }
    }
}
