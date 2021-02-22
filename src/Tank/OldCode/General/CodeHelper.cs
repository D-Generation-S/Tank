using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Tank.Code.General
{
    public static class CodeHelper
    {
        public static string ToSafeString(this object o)
        {
            return o == null ? string.Empty : o.ToString();
        }

        public static int ToInt(this object o)
        {
            int iReturn = 0;

            int.TryParse(o.ToSafeString(), out iReturn);

            return iReturn;
        }

        public static bool ToBool(this object o)
        {
            bool bReturn = false;

            bool.TryParse(o.ToSafeString(), out bReturn);

            return bReturn;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do
                {

                    provider.GetBytes(box);
                } while (!(box[0] < n * (byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static string LoadJson(string filename)
        {
            string startupPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            string strReturn = string.Empty;
            string fileName = $@"{startupPath}\Content\Data\{filename}.json";
            if (File.Exists(fileName))
            {
                using (StreamReader reader = new StreamReader(File.OpenRead(fileName)))
                {
                    strReturn = reader.ReadToEnd();
                }
            }

            return strReturn;
        }

        /// <summary>
        /// Converts a Hexadecimal string into a Microsoft.Xna.Framework.Color
        /// </summary>
        /// <param name="strHex">Either 7 or 3 characters long #000 or #000000</param>
        /// <returns>The color</returns>
        public static Color ConvertFromHex(string strHex)
        {
            Regex isHex = new Regex("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$");
            Color cReturn = Color.Transparent;
            if (isHex.IsMatch(strHex))
            {
                string rValue = string.Empty;
                string gValue = string.Empty;
                string bValue = string.Empty;
                if (strHex.Length == 4)
                {
                    rValue = strHex[1].ToString() + strHex[1].ToString();
                    gValue = strHex[2].ToString() + strHex[2].ToString();
                    bValue = strHex[3].ToString() + strHex[3].ToString();
                }
                else if (strHex.Length == 7)
                {
                    rValue = strHex.Substring(1, 2);
                    gValue = strHex.Substring(3, 2);
                    bValue = strHex.Substring(5, 2);
                }

                float R = (float)int.Parse(rValue, System.Globalization.NumberStyles.HexNumber) / 255;
                float G = (float)int.Parse(gValue, System.Globalization.NumberStyles.HexNumber) / 255;
                float B = (float)int.Parse(bValue, System.Globalization.NumberStyles.HexNumber) / 255;
                cReturn = new Color(R, G, B);
            }
            return cReturn;
        }

        private static bool IntersectsCircle(this Rectangle rect, int Cx, int Cy, float Radius)
        {
            //float TestValue = (float)Math.Sqrt(Math.Pow((Px - Cx), 2) + Math.Pow((Py - Cy), 2));
            //if (TestValue < Radius)
            //    return true;
            return false;
        }
    }
}
