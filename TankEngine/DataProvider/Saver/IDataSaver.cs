namespace Tank.DataManagement.Saver
{
    /// <summary>
    /// Interface to save data
    /// </summary>
    /// <typeparam name="T">The type of the data to save</typeparam>
    public interface IDataSaver<T>
    {
        /// <summary>
        /// Save the data set to the file name
        /// </summary>
        /// <param name="dataToSave">The data to save</param>
        /// <param name="fileName">The file name to store the data in</param>
        /// <returns></returns>
        bool SaveData(T dataToSave, string fileName);
    }
}
