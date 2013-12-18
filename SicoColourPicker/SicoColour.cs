using System.Collections.ObjectModel;

namespace SicoColourPicker
{
    public class SicoColour : CustomClass
    {
        private string _background;
        private string _colorCode;
        private string _displayCode;
        private string _font;
        private ObservableCollection<string> _hue = new ObservableCollection<string>();
        private int _id;
        private bool _isExtra;
        private string _name;
        private double _swatchHeight;
        public double SwatchHeight
        {
            get
            {
                return _swatchHeight;
            }
            set
            {
                if (value == _swatchHeight) return;
                _swatchHeight = value;
                NotifyPropertyChanged("SwatchHeight");
            }
        }

        private double _swatchWidth;
        public double SwatchWidth
        {
            get
            {
                return _swatchWidth;
            }
            set
            {
                if (value == _swatchWidth) return;
                _swatchWidth = value;
                NotifyPropertyChanged("SwatchWidth");
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
        public string DisplayCode
        {
            get
            {
                return _displayCode;
            }
            set
            {
                if (value == _displayCode) return;
                _displayCode = value;
                NotifyPropertyChanged("DisplayCode");
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
        public ObservableCollection<string> Hue
        {
            get
            {
                return _hue;
            }
            set
            {
                if (value == _hue) return;                
                foreach (var item in value) {
                    _hue.Add(item);
                }
                NotifyPropertyChanged("Hue");
            }
        }        
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
        public bool IsExtra
        {
            get
            {
                return _isExtra;
            }
            set
            {
                if (value == _isExtra) return;
                _isExtra = value;
                NotifyPropertyChanged("IsExtra");
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
        
    }
}
