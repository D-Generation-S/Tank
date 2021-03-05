using Microsoft.Xna.Framework.Content;

namespace Tank.Wrapper
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
        [System.Obsolete]
        public ContentManager Content => contentManager;

        /// <summary>
        /// Create a new instance of the wrapper
        /// </summary>
        /// <param name="contentManager"></param>
        public ContentWrapper(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public T Load<T>(string fileName)
        {
            return Content.Load<T>(fileName);
        }
    }
}
