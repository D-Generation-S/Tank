using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.DataStructure.Spritesheet;
using Tank.Factories;
using Tank.Factories.Gui;
using Tank.Gui.Data;
using Tank.Wrapper;

namespace Tank.Gui
{
    /// <summary>
    /// A simple select box
    /// </summary>
    class SelectBox : TextArea
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
        /// Create a new instance of this class
        /// </summary>
        /// <param name="position">The position to use</param>
        /// <param name="width">The width to use</param>
        /// <param name="textureToShow">The texture to show</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        public SelectBox(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width, textureToShow, spriteBatch)
        {
        }

        /// <inheritdoc/>
        public override void SetFont(SpriteFont font)
        {
            leftButton.SetFont(font);
            rightButton.SetFont(font);
            base.SetFont(font);
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
            this.data.setCurrentDataset(start);
            text = this.data.GetCurrentDataSet().DisplayText;
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
            leftButton.Update(gameTime);
            base.Update(gameTime);
            rightButton.Update(gameTime);

            if (leftButton.Clicked && data != null)
            {
                data.PreviousDataSet();
            }

            if (rightButton.Clicked && data != null)
            {
                data.NextDataSet();
            }


            text = data.GetCurrentDataSet().DisplayText;
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            leftButton.Draw(gameTime);
            base.Draw(gameTime);
            rightButton.Draw(gameTime);
        }
    }
}
