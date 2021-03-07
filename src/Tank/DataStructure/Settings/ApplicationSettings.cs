namespace Tank.DataStructure.Settings
{
    class ApplicationSettings
    {
        public float MasterVolume;

        public float MusicVolume
        {
            get => musicVolume * MasterVolume;
            set => musicVolume = value;
        }
        private float musicVolume;

        public float EffectVolume
        {
            get => effectVolume * MasterVolume;
            set => effectVolume = value;
        }
    private float effectVolume;

        public ApplicationSettings()
        {
            MasterVolume = 1f;
            musicVolume = 1f;
            effectVolume = 0.5f;
        }
    }
}
