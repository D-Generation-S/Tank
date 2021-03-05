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

        /// <summary>
        /// Load the content
        /// </summary>
        /// <typeparam name="T">The type of content to load</typeparam>
        /// <param name="fileName">The name of the content file to load</param>
        /// <returns>The loaded content or null</returns>
        public T Load<T>(string fileName)
        {
            return contentManager.Load<T>(fileName);
        }
    }
}
