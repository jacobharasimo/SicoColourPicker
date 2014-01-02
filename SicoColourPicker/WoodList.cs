using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SicoColourPicker
{
    public class WoodList : CustomClass
    {
        private int _id;
        private string _colorCode;
        private string _name;
        
        private string _background;
        private string _backgroundImage;
        private string _font;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value == _id) return;
                _id = value;
                NotifyPropertyChanged("Id");
            }
        }
        public string ColorCode
        {
            get
            {
                return _colorCode;
            }
            set
            {
                if (value == _colorCode) return;
                _colorCode = value;
                NotifyPropertyChanged("ColorCode");
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name) return;
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }
        
        public string Background
        {
            get
            {
                return _background;
            }
            set
            {
                if (value == _background) return;
                _background = value;
                NotifyPropertyChanged("Background");
            }
        }
        public string BackgroundImage
        {
            get
            {
                return _backgroundImage;
            }
            set
            {
                if (value == _backgroundImage) return;
                _backgroundImage = value;
                NotifyPropertyChanged("BackgroundImage");
            }
        }
        public string Font
        {
            get
            {
                return _font;
            }
            set
            {
                if (value == _font) return;
                _font = value;
                NotifyPropertyChanged("Font");
            }
        }

        public static ObservableCollection<ColourList> ToColourList(ObservableCollection<WoodList> collection)
        {
            var result = new ObservableCollection<ColourList>();
            foreach (var item in collection)
            {
                var cl = new ColourList();                
                var colour = new SicoColour
                {
                    Id = item.Id,
                    ColorCode = item.ColorCode,
                    Name = item.Name,
                    Background = item.Background,
                    Font = item.Font
                };

                cl.Colours.Add(colour);                
                result.Add(cl);
            }
            return result;
        }
    }
}
