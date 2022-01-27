namespace DebugFramework.DataTypes
{
    /// <summary>
    /// Request to change the pipe to a new named one
    /// </summary>
    public class ChangeNamedPipeRequest : BaseDataType
    {
        /// <summary>
        /// The base name to use
        /// </summary>
        public string BaseName { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public ChangeNamedPipeRequest()
        {
            BaseName = string.Empty;
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="baseName">The base name of the stream to use</param>
        public ChangeNamedPipeRequest(string baseName)
        {
            BaseName = baseName;
        }
    }
}
