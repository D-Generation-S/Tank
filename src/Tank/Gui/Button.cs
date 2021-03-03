using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataStructure.Spritesheet;

namespace Tank.Gui
{
    class Button : UiElement
    {
        private Texture2D leftButtonTexture;
        private Texture2D middleButtonTexture;
        private Texture2D rightButtonTexture;

        private Texture2D leftActiveButtonTexture;
        private Texture2D middleActiveButtonTexture;
        private Texture2D rightActiveButtonTexture;

        private Texture2D currentLeftTexture;
        private Texture2D currentmiddleTexture;
        private Texture2D currentrightTexture;

        public string Text;
        private readonly SpriteFont font;

        private int middlePartCount;
        private Rectangle collider;
        private int completeXSize;

        public bool Clicked { get; private set; }

        public Button(Vector2 position, int width, SpriteSheet textureToShow, SpriteBatch spritebatch, SpriteFont font)
            : base(position, width, textureToShow, spritebatch)
        {
            this.font = font;
            leftButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);
            middleButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);
            rightButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);

            leftActiveButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);
            middleActiveButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);
            rightActiveButtonTexture = new Texture2D(TankGame.PublicGraphicsDevice, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y);

            leftButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonLeft").Array);
            middleButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonMiddle").Array);
            rightButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonRight").Array);

            leftActiveButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonActiveLeft").Array);
            middleActiveButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonActiveMiddle").Array);
            rightActiveButtonTexture.SetData<Color>(textureToShow.GetTextureByName("ButtonActiveRight").Array);

            middlePartCount = (int)Math.Round((float)width / textureToShow.SingleImageSize.X);
            middlePartCount = middlePartCount == 0 ? 1 : middlePartCount;

            currentLeftTexture = leftButtonTexture;
            currentmiddleTexture = middleButtonTexture;
            currentrightTexture = rightButtonTexture;
            completeXSize = textureToShow.SingleImageSize.X * 2;
            completeXSize += textureToShow.SingleImageSize.X * middlePartCount;
            collider = new Rectangle((int)position.X, (int)position.Y, completeXSize, textureToShow.SingleImageSize.Y);
            Text = string.Empty;
        }

        public void Draw(GameTime gameTime)
        {
            spriteBatch.Draw(currentLeftTexture, new Rectangle((int)position.X, (int)position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y), Color.White);

            for (int i = 0; i < middlePartCount; i++)
            {
                int index = i + 1;
                spriteBatch.Draw(currentmiddleTexture, new Rectangle((int)position.X + textureToShow.SingleImageSize.X * index, (int)position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y), Color.White);
            }
            int xPosition = (int)position.X + textureToShow.SingleImageSize.X * 2;
            xPosition += textureToShow.SingleImageSize.X * (middlePartCount - 1);
            spriteBatch.Draw(currentrightTexture, new Rectangle(xPosition, (int)position.Y, textureToShow.SingleImageSize.X, textureToShow.SingleImageSize.Y), Color.White);

            Vector2 textPosition = position;
            textPosition.X += textureToShow.SingleImageSize.X;
            Vector2 textSize = GetTextLenght(Text, font);
            textPosition.Y += textSize.Y;
            textPosition.X += textSize.X / 2;
            textPosition += Vector2.UnitX * textureToShow.SingleImageSize.X * middlePartCount;
            textPosition.X = textPosition.X / 2;
            spriteBatch.DrawString(font, Text, textPosition, Color.Black);
        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            if (collider.Contains(mouseState.Position))
            {
                currentLeftTexture = leftActiveButtonTexture;
                currentmiddleTexture = middleActiveButtonTexture;
                currentrightTexture = rightActiveButtonTexture;

                Clicked = false;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                }
                return;
            }

            currentLeftTexture = leftButtonTexture;
            currentmiddleTexture = middleButtonTexture;
            currentrightTexture = rightButtonTexture;
        }
    }
}
