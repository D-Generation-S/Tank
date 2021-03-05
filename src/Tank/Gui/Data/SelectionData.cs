using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Gui.Data
{
    /// <summary>
    /// Data set for the selection gui element
    /// </summary>
    class SelectionData
    {
        /// <summary>
        /// The list with all the possible selections
        /// </summary>
        private List<SelectionDataSet> data;

        /// <summary>
        /// The current int value of the selected data set
        /// </summary>
        private int currentDataSet;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="dataForUse">The data to use</param>
        public SelectionData(List<SelectionDataSet> dataForUse)
        {
            data = dataForUse;
            currentDataSet = 0;
        }

        /// <summary>
        /// Set the current position for the data set
        /// </summary>
        /// <param name="currentDataSet">The current data set</param>
        public void setCurrentDataset(int currentDataSet)
        {
            currentDataSet = currentDataSet < 0 ? 0 : currentDataSet;
            currentDataSet = currentDataSet > data.Count - 1 ? data.Count - 1 : currentDataSet;
            this.currentDataSet = currentDataSet;
        }

        /// <summary>
        /// Go to the next data set
        /// </summary>
        public void NextDataSet()
        {
            currentDataSet++;
            currentDataSet = currentDataSet > data.Count - 1 ? data.Count - 1 : currentDataSet;
        }

        /// <summary>
        /// Go to the previous data set
        /// </summary>
        public void PreviousDataSet()
        {
            currentDataSet--;
            currentDataSet = currentDataSet < 0 ? 0 : currentDataSet;
        }

        /// <summary>
        /// Get the current data set
        /// </summary>
        /// <returns>The current data set</returns>
        public SelectionDataSet GetCurrentDataSet()
        {
            return data[currentDataSet];
        }
    }
}
