using System.Collections.Generic;


namespace SicoColourPicker
{
    public class SicoColorExtended : SicoColour
    {
        private string _positionInDisplay;
        private string _prevColorCode;
        private string _nextColorCode;
        private string _recommendedPrimer;
        private string _primerSymbol;
        private string _primerNote;
        private string _isExtraNote;
        private bool _requiresAdditionalCoat;
        private string _requiresAddtionalCoatNote;
        private bool _isAvailableOnlyInGallon;
        private string _isAvailableOnlyInGallonNote;
        private List<SicoColour> _complementaryColours = new List<SicoColour>();
        private List<SicoColour> _triadColours = new List<SicoColour>();
        private List<SicoColour> _shadesColours = new List<SicoColour>();
        private List<ColourCategoryPage> _categoryPages = new List<ColourCategoryPage>();
        public string PositionInDisplay 
        {
            get { 
                return _positionInDisplay; 
            }
            set {
                _positionInDisplay = value;
                NotifyPropertyChanged("PositionInDisplay");
            } 
        } // e.g. "A37", position of chip in store display
        public string PrevColorCode
        {
            get
            {
                return _prevColorCode;
            }
            set
            {
                _prevColorCode = value;
                NotifyPropertyChanged("PrevColorCode");
            }
        } // link to prev Chip in palette
        public string NextColorCode
        {
            get
            {
                return _nextColorCode;
            }
            set
            {
                _nextColorCode = value;
                NotifyPropertyChanged("NextColorCode");
            }
        } // link to next Chip in palette
        public string RecommendedPrimer
        {
            get
            {
                return _recommendedPrimer;
            }
            set
            {
                _recommendedPrimer = value;
                NotifyPropertyChanged("RecommendedPrimer");
            }
        } // AE, AT or empty
        public string PrimerSymbol
        {
            get
            {
                return _primerSymbol;
            }
            set
            {
                _primerSymbol = value;
                NotifyPropertyChanged("PrimerSymbol");
            }
        } // solid triangle, outlined triangle or empty
        public string PrimerNote
        {
            get
            {
                return _primerNote;
            }
            set
            {
                _primerNote = value;
                NotifyPropertyChanged("PrimerNote");
            }
        } // word version from UI Culture string, e.g. "This colour requires a base coat of Goprime all-in-one Alkyd Emulsion"
        public string IsExtraNote
        {
            get
            {
                return _isExtraNote;
            }
            set
            {
                _isExtraNote = value;
                NotifyPropertyChanged("IsExtraNote");
            }
        } // e.g. "Attention: this colour is available in stores, but not as a Colour Chip"
        public bool RequiresAdditionalCoat
        {
            get
            {
                return _requiresAdditionalCoat;
            }
            set
            {
                _requiresAdditionalCoat = value;
                NotifyPropertyChanged("RequiresAdditionalCoat");
            }
        }
        public string RequiresAddtionalCoatNote
        {
            get
            {
                return _requiresAddtionalCoatNote;
            }
            set
            {
                _requiresAddtionalCoatNote = value;
                NotifyPropertyChanged("RequiresAddtionalCoatNote");
            }
        } // e.g. "This colour may require additional coat(s)"
        public bool IsAvailableOnlyInGallon
        {
            get
            {
                return _isAvailableOnlyInGallon;
            }
            set
            {
                _isAvailableOnlyInGallon = value;
                NotifyPropertyChanged("IsAvailableOnlyInGallon");
            }
        }
        public string IsAvailableOnlyInGallonNote
        {
            get
            {
                return _isAvailableOnlyInGallonNote;
            }
            set
            {
                _isAvailableOnlyInGallonNote = value;
                NotifyPropertyChanged("IsAvailableOnlyInGallonNote");
            }
        } // e.g. "This colour is available in 3.78L size only"
        public List<SicoColour> ComplementaryColours
        {
            get
            {
                return _complementaryColours;
            }
            set
            {
                _complementaryColours = value;
                NotifyPropertyChanged("ComplementaryColours");
            }
        } // aka Duo colour; expect one ColorCode value but use List for uniformity
        public List<SicoColour> TriadColours
        {
            get
            {
                return _triadColours;
            }
            set
            {
                _triadColours = value;
                NotifyPropertyChanged("TriadColours");
            }
        } // expect 2 ColorCode values
        public List<SicoColour> ShadesColours
        {
            get
            {
                return _shadesColours;
            }
            set
            {
                _shadesColours = value;
                NotifyPropertyChanged("ShadesColours");
            }
        } // expect 2 ColorCode values
        public List<ColourCategoryPage> CategoryPages
        {
            get
            {
                return _categoryPages;
            }
            set
            {
                _categoryPages = value;
                NotifyPropertyChanged("CategoryPages");
            }
        } // e.g. page info for related Colour by Hue, Inspiration by Room
		//public SicoColorExtended() : base(){}		
    }
}
