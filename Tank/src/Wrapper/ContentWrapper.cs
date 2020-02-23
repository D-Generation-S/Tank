﻿using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Wrapper
{
    /// <summary>
    /// This class will warp the content manager
    /// </summary>
    class ContentWrapper
    {
        /// <summary>
        /// The Monogame content manager to use
        /// </summary>
        private readonly ContentManager contentManager;

        /// <summary>
        /// Read access to the Monogame content manager
        /// </summary>
        public ContentManager Content => contentManager;

        /// <summary>
        /// Create a new instance of the wrapper
        /// </summary>
        /// <param name="contentManager"></param>
        public ContentWrapper(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }
    }
}
