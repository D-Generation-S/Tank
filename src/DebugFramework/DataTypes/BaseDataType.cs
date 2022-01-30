using System;
using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes
{
    /// <summary>
    /// Base data for the pipe streaming communication
    /// </summary>
    public class BaseDataType
    {
        /// <summary>
        /// The type which can be castest to a type object
        /// </summary>
        [JsonPropertyName("aQName")]
        public string AssemblyQualifiedName { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BaseDataType()
        {
            AssemblyQualifiedName = GetType().AssemblyQualifiedName;
        }

        /// <summary>
        /// Get the real type of this data set
        /// </summary>
        /// <returns></returns>
        public Type GetRealType()
        {
            return Type.GetType(AssemblyQualifiedName);
        }
    }
}
