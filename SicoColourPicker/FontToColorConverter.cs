using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace SicoColourPicker
{
    public class FontToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            var output = new SolidColorBrush(Colors.White);
            if (text == null)
            {
                throw new Exception("Value is null");
            }
            if (text.Equals("dark-font"))
            {
                output = new SolidColorBrush(ToColorFromHex("#494949"));
            }

            return output;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var output = "dark-font";
            var sample = value as SolidColorBrush;
            if (sample == null)
            {
                throw new Exception("Value is null");
            }
            if (sample.Color == Colors.White) {
                output = "light-font";
            }
            return output;
        }


        public static Color ToColorFromHex(string hex)
        {
            if (string.IsNullOrEmpty(hex))
            {
                throw new ArgumentNullException("hex");
            }

            // remove any "#" characters
            while (hex.StartsWith("#"))
            {
                hex = hex.Substring(1);
            }

            int num;
            // get the number out of the string 
            if (!Int32.TryParse(hex, NumberStyles.HexNumber, null, out num))
            {
                throw new ArgumentException("Color not in a recognized Hex format.");
            }
            var pieces = new int[4];
            if (hex.Length > 7)
            {
                pieces[0] = ((num >> 24) & 0x000000ff);
                pieces[1] = ((num >> 16) & 0x000000ff);
                pieces[2] = ((num >> 8) & 0x000000ff);
                pieces[3] = (num & 0x000000ff);
            }
            else if (hex.Length > 5)
            {
                pieces[0] = 255;
                pieces[1] = ((num >> 16) & 0x000000ff);
                pieces[2] = ((num >> 8) & 0x000000ff);
                pieces[3] = (num & 0x000000ff);
            }
            else if (hex.Length == 3)
            {
                pieces[0] = 255;
                pieces[1] = ((num >> 8) & 0x0000000f);
                pieces[1] += pieces[1] * 16;
                pieces[2] = ((num >> 4) & 0x000000f);
                pieces[2] += pieces[2] * 16;
                pieces[3] = (num & 0x000000f);
                pieces[3] += pieces[3] * 16;
            }
            return Color.FromArgb((byte)pieces[0], (byte)pieces[1], (byte)pieces[2], (byte)pieces[3]);
        }

    }
}
