using System.Collections.Generic;

namespace Tank.Code.JSonClasses
{
    public class SplashObject
    {
        public string Name
        {
            get; set;
        }

        public string Image
        {
            get; set;
        }

        public int Duration
        {
            get; set;
        }

        public string Effect
        {
            get; set;
        }
    }

    public class ScreenDefinition
    {
        public int ID
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public string Class
        {
            get; set;
        }

        public string TrackType
        {
            get; set;
        }

        public string BackgroundImage
        {
            get; set;
        }

        public string BackgroundColor
        {
            get; set;
        }

        public List<SplashObject> SplashList
        {
            get; set;
        }
    }
}