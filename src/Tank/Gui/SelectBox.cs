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
    class SelectBox : TextArea
    {
        private Button leftButton;
        private Button rightButton;

        private SelectionData data;

        public SelectBox(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spriteBatch) : base(position, width, textureToShow, spriteBatch)
        {
        }

        public override void SetFont(SpriteFont font)
        {
            leftButton.SetFont(font);
            rightButton.SetFont(font);
            base.SetFont(font);
        }

        public void SetData(List<SelectionDataSet> data)
        {
            SetData(data, 0);
        }

        public void SetData(List<SelectionDataSet> data, int start)
        {
            this.data = new SelectionData(data);
            this.data.setCurrentDataset(start);
            text = this.data.GetCurrentDataSet().DisplayText;
        }

        public SelectionDataSet GetData()
        {
            if (data == null)
            {
                return null;
            }
            return data.GetCurrentDataSet();
        }

        public override void SetPosition(Vector2 position)
        {
            leftButton.SetPosition(position);
            position.X += leftButton.Size.X;
            Vector2 rightButtonPosition = position;
            rightButtonPosition.X += Size.X;
            rightButton.SetPosition(rightButtonPosition);
            base.SetPosition(position);
        }

        public override void SetClickEffect(SoundEffect clickSound)
        {
            leftButton.SetClickEffect(clickSound);
            rightButton.SetClickEffect(clickSound);
        }

        public override void SetHoverEffect(SoundEffect hoverSound)
        {
            leftButton.SetHoverEffect(hoverSound);
            rightButton.SetHoverEffect(hoverSound);
        }

        public override void SetMouseWrapper(MouseWrapper mouseWrapper)
        {
            leftButton.SetMouseWrapper(mouseWrapper);
            rightButton.SetMouseWrapper(mouseWrapper);
            base.SetMouseWrapper(mouseWrapper);
        }

        protected override void Setup()
        {
            base.Setup();
            IFactory<Button> buttonFactory = new ButtonFactory(font, textureToShow, spriteBatch, 50, Vector2.Zero);
            leftButton = buttonFactory.GetNewObject();
            rightButton = buttonFactory.GetNewObject();

            leftButton.SetText("<<");
            rightButton.SetText(">>");
        }

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

        public override void Draw(GameTime gameTime)
        {
            leftButton.Draw(gameTime);
            base.Draw(gameTime);
            rightButton.Draw(gameTime);
        }
    }
}
