using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Wrapper
{
    class ContentWrapper
    {
        private readonly ContentManager contentManager;
        public ContentManager Content;

        public ContentWrapper(ContentManager contentManager)
        {
            contentManager = contentManager;
        }
    }
}
