using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Tank.Code.SubClasses;

namespace Tank.Code
{
    public static class Settings
    {
        public enum ClimaticZones
        {
            Snow,
            Grass,
            Dirt,
            Sand,
            Rock
        }
        public static bool Debug = false;
        public static bool Drawing = false;
        public static bool ExplodingClicks = false;
        public static bool ShowRayCast = false;
        public static bool ShowPerlinNoise = false;
        public static bool PlayerMovingAllowed = false;
        public static bool ShowSplashScreen = true;

        public static bool PlayerCurrentDisabled = false;

        public static int HelpTextShowTime = 300;

        public static Color DebugTextColor = Color.Transparent;

        public static SpriteFont GlobalFont;

        private static List<ColorTextureAssignment> colorTextureAssignments;

        public static List<ColorTextureAssignment> ColorTextureAssignments
        {
            get
            {
                if (colorTextureAssignments == null)
                    colorTextureAssignments = new List<ColorTextureAssignment>();
                return colorTextureAssignments;
            }
            set
            {
                colorTextureAssignments = value;
            }
        }

        public static Rectangle MaxWindowSize
        {
            get; set;
        }

        public static Texture2D DynamicPixelTexture
        {
            get; set;
        }

        public static string ScreenshotFolder
        {
            get; set;
        }

        public static Color[] BackgroundPixels
        {
            get; set;
        }

        public static KeyboardState CurrentKeyboardState
        {
            get; set;
        }
        public static KeyboardState PreviousKeyboardState
        {
            get; set;
        }

        public static Keys[] GoLeftKey
        {
            get; set;
        }

        public static Keys[] GoRightKey
        {
            get; set;
        }

        public static Keys[] ShootKey
        {
            get; set;
        }
        public static Keys[] IncreasePower
        {
            get;
            set;
        }
        public static Keys[] DecreasePower
        {
            get; set;
        }

        public static Buttons Left;
        public static Buttons Right;
        public static Buttons Shoot;
        public static Buttons IncPower;
        public static Buttons DecPower;

        public static bool ExitGame;

        public static Texture2D GameCursor
        {
            get;
            set;
        }
        public static Texture2D Meter
        {
            get;
            set;
        }
        public static Texture2D Bar
        {
            get;
            set;
        }
        public static Texture2D ProgressBackground
        {
            get;
            set;
        }
        public static Texture2D ProgressForeground
        {
            get;
            set;
        }
        public static Texture2D PlayerAngleForeground
        {
            get; internal set;
        }
        public static Texture2D PlayerAngleBackground
        {
            get; internal set;
        }
        public static Texture2D BasicMunitionSprite
        {
            get;
            internal set;
        }
        public static GameTime GameTime
        {
            get;
            internal set;
        }

        public static SoundEffect ClickSound;
        public static SoundEffect ShootSound;
        public static SoundEffect ExplosionSound;
        public static SoundEffect HitSound;
        public static SoundEffect DeathSound;

        public static float SoundVolume = 1;

        public static string SaveFilePath
        {
            set
            {
                SaveFile = value;
            }

        }

        public static Texture2D MainMenuBackground
        {
            get;
            internal set;
        }
        public static SoundEffect BackgroundMusic
        {
            get;
            internal set;
        }

        public static Color GetColorByClimaticZone(ClimaticZones CurrentZone)
        {
            switch (CurrentZone)
            {
                case Settings.ClimaticZones.Snow:
                    return Color.White;
                    break;
                case Settings.ClimaticZones.Dirt:
                    return Color.SaddleBrown;
                    break;
                case Settings.ClimaticZones.Grass:
                    return Color.Green;
                    break;
                case Settings.ClimaticZones.Rock:
                    return Color.DarkGray;
                    break;
                case Settings.ClimaticZones.Sand:
                    return Color.Bisque;
                    break;


                default:
                    break;
            }
            return Color.Violet;
        }

        private static string SaveFile;
        public static string CustomMapFolder
        {
            get
            {
                return $"{new FileInfo(SaveFile).DirectoryName}\\CustomMaps\\";
            }

        }

        public static float MusicVolume
        {
            get;
            internal set;
        }

