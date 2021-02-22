﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tank.Code
{
    public class Postprocessor
    {
        private Effect _postEffect;

        public Postprocessor(Effect EffectToUse)
        {
            _postEffect = EffectToUse;
        }

        public void Draw(SpriteBatch sb, RenderTarget2D Target)
        {
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, _postEffect, null);
            sb.Draw(Target, new Vector2(0, 0), Color.White);
            sb.End();
        }
    }
}
