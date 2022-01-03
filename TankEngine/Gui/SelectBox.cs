using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.Factories;
using TankEngine.Factories.Gui;
using TankEngine.Gui.Data;
using TankEngine.Wrapper;

namespace TankEngine.Gui
{
    /// <summary>
    /// A simple select box
    /// </summary>
    public class SelectBox : TextArea
    {
        /// <summary>
        /// The left button of the box
        /// </summary>
        private Button leftButton;

        /// <summary>
        /// The right button of the box
        /// </summary>
        private Button rightButton;

        /// <summary>
        /// The data of this selection
        /// </summary>
        private SelectionData data;

        /// <summary>
        /// The currentSelectionText
        /// </summary>
        private string selectionText;

        /// <summary>
        /// Was there s specific text set?
        /// </summary>
        private bool textSet;

        /// <summary>
        /// Some specific text offset
        /// </summary>
        private Vector2 textOffset;

        /// <summary>
        /// Has the content be changed
        /// </summary>
        public bool Changed { get; private set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="width">The width to use</param>
        /// <param name="textureToShow">The texture to show</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        public SelectBox(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width, textureToShow, spriteBatch)
        {
            textOffset = Vector2.Zero;
        }

        /// <inheritdoc/>
        public override void SetFont(SpriteFont font)
        {
            leftButton.SetFont(font);
            rightButton.SetFont(font);
            base.SetFont(font);
        }

        /// <summary>
        /// Set a  specific y-axis text offset
        /// </summary>
        /// <param name="textOffset">The text offset to use</param>
        public virtual void SetTextOffset(float textOffset)
        {
            this.textOffset = Vector2.UnitY * textOffset;
        }

        /// <inheritdoc/>
        public override void SetText(string text)
        {
            base.SetText(text);
            textSet = true;
        }

        /// <summary>
        /// Set the data for this selection
        /// </summary>
        /// <param name="data">The data to use</param>
        public void SetData(List<SelectionDataSet> data)
        {
            SetData(data, 0);
        }

        /// <summary>
        /// Set the data for this election
        /// </summary>
        /// <param name="data">The data to use</param>
        /// <param name="start">The position to start on the selection</param>
        public void SetData(List<SelectionDataSet> data, int start)
        {
            this.data = new SelectionData(data);
            SetCurrentDataSet(start);
        }

        /// <summary>
        /// Set the current data set
        /// </summary>
        /// <param name="start">The data set to set</param>
        public void SetCurrentDataSet(int start)
        {
            data.setCurrentDataset(start);
            selectionText = data.GetCurrentDataSet().DisplayText;
        }

        /// <summary>
        /// Get the current selected data
        /// </summary>
        /// <returns>The current data</returns>
        public SelectionDataSet GetData()
        {
            if (data == null)
            {
                return null;
            }
            return data.GetCurrentDataSet();
        }

        /// <inheritdoc/>
        public override void SetPosition(Vector2 position)
        {
            leftButton.SetPosition(position);
            position.X += leftButton.Size.X;
            Vector2 rightButtonPosition = position;
            rightButtonPosition.X += Size.X;
            rightButton.SetPosition(rightButtonPosition);
            base.SetPosition(position);
        }

        /// <inheritdoc/>
        public override void SetEffectVolume(float volume)
        {
            leftButton.SetEffectVolume(volume);
            rightButton.SetEffectVolume(volume);
        }

        /// <inheritdoc/>
        public override void SetClickEffect(SoundEffect clickSound)
        {
            leftButton.SetClickEffect(clickSound);
            rightButton.SetClickEffect(clickSound);
        }

        /// <inheritdoc/>
        public override void SetHoverEffect(SoundEffect hoverSound)
        {
            leftButton.SetHoverEffect(hoverSound);
            rightButton.SetHoverEffect(hoverSound);
        }

        /// <inheritdoc/>
        public override void SetMouseWrapper(MouseWrapper mouseWrapper)
        {
            leftButton.SetMouseWrapper(mouseWrapper);
            rightButton.SetMouseWrapper(mouseWrapper);
            base.SetMouseWrapper(mouseWrapper);
        }

        /// <inheritdoc/>
        protected override void Setup()
        {
            base.Setup();
            IFactory<Button> buttonFactory = new ButtonFactory(font, textureToShow, spriteBatch, 50, Vector2.Zero);
            leftButton = buttonFactory.GetNewObject();
            rightButton = buttonFactory.GetNewObject();

            leftButton.SetText("<<");
            rightButton.SetText(">>");
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            Changed = false;
            leftButton.Update(gameTime);
            base.Update(gameTime);
            rightButton.Update(gameTime);

            if (leftButton.Clicked && data != null)
            {
                data.PreviousDataSet();
                Changed = true;
            }

            if (rightButton.Clicked && data != null)
            {
                data.NextDataSet();
                Changed = true;
            }

            if (data == null)
            {
                return;
            }
            selectionText = data.GetCurrentDataSet().DisplayText;

        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            leftButton.Draw(gameTime);
            base.Draw(gameTime);
            rightButton.Draw(gameTime);
        }

        /// <inheritdoc/>
        protected override void DrawText()
        {
            if (font == null)
            {
                return;
            }
            string upperText = text;
            string lowerText = selectionText;
            if (!textSet)
            {
                text = selectionText;
                base.DrawText();
                return;
            }
            Vector2 upperTextPosition = GetHorizontalTextMiddle(upperText);
            Vector2 lowerTextPosition = GetHorizontalTextMiddle(lowerText);
            lowerTextPosition += Vector2.UnitY * imageSize.Y - Vector2.UnitY * GetTextLenght(lowerText).Y;

            upperTextPosition += textOffset;
            lowerTextPosition -= textOffset;

            spriteBatch.DrawString(font, upperText, upperTextPosition, Color.Black);
            spriteBatch.DrawString(font, lowerText, lowerTextPosition, Color.Black);
        }
    }
}
