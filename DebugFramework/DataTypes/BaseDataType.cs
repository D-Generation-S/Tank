using System;

namespace DebugFramework.DataTypes
{
    /// <summary>
    /// Base data for the pipe streaming communication
    /// </summary>
    public class BaseDataType
    {
        /// <summary>
        /// The 
        /// </summary>
        public string AssemblyQualifiedName { get; set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public BaseDataType()
        {
            AssemblyQualifiedName = GetType().AssemblyQualifiedName;
        }

        public Type GetRealType()
        {
            return Type.GetType(AssemblyQualifiedName);
        }
    }
}