        public static void Save()
        {
            if (string.IsNullOrEmpty(SaveFile))
                return;
            if (!Directory.Exists(new FileInfo(SaveFile).DirectoryName))
                Directory.CreateDirectory(new FileInfo(SaveFile).DirectoryName);

            XmlSerializer SaveFileSerializer = new XmlSerializer(typeof(SettingsSerializerHelp), "Settings");
            SettingsSerializerHelp CurrentSettings = new SettingsSerializerHelp()
            {
                GoLeftKeys = GoLeftKey,
                GoRightKeys = GoRightKey,
                ShootKeys = ShootKey,
                DecreasePowerKeys = DecreasePower,
                IncreasePowerKeys = IncreasePower,
                ShowSplashScreen = ShowSplashScreen,
                MusicVolume = MusicVolume,
                SoundVolume = SoundVolume
            };
            GoLeftKey = CurrentSettings.GoLeftKeys;
            using (StreamWriter writer = new StreamWriter(SaveFile))
            {
                SaveFileSerializer.Serialize(writer, CurrentSettings);
            }
        }

        public static void Load()
        {
            if (string.IsNullOrEmpty(SaveFile))
                return;

            if (!File.Exists(SaveFile))
                Save();
            XmlSerializer LoadFileSerializer = new XmlSerializer(typeof(SettingsSerializerHelp), "Settings");
            using (StreamReader reader = new StreamReader(SaveFile))
            {
                object ReturnObject = LoadFileSerializer.Deserialize(reader);
                if (ReturnObject.GetType() == typeof(SettingsSerializerHelp))
                {
                    SettingsSerializerHelp LoadetSettings = (SettingsSerializerHelp)ReturnObject;
                    GoLeftKey = LoadetSettings.GoLeftKeys;
                    GoRightKey = LoadetSettings.GoRightKeys;
                    ShowSplashScreen = LoadetSettings.ShowSplashScreen;
                    MusicVolume = LoadetSettings.MusicVolume;
                    SoundVolume = LoadetSettings.SoundVolume;
                }
            }

        }
    }

    [Serializable]
    public class SettingsSerializerHelp
    {
        public string[] SaveGoLeftKeys;
        [XmlIgnore]
        public Keys[] GoLeftKeys
        {
            set
            {
                SaveGoLeftKeys = GetStringListFormKeyList(value);
            }
            get
            {
                return GetKeyListFromStringList(SaveGoLeftKeys);
            }
        }

        public string[] SaveGoRightKeys;
        [XmlIgnore]
        public Keys[] GoRightKeys
        {
            set
            {
                SaveGoRightKeys = GetStringListFormKeyList(value);
            }
            get
            {
                return GetKeyListFromStringList(SaveGoRightKeys);
            }
        }

        public string[] SaveShootKeys;
        [XmlIgnore]
        public Keys[] ShootKeys
        {
            set
            {
                SaveShootKeys = GetStringListFormKeyList(value);
            }
            get
            {
                return GetKeyListFromStringList(SaveShootKeys);
            }
        }

        public string[] SaveIncreasePowerKeys;
        [XmlIgnore]
        public Keys[] IncreasePowerKeys
        {
            set
            {
                SaveIncreasePowerKeys = GetStringListFormKeyList(value);
            }
            get
            {
                return GetKeyListFromStringList(SaveIncreasePowerKeys);
            }
        }

        public string[] SaveDecreasePowerKeys;
        [XmlIgnore]
        public Keys[] DecreasePowerKeys
        {
            set
            {
                SaveDecreasePowerKeys = GetStringListFormKeyList(value);
            }
            get
            {
                return GetKeyListFromStringList(SaveDecreasePowerKeys);
            }
        }

        public SettingsSerializerHelp()
        {

        }

        private Keys[] GetKeyListFromStringList(string[] CurrentList)
        {
            List<Keys> ReturnKeys = new List<Keys>();
            foreach (string CurrentKey in CurrentList)
            {
                Keys NewKey = Keys.None;
                if (Enum.TryParse(CurrentKey, out NewKey))
                    ReturnKeys.Add(NewKey);
            }
            return ReturnKeys.ToArray();
        }
        private string[] GetStringListFormKeyList(Keys[] CurrentList)
        {
            List<string> ReturnStrings = new List<string>();
            foreach (Keys CurrentKey in CurrentList)
            {
                ReturnStrings.Add(CurrentKey.ToString());
            }
            return ReturnStrings.ToArray();
        }

        public bool ShowSplashScreen;
        public float SoundVolume;
        public float MusicVolume;
    }
}
